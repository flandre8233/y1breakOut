using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Media;
using System.Threading;

/* Updata Log
    19/6/2015 彈珠台 ver 1:00
 * 像一個遊戲的樣子
 * 修正一開始球往下跑的問題
 * 分數系統
 * Combo系統
 * 多重球系統
 * 20/6/2015 彈珠台 ver 1:01
 * 新增新的擋板
 * 21/6/2015 彈珠台 ver 1:10
 * 亂數系統
 * 增建重生系統
 * 22/6/2015 彈珠台 ver 1:20
 * 新增商店系統
 * 增加兩人遊玩模式
 * 快速離開遊戲 "P"
 * 23/6/2015 彈珠台 ver 1:30
 * 商店升級能力往上調整
 * 價格往下調整
 * 新增輸入密技
 * 修正分數顯示問題
 * 24/6/2015 彈珠台 ver 1:40
 * 價格往下調整
 * autobot按鈕
*/
namespace 彈珠台
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None; //放大化設定
            this.WindowState = FormWindowState.Maximized; //放大化設定
            this.TopMost = true; //放大化設定
        }
        Object RectObject;
        BallClass CreateBall;
        Player PlayerControl;
        Player PlayerControl2;
        List<Object> RectObjects = new List<Object>() ;
        List<BallClass> CreateBallList = new List<BallClass>();
        List<BallClass> EnemyBallList = new List<BallClass>();
        int Level = 1;
        int ListSize = 10;
        int TotalScore = 0;
        public int TotalGameScore = 0;
        int LastRoundTotalScore = 0;
        int Score = 1;
        int Bonus = 1;
        public float ShopBuff = 1;
        float LevelBuff = 1;
        public int Ball_ValTotal = 10;
        public int PlayerBar_Length = 140;
        int MouseX;
        int ADKey;
        int ADKeyVal;
        Random Ran = new Random(DateTime.Now.Millisecond);
        int RanNum;
        Point RectPos;
        TimeSpan Timer = new TimeSpan();
        float ObjectRotation = 0;
        bool IsStartTheGame = false;
        bool IsCoopMode = false;
        
        public int ApronLengthLevel = 1;
        public int BallSpeedLevel = 1;
        public int PointBoosterLevel = 1;
        public int ApronLengthPrice = 40000;
        public int BallSpeedPrice = 3000;
        public int PointBoosterPrice = 1000;
        int BallPrice = 500;

        private void Form1_Load(object sender, EventArgs e)
        {
            IsStartTheGame = false;
            LabelTextUpdata();
            label1.BackColor = label2.BackColor = label3.BackColor = label4.BackColor = label5.BackColor = label6.BackColor = button1.BackColor = button2.BackColor = button3.BackColor = button4.BackColor = checkBox1.BackColor = Color.Transparent;
            comboBox1.SelectedIndex = 0;
            SpawnRectangle();
            SpawnPlayerBar();
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                Close();
            }

            if (e.KeyCode == Keys.A)
            {
                ADKeyVal=0;
            }
            if (e.KeyCode == Keys.D)
            {
                ADKeyVal = 0;
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && ADKeyVal > -12)
            {
                ADKeyVal -=6;
            }
            if (e.KeyCode == Keys.D && ADKeyVal < +12)
            {
                ADKeyVal +=6;
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (IsStartTheGame == true)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                for (int i = 0; i < RectObjects.Count; i++)
                {
                    RectObjects[i].Draw(e.Graphics);
                }
                for (int i = 0; i < CreateBallList.Count; i++)
                {
                    CreateBallList[i].Draw(e.Graphics);
                }
                for (int i = 0; i < EnemyBallList.Count; i++)
                {
                    EnemyBallList[i].Draw(e.Graphics);
                }
                PlayerControl.Draw(e.Graphics);
                PlayerControl2.Draw(e.Graphics);
                if (Level > 5)
                {
                    e.Graphics.RotateTransform(ObjectRotation - 90);
                    e.Graphics.FillRectangle(Brushes.DarkGray, this.ClientSize.Height / 2, this.ClientSize.Width / 2, 300, 300);
                    e.Graphics.RotateTransform(0);
                    ObjectRotation += 0.01f;
                }
            }
        }
        private void BackgoundTimer_Tick(object sender, EventArgs e)
        {
            PlayerControlSetting();
            if (IsStartTheGame != true)
            {
                Gameover();
            }
            else
            {
                PlayerControl.Update(MouseX);
                if (IsCoopMode == false)
                {
                    PlayerControl2.Update(this.ClientSize.Width - MouseX);
                }
                else
                {
                    PlayerControl2.Update(ADKey);
                }
                for (int i = 0; i < RectObjects.Count; i++)
                {
                    RectObjects[i].Update();
                }
                for (int i = 0; i < CreateBallList.Count; i++)
                {
                    CreateBallList[i].Update();
                    for (int j = 0; j < RectObjects.Count; j++)
                    {

                        if (RectObjects[j].Rectangle.Contains(CreateBallList[i].Ball_Pos)) //球碰撞其他板
                        {
                            RectObjects.RemoveAt(j);
                            Bonus++;
                            TotalScore += (int)(Score * Bonus * CreateBallList.Count * ShopBuff * LevelBuff);
                            TotalGameScore = LastRoundTotalScore + TotalScore;
                            LabelTextUpdata();
                            CreateBallList[i].Ball_Val = ValRandom(Ball_ValTotal);
                            RanNum = Ran.Next(1, 3);
                            if (RanNum == 1)
                                CreateBallList[i].Ball_Val.X *= -1;
                            else if (RanNum == 2)
                                CreateBallList[i].Ball_Val.Y *= -1;
                            else
                            {
                                CreateBallList[i].Ball_Val.X *= -1;
                                CreateBallList[i].Ball_Val.Y *= -1;
                            }
                        }
                    }
                    if (CreateBallList[i].GameOver == true || IsStartTheGame != true) //碰到下方就輸掉
                    {
                        Gameover();
                        MessageBox.Show("EndGame");
                    }
                    if (PlayerControl.RectangleBar.Contains(new Point(CreateBallList[i].Ball_Pos.X, CreateBallList[i].Ball_Pos.Y + CreateBall.BallRectangle.Height / 2)) || PlayerControl2.RectangleBar.Contains(new Point(CreateBallList[i].Ball_Pos.X, CreateBallList[i].Ball_Pos.Y + CreateBall.BallRectangle.Height / 2)))//玩家板碰撞
                    {
                        Bonus = 1; //分數獎勵清空
                        label3.Text = Bonus * CreateBallList.Count * ShopBuff * LevelBuff + "X Times";
                        CreateBallList[i].Ball_Val.Y *= -1;
                        if (PlayerControl.RectangleBar.Contains(new Point(CreateBallList[i].Ball_Pos.X, CreateBallList[i].Ball_Pos.Y + CreateBall.BallRectangle.Height / 2)))
                            CreateBallList[i].Ball_Pos.Y = PlayerControl.RectangleBar.Top - CreateBall.BallRectangle.Height;
                        else if (PlayerControl2.RectangleBar.Contains(new Point(CreateBallList[i].Ball_Pos.X, CreateBallList[i].Ball_Pos.Y + CreateBall.BallRectangle.Height / 2)))
                            CreateBallList[i].Ball_Pos.Y = PlayerControl2.RectangleBar.Top - CreateBall.BallRectangle.Height;
                    }
                }
                for (int i = 0; i < EnemyBallList.Count; i++)
                {
                    EnemyBallList[i].Update();
                    if (PlayerControl.RectangleBar.Contains(EnemyBallList[i].Ball_Pos) || PlayerControl2.RectangleBar.Contains(EnemyBallList[i].Ball_Pos)) //玩家板碰撞
                    {
                        Gameover();
                        MessageBox.Show("EndGame");
                    }
                    for (int j = 0; j < CreateBallList.Count; j++)
                    {
                        if (EnemyBallList[i].BallRectangle.Contains(CreateBallList[j].Ball_Pos))
                        {
                            //EnemyBallList.RemoveAt(i);
                        }
                    }
                    if (EnemyBallList[i].GameOver == true) //
                    {
                        EnemyBallList.RemoveAt(i);
                    }
                }
                AutoBot();
                NextLevel();
                this.Invalidate();
            }
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            Timer = Timer.Add(new TimeSpan(00, 00, 01));
            label2.Text = Timer.Minutes.ToString() + ":" + Timer.Seconds.ToString();

            if (Level > 1)
            {
                
                if (Timer.Seconds % 5 == 0)
                {
                    CreateBall = new BallClass(
                    new Point(Ran.Next(25, this.ClientSize.Width), 0 + 10),
                    ValRandom((int)(Ball_ValTotal*0.25f)),
                    10,
                    Brushes.DarkRed,
                    this.ClientSize.Width,
                    this.ClientSize.Height
                    );
                    EnemyBallList.Add(CreateBall);
                }
                 
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseX = e.Location.X;
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (BackgoundTimer.Enabled == true)
            {
                GameTimer.Enabled = true;
                if (CreateBallList.Count == 0)
                {
                    CreateBall = new BallClass(
                        new Point(PlayerControl.RectangleBar.X+PlayerControl.Bar_Length/2,PlayerControl.RectangleBar.Top),
                        new Point(-0, -Ball_ValTotal),
                        10,
                        Brushes.DarkBlue,
                        this.ClientSize.Width,
                        this.ClientSize.Height
                        );
                    CreateBallList.Add(CreateBall);
                }
                else if (CreateBallList.Count != 0 && TotalScore - BallPrice > 0)
                {
                    CreateBall = new BallClass(
                        new Point(PlayerControl.RectangleBar.X+PlayerControl.Bar_Length/2,PlayerControl.RectangleBar.Top),
                        new Point(-0, -Ball_ValTotal),
                        10,
                        Brushes.DarkBlue,
                        this.ClientSize.Width,
                        this.ClientSize.Height
                        );
                    CreateBallList.Add(CreateBall);
                    TotalScore -= BallPrice;
                }
            }
            TotalGameScore = LastRoundTotalScore + TotalScore;
            LabelTextUpdata();
        }
        Point ValRandom(int ValNumber)
        {
            Random Ran1 = new Random(DateTime.Now.Millisecond);
            Random Ran2 = new Random(DateTime.Now.Second);
            int RanNumber1 ;
            int RanNumber2 ;
            
            RanNumber1 = Ran1.Next(-ValNumber,ValNumber);
            RanNumber2 = Ran1.Next(-ValNumber,ValNumber);

            while (RanNumber1 + RanNumber2 != ValNumber)
            {
                RanNumber1 = Ran1.Next(-ValNumber, ValNumber);
                RanNumber2 = Ran2.Next(-ValNumber, ValNumber);
            }
            Point Val = new Point(-RanNumber1, -RanNumber2);
            return Val;
        }
        void AutoBot() //自動接球
        {
            if (checkBox1.Checked == true)
            {
                bool notisbiggest = false;
                for (int i = 0; i < CreateBallList.Count; i++)
                {
                    notisbiggest = false;
                    for (int j = 0; j < CreateBallList.Count; j++)
                    {
                        if (CreateBallList[i].Ball_Pos.Y < CreateBallList[j].Ball_Pos.Y)
                            notisbiggest = true;
                    }
                    if (notisbiggest == false)
                    {
                        MouseX = CreateBallList[i].Ball_Pos.X;
                    }
                }
            }
        }
        
        void NextLevel()//重生
        {
            if (RectObjects.Count < 10)
            {
                RectObjects.Clear();
                SpawnRectangle();
                ListSize++;
                Level++;
                LevelBuff *= 2.5f;
                label6.Text = "Level = " + Level;
            }
        }
        void Gameover()
        {
            IsStartTheGame = false;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow; //還原
            this.WindowState = FormWindowState.Normal; //還原
            this.TopMost = false; //還原
            BackgoundTimer.Enabled = false;
            GameTimer.Enabled = false;
            this.Invalidate();
            button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = comboBox1.Enabled = textBox1.Enabled = checkBox1.Enabled = true;
            button1.Visible = button2.Visible = button3.Visible = button4.Visible = comboBox1.Visible = textBox1.Visible = checkBox1.Visible = true;
            BackgoundTimer.Enabled = false;
            GameTimer.Enabled = false;
        }
        void SpawnRectangle()
        {
            for (int i = 0; i < ListSize ; i++)//Y
            {
                for (int j = 0; j <= 15; j++)//X
                {
                    RectPos = new Point(80 * j + 10, 20 * i + 10);
                    RectObject = new Object(
                        RectPos,
                        60,
                        new Point(5, 5),
                        this.ClientSize.Width,
                        this.ClientSize.Height);

                    RectObjects.Add(RectObject);
                }
            }
        }
        void SpawnPlayerBar()
        {
            PlayerControl = new Player(
                PlayerBar_Length,
                60,
                this.ClientSize.Width,
                this.ClientSize.Height
                );
            PlayerControl2 = new Player(
                PlayerBar_Length * 2,
                40,
                this.ClientSize.Width,
                this.ClientSize.Height
                );
            ADKey = this.ClientSize.Width / 2;
        }
        void PlayerControlSetting()
        {
            if (IsCoopMode != true)
            {
                PlayerControl.RectangleBar.X = MouseX;
                PlayerControl2.RectangleBar.X = this.ClientSize.Width - MouseX;
            }
            else
            {
                ADKey += ADKeyVal;

                if (ADKey < 0)
                {
                    ADKeyVal = 0;
                    ADKey = 0;
                    PlayerControl2.RectangleBar.X = 0;
                }
                else if (ADKey > this.ClientSize.Width - PlayerControl2.Bar_Length/2)
                {
                    ADKeyVal = 0;
                    ADKey = this.ClientSize.Width - PlayerControl2.Bar_Length/2;
                    PlayerControl2.RectangleBar.X = this.ClientSize.Width - PlayerControl2.Bar_Length/2;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e) //restart
        {
            if (comboBox1.SelectedItem == null || comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Select Your Player Number");
            }
            else
            {
                IsStartTheGame = true;
            }

            this.FormBorderStyle = FormBorderStyle.None; //放大化設定
            this.WindowState = FormWindowState.Maximized; //放大化設定
            this.TopMost = true; //放大化設定
            BackgoundTimer.Enabled = true;
            GameTimer.Enabled = true;

            Timer = new TimeSpan();
            GameTimer.Enabled = false;
            CreateBallList.Clear();
            RectObjects.Clear();
            EnemyBallList.Clear();
            SpawnRectangle();
            SpawnPlayerBar();

            button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = textBox1.Enabled = comboBox1.Enabled = checkBox1.Enabled = false;
            button1.Visible = button2.Visible = button3.Visible = button4.Visible = textBox1.Visible = comboBox1.Visible = checkBox1.Visible = false;
            
            LastRoundTotalScore = TotalGameScore;
            TotalScore = 0;
            Score = 1;
            Bonus = 1;
            Level = 1;
            LevelBuff = 1;
            ListSize = 10;
            LabelTextUpdata();
        }
        private void button2_Click(object sender, EventArgs e) //shop
        {
            Form2 GoToShopForm = new Form2(this);
            GoToShopForm.Show();
        }
        private void button3_Click(object sender, EventArgs e) //exit
        {
            MessageBox.Show("YourTotalGameScore ： " + TotalGameScore);
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        IsCoopMode = false;
                        break;
                    case 2:
                        IsCoopMode = true;
                        break;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "nana")
            {
                TotalGameScore += 1000000000;
                textBox1.SelectedText = "";
            }
            else
            {
                textBox1.SelectedText = "";
            }
            
        }

        void LabelTextUpdata()
        {
            label1.Text = "Score = " + TotalScore;
            label2.Text = Timer.Minutes.ToString() + ":" + Timer.Seconds.ToString();
            label3.Text = Bonus * CreateBallList.Count * ShopBuff * LevelBuff + "X Times";
            label4.Text = "0Ball";
            label5.Text = "YourTotalPoint = " + TotalGameScore;
            label6.Text = "Level = " + Level;
            button1.Text = "START";
            button2.Text = "SHOP";
            button3.Text = "GIVE UP";
            button4.Text = "DontClickMe";
            checkBox1.Text = "Open AutoBot";
        }

    }
}
