using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace 彈珠台
{
    class BallClass
    {
        public Rectangle BallRectangle;
        public Point Ball_Pos; //球的當前坐標
        public Point Ball_Val; //球的動量
        public bool GameOver = false;
        int Ball_D; //球的半徑
        Brush Ball_Color;
        int ClientSizeW;
        int ClientSizeH;
        
        public BallClass( 
            Point Ball_Pos,
            Point Ball_Val,
            int Ball_D,
            Brush Ball_Color,
            int ClientSizeW,
            int ClientSizeH
            )
        {
            this.Ball_D = Ball_D;
            this.Ball_Pos = Ball_Pos;
            this.Ball_Val = Ball_Val;
            this.Ball_Color = Ball_Color;
            this.ClientSizeW = ClientSizeW;
            this.ClientSizeH = ClientSizeH;
        }

        public void Draw(Graphics G)
        {
            G.FillEllipse(Ball_Color, BallRectangle);
        }
        public void Update()
        {

            BallRectangle = new Rectangle(Ball_Pos.X - Ball_D, Ball_Pos.Y - Ball_D, Ball_D * 2, Ball_D * 2);

            BallContol();
        }
         void BallContol()
        {
            if (Ball_Pos.X + Ball_D / 2 + Ball_Val.X > ClientSizeW)//球的碰撞
            {
                Ball_Pos.X = ClientSizeW - Ball_D;
                Ball_Val.X *= -1;
            }
            else if (Ball_Pos.X - Ball_D / 2 + Ball_Val.X < 0)
            {
                Ball_Pos.X = Ball_D;
                Ball_Val.X *= -1;
            }
            else
            {
                Ball_Pos.X += Ball_Val.X;
            }
            if (Ball_Pos.Y + Ball_D / 2 + Ball_Val.Y > ClientSizeH)//球的碰撞
            {
                GameOver = true; //碰到下方就輸掉
            }
            else if (Ball_Pos.Y - Ball_D / 2 + Ball_Val.Y < 0)
            {
                Ball_Pos.Y = Ball_D;
                Ball_Val.Y *= -1;
            }
            else
            {
                Ball_Pos.Y += Ball_Val.Y;
            }
        }
    }
}
