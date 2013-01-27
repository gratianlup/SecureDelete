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

#ifndef STACK_H
#define STACK_H

#include "debug.h"


template <class T> class Stack {
public:    
	Stack() {
		nextElement = NULL;
		ct = 0;
	}

	~Stack() {
		if( nextElement != NULL ) {
			delete nextElement;
			nextElement = NULL;
		}
	}

	unsigned int insert(T &);
	unsigned int extract(T *);
	unsigned int isEmpty() {
		return (nextElement == NULL);
	}
	
	void clear() {
    	delete nextElement;
	}
	ULONGLONG ct;

protected: 
	T         value;
	Stack<T> *nextElement;
};


// ****************************************************************************
// *                                                                          *
// * insert - pushes an object in the stack                                   *
// *                                                                          *
// ****************************************************************************
template <class T> unsigned int Stack<T>::insert(T &newValue) {
    Stack<T> *newElement = new Stack<T>();
    newElement->value = newValue;

    newElement->nextElement = nextElement;
    nextElement = newElement;
    ct++;
    return 1;
}


// ****************************************************************************
// *                                                                          *
// * extract - pops an object from the stack                                  *
// *                                                                          *
// ****************************************************************************
template <class T> unsigned int Stack<T>::extract(T *value) {
    if ( value != NULL && nextElement != NULL ) {
        Stack<T> *next = nextElement->nextElement;

        *value = nextElement->value;

        nextElement->nextElement = 0;
        delete nextElement;

        nextElement = next;
        ct--;
        return 1;
    }
    
    return 0;
}

#endif
