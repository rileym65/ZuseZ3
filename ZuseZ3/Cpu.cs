using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class Cpu
    {
        public const int E_ALARM = 1;
        public const int E_ZERO = 2;
        public const int E_INF = 4;
        public const int E_ZEROMULINF = 8;
        public const int E_DIVZERO = 16;
        public const int E_DIVINF = 32;
        public const int E_ADDINF = 64;
        public const int E_EINTASTEN = 128;

        public const int RET_WAITKEY = 1;
        public const int RET_DISP_CHANGED = 2;
        public const int RET_FLAGS_CHANGED = 4;

        private uint[] memory;
        private uint r1;
        private uint r2;
        private uint Ba;
        private uint Be;
        private Boolean first;
        private String debug;
        private int cycles;
        private uint[] tape;
        private int tapePos;
        private Boolean stopped;
        private uint[] keyboardNumber;
        private int keyboardExponent;
        private Boolean keyboardSign;
        private Boolean readKeyboard;
        private int retVal;
        private int flags;
        private int displayNumber;
        private int displayExponent;
        private int displaySign;
        private Boolean endlessLoop;

        public Cpu()
        {
            int i;
            memory = new uint[64];
            tape = null;
            keyboardExponent = 0;
            keyboardSign = false;
            readKeyboard = false;
            keyboardNumber = new uint[4];
            for (i = 0; i < 4; i++) keyboardNumber[i] = 0;
            endlessLoop = false;
            reset();
        }

        public void reset()
        {
            r1 = 0;
            r2 = 0;
            first = true;
            cycles = 0;
            stopped = true;
            flags = 0;
        }

        public Boolean EndlessLoop
        {
            set { endlessLoop = value; }
        }

        public int getDisplayNumber()
        {
            return displayNumber;
        }

        public int getDisplayExponent()
        {
            return displayExponent;
        }

        public int getDisplaySign()
        {
            return displaySign;
        }

        public String getDebug()
        {
            String ret;
            ret = debug;
            debug = "";
            return ret;
        }

        public void go()
        {
            stopped = false;
            flags = 0;
        }

        public void stop()
        {
            stopped = true;
        }

//        public void setKeyboard(int number, int exponent, Boolean sign)
//        {
//            keyboardNumber = number;
//            keyboardExponent = exponent;
//            keyboardSign = sign;
//        }

        public void setKeyboardNumber(int column, uint key)
        {
            keyboardNumber[column - 1] = key;
        }

        public void setKeyboardExponent(int e)
        {
            keyboardExponent = e;
        }

        public void setKeyboardSign(Boolean b)
        {
            keyboardSign = b;
        }

        public void setMemory(int addr, uint value)
        {
            memory[addr] = value;
        }

        public uint getMemory(int addr)
        {
            return memory[addr];
        }

        public int getFlags()
        {
            return flags;
        }

        public Boolean getStopped()
        {
            return stopped;
        }

        public void loadTape(uint[] t)
        {
            tape = t;
            tapePos = 0;
        }

        private uint readTape()
        {
            if (tape == null) return 0xffffffff;
            if (tapePos >= tape.Length)
            {
                if (endlessLoop == false) return 0xffffffff;
                tapePos = 0;
            }
            return tape[tapePos++];
        }

        public String binaryToStr(uint n)
        {
            int i;
            int exp1;
            int sig1;
            Boolean sign1;
            String ret;
            exp1 = (int)((n >> 14) & 0x7f);
            if (exp1 > 0x3f) exp1 = -((exp1 ^ 0x7f) + 1);
            sign1 = (((n >> 21) & 1) == 1) ? true : false;
            sig1 = (int)(n & 0x3fff);
            ret = (sign1) ? "-" : "";
            ret += "1.";
            for (i = 0; i < 14; i++)
            {
                ret += Convert.ToChar(((sig1 & 0x2000) == 0x2000) ? "1" : "0").ToString();
                sig1 <<= 1;
            }
            ret += "e";
            ret += exp1.ToString();
            return ret;
        }

        private void doLoadAddress(uint inst)
        {
            uint addr;
            addr = inst & 0x3f;
            if (first)
            {
                r1 = memory[addr];
                first = false;
                debug += "PR " + addr.ToString() + ": \tR1 = " + binaryToStr(r1);
            }
            else
            {
                r2 = memory[addr];
                debug += "PR " + addr.ToString() + ": \tR2 = " + binaryToStr(r2);
            }
        }

        private void normalize(Boolean sign, int exponent, uint sig)
        {
            if (sig == 0)
            {
                r1 = 0x100000;
                r1 |= (uint)((sign) ? 0x200000 : 0);
                return;
            }
            while (sig >= 0x40000)
            {
                sig >>= 1;
                exponent++;
            }
            while (sig < 0x20000)
            {
                sig <<= 1;
                exponent--;
            }
            if (exponent >= 63) exponent = 63;
            if (exponent <= -64) exponent = -64;
            exponent &= 0x7f;
            r1 = (sig >> 3) & 0x3fff;
            r1 |= (uint)((exponent & 0x7f) << 14);
            r1 |= (uint)((sign) ? 0x200000 : 0);
        }

        private void doAddition(uint inst, char mode)
        {
            int exp1, exp2, exp3;
            Boolean sign;
            if (mode == 'A') debug += "LS1:\tR1 = " + binaryToStr(r1) + " + " + binaryToStr(r2) + " = ";
            if (mode == 'S') debug += "LS2:\tR1 = " + binaryToStr(r1) + " + " + binaryToStr(r2) + " = ";
            exp1 = (int)((r1 >> 14) & 0x7f);
            exp2 = (int)((r2 >> 14) & 0x7f);
            sign = ((r1 & 0x200000) == (r2 & 0x200000)) ? true : false;
            if (exp1 == 0x3f && exp2 == 0x3f)
            {
                flags |= E_ALARM;
                flags |= (exp1 == exp2) ? E_ADDINF : E_INF;
                stopped = true;
            }
            r1 = LibRcs.addFp(r1 << 10, r2 << 10) >> 10;
            exp3 = (int)((r1 >> 14) & 0x7f);
            if (exp1 != 0x40 && exp2 != 0x40 && exp3 == 0x40 && sign)
            {
                flags |= E_ALARM;
                flags |= E_ZERO;
                stopped = true;
            }
            if (exp1 != 0x3f && exp2 != 0x3f && exp3 == 0x3f)
            {
                flags |= E_ALARM;
                flags |= E_INF;
                stopped = true;
            }
            debug += binaryToStr(r1);
        }

        private void doSubtraction(uint inst)
        {
            r2 ^= 0x200000;
            doAddition(inst, 'S');
        }

        private void doMultiplication(uint inst)
        {
            int exp1, exp2, exp3;
            debug += "LM:\tR1 = " + binaryToStr(r1) + " * " + binaryToStr(r2) + " = ";
            exp1 = (int)((r1 >> 14) & 0x7f);
            exp2 = (int)((r2 >> 14) & 0x7f);
            if (exp1 == 0x40 && exp2 == 0x3f)
            {
                flags |= E_ZEROMULINF | E_ALARM;
                stopped = true;
            }
            if (exp1 == 0x3f && exp2 == 0x40)
            {
                flags |= E_ZEROMULINF | E_ALARM;
                stopped = true;
            }

            r1 = LibRcs.mulFp((uint)(r1 << 10), (uint)(r2 << 10)) >> 10;
            exp3 = (int)((r1 >> 14) & 0x7f);
            if (exp1 != 0x40 && exp2 != 0x40 && exp3 == 0x40)
            {
                flags |= E_ALARM;
                flags |= E_ZERO;
                stopped = true;
            }
            if (exp1 != 0x3f && exp2 != 0x3f && exp3 == 0x3f)
            {
                flags |= E_ALARM;
                flags |= E_INF;
                stopped = true;
            }
            debug += binaryToStr(r1);
            return;
        }

        /*
        private int mul(int a, int b)
        {
            int i;
            int tmp;
            tmp = 0;
            for (i = 0; i < 17; i++)
            {
                if ((b & 0x20000) == 0x20000)
                {
                    tmp += a;
                }
                b <<= 1;
                a >>= 1;
            }
            return tmp;
        }
        */
        private void doDivision(uint inst)
        {
            int exp1, exp2, exp3;
            debug += "LI:\tR1 = " + binaryToStr(r1) + " / " + binaryToStr(r2) + " = ";
            exp1 = (int)((r1 >> 14) & 0x7f);
            exp2 = (int)((r2 >> 14) & 0x7f);
            if (exp1 == 0x40 && exp2 == 0x40)
            {
                flags |= E_DIVZERO | E_ALARM;
                stopped = true;
            }
            if (exp1 == 0x3f && exp2 == 0x3f)
            {
                flags |= E_DIVINF | E_ALARM;
                stopped = true;
            }
            r1 = LibRcs.divFp(r1 << 10, r2 << 10) >> 10;
            r2 = 0;
            exp3 = (int)((r1 >> 14) & 0x7f);
            if (exp1 != 0x40 && exp2 != 0x40 && exp3 == 0x40)
            {
                flags |= E_ALARM;
                flags |= E_ZERO;
                stopped = true;
            }
            if (exp1 != 0x3f && exp2 != 0x3f && exp3 == 0x3f)
            {
                flags |= E_ALARM;
                flags |= E_INF;
                stopped = true;
            }
            debug += binaryToStr(r1);
        }

        private void doRoot(uint inst)
        {
            debug += "LW:\tR1 = " + binaryToStr(r1) + " = ";
            r1 = LibRcs.squareRootFp(r1 << 10) >> 10;
            debug += binaryToStr(r1);
            return;
        }

        private void doReadKeyboard(uint inst)
        {
            retVal = RET_FLAGS_CHANGED | RET_WAITKEY;
            flags |= E_EINTASTEN;
            debug += "LU: Machine stopped to await user input";
            stopped = true;
            readKeyboard = true;
        }

        private void doDisplayResult(uint inst)
        {
            int exp;
            uint sig;
            double value;
            double frac;
            debug += "LD ";
            r2 = 0;
            retVal |= RET_DISP_CHANGED;
            first = true;
            displaySign = ((r1 & 0x200000) == 0x200000) ? -1 : 1;
            value = 0;
            exp = (int)((r1 >> 14) & 0x7f);
            if (exp == 0x40)
            {
                displaySign = 0;
                displayExponent = 0;
                displayNumber = 0;
                r1 = 0;
                stopped = true;
                return;
            }
            if (exp > 63) exp = -((exp ^ 0x7f) + 1);
            sig = (r1 & 0x3fff) | 0x4000;
            value = 0;
            exp++;
            while (exp > 0)
            {
                value *= 2;
                exp--;
                if ((sig & 0x4000) == 0x4000) value++;
                sig = (sig << 1) & 0x7fff;
            }
            frac = .5;
            while (exp < 0)
            {
                frac /= 2;
                exp++;
            }
            while (sig > 0)
            {
                if ((sig & 0x4000) == 0x4000) value += frac;
                sig = (sig << 1) & 0x7fff;
                frac /= 2;
            }
            displayExponent = 3;
            while (value < 1999)
            {
                value *= 10;
                displayExponent--;
            }
            while (value > 19999)
            {
                value /= 10;
                displayExponent++;
            }
            displayNumber = (int)value;
            r1 = 0;
            first = true;
            stopped = true;
        }

        private void doStoreAddress(uint inst)
        {
            uint addr;
            addr = inst & 0x3f;
            memory[addr] = r1;
            debug += "PS " + addr.ToString() + ": \tmem[" + addr.ToString() + "] = " + binaryToStr(r1);
            r1 = 0;
            first = true;
        }

        public String toHex(uint n, int digits)
        {
            int i;
            uint d;
            String ret;
            ret = "";
            for (i = 0; i < digits; i++)
            {
                d = n & 0xf;
                ret = Convert.ToChar((d < 10) ? '0' + d : 'A' + d - 10).ToString() + ret;
                n >>= 4;
            }
            return ret;
        }

        private void convertKeyboard()
        {
            int i;
            int e;
            int ke;
            int pointOne;
            Ba = 0;
            for (i = 0; i < 4; i++)
            {
                Ba *= 10;
                Ba += keyboardNumber[i];
            }
            if (Ba == 0)
            {
                r1 = 0x100000;
            }
            else
            {
                e = 14;
                while ((Ba & 0x4000) != 0x4000)
                {
                    Ba <<= 1;
                    e--;
                }
                ke = keyboardExponent;
                while (ke > 0)
                {
                    Ba <<= 1;
                    Be = Ba << 2;
                    Ba += Be;
                    while (Ba >= 0x8000)
                    {
                        Ba >>= 1;
                        e++;
                    }
                    ke--;
                }
                while (ke < 0)
                {
                    Be = Ba << 3;
                    Ba = 0;
                    pointOne = 0x033333;
                    while (pointOne != 0)
                    {
                        if ((pointOne & 0x200000) == 0x200000)
                        {
                            Ba += Be;
                        }
                        Be >>= 1;
                        pointOne = (pointOne << 1) & 0x3fffff;
                    }
                    while ((Ba & 0x20000) != 0x20000)
                    {
                        Ba <<= 1;
                        e--;
                    }
                    Ba >>= 3;
                    ke++;
                }
                r1 = (uint)((Ba & 0x3fff) | ((e & 0x7f) << 14));
                if (keyboardSign) r1 |= 0x200000;
            }
        }

        public int executeInstruction(uint inst)
        {
            if ((inst & 0xc0) == 0xc0) doLoadAddress(inst);
            else if ((inst & 0xc0) == 0x80) doStoreAddress(inst);
            else switch (inst)
                {
                    case 0x48: doMultiplication(inst); break;
                    case 0x50: doDivision(inst); break;
                    case 0x58: doRoot(inst); break;
                    case 0x60: doAddition(inst, 'A'); break;
                    case 0x68: doSubtraction(inst); break;
                    case 0x70: doReadKeyboard(inst); break;
                    case 0x78: doDisplayResult(inst); break;
                }
            return retVal;
        }

        public int cycle()
        {
            uint inst;
            if (stopped) return -1;
            retVal = 0;
            if (readKeyboard)
            {
                readKeyboard = false;
                convertKeyboard();
                debug += "\t\tR1 = " + binaryToStr(r1);
                r2 = 0;
//                flags ^= E_EINTASTEN;
                retVal |= RET_FLAGS_CHANGED;
                first = false;
                return retVal;
            }
            inst = readTape();
            if (inst == 0xffffffff)
            {
                stopped = true;
                return -1;
            }
            cycles++;
            debug = "[" + cycles.ToString() + "]\t0x" + toHex(inst,2) + "\t";
            return executeInstruction(inst);
        }
    }
}
