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
        private Color colorBorder = Color.Red;
        private string stringText = "";
        private float Opacity = 0.0f;
        public int BackOpacty = 200;
        int OpacityBuffer = 200;
        public bool Active;

        private CustomInfoBox()
            : base()
        {
            Active = false;
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor|
                ControlStyles.OptimizedDoubleBuffer| 
                ControlStyles.AllPaintingInWmPaint |  
                ControlStyles.UserPaint,
                true);
            BackColor = Color.Transparent;
            Opacity = 1.0f;
            //this.DoubleBuffered = true;
        }

        public void Fade()
        {
            Opacity = Properties.Settings.Default.ImageInfoOpacity;
            OpacityBuffer = Properties.Settings.Default.ImageInfoBuffer;

            if (OpacityBuffer > 0)
            {
                Properties.Settings.Default.ImageInfoBuffer--;
                Visible = true;
            }
            else
            {
                if (Opacity > 0)
                {
                    Properties.Settings.Default.ImageInfoOpacity -= 0.02f;
                }
                if (Opacity < 0)
                {
                    Visible = false;
                    Opacity = 0;
                }
            }

            //Properties.Settings.Default.ImageInfoOpacity = Opacity;
            //Properties.Settings.Default.ImageInfoBuffer = OpacityBuffer;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Active)
                return;

            if (!Properties.Settings.Default.TTPageInfo)
                return;

            if (Opacity != 0)
            {
                Location = new Point(3, Parent.ClientSize.Height - Height - 3);

                int TextOpacity = (int)(255 * Opacity);
                Color TextColor = Color.FromArgb(TextOpacity, 255, 255, 255);

                Color BrushColorBorder = Color.FromArgb((int)(colorBorder.A * Opacity), colorBorder.R, colorBorder.G, colorBorder.B);


                string RenderText = stringText.Replace("@n", Environment.NewLine);
                Size = SetAutoSize(e.Graphics, RenderText);
                //base.OnPaint(e);
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb((int)(200 * Opacity), 0, 0, 0)), 0, 0, Width, Height);
                //e.Graphics.DrawRectangle(new Pen(Color.FromArgb(155,0,0,0),20),0,0,Width,Height);

                if (e.ClipRectangle.X == 0 && e.ClipRectangle.Y == 0)
                    e.Graphics.DrawRectangle(
                        new Pen(
                            new SolidBrush(BrushColorBorder), 3),
                            new Rectangle(e.ClipRectangle.Location.X, e.ClipRectangle.Location.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1));
                e.Graphics.DrawString(RenderText, Font, new SolidBrush(BrushColorBorder), new Point(3, 2), StringFormat.GenericTypographic);

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
