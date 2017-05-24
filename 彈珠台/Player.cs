using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace 彈珠台
{
    class Player
    {
        public int Bar_Length;
        int Bar_Margin;
        public Rectangle RectangleBar;
        int ClientSizeW;
        int ClientSizeH;

        public Player(
            int Bar_Length,
            int Bar_Margin,
            int ClientSizeW,
            int ClientSizeH
        )
        {
            this.Bar_Length=Bar_Length;
            this.Bar_Margin=Bar_Margin;
            this.ClientSizeW = ClientSizeW;
            this.ClientSizeH = ClientSizeH;
        }
        public void Draw(Graphics G)
        {
            G.FillRectangle(Brushes.Blue, RectangleBar);
        }
        public void Update(int MouseLocationX)
        {
            RectangleBar = new Rectangle(MouseLocationX - Bar_Length/2 , ClientSizeH - Bar_Margin - 5, Bar_Length,20);

            if (RectangleBar.Right >= ClientSizeW)
            {
                RectangleBar.X = ClientSizeW - Bar_Length;
            }
            else if (RectangleBar.Left <= 0)
            {
                RectangleBar.X = 0;
            }
        }
    }
}
