namespace FileCompare
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainerMain = new SplitContainer();
            leftBottomPanel = new Panel();
            lvwLeftDir = new ListView();
            leftTopPanel = new Panel();
            btnCopyFromLeft = new Button();
            lblAppName = new Label();
            txtLeftDir = new TextBox();
            btnLeftDir = new Button();
            rightBottomPanel = new Panel();
            lvwRightDir = new ListView();
            rightTopPanel = new Panel();
            btnCopyFromRight = new Button();
            txtListDir = new TextBox();
            btnRightDir = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            leftBottomPanel.SuspendLayout();
            leftTopPanel.SuspendLayout();
            rightBottomPanel.SuspendLayout();
            rightTopPanel.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.BorderStyle = BorderStyle.FixedSingle;
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerMain.Location = new Point(12, 12);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(leftBottomPanel);
            splitContainerMain.Panel1.Controls.Add(leftTopPanel);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(rightBottomPanel);
            splitContainerMain.Panel2.Controls.Add(rightTopPanel);
            splitContainerMain.Size = new Size(1234, 644);
            splitContainerMain.SplitterDistance = 617;
            splitContainerMain.SplitterWidth = 12;
            splitContainerMain.TabIndex = 0;
            // 
            // leftBottomPanel
            // 
            leftBottomPanel.Controls.Add(lvwLeftDir);
            leftBottomPanel.Dock = DockStyle.Fill;
            leftBottomPanel.Location = new Point(0, 128);
            leftBottomPanel.Name = "leftBottomPanel";
            leftBottomPanel.Padding = new Padding(8);
            leftBottomPanel.Size = new Size(615, 514);
            leftBottomPanel.TabIndex = 0;
            // 
            // lvwLeftDir
            // 
            lvwLeftDir.Dock = DockStyle.Fill;
            lvwLeftDir.FullRowSelect = true;
            lvwLeftDir.Location = new Point(8, 8);
            lvwLeftDir.Name = "lvwLeftDir";
            lvwLeftDir.Size = new Size(599, 498);
            lvwLeftDir.TabIndex = 0;
            lvwLeftDir.UseCompatibleStateImageBehavior = false;
            lvwLeftDir.View = View.Details;
            // 
            // leftTopPanel
            // 
            leftTopPanel.Controls.Add(btnCopyFromLeft);
            leftTopPanel.Controls.Add(lblAppName);
            leftTopPanel.Controls.Add(txtLeftDir);
            leftTopPanel.Controls.Add(btnLeftDir);
            leftTopPanel.Dock = DockStyle.Top;
            leftTopPanel.Location = new Point(0, 0);
            leftTopPanel.Name = "leftTopPanel";
            leftTopPanel.Padding = new Padding(8);
            leftTopPanel.Size = new Size(615, 128);
            leftTopPanel.TabIndex = 1;
            // 
            // btnCopyFromLeft
            // 
            btnCopyFromLeft.Anchor = AnchorStyles.None;
            btnCopyFromLeft.Font = new Font("Pretendard JP Variable", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnCopyFromLeft.Location = new Point(520, 11);
            btnCopyFromLeft.Name = "btnCopyFromLeft";
            btnCopyFromLeft.Size = new Size(84, 45);
            btnCopyFromLeft.TabIndex = 0;
            btnCopyFromLeft.Text = ">>>";
            btnCopyFromLeft.Click += btnCopyFromLeft_Click;
            // 
            // lblAppName
            // 
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("Pretendard JP Variable Medium", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAppName.Location = new Point(12, 8);
            lblAppName.Name = "lblAppName";
            lblAppName.Size = new Size(255, 48);
            lblAppName.TabIndex = 0;
            lblAppName.Text = "File Compare";
            // 
            // txtLeftDir
            // 
            txtLeftDir.Font = new Font("Pretendard JP Variable", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLeftDir.Location = new Point(12, 70);
            txtLeftDir.Name = "txtLeftDir";
            txtLeftDir.Size = new Size(473, 46);
            txtLeftDir.TabIndex = 1;
            // 
            // btnLeftDir
            // 
            btnLeftDir.AutoSize = true;
            btnLeftDir.Font = new Font("Pretendard JP Variable", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnLeftDir.Location = new Point(491, 71);
            btnLeftDir.Name = "btnLeftDir";
            btnLeftDir.Size = new Size(113, 46);
            btnLeftDir.TabIndex = 2;
            btnLeftDir.Text = "폴더 선택";
            btnLeftDir.Click += btnLeftDir_Click;
            // 
            // rightBottomPanel
            // 
            rightBottomPanel.Controls.Add(lvwRightDir);
            rightBottomPanel.Dock = DockStyle.Fill;
            rightBottomPanel.Location = new Point(0, 128);
            rightBottomPanel.Name = "rightBottomPanel";
            rightBottomPanel.Padding = new Padding(8);
            rightBottomPanel.Size = new Size(603, 514);
            rightBottomPanel.TabIndex = 0;
            // 
            // lvwRightDir
            // 
            lvwRightDir.Dock = DockStyle.Fill;
            lvwRightDir.FullRowSelect = true;
            lvwRightDir.Location = new Point(8, 8);
            lvwRightDir.Name = "lvwRightDir";
            lvwRightDir.Size = new Size(587, 498);
            lvwRightDir.TabIndex = 0;
            lvwRightDir.UseCompatibleStateImageBehavior = false;
            lvwRightDir.View = View.Details;
            // 
            // rightTopPanel
            // 
            rightTopPanel.Controls.Add(btnCopyFromRight);
            rightTopPanel.Controls.Add(txtListDir);
            rightTopPanel.Controls.Add(btnRightDir);
            rightTopPanel.Dock = DockStyle.Top;
            rightTopPanel.Location = new Point(0, 0);
            rightTopPanel.Name = "rightTopPanel";
            rightTopPanel.Padding = new Padding(8);
            rightTopPanel.Size = new Size(603, 128);
            rightTopPanel.TabIndex = 1;
            // 
            // btnCopyFromRight
            // 
            btnCopyFromRight.Anchor = AnchorStyles.None;
            btnCopyFromRight.Font = new Font("Pretendard JP Variable", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnCopyFromRight.Location = new Point(11, 11);
            btnCopyFromRight.Name = "btnCopyFromRight";
            btnCopyFromRight.Size = new Size(84, 45);
            btnCopyFromRight.TabIndex = 3;
            btnCopyFromRight.Text = ">>>";
            btnCopyFromRight.Click += btnCopyFromRight_Click;
            // 
            // txtListDir
            // 
            txtListDir.Font = new Font("Pretendard JP Variable", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtListDir.Location = new Point(12, 69);
            txtListDir.Name = "txtListDir";
            txtListDir.Size = new Size(461, 46);
            txtListDir.TabIndex = 4;
            // 
            // btnRightDir
            // 
            btnRightDir.AutoSize = true;
            btnRightDir.Font = new Font("Pretendard JP Variable", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnRightDir.Location = new Point(479, 69);
            btnRightDir.Name = "btnRightDir";
            btnRightDir.Size = new Size(113, 46);
            btnRightDir.TabIndex = 5;
            btnRightDir.Text = "폴더 선택";
            btnRightDir.Click += btnRightDir_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1258, 668);
            Controls.Add(splitContainerMain);
            Name = "Form1";
            Padding = new Padding(12);
            Text = "File Compare";
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            leftBottomPanel.ResumeLayout(false);
            leftTopPanel.ResumeLayout(false);
            leftTopPanel.PerformLayout();
            rightBottomPanel.ResumeLayout(false);
            rightTopPanel.ResumeLayout(false);
            rightTopPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainerMain;
        private Panel leftTopPanel;
        private Panel leftBottomPanel;
        private Panel rightTopPanel;
        private Panel rightBottomPanel;

        private Label lblAppName;
        private TextBox txtLeftDir;
        private Button btnLeftDir;

        private Button btnCopyFromLeft;

        private ListView lvwLeftDir;
        private ListView lvwRightDir;
        private Button btnCopyFromRight;
        private TextBox txtListDir;
        private Button btnRightDir;
    }
}
