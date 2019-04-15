using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoCrypto
{
    class Controller_PSNR
    {
        private double MSE,PSNR, Average_Changes;
        private int Bit_Changes, Amount_Bits, Bit_Increase, Bit_Descrease;

        public void setMSE_PSNR(Bitmap cCover,Bitmap cStego)
        {
            int Width = cCover.Width;
            int Height = cCover.Height;
            int x, y;

            Amount_Bits = Width * Height * 4;

            if(cCover == null || cStego == null)
            {
                Console.WriteLine("KSONG");
            }

            for (x = 0; x < Height; x++)
            {
                for(y = 0; y < Width; y++)
                {
                    Color iCover = cCover.GetPixel(x, y);
                    Color iStego = cStego.GetPixel(x, y);
                    
                    //Bit Changes
                    if(iCover.A != iStego.A)
                    {
                        Bit_Changes++;
                        if(iCover.A > iStego.A)
                        {
                            Bit_Descrease++;
                        }
                        else
                        {
                            Bit_Increase++;
                        }
                    }
                    if (iCover.R != iStego.R)
                    {
                        Bit_Changes++;
                        if (iCover.R > iStego.R)
                        {
                            Bit_Descrease++;
                        }
                        else
                        {
                            Bit_Increase++;
                        }
                    }
                    if (iCover.G != iStego.G)
                    {
                        Bit_Changes++;
                        if (iCover.G > iStego.G)
                        {
                            Bit_Descrease++;
                        }
                        else
                        {
                            Bit_Increase++;
                        }
                    }
                    if (iCover.B != iStego.B)
                    {
                        Bit_Changes++;
                        if (iCover.B > iStego.B)
                        {
                            Bit_Descrease++;
                        }
                        else
                        {
                            Bit_Increase++;
                        }
                    }

                    double tempMSE = (Math.Pow(iCover.A - iStego.A,2) + Math.Pow(iCover.R - iStego.R, 2) 
                        + Math.Pow(iCover.G - iStego.G, 2) + Math.Pow(iCover.B - iStego.B, 2));
                    MSE += tempMSE;
                    
                }               
            }
            MSE /= Width * Height;
            PSNR = 20 * Math.Log10(255 / Math.Sqrt(MSE));
        }

        public double getMSE()
        {
            return Math.Round(MSE, 2);
        }

        public double getPSNR()
        {
            return Math.Round(PSNR, 2);
        }

        public int getAmount_Bits()
        {
            return Amount_Bits;
        }
        
        public int getBit_Changes()
        {
            return Bit_Changes;
        }
    
        public double getAverage_Changes()
        {
            Average_Changes = Math.Round(((double)getBit_Changes() / getAmount_Bits()) * 100, 2);
            return Average_Changes;
        }

        public int getBit_Increase()
        {
            return Bit_Increase;
        }

        public int getBit_Decrease()
        {
            return Bit_Descrease;
        }
    }
}
