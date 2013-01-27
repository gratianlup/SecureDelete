// Copyright (c) Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
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

/* ------------------------------------------------------------------------------
rand.c: By Bob Jenkins.  My random number generator, ISAAC.  Public Domain.
MODIFIED:
  960327: Creation (addition of randinit, really)
  970719: use context, not global variables, for internal state
  980324: added main (ifdef'ed out), also rearranged randinit()
  010626: Note that this is public domain
------------------------------------------------------------------------------
*/

#ifndef ISAAC_32_H
#define ISAAC_32_H

#include <windows.h>

#define bis(target,mask)  ((target) |=  (mask))
#define bic(target,mask)  ((target) &= ~(mask))
#define bit(target,mask)  ((target) &   (mask))

#define RANDSIZL (8)  // 8 pentru criptografie
#define RANDSIZ  (1<<RANDSIZL)

// contextul RNG
typedef struct _randctx_isaac32 {
  unsigned long  randcnt;
  unsigned long *randrsl;
  unsigned long *randmem;
  unsigned long  randa;
  unsigned long  randb;
  unsigned long  randc;
} randctx_isaac32;

/*#define isaacRand32(r) \
   (!(r)->randcnt-- ? \
     (isaac32(r), (r)->randcnt=RANDSIZ-1, (r)->randrsl[(r)->randcnt]) : \
     (r)->randrsl[(r)->randcnt])    */

#define ind32(mm,x)  (*(unsigned long *)((unsigned char *)(mm) + ((x) & ((RANDSIZ-1)<<2))))
#define rngstep32(mix,a,b,mm,m,m2,r,x) \
{ \
  x = *m;  \
  a = (a^(mix)) + *(m2++); \
  *(m++) = y = ind32(mm,x) + a + b; \
  *(r++) = b = ind32(mm,y>>RANDSIZL) + x; \
}

void isaac32(randctx_isaac32 *ctx) {
   unsigned long a,b,x,y,*m,*mm,*m2,*r,*mend;
   mm = ctx->randmem;
   r = ctx->randrsl;
   a = ctx->randa;
   b = ctx->randb + (++ctx->randc);

   for (m = mm, mend = m2 = m+128;m<mend;) {
      rngstep32( a<<13, a, b, mm, m, m2, r, x);
      rngstep32( a>>6 , a, b, mm, m, m2, r, x);
      rngstep32( a<<2 , a, b, mm, m, m2, r, x);
      rngstep32( a>>16, a, b, mm, m, m2, r, x);
   }

   for (m2 = mm;m2 < mend;) {
      rngstep32( a<<13, a, b, mm, m, m2, r, x);
      rngstep32( a>>6 , a, b, mm, m, m2, r, x);
      rngstep32( a<<2 , a, b, mm, m, m2, r, x);
      rngstep32( a>>16, a, b, mm, m, m2, r, x);
   }
   
   ctx->randb = b; ctx->randa = a;
}

#define mix(a,b,c,d,e,f,g,h) \
{ \
   a^=b<<11; d+=a; b+=c; \
   b^=c>>2;  e+=b; c+=d; \
   c^=d<<8;  f+=c; d+=e; \
   d^=e>>16; g+=d; e+=f; \
   e^=f<<10; h+=e; f+=g; \
   f^=g>>4;  a+=f; g+=h; \
   g^=h<<8;  b+=g; h+=a; \
   h^=a>>9;  c+=h; a+=b; \
}

// daca flag = 1 folosesc continutul din randrsl[] pentru a initializa mm[]
void randinit32(randctx_isaac32 *ctx,unsigned int flag) {
    unsigned int i;
    unsigned long a,b,c,d,e,f,g,h;
    unsigned long *m,*r;
    ctx->randa = ctx->randb = ctx->randc = 0;
    m = ctx->randmem;
    r = ctx->randrsl;
    a = b = c = d = e = f = g = h = 0x9e3779b9;

    mix(a,b,c,d,e,f,g,h);
    mix(a,b,c,d,e,f,g,h);
    mix(a,b,c,d,e,f,g,h);
    mix(a,b,c,d,e,f,g,h);

    if (flag) {
        for (i = 0; i < RANDSIZ; i += 8) {
            a+=r[i  ]; b+=r[i+1]; c+=r[i+2]; d+=r[i+3];
            e+=r[i+4]; f+=r[i+5]; g+=r[i+6]; h+=r[i+7];
            mix(a,b,c,d,e,f,g,h);
            m[i  ]=a; m[i+1]=b; m[i+2]=c; m[i+3]=d;
            m[i+4]=e; m[i+5]=f; m[i+6]=g; m[i+7]=h;
        }
        
    	for (i = 0; i < RANDSIZ; i += 8) {
           a += m[i  ]; b += m[i+1]; c += m[i+2]; d += m[i+3];
           e += m[i+4]; f += m[i+5]; g += m[i+6]; h += m[i+7];

           mix(a,b,c,d,e,f,g,h);

           m[i  ] =a; m[i+1] =b; m[i+2] =c; m[i+3] =d;
           m[i+4] =e; m[i+5] =f; m[i+6] =g; m[i+7] =h;
        }
    }
    else {
        for (i = 0; i < RANDSIZ; i += 8) {
            mix(a,b,c,d,e,f,g,h);
            m[i  ]=a; m[i+1]=b; m[i+2]=c; m[i+3]=d;
            m[i+4]=e; m[i+5]=f; m[i+6]=g; m[i+7]=h;
        }
    }

    isaac32(ctx);
    ctx->randcnt=RANDSIZ;
}

// obtine un numar pe 32 biti
inline unsigned long isaacRand32(randctx_isaac32 *ctx) {
	if( !ctx->randcnt-- ) {
     	isaac32(ctx);
        ctx->randcnt=RANDSIZ-1;
        return ctx->randrsl[ctx->randcnt];
    }
    else {
    	return ctx->randrsl[ctx->randcnt];
    }

    // eroare
	return 0;
}

#endif
