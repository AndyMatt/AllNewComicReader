using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AllNewComicReader
{
    public partial class CustomInfoBox : Panel
    {
        private static CustomInfoBox instance = null;

        public static CustomInfoBox Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomInfoBox();
                }

                return instance;
            }
        }
        private Color colorBorder = Color.White;
        private string stringText = "";

        public static int BORDER_OFFSET = 1;
        public static int TEXT_OFFSET = 3;
        static int mAlpha = 0;
        static int mTrueAlpha = 0;

        private CustomInfoBox()
            : base()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor|
                ControlStyles.OptimizedDoubleBuffer| 
                ControlStyles.AllPaintingInWmPaint |  
                ControlStyles.UserPaint,
                true);
            BackColor = Color.Transparent;
            //this.DoubleBuffered = true;
        }

        public static new void Show()
        {
            mAlpha = 1000;
            mTrueAlpha = 255;
        }

        public static new void Hide()
        {
            mAlpha = 0;
            mTrueAlpha = 0;
        }


        public new void Update()
        {
            if (mAlpha > 0)
                mAlpha -= 6;

            if (mAlpha < 0)
                mAlpha = 0;

            mTrueAlpha = mAlpha;

            if (mTrueAlpha > 255)
                mTrueAlpha = 255;
        }

        public new void Paint(object sender, PaintEventArgs e)
        {
            if (!Visible)
                return;

            if (!Properties.Settings.Default.TTPageInfo)
                return;

            if (mAlpha > 0)
            {
                Location = new Point(BORDER_OFFSET, Parent.ClientSize.Height - Height - BORDER_OFFSET);

                Color TextColor = Color.FromArgb(mTrueAlpha, 255, 255, 255);

                Color BrushColorBorder = Color.FromArgb(mTrueAlpha, colorBorder.R, colorBorder.G, colorBorder.B);


                string RenderText = stringText.Replace("@n", Environment.NewLine);
                Size = SetAutoSize(e.Graphics, RenderText);

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb((int)(200.0f * (mTrueAlpha/255.0f)), 0, 0, 0)), Location.X, Location.Y, Size.Width, Size.Height);

                if (e.ClipRectangle.X == 0 && e.ClipRectangle.Y == 0)
                    e.Graphics.DrawRectangle(
                        new Pen(
                            new SolidBrush(BrushColorBorder), 3),
                            new Rectangle(Location.X, Location.Y, Size.Width - 1, Size.Height - 1));
                e.Graphics.DrawString(RenderText, Font, new SolidBrush(BrushColorBorder), new Point(Location.X + TEXT_OFFSET, Location.Y + TEXT_OFFSET), StringFormat.GenericTypographic);

            }  
        }

        public Size SetAutoSize(Graphics g, string RenderText)
        {
            string[] separators = {"@n"};
            Size AdjustedSize = g.MeasureString(RenderText, Font).ToSize();
            string[] substrings = RenderText.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < substrings.Length; i++)
            {
                Size newAdjustedSize = g.MeasureString(substrings[i], Font).ToSize();

                if (newAdjustedSize.Width > AdjustedSize.Width)
                    AdjustedSize = newAdjustedSize;
            }

            AdjustedSize.Height += 5;

            return AdjustedSize;

            
        }
        public Color BorderColor
        {
            get
            {
                return colorBorder;
            }
            set
            {
                colorBorder = value;
            }
        }

        public string TextInput
        {
            get
            {
                return stringText;
            }
            set
            {
                stringText = value;
            }
        }
    }
}
