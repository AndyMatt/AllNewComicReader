namespace AllNewComicReader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItemView = new System.Windows.Forms.MenuItem();
            this.menuItemPageView = new System.Windows.Forms.MenuItem();
            this.menuItemSettoWidth = new System.Windows.Forms.MenuItem();
            this.menuItemSettoHeight = new System.Windows.Forms.MenuItem();
            this.menuItemSettoNone = new System.Windows.Forms.MenuItem();
            this.menuItemFitView = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItemRotate0 = new System.Windows.Forms.MenuItem();
            this.menuItemRotate90 = new System.Windows.Forms.MenuItem();
            this.menuItemRotate180 = new System.Windows.Forms.MenuItem();
            this.menuItemRotate270 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItemDoublePage = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.InputControl = new System.Windows.Forms.Panel();
            this.customInfoBox1 = CustomInfoBox.Instance;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.InputControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(164, 56);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(726, 371);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(475, 433);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 47);
            this.label1.TabIndex = 1;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemView,
            this.menuItem5});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem6,
            this.menuItem7,
            this.menuItem1,
            this.menuItem9,
            this.menuItem3,
            this.menuItem8});
            this.menuItemFile.ShowShortcut = false;
            this.menuItemFile.Text = "File";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.menuItem2.ShowShortcut = false;
            this.menuItem2.Text = "Open Archive...";
            this.menuItem2.Click += new System.EventHandler(this.OpenFile);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "Open Next Archive";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 2;
            this.menuItem7.Text = "Open Previous Archive";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 4;
            this.menuItem9.Text = "-";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 5;
            this.menuItem3.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.menuItem3.ShowShortcut = false;
            this.menuItem3.Text = "Settings";
            this.menuItem3.Click += new System.EventHandler(this.OpenSettings);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 6;
            this.menuItem8.Text = "Exit";
            // 
            // menuItemView
            // 
            this.menuItemView.Index = 1;
            this.menuItemView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPageView,
            this.menuItem15,
            this.menuItem4,
            this.menuItem10,
            this.menuItem11,
            this.menuItem12,
            this.menuItem13,
            this.menuItem14,
            this.menuItemDoublePage});
            this.menuItemView.Text = "View";
            this.menuItemView.Popup += new System.EventHandler(this.menuItemView_Popup);
            // 
            // menuItemPageView
            // 
            this.menuItemPageView.Index = 0;
            this.menuItemPageView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSettoWidth,
            this.menuItemSettoHeight,
            this.menuItemSettoNone,
            this.menuItemFitView});
            this.menuItemPageView.Text = "Page View";
            // 
            // menuItemSettoWidth
            // 
            this.menuItemSettoWidth.Index = 0;
            this.menuItemSettoWidth.Text = "Set to Width";
            this.menuItemSettoWidth.Click += new System.EventHandler(this.menuItemSettoWidth_Click);
            // 
            // menuItemSettoHeight
            // 
            this.menuItemSettoHeight.Index = 1;
            this.menuItemSettoHeight.Text = "Set to Height";
            this.menuItemSettoHeight.Click += new System.EventHandler(this.menuItemSettoHeight_Click);
            // 
            // menuItemSettoNone
            // 
            this.menuItemSettoNone.Index = 2;
            this.menuItemSettoNone.Text = "None";
            this.menuItemSettoNone.Click += new System.EventHandler(this.menuItemSettoNone_Click);
            // 
            // menuItemFitView
            // 
            this.menuItemFitView.Index = 3;
            this.menuItemFitView.Text = "Fit to Screen";
            this.menuItemFitView.Click += new System.EventHandler(this.menuItemFitView_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 1;
            this.menuItem15.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemRotate0,
            this.menuItemRotate90,
            this.menuItemRotate180,
            this.menuItemRotate270});
            this.menuItem15.Text = "Rotate";
            // 
            // menuItemRotate0
            // 
            this.menuItemRotate0.Index = 0;
            this.menuItemRotate0.Text = "0";
            this.menuItemRotate0.Click += new System.EventHandler(this.menuItemRotate0_Click);
            // 
            // menuItemRotate90
            // 
            this.menuItemRotate90.Index = 1;
            this.menuItemRotate90.Text = "90";
            this.menuItemRotate90.Click += new System.EventHandler(this.menuItemRotate90_Click);
            // 
            // menuItemRotate180
            // 
            this.menuItemRotate180.Index = 2;
            this.menuItemRotate180.Text = "180";
            this.menuItemRotate180.Click += new System.EventHandler(this.menuItemRotate180_Click);
            // 
            // menuItemRotate270
            // 
            this.menuItemRotate270.Index = 3;
            this.menuItemRotate270.Text = "270";
            this.menuItemRotate270.Click += new System.EventHandler(this.menuItemRotate270_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 3;
            this.menuItem10.Text = "Next Page";
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 4;
            this.menuItem11.Text = "Previous Page";
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 5;
            this.menuItem12.Text = "First Page";
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 6;
            this.menuItem13.Text = "Last Page";
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 7;
            this.menuItem14.Text = "-";
            // 
            // menuItemDoublePage
            // 
            this.menuItemDoublePage.Index = 8;
            this.menuItemDoublePage.Text = "Double Page";
            this.menuItemDoublePage.Click += new System.EventHandler(this.menuItemDoublePage_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 2;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAbout});
            this.menuItem5.Text = "Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            this.menuItemAbout.Text = "About";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // InputControl
            // 
            this.InputControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputControl.Controls.Add(this.customInfoBox1);
            this.InputControl.Location = new System.Drawing.Point(-3, -1);
            this.InputControl.Name = "InputControl";
            this.InputControl.Size = new System.Drawing.Size(1088, 572);
            this.InputControl.TabIndex = 3;
            this.InputControl.Visible = false;
            // 
            // customInfoBox1
            // 
            this.customInfoBox1.BackColor = System.Drawing.Color.Transparent;
            this.customInfoBox1.BorderColor = System.Drawing.Color.White;
            this.customInfoBox1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customInfoBox1.Location = new System.Drawing.Point(3, 477);
            this.customInfoBox1.Name = "customInfoBox1";
            this.customInfoBox1.Size = new System.Drawing.Size(147, 21);
            this.customInfoBox1.TabIndex = 2;
            this.customInfoBox1.TextInput = " aaaaaaaaaaaaaaaaaaaaaaa                   ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1084, 562);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.InputControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "All-New Comic Reader";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.InputControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItemFile;
        private CustomInfoBox customInfoBox1;
        private System.Windows.Forms.Panel InputControl;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItemView;
        private System.Windows.Forms.MenuItem menuItemPageView;
        private System.Windows.Forms.MenuItem menuItemSettoWidth;
        private System.Windows.Forms.MenuItem menuItemSettoHeight;
        private System.Windows.Forms.MenuItem menuItemSettoNone;
        private System.Windows.Forms.MenuItem menuItemFitView;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItemDoublePage;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItemAbout;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem menuItemRotate0;
        private System.Windows.Forms.MenuItem menuItemRotate90;
        private System.Windows.Forms.MenuItem menuItemRotate180;
        private System.Windows.Forms.MenuItem menuItemRotate270;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.MenuItem menuItem14;
        private System.Windows.Forms.MenuItem menuItem1;
    }
}

