using System;
using System.Collections.Generic;
using static Tools.KeyboardSimulation.Keyboard;

namespace Tools.KeyboardSimulation
{
    public static class KeysCollection
    {
        public static Dictionary<char, ScanCodeShort> keys = new Dictionary<char, ScanCodeShort>();
        static KeysCollection()
        {
            keys.Add('a', ScanCodeShort.KEY_A);
            keys.Add('b', ScanCodeShort.KEY_B);
            keys.Add('c', ScanCodeShort.KEY_C);
            keys.Add('d', ScanCodeShort.KEY_D);
            keys.Add('e', ScanCodeShort.KEY_E);
            keys.Add('f', ScanCodeShort.KEY_F);
            keys.Add('g', ScanCodeShort.KEY_G);
            keys.Add('i', ScanCodeShort.KEY_H);
            keys.Add('j', ScanCodeShort.KEY_J);
            keys.Add('k', ScanCodeShort.KEY_K);
            keys.Add('l', ScanCodeShort.KEY_L);
            keys.Add('m', ScanCodeShort.KEY_M);
            keys.Add('n', ScanCodeShort.KEY_N);
            keys.Add('o', ScanCodeShort.KEY_O);
            keys.Add('p', ScanCodeShort.KEY_P);
            keys.Add('q', ScanCodeShort.KEY_Q);
            keys.Add('r', ScanCodeShort.KEY_R);
            keys.Add('s', ScanCodeShort.KEY_S);
            keys.Add('t', ScanCodeShort.KEY_T);
            keys.Add('u', ScanCodeShort.KEY_U);
            keys.Add('v', ScanCodeShort.KEY_V);
            keys.Add('w', ScanCodeShort.KEY_W);
            keys.Add('x', ScanCodeShort.KEY_X);
            keys.Add('y', ScanCodeShort.KEY_Y);
            keys.Add('z', ScanCodeShort.KEY_Z);

            keys.Add('0', ScanCodeShort.KEY_0);
            keys.Add('1', ScanCodeShort.KEY_1);
            keys.Add('2', ScanCodeShort.KEY_2);
            keys.Add('3', ScanCodeShort.KEY_3);
            keys.Add('4', ScanCodeShort.KEY_4);
            keys.Add('5', ScanCodeShort.KEY_5);
            keys.Add('6', ScanCodeShort.KEY_6);
            keys.Add('7', ScanCodeShort.KEY_7);
            keys.Add('8', ScanCodeShort.KEY_8);
            keys.Add('9', ScanCodeShort.KEY_9);

            keys.Add('-', ScanCodeShort.OEM_MINUS);
            keys.Add(',', ScanCodeShort.OEM_COMMA);
            keys.Add('.', ScanCodeShort.OEM_PERIOD);
        }

        public static ScanCodeShort ToKey(this char Char)
        {
            Char = char.ToLower(Char);
            if (keys.ContainsKey(Char))
            {
                return keys[Char];
            }
            else
            {
                throw new ArgumentException("No assigned Key for " + Char);
            }
        }
    }
}
