using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class LibRcs
    {

        static public uint addFp(uint v1, uint v2)
        {
            int exp1, exp2, expT;
            uint m1, m2;
            uint res;
            Boolean negative;
            negative = false;
            exp1 = (int)((v1 >> 24) & 0x7f);
            exp2 = (int)((v2 >> 24) & 0x7f);
            if (exp1 == 0x40) return v2;
            if (exp2 == 0x40) return v1;
            if (exp1 >= 0x40) { exp1 = -((~exp1 + 1) & 0x7f); }
            if (exp2 >= 0x40) { exp2 = -((~exp2 + 1) & 0x7f); }
            if (exp2 > exp1)
            {
                expT = exp1; exp1 = exp2; exp2 = expT;
                m1 = v1; v1 = v2; v2 = m1;
            }
            m1 = (v1 & 0xffffff) | 0x1000000;
            m2 = (v2 & 0xffffff) | 0x1000000;
            while (exp1 > exp2)
            {
                m2 >>= 1;
                exp2++;
            }
            if ((v1 & 0x80000000) == (v2 & 0x80000000))
            {
                res = m1 + m2;
                negative = ((v1 & 0x80000000) == 0x80000000) ? true : false;
            }
            else
            {
                res = (m1 >= m2) ? m1 - m2 : m2 - m1;
                if (m1 >= m2)
                    negative = ((v1 & 0x80000000) == 0x80000000) ? true : false;
                else
                    negative = ((v2 & 0x80000000) == 0x80000000) ? true : false;
            }
            if (res == 0)
            {
                exp1 = -64;
            }
            else
            {
                while (res >= 0x2000000)
                {
                    res >>= 1;
                    exp1++;
                }
                while (res < 0x1000000)
                {
                    res <<= 1;
                    exp1--;
                }
            }
            if (exp1 > 63) exp1 = 63;
            if (exp1 < -64) exp1 = -64;
            res = (uint)((res & 0xffffff) | (uint)((exp1 & 0x7f) << 24));
            res |= (negative) ? 0x80000000 : 0;
            return res;
        }

        static public uint mulFp(uint v1, uint v2)
        {
            int exp1, exp2;
            uint m1, m2;
            uint res;
            Boolean negative;
            negative = ((v1 & 0x80000000) == (v2 & 0x80000000)) ? false : true;
            exp1 = (int)((v1 >> 24) & 0x7f);
            if (exp1 >= 0x40) { exp1 = -((~exp1 + 1) & 0x7f); }
            m1 = (v1 & 0xffffff) | 0x1000000;
            exp2 = (int)((v2 >> 24) & 0x7f);
            if (exp2 >= 0x40) { exp2 = -((~exp2 + 1) & 0x7f); }
            m2 = (v2 & 0xffffff) | 0x1000000;
            if (exp1 == -64 || exp2 == -64) return 0x40000000;
            if (exp1 == 0x3f) return v2;
            if (exp2 == 0x3f) return v1;
            res = 0;
            while (m2 != 0 && m1 != 0)
            {
                if ((m2 & 0x1000000) == 0x1000000) res += m1;
                m1 >>= 1;
                m2 <<= 1;
                m2 = m2 & 0x1ffffff;
            }
            exp1 += exp2;
            while (res >= 0x2000000)
            {
                res >>= 1;
                exp1++;
            }
            while (res < 0x1000000)
            {
                res <<= 1;
                exp1--;
            }
            if (exp1 > 63) exp1 = 63;
            if (exp1 < -64) exp1 = -64;
            res &= 0xffffff;
            res |= (uint)((exp1 & 0x7f) << 24);
            res |= (negative) ? 0x80000000 : 0;
            return res;
        }

        static public uint divFp(uint v1, uint v2)
        {
            int i;
            int exp1, exp2;
            uint m1, m2;
            uint res;
            uint mask;
            Boolean negative;
            negative = ((v1 & 0x80000000) == (v2 & 0x80000000)) ? false : true;
            exp1 = (int)((v1 >> 24) & 0x7f);
            if (exp1 >= 0x40) { exp1 = -((~exp1 + 1) & 0x7f); }
            m1 = (v1 & 0xffffff) | 0x1000000;
            exp2 = (int)((v2 >> 24) & 0x7f);
            if (exp2 >= 0x40) { exp2 = -((~exp2 + 1) & 0x7f); }
            m2 = (v2 & 0xffffff) | 0x1000000;
            res = 0;
            mask = 0x1000000;
            for (i = 0; i < 24; i++)
            {
                if (m2 <= m1)
                {
                    res |= mask;
                    m1 -= m2;
                }
                mask >>= 1;
                m2 >>= 1;
            }
            if (exp1 <= -64 || exp2 <= -64) exp1 = -64;
            else if (exp1 >= 63 || exp2 >= 63) exp1 = 63;
            else exp1 -= exp2;
            while (res >= 0x2000000)
            {
                res >>= 1;
                exp1++;
            }
            while (res < 0x1000000)
            {
                res <<= 1;
                exp1--;
            }
            if (exp1 > 63) exp1 = 63;
            if (exp1 < -64) exp1 = -64;
            res &= 0xffffff;
            res |= (uint)((exp1 & 0x7f) << 24);
            res |= (negative) ? 0x80000000 : 0;
            return res;
        }

        static public uint squareRootFp(uint v)
        {
            int i;
            uint bt;
            uint Ba;
            uint Bb;
            uint Bf;
            uint Be;
            uint Aa;
            uint Af;
            uint res;
            Af = (v >> 24) & 0x7f;
            Bf = (v & 0xffffff) | 0x1000000;
            Aa = 0;
            Ba = 0;
            Ba = ((Af & 1) == 1) ? Bf << 1 : Bf;
            Bb = 0x1000000;
            Bf = 0;
            for (i = 0; i < 26; i++)
            {
                if (Ba >= Bb)
                {
                    Be = Ba - Bb;
                    bt = (uint)(0x1000000 >> i);
                }
                else
                {
                    Be = Ba;
                    bt = 0;
                }
                if (bt != 0) Bf |= bt;
                Ba = Be << 1;
                Bb = Bf << 1;
                Bb |= (uint)(0x800000 >> i);
            }
            Aa = (Af < 0x40) ? Af >> 1 : (Af >> 1) | 0x40;
            res = (Bf & 0xffffff) | ((Aa & 0x7f) << 24);
            return res;
        }

        static public uint mul10(uint n)
        {
            return mulFp(n, 0x03400000);
        }

        static public uint mulPoint1(uint n)
        {
            return mulFp(n, 0x7c999999);
        }

        static public uint strToSingle(String s)
        {
            int exp;
            uint mant;
            Boolean sign;
            int[] digits;
            int carry;
            Boolean zero;
            int i;
            int mask;
            digits = new int[24];
            sign = false;
            if (s[0] == '-')
            {
                sign = true;
                s = s.Substring(1);
            }
            if (s[0] == '+')
            {
                sign = false;
                s = s.Substring(1);
            }
            mant = 0;
            while (s.Length > 0 && s[0] >= '0' && s[0] <= '9')
            {
                mant *= 10;
                mant += (uint)(s[0] - '0');
                s = s.Substring(1);
            }
            if (mant != 0)
            {
                exp = 24;
                while (mant < 0x1000000)
                {
                    mant <<= 1;
                    exp--;
                }
            }
            else
            {
                exp = 0;
            }
            if (s.Length > 0 && s[0] == '.')
            {
                s = s.Substring(1);
                i = 0;
                while (i < 24 && s.Length > 0 && s[0] >= '0' && s[0] <= '9')
                {
                    digits[i] = s[0] - '0';
                    i++;
                    s = s.Substring(1);
                }
                mask = 0x800000 >> exp;
                while (mask != 0)
                {
                    zero = true;
                    carry = 0;
                    for (i = 23; i >= 0; i--)
                    {
                        digits[i] = digits[i] + digits[i] + carry;
                        if (digits[i] >= 10)
                        {
                            digits[i] -= 10;
                            carry = 1;
                        }
                        else carry = 0;
                        if (digits[i] != 0) zero = false;
                    }
                    if (carry > 0) mant |= (uint)mask;
                    mask >>= 1;
                    if (zero) mask = 0;
                }
                while (mant < 0x1000000)
                {
                    mant <<= 1;
                    exp--;
                }
            }
            if (mant == 0) return 0x40000000;
            mant &= 0xffffff;
            mant |= (uint)((exp & 0x7f) << 24);
            mant |= (sign) ? 0x80000000 : 0;
            if (s.Length > 0 && (s[0] == 'e' || s[0] == 'E'))
            {
                s = s.Substring(1);
                sign = false;
                if (s[0] == '-')
                {
                    sign = true;
                    s = s.Substring(1);
                }
                if (s[0] == '+')
                {
                    sign = false;
                    s = s.Substring(1);
                }
                exp = 0;
                while (s.Length > 0 && s[0] >= '0' && s[0] <= '9')
                {
                    exp *= 10;
                    exp += (s[0] - '0');
                    s = s.Substring(1);
                }
                if (sign) exp = -exp;
                while (exp > 0) { mant = mul10(mant); exp--; }
                while (exp < 0) { mant = mulPoint1(mant); exp++; }
            }
            return mant;
        }

        static public int getExp(uint n)
        {
            int ret;
            ret = (int)(n >> 24) & 0x7f;
            if (ret >= 0x40) { ret = -((~ret + 1) & 0x7f); }
            return ret;
        }

        static public String singleToStr(uint n)
        {
            String ret;
            int exp;
            if (((n >> 24) & 0x7f) == 0x40) return "0";
            if (((n >> 24) & 0x7f) == 0x3f) return "<INF>";
            ret = "";
            exp = 0;
            return "0";
            while (ret.Length < 4)
            {
                if ((n & 0x40000000) == 0x40000000)
                {
                }
                else
                {
                }
            }


            return "<UNK>";
        }


    }

}
