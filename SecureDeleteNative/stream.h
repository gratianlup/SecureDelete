// Copyright (c) 2005 Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
// * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials provided
// with the distribution.
//
// * The name "SecureDelete" must not be used to endorse or promote
// products derived from this software without prior written permission.
//
// * Products derived from this software may not be called "SecureDelete" nor
// may "SecureDelete" appear in their names without prior written
// permission of the author.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#ifndef STREAM_H
#define STREAM_H

#include "wcontext.h"


// ****************************************************************************
// *                                                                          *
// * roundToCluster - computes the nearest multiple of the cluster greater    *
// * than dataSize ( ex. 15032 => 16384 for cluster = 4096                    *
// *                                                                          *
// ****************************************************************************
inline __int64 WCONTEXT::roundToCluster(__int64 dataSize) {
	int clusterSize = volInfo.getVolumeClusterSize(volInfo.cv);

	if(clusterSize == 0) {
		clusterSize = 512;
	}

	if( dataSize % clusterSize != 0 ) {
		dataSize += clusterSize;
		dataSize -= dataSize % clusterSize;
	}
	
	return dataSize;
}


// ****************************************************************************
// *                                                                          *
// * computeStreamCrc - reads a stream from disk and computes the CRC         *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::computeStreamCrc(HANDLE handle,__int64 streamSize) {
	unsigned long readLength = BUFFER_LENGTH;
	unsigned long bytesRead;
	
	if( SetFilePointer(handle,woPosition.LowPart,
                       (long *)&woPosition.HighPart,FILE_BEGIN) == 
                       INVALID_SET_FILE_POINTER && GetLastError() != NO_ERROR ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set stream pointer to position %d",
                                         woPosition.QuadPart);
			return ERRORCODE_WRITE;
	}
	
	crc2 = 0;
	
	while( streamSize > 0 ) {
		if( stopped ) {
			return ERRORCODE_STOPPED;
		}
		
		if( streamSize < readLength) {
			readLength = streamSize;
		}
		
		/*
		** read the data
		*/
		if( !ReadFile(handle,buffer,readLength,&bytesRead,NULL) || 
            bytesRead != readLength ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't read file for CRC");
			return ERRORCODE_CRC;
		}
		
		/*
		** update status
		*/
		updateWobjectWiped(readLength);
		updateWipedBytes(readLength);
		
		/*
		** compute CRC
		*/
		computeCrc(&crc2,buffer,readLength);
		streamSize -= readLength;
	}
	
	return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * wipeStream - THE MOST IMPORTANT FUNCTION - wipes a data stream           *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeStream(HANDLE handle) {
	unsigned long bufferLength;
	__int64       streamSize;
    __int64       streamRealSize; // used for CRC checking
	unsigned long bytesWritten;
	int           bufferFilled;
	int           wipeCheckResult;
	
	if( handle == INVALID_HANDLE_VALUE || woSize.QuadPart <= 0 || buffer == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameters for wipeStream");
		return ERRORCODE_UNKNOWN;
	}
	
	/*
	** set wipe method for current object
	*/
	if( !setWOMethod(wobject->wmethod) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set wipe method %d",wobject->wmethod);
		return ERRORCODE_WMETHOD;
	}	
	
	/*
	** shuffle step order ?
	*/
	if( wowmethod->shuffle ) {
		if( !wowmethod->performShuffle() ) {
			return ERRORCODE_WMETHOD;
		}
	}
	
	/*
	** wipe the data stream
	*/
	for(int i = 0;i < wowmethod->nSteps;i++) {
		if( stopped ) {
			return ERRORCODE_STOPPED;
		}
		
		/*
		** update step information
		*/
		updateStepInfo(i + 1,wowmethod->nSteps);
		
		/*
		** reset
		*/
		bufferLength = BUFFER_LENGTH;
		bufferFilled = 0;
		streamSize = streamRealSize = woSize.QuadPart;
		crc1 = crc2 = 0;
		
		if( SetFilePointer(handle,woPosition.LowPart,
                           (long *)&woPosition.HighPart,FILE_BEGIN) == 
                           INVALID_SET_FILE_POINTER && GetLastError() != NO_ERROR ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set stream pointer to position %d",
                                        woPosition.QuadPart);
			return ERRORCODE_WRITE;
		}
		
		/*
		** write in the data stream
		*/
		while( streamSize > 0 ) {
			if( stopped ) {
				return ERRORCODE_STOPPED;
			}
			
			/*
			** compute buffer length
			*/
			if( streamSize < BUFFER_LENGTH ) {
				if( fastWrite ) {
					bufferLength = roundToCluster(streamSize);
					updateClusterTips(bufferLength - streamSize);
				}
				else {
					bufferLength = streamSize;
				}
				
				streamRealSize += bufferLength - streamSize;
				streamSize = 0;
			}
			else {
				streamSize -= BUFFER_LENGTH;
			}
		
			/*
			** fill buffer with current step data
			*/
			if( !bufferFilled || wowmethod->steps[i].patternType == WSTEP_RANDOM ) {
				if( !fillStepData(bufferLength,i) ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't fill pass data for step %d",i);
					return ERRORCODE_WMETHOD;
				}
				
				bufferFilled = 1;
			}
			
			/*
			** now write...
			*/
			if( !WriteFile(handle,buffer,bufferLength,&bytesWritten,NULL) || 
                bytesWritten != bufferLength ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Write error ( bufferLength %d byestWrited %d ). Error %d",
                                            bufferLength,bytesWritten,GetLastError());
				return ERRORCODE_WRITE;
			}
			
			/*
			** update status
			*/
			updateWobjectWiped(bufferLength);
			updateWipedBytes(bufferLength);
			
			/*
			** perform CRC check ?
			*/
			if( wowmethod->checkWipe ) {
				computeCrc(&crc1,buffer,bufferLength);
			}
		}
		
		/*
		** perform CRC check ?
		*/
		if( wowmethod->checkWipe ) {
			wipeCheckResult = computeStreamCrc(handle,streamRealSize);
			
			if( wipeCheckResult != ERRORCODE_SUCCESS ) {
				if( wipeCheckResult == ERRORCODE_STOPPED ) {
					return ERRORCODE_STOPPED;
				}
				else {
					return ERRORCODE_CRC;
				}
			}
			
			if( crc1 != crc2 ) {
				dbgPrint(__WFILE__,__LINE__,L"WARNING: CRCs not equal");
				return ERRORCODE_CRC;
			}
		}
	}
	
	return ERRORCODE_SUCCESS;
}

#endif
