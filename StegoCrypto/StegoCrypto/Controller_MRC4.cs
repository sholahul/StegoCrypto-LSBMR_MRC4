using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoCrypto
{
    class Controller_MRC4
    {
        protected int[] Decimal_to_Binary(int Value)
        {
            int i;
            int[] bytes = new int[8];

            for (i = 7; i >= 0; i--)
            {
                bytes[i] = (Value % 2);
                Value = (Value / 2);
            }

            return bytes;
        }

        protected int Binary_to_Decimal(int[] Binary)
        {
            int Value = 0, i, Flag = 7;
            for (i = 0; i < Binary.Length; i++)
            {
                Value = Value + (int)(Binary[i] * Math.Pow(2, Flag));
                Flag--;
            }

            return Value;
        }

        protected int[] Shift_Left(int[] Value, int Index)
        {
            int[] Temp = new int[8];
            int i, j, Left, Right;

            Right = 8 - Index;
            Left = Index;

            i = Right;
            j = 0;

            while (i < 8)
            {
                Temp[j] = Value[i];
                i++;
                j++;
            }

            i = 0;
            j = Left;

            while (i < Right)
            {
                Temp[j] = Value[i];
                i++;
                j++;
            }

            return Temp;
        }

        protected int[] Shift_Right(int[] Value, int Index)
        {
            int[] temp = new int[8];
            int i, j, left, right;

            right = 8 - Index;
            left = Index;

            i = 0;
            j = left;

            while (i < right)
            {
                temp[i] = Value[j];
                i++;
                j++;
            }

            i = right;
            j = 0;

            while (i < 8)
            {
                temp[i] = Value[j];
                i++;
                j++;
            }

            return temp;
        }

        protected string Convert_IntArray_to_String(int[] Value)
        {
            char[] Temp = new char[Value.Length];
            int i;

            for (i = 0; i < Value.Length; i++)
            {
                Temp[i] = (char)Value[i];
            }

            string Strings = new string(Temp);

            return Strings;
        }

        protected int[] Convert_String_to_IntArray(string text)
        {
            int i;
            char[] Plain_index = text.ToCharArray();
            int[] temp = new int[text.Length];

            for (i = 0; i < text.Length; i++)
            {
                temp[i] = Convert.ToInt32(Plain_index[i]);
            }
            return temp;
        }

        protected int[] Get_KeyStream(int Length, int[] Key)
        {
            int i, temp = 0, j = 0, t = 0, k = 0;
            int[] S_BOX = new int[256];
            int[] T = new int[256];
            int[] KS = new int[Length];

            //1. Inisialisasi SBOX dan Padding kunci ke Array T
            for (i = 0; i < 255; i++)
            {
                S_BOX[i] = i;
                T[i] = Key[i % Key.Length];
            }

            //2. Permutasi Isi Array S_BOX
            for (i = 0; i < 255; i++)
            {
                j = (j + S_BOX[i] + T[i]) % 256;
                temp = S_BOX[i];
                S_BOX[i] = S_BOX[j];
                S_BOX[j] = temp;
            }

            j = 0;
            temp = 0;
            //3. PRGA
            while (k < Length)
            {
                i = (k + 1) % 256;
                j = (j + S_BOX[i]) % 256;
                temp = S_BOX[i];
                S_BOX[i] = S_BOX[j];
                S_BOX[j] = temp;
                t = (S_BOX[i] + S_BOX[j]) % 256;
                KS[k] = S_BOX[t];
                k++;
            }

            return KS;
        }

        protected int[] Encryption(int[] Plain_index, int[] KeyStream, int IV,int Shift_Bit)
        {
            int i, Temp = 0;
            int[] Temp_Binary = new int[8];

            for (i = 0; i < Plain_index.Length; i++)
            {
                Temp = (Plain_index[i] ^ IV) ^ KeyStream[i];
                Temp_Binary = Decimal_to_Binary(Temp);
                Temp_Binary = Shift_Right(Temp_Binary, Shift_Bit);
                IV = Binary_to_Decimal(Temp_Binary);
                Plain_index[i] = IV;
            }

            return Plain_index;
        }

        protected int[] Decryption(int[] Chiper_index, int[] KeyStream, int IV,int Shift_Bit)
        {
            int i;
            int[] Temp_Binary = new int[8];
            int[] Temp_Chiper = new int[Chiper_index.Length];

            for (i = 0; i < Chiper_index.Length; i++)
            {
                Temp_Chiper[i] = Chiper_index[i];
            }

            for (i = 0; i < Chiper_index.Length; i++)
            {
                Temp_Binary = Decimal_to_Binary(Chiper_index[i]);
                Temp_Binary = Shift_Left(Temp_Binary, Shift_Bit);
                Chiper_index[i] = Binary_to_Decimal(Temp_Binary);
                Chiper_index[i] = (Chiper_index[i] ^ IV) ^ KeyStream[i];
                IV = Temp_Chiper[i];
            }

            return Chiper_index;
        }
    }
}
