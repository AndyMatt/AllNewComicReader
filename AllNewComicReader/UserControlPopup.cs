using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using TextDesignerCSLibrary;

namespace AllNewComicReader
{
    public class UserControlPopup
    {
        public static int Opacity = 0;
        public static string Text;
        public static bool isloading = false;

        public static void Update()
        {

            if(Opacity > -1)
            Opacity-= 9;
        }

        public static void clear()
        {
            Opacity = 0;
        }

        public static void Setup(string text)
        {
            Text = text;
            Opacity = 800;
        }

        public static void Loading()
        {
            isloading = true;
        }

        public static void Loaded()
        {
            isloading = false;
        }


        public static void Paint(object sender, PaintEventArgs e, Size ClientSize)
        {
            if (!isloading && Opacity < 1)
                return;

            if (Text == null)
                return;

            FontFamily fontFamily = new FontFamily("Arial Black");
            OutlineText text = new OutlineText();
            StringFormat strformat = new StringFormat();

            SolidBrush blackbrush = new SolidBrush(Color.FromArgb(155, 0, 0, 0));
            Rectangle bgRect = new Rectangle(-5, -5, Screen.PrimaryScreen.Bounds.Width+5, Screen.PrimaryScreen.Bounds.Height+5);

            float referX = 0.0f;
            float referY = 0.0f;
            float referWidth = 0.0f;
            float referHeight = 0.0f;

            text.TextOutline(Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0), 5);
            bool yes = text.MeasureString(e.Graphics, fontFamily, FontStyle.Bold, 17, Text, new Rectangle(0, 0, 0, 0), strformat, ref referX, ref referY, ref referWidth, ref referHeight);


            if (isloading)
            {
                e.Graphics.FillRectangle(blackbrush, bgRect);
                text.DrawString(e.Graphics, fontFamily, FontStyle.Bold, 17, "Loading...", new Point((ClientSize.Width - (int)referWidth) / 2, (ClientSize.Height - (int)referHeight) / 2), strformat);
            }

            if (Opacity < 1)
                return;

            int alpha = Opacity;
            if (alpha > 255)
                alpha = 255;

            
            

//            SizeF stringsize = e.Graphics.MeasureString("StringFormat strformat = new StringFormat();", font, Int32.MaxValue, strformat);
            

            

            Point Origin = new Point(10, 10);

            text.TextOutline(Color.FromArgb(alpha, 255, 255, 255), Color.FromArgb(alpha, 0, 0, 0), 5);
            yes = text.MeasureString(e.Graphics, fontFamily, FontStyle.Bold, 17, Text, new Rectangle(0, 0, 0, 0), strformat, ref referX, ref referY, ref referWidth, ref referHeight);

                  text.DrawString(e.Graphics, fontFamily, FontStyle.Bold, 17, Text, new Point(ClientSize.Width - (int)referWidth, 1), strformat);

            //e.Graphics.DrawString("StringFormat strformat = new StringFormat();", font, Brushes.Black, new Point(ClientSize.Width - (int)stringsize.Width, 1));

        }
    }


}
