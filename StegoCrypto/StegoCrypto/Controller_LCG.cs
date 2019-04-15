using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoCrypto
{
    class Controller_LCG
    {
        int Height, Width;

        public Controller_LCG(int Height, int Width)
        {
            this.Height = Height;
            this.Width = Width;
        }

        private int[,] Index_Matrix()
        {
            int i, j;
            int flag = 0;
            int Row = Height;
            int Col = Width;
            int Length = Row * Col;
            int[,] Temp = new int[Length, 2];


            for (i = 0; i < Row; i++)
            {
                for (j = 0; j < Col; j++)
                {
                    Temp[flag, 0] = i;
                    Temp[flag, 1] = j;
                    flag++;
                }
            }

            return Temp;
        }

        private int[] PRNG_LCG(int A, int C, int M, int Seed, int Pixel_Needed)
        {
            int i;
            int Temp = 0;
            int[] Xn = new int[Pixel_Needed];

            for (i = 0; i < Pixel_Needed; i++)
            {
                Temp = ((A * Seed) + C) % M;
                Seed = Temp;
                Xn[i] = Seed;
            }

            return Xn;
        }

        public int[,] Get_Pixel_Random(int A, int C, int M, int Seed, int Piksel_Needed)
        {
            int Length = Height * Width; //banyak piksel
            int[,] Index = new int[Length, 2]; //Convert 2D Matrix
            int[,] Index_Temp = new int[Piksel_Needed, 2];

            int[] Temp_Random = new int[Piksel_Needed];
            int i, index_random;

            Index = Index_Matrix();

            Temp_Random = PRNG_LCG(A, C, M, Seed, Piksel_Needed);

            for (i = 0; i < Piksel_Needed; i++)
            {
                index_random = Temp_Random[i];
                Index_Temp[i, 0] = Index[index_random, 0];
                Index_Temp[i, 1] = Index[index_random, 1];
            }

            return Index_Temp;
        }
    }
}
