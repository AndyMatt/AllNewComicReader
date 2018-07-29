using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AllNewComicReader
{
    public partial class Settings : Form
    {
        public KeyDef InputNextPage;
        public KeyDef InputPrevPage;
        public KeyDef InputFirstPage;
        public KeyDef InputLastPage;
        public KeyDef InputLoadNext;
        public KeyDef InputLoadPrev;
        public KeyDef InputPageNumber;
        public KeyDef InputFullscreen;
        public KeyDef InputToolbar;
        public KeyDef InputScrollUp;
        public KeyDef InputScrollDown;
        public KeyDef InputScrollLeft;
        public KeyDef InputScrollRight;
        public KeyDef InputRotate;
        public KeyDef InputPageInfo;
        public KeyDef InputViewMode;
        public KeyDef InputDoublePage;
        public KeyDef InputExit;
        public KeyDef InputYellow;

        public bool DoublePage;
        public bool bRefresh;


        KeyInput NewInput;
        public Settings()
        {
            
            InitializeComponent();
            SaturationtrackBar.ValueChanged += new EventHandler(ColortrackBar_ValueChanged);
            BrightnesstrackBar.ValueChanged += new EventHandler(ColortrackBar_ValueChanged);
            HuetrackBar.ValueChanged += new EventHandler(ColortrackBar_ValueChanged);
            trackBarScroll.ValueChanged += new EventHandler(ColortrackBar_ValueChanged);

            bRefresh = false;
            DoublePage = true;
            ColortrackBar_ValueChanged(this, new EventArgs());
            RefreshSettings();
        }

        public void RefreshSettings()
        {
            RefreshInput();

            PopulateComboBox(comboBoxPageView, default(ViewSetting));
            PopulateComboBox(comboBoxSample, default(ImageMagick.FilterType));
            PopulateComboBox(comboBoxPixelInterp, default(ImageMagick.PixelInterpolateMethod));

            if (Properties.Settings.Default.FastRender)
            {
                comboBoxSample.Enabled = false;
                comboBoxPixelInterp.Enabled = false;
            }

            checkBoxFastSample.Checked = Properties.Settings.Default.FastRender;

            comboBoxSample.Text = Properties.Settings.Default.ImageFilter.ToString();
            comboBoxPixelInterp.Text = Properties.Settings.Default.PixelInterpolation.ToString();
            comboBoxPageView.Text = Properties.Settings.Default.PageView.ToString();

            checkBoxDoublePage.Checked = Properties.Settings.Default.DoublePage;

            checkBoxNativeSpread.Enabled = Properties.Settings.Default.DoublePage;
            checkBoxNativeSpread.Checked = Properties.Settings.Default.SupressForSpread;

            checkBoxYellowCorrect.Checked = Properties.Settings.Default.AutoYellowBalance;

            trackBarScroll.Value = (int)Properties.Settings.Default.ScrollAmount;
            textBoxScroll.Text = ((int)Properties.Settings.Default.ScrollAmount).ToString();

            checkBoxBookmarkAsk.Enabled = Properties.Settings.Default.Bookmarks;
            checkBoxSavePosition.Checked = Properties.Settings.Default.Bookmarks;
            checkBoxBookmarkAsk.Checked = Properties.Settings.Default.BookmarkAsk;

            checkBoxColorControl.Checked = Properties.Settings.Default.HueSaturation;

            checkBoxPageInfo.Checked = Properties.Settings.Default.TTPageInfo;
            checkBoxPageNum.Checked = Properties.Settings.Default.TTPageNumber;

            checkBoxHideMouse.Checked = Properties.Settings.Default.HideCursor;

            checkBoxColorControl.Checked = Properties.Settings.Default.HueSaturation;

            checkBoxPagePopup.Checked = Properties.Settings.Default.PageContolPopup;

            HuetrackBar.Value = (int)Properties.Settings.Default.Hue;
            SaturationtrackBar.Value = (int)Properties.Settings.Default.Saturation;
            BrightnesstrackBar.Value = (int)Properties.Settings.Default.Brightness;

            HuetextBox.Text = ((int)(HuetrackBar.Value * 1.8) - 180).ToString();
            SaturationtextBox.Text = SaturationtrackBar.Value.ToString();
            BrightnesstextBox.Text = BrightnesstrackBar.Value.ToString();

            SetHueEnable();
        }

        void PopulateComboBox(ComboBox combo, Enum EnumType)
        {
            combo.Items.Clear();
            Array values = Enum.GetValues(EnumType.GetType());

            foreach (var val in values)
                combo.Items.Add(val.ToString());
        }

        public void ColortrackBar_ValueChanged(object sender, EventArgs e)
        {
            HuetextBox.Text = ((int)(HuetrackBar.Value * 1.8) - 180).ToString();
            SaturationtextBox.Text = SaturationtrackBar.Value.ToString();
            BrightnesstextBox.Text = BrightnesstrackBar.Value.ToString();
            textBoxScroll.Text = trackBarScroll.Value.ToString();
        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            Button referer = sender as Button;

            if(referer == buttonNextPage)
                NewInput = new KeyInput("NextPage");
            else if (referer == buttonPreviousPage)
                NewInput = new KeyInput("PrevPage");
            else if (referer == buttonFirstPage)
                NewInput = new KeyInput("FirstPage");
            else if (referer == buttonLastPage)
                NewInput = new KeyInput("LastPage");
            else if (referer == buttonLoadNext)
                NewInput = new KeyInput("LoadNext");
            else if (referer == buttonLoadPrev)
                NewInput = new KeyInput("LoadPrev");
            else if (referer == buttonPageNumber)
                NewInput = new KeyInput("PageNum");
            else if (referer == buttonFullScreen)
                NewInput = new KeyInput("Fullscreen");
            else if (referer == buttonToolBar)
                NewInput = new KeyInput("Toolbar");
            else if (referer == buttonScrollUp)
                NewInput = new KeyInput("ScrollUp");
            else if (referer == buttonScrollDown)
                NewInput = new KeyInput("ScrollDown");
            else if (referer == buttonScrollLeft)
                NewInput = new KeyInput("ScrollLeft");
            else if (referer == buttonScrollRight)
                NewInput = new KeyInput("ScrollRight");
            else if (referer == buttonRotate)
                NewInput = new KeyInput("Rotate");
            else if (referer == buttonPageInfo)
                NewInput = new KeyInput("PageInfo");
            else if (referer == buttonViewMode)
                NewInput = new KeyInput("ViewMode");
            else if (referer == buttonDoublePage)
                NewInput = new KeyInput("DoublePage");
            else if (referer == buttonAutoYellow)
                NewInput = new KeyInput("Yellow");
            else if (referer == buttonExit)
                NewInput = new KeyInput("Exit");


            NewInput.Show();
            NewInput.FormClosing += InputRecived;
        }

        void InputRecived(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();

            RefreshInput();
                
        }

        private void RefreshInput()
        {
            
            textBoxNextPage.Text = Properties.Settings.Default.InputNextPage;
            textBoxPrevPage.Text = Properties.Settings.Default.InputPrevPage;
            textBoxFirstPage.Text = Properties.Settings.Default.InputFirstPage;
            textBoxLastPage.Text = Properties.Settings.Default.InputLastPage;
            textBoxLoadNext.Text = Properties.Settings.Default.InputLoadNext;
            textBoxLoadPrev.Text = Properties.Settings.Default.InputLoadPrev;
            textBoxPageToggle.Text = Properties.Settings.Default.InputPageNumber;
            textBoxFullScreen.Text = Properties.Settings.Default.InputFullscreen;
            textBoxToolbar.Text = Properties.Settings.Default.InputToolbar;
            textBoxScrollUp.Text = Properties.Settings.Default.InputScrollUp;
            textBoxScrollDown.Text = Properties.Settings.Default.InputScrollDown;
            textBoxScrollLeft.Text = Properties.Settings.Default.InputScrollLeft;
            textBoxScrollRight.Text = Properties.Settings.Default.InputScrollRight;
            textBoxRotate.Text = Properties.Settings.Default.InputRotate;
            textBoxPageInfo.Text = Properties.Settings.Default.InputPageInfo;
            textBoxViewMode.Text = Properties.Settings.Default.InputViewMode;
            textBoxDoublePage.Text = Properties.Settings.Default.InputDoublePage;
            textBoxExit.Text = Properties.Settings.Default.InputExit;
            textBoxAutoYellow.Text = Properties.Settings.Default.InputYellow;

            InputNextPage = SetInput(InputNextPage, Properties.Settings.Default.InputNextPage);
            InputPrevPage = SetInput(InputPrevPage, Properties.Settings.Default.InputPrevPage);
            InputFirstPage = SetInput(InputFirstPage, Properties.Settings.Default.InputFirstPage);
            InputLastPage = SetInput(InputLastPage, Properties.Settings.Default.InputLastPage);
            InputLoadNext = SetInput(InputLoadNext, Properties.Settings.Default.InputLoadNext);
            InputLoadPrev = SetInput(InputLoadPrev, Properties.Settings.Default.InputLoadPrev);
            InputPageNumber = SetInput(InputPageNumber, Properties.Settings.Default.InputPageNumber);
            InputFullscreen = SetInput(InputFullscreen, Properties.Settings.Default.InputFullscreen);
            InputToolbar = SetInput(InputToolbar, Properties.Settings.Default.InputToolbar);
            InputScrollUp = SetInput(InputScrollUp, Properties.Settings.Default.InputScrollUp);
            InputScrollDown = SetInput(InputScrollDown, Properties.Settings.Default.InputScrollDown);
            InputScrollLeft = SetInput(InputScrollLeft, Properties.Settings.Default.InputScrollLeft);
            InputScrollRight = SetInput(InputScrollRight, Properties.Settings.Default.InputScrollRight);
            InputRotate = SetInput(InputRotate, Properties.Settings.Default.InputRotate);
            InputPageInfo = SetInput(InputPageInfo, Properties.Settings.Default.InputPageInfo);
            InputViewMode = SetInput(InputViewMode, Properties.Settings.Default.InputViewMode);
            InputDoublePage = SetInput(InputDoublePage, Properties.Settings.Default.InputDoublePage);
            InputExit = SetInput(InputExit, Properties.Settings.Default.InputExit);
            InputYellow = SetInput(InputYellow, Properties.Settings.Default.InputYellow);

            PopulateComboBox(comboBoxM1Single, default(MouseControls));
            PopulateComboBox(comboBoxM2Single, default(MouseControls));
            PopulateComboBox(comboBoxM3Single, default(MouseControls));
            PopulateComboBox(comboBoxM4Single, default(MouseControls));
            PopulateComboBox(comboBoxM5Single, default(MouseControls));

            PopulateComboBox(comboBoxM1Double, default(MouseControls));
            PopulateComboBox(comboBoxM2Double, default(MouseControls));
            PopulateComboBox(comboBoxM3Double, default(MouseControls));
            PopulateComboBox(comboBoxM4Double, default(MouseControls));
            PopulateComboBox(comboBoxM5Double, default(MouseControls));

            PopulateComboBox(comboBoxM1Hold, default(MouseHoldControls));
            PopulateComboBox(comboBoxM2Hold, default(MouseHoldControls));
            PopulateComboBox(comboBoxM3Hold, default(MouseHoldControls));
            PopulateComboBox(comboBoxM4Hold, default(MouseHoldControls));
            PopulateComboBox(comboBoxM5Hold, default(MouseHoldControls));

            comboBoxM1Single.Text = Properties.Settings.Default.Mouse1Click;
            comboBoxM2Single.Text = Properties.Settings.Default.Mouse2Click;
            comboBoxM3Single.Text = Properties.Settings.Default.Mouse3Click;
            comboBoxM4Single.Text = Properties.Settings.Default.Mouse4Click;
            comboBoxM5Single.Text = Properties.Settings.Default.Mouse5Click;

            comboBoxM1Double.Text = Properties.Settings.Default.Mouse1Double;
            comboBoxM2Double.Text = Properties.Settings.Default.Mouse2Double;
            comboBoxM3Double.Text = Properties.Settings.Default.Mouse3Double;
            comboBoxM4Double.Text = Properties.Settings.Default.Mouse4Double;
            comboBoxM5Double.Text = Properties.Settings.Default.Mouse5Double;

            comboBoxM1Hold.Text = Properties.Settings.Default.Mouse1Hold;
            comboBoxM2Hold.Text = Properties.Settings.Default.Mouse2Hold;
            comboBoxM3Hold.Text = Properties.Settings.Default.Mouse3Hold;
            comboBoxM4Hold.Text = Properties.Settings.Default.Mouse4Hold;
            comboBoxM5Hold.Text = Properties.Settings.Default.Mouse5Hold;
        }

        private KeyDef SetInput(KeyDef key, String input)
        {
            key = new KeyDef();

            if (input.StartsWith("Ctrl+"))
            {
                input = input.Remove(0,5);
                key.Ctrl = true;
            }

            if (input.StartsWith("Alt+"))
            {
                input = input.Remove(0,4);
                key.Alt = true;
            }

            Enum.TryParse<Keys>(input, out key.Key);

            return key; 
        }
        
        void SaveSetting()
        {
            ViewSetting def = (ViewSetting)Enum.Parse(typeof(ViewSetting), comboBoxPageView.Text);
            Properties.Settings.Default.PageView = def;

            ImageMagick.FilterType type = (ImageMagick.FilterType)Enum.Parse(typeof(ImageMagick.FilterType), comboBoxSample.Text);
            Properties.Settings.Default.ImageFilter = type;

            ImageMagick.PixelInterpolateMethod pix = (ImageMagick.PixelInterpolateMethod)Enum.Parse(typeof(ImageMagick.PixelInterpolateMethod), comboBoxPixelInterp.Text);
            Properties.Settings.Default.PixelInterpolation = pix;

            Properties.Settings.Default.FastRender = checkBoxFastSample.Checked;

            Properties.Settings.Default.DoublePage = checkBoxDoublePage.Checked;

            Properties.Settings.Default.SupressForSpread = checkBoxNativeSpread.Checked;

            Properties.Settings.Default.ScrollAmount = trackBarScroll.Value;

            Properties.Settings.Default.AutoYellowBalance = checkBoxYellowCorrect.Checked;

            Properties.Settings.Default.Bookmarks   = checkBoxSavePosition.Checked;
            Properties.Settings.Default.BookmarkAsk = checkBoxBookmarkAsk.Checked;

            Properties.Settings.Default.HideCursor = checkBoxHideMouse.Checked;

            Properties.Settings.Default.TTPageInfo = checkBoxPageInfo.Checked;
            Properties.Settings.Default.TTPageNumber = checkBoxPageNum.Checked;

            Properties.Settings.Default.Hue = HuetrackBar.Value;
            Properties.Settings.Default.Saturation = SaturationtrackBar.Value;
            Properties.Settings.Default.Brightness = BrightnesstrackBar.Value;
            Properties.Settings.Default.HueSaturation = checkBoxColorControl.Checked;

            Properties.Settings.Default.PageContolPopup = checkBoxPagePopup.Checked;
            SaveMouseControls();

        }

        void SaveMouseControls()
        {
            
            Properties.Settings.Default.Mouse1Click= comboBoxM1Single.Text;
            Properties.Settings.Default.Mouse2Click= comboBoxM2Single.Text;
            Properties.Settings.Default.Mouse3Click= comboBoxM3Single.Text;
            Properties.Settings.Default.Mouse4Click= comboBoxM4Single.Text;
            Properties.Settings.Default.Mouse5Click= comboBoxM5Single.Text;
                                                                          
            Properties.Settings.Default.Mouse1Double= comboBoxM1Double.Text;
            Properties.Settings.Default.Mouse2Double= comboBoxM2Double.Text;
            Properties.Settings.Default.Mouse3Double= comboBoxM3Double.Text;
            Properties.Settings.Default.Mouse4Double= comboBoxM4Double.Text;
            Properties.Settings.Default.Mouse5Double= comboBoxM5Double.Text;

            Properties.Settings.Default.Mouse1Hold=comboBoxM1Hold.Text;
            Properties.Settings.Default.Mouse2Hold=comboBoxM2Hold.Text;
            Properties.Settings.Default.Mouse3Hold=comboBoxM3Hold.Text;
            Properties.Settings.Default.Mouse4Hold=comboBoxM4Hold.Text;
            Properties.Settings.Default.Mouse5Hold = comboBoxM5Hold.Text;
        }                                         

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SaveSetting();
            bRefresh = true;
            Properties.Settings.Default.Save();
            Hide();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            e.Cancel = true;
            Hide();
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxNativeSpread.Enabled = checkBoxDoublePage.Checked;
            checkBoxBookmarkAsk.Enabled = checkBoxSavePosition.Checked;

            SetHueEnable();
        }

        private void SetHueEnable()
        {
            HuetextBox.Enabled = checkBoxColorControl.Checked;
            HuetrackBar.Enabled = checkBoxColorControl.Checked;
            BrightnesstextBox.Enabled = checkBoxColorControl.Checked;
            BrightnesstrackBar.Enabled = checkBoxColorControl.Checked;
            SaturationtextBox.Enabled = checkBoxColorControl.Checked;
            SaturationtrackBar.Enabled = checkBoxColorControl.Checked;
        }

        private void checkBoxFastSample_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxPixelInterp.Enabled = !checkBoxFastSample.Checked;
            comboBoxSample.Enabled = !checkBoxFastSample.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            RefreshSettings();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }


    }
}
