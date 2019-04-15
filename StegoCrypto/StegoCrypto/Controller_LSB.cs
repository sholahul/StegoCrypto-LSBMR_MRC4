using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Windows.Forms;

namespace StegoCrypto
{
    class Controller_LSB : Controller_MRC4
    {
        Bitmap Image_Processing,Stego;
        string Key, Plaintext;
        int[] Setting;
        int NCharacter;

        public Controller_LSB(Bitmap Img,string Key,string Plaintext,int[] Setting)
        {
            Image_Processing = Img;
            this.Key = Key;
            this.Plaintext = Plaintext;
            this.Setting = Setting;
        }

        public Controller_LSB(Bitmap Stego,string Key, decimal NCharacter, int[] Setting)
        {
            this.Stego = Stego;
            this.Key = Key;
            this.Setting = Setting;
            this.NCharacter = (int)NCharacter;
        }

        public int GetLSB(int Value,int Bit)
        {
            int[] temp = Decimal_to_Binary(Value);
            temp[7] = Bit;
            int new_value = Binary_to_Decimal(temp);

            return new_value;
        }

        public int GetLSB2(int value)
        {
            int temp = 0;
            if(value %2 == 0)
            {
                temp = 0 ;
            }
            else
            {
                temp = 1;
            }
            return temp;
        }

