using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ImageMagick;
using SevenZip;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using TextDesignerCSLibrary;
using System.Drawing.Text;
using System.Diagnostics;
using System.Threading;

namespace AllNewComicReader
{
    public partial class Form1 : Form
    {

        const int SC_MAXIMIZE = 0xF030;
        const int WM_LBUTTONDBLCLK = 0x0203;
        const int WM_NCLBUTTONDBLCLK = 0x00A3;

        List<MenuItem> RecentFilesMenuItems;
        ImageEngine imageEngine;
        Settings ProgramSettings;
        Input input;
        Compression CompressEngine;
        FormWindowState bPreviousState;
        Bookmark BookMarksonFile;


        bool Open;
        string OpenedFile;
        System.Windows.Forms.Timer DrawTimer;

        public Form1()
        {
            InitializeComponent();

            if (IntPtr.Size == 8) //x64
                Text = "All New Comic Reader 64bit";
            else //x86
                Text = "All New Comic Reader 32bit";

            Onstartup();
            SetFullscreen();
        }

        public Form1(string[] args)
        {
                InitializeComponent();
                Onstartup();

                if (imageEngine != null)
                    imageEngine.bActive = false;

                Open = false;

                Text = Path.GetFileName(args[0]);
                OpenedFile = args[0];

                Setup(args[0]);

                SetFullscreen();            
        }

        public void SetFullscreen()
        {
            if (Properties.Settings.Default.Fullscreen == true)
            {
                if (imageEngine != null)
                    imageEngine.blackout += 15;

                Menu = null;

                Opacity = 0;

                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                if (WindowState == FormWindowState.Maximized)
                {

                    WindowState = FormWindowState.Normal;
                }
                WindowState = FormWindowState.Maximized;

                pictureBox.Refresh();

                //Screen screen = Screen.FromControl(this);
                //ClientSize = screen.WorkingArea.Size;

                Opacity = 100;
                pictureBox.Refresh();

            }
        }

        void Onstartup()
        {
            RecentFilesMenuItems = new List<MenuItem>();
            PopulateRecentFiles();

            BookMarksonFile = new Bookmark();
            BookMarksonFile.Deserialize();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            bPreviousState = FormWindowState.Normal;

            this.DoubleBuffered = true;

            MagickNET.Initialize(Path.GetDirectoryName(Application.ExecutablePath) + "//MyImageMagickXmlFiles");


            this.Resize += new EventHandler(Form1_Resize);
            this.ResizeEnd += new EventHandler(Form1_ResizeEnd);
            label1.Paint += new PaintEventHandler(OnPaint);

            ElevatedDragDropManager.Instance.EnableDragDrop(Handle);

            ElevatedDragDropManager.Instance.EnableDragDrop(pictureBox.Handle);

            ElevatedDragDropManager.Instance.ElevatedDragDrop += DragDroptoViewer;

            ProgramSettings = new Settings();
            ProgramSettings.VisibleChanged += new EventHandler(ProgramSettings_Hide);
            //Menu = mainMenu1;
            //pictureBox.Parent = this;

            RefreshMenuItems();

            customInfoBox1.Parent = pictureBox;
            customInfoBox1.Location = new Point(0, 0);
            //customInfoBox1.BackColor = Color.FromArgb(155, 0, 0, 0);
            
            //input = new Input(this, mainMenu1, imageEngine, pictureBox, customInfoBox1, ProgramSettings, this);            

            //SetFullscreen();

            input = new Input(this, mainMenu1, pictureBox, ProgramSettings, this);

            pictureBox.Location = new Point(0, 0);

            if (WindowState == FormWindowState.Maximized)
                pictureBox.Size = Screen.PrimaryScreen.Bounds.Size;
            else
                pictureBox.Size = this.ClientSize; ;

            
            //FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //WindowState = FormWindowState.Maximized;
        }

