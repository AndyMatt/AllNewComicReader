using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace AllNewComicReader
{
    public enum MouseControls
    {
        None,
        NextPage,
        PreviousPage,
        FirstPage,
        LastPage,
        NextArchive,
        PreviousArchive,
        ContextMenu,
        Rotate,
        YellowToggle,
        HueSaturationToggle,
        ChangeViewMode,
        DoublePageToggle,
        Exit
    }

    public enum MouseHoldControls
    {
        None,
        ScrollUp,
        ScrollDown,
        ScrollLeft,
        ScrollRight,
        ContextMenu,
        MoveImage, 
    }

    class Input
    {
        public bool bDownKey;
        public bool bUpKey;
        public bool bLeftKey;
        public bool bRightKey;
        public bool bMoveActive;
        public int iMouseScroll;

        //public int TextAlpha = 1500;

        Point pMouseLastPos;
        public Point pMouseDelta;
        Settings settings;
        Form form;
        MainMenu mMenu;
        ImageEngine image;
        PictureBox pPictureBox;
        CustomInfoBox Info;
        Form1 GlobalFunctions;

        bool MouseonPictureBox;
        Timer MouseTimer;
        public TimeSpan TimeoutToHide;
        public DateTime LastMouseMove;
        public bool IsHidden;

        public Input(Form refForm, MainMenu Menu, PictureBox tex,Settings set, Form1 refer)
        {
            GlobalFunctions = refer;
            settings = set;
            mMenu = Menu;
            form = refForm;
            pPictureBox = tex;
            MouseonPictureBox = false;
            pMouseLastPos = new Point();

            form.KeyDown += new KeyEventHandler(Input_KeyDown);
            form.KeyUp += new KeyEventHandler(Input_KeyUp);

            form.MouseWheel += new MouseEventHandler(Input_MouseWheel);
            pPictureBox.MouseDown += new MouseEventHandler(Input_MouseDown);
            pPictureBox.MouseMove += new MouseEventHandler(Input_MouseMove);
            pPictureBox.MouseUp += new MouseEventHandler(Input_MouseUp);
            pPictureBox.MouseLeave += new EventHandler(Input_MouseLeave);
            pPictureBox.MouseEnter += new EventHandler(Input_MouseEnter);
            pPictureBox.MouseClick += new MouseEventHandler(Input_MouseClick);
            pPictureBox.MouseDoubleClick += new MouseEventHandler(Input_MouseDoubleClick);

            MouseTimer = new Timer();
            MouseTimer.Interval = 1000;
            MouseTimer.Tick += MouseTimer_Tick;
            MouseTimer.Start();
            TimeoutToHide = TimeSpan.FromSeconds(1);
        }

        public void Setup(ImageEngine img, CustomInfoBox info)
        {
            Info = info;
            image = img;
        }
        
        public void Input_MouseDown(object sender, MouseEventArgs e)
        {
            pMouseLastPos = new Point(e.X, e.Y);

            if (e.Button == MouseButtons.Left)
                ProcessMouseFunction(Properties.Settings.Default.Mouse1Hold);

            if (e.Button == MouseButtons.Right)
                ProcessMouseFunction(Properties.Settings.Default.Mouse3Hold);

            if (e.Button == MouseButtons.Middle)
                ProcessMouseFunction(Properties.Settings.Default.Mouse2Hold);

            if (e.Button == MouseButtons.XButton1)
                ProcessMouseFunction(Properties.Settings.Default.Mouse4Hold);

            if (e.Button == MouseButtons.XButton2)
                ProcessMouseFunction(Properties.Settings.Default.Mouse5Hold);

        }

        public void Input_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ProcessMouseFunction(Properties.Settings.Default.Mouse1Click);
            else if (e.Button == MouseButtons.Middle)
                ProcessMouseFunction(Properties.Settings.Default.Mouse2Click);
            else if (e.Button == MouseButtons.Right)
                ProcessMouseFunction(Properties.Settings.Default.Mouse3Click);
            else if (e.Button == MouseButtons.XButton1)
                ProcessMouseFunction(Properties.Settings.Default.Mouse4Click);
            else if (e.Button == MouseButtons.XButton2)
                ProcessMouseFunction(Properties.Settings.Default.Mouse5Click);
        }

        public void Input_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ProcessMouseFunction(Properties.Settings.Default.Mouse1Double);
            else if (e.Button == MouseButtons.Right)
                ProcessMouseFunction(Properties.Settings.Default.Mouse3Double);
            else if (e.Button == MouseButtons.Middle)
                ProcessMouseFunction(Properties.Settings.Default.Mouse2Double);
            else if (e.Button == MouseButtons.XButton1)
                ProcessMouseFunction(Properties.Settings.Default.Mouse4Double);
            else if (e.Button == MouseButtons.XButton2)
                ProcessMouseFunction(Properties.Settings.Default.Mouse5Double);
        }

        public void Input_MouseEnter(object sender, EventArgs e)
        {
            MouseonPictureBox = true;
        }

        public void Input_MouseLeave(object sender, EventArgs e)
        {
            MouseonPictureBox = false;
                //Cursor.Show();
        }

        public void Input_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bMoveActive = false;
                Console.WriteLine("Up");
            }

            if(e.Button == MouseButtons.Left)
                CheckMouseholdUp(Properties.Settings.Default.Mouse1Hold);

            if(e.Button == MouseButtons.Right)
                CheckMouseholdUp(Properties.Settings.Default.Mouse3Hold);

            if(e.Button == MouseButtons.Middle)
                CheckMouseholdUp(Properties.Settings.Default.Mouse2Hold);

            if(e.Button == MouseButtons.XButton1)
                CheckMouseholdUp(Properties.Settings.Default.Mouse4Hold);

            if(e.Button == MouseButtons.XButton2)
                CheckMouseholdUp(Properties.Settings.Default.Mouse5Hold);
       
        }

        public void Input_MouseMove(object sender, MouseEventArgs e)
        {
            if (bMoveActive)
            {
                Point newDelta = new Point(e.X - pMouseLastPos.X, e.Y - pMouseLastPos.Y);
                pMouseDelta = new Point(pMouseDelta.X + newDelta.X,pMouseDelta.Y + newDelta.Y);
            }

            pMouseLastPos = new Point(e.X, e.Y);

            if(Properties.Settings.Default.HideCursor)
            LastMouseMove = DateTime.Now;

            if (IsHidden)
            {
                MouseTimer.Start();
                Cursor.Show();
                IsHidden = false;
            }
            
        }

        public void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (ImageEngine.IsLoading)
               return;

            if (CheckInput(e, settings.InputScrollDown))
                bDownKey = true;

            if (CheckInput(e, settings.InputScrollUp))
                bUpKey = true;

            if (CheckInput(e, settings.InputScrollLeft))
                bLeftKey = true;

            if (CheckInput(e, settings.InputScrollRight))
                bRightKey = true;

            if (CheckInput(e, settings.InputYellow))
                image.ToggleYellow();

            /* Menu Control
            if (e.KeyCode == Keys.M)
            {
                if (mMenu.MenuItems[0].Visible)
                {
                    Properties.Settings.Default.MainMenuOffset = 0; 
                    mMenu.MenuItems[0].Visible = false;
                    mMenu.MenuItems[1].Visible = false;
                }
                else
                {
                    Properties.Settings.Default.MainMenuOffset = 20;
                    mMenu.MenuItems[0].Visible = true;
                    mMenu.MenuItems[1].Visible = true;
                }

            }
            */

            if (CheckInput(e, settings.InputExit))
                Environment.Exit(0);

            if (CheckInput(e, settings.InputFullscreen))
                FunctionFullscreen();


            if(CheckInput(e, settings.InputNextPage))
                FunctionNextPage();

            if (CheckInput(e, settings.InputLastPage))
                FunctionLastPage();

            if (CheckInput(e, settings.InputFirstPage))
                FunctionFirstPage();


            if (CheckInput(e, settings.InputPrevPage))
                FunctionPreviousPage();

            if (CheckInput(e, settings.InputLoadNext))
                FunctionNextArchive();

            if (CheckInput(e, settings.InputLoadPrev))
                FunctionPreviousArchive();  

            if (CheckInput(e, settings.InputPageNumber))
                FunctionNumberToggle();

            if (CheckInput(e, settings.InputPageInfo))
                FunctionInfoToggle();

            if (CheckInput(e, settings.InputRotate))
                if (image != null)
                image.Rotate();

            if (CheckInput(e, settings.InputViewMode))
                if (image != null)
                image.ToggleViewMode();

            if (CheckInput(e, settings.InputDoublePage))
            {
                if (image != null)
                image.ToggleDouble();
            }
            
        }

        bool CheckInput(KeyEventArgs e, KeyDef Key)
        {
            if (e.KeyCode == Key.Key)
            {
                if (e.Alt != Key.Alt || e.Control != Key.Ctrl)
                    return false;
                else
                    return true;
            }

            return false;
        }

        public void Input_KeyUp(object sender, KeyEventArgs e)
        {
            if (CheckInput(e, settings.InputScrollDown))
                bDownKey = false;

            if (CheckInput(e, settings.InputScrollUp))
                bUpKey = false;

            if (CheckInput(e, settings.InputScrollLeft))
                bLeftKey = false;

            if (CheckInput(e, settings.InputScrollRight))
                bRightKey = false;
        }

        public void Input_MouseWheel(object sender, MouseEventArgs e)
        {
            iMouseScroll = e.Delta;

        }

        private void MouseTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan elaped = DateTime.Now - LastMouseMove;

            if (form.Focused && MouseonPictureBox)
            {
                if (elaped >= TimeoutToHide && Properties.Settings.Default.HideCursor)
                {
                    Cursor.Hide();
                    MouseTimer.Stop();
                    IsHidden = true;
                }
            }
        }

        void ProcessMouseFunction(string Command)
        {
            if (Command == MouseControls.FirstPage.ToString())
                FunctionFirstPage();
            else if (Command == MouseControls.LastPage.ToString())
                FunctionFirstPage();
            else if (Command == MouseControls.NextPage.ToString())
                FunctionNextPage();
            else if (Command == MouseControls.PreviousPage.ToString())
                FunctionPreviousPage();
            else if (Command == MouseControls.NextArchive.ToString())
                FunctionNextArchive();
            else if (Command == MouseControls.PreviousArchive.ToString())
                FunctionPreviousArchive();
            else if (Command == MouseControls.ContextMenu.ToString())
                GlobalFunctions.ShowContextMenu(this, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
            else if (Command == MouseControls.Rotate.ToString())
                image.Rotate();
            else if (Command == MouseControls.YellowToggle.ToString())
                image.ToggleYellow();
            else if (Command == MouseControls.HueSaturationToggle.ToString())
                image.ToggleHue();
            else if (Command == MouseControls.ChangeViewMode.ToString())
                image.ToggleViewMode();
            else if (Command == MouseControls.DoublePageToggle.ToString())
                image.ToggleDouble();
            else if (Command == MouseControls.Exit.ToString())
                Environment.Exit(0);
            
            if (Command == MouseHoldControls.ScrollDown.ToString())
                bDownKey = true;
            else if (Command == MouseHoldControls.ScrollUp.ToString())
                bUpKey = true;
            else if (Command == MouseHoldControls.ScrollLeft.ToString())
                bLeftKey = true;
            else if (Command == MouseHoldControls.ScrollRight.ToString())
                bRightKey = true;
            else if (Command == MouseHoldControls.MoveImage.ToString())
                bMoveActive = true;

            GlobalFunctions.RefreshMenuItems();
        }


        void CheckMouseholdUp(string Command)
        {
            if (Command == MouseHoldControls.ScrollDown.ToString())
                bDownKey = false;
            if (Command == MouseHoldControls.ScrollUp.ToString())
                bUpKey = false;
            if (Command == MouseHoldControls.ScrollLeft.ToString())
                bLeftKey = false;
            if (Command == MouseHoldControls.ScrollRight.ToString())
                bRightKey = false;
            if (Command == MouseHoldControls.MoveImage.ToString())
                bMoveActive = false;
        }

        //Control Functions
        public void FunctionNextPage()
        {
            if (image == null)
                return;
            if (Properties.Settings.Default.DoublePage)
                image.NextDoubleImage();
            else
                image.NextImage();
            //RefreshHud();
        }

        public void FunctionPreviousPage()
        {
            if (image == null)
                return;
            if (Properties.Settings.Default.DoublePage)
                image.PreviousDoubleImage();
            else
                image.PreviousImage();
            //RefreshHud();
        }

        void FunctionNumberToggle()
        {
            if (!Properties.Settings.Default.TTPageNumber)
            {
                Properties.Settings.Default.TTPageNumber = true;
                UserControlPopup.Setup("Page Number Popup Enabled");
            }
            else
            {
                Properties.Settings.Default.TTPageNumber = false;
                UserControlPopup.Setup("Page Number Popup Disabled");
            }
        }

        void FunctionInfoToggle()
        {
            if (!Properties.Settings.Default.TTPageInfo)
            {
                Properties.Settings.Default.TTPageInfo = true;
                UserControlPopup.Setup("Page Info Popup Enabled");
            }
            else
            {
                Properties.Settings.Default.TTPageInfo = false;
                UserControlPopup.Setup("Page Info Popup Disabled");
            }
        }

        public void FunctionLastPage()
        {
            if (image == null)
                return;
            image.LastImage();
            //RefreshHud();
        }

        public void FunctionFirstPage()
        {
            if (image == null)
                return;

            image.FirstImage();
            //RefreshHud();
        }

        void FunctionNextArchive()
        {
            GlobalFunctions.LoadNextFile(this, new EventArgs());

        }

        public void FunctionPreviousArchive()
        {
            GlobalFunctions.LoadPreviousFile(this, new EventArgs());
        }

        public void FunctionFullscreen()
        {
            if (form.FormBorderStyle == System.Windows.Forms.FormBorderStyle.None)
            {
                Properties.Settings.Default.Fullscreen = false;
                
                if (image != null)
                image.blackout += 15;

                form.Opacity = 0;
                pPictureBox.Refresh();
                form.Menu = mMenu;
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                form.WindowState = FormWindowState.Normal;
                
                form.Opacity = 100;

                if (form.Location.Y < 0)
                    form.Location = new Point(form.Location.X, 0);

                //UserControlPopup.Setup("Rotate 90°");
                
            }
            else
            {
                Properties.Settings.Default.Fullscreen = true;
                
                if(image !=null)
                image.blackout += 15;

                form.Menu = null;

                form.Opacity = 0;

                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                if (form.WindowState == FormWindowState.Maximized)
                {
                    
                    form.WindowState = FormWindowState.Normal;
                }
                form.WindowState = FormWindowState.Maximized;
                form.Opacity = 100;
                pPictureBox.Refresh();
                
            }

            Properties.Settings.Default.Save();
        }


    }
}
