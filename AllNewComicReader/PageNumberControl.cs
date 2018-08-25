using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TextDesignerCSLibrary;

namespace AllNewComicReader
{
    class PageNumberControl
    {
        static int mAlpha;
        static int mTrueAlpha;
        static int mCurrentPage;
        static int mCurrentDoublePage;
        static int mTotalPages;

        public static void Update()
        {
            if(mAlpha > 0)
            mAlpha -= 6;

            if (mAlpha < 0)
                mAlpha = 0;

            mTrueAlpha = mAlpha;

            if (mTrueAlpha > 255)
                mTrueAlpha = 255;
        }

        public static void Show(int TotalPages, int CurrentPage, int DoublePage = -1)
        {
            mCurrentPage = CurrentPage;
            mCurrentDoublePage = DoublePage;
            mTotalPages = TotalPages;

            mAlpha = 1000;
            mTrueAlpha = 255;
        }

        public static void Hide()
        {
            mAlpha = 0;
        }

        public static void Paint(object sender, PaintEventArgs e, Size ClientSize)
        {
            if (!Properties.Settings.Default.TTPageNumber)
                return;

            if (mAlpha > 0)
            {
                FontFamily fontFamily = new FontFamily("Arial Black");

                Font font = new Font(fontFamily, 15, FontStyle.Bold);
                StringFormat strformat = new StringFormat();

                string szbuf;

                if(mCurrentDoublePage == mCurrentPage)
                {
                    szbuf = mCurrentPage + 1 + "/" + mTotalPages;
                }
                else
                {
                    if (mCurrentPage < mCurrentDoublePage)
                        szbuf = mCurrentPage + 1 + "-" + (mCurrentDoublePage + 1) + "/" + mTotalPages;
                    else
                        szbuf = (mCurrentDoublePage + 1) + "-" + (mCurrentPage + 1) + "/" + mTotalPages;
                }

                Size stringsize = e.Graphics.MeasureString(szbuf, font).ToSize();
                OutlineText text = new OutlineText();
                text.TextOutline(Color.FromArgb(mTrueAlpha, 255, 255, 255), Color.FromArgb(mTrueAlpha, 0, 0, 0), 8);
                text.DrawString(e.Graphics, fontFamily,
                    FontStyle.Bold, 17, szbuf, new Point(ClientSize.Width / 2 - (stringsize.Width / 2), ClientSize.Height - stringsize.Height), strformat);
            }
        }
    }
}
