using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 彈珠台
{
    public partial class Form2 : Form
    {
        Form1 ownerForm01;
        public Form2(Form1 ownerForm)
        {
            InitializeComponent();
            this.ownerForm01 = ownerForm;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            money();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) //ApronLength
        {
            ownerForm01.TotalGameScore -= ownerForm01.ApronLengthPrice;
            ownerForm01.ApronLengthPrice = (int)(1.5 * ownerForm01.ApronLengthPrice);
            ownerForm01.ApronLengthLevel++;
            ownerForm01.PlayerBar_Length+= 30;
            money();
        }

        private void button3_Click(object sender, EventArgs e) //BallSpeed
        {
            ownerForm01.TotalGameScore -= ownerForm01.BallSpeedPrice;
            ownerForm01.BallSpeedPrice = (int)(1.1 * ownerForm01.BallSpeedPrice);
            ownerForm01.BallSpeedLevel++;
            ownerForm01.Ball_ValTotal++;
            money();
        }

        private void button4_Click(object sender, EventArgs e) //PointBooster
        {
            ownerForm01.TotalGameScore -= ownerForm01.PointBoosterPrice;
            ownerForm01.PointBoosterPrice = (int)(1.25f * ownerForm01.PointBoosterPrice);
            ownerForm01.PointBoosterLevel++;
            ownerForm01.ShopBuff += 0.75f;
            money();
        }
        void money()
        {
            if (ownerForm01.TotalGameScore - ownerForm01.ApronLengthPrice < 0)
            {
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
            }
            if (ownerForm01.TotalGameScore - ownerForm01.BallSpeedPrice < 0)
            {
                button3.Enabled = false;
            }
            else
            {
                button3.Enabled = true;
            }
            if (ownerForm01.TotalGameScore - ownerForm01.PointBoosterPrice < 0)
            {
                button4.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
            }
            label1.Text = "YourTotalScore = " + ownerForm01.TotalGameScore;
            label2.Text = "lv." + ownerForm01.ApronLengthLevel + "ApronLength";
            label3.Text = "lv." + ownerForm01.BallSpeedLevel + "BallSpeed";
            label4.Text = "lv." + ownerForm01.PointBoosterLevel + "PointBooster";
            label5.Text = "Upgrade";
            button1.Text = "Exit";
            button2.Text = "$" + ownerForm01.ApronLengthPrice;
            button3.Text = "$" + ownerForm01.BallSpeedPrice;
            button4.Text = "$" + ownerForm01.PointBoosterPrice;
        }
    }
}
