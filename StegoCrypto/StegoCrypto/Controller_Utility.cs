using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StegoCrypto
{
    class Controller_Utility
    {
        private OpenFileDialog ofd = new OpenFileDialog();
        
        Bitmap Cover_Image, Image_Processing,Stego_Image;
        string Plaintext;
        int Max_Char,Height_Img,Width_Img;

        int Sharp(int value)
        {
            int a;
            if (value == 255)
            {
                a = value - 1;
            }
            else if (value == 0)
            {
                a = value + 1;
            }
            else
            {
                a = value;
            }


            return a;
        }

        private void Normalization(PictureBox Img)
        {
            int i,j,a,r,g,b;
            Image_Processing = new Bitmap(Cover_Image);

            for (i = 0; i < Image_Processing.Height; i++)
            {
                for (j = 0; j < Image_Processing.Width; j++)
                {
                    Color p = Cover_Image.GetPixel(i, j);
                    a = p.A;
                    r = p.R;
                    g = p.G;
                    b = p.B;

                    a = Sharp(a);
                    r = Sharp(r);
                    g = Sharp(g);
                    b = Sharp(b);
                    Image_Processing.SetPixel(i, j, Color.FromArgb(a, r, g, b));  
                }
            }
            Cover_Image = Image_Processing;
            Img.Image = Cover_Image;
            Save_Image(Image_Processing);
        }   
        
        public void Load_Citra(PictureBox Img)
        {           
            ofd.Filter = "bmp|*.bmp";
            DialogResult res = ofd.ShowDialog();

            if (res == DialogResult.OK)
            {
                Cover_Image = (Bitmap)Image.FromFile(ofd.FileName);
                Img.Image = Cover_Image;
                Width_Img = this.Cover_Image.Width;
                Height_Img = this.Cover_Image.Height;
                Max_Char = Max_Character_Embeed(Width_Img, Height_Img);
                Normalization(Img);
            }
            else
            {
                Cover_Image = null;
                Img.Image = null;
                Height_Img = 0;
                Width_Img = 0;
                Max_Char = 0;
                MessageBox.Show("No file selected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    
        public void GetDetailImage(Label Max_Char,Label Width_Img,Label Height_Img)
        {
            Max_Char.Text = this.Max_Char.ToString();
            Height_Img.Text = this.Height_Img.ToString();
            Width_Img.Text = this.Width_Img.ToString();

        }

        public void Load_Stego(PictureBox Img, Button B_Enabled)
        {
            Img.Image = null;
            ofd.Filter = "bmp|*.bmp";
            DialogResult res = ofd.ShowDialog();

            if (res == DialogResult.OK)
            {
                Stego_Image = (Bitmap)Image.FromFile(ofd.FileName);

                Img.Image = Stego_Image;

                Width_Img = this.Stego_Image.Width;
                Height_Img = this.Stego_Image.Height;
                Max_Char = Max_Character_Embeed(Width_Img, Height_Img);
                B_Enabled.Enabled = true;
            }
            else
            {
                B_Enabled.Enabled = false;
                MessageBox.Show("No file selected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public Bitmap GetImage()
        {
            return Cover_Image;
        }

        public Bitmap GetStego()
        {
            return Stego_Image;
        }

        public int Max_Character_Embeed(int Width, int Height)
        {
            return ((Width * Height) * 4) / 8;
        }

        public void Load_Text(RichTextBox Text, Label Num_Character)
        {
            Text.Text = "";
            ofd.Filter = "txt|*.txt";
            DialogResult res = ofd.ShowDialog();
            if (res == DialogResult.OK)
            {
                this.Plaintext = File.ReadAllText(ofd.FileName);
                Num_Character.Text = this.Plaintext.Length.ToString();
                int Plaintext_length = this.Plaintext.Length;
                if (Plaintext_length <= Max_Char)
                {
                    Text.Text = this.Plaintext;
                }
                else
                {
                    MessageBox.Show("Character Exceeds Capacity", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
            else
            {
                Num_Character.Text = "0";
                MessageBox.Show("No file selected", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Load_Text2(RichTextBox Text,Label NText)
        {
            Text.Text = "";
            ofd.Filter = "txt|*.txt";
            string txt;

            DialogResult res = ofd.ShowDialog();
            if (res == DialogResult.OK)
            {
                txt = File.ReadAllText(ofd.FileName);
                NText.Text = txt.Length.ToString();
                Text.Text = txt;
            }
            else
            {
                MessageBox.Show("No file selected", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Similarity(string Text1, string Text2, Label Similarity)
        {
            if (Text1 == "")
            {
                MessageBox.Show("Text1 is Empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (Text2 == ""
)
            {
                MessageBox.Show("Text2 is Empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (Text1.Equals(Text2))
                {
                    Similarity.Text = "Similar";
                }
                else
                {
                    Similarity.Text = "Not Similar";
                }
            }
        }

        public void Save_Image(Bitmap Citra)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Bitmap Images (*.bmp)|*.bmp";
            
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                Citra.Save(sfd.FileName);
                MessageBox.Show("Image saved successfully","Success");
            }
        }

        public void Save_Text(RichTextBox Message)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Txt File (*.txt)|*.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Message.SaveFile(sfd.FileName,RichTextBoxStreamType.PlainText);
                MessageBox.Show("Message saved successfully", "Success",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Load_Citra2(PictureBox Img)
        {
            Img.Image = null;
            ofd.Filter = "bmp|*.bmp";
            DialogResult res = ofd.ShowDialog();

            if (res == DialogResult.OK)
            {
                Cover_Image = (Bitmap)Image.FromFile(ofd.FileName);
                Img.Image = Cover_Image;
            }
            else
            {
                MessageBox.Show("No file selected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Load_Stego(PictureBox Img)
        {
            Img.Image = null;
            ofd.Filter = "bmp|*.bmp";
            DialogResult res = ofd.ShowDialog();

            if (res == DialogResult.OK)
            {
                Stego_Image = (Bitmap)Image.FromFile(ofd.FileName);
                Img.Image = Stego_Image;
                Width_Img = this.Cover_Image.Width;
                Height_Img = this.Cover_Image.Height;
                Max_Char = Max_Character_Embeed(Width_Img, Height_Img);
            }
            else
            {
                MessageBox.Show("No file selected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        
    }
}