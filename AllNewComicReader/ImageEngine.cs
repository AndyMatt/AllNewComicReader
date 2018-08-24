using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ImageMagick;
using System.Drawing;
using TextDesignerCSLibrary;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AllNewComicReader
{
    public class KeyDef
    {
        public Keys Key;
        public bool Ctrl;
        public bool Alt;

        public KeyDef()
        {
            Ctrl = false;
            Alt = false;
        }

    }
    public enum ViewSetting
    {
        None,
        Width,
        Height,
        Fit
    }

    class ImageEngine
    {
        public static bool IsLoading;


        private delegate void RefreshSizeDelegate();
        private RefreshSizeDelegate async;

        List<ImageMagick.MagickImage> images;

        public int TextAlpha = 1500;
        int imagepos = 1;

        Size ClientSize;
        public ViewSetting pImageView;
        PictureBox pPictureBox;
        MagickImage image;
        MagickImage Originalimage;
        Bitmap img;
        Compression Comp;
        System.Windows.Forms.Timer AnimatedTimer;

        public string ImageInfo;

        public bool bMaximise;
        public bool bDoubleDisplay;

        public bool bActive;

        int ImageHeight;
        int ImageWidth;

        //bool animated;

        double Rotation;

        float ImageScaledHeight;
        float ImageScaledWidth;

        int OffsetX = 0;
        int OffsetY = 0;

        public int blackout = 0;

        public float ScrollVerticalDelta;
        public float ScrollHorizontalDelta;
        public float MoveVerticalDelta;

        public float MousePrevPageCounter;
        public float MouseNextPageCounter;


        //MagickImageCollection AnimatedFrames;

        public ImageEngine(PictureBox pic, Size Client)
        {
            ImageInfo = "";
            bMaximise = Properties.Settings.Default.Fullscreen;
            ClientSize = Client;
            pPictureBox = pic;
            pImageView = Properties.Settings.Default.PageView;
            pPictureBox.Paint += new PaintEventHandler(OnPaint);
            async = new RefreshSizeDelegate(ResizeEnd);
            bActive = true;
            bDoubleDisplay = false;
            AnimatedTimer = new System.Windows.Forms.Timer();

            MousePrevPageCounter = 0;
            MouseNextPageCounter = 0;
            Rotation = 0;

        }

        public void ChangeArchive(Compression comp)
        {
            Comp = comp;
        }


        static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 2);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        void SetInfo(int FileSize)
        {
            ImageInfo = Comp.GetFileName() + "@n";
            ImageInfo += "Resolution: " + image.Width + "x" + image.Height + "@n";
            ImageInfo += "Size: " + BytesToString(FileSize) + "@n";
            ImageInfo += "Format: " + image.Depth + "-bit " + image.Format.ToString().ToUpper() + "@n";
            ImageInfo += "Density: " + image.Width + "@n";
        }

        public void AddFiletoList(byte[] aray)
        {
            images.Add(new MagickImage(aray));
        }

        public void PreviousImage()
        {
            if (!CheckFirstPage())
            {
                if (Properties.Settings.Default.PageContolPopup)
                    UserControlPopup.Setup("Previous Page");

                byte[] byteStream = Comp.ExtractPrevFile();

                image = new MagickImage(byteStream);

                Originalimage = new MagickImage(image);

                SetInfo(byteStream.Length * sizeof(byte));

                TextAlpha = 1000;
                Properties.Settings.Default.ImageInfoOpacity = 1.0f;
                Properties.Settings.Default.ImageInfoBuffer = 200;

                SetupImage();
            }
        }

        public void ClearHUD()
        {
            UserControlPopup.clear();
            TextAlpha = 0;
            Properties.Settings.Default.ImageInfoOpacity = 0.0f;
            Properties.Settings.Default.ImageInfoBuffer = 0;
        }

        public void SaveOriginalPage()
        {
            SaveFileDialog SaveF = new SaveFileDialog();
            SaveF.FileName = Comp.GetFileName();
            if (SaveF.ShowDialog() == DialogResult.OK)
            {
                Originalimage.Write(new FileInfo(SaveF.FileName));
            }
        }

        public void SaveFilteredPage()
        {
            SaveFileDialog SaveF = new SaveFileDialog();
            SaveF.FileName = Comp.GetFileName();
            if (SaveF.ShowDialog() == DialogResult.OK)
            {
                image.Write(new FileInfo(SaveF.FileName));
            }
        }

        public void GotoPage(uint PageNumber)
        {
            byte[] byteStream = Comp.GetPage(PageNumber);

            image = new MagickImage(byteStream);

            Originalimage = new MagickImage(image);

            SetInfo(byteStream.Length * sizeof(byte));

            SetupImage();
        }

        public void FirstImage()
        {

            images = new List<MagickImage>();
            //List<byte[]> files = Comp.GetAllPages();
            //for(int i = 0; i < files.Count - 1; i++)
            //{
            //    images.Add(new MagickImage(files[i]));
            //}

            if (!CheckFirstPage())
            {
                if (Properties.Settings.Default.PageContolPopup)
                    UserControlPopup.Setup("First Page");

                byte[] byteStream = Comp.GetFirstFile();

                image = new MagickImage(byteStream);

                Originalimage = new MagickImage(image);

                SetInfo(byteStream.Length * sizeof(byte));

                TextAlpha = 1000;
                Properties.Settings.Default.ImageInfoOpacity = 1.0f;
                Properties.Settings.Default.ImageInfoBuffer = 200;

                SetupImage();
            }
        }

        public void LastImage()
        {
            if (!CheckLastPage())
            {
                if (Properties.Settings.Default.PageContolPopup)
                    UserControlPopup.Setup("Last Page");

                byte[] byteStream = Comp.GetLastFile();

                image = new MagickImage(byteStream);
                Comp.iCurrentDoublePage = Comp.iCurrentPage;

                SetInfo(byteStream.Length * sizeof(byte));

                Originalimage = new MagickImage(image);
                bDoubleDisplay = false;

                TextAlpha = 1000;
                Properties.Settings.Default.ImageInfoOpacity = 1.0f;
                Properties.Settings.Default.ImageInfoBuffer = 200;

                SetupImage();
            }
        }

        public bool CheckFirstPage()
        {
            if (Comp.iCurrentPage != 0)
                return false;

            return true;
        }

        public bool CheckLastPage()
        {
            if (Comp.iCurrentPage+1 != Comp.TotalPages)
                return false;

            return true;
        }

        public void NextImage()
        {
            if (!CheckLastPage())
            {
                Console.Out.WriteLine("NextPage");
                ClearHUD();
                CustomInfoBox.Instance.Hide();
                UserControlPopup.Loading();

                IsLoading = true;
                //Main Function f
                BackgroundWorker Loader = new BackgroundWorker();


                Loader.DoWork += delegate(object s, DoWorkEventArgs args)
                {
                    Properties.Settings.Default.ImageInfoOpacity = 0.0f;

                    byte[] byteStream = Comp.ExtractNextFile();

                    image = new MagickImage(byteStream);

                //image = images[imagepos++];
                //stop1.Stop();

                    Comp.iCurrentDoublePage = Comp.iCurrentPage;
                
                    SetInfo(byteStream.Length * sizeof(byte));

                    Originalimage = new MagickImage(image);
                };

                //Runs after Worker has finished
                Loader.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
                {
                    UserControlPopup.Loaded();
                    IsLoading = false;
                    TextAlpha = 1000;
                    Properties.Settings.Default.ImageInfoOpacity = 1.0f;
                    Properties.Settings.Default.ImageInfoBuffer = 200;
                    if (Properties.Settings.Default.PageContolPopup)
                        UserControlPopup.Setup("Next Page");

                    bDoubleDisplay = false;
                    SetupImage();
                };

                //Start new Worker (Thread)
                Loader.RunWorkerAsync();


                //Stopwatch stop1 = new Stopwatch();
                //stop1.Start();

                

            }
        }

        public void NextDoubleImage()
        {

            if (!CheckLastPage())
            {
                Console.Out.WriteLine("NextPage");
                ClearHUD();
                CustomInfoBox.Instance.Hide();
                UserControlPopup.Loading();

                IsLoading = true;
                //Main Function f
                BackgroundWorker Loader = new BackgroundWorker();


                Loader.DoWork += delegate (object s, DoWorkEventArgs args)
                {
                    byte[] byteStream = Comp.ExtractNextFile();
                    image = new MagickImage(byteStream);

                    if (Comp.iCurrentPage + 1 < Comp.TotalPages)
                    {
                        byte[] byteStreamDouble = Comp.ExtractNextDoubleFile();

                        MagickImage image2 = new MagickImage(byteStreamDouble);

                        if (!IsNativeDoublePage(image) && !IsNativeDoublePage(image2))
                        {
                          //  int FirstWidth = image.Width;
                            image.Crop(image.Width + image2.Width, image.Height);
                            image.Composite(image2, image.Width/2, 0);

                            //MagickImage Merged = new MagickImage(new MagickColor(Color.Black), image.Width + image2.Width, image.Height);

                            //Merged.Composite(image, 0, 0);

                            //image = Merged;
                        }
                        else
                            Comp.iCurrentDoublePage--;

                        Originalimage = new MagickImage(image);
                        bDoubleDisplay = true;
                    }
                    else
                    {
                        Comp.iCurrentDoublePage = Comp.iCurrentPage;
                        bDoubleDisplay = false;
                        Originalimage = new MagickImage(image);
                    }



                    //image = new MagickImage(byteStream);

                    //image = images[imagepos++];
                    //stop1.Stop();

                    //Comp.iCurrentDoublePage = Comp.iCurrentPage;
                    //
                    SetInfo(byteStream.Length * sizeof(byte));

                    //Originalimage = new MagickImage(image);
                };

                //Runs after Worker has finished
                Loader.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs args)
                {
                    UserControlPopup.Loaded();
                    IsLoading = false;
                    TextAlpha = 1000;
                    Properties.Settings.Default.ImageInfoOpacity = 1.0f;
                    Properties.Settings.Default.ImageInfoBuffer = 200;
                    if (Properties.Settings.Default.PageContolPopup)
                        UserControlPopup.Setup("Next Page");

                    //bDoubleDisplay = false;
                    SetupImage();
                };

                //Start new Worker (Thread)
                Loader.RunWorkerAsync();


                //Stopwatch stop1 = new Stopwatch();
                //stop1.Start();

            }
        }

        public void ToggleYellow()
        {
            int oldoffsetx = OffsetX;
            int oldoffsety = OffsetY;

            if (Properties.Settings.Default.AutoYellowBalance)
            {
                Properties.Settings.Default.AutoYellowBalance = false;
                UserControlPopup.Setup("Auto Yellow Balance Disabled");
            }
            else
            {
                Properties.Settings.Default.AutoYellowBalance = true;
                UserControlPopup.Setup("Auto Yellow Balance Enabled");
            }

            image = new MagickImage(Comp.ExtractCurrentFile());

            if (Properties.Settings.Default.DoublePage)
            {
                            GetDoubleImage();
            }

            SetupImage();

            OffsetX = oldoffsetx;
            OffsetY = oldoffsety;
        }

        public void ToggleViewMode()
        {
            OffsetX = 0;
            OffsetY = 0;

            if(pImageView == ViewSetting.None)
                pImageView = ViewSetting.Width;
            else if(pImageView == ViewSetting.Width)
                pImageView = ViewSetting.Height;
            else if(pImageView == ViewSetting.Height)
                pImageView = ViewSetting.Fit;
            else if(pImageView == ViewSetting.Fit)
                pImageView = ViewSetting.None;

            UserControlPopup.Setup("View Mode Set to " + pImageView.ToString());

            image = new MagickImage(Originalimage);
            SetupImage();
        }

         public void ToggleDouble()
        {
            //int oldoffsetx = OffsetX;
            //int oldoffsety = OffsetY;

            if (Properties.Settings.Default.DoublePage)
            {
                UserControlPopup.Setup("Double Page Mode Disabled");
                Properties.Settings.Default.DoublePage = false;
                image = new MagickImage(Comp.ExtractCurrentFile());
                Originalimage = new MagickImage(image);
            }
            else
            {
                UserControlPopup.Setup("Double Page Mode Enabled");
                Properties.Settings.Default.DoublePage = true;
                CurrentDoubleImage();
            }

            SetupImage();

            //OffsetX = oldoffsetx;
            //OffsetY = oldoffsety;
        }
                     
        public void ToggleHue()
        {
            int oldoffsetx = OffsetX;
            int oldoffsety = OffsetY;

            if (Properties.Settings.Default.HueSaturation)
            {
                UserControlPopup.Setup("Disable Hue/Saturation Adjustment");
                Properties.Settings.Default.HueSaturation = false;
            }
            else
            {
                UserControlPopup.Setup("Enable Hue/Saturation Adjustment");
                Properties.Settings.Default.HueSaturation = true;
            }

            image = new MagickImage(Comp.ExtractCurrentFile());
          
            SetupImage();

            OffsetX = oldoffsetx;
            OffsetY = oldoffsety;
        }

        void AutoYellowBalance()
        {
            if (!Properties.Settings.Default.AutoYellowBalance)
                return;

            MagickImage sampleimage = new MagickImage(image);
            sampleimage.Scale(100, 100);
            
            //MemoryStream Newfile = new MemoryStream();
            //sampleimage.Write(Newfile);
            //Newfile.Seek(0, 0);
            //sampleimage.Write("test.bmp");
            //sampleimage = new MagickImage("test.bmp");
            //Newfile.Close();
            //image.Equalize();
            IPixelCollection pixels = sampleimage.GetPixels();

            Pixel WhiteThreshold = pixels.GetPixel(0,0);

            for (int x = 0; x < sampleimage.Width; x++)
            {
                for (int y = 0; y < sampleimage.Height; y++)
                    WhiteThreshold = DetermineWhite(WhiteThreshold, pixels.GetPixel(x, y));
            }

            //WhiteThreshold[0] = 65535.0f - WhiteThreshold[0];
            //WhiteThreshold[1] = 65535.0f - WhiteThreshold[1];
            //WhiteThreshold[2] = 65535.0f - WhiteThreshold[2];
            //WhiteThreshold[3] = 65535.0f - WhiteThreshold[3];

            //int a = (int)(WhiteThreshold[3] / 65535.0f * 255.0f);
            int r = (int)(WhiteThreshold.GetChannel(0));
            int g = (int)(WhiteThreshold.GetChannel(1));
            int b = (int)(WhiteThreshold.GetChannel(2));

            Color newc = Color.FromArgb(r,g,b);

            image.LevelColors(new MagickColor(Color.Black), new MagickColor(newc), Channels.All);
            
            /* old style
            //image = Originalimage;
            pixels = image.GetWritablePixels();


            Parallel.For(0, pixels.Width, x =>
            {
                Parallel.For(0, pixels.Height, y =>
       {
           float[] rgb = pixels.GetValue(x, y);
           rgb[0] = SetColor(rgb[0], WhiteThreshold[0]);
           rgb[1] = SetColor(rgb[1], WhiteThreshold[1]);
           rgb[2] = SetColor(rgb[2], WhiteThreshold[2]);
           rgb[3] = SetColor(rgb[3], WhiteThreshold[3]);

           pixels.Set(x, y, rgb);
           //Pixel pix = pixels.GetPixel(x,y);
           //pix = Color.FromArgb(255, 7, 44, 99);
           //img.SetPixel(x, y, pix);
       });
            });

            watch1.Stop();

            Int64 firstTicks = watch1.ElapsedTicks;

            int io = 01 + 10;


            //image.AutoLevel();
            //image.Normalize();
            //image.Contrast();
             * 
             */
        }

        public void ResampleImage()
        {
            image = new MagickImage(Comp.ExtractCurrentFile());

            if (Properties.Settings.Default.DoublePage == false && bDoubleDisplay)
                ReExtractCurentPage();
            else if (Properties.Settings.Default.DoublePage && bDoubleDisplay == false)
                GetDoubleImage();
            
            SetupImage();
        }

        public void ReExtractCurentPage()
        {
            image = new MagickImage(Comp.ExtractCurrentFile());
        }

        public void Rotate()
        {
            UserControlPopup.Setup("Rotate 90°");
            Rotation += 90.0;

            if (Rotation >= 360.0)
            {
                Rotation = 0.0;
            }

            image = new MagickImage(Originalimage);
            SetupImage();
        }

        public double GetRotation()
        {
            return Rotation;
        }

        public void SetRotation(double rotation)
        {
            Rotation = rotation;

            image = new MagickImage(Originalimage);
            SetupImage();
        }

        public void SetupImage()
        {
            /*
            animated = false;
            byte[] file = Comp.ExtractCurrentFile();
            MemoryStream stream = new MemoryStream(file);
            Image gifImg = Image.FromStream(stream);
            System.Drawing.Imaging.FrameDimension dimension = new System.Drawing.Imaging.FrameDimension(gifImg.FrameDimensionsList[0]);
            // Number of frames
            int frameCount = gifImg.GetFrameCount(dimension);

            if(frameCount > 1)
            {
                animated = true;
           

            try
            {
                ImageMagick.MagickImageCollection collect = new MagickImageCollection();
                collect.Read(Comp.ExtractCurrentFile());

                AnimatedFrames = collect;
            }
            catch
            {

            }
            }
            
            */

           
            AutoYellowBalance();

            image.Rotate(Rotation);
             
            image.FilterType = Properties.Settings.Default.ImageFilter;
            image.Interpolate = Properties.Settings.Default.PixelInterpolation;
            

            if (Properties.Settings.Default.HueSaturation)
            {
                double Hue = Properties.Settings.Default.Hue;
                double Saturation = Properties.Settings.Default.Saturation;
                double Brightness = Properties.Settings.Default.Brightness;

                image.Modulate(new Percentage(Brightness), new Percentage(Saturation), new Percentage(Hue));
            }

            



            ScaleBufferedImage();
            

            ImageHeight = image.Height;
            ImageWidth = image.Width;

            MousePrevPageCounter = 0;
            MouseNextPageCounter = 0;            

            //image = new MagickImage(@"C:\Users\Andrew\Pictures\6f68d6c0-0f4e-0131-1f7e-0e4152b67a92.gif");
            //image.


            img = image.ToBitmap();


            //img = new Bitmap(@"C:\Users\Andrew\Pictures\6f68d6c0-0f4e-0131-1f7e-0e4152b67a92.gif");


            /*
            if (!AnimatedTimer.Enabled)
            {
                AnimatedTimer.Interval = 1000 / 30;
                AnimatedTimer.Tick += new EventHandler(Update);
                //AnimatedTimer.Start();
            }
             */

            SetViewMode();

            ScrollVerticalDelta = 0;

            OffsetX = 0;
            OffsetY = 0;

            pPictureBox.Refresh();
        }

        /*
        public void Update(object sender, EventArgs e)
        {
            if (!animated)
                return;

            icurrentfram++;

            if (icurrentfram >= AnimatedFrames.Count)
                icurrentfram = 0;

            if( AnimatedFrames.Count != 0)
                
                img = new Bitmap(BytesToBitmap(AnimatedFrames[icurrentfram].ToByteArray()),new Size(image.Width, image.Height));

           
            //img = new Bitmap(@"C:\Users\Andrew\Pictures\6f68d6c0-0f4e-0131-1f7e-0e4152b67a92.gif");

            //SetViewMode();

            //System.Drawing.Imaging.FrameDimension dimens = new System.Drawing.Imaging.FrameDimension(img.FrameDimensionsList[icurrentfram]);
            //img.SelectActiveFrame(dimens, icurrentfram);
            //img.SelectActiveFrame(img.PhysicalDimension, icurrentfram);
             
        }

         */

        public static Bitmap BytesToBitmap(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                Bitmap img = (Bitmap)Image.FromStream(ms);
                return img;

            }

        }

        void ScaleBufferedImage()
        {
            if (pImageView == ViewSetting.None)
                return;

            MagickGeometry geometry = new MagickGeometry(0, 0);

            if (pImageView == ViewSetting.Width)
            {
                float scale = (float)ClientSize.Width / (float)image.Width;

                int newsizew = (int)Math.Round((image.Width * scale), 1);
                int newsizeh = (int)Math.Round((image.Height * scale), 1);

                geometry = new MagickGeometry(newsizew, newsizeh);
            }
            else if (pImageView == ViewSetting.Height)
            {
                float scale = (float)ClientSize.Height / (float)image.Height;

                    int newsizew = (int)(image.Width * scale);
                    int newsizeh = (int)(image.Height * scale);

                geometry = new MagickGeometry(newsizew, newsizeh);
            }
            else if (pImageView == ViewSetting.Fit)
            {
                float hscale = (float)ClientSize.Height / (float)image.Height;
                float wscale = (float)ClientSize.Width / (float)image.Width;

                int newsizew;
                int newsizeh;

                if (hscale > wscale)
                {
                    newsizew = (int)(image.Width * wscale);
                    newsizeh = (int)(image.Height * wscale);
                }
                else
                {
                    newsizew = (int)(image.Width * hscale);
                    newsizeh = (int)(image.Height * hscale);
                }

                geometry = new MagickGeometry(newsizew, newsizeh);
            }
            else
            {
                geometry = new MagickGeometry(image.Width, image.Height);
            }

            geometry.IgnoreAspectRatio = true;

            if (Properties.Settings.Default.FastRender)
                image.Scale(geometry);

            else
                image.Resize(geometry);
        }

        float SetColor(float pixelcolor, int offset)
        {
            float basef = 65535.0f;

            float newoffset = (float)offset / 255.0f;
            newoffset *= basef;


            float newcolor = pixelcolor - newoffset;
            if (newcolor < 0.0f)
                return 0.0f;

            if (newcolor > basef)
                return basef;

            return newcolor;
        }

        float SetColor(float pixelcolor, float offset)
        {
            float basef = 65535.0f;

            float newcolor = pixelcolor + offset;
            if (newcolor < 0.0f)
                return 0.0f;

            if (newcolor > basef)
                return basef;

            return newcolor;
        }

        Pixel DetermineWhite(Pixel first, Pixel second)
        {
            if (first.GetChannel(0) + first.GetChannel(1) + first.GetChannel(2) < second.GetChannel(0) + second.GetChannel(1) + second.GetChannel(2))
            return second;

                return first;

        }

        public void GetDoubleImage()
        {
            if (Comp.iCurrentPage + 1 < Comp.TotalPages)
            {
                MagickImage image2 = new MagickImage(Comp.ExtractNextDoubleFile());

                if (!IsNativeDoublePage(image) && !IsNativeDoublePage(image2))
                {
                    MagickImage Merged = new MagickImage(new MagickColor(Color.Black), image.Width + image2.Width, image.Height);

                    Merged.Composite(image, 0, 0);
                    Merged.Composite(image2, image.Width, 0);

                    image = Merged;
                }
                else
                    Comp.iCurrentDoublePage--;

                Originalimage = new MagickImage(image);
                bDoubleDisplay = true;
            }
            else
            {
                bDoubleDisplay = false;
                Originalimage = new MagickImage(image);
            }
        }

        public void CurrentDoubleImage()
        {
            image = new MagickImage(Comp.ExtractCurrentFile());

            if (Comp.iCurrentPage + 1 < Comp.TotalPages)
            {
                MagickImage image2 = new MagickImage(Comp.ExtractNextDoubleFile());

                if (!IsNativeDoublePage(image) && !IsNativeDoublePage(image2))
                {
                    MagickImage Merged = new MagickImage(new MagickColor(Color.Black), image.Width + image2.Width, image.Height);

                    Merged.Composite(image, 0, 0);
                    Merged.Composite(image2, image.Width, 0);

                    image = Merged;
                }
                else
                    Comp.iCurrentDoublePage--;

                Originalimage = new MagickImage(image);
                bDoubleDisplay = true;
            }
            else
            {
                bDoubleDisplay = false;
                Originalimage = new MagickImage(image);
            }

            SetupImage();
        }

        public bool IsNativeDoublePage(MagickImage image)
        {
            /*
            MemoryStream stream = new MemoryStream(image.ToByteArray());
            Image gifImg = Image.FromStream(stream);
            System.Drawing.Imaging.FrameDimension dimension = new System.Drawing.Imaging.FrameDimension(gifImg.FrameDimensionsList[0]);
            int frameCount = gifImg.GetFrameCount(dimension);

            if (frameCount > 1)
                return true;
             */

            if (Properties.Settings.Default.SupressForSpread == false)
                return false;

            if (image.Width > image.Height)
                return true;

            return false;
        }

        public void PreviousDoubleImage()
        {
            image = new MagickImage(Comp.ExtractPrevFile());
            if (Comp.iCurrentPage > 0)
            {
                MagickImage image2 = new MagickImage(Comp.ExtractPrevDoubleFile());
                //image = new MagickImage(Comp.ExtractPrevFile());
                if (!IsNativeDoublePage(image) && !IsNativeDoublePage(image2))
                {
                
                MagickImage Merged = new MagickImage(new MagickColor(Color.Black), image.Width + image2.Width, image.Height);

                Merged.Composite(image, image2.Width, 0);
                Merged.Composite(image2, 0, 0);

                image = Merged; 
                }
                else
                    Comp.iCurrentDoublePage++;

                Originalimage = new MagickImage(image);
                bDoubleDisplay = true;
            }
            else
            {
                Comp.iCurrentDoublePage = Comp.iCurrentPage;
                //image = new MagickImage(Comp.ExtractPrevFile());
                Originalimage = new MagickImage(image);
                bDoubleDisplay = false;
            }
            SetupImage();
        }


        void SetViewMode()
        {
            if (pImageView == ViewSetting.None)
                SettoNone();
            else if (pImageView == ViewSetting.Width)
                SettoWidth();
            else if (pImageView == ViewSetting.Height)
                SettoHeight();
            else if (pImageView == ViewSetting.Fit)
                SettoFit();
        }

        public void SettoFit()
        {
            Size Propersize = GetRotatedSize();

            if (Propersize.Height > Propersize.Width)
            {
                ImageScaledHeight = (float)ClientSize.Height;
                ImageScaledWidth = (float)ClientSize.Height / (float)Propersize.Height * (float)Propersize.Width;
            }
            else
            {
                ImageScaledWidth = (float)ClientSize.Width;
                ImageScaledHeight = (float)ClientSize.Width / (float)Propersize.Width * (float)Propersize.Height;
            }

            if (ImageScaledWidth > (float)ClientSize.Width)
            {
                ImageScaledWidth = (float)ClientSize.Width;
                ImageScaledHeight = (float)ClientSize.Width / (float)Propersize.Width * (float)Propersize.Height;
            }
            else if (ImageScaledHeight > (float)ClientSize.Height)
            {
                ImageScaledHeight = (float)ClientSize.Height;
                ImageScaledWidth = (float)ClientSize.Height / (float)Propersize.Height * (float)Propersize.Width;
            }
        }

        public void SettoHeight()
        {
            Size Propersize = GetRotatedSize();

            ImageScaledHeight = (float)ClientSize.Height;
            ImageScaledWidth = (float)ClientSize.Height / (float)Propersize.Height * (float)Propersize.Width;
        }

        public void SettoWidth()
        {
            Size Propersize = GetRotatedSize();

            ImageScaledWidth = (float)ClientSize.Width;
            ImageScaledHeight = (float)ClientSize.Width / (float)Propersize.Width * (float)Propersize.Height;
        }

        public void SettoNone()
        {
            ImageScaledWidth = (float)ImageWidth;
            ImageScaledHeight = (float)ImageHeight;
        }

        Size GetRotatedSize()
        {
            int Width = image.BaseWidth;
            int Height = image.BaseHeight;

            if (Rotation == 90.0 || Rotation == 270.0)
            {
                Width = image.BaseHeight;
                Height = image.BaseWidth;
            }

            return new Size(Width,Height);
        }

        public void OnResize(Size Client)
        {

            ClientSize = Client;

            

            SetViewMode();


            RepositionImage();

            //float scale = (float)ClientSize.Width / (float)image.Width;
            //image.Resize(ClientSize.Height, ClientSize.Height);

           // pPictureBox.Update();
        //    pPictureBox.Refresh();
           
        }

        public void RecieveNewSize(Size Client)
        {
            ClientSize = Client;
        }


        public void ResizeEnd()
        {
            blackout = 0;
            image = new MagickImage(Originalimage);
            image.FilterType = Properties.Settings.Default.ImageFilter;
            image.Interpolate = Properties.Settings.Default.PixelInterpolation;
            //image.TransformReset();

            ScaleBufferedImage();

            ImageHeight = image.Height;
            ImageWidth = image.Width;

           
            //Bitmap bmp = image.ToBitmap();
            //img.SetResolution(1, 1);


            img = image.ToBitmap(System.Drawing.Imaging.ImageFormat.Bmp);


            SetViewMode();
        }

        public void SpeedTest()
        {
            async.BeginInvoke(null, null);
        }


        private void OnFrameChanged(object o, EventArgs e)
        {

            //Force a call to the Paint event handler. 
            pPictureBox.Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            //return;
                
            if (!bActive)
                return;
            /*
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
             */

            //AnimateImage();
            //ImageAnimator.UpdateFrames();

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            

            Rectangle Pos = CenterImage();

            e.Graphics.ResetTransform();

            e.Graphics.TranslateTransform(OffsetY, OffsetX);

            if (blackout > 0)
            {
                blackout--;
                e.Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(Pos.Left, Pos.Top, (int)ImageScaledWidth, (int)ImageScaledHeight));
            }

            else
            {
                
                e.Graphics.DrawImage(
                    img,
                    new Rectangle(Pos.Left, Pos.Top, (int)ImageScaledWidth, (int)ImageScaledHeight),
                    // destination rectangle 
                    0,
                   0,           // upper-left corner of source rectangle
                   Pos.Width,       // width of source rectangle
                    Pos.Height,       // height of source rectangle
                    GraphicsUnit.Pixel);
                 
            }
           //e.Graphics.Dispose();

           // Console.WriteLine(OffsetX);

        }

        public void OnUpdate(Input input)
        {
            MoveVerticalDelta = input.pMouseDelta.Y;
            input.pMouseDelta = new Point(0,0);

            ScrollVerticalDelta -= (input.iMouseScroll);

            float scrollamount = Properties.Settings.Default.ScrollAmount;

            if (input.bDownKey)
                ScrollVerticalDelta += scrollamount;
            if (input.bUpKey)
                ScrollVerticalDelta -= scrollamount;

            if (input.bLeftKey)
                ScrollHorizontalDelta -= scrollamount;
            if (input.bRightKey)
                ScrollHorizontalDelta += scrollamount;

            if (ScrollVerticalDelta > 0.000)
            {
                float scrolld = lerp(0.0f, ScrollVerticalDelta, 0.1f);

                if (ImageScaledHeight - ClientSize.Height + OffsetX > 0)
                {
                    if (ImageScaledHeight - ClientSize.Height + OffsetX < scrolld)
                    {
                        OffsetX -= (int)ImageScaledHeight - ClientSize.Height + OffsetX;
                        ScrollVerticalDelta = 0;
                        //MouseNextPageCounter++;
                    }
                    else
                    {
                        OffsetX -= (int)scrolld;
                        ScrollVerticalDelta -= scrolld;
                    }
                }
                else
                    ScrollVerticalDelta = 0;

                if (OffsetX == ClientSize.Height - (int)ImageScaledHeight && ScrollVerticalDelta == 0 && scrolld > 0 && input.iMouseScroll < 0)
                    MouseNextPageCounter++;

            }
            else if (ScrollVerticalDelta < 0.000)
            {
                float scrolld = lerp(0.0f, ScrollVerticalDelta, -0.1f);

                if (OffsetX < 0)
                {
                    if (OffsetX + scrolld > 0)
                    {
                        scrolld = OffsetX = 0;
                        ScrollVerticalDelta = 0;
                        
                    }
                    else
                    {
                        ScrollVerticalDelta += scrolld;
                        OffsetX += (int)scrolld;
                    }
                }
                else
                    ScrollVerticalDelta = 0;

                if (OffsetX == 0 && ScrollVerticalDelta == 0 && scrolld > 0 && input.iMouseScroll > 0)
                MousePrevPageCounter++;

            }

            //HOrizonalMovement
            if (ScrollHorizontalDelta > 0.000)
            {
                float scrolld = lerp(0.0f, ScrollHorizontalDelta, 0.1f);

                if (ImageScaledWidth - ClientSize.Width + OffsetY > 0)
                {
                    if (ImageScaledWidth - ClientSize.Width + OffsetY < scrolld)
                    {
                        OffsetY -= (int)ImageScaledWidth - ClientSize.Width + OffsetY;
                        ScrollHorizontalDelta = 0;
                    }
                    else
                    {
                        OffsetY -= (int)scrolld;
                        ScrollHorizontalDelta -= scrolld;
                    }
                }
                else
                    ScrollHorizontalDelta = 0;
            }
            else if (ScrollHorizontalDelta < 0.000)
            {
                float scrolld = lerp(0.0f, ScrollHorizontalDelta, -0.1f);

                if (OffsetY < 0)
                {
                    if (OffsetY + scrolld > 0)
                    {
                        scrolld = OffsetY = 0;
                        ScrollHorizontalDelta = 0;

                    }
                    else
                    {
                        ScrollHorizontalDelta += scrolld;
                        OffsetY += (int)scrolld;
                    }
                }
                else
                    ScrollHorizontalDelta = 0;
            }
           

            //Mouse Move Input
            if (OffsetX + (int)MoveVerticalDelta > 0)
            {
                OffsetX = 0;
            }

            else if (OffsetX + (int)MoveVerticalDelta < ClientSize.Height - ImageScaledHeight)
            {
                OffsetX = ClientSize.Height - (int)ImageScaledHeight;
            }
            else
            {
                OffsetX += (int)MoveVerticalDelta;
            }
           // Console.Write(ScrollDownDelta + Environment.NewLine);

            //MouseScrollPageSwap
            if (MouseNextPageCounter > 3.0f)
            {
                input.FunctionNextPage();
                MouseNextPageCounter = 0.0f;
            }
            if (MousePrevPageCounter > 3.0f)
            {
                input.FunctionPreviousPage();
                MousePrevPageCounter = 0.0f;
            }

            if (MouseNextPageCounter > 0)
            MouseNextPageCounter -= 0.03f;
            if (MousePrevPageCounter > 0)
            MousePrevPageCounter -= 0.03f;

            input.iMouseScroll = 0;

        }

        void RepositionImage()
        {
            if (ImageScaledHeight - ClientSize.Height + OffsetX < 0)
            {
                OffsetX = ClientSize.Height - (int)ImageScaledHeight;
            }
        }

        Rectangle CenterImage()
        {
            int PositionX = 0;
            int PositionY = 0;

            if (ImageScaledWidth < ClientSize.Width)
            {
                PositionX = (ClientSize.Width / 2) - (int)(ImageScaledWidth / 2);
            }


            if (ImageScaledHeight < ClientSize.Height)
            {
                OffsetX = 0;
                PositionY = (ClientSize.Height / 2) - (int)(ImageScaledHeight / 2);
            }
            else
            {
                PositionY = 0;
            }

            return new Rectangle(PositionX, PositionY, ImageWidth, ImageHeight);
        }



        float lerp(float v0, float v1, float t)
        {
            return v0 * (1 -t) + v1 * t;
        }
    }
}
