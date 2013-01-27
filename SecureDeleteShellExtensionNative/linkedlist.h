// Copyright (c) Gratian Lup. All rights reserved.
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

#ifndef LINKED_LIST_H
#define LINKED_LIST_H

/*
** maxim number of cached objects
*/
#define MAXIMUM_CACHE_SIZE 16

template <class T> class NODE {
public:
	T        data;
	NODE<T> *next;
	NODE<T> *previous;

	NODE() {
		next     = NULL;
		previous = NULL;
	}
};

template <class T> class LLIST {
public:
	NODE<T> *head;
	NODE<T> *last;

	NODE<T> *cache[MAXIMUM_CACHE_SIZE];
	unsigned long  x;
	NODE<T>       *xn; 

	long number;

	LLIST() {
		head = NULL;
		last = NULL;
		number = 0;
		for(unsigned int i = 0;i < MAXIMUM_CACHE_SIZE;i++) {
			cache[i] = NULL;
		}
		x  = 0;
		xn = NULL;
	}
	~LLIST() {
		clear();
	}

	void insert(T *value);
	void clear();
	T &  operator[](unsigned long i);
};


// ****************************************************************************
// *                                                                          *
// * clear - deletes all nodes from the list                                  *
// *                                                                          *
// ****************************************************************************
template <class T> void LLIST<T>::clear() {
	NODE<T> *previous;
	NODE<T> *aux;

	if( last != NULL ) {
		try {
			previous = last;

			while( previous != NULL ) {
				aux = previous->previous;
				delete previous;

				previous = aux;
			}

			head = NULL;
			last = NULL;
			for(unsigned int i = 0;i < MAXIMUM_CACHE_SIZE;i++) {
				cache[i] = NULL;
			}

			number = 0;
			x  = 0;
			xn = NULL;
		} catch(...) {
		}
	}	
}


// ****************************************************************************
// *                                                                          *
// * insert - add a node to the end of the list                               *
// *                                                                          *
// ****************************************************************************
template <class T> inline void LLIST<T>::insert(T *value) {
	try {
		NODE<T> *newNode = new NODE<T>;
		newNode->data = *value;
		newNode->next = NULL;

		if( last == NULL ) {
			newNode->previous = NULL;
			last = newNode;
		}			
		else {
			last->next = newNode;
			newNode->previous = last;
			last = newNode;
		}
		if( head == NULL ) head = newNode;

		/*
		** incerc sa introduc in cache
		*/
		if( number < MAXIMUM_CACHE_SIZE ) cache[number] = newNode;
		number++;
	} catch(...) {
	}
}


// ****************************************************************************
// *                                                                          *
// * operator[] - get the value of a node based on index                      *
// *                                                                          *
// ****************************************************************************
template <class T> inline T & LLIST<T>::operator[](unsigned long i) {
	if( i < number ) {
		if ( i < MAXIMUM_CACHE_SIZE ) {
			x = i;
			xn = cache[i];
			return cache[i]->data;
		}
		else if( i == x + 1 && xn != NULL ) {
			/*
			** optimization for access in the order 1 2 3
			*/
			x++;
			xn = xn->next;
			return xn->data;
		}
		else if( i == x - 1 && xn != NULL ) {
			/*
			** optimization for access in the order 3 2 1
			*/
			x--;
			xn = xn->previous;
			return xn->data;
		}
		else {
			/*
			** start searching from the end
			*/
			NODE<T> *temp;
			temp = cache[MAXIMUM_CACHE_SIZE - 1];

			for(int j = MAXIMUM_CACHE_SIZE - 1;j < i && temp->next != NULL;j++) {
				temp = temp->next;
			}

			x = i;
			xn = temp;
			return temp->data;
		}
	}	
}


/*
** linked list optimized for small memory consumption
*/
template <class T> struct SMALL_LLIST {
	T               data;
	SMALL_LLIST<T> *next;

	SMALL_LLIST() {
		next = NULL;
	}

	~SMALL_LLIST() {
		if( next != NULL ) {
			delete next;
		}
	}
};

#endif
