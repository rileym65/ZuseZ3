using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private int exponent;
        private int[] numbers;
        private Boolean sign;
        private List<uint> tape;
        private Cpu cpu;
        private String label;
        private String cmd;
        private List<String> args;
        private List<String> labels;
        private List<int> values;
        private List<int> repLine;
        private List<int> repCount;
        private List<int> callStack;
        private List<String> tokens;
        private int lastLamps;

        public Form1()
        {
            Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            InitializeComponent();
            cpu = new Cpu();
            cpu.reset();
            exponent = 0;
            numbers = new int[4];
            numbers[0] = 0; numbers[1] = 0; numbers[2] = 0; numbers[3] = 0;
            sign = false;
            lastLamps = 0;
            showExponentButtons();
            showNumberGroup1();
            showNumberGroup2();
            showNumberGroup3();
            showNumberGroup4();
            showSignButtons();
            showMemory();
/*
            test("1");
            test("10");
            test("10.5");
            test(".5");
            test(".25");
            test(".125");
            test("5.75");
            test("0.1");
            test(".1");
            test("2e1");
            test("5e-1");
            test("-5");
            test("0");
*/
        }

        private void test(String s)
        {
            int i;
            String str;
            uint t = LibRcs.strToSingle(s);
            str = LibRcs.singleToStr(t);
            if ((t & 0x80000000) == 0x80000000) debugOutput.AppendText("- "); else debugOutput.AppendText("+ ");
            t <<= 1;
            for (i = 0; i < 7; i++)
            {
                if ((t & 0x80000000) == 0x80000000) debugOutput.AppendText("1"); else debugOutput.AppendText("0");
                t <<= 1;
            }
            debugOutput.AppendText(" 1.");
            for (i = 0; i < 24; i++)
            {
                if ((t & 0x80000000) == 0x80000000) debugOutput.AppendText("1"); else debugOutput.AppendText("0");
                t <<= 1;
            }
            debugOutput.AppendText("  = " + str + "\r\n");
        }

        private void showExponentButtons()
        {
            expMinus9.BackColor = (exponent == -9) ? Color.White : Color.Gray;
            expMinus8.BackColor = (exponent == -8) ? Color.White : Color.Gray;
            expMinus7.BackColor = (exponent == -7) ? Color.White : Color.Gray;
            expMinus6.BackColor = (exponent == -6) ? Color.White : Color.Gray;
            expMinus5.BackColor = (exponent == -5) ? Color.White : Color.Gray;
            expMinus4.BackColor = (exponent == -4) ? Color.White : Color.Gray;
            expMinus3.BackColor = (exponent == -3) ? Color.White : Color.Gray;
            expMinus2.BackColor = (exponent == -2) ? Color.White : Color.Gray;
            expMinus1.BackColor = (exponent == -1) ? Color.White : Color.Gray;
            exp0.BackColor = (exponent == 0) ? Color.White : Color.Gray;
            expPlus1.BackColor = (exponent == 1) ? Color.White : Color.Gray;
            expPlus2.BackColor = (exponent == 2) ? Color.White : Color.Gray;
            expPlus3.BackColor = (exponent == 3) ? Color.White : Color.Gray;
            expPlus4.BackColor = (exponent == 4) ? Color.White : Color.Gray;
            expPlus5.BackColor = (exponent == 5) ? Color.White : Color.Gray;
            expPlus6.BackColor = (exponent == 6) ? Color.White : Color.Gray;
            expPlus7.BackColor = (exponent == 7) ? Color.White : Color.Gray;
            expPlus8.BackColor = (exponent == 8) ? Color.White : Color.Gray;
            expPlus9.BackColor = (exponent == 9) ? Color.White : Color.Gray;
        }

        private void showNumberGroup1()
        {
            number19.BackColor = (numbers[0] == 9) ? Color.White : Color.Gray;
            number18.BackColor = (numbers[0] == 8) ? Color.White : Color.Gray;
            number17.BackColor = (numbers[0] == 7) ? Color.White : Color.Gray;
            number16.BackColor = (numbers[0] == 6) ? Color.White : Color.Gray;
            number15.BackColor = (numbers[0] == 5) ? Color.White : Color.Gray;
            number14.BackColor = (numbers[0] == 4) ? Color.White : Color.Gray;
            number13.BackColor = (numbers[0] == 3) ? Color.White : Color.Gray;
            number12.BackColor = (numbers[0] == 2) ? Color.White : Color.Gray;
            number11.BackColor = (numbers[0] == 1) ? Color.White : Color.Gray;
            number10.BackColor = (numbers[0] == 0) ? Color.White : Color.Gray;
        }

        private void showNumberGroup2()
        {
            number29.BackColor = (numbers[1] == 9) ? Color.White : Color.Gray;
            number28.BackColor = (numbers[1] == 8) ? Color.White : Color.Gray;
            number27.BackColor = (numbers[1] == 7) ? Color.White : Color.Gray;
            number26.BackColor = (numbers[1] == 6) ? Color.White : Color.Gray;
            number25.BackColor = (numbers[1] == 5) ? Color.White : Color.Gray;
            number24.BackColor = (numbers[1] == 4) ? Color.White : Color.Gray;
            number23.BackColor = (numbers[1] == 3) ? Color.White : Color.Gray;
            number22.BackColor = (numbers[1] == 2) ? Color.White : Color.Gray;
            number21.BackColor = (numbers[1] == 1) ? Color.White : Color.Gray;
            number20.BackColor = (numbers[1] == 0) ? Color.White : Color.Gray;
        }

        private void showNumberGroup3()
        {
            number39.BackColor = (numbers[2] == 9) ? Color.White : Color.Gray;
            number38.BackColor = (numbers[2] == 8) ? Color.White : Color.Gray;
            number37.BackColor = (numbers[2] == 7) ? Color.White : Color.Gray;
            number36.BackColor = (numbers[2] == 6) ? Color.White : Color.Gray;
            number35.BackColor = (numbers[2] == 5) ? Color.White : Color.Gray;
            number34.BackColor = (numbers[2] == 4) ? Color.White : Color.Gray;
            number33.BackColor = (numbers[2] == 3) ? Color.White : Color.Gray;
            number32.BackColor = (numbers[2] == 2) ? Color.White : Color.Gray;
            number31.BackColor = (numbers[2] == 1) ? Color.White : Color.Gray;
            number30.BackColor = (numbers[2] == 0) ? Color.White : Color.Gray;
        }

        private void showNumberGroup4()
        {
            number49.BackColor = (numbers[3] == 9) ? Color.White : Color.Gray;
            number48.BackColor = (numbers[3] == 8) ? Color.White : Color.Gray;
            number47.BackColor = (numbers[3] == 7) ? Color.White : Color.Gray;
            number46.BackColor = (numbers[3] == 6) ? Color.White : Color.Gray;
            number45.BackColor = (numbers[3] == 5) ? Color.White : Color.Gray;
            number44.BackColor = (numbers[3] == 4) ? Color.White : Color.Gray;
            number43.BackColor = (numbers[3] == 3) ? Color.White : Color.Gray;
            number42.BackColor = (numbers[3] == 2) ? Color.White : Color.Gray;
            number41.BackColor = (numbers[3] == 1) ? Color.White : Color.Gray;
            number40.BackColor = (numbers[3] == 0) ? Color.White : Color.Gray;
        }

        private void showSignButtons()
        {
            buttonPlus.BackColor = (sign) ? Color.Gray : Color.White;
            buttonMinus.BackColor = (sign) ? Color.White : Color.Gray;
        }

        private void numberButton_Click(object sender, EventArgs e)
        {
            int tag, group, num;
            tag = Convert.ToInt32(((Button)sender).Tag);
            group = (tag / 10) - 1;
            num = (tag % 10);
            numbers[group] = num;
            if (group == 0) showNumberGroup1();
            if (group == 1) showNumberGroup2();
            if (group == 2) showNumberGroup3();
            if (group == 3) showNumberGroup4();
            cpu.setKeyboardNumber(group + 1, (uint)num);
        }

        private void expButton_Click(object sender, EventArgs e)
        {
            exponent = Convert.ToInt32(((Button)sender).Tag);
            showExponentButtons();
            cpu.setKeyboardExponent(exponent);
        }

        private void buttonSign_click(object sender, EventArgs e)
        {
            int tag;
            tag = Convert.ToInt32(((Button)sender).Tag);
            sign = (tag == 0) ? false : true;
            showSignButtons();
            cpu.setKeyboardSign(sign);
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            sourceBox.Clear();
            tapeBox.Clear();
        }

        private void assembleButton_Click(object sender, EventArgs e)
        {
            assemble();
        }

        private void error(int lineNum, String msg)
        {
            asmResults.AppendText(msg + ": [" + lineNum.ToString() + "] " + (String)sourceBox.Lines[lineNum] + "\r\n");
        }

        private int IndexOfWhiteSpace(String s)
        {
            int i;
            int ret;
            List<char> chars;
            chars = new List<char>();
            chars.Add(' ');
            chars.Add('\t');
            ret = -1;
            for (i = 0; i < chars.Count; i++)
            {
                if ((ret < 0 && s.IndexOf(chars[i]) > 0) || (s.IndexOf(chars[i]) >= 0 && s.IndexOf(chars[i]) < ret)) ret = s.IndexOf(chars[i]);
            }
            return ret;
        }

        private void parse(String line,int pass)
        {
            label = "";
            cmd = "";
            int space;
            line = line.Trim();
            args = new List<String>();
            if (line.IndexOf(';') > 0)
            {
                line = line.Substring(0, line.IndexOf(';'));
            }
            if (line.IndexOf(':') > 0)
            {
                label = line.Substring(0, line.IndexOf(':')).Trim();
                line = line.Substring(line.IndexOf(':') + 1).Trim();
            }
            space = IndexOfWhiteSpace(line);
            if (space >= 0)
            {
                cmd = line.Substring(0,space).Trim();
                line = line.Substring(space + 1).Trim();
            }
            else if (line.Length > 0)
            {
                cmd = line;
                line = "";
            }
            if (line.Length > 0)
            {
                while (line.IndexOf(',') > 0)
                {
                    args.Add(line.Substring(0,line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',')+1).Trim();
                }
                args.Add(line.Trim());
            }
        }

        private int findLabel(String s)
        {
            int ret;
            int i;
            ret = -1;
            for (i=0; i<labels.Count; i++)
                if (s.Equals((String)labels[i])) ret = i;
            return ret;
        }

        private void tokenize(String s)
        {
            int p;
            String tmp;
            s = s.ToUpper().Trim();
            tokens = new List<String>();
            p = 0;
            while (p < s.Length)
            {
                if (s[p] == ' ') p++;
                else if (s[p] >= '0' && s[p] <= '9')
                {
                    tmp = "";
                    while (p < s.Length && s[p] >= '0' && s[p] <= '9') tmp += ((char)s[p++]).ToString();
                    tokens.Add(tmp);
                }
                else if ((s[p] >= 'A' && s[p] <= 'Z') || s[p] == '_')
                {
                    tmp = "";
                    while (p < s.Length &&
                           ((s[p] >= 'A' && s[p] <= 'Z') ||
                           (s[p] >= '0' && s[p] <= '9') ||
                            s[p] == '_')) tmp += ((char)s[p++]).ToString();
                    tokens.Add(tmp);
                }
                else tokens.Add(((char)s[p++]).ToString());
            }
        }

        private Boolean eval(int lineNum,int start)
        {
            int i;
            int v1,v2;
            // ***** Evaluate variables *****
            if (start < 0)
            {
                start = 0;
                for (i=0; i<tokens.Count; i++)
                    if ((((String)tokens[i])[0] >= 'A' && ((String)tokens[i])[0] <= 'Z') ||
                        ((String)tokens[i])[0] == '_')
                    {
                        v1 = findLabel((String)tokens[i]);
                        if (v1 < 0)
                        {
                            error(lineNum,"Label not found");
                            return false;
                        }
                        tokens[i] = values[v1].ToString();
                    }
            }
            // ***** Evaluate open parens *****
            i = start;
            while (i < tokens.Count)
            {
                if (tokens[i].Equals("("))
                {
                    eval(lineNum, i+1);
                    tokens.RemoveAt(i);
                }
                else i++;
            }
            // ***** Evaluate multiplication/division *****
            i = start;
            while (i < tokens.Count)
            {
                if (tokens[i].Equals(")")) i = tokens.Count;
                else if (tokens[i].Equals("*"))
                {
                    v1 = Convert.ToInt32(tokens[i-1]);
                    v2 = Convert.ToInt32(tokens[i + 1]);
                    v1 *= v2;
                    tokens[i - 1] = v1.ToString();
                    tokens.RemoveAt(i);
                    tokens.RemoveAt(i);
                }
                else if (tokens[i].Equals("/"))
                {
                    v1 = Convert.ToInt32(tokens[i - 1]);
                    v2 = Convert.ToInt32(tokens[i + 1]);
                    v1 /= v2;
                    tokens[i - 1] = v1.ToString();
                    tokens.RemoveAt(i);
                    tokens.RemoveAt(i);
                }
                else i++;
            }
            // ***** Evaluate addition/subtraction *****
            i = start;
            while (i < tokens.Count)
            {
                if (tokens[i].Equals(")")) i = tokens.Count;
                else if (tokens[i].Equals("+"))
                {
                    v1 = Convert.ToInt32(tokens[i - 1]);
                    v2 = Convert.ToInt32(tokens[i + 1]);
                    v1 += v2;
                    tokens[i - 1] = v1.ToString();
                    tokens.RemoveAt(i);
                    tokens.RemoveAt(i);
                }
                else if (tokens[i].Equals("-"))
                {
                    v1 = Convert.ToInt32(tokens[i - 1]);
                    v2 = Convert.ToInt32(tokens[i + 1]);
                    v1 -= v2;
                    tokens[i - 1] = v1.ToString();
                    tokens.RemoveAt(i);
                    tokens.RemoveAt(i);
                }
                else i++;
            }
            // ***** Evaluate close parens *****
            i = start;
            while (i < tokens.Count)
            {
                if (tokens[i].Equals(")"))
                {
                    tokens.RemoveAt(i);
                    return true;
                }
                else i++;
            }

            return true;
        }

        private int getArgValue(int lineNum,int n)
        {
            int i;
            if (n >= args.Count) return -1;
            tokenize(args[n]);
            if (eval(lineNum, -1))
            {
                if (tokens.Count != 1)
                {
                    error(lineNum, "Expression error");
                    return -1;
                }
                try
                {
                    i = Convert.ToInt32(tokens[0]);
                }
                catch
                {
                    error(lineNum, "Expression error");
                    i = -1;
                }
                return i;
            }
            else
            {
                error(lineNum, "Expression error");
                return -1;
            }
        }
        

        private void assemble()
        {
            int i;
            int l;
            uint inst;
            Boolean err;
            uint[] t;
            String line;
            int arg,arg1,arg2;
            int pass;
            tape = new List<uint>();
            tapeBox.Clear();
            labels = new List<String>();
            values = new List<int>();
            repCount = new List<int>();
            repLine = new List<int>();
            callStack = new List<int>();
            asmResults.Clear();
            for (pass = 1; pass <= 2; pass++)
            {
                i = 0;
                while (i < sourceBox.Lines.Length)
                {
                    line = sourceBox.Lines[i].Trim().ToUpper();
                    i++;
                    parse(line, pass);
                    if (pass == 1)
                    {
                        if (label.Length > 0)
                        {
                            err = false;
                            for (l = 0; l < labels.Count; l++)
                            {
                                if (label.Equals((String)labels[l]))
                                {
                                    error(i - 1, "Label multiply defined");
                                    err = true;
                                }
                            }
                            if (err == false)
                            {
                                labels.Add(label);
                                values.Add(i - 1);
                            }
                        }
                        if (cmd.Equals("EQU"))
                        {
                            arg = getArgValue(i-1,0);
                            if (arg >= 0)
                            {
                                values[values.Count - 1] = arg;
                            }
                        }
                    }
                    if (pass == 2)
                    {
                        if (cmd.Equals("REP"))
                        {
                            arg = getArgValue(i-1,0);
                            if (arg > 0)
                            {
                                repLine.Add(i);
                                repCount.Add(arg);
                            }
                        }
                        else if (cmd.Equals("ENDREP"))
                        {
                            if (repLine.Count > 0)
                            {
                                arg = repCount[repLine.Count - 1];
                                arg--;
                                if (arg == 0)
                                {
                                    repCount.RemoveAt(repLine.Count - 1);
                                    repLine.RemoveAt(repLine.Count - 1);
                                }
                                else
                                {
                                    repCount[repLine.Count - 1] = arg;
                                    i = repLine[repLine.Count - 1];
                                }
                            }
                            else error(i - 1, "ENDREP without a matching REP"); 
                        }
                        else if (cmd.Equals("JUMP"))
                        {
                            if (args.Count < 1) error(i-1,"Destination not specified");
                            else
                            {
                                arg = getArgValue(i-1,0);
                                if (arg >= 0)
                                {
                                    i = arg;
                                }
                            }
                        }
                        else if (cmd.Equals("CALL"))
                        {
                            if (args.Count < 1) error(i-1,"Destination not specified");
                            else
                            {
                                arg = getArgValue(i-1,0);
                                if (arg >= 0)
                                {
                                    callStack.Add(i);
                                    i = arg;
                                }
                            }
                        }
                        else if (cmd.Equals("RET"))
                        {
                            if (callStack.Count < 1) error(i-1,"RET without a matching CALL");
                            else
                            {
                                i = (int)callStack[callStack.Count - 1];
                                callStack.RemoveAt(callStack.Count - 1);
                            }
                        }
                        else if (cmd.Equals("SET"))
                        {
                            if (args.Count != 2) error(i-1,"Invalid argument count");
                            else
                            {
                                arg1 = findLabel((String)args[0]);
                                arg2 = getArgValue(i-1,1);
                                if (arg1 < 0) error(i=1,"Label not found");
                                else values[arg1] = arg2;
                            }
                        }
                        else if (cmd.Equals("LU") || cmd.Equals("IN"))
                        {
                            tapeBox.AppendText("0x70\r\n");
                            tape.Add(0x70);
                        }
                        else if (cmd.Equals("LD") || cmd.Equals("OUT"))
                        {
                            tapeBox.AppendText("0x78\r\n");
                            tape.Add(0x78);
                        }
                        else if (cmd.Equals("LM") || cmd.Equals("MUL"))
                        {
                            tapeBox.AppendText("0x48\r\n");
                            tape.Add(0x48);
                        }
                        else if (cmd.Equals("LI") || cmd.Equals("DIV"))
                        {
                            tapeBox.AppendText("0x50\r\n");
                            tape.Add(0x50);
                        }
                        else if (cmd.Equals("LW") || cmd.Equals("ROOT"))
                        {
                            tapeBox.AppendText("0x58\r\n");
                            tape.Add(0x58);
                        }
                        else if (cmd.Equals("LS1") || cmd.Equals("ADD"))
                        {
                            tapeBox.AppendText("0x60\r\n");
                            tape.Add(0x60);
                        }
                        else if (cmd.Equals("LS2") || cmd.Equals("SUB"))
                        {
                            tapeBox.AppendText("0x68\r\n");
                            tape.Add(0x68);
                        }
                        else if (cmd.Equals("PR") || cmd.Equals("PS") || cmd.Equals("LOD") || cmd.Equals("STO"))
                        {
                            inst = (uint)((cmd.Equals("PR") || cmd.Equals("LOD")) ? 0xc0 : 0x80);
                            arg = getArgValue(i-1,0);
                            if (arg < 0) arg = 0;
                            if (arg > 64) arg = 63;
                            inst += (uint)arg;
                            tapeBox.AppendText("0x" + cpu.toHex(inst, 2) + "\r\n");
                            tape.Add(inst);
                        }
                        else if (cmd.Equals("EQU"))
                        {
                        }
                        else if (cmd.Length > 0)
                        {
                            error(i = 1, "Invalid command");
                        }
                    }
                }
            }
            t = new uint[tape.Count];
            for (i = 0; i < tape.Count; i++)
                t[i] = tape[i];
            cpu.loadTape(t);
        }

        private void clearDebugButton_Click(object sender, EventArgs e)
        {
            debugOutput.Clear();
        }

        private void cycle()
        {
            String debug;
            int ret;
            if (cpu.getStopped() == false)
            {
                ret = cpu.cycle();
                lightFlagLamps(cpu.getFlags());
                if (ret > 0 && (ret & Cpu.RET_DISP_CHANGED) == Cpu.RET_DISP_CHANGED)
                {
                    lightExponentLamps(cpu.getDisplayExponent());
                    lightNumberLamps(cpu.getDisplayNumber());
                    lightSignLamps(cpu.getDisplaySign());
                }
                debug = cpu.getDebug();
                if (debugEnabled.Checked) debugOutput.AppendText(debug + "\r\n");
            }
        }

        private void stepButton_Click(object sender, EventArgs e)
        {
            cpu.go();
            cycle();
            cpu.stop();
        }

        private uint fromHexString(String n)
        {
            uint ret;
            int d;
            ret = 0;
            n = n.Trim().ToUpper();
            while (n.Length > 0)
            {
                d = 0;
                if (n[0] >= '0' && n[0] <= '9') d = n[0] - '0';
                if (n[0] >= 'A' && n[0] <= 'F') d = n[0] - 'A' + 10;
                ret = (uint)((ret << 4) + d);
                n = n.Substring(1);
            }
            return ret;
        }

        private void showMemory()
        {
            mem00.Text = cpu.toHex(cpu.getMemory(0), 6);
            mem01.Text = cpu.toHex(cpu.getMemory(1), 6);
            mem02.Text = cpu.toHex(cpu.getMemory(2), 6);
            mem03.Text = cpu.toHex(cpu.getMemory(3), 6);
            mem04.Text = cpu.toHex(cpu.getMemory(4), 6);
            mem05.Text = cpu.toHex(cpu.getMemory(5), 6);
            mem06.Text = cpu.toHex(cpu.getMemory(6), 6);
            mem07.Text = cpu.toHex(cpu.getMemory(7), 6);
            mem08.Text = cpu.toHex(cpu.getMemory(8), 6);
            mem09.Text = cpu.toHex(cpu.getMemory(9), 6);
            mem10.Text = cpu.toHex(cpu.getMemory(10), 6);
            mem11.Text = cpu.toHex(cpu.getMemory(11), 6);
            mem12.Text = cpu.toHex(cpu.getMemory(12), 6);
            mem13.Text = cpu.toHex(cpu.getMemory(13), 6);
            mem14.Text = cpu.toHex(cpu.getMemory(14), 6);
            mem15.Text = cpu.toHex(cpu.getMemory(15), 6);
            mem16.Text = cpu.toHex(cpu.getMemory(16), 6);
            mem17.Text = cpu.toHex(cpu.getMemory(17), 6);
            mem18.Text = cpu.toHex(cpu.getMemory(18), 6);
            mem19.Text = cpu.toHex(cpu.getMemory(19), 6);
            mem20.Text = cpu.toHex(cpu.getMemory(20), 6);
            mem21.Text = cpu.toHex(cpu.getMemory(21), 6);
            mem22.Text = cpu.toHex(cpu.getMemory(22), 6);
            mem23.Text = cpu.toHex(cpu.getMemory(23), 6);
            mem24.Text = cpu.toHex(cpu.getMemory(24), 6);
            mem25.Text = cpu.toHex(cpu.getMemory(25), 6);
            mem26.Text = cpu.toHex(cpu.getMemory(26), 6);
            mem27.Text = cpu.toHex(cpu.getMemory(27), 6);
            mem28.Text = cpu.toHex(cpu.getMemory(28), 6);
            mem29.Text = cpu.toHex(cpu.getMemory(29), 6);
            mem30.Text = cpu.toHex(cpu.getMemory(30), 6);
            mem31.Text = cpu.toHex(cpu.getMemory(31), 6);
            mem32.Text = cpu.toHex(cpu.getMemory(32), 6);
            mem33.Text = cpu.toHex(cpu.getMemory(33), 6);
            mem34.Text = cpu.toHex(cpu.getMemory(34), 6);
            mem35.Text = cpu.toHex(cpu.getMemory(35), 6);
            mem36.Text = cpu.toHex(cpu.getMemory(36), 6);
            mem37.Text = cpu.toHex(cpu.getMemory(37), 6);
            mem38.Text = cpu.toHex(cpu.getMemory(38), 6);
            mem39.Text = cpu.toHex(cpu.getMemory(39), 6);
            mem40.Text = cpu.toHex(cpu.getMemory(40), 6);
            mem41.Text = cpu.toHex(cpu.getMemory(41), 6);
            mem42.Text = cpu.toHex(cpu.getMemory(42), 6);
            mem43.Text = cpu.toHex(cpu.getMemory(43), 6);
            mem44.Text = cpu.toHex(cpu.getMemory(44), 6);
            mem45.Text = cpu.toHex(cpu.getMemory(45), 6);
            mem46.Text = cpu.toHex(cpu.getMemory(46), 6);
            mem47.Text = cpu.toHex(cpu.getMemory(47), 6);
            mem48.Text = cpu.toHex(cpu.getMemory(48), 6);
            mem49.Text = cpu.toHex(cpu.getMemory(49), 6);
            mem50.Text = cpu.toHex(cpu.getMemory(50), 6);
            mem51.Text = cpu.toHex(cpu.getMemory(51), 6);
            mem52.Text = cpu.toHex(cpu.getMemory(52), 6);
            mem53.Text = cpu.toHex(cpu.getMemory(53), 6);
            mem54.Text = cpu.toHex(cpu.getMemory(54), 6);
            mem55.Text = cpu.toHex(cpu.getMemory(55), 6);
            mem56.Text = cpu.toHex(cpu.getMemory(56), 6);
            mem57.Text = cpu.toHex(cpu.getMemory(57), 6);
            mem58.Text = cpu.toHex(cpu.getMemory(58), 6);
            mem59.Text = cpu.toHex(cpu.getMemory(59), 6);
            mem60.Text = cpu.toHex(cpu.getMemory(60), 6);
            mem61.Text = cpu.toHex(cpu.getMemory(61), 6);
            mem62.Text = cpu.toHex(cpu.getMemory(62), 6);
            mem63.Text = cpu.toHex(cpu.getMemory(63), 6);
        }

        private void memory_changed(object sender, EventArgs e)
        {
            int tag;
            uint value;
            tag = Convert.ToInt32(((TextBox)sender).Tag);
            value = fromHexString(((TextBox)sender).Text);
            cpu.setMemory(tag, value);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            showMemory();
        }

        private void debugResetButton_Click(object sender, EventArgs e)
        {
            cpu.reset();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            cpu.go();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cpu.getStopped() == false)
            {
                cycle();
            }
            timer1.Enabled = true;
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            uint[] t;
            int i;
            t = new uint[tape.Capacity];
            for (i = 0; i < tape.Count; i++)
                t[i] = (uint)tape[i];
            cpu.loadTape(t);

        }

        private void lightFlagLamps(int flags)
        {
            if (flags == lastLamps) return;
            lastLamps = flags;
            labelEintasten.BackColor = ((flags & Cpu.E_EINTASTEN) == Cpu.E_EINTASTEN) ? Color.White : Color.Gray;
            labelAlarm.BackColor = ((flags & Cpu.E_ALARM) == Cpu.E_ALARM) ? Color.White : Color.Gray;
            labelZero.BackColor = ((flags & Cpu.E_ZERO) == Cpu.E_ZERO) ? Color.White : Color.Gray;
            labelInfinity.BackColor = ((flags & Cpu.E_INF) == Cpu.E_INF) ? Color.White : Color.Gray;
            labelZeroMulInfinity.BackColor = ((flags & Cpu.E_ZEROMULINF) == Cpu.E_ZEROMULINF) ? Color.White : Color.Gray;
            labelZeroDivZero.BackColor = ((flags & Cpu.E_DIVZERO) == Cpu.E_DIVZERO) ? Color.White : Color.Gray;
            labelInfinityDivInfinity.BackColor = ((flags & Cpu.E_DIVINF) == Cpu.E_DIVINF) ? Color.White : Color.Gray;
            labelInfinityPlusInfinity.BackColor = ((flags & Cpu.E_ADDINF) == Cpu.E_ADDINF) ? Color.White : Color.Gray;
        }

        private void lightNumberLamps(int number)
        {
            int n;
            n = (number >= 0) ? number / 10000 : -1;
            if (n >= 0) number -= (n * 10000);
            lamp1.BackColor = (n == 1) ? Color.White : Color.Gray;
            n = (number >= 0) ? number / 1000 : -1;
            if (n >= 0) number -= (n * 1000);
            lamp10.BackColor = (n == 0) ? Color.White : Color.Gray;
            lamp11.BackColor = (n == 1) ? Color.White : Color.Gray;
            lamp12.BackColor = (n == 2) ? Color.White : Color.Gray;
            lamp13.BackColor = (n == 3) ? Color.White : Color.Gray;
            lamp14.BackColor = (n == 4) ? Color.White : Color.Gray;
            lamp15.BackColor = (n == 5) ? Color.White : Color.Gray;
            lamp16.BackColor = (n == 6) ? Color.White : Color.Gray;
            lamp17.BackColor = (n == 7) ? Color.White : Color.Gray;
            lamp18.BackColor = (n == 8) ? Color.White : Color.Gray;
            lamp19.BackColor = (n == 9) ? Color.White : Color.Gray;
            n = (number >= 0) ? number / 100 : -1;
            if (n >= 0) number -= (n * 100);
            lamp20.BackColor = (n == 0) ? Color.White : Color.Gray;
            lamp21.BackColor = (n == 1) ? Color.White : Color.Gray;
            lamp22.BackColor = (n == 2) ? Color.White : Color.Gray;
            lamp23.BackColor = (n == 3) ? Color.White : Color.Gray;
            lamp24.BackColor = (n == 4) ? Color.White : Color.Gray;
            lamp25.BackColor = (n == 5) ? Color.White : Color.Gray;
            lamp26.BackColor = (n == 6) ? Color.White : Color.Gray;
            lamp27.BackColor = (n == 7) ? Color.White : Color.Gray;
            lamp28.BackColor = (n == 8) ? Color.White : Color.Gray;
            lamp29.BackColor = (n == 9) ? Color.White : Color.Gray;
            n = (number >= 0) ? number / 10 : -1;
            if (n >= 0) number -= (n * 10);
            lamp30.BackColor = (n == 0) ? Color.White : Color.Gray;
            lamp31.BackColor = (n == 1) ? Color.White : Color.Gray;
            lamp32.BackColor = (n == 2) ? Color.White : Color.Gray;
            lamp33.BackColor = (n == 3) ? Color.White : Color.Gray;
            lamp34.BackColor = (n == 4) ? Color.White : Color.Gray;
            lamp35.BackColor = (n == 5) ? Color.White : Color.Gray;
            lamp36.BackColor = (n == 6) ? Color.White : Color.Gray;
            lamp37.BackColor = (n == 7) ? Color.White : Color.Gray;
            lamp38.BackColor = (n == 8) ? Color.White : Color.Gray;
            lamp39.BackColor = (n == 9) ? Color.White : Color.Gray;
            n = number;
            lamp40.BackColor = (n == 0) ? Color.White : Color.Gray;
            lamp41.BackColor = (n == 1) ? Color.White : Color.Gray;
            lamp42.BackColor = (n == 2) ? Color.White : Color.Gray;
            lamp43.BackColor = (n == 3) ? Color.White : Color.Gray;
            lamp44.BackColor = (n == 4) ? Color.White : Color.Gray;
            lamp45.BackColor = (n == 5) ? Color.White : Color.Gray;
            lamp46.BackColor = (n == 6) ? Color.White : Color.Gray;
            lamp47.BackColor = (n == 7) ? Color.White : Color.Gray;
            lamp48.BackColor = (n == 8) ? Color.White : Color.Gray;
            lamp49.BackColor = (n == 9) ? Color.White : Color.Gray;
        }

        private void lightExponentLamps(int exp)
        {
            lampMinusInf.BackColor = (exp > -9999 && exp < -12) ? Color.White : Color.Gray;
            lampMinus12.BackColor = (exp == -12) ? Color.White : Color.Gray;
            lampMinus11.BackColor = (exp == -11) ? Color.White : Color.Gray;
            lampMinus10.BackColor = (exp == -10) ? Color.White : Color.Gray;
            lampMinus9.BackColor = (exp == -9) ? Color.White : Color.Gray;
            lampMinus8.BackColor = (exp == -8) ? Color.White : Color.Gray;
            lampMinus7.BackColor = (exp == -7) ? Color.White : Color.Gray;
            lampMinus6.BackColor = (exp == -6) ? Color.White : Color.Gray;
            lampMinus5.BackColor = (exp == -5) ? Color.White : Color.Gray;
            lampMinus4.BackColor = (exp == -4) ? Color.White : Color.Gray;
            lampMinus3.BackColor = (exp == -3) ? Color.White : Color.Gray;
            lampMinus2.BackColor = (exp == -2) ? Color.White : Color.Gray;
            lampMinus1.BackColor = (exp == -1) ? Color.White : Color.Gray;
            lampPlusInf.BackColor = (exp < 9999 && exp > 12) ? Color.White : Color.Gray;
            lampPlus12.BackColor = (exp == 12) ? Color.White : Color.Gray;
            lampPlus11.BackColor = (exp == 11) ? Color.White : Color.Gray;
            lampPlus10.BackColor = (exp == 10) ? Color.White : Color.Gray;
            lampPlus9.BackColor = (exp == 9) ? Color.White : Color.Gray;
            lampPlus8.BackColor = (exp == 8) ? Color.White : Color.Gray;
            lampPlus7.BackColor = (exp == 7) ? Color.White : Color.Gray;
            lampPlus6.BackColor = (exp == 6) ? Color.White : Color.Gray;
            lampPlus5.BackColor = (exp == 5) ? Color.White : Color.Gray;
            lampPlus4.BackColor = (exp == 4) ? Color.White : Color.Gray;
            lampPlus3.BackColor = (exp == 3) ? Color.White : Color.Gray;
            lampPlus2.BackColor = (exp == 2) ? Color.White : Color.Gray;
            lampPlus1.BackColor = (exp == 1) ? Color.White : Color.Gray;
            lampExp0.BackColor = (exp == 0) ? Color.White : Color.Gray;
        }

        private void lightSignLamps(int sign)
        {
            lampPlus.BackColor = (sign == 1) ? Color.White : Color.Gray;
            lampMinus.BackColor = (sign == -1) ? Color.White : Color.Gray;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            lightNumberLamps(-1);
            lightFlagLamps(0);
            lightExponentLamps(-9999);
            lightSignLamps(0);
        }

        private void numberBox_TextChanged(object sender, EventArgs e)
        {
            double mantissa;
            int exponent;
            int ex;
            int m;
            int mask;
            int result;
            Boolean sign;
            try
            {
                mantissa = Convert.ToDouble(significandBox.Text);
            }
            catch
            {
                mantissa = 0;
            }
            try
            {
                exponent = Convert.ToInt32(exponentBox.Text);
            }
            catch
            {
                exponent = 0;
            }
            if (exponent > 63) exponent = 63;
            if (exponent < -64) exponent = -64;
            sign = (mantissa >= 0) ? false : true;
            if (sign) mantissa = -mantissa;
            if (mantissa != 0)
            {
                m = (int)mantissa;
                mantissa -= m;
                if (m != 0)
                {
                    ex = 14;
                    while (m > 0x4000)
                    {
                        m >>= 1;
                        ex++;
                    }
                    while (m != 0 && (m & 0x4000) != 0x4000)
                    {
                        m <<= 1;
                        ex--;
                    }
                    mask = 1 << (13 - ex);
                }
                else
                {
                    ex = -1;
                    mask = 0x4000;
                }
                while (mask > 0 && mantissa > 0)
                {
                    mantissa *= 2;
                    if (mantissa >= 1.0)
                    {
                        m |= mask;
                        mantissa -= 1;
                    }
                    if (m >= 0x4000) mask >>= 1; else ex--;
                }
                if (ex < -64) ex = -64;
                if (ex > 63) ex = 63;
                result = ((ex & 0x7f) << 14) | (m & 0x3fff);
                if (sign) result |= 1 << 21;
            }
            else
            {
                result = (0x40 << 14);
            }
            hexbox.Text = result.ToString("X6");
            binaryBox.Text = cpu.binaryToStr((uint)result);
        }

        private void endlessLoop_CheckedChanged(object sender, EventArgs e)
        {
            cpu.EndlessLoop = endlessLoop.Checked;
        }

        private void manualCommand_Click(object sender, EventArgs e)
        {
            int tag;
            int addr;
            String debug;
            tag = Convert.ToInt32(((Button)sender).Tag);
            if (tag == 192 || tag == 128)
            {
                addr = numbers[2] * 10 + numbers[3];
                if (addr > 59) addr = 59;
                tag |= addr;
            }
            cpu.executeInstruction((uint)tag);
            lightFlagLamps(cpu.getFlags());
            if (tag == 120)
            {
                lightExponentLamps(cpu.getDisplayExponent());
                lightNumberLamps(cpu.getDisplayNumber());
                lightSignLamps(cpu.getDisplaySign());
            }
            debug = cpu.getDebug();
            if (debugEnabled.Checked) debugOutput.AppendText("Manual " + debug + "\r\n");

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int i;
            StreamWriter file;
            saveFileDialog1.Filter = "Assembly files (*.asm)|*.asm|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file = new StreamWriter(saveFileDialog1.FileName);
                if (file != null)
                {
                    for (i = 0; i < sourceBox.Lines.Length; i++)
                        file.WriteLine(sourceBox.Lines[i]);
                    file.Close();
                }
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            StreamReader file;
            String line;
            openFileDialog1.Filter = "Assembly files (*.asm)|*.asm|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file = new StreamReader(openFileDialog1.FileName);
                if (file != null)
                {
                    sourceBox.Clear();
                    while (!file.EndOfStream)
                    {
                        line = file.ReadLine();
                        sourceBox.AppendText(line + "\r\n");
                    }
                    file.Close();
                }
            }
        }


    }
}
