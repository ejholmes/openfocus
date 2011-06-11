#ifndef __util_h_
#define __util_h_

static inline void endian_swap(void *dest, const void *source, int length)
{
    unsigned char *dptr = (unsigned char *)dest;
    unsigned char *sptr = (unsigned char *)source;
    int i;
    for (i = 0; i < length; i++) {
        dptr[i] = (sptr[length - i]);
    }
}

#endif
