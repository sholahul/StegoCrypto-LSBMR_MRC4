using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace StegoCrypto
{
    public partial class Form_LSBMR_MRC4 : Form
    {
        Bitmap C_Image, S_Image;
        Controller_Utility Utility = new Controller_Utility();
        int[] Setting = new int[5];


        public Form_LSBMR_MRC4()
        {
            InitializeComponent();
            Setting[0] = (int)C.Value;
            Setting[1] = (int)Seed.Value;
            Setting[2] = (int)Shift_Bit.Value;
            Setting[3] = (int)IV.Value;
        }

        private void Enable_Button(Bitmap Img)
        {
            if (Cover_Image.Image != null && Plaintext.Text != "")
            {
                Button_Encode.Enabled = true;
            }
            else
            {
                Button_Encode.Enabled = false;
            }

            if (Img != null)
            {
                B_LoadText.Enabled = true;
            }
            else
            {
                B_LoadText.Enabled = false;
                Plaintext.Text = "";
            }
        }

        private void B_LoadImage_Click(object sender, EventArgs e)
        {
            Utility.Load_Citra(Cover_Image);
            Utility.GetDetailImage(Max_Character, Width_Cover, Height_Cover);
            C_Image = Utility.GetImage();
            Enable_Button(C_Image);
        }

        private void B_LoadText_Click(object sender, EventArgs e)
        {
            Utility.Load_Text(Plaintext, NCharacter);
            Enable_Button(C_Image);
        }

        private void Reset()
        {
            B_LoadImage.Enabled = true;
            B_LoadText.Enabled = false;
            Plaintext.Text = "";
            Cover_Image.Image = null;
            Ciphertext.Text = "";
            Button_Encode.Enabled = false;
            Button_Save.Enabled = false;
            Stego_Image.Image = null;
            Key.Text = "";
        }

        private void Button_Reset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Button_Encode_Click(object sender, EventArgs e)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Controller_LSBMR LSBMR = new Controller_LSBMR(C_Image, Key.Text, Plaintext.Text, Setting);
            S_Image = LSBMR.Encode(Ciphertext, Stego_Image, Button_Save);
            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString()); 
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            Utility.Save_Image(S_Image);
        }

        private void Home_button_Click(object sender, EventArgs e)
        {
            Form_Menu Menu = new Form_Menu();
            this.Hide();
            Menu.ShowDialog();
            this.Close();
        }

        private void B_LoadImage2_Click(object sender, EventArgs e)
        {
            Utility.Load_Stego(Stego_Image1, Button_Decode);
            Utility.GetDetailImage(Max_Character2, Width_Stego, Height_Stego);
        }

        private void Button_Decode_Click(object sender, EventArgs e)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            S_Image = Utility.GetStego();
            Controller_LSBMR LSBMR = new Controller_LSBMR(S_Image, Key2.Text, NCharacter2.Value, Setting);
            LSBMR.Decode(Ciphertext2, Plaintext2, Button_Save2);
            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }

        private void Button_Save2_Click(object sender, EventArgs e)
        {
            Utility.Save_Text(Plaintext2);
        }

        private void Reset2()
        {
            Key.Text = "";
            Key.Enabled = false;
            Button_Decode.Enabled = false;
            Button_Save2.Enabled = false;
            Ciphertext2.Text = "";
            Plaintext2.Text = "";
            Stego_Image1.Image = null;
            NCharacter2.Text = "";
            Key2.Text = "";
        }

        private void Button_Reset2_Click(object sender, EventArgs e)
        {
            Reset2();
        }

        private void Enable_Button_Process()
        {
            if (Stego_Image2.Image != null && Cover_Image2.Image != null)
            {
                Button_Process2.Enabled = true;
            }
            else
            {
                Button_Process2.Enabled = false;
            }
        }

        private void Browse_Image_Click(object sender, EventArgs e)
        {
            Utility.Load_Citra2(Cover_Image2);
            C_Image = Utility.GetImage();
            Enable_Button_Process();
        }

        private void Browse_Stego_Click(object sender, EventArgs e)
        {
            Utility.Load_Stego(Stego_Image2);
            S_Image = Utility.GetStego();
            Enable_Button_Process();
        }

        private void Show_PSNR()
        {
            Controller_PSNR MP = new Controller_PSNR();
            MP.setMSE_PSNR(C_Image, S_Image);
            MSE.Text = MP.getMSE().ToString();
            PSNR.Text = MP.getPSNR().ToString();
            Amount_Bit.Text = MP.getAmount_Bits().ToString() + " Bit";
            Bit_Changes.Text = MP.getBit_Changes().ToString() + " Bit";
            Average_Changes.Text = MP.getAverage_Changes().ToString() + " %";
            Bit_Increase.Text = MP.getBit_Increase().ToString() + " Bit";
            Bit_Decrease.Text = MP.getBit_Decrease().ToString() + " Bit";
        }

        private void Button_Process2_Click(object sender, EventArgs e)
        {
            Show_PSNR();
        }

        private void Reset3()
        {
            Cover_Image2.Image = null;
            Stego_Image2.Image = null;
            Button_Process.Enabled = false;
            PSNR.Text = "0";
            MSE.Text = "0";
            Amount_Bit.Text = "0";
            Average_Changes.Text = "0";
            Bit_Changes.Text = "0";
            Bit_Increase.Text = "0";
            Bit_Decrease.Text = "0";
        }

        private void Edit()
        {
            Shift_Bit.Enabled = true;
            C.Enabled = true;
            Seed.Enabled = true;
            IV.Enabled = true;
            Button_Edit.Visible = false;
            Button_Done.Visible = true;
        }

        private void Button_Edit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void Done()
        {
            Setting[0] = (int)C.Value;
            Setting[1] = (int)Seed.Value;
            Setting[2] = (int)Shift_Bit.Value;
            Setting[3] = (int)IV.Value;

            if (Setting[0] % 2 != 0)
            {
                Shift_Bit.Enabled = false;
                C.Enabled = false;
                Seed.Enabled = false;
                IV.Enabled = false;
                Button_Edit.Visible = true;
                Button_Done.Visible = false;
                MessageBox.Show("Data Saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("C value can't be even", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Button_Done_Click(object sender, EventArgs e)
        {
            Done();
        }

        private void Button_LoadText_Click(object sender, EventArgs e)
        {
            Utility.Load_Text2(First_Message, NText1);
        }

        private void Button_LoadText2_Click(object sender, EventArgs e)
        {
            Utility.Load_Text2(Final_Message, NText2);
        }

        private void Button_Process_Click(object sender, EventArgs e)
        {
            Utility.Similarity(First_Message.Text, Final_Message.Text, Message_Similarity);
        }

        private void Reset4()
        {
            Final_Message.Text = "";
            First_Message.Text = "";
        }

        private void Button_Reset4_Click(object sender, EventArgs e)
        {
            Reset4();
        }

        private void Button_Reset3_Click(object sender, EventArgs e)
        {
            Reset3();
            Enable_Button_Process();
        }
    }
}