        public Bitmap Encode(RichTextBox Ciphertext,PictureBox Stego_Image,Button SaveStego)
        {
            Bitmap new_image = new Bitmap(Image_Processing); ;
            if (Key != "")
            {
                //1. Encryption Part
                int[] Plain_index = Convert_String_to_IntArray(Plaintext);
                //Console.WriteLine("===Plaintext in ASCII===");
                //Printarray(Plain_index);

                int[] Key_index = Convert_String_to_IntArray(Key);
                //Console.WriteLine("\n===Key in ASCII===");
                //Printarray(Key_index);

                int[] Key_Stream = Get_KeyStream(Plain_index.Length, Key_index);
                int IV = Setting[3];
                int Shift_Bit = Setting[2];
                int[] Temp = Encryption(Plain_index, Key_Stream, Setting[3],Shift_Bit);
                //Console.WriteLine("\n===Ciphertext in ASCII===");
                //Printarray(Temp);
                Ciphertext.Text = Convert_IntArray_to_String(Temp);


                //2. Pixel Random Part
                int Width = Image_Processing.Width;
                int Height = Image_Processing.Height;
                int A = 1;
                int C = Setting[0];
                int M = Width * Height;
                int Seed = Setting[1];
                int Piksel_needed =  Temp.Length * 2;

                Controller_LCG LCG = new Controller_LCG(Width,Height);
                int[,] Pixel_Random = LCG.Get_Pixel_Random(A, C, M, Seed, Piksel_needed);

                //PrintArray2d(Pixel_Random);
                //
                
                //3. Part Encoding
                int Counter_Text = 0;
                int Counter_Row1 = 0;
                int Counter_Row2 = 1;
                int x1, y1, x2, y2;
                int r1, g1, b1, a1, r2, g2, b2, a2;
                

                while(Counter_Row1 < Piksel_needed)
                {
                    x1 = Pixel_Random[Counter_Row1, 0];
                    y1 = Pixel_Random[Counter_Row1, 1];
                    x2 = Pixel_Random[Counter_Row2, 0];
                    y2 = Pixel_Random[Counter_Row2, 1];

                    Color p = this.Image_Processing.GetPixel(x1, y1);
                    Color p2 = this.Image_Processing.GetPixel(x2, y2);

                    a1 = p.A;
                    r1 = p.R;
                    g1 = p.G;
                    b1 = p.B;

                    a2 = p2.A;
                    r2 = p2.R;
                    g2 = p2.G;
                    b2 = p2.B;

                    //Console.WriteLine("Pixel1 = " + a1 + "," + r1 + "," + g1 + "," + b1);
                    //Console.WriteLine("Pixel1 = " + a2 + "," + r2 + "," + g2 + "," + b2);
                    int[] bytes = Decimal_to_Binary(Temp[Counter_Text]);
                    int bit0 = bytes[0];
                    int bit1 = bytes[1];
                    int bit2 = bytes[2];
                    int bit3 = bytes[3];
                    int bit4 = bytes[4];
                    int bit5 = bytes[5];
                    int bit6 = bytes[6];
                    int bit7 = bytes[7];
                    //Console.WriteLine("Index-" + Counter_Text + " = " + bit0 + "," + bit1 + "," + bit2 + "," + bit3 + "," + bit4 + "," + bit5 + "," + bit6 +","+bit7);

                    a1 = GetLSB(a1, bit0);
                    r1 = GetLSB(r1, bit1);
                    g1 = GetLSB(g1, bit2);
                    b1 = GetLSB(b1, bit3);
                    a2 = GetLSB(a2, bit4);
                    r2 = GetLSB(r2, bit5);
                    g2 = GetLSB(g2, bit6);
                    b2 = GetLSB(b2, bit7);

                    //Console.WriteLine("New Image = " + a1 + "," + r1 + "," + g1 + "," + b1+ ", "+ a2 + "," + r2 + "," + g2 + "," + b2);
                    new_image.SetPixel(x1, y1, Color.FromArgb(a1, r1, g1, b1));
                    new_image.SetPixel(x2, y2, Color.FromArgb(a2, r2, g2, b2));

                    //Color p3 = new_image.GetPixel(x1, y1);
                    //Color p4 = new_image.GetPixel(x2, y2);
                    //Console.WriteLine("New Image = " + p3.A + "," + p3.R + "," + p3.G + "," + p3.B + ", " + p4.A + "," + p4.R + "," + p4.G + "," + p4.B);
                    Counter_Text += 1;
                    Counter_Row1 += 2;
                    Counter_Row2 += 2;
                }

                Stego_Image.Image = new_image; 
                SaveStego.Enabled = true;
                MessageBox.Show("Done", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SaveStego.Enabled = false;

                MessageBox.Show("Key Is Empty", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return new_image;
        }

        public void Decode(RichTextBox Ciphertext,RichTextBox Plaintext,Button SaveText)
        {
            if (Key != "")
            {
                //1. Get Pixel Random First
                int Width = Stego.Width;
                int Height = Stego.Height;
                int A = 1;
                int C = Setting[0];
                int M = Width * Height;
                int Seed = Setting[1];
                int Piksel_needed = NCharacter*2;

                Controller_LCG LCG = new Controller_LCG(Width, Height);
                int[,] Pixel_Random = LCG.Get_Pixel_Random(A, C, M, Seed, Piksel_needed);

                //2. Part Decode
                int Counter_Text = 0;
                int Counter_Row1 = 0;
                int Counter_Row2 = 1;
                int x1, y1, x2, y2;
                int r1, g1, b1, a1, r2, g2, b2, a2;
                int[] Plain_Index = new int[NCharacter];

                while (Counter_Row1 < Piksel_needed)
                {
                    x1 = Pixel_Random[Counter_Row1, 0];
                    y1 = Pixel_Random[Counter_Row1, 1];
                    x2 = Pixel_Random[Counter_Row2, 0];
                    y2 = Pixel_Random[Counter_Row2, 1];

                    Color p = this.Stego.GetPixel(x1, y1);
                    Color p2 = this.Stego.GetPixel(x2, y2);

                    a1 = p.A;
                    r1 = p.R;
                    g1 = p.G;
                    b1 = p.B;

                    a2 = p2.A;
                    r2 = p2.R;
                    g2 = p2.G;
                    b2 = p2.B;

                    //Console.WriteLine("Pixel1 = " + a1 + "," + r1 + "," + g1 + "," + b1);
                    //Console.WriteLine("Pixel2 = " + a2 + "," + r2 + "," + g2 + "," + b2);

                    int[] bytes = new int[8];
                    bytes[0] = GetLSB2(a1);
                    bytes[1] = GetLSB2(r1);
                    bytes[2] = GetLSB2(g1);
                    bytes[3] = GetLSB2(b1);
                    bytes[4] = GetLSB2(a2);
                    bytes[5] = GetLSB2(r2);
                    bytes[6] = GetLSB2(g2);
                    bytes[7] = GetLSB2(b2);

                    Plain_Index[Counter_Text] = Binary_to_Decimal(bytes);
                   // Console.WriteLine("Index-" + Counter_Text + " = " + Binary_to_Decimal(bytes));

                    Counter_Text += 1;
                    Counter_Row1 += 2;
                    Counter_Row2 += 2;
                }

                Ciphertext.Text = Convert_IntArray_to_String(Plain_Index);
                int[] Key_Index = Convert_String_to_IntArray(Key);
                int IV = Setting[3];
                int shift_bit = Setting[2];
                int[] Key_Stream = Get_KeyStream(Plain_Index.Length, Key_Index);
                int[] Temp2 = Decryption(Plain_Index, Key_Stream,IV,shift_bit);
                string plain = Convert_IntArray_to_String(Temp2);
                Plaintext.Text = plain;
                SaveText.Enabled = true;
                MessageBox.Show("Done", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SaveText.Enabled = false;

                MessageBox.Show("Key Is Empty", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


    }
}
