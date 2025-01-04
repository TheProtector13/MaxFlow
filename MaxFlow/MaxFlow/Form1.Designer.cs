namespace MaxFlow
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
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.nodeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsSinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splineContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setStreamSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.defaContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newBlankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MaxFlowLabel = new System.Windows.Forms.Label();
            this.RadioButtonDFS = new System.Windows.Forms.RadioButton();
            this.RadioButtonBFS = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RadioButtonBK = new System.Windows.Forms.RadioButton();
            this.sourcelabel = new System.Windows.Forms.Label();
            this.sinklabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CPath_box = new System.Windows.Forms.TextBox();
            this.ResetScalebutton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.redrawbutton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.CalcTimeLabel = new System.Windows.Forms.Label();
            this.nodeContext.SuspendLayout();
            this.splineContext.SuspendLayout();
            this.defaContext.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(12, 27);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(622, 422);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = null;
            // 
            // nodeContext
            // 
            this.nodeContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.setAsSourceToolStripMenuItem,
            this.setAsSinkToolStripMenuItem});
            this.nodeContext.Name = "nodeContext";
            this.nodeContext.Size = new System.Drawing.Size(144, 92);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.rename;
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.RenameToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.remove;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.RemoveToolStripMenuItem_Click);
            // 
            // setAsSourceToolStripMenuItem
            // 
            this.setAsSourceToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.source;
            this.setAsSourceToolStripMenuItem.Name = "setAsSourceToolStripMenuItem";
            this.setAsSourceToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.setAsSourceToolStripMenuItem.Text = "Set as Source";
            this.setAsSourceToolStripMenuItem.Click += new System.EventHandler(this.SetAsSourceToolStripMenuItem_Click);
            // 
            // setAsSinkToolStripMenuItem
            // 
            this.setAsSinkToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.sink;
            this.setAsSinkToolStripMenuItem.Name = "setAsSinkToolStripMenuItem";
            this.setAsSinkToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.setAsSinkToolStripMenuItem.Text = "Set as Sink";
            this.setAsSinkToolStripMenuItem.Click += new System.EventHandler(this.SetAsSinkToolStripMenuItem_Click);
            // 
            // splineContext
            // 
            this.splineContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setStreamSizeToolStripMenuItem,
            this.removeToolStripMenuItem1});
            this.splineContext.Name = "splineContext";
            this.splineContext.Size = new System.Drawing.Size(151, 48);
            // 
            // setStreamSizeToolStripMenuItem
            // 
            this.setStreamSizeToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.expand;
            this.setStreamSizeToolStripMenuItem.Name = "setStreamSizeToolStripMenuItem";
            this.setStreamSizeToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.setStreamSizeToolStripMenuItem.Text = "Set StreamSize";
            this.setStreamSizeToolStripMenuItem.Click += new System.EventHandler(this.SetStreamSizeToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem1
            // 
            this.removeToolStripMenuItem1.Image = global::MaxFlow.Properties.Resources.remove;
            this.removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
            this.removeToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.removeToolStripMenuItem1.Text = "Remove";
            this.removeToolStripMenuItem1.Click += new System.EventHandler(this.RemoveToolStripMenuItem1_Click);
            // 
            // defaContext
            // 
            this.defaContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newNodeToolStripMenuItem});
            this.defaContext.Name = "defaContext";
            this.defaContext.Size = new System.Drawing.Size(131, 26);
            // 
            // newNodeToolStripMenuItem
            // 
            this.newNodeToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.packaging;
            this.newNodeToolStripMenuItem.Name = "newNodeToolStripMenuItem";
            this.newNodeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.newNodeToolStripMenuItem.Text = "New Node";
            this.newNodeToolStripMenuItem.Click += new System.EventHandler(this.NewNodeToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newBlankToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(824, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newBlankToolStripMenuItem
            // 
            this.newBlankToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.blank_page;
            this.newBlankToolStripMenuItem.Name = "newBlankToolStripMenuItem";
            this.newBlankToolStripMenuItem.Size = new System.Drawing.Size(91, 20);
            this.newBlankToolStripMenuItem.Text = "New blank";
            this.newBlankToolStripMenuItem.Click += new System.EventHandler(this.NewBlankToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.folder;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.save;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.saveAsToolStripMenuItem.Text = "Save as ...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Image = global::MaxFlow.Properties.Resources.manual;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.HelpToolStripMenuItem_Click);
            // 
            // MaxFlowLabel
            // 
            this.MaxFlowLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxFlowLabel.AutoSize = true;
            this.MaxFlowLabel.Location = new System.Drawing.Point(640, 40);
            this.MaxFlowLabel.Name = "MaxFlowLabel";
            this.MaxFlowLabel.Size = new System.Drawing.Size(88, 13);
            this.MaxFlowLabel.TabIndex = 5;
            this.MaxFlowLabel.Text = "Maximum Flow: 0";
            // 
            // RadioButtonDFS
            // 
            this.RadioButtonDFS.AutoSize = true;
            this.RadioButtonDFS.Checked = true;
            this.RadioButtonDFS.Location = new System.Drawing.Point(6, 19);
            this.RadioButtonDFS.Name = "RadioButtonDFS";
            this.RadioButtonDFS.Size = new System.Drawing.Size(125, 17);
            this.RadioButtonDFS.TabIndex = 6;
            this.RadioButtonDFS.TabStop = true;
            this.RadioButtonDFS.Text = "DFS (Ford-Fulkerson)";
            this.RadioButtonDFS.UseVisualStyleBackColor = true;
            // 
            // RadioButtonBFS
            // 
            this.RadioButtonBFS.AutoSize = true;
            this.RadioButtonBFS.Location = new System.Drawing.Point(6, 42);
            this.RadioButtonBFS.Name = "RadioButtonBFS";
            this.RadioButtonBFS.Size = new System.Drawing.Size(123, 17);
            this.RadioButtonBFS.TabIndex = 7;
            this.RadioButtonBFS.Text = "BFS (Edmonds-Karp)";
            this.RadioButtonBFS.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.RadioButtonBK);
            this.groupBox1.Controls.Add(this.RadioButtonBFS);
            this.groupBox1.Controls.Add(this.RadioButtonDFS);
            this.groupBox1.Location = new System.Drawing.Point(643, 320);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(172, 86);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Method Chooser";
            // 
            // RadioButtonBK
            // 
            this.RadioButtonBK.AutoSize = true;
            this.RadioButtonBK.Location = new System.Drawing.Point(6, 65);
            this.RadioButtonBK.Name = "RadioButtonBK";
            this.RadioButtonBK.Size = new System.Drawing.Size(120, 17);
            this.RadioButtonBK.TabIndex = 8;
            this.RadioButtonBK.TabStop = true;
            this.RadioButtonBK.Text = "Boykov Kolmogorov";
            this.RadioButtonBK.UseVisualStyleBackColor = true;
            // 
            // sourcelabel
            // 
            this.sourcelabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sourcelabel.AutoSize = true;
            this.sourcelabel.Location = new System.Drawing.Point(640, 85);
            this.sourcelabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.sourcelabel.Name = "sourcelabel";
            this.sourcelabel.Size = new System.Drawing.Size(35, 13);
            this.sourcelabel.TabIndex = 10;
            this.sourcelabel.Text = "label1";
            // 
            // sinklabel
            // 
            this.sinklabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sinklabel.AutoSize = true;
            this.sinklabel.Location = new System.Drawing.Point(640, 101);
            this.sinklabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.sinklabel.Name = "sinklabel";
            this.sinklabel.Size = new System.Drawing.Size(35, 13);
            this.sinklabel.TabIndex = 11;
            this.sinklabel.Text = "label2";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.CPath_box);
            this.groupBox2.Location = new System.Drawing.Point(643, 117);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(172, 197);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Correction Paths";
            // 
            // CPath_box
            // 
            this.CPath_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CPath_box.Location = new System.Drawing.Point(2, 15);
            this.CPath_box.Margin = new System.Windows.Forms.Padding(2);
            this.CPath_box.Multiline = true;
            this.CPath_box.Name = "CPath_box";
            this.CPath_box.ReadOnly = true;
            this.CPath_box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CPath_box.Size = new System.Drawing.Size(168, 180);
            this.CPath_box.TabIndex = 0;
            // 
            // ResetScalebutton
            // 
            this.ResetScalebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ResetScalebutton.Image = global::MaxFlow.Properties.Resources.scale;
            this.ResetScalebutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ResetScalebutton.Location = new System.Drawing.Point(740, 56);
            this.ResetScalebutton.Name = "ResetScalebutton";
            this.ResetScalebutton.Size = new System.Drawing.Size(75, 23);
            this.ResetScalebutton.TabIndex = 14;
            this.ResetScalebutton.Text = "Reset";
            this.ResetScalebutton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ResetScalebutton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.ResetScalebutton.UseVisualStyleBackColor = true;
            this.ResetScalebutton.Click += new System.EventHandler(this.ResetScalebutton_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Image = global::MaxFlow.Properties.Resources.shoe_prints;
            this.button2.Location = new System.Drawing.Point(719, 412);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 3, 3, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 37);
            this.button2.TabIndex = 13;
            this.button2.Text = "StepByStep";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // redrawbutton
            // 
            this.redrawbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.redrawbutton.Image = global::MaxFlow.Properties.Resources.reload;
            this.redrawbutton.Location = new System.Drawing.Point(643, 56);
            this.redrawbutton.Name = "redrawbutton";
            this.redrawbutton.Size = new System.Drawing.Size(75, 23);
            this.redrawbutton.TabIndex = 9;
            this.redrawbutton.Text = "Refresh";
            this.redrawbutton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.redrawbutton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.redrawbutton.UseVisualStyleBackColor = true;
            this.redrawbutton.Click += new System.EventHandler(this.Redrawbutton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = global::MaxFlow.Properties.Resources.play_button;
            this.button1.Location = new System.Drawing.Point(643, 412);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 3, 2, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 37);
            this.button1.TabIndex = 4;
            this.button1.Text = "RUN";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // CalcTimeLabel
            // 
            this.CalcTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CalcTimeLabel.AutoSize = true;
            this.CalcTimeLabel.Location = new System.Drawing.Point(640, 27);
            this.CalcTimeLabel.Name = "CalcTimeLabel";
            this.CalcTimeLabel.Size = new System.Drawing.Size(77, 13);
            this.CalcTimeLabel.TabIndex = 15;
            this.CalcTimeLabel.Text = "Calc. Time: 0 s";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 461);
            this.Controls.Add(this.CalcTimeLabel);
            this.Controls.Add(this.ResetScalebutton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.sinklabel);
            this.Controls.Add(this.sourcelabel);
            this.Controls.Add(this.redrawbutton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.MaxFlowLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.elementHost1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(840, 500);
            this.Name = "Form1";
            this.Text = "MaxFlow";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.nodeContext.ResumeLayout(false);
            this.splineContext.ResumeLayout(false);
            this.defaContext.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setStreamSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newNodeToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newBlankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip nodeContext;
        public System.Windows.Forms.ContextMenuStrip splineContext;
        public System.Windows.Forms.ContextMenuStrip defaContext;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label MaxFlowLabel;
        private System.Windows.Forms.RadioButton RadioButtonDFS;
        private System.Windows.Forms.RadioButton RadioButtonBFS;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem setAsSinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAsSourceToolStripMenuItem;
        private System.Windows.Forms.Button redrawbutton;
        public System.Windows.Forms.Label sourcelabel;
        public System.Windows.Forms.Label sinklabel;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox CPath_box;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button ResetScalebutton;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.RadioButton RadioButtonBK;
        private System.Windows.Forms.Label CalcTimeLabel;
    }
}