        void PopulateRecentFiles()
        {
            try
            {
                foreach (MenuItem item in RecentFilesMenuItems)
                {
                    menuItemFile.MenuItems.Remove(item);
                }
                RecentFilesMenuItems = new List<MenuItem>();

                if (RecentFiles.Recent.Count == 0)
                {
                    MenuItem recent = new MenuItem("No Recent Files");
                    recent.Enabled = false;
                    RecentFilesMenuItems.Add(recent);

                    menuItemFile.MenuItems.Add(4, recent);
                }
                else
                {
                    foreach (string filename in RecentFiles.Recent)
                    {
                        MenuItem recent = new MenuItem(filename, RecentClick);
                        RecentFilesMenuItems.Add(recent);
                        menuItemFile.MenuItems.Add(4, recent);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception Raised by Recent Files: " + e.InnerException,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        void RecentClick(object sender, EventArgs e)
        {
            MenuItem selected = (MenuItem)sender;
            string path = RecentFiles.GetFilepath(selected.Text);

            if (path != "")
            {
                if (File.Exists(path))
                {
                    Open = false;
                    if (imageEngine != null)
                        imageEngine.bActive = false;
                    Text = Path.GetFileName(path);
                    OpenedFile = path;
                    Setup(path);
                }
                else
                {
                    MessageBox.Show("Could not locate the file '" + path + "'", "Cannot Open File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
                
        }

        void Setup(string Filename)
        {
            bool gotoBookmark = false;
            //BookMarkCheck
            if(BookMarksonFile.CheckifBookMarked(Filename))
            {
                if (Properties.Settings.Default.Bookmarks && Properties.Settings.Default.BookmarkAsk)
                {
                    DialogResult dialogResult = MessageBox.Show("Go to last opened Page? (Page " + (BookMarksonFile.GetPage(Filename)+1).ToString() + ")", "Bookmark", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                        gotoBookmark = true;
                }
                else if(Properties.Settings.Default.Bookmarks && Properties.Settings.Default.BookmarkAsk == false)
                {
                     gotoBookmark = true;
                }
            }

            RecentFiles.Serialize(Filename);
            PopulateRecentFiles();

            CompressEngine = new Compression(Filename);  
            if (!Open)
                Init(Filename);

            imageEngine.ChangeArchive(CompressEngine);

            menuItemRotate0.Checked = true;
            menuItemRotate180.Checked = false;
            menuItemRotate270.Checked = false;
            menuItemRotate90.Checked = false;


            if (WindowState == FormWindowState.Maximized)
                pictureBox.Size = Screen.PrimaryScreen.Bounds.Size;
            else
                pictureBox.Size = this.ClientSize; ;


            //PageNumber

                   if(gotoBookmark)
                   {
                        imageEngine.GotoPage(BookMarksonFile.GetPage(Filename));
                   }
                   else
                   {
                        imageEngine.FirstImage();
                   }


                   input.Setup(imageEngine, customInfoBox1);

                   imageEngine.TextAlpha = 1000;
                   Properties.Settings.Default.ImageInfoOpacity = 1.0f;
                   Properties.Settings.Default.ImageInfoBuffer = 200;
           

            label1.Parent = pictureBox;
            label1.Location = new Point(pictureBox.Width - label1.Width + 32, pictureBox.Height - label1.Height - 30);
            label1.Size = Size;
            label1.Location = new Point(0, 0);


            customInfoBox1.TextInput = imageEngine.ImageInfo;
            customInfoBox1.Active = true;

            if (InputControl.Parent != pictureBox)
            InputControl.Parent = pictureBox;

            Open = true;
            
        }



        void ProgramSettings_Hide(object sender, EventArgs e)
        {
            if (ProgramSettings.bRefresh)
            {
                ProgramSettings.bRefresh = false;
                if (imageEngine != null)
                {
                    imageEngine.pImageView = Properties.Settings.Default.PageView;
                    imageEngine.ResampleImage();
                }
            }
        }

        void DragDroptoViewer(System.Object sender, ElevatedDragDropArgs e)
        {
            try
            {
                if (imageEngine != null)
                    imageEngine.bActive = false;

                Open = false;

                Text = Path.GetFileName(e.Files[0]);
                OpenedFile = e.Files[0];
                    
                    Setup(e.Files[0]);

                    
                
            }
            catch
            {
                Text = "All New Comic Reader";

                MessageBox.Show("File '" + e.Files[0].ToString() + "' is not Supported"); 
            }
            
            //Init();
        }

        void Init(string Filename)
        {
            if (!timer1.Enabled)
            {
                timer1.Interval = 1000 / 200;
                timer1.Tick += new EventHandler(Update);
                timer1.Start();
            }


            if (DrawTimer == null)
            {
                DrawTimer = new System.Windows.Forms.Timer();
                DrawTimer.Interval = 1000 / 200;
                DrawTimer.Tick += new EventHandler(Redraw);
                DrawTimer.Start();
            }

           imageEngine = new ImageEngine(pictureBox, ClientSize);
        }

        public void ShowContextMenu(object sender, MouseEventArgs e)
        {
                MenuItem ViewMode = new MenuItem("View Mode");
                ViewMode.MenuItems.Add("None", new EventHandler(menuItemSettoNone_Click));
                ViewMode.MenuItems.Add("Set to Width", new EventHandler(menuItemSettoWidth_Click));
                ViewMode.MenuItems.Add("Set to Height", new EventHandler(menuItemSettoHeight_Click));
                ViewMode.MenuItems.Add("Set to Fit", new EventHandler(menuItemFitView_Click));

                MenuItem Rotate = new MenuItem("Rotate");
                Rotate.MenuItems.Add("0 Degrees", delegate { imageEngine.SetRotation(0.0); });
                Rotate.MenuItems.Add("90 Degrees", delegate { imageEngine.SetRotation(90.0); });
                Rotate.MenuItems.Add("180 Degrees", delegate { imageEngine.SetRotation(180.0); });
                Rotate.MenuItems.Add("270 Degrees", delegate { imageEngine.SetRotation(270.0); });


                if (imageEngine != null)
                {
                    if (imageEngine.GetRotation() == 0.0) Rotate.MenuItems[0].Checked = true;
                    else if (imageEngine.GetRotation() == 90.0) Rotate.MenuItems[1].Checked = true;
                    else if (imageEngine.GetRotation() == 180.0) Rotate.MenuItems[2].Checked = true;
                    else if (imageEngine.GetRotation() == 270.0) Rotate.MenuItems[3].Checked = true;
                }
                else { Rotate.MenuItems[0].Checked = true; }

                if (Properties.Settings.Default.PageView == ViewSetting.None) ViewMode.MenuItems[0].Checked = true;
                else if (Properties.Settings.Default.PageView == ViewSetting.Width) ViewMode.MenuItems[1].Checked = true;
                else if (Properties.Settings.Default.PageView == ViewSetting.Height) ViewMode.MenuItems[2].Checked = true;
                else if (Properties.Settings.Default.PageView == ViewSetting.Fit) ViewMode.MenuItems[3].Checked = true;

                MenuItem Recent = new MenuItem("Recent");
                if (RecentFiles.Recent.Count == 0)
                {
                    MenuItem recent = new MenuItem("No Recent Files");
                    recent.Enabled = false;
                    Recent.MenuItems.Add(recent);
                }
                else
                {
                    foreach (string filename in RecentFiles.Recent)
                    {
                        MenuItem recent = new MenuItem(filename, RecentClick);
                        Recent.MenuItems.Add(0,recent);
                    }
                }

                //EventHandler NextPage += delegate
                ContextMenu cm = new ContextMenu();
                cm.MenuItems.Add("Open", new EventHandler(OpenFile));
                cm.MenuItems.Add(Recent);
                cm.MenuItems.Add("-");
                cm.MenuItems.Add("Previous Page", delegate { input.FunctionPreviousPage(); });
                cm.MenuItems.Add("Next Page", delegate { input.FunctionNextPage(); });
                cm.MenuItems.Add("First Page", delegate { input.FunctionFirstPage(); });
                cm.MenuItems.Add("Last Page", delegate { input.FunctionLastPage(); });
                cm.MenuItems.Add("-");
                cm.MenuItems.Add("Load Next Archive", new EventHandler(LoadNextFile));
                cm.MenuItems.Add("Load Previous Archive", new EventHandler(LoadPreviousFile));
                cm.MenuItems.Add("-");
                cm.MenuItems.Add(ViewMode);
                cm.MenuItems.Add(Rotate); 
                cm.MenuItems.Add("-");
                cm.MenuItems.Add("Save Original Image As", delegate { if(imageEngine!=null) imageEngine.SaveOriginalPage(); });
                cm.MenuItems.Add("Save Processed Image As", delegate { if (imageEngine != null)  imageEngine.SaveFilteredPage(); });
                cm.MenuItems.Add("Minimise", delegate { this.WindowState = FormWindowState.Minimized; });
                cm.MenuItems.Add("Fullscreen", delegate { input.FunctionFullscreen(); });
                cm.MenuItems.Add("-");
                cm.MenuItems.Add("Options", new EventHandler(OpenSettings));
                cm.MenuItems.Add("About", new EventHandler(menuItemAbout_Click));
                cm.MenuItems.Add("Exit", delegate { Environment.Exit(0); });

                Point pt = PointToClient(System.Windows.Forms.Control.MousePosition);
                cm.Show(this, pt);
        }

        void Update(object sender, EventArgs e)
        {
            RefreshMenuItems();
            UserControlPopup.Update();
            customInfoBox1.TextInput = imageEngine.ImageInfo;
            customInfoBox1.Fade();
            imageEngine.TextAlpha -= 6;

            //Refresh();
            //pictureBox.Refresh();
            //imageEngine.Update();
            imageEngine.OnUpdate(input);
        }

        void Redraw(object sender, EventArgs e)
        {
            //Refresh();
            pictureBox.Refresh();
        }

        void Form1_Resize(object sender, EventArgs e)
        {

            if (!Open)
                return;

            customInfoBox1.Location = new Point(0, 0);
            label1.Size = Size;
            label1.Location = new Point(0, 0);

           // if (WindowState == FormWindowState.Maximized)

            pictureBox.Size = ClientSize;

            pictureBox.Location = new Point(0, 0);

            pictureBox.Refresh();

            imageEngine.RecieveNewSize(this.ClientSize);

            if (WindowState != bPreviousState)
            {
                switch (this.WindowState)
                {
                    case FormWindowState.Maximized:

                        imageEngine.ResizeEnd();
                        imageEngine.bMaximise = false;
                        break;
                    case FormWindowState.Normal:
                        imageEngine.ResizeEnd();
                        
                        break;
                    default:
                        break;
                }
            }

            imageEngine.OnResize(ClientSize);


            pictureBox.Refresh();
            bPreviousState = WindowState;
        }

         void Form1_ResizeEnd(object sender, EventArgs e)
        {

            if (imageEngine != null)
            imageEngine.ResizeEnd();
            pictureBox.Refresh();
            
        }

        
         protected override void WndProc(ref Message m)
         {

             const int HTCAPTION = 0x2;
             const int WM_NCLBUTTONDBLCLK = 0xA3;

             Point CursorPos = PointToClient(Cursor.Position);
             //if (m.Msg == WM_NCLBUTTONDBLCLK && this.WindowState == FormWindowState.Normal && CursorPos.Y < -20 && CursorPos.Y > -45)
             

             switch (m.Msg)
             {
                 case WM_NCLBUTTONDBLCLK:
                     if (m.WParam.ToInt32() == HTCAPTION)
                     {
                         //return;
                        if(imageEngine != null)
                         imageEngine.blackout += 10;

                     }
                     break;
             };





             if (m.Msg == 0x0112) // WM_SYSCOMMAND
             {
                 // Check your window state here
                 if (m.WParam == new IntPtr(0xF030)) // Maximize event - SC_MAXIMIZE from Winuser.h
                 {
                     imageEngine.blackout += 10;
                 }
             }

             base.WndProc(ref m);
         }
        
        

          void OnPaint(object sender, PaintEventArgs e)
         {
             if (!Open)
                 return;

             e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
             e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

             UserControlPopup.Paint(sender, e, ClientSize);

             if (imageEngine.MouseNextPageCounter > 0 && !imageEngine.CheckLastPage())
              {
                  Image myImage = Properties.Resources.NextPage;
                  myImage = (Image)(new Bitmap(myImage, new Size(100,100)));
                  myImage = SetImageOpacity(myImage, (float)imageEngine.MouseNextPageCounter / 3.0f);
                  myImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                  e.Graphics.DrawImage(myImage, new Point(e.ClipRectangle.Width - 100, e.ClipRectangle.Height -100));

              }

              if (imageEngine.MousePrevPageCounter > 0 && !imageEngine.CheckFirstPage())
              {
                  Image myImage = Properties.Resources.NextPage;
                  myImage = (Image)(new Bitmap(myImage, new Size(100, 100)));
                  myImage = SetImageOpacity(myImage, (float)imageEngine.MousePrevPageCounter / 3.0f);
                  e.Graphics.DrawImage(myImage, new Point(0, e.ClipRectangle.Height - 100));

              }

              if (!Properties.Settings.Default.TTPageNumber)
                return;

              int alpha = imageEngine.TextAlpha;
              if (imageEngine.TextAlpha > 255)
                 alpha = 255;

              if (imageEngine.TextAlpha > 0)
             {
                 FontFamily fontFamily = new FontFamily("Arial Black");

                 Font font = new Font(fontFamily, 15, FontStyle.Bold);
                 StringFormat strformat = new StringFormat();

                 string szbuf;

                 if (Properties.Settings.Default.DoublePage && CompressEngine.iCurrentPage != CompressEngine.iCurrentDoublePage)
                 {
                     if(CompressEngine.iCurrentPage < CompressEngine.iCurrentDoublePage)
                         szbuf = CompressEngine.iCurrentPage + 1 + "-" + (CompressEngine.iCurrentDoublePage + 1) + "/" + CompressEngine.TotalPages;
                     else
                         szbuf = (CompressEngine.iCurrentDoublePage + 1) + "-" + (CompressEngine.iCurrentPage + 1) + "/" + CompressEngine.TotalPages;
                 }
                 else
                 szbuf = CompressEngine.iCurrentPage + 1 + "/" + CompressEngine.TotalPages;

                 Size stringsize = e.Graphics.MeasureString(szbuf, font).ToSize();
                 OutlineText text = new OutlineText();
                 text.TextOutline(Color.FromArgb(alpha, 255, 255, 255), Color.FromArgb(alpha, 0, 0, 0), 8);
                 text.DrawString(e.Graphics, fontFamily,
                     FontStyle.Bold, 17, szbuf, new Point(ClientSize.Width / 2 - (stringsize.Width / 2), ClientSize.Height - stringsize.Height), strformat);
                 //fontFamily.Dispose();
                 //e.Graphics.Dispose();
             }
         }

          private void OpenFile(object sender, EventArgs e)
          {
              OpenFileDialog OpenFile = new OpenFileDialog();
              OpenFile.Filter = "Comic Book Files(*.cbz;*.cbr;*.cz,*.cbt)|*.CBZ;*.CBR;*.CZ;*.CBT";
              OpenFile.Multiselect = false;
              OpenFile.FileName = "";
              OpenFile.Title = "Open Comic";
              DialogResult result = OpenFile.ShowDialog();

              if (result == System.Windows.Forms.DialogResult.OK)
              {
                  Open = false;
                  if(imageEngine != null)
                  imageEngine.bActive = false;
                  Text = Path.GetFileName(OpenFile.FileName);
                  OpenedFile = OpenFile.FileName;
                  Setup(OpenFile.FileName);
              }
          }

          public void LoadNextFile(object sender, EventArgs e)
          {
              if (OpenedFile == null)
                  return;

              string[] FilesinDir = GetSupportedFiles(Directory.EnumerateFiles(Path.GetDirectoryName(OpenedFile)).ToArray());
              int FilePosition = -1;

              for (int i = 0; i < FilesinDir.Length; i++)
              {
                  if (FilesinDir[i] == OpenedFile)
                      FilePosition = i;
              }

              if (FilePosition != FilesinDir.Length - 1)
              {
                  UserControlPopup.Setup("Loaded Next Comic");
                  OpenedFile = FilesinDir[FilePosition + 1];
                  Setup(FilesinDir[FilePosition + 1]);
              }

              else
              {
                  MessageBox.Show("This is the last Comic in the Directory");
              }


          }

          public void LoadPreviousFile(object sender, EventArgs e)
          {
              if (OpenedFile == null)
                  return;

              string[] FilesinDir = GetSupportedFiles(Directory.EnumerateFiles(Path.GetDirectoryName(OpenedFile)).ToArray());
              int FilePosition = -1;

              for (int i = 0; i < FilesinDir.Length; i++)
              {
                  if (FilesinDir[i] == OpenedFile)
                      FilePosition = i;
              }

              if (FilePosition > 0)
              {
                  UserControlPopup.Setup("Previous Next Comic");
                  OpenedFile = FilesinDir[FilePosition - 1];
                  Setup(FilesinDir[FilePosition - 1]);
              }
              else
                  MessageBox.Show("This is the first Comic in the Directory");



          }

          public string[] GetSupportedFiles(string[] RawFiles)
          {
              List<string> supported = new List<string>();

              for (int i = 0; i < RawFiles.Length; i++)
              {
                  if (Path.GetExtension(RawFiles[i]) == ".cbr" || Path.GetExtension(RawFiles[i]) == ".cbz")
                      supported.Add(RawFiles[i]);
              }

              return supported.ToArray();
          }

          private void OpenSettings(object sender, EventArgs e)
          {
              ProgramSettings.StartPosition = FormStartPosition.CenterParent;
              ProgramSettings.RefreshSettings();
              ProgramSettings.ShowDialog();

          }

          private void menuItemSettoWidth_Click(object sender, EventArgs e)
          {
              Properties.Settings.Default.PageView = ViewSetting.Width;
            
              if(imageEngine != null)
              {
              imageEngine.pImageView = ViewSetting.Width;
              imageEngine.ResampleImage();
              }
          }

          private void menuItemSettoHeight_Click(object sender, EventArgs e)
          {
              Properties.Settings.Default.PageView = ViewSetting.Height;
              if (imageEngine != null)
              {
                  imageEngine.pImageView = ViewSetting.Height;
                  imageEngine.ResampleImage();
              }
          }

          private void menuItemFitView_Click(object sender, EventArgs e)
          {
              Properties.Settings.Default.PageView = ViewSetting.Fit;
              if (imageEngine != null)
              {
                  imageEngine.pImageView = ViewSetting.Fit;
                  imageEngine.ResampleImage();
              }
          }

          private void menuItemSettoNone_Click(object sender, EventArgs e)
          {
              Properties.Settings.Default.PageView = ViewSetting.None;
              if (imageEngine != null)
              {
                  imageEngine.pImageView = ViewSetting.None;
                  imageEngine.ResampleImage();
              }
          }

          private void menuItemDoublePage_Click(object sender, EventArgs e)
          {
              if (imageEngine != null)
              {
                  imageEngine.ToggleDouble();
              }
              else { Properties.Settings.Default.DoublePage = !Properties.Settings.Default.DoublePage; }
              RefreshMenuItems();
          }


          public void RefreshMenuItems()
          {
              menuItemDoublePage.Checked = Properties.Settings.Default.DoublePage;
          }


          protected override void OnFormClosing(FormClosingEventArgs e)
          {
              base.OnFormClosing(e);

              if(Open)
              BookMarksonFile.Serialize(OpenedFile, (uint)CompressEngine.iCurrentPage);

              Properties.Settings.Default.Save();

              //Close();
              
              
          }

          private void menuItemView_Popup(object sender, EventArgs e)
          {
              menuItemRotate0.Checked = false;
              menuItemRotate90.Checked = false;
              menuItemRotate180.Checked = false;
              menuItemRotate270.Checked = false;

              menuItemSettoNone.Checked = false;
              menuItemSettoWidth.Checked = false;
              menuItemSettoHeight.Checked = false;
              menuItemFitView.Checked = false;

              if (imageEngine != null)
              {
                  if (imageEngine.GetRotation() == 0.0) menuItemRotate0.Checked = true;
                  else if (imageEngine.GetRotation() == 90.0) menuItemRotate90.Checked = true;
                  else if (imageEngine.GetRotation() == 180.0) menuItemRotate180.Checked = true;
                  else if (imageEngine.GetRotation() == 270.0) menuItemRotate270.Checked = true;
              }
              else
              {
                  menuItemRotate0.Checked = true;
              }

              if (Properties.Settings.Default.PageView == ViewSetting.None) menuItemSettoNone.Checked = true;
              else if (Properties.Settings.Default.PageView == ViewSetting.Width) menuItemSettoWidth.Checked = true;
              else if (Properties.Settings.Default.PageView == ViewSetting.Height) menuItemSettoHeight.Checked = true;
              else if (Properties.Settings.Default.PageView == ViewSetting.Fit) menuItemFitView.Checked = true;

         }

          private void menuItemAbout_Click(object sender, EventArgs e)
          { new About().ShowDialog(this); }

          private void menuItemRotate0_Click(object sender, EventArgs e)
          {
              imageEngine.SetRotation(0.0);
          }

          private void menuItemRotate90_Click(object sender, EventArgs e)
          {
              imageEngine.SetRotation(90.0);
          }

          private void menuItemRotate180_Click(object sender, EventArgs e)
          {
              imageEngine.SetRotation(180.0);
          }

          private void menuItemRotate270_Click(object sender, EventArgs e)
          {
              imageEngine.SetRotation(270.0);
          }

          public Image SetImageOpacity(Image image, float opacity)
          {
              try
              {
                  //create a Bitmap the size of the image provided  
                  Bitmap bmp = new Bitmap(image.Width, image.Height);

                  //create a graphics object from the image  
                  using (Graphics gfx = Graphics.FromImage(bmp))
                  {
                      
                      //create a color matrix object  
                      System.Drawing.Imaging.ColorMatrix matrix = new System.Drawing.Imaging.ColorMatrix();

                      //set the opacity  
                      matrix.Matrix33 = opacity;

                      //create image attributes  
                      ImageAttributes attributes = new ImageAttributes();

                      //set the color(opacity) of the image  
                      attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                      //now draw the image  
                      gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                  }
                  return bmp;
              }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
                  return null;
              }
          } 
    }
}
