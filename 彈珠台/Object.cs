using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace 彈珠台
{
    class Object
    {
        public Rectangle Rectangle;
        Point Rectangle_Pos;
        int Rectangle_Size;
        Point Rectangle_Val;
        Color Rectangle_Color;
        Brush Rectangle_Brush;
        int ClientSizeW;
        int ClientSizeH;

        public Object( 
            Point Rectangle_Pos,
            int Rectangle_Size,
            Point Rectangle_Val,
            int ClientSizeW,
            int ClientSizeH)
        {
            this.Rectangle_Pos = Rectangle_Pos;
            this.Rectangle_Size = Rectangle_Size;
            this.Rectangle_Val = Rectangle_Val;

            Rectangle_Color = Color.Red;
            Rectangle_Brush = Brushes.Red;

            this.ClientSizeW = ClientSizeW;
            this.ClientSizeH = ClientSizeH;
        }

        public void Draw(Graphics G)
        {
                G.FillRectangle(Rectangle_Brush, Rectangle);
        }

        public void Update()
        {
            Rectangle = new Rectangle(Rectangle_Pos.X, Rectangle_Pos.Y, Rectangle_Size, Rectangle_Size / 4);
            
            if (Rectangle_Pos.X + Rectangle_Size / 2 + Rectangle_Val.X > ClientSizeW || (Rectangle_Pos.X + Rectangle_Size / 2 + Rectangle_Val.X < 0))
            {
                Rectangle_Val.X *= -1;
            }
            else
            {
                Rectangle_Pos.X += Rectangle_Val.X;
            }
             
        }
    }
}
