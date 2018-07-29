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
    public partial class KeyInput : Form
    {
        public string CurrentKey;

        public KeyInput(string Input)
        {
            InitializeComponent();

            CurrentKey = Input;
            KeyDown += RecordInput;
            KeyUp += ModifierUp;
            
        }

        void ModifierUp(object sender, KeyEventArgs e)
        {
            string Modifier = "";

            if (e.Control)
                Modifier += "Ctrl+";
            if (e.Alt)
                Modifier += "Alt+";

            label1.Text = Modifier;
        }

        void RecordInput(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.Menu)
            {
                string NewKey = "";

                if (e.Control)
                    NewKey += "Ctrl+";
                if (e.Alt)
                    NewKey += "Alt+";

                NewKey += e.KeyCode;

                SetKey(NewKey);

                Properties.Settings.Default.Save();

                Close();
            }
            else
            {
                string Modifier = "";

                if (e.Control)
                    Modifier += "Ctrl+";
                if (e.Alt)
                    Modifier += "Alt+";

                label1.Text = Modifier;

            }
        }

        void SetKey(string NewKey)
        {
            if (CurrentKey == "NextPage")
                Properties.Settings.Default.InputNextPage = NewKey;
            else if (CurrentKey == "PrevPage")
                Properties.Settings.Default.InputPrevPage = NewKey;
                else if (CurrentKey == "FirstPage")
                Properties.Settings.Default.InputFirstPage = NewKey;
                else if (CurrentKey == "LastPage")
                Properties.Settings.Default.InputLastPage = NewKey;
                else if (CurrentKey == "LoadNext")
                Properties.Settings.Default.InputLoadNext = NewKey;
                else if (CurrentKey == "LoadPrev")
                    Properties.Settings.Default.InputLoadPrev = NewKey;
                else if (CurrentKey == "PageNum")
                    Properties.Settings.Default.InputPageNumber = NewKey;
                else if (CurrentKey == "Fullscreen")
                    Properties.Settings.Default.InputFullscreen = NewKey;
                else if (CurrentKey == "Toolbar")
                    Properties.Settings.Default.InputToolbar = NewKey;
                else if (CurrentKey == "ScrollUp")
                    Properties.Settings.Default.InputScrollUp = NewKey;
                else if (CurrentKey == "ScrollDown")
                    Properties.Settings.Default.InputScrollDown = NewKey;
                else if (CurrentKey == "ScrollLeft")
                    Properties.Settings.Default.InputScrollLeft = NewKey;
                else if (CurrentKey == "ScrollRight")
                    Properties.Settings.Default.InputScrollRight = NewKey;
                else if (CurrentKey == "Rotate")
                    Properties.Settings.Default.InputRotate = NewKey;
                else if (CurrentKey == "PageInfo")
                    Properties.Settings.Default.InputPageInfo = NewKey;
                else if (CurrentKey == "ViewMode")
                    Properties.Settings.Default.InputViewMode = NewKey;
                else if (CurrentKey == "DoublePage")
                    Properties.Settings.Default.InputDoublePage = NewKey;
                 else if (CurrentKey == "Yellow")
                Properties.Settings.Default.InputYellow = NewKey;
                else if (CurrentKey == "Exit")
                    Properties.Settings.Default.InputExit = NewKey;
        }
    }
}
