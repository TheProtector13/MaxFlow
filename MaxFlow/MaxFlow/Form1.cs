using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace MaxFlow {
    public partial class Form1 : Form {
        private Graph graph;
        private readonly Canvas canvas;
        private Point elementhost_size_offset, pathbox_size_offset;
        private bool stepbystep = false;
        private int currentstep = 0, currentmaxf = 0;
        private FormWindowState LastWindowState = FormWindowState.Minimized;
        public DpiScale DpiScale { get; private set; }
        public System.Drawing.Point CanvasAbsoluteLocation { get; private set; }

        public Form1()
        {
            InitializeComponent();
            canvas = new Canvas();
            elementHost1.Child = canvas;
            this.graph = new Graph(canvas, this, new Point(elementHost1.Width, elementHost1.Height));
            this.Refresh();
            this.Activated += MoveEvent;
            this.Move += MoveEvent;
            this.Resize += Form1_Resize;
            this.ResizeEnd += Form1_ResizeEnd;
        }

        private void MoveEvent(object sender, EventArgs args)
        {
            DpiScale = VisualTreeHelper.GetDpi(canvas);
            CanvasAbsoluteLocation = elementHost1.PointToScreen(new System.Drawing.Point(0, 0));
        }

        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newname = InputForm.ShowForm(this);
            if (newname == string.Empty) {
                return;
            }
            graph.RenameNode(newname);
        }

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.RemoveNode();
        }

        private void SetStreamSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string news = InputForm.ShowForm(this, "Enter new stream size!");
            if (!int.TryParse(news, out int newsize)) {
                return;
            }
            if (newsize < 1) {
                return;
            }
            graph.SetStreamSizeOfSPLine(newsize);
        }

        private void RemoveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            graph.RemoveSPLine();
        }

        private void NewNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.NewNode();
        }

        private void NewBlankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.graph.Dispose();
            this.graph = new Graph(canvas, this, new Point(elementHost1.Width, elementHost1.Height));
            MaxFlowLabel.Text = "Maximum Flow: 0";
            this.Refresh();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() {
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = "gbin",
                DereferenceLinks = true,
                Filter = "GBIN files (*.gbin)|*.gbin",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                bool readable = false, formaterror = false;
                try {
                    using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read)) {
                        readable = fs.CanRead;
                        if (readable) {
                            using (DeflateStream decompstream = new DeflateStream(fs, CompressionMode.Decompress)) {
                                using (StreamReader streamReader = new StreamReader(decompstream, true)) {
                                    string line = streamReader.ReadLine();
                                    string[] linedata = line.Split(' ');
                                    if (linedata.Length != 2) {
                                        formaterror = true;
                                        readable = false;
                                    }
                                    line = streamReader.ReadLine();
                                    linedata = line.Split(' ');
                                    if (linedata.Length != 3 || !int.TryParse(linedata[0], out int _) || !int.TryParse(linedata[1], out int _) || !int.TryParse(linedata[2], out int _)) {
                                        formaterror = true;
                                        readable = false;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (InvalidDataException) {
                    formaterror = true;
                    readable = false;
                }
                catch (SystemException err) {
                    System.Windows.Forms.MessageBox.Show("An error occurred during the open process:\n\"" + err.Message + "\"\nThe file cannot be read!", "Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (readable) {
                    this.graph.Dispose();
                    this.graph = new Graph(dialog.FileName, canvas, this, new Point(elementHost1.Width, elementHost1.Height));
                    MaxFlowLabel.Text = "Maximum Flow: 0";
                    this.Refresh();
                }
                else if (formaterror) {
                    System.Windows.Forms.MessageBox.Show("An error occurred during the open process!\nThe file format is incompatible!", "Open Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else {
                    System.Windows.Forms.MessageBox.Show("An error occurred during the open process!\nThe file cannot be read!", "Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.Save();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (stepbystep) {
                graph.Draw();
                MaxFlowLabel.Text = "Maximum Flow: 0";
                ChangeSBSStatus();
            }
            else {
                int maxf = graph.MaxFlow(RadioButtonBK.Checked ? Graph.Method.Boykov_Kolmogorov :
                    (RadioButtonBFS.Checked ? Graph.Method.BFS : Graph.Method.DFS));
                MaxFlowLabel.Text = "Maximum Flow: " + maxf;
                CalcTimeLabel.Text = "Calc.Time: " + graph.LastCalcTime.ToString("F12").TrimEnd('0') + " s";
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (stepbystep) {
                if (currentstep == graph.GetLocalFlowPathsSize() - 1) {
                    currentmaxf += graph.SetStreamValues_StepByStep(currentstep);
                    currentstep++;
                    MaxFlowLabel.Text = "Maximum Flow: " + currentmaxf;
                    ChangeSBSStatus();
                }
                else {
                    currentmaxf += graph.SetStreamValues_StepByStep(currentstep);
                    currentstep++;
                    MaxFlowLabel.Text = "Maximum Flow: " + currentmaxf;
                }
            }
            else {
                graph.Draw();
                ChangeSBSStatus();
                graph.MaxFlow((RadioButtonBK.Checked ? Graph.Method.Boykov_Kolmogorov :
                    (RadioButtonBFS.Checked ? Graph.Method.BFS : Graph.Method.DFS)), true);
                CalcTimeLabel.Text = "Calc.Time: " + graph.LastCalcTime.ToString("F12").TrimEnd('0') + " s";
                if (currentstep < graph.GetLocalFlowPathsSize()) {
                    currentmaxf += graph.SetStreamValues_StepByStep(currentstep);
                    currentstep++;
                    MaxFlowLabel.Text = "Maximum Flow: " + currentmaxf;
                    if (currentstep == graph.GetLocalFlowPathsSize()) {
                        ChangeSBSStatus();
                    }
                }
                else {
                    ChangeSBSStatus();
                }
            }
        }

        private void ChangeSBSStatus()
        {
            if (stepbystep) {
                stepbystep = false;
                button1.Image = Properties.Resources.play_button;
                button1.Text = "RUN";
                button2.Image = Properties.Resources.shoe_prints;
                button2.Text = "StepByStep";
                currentstep = 0;
                currentmaxf = 0;
                groupBox1.Enabled = true;
                redrawbutton.Enabled = true;
                menuStrip1.Enabled = true;
                graph.ControlsEnabled = true;
                ResetScalebutton.Enabled = true;
            }
            else {
                stepbystep = true;
                button1.Image = Properties.Resources.stop;
                button1.Text = "STOP";
                button2.Image = Properties.Resources.next;
                button2.Text = "Next";
                groupBox1.Enabled = false;
                redrawbutton.Enabled = false;
                menuStrip1.Enabled = false;
                graph.ControlsEnabled = false;
                ResetScalebutton.Enabled = false;
            }
        }

        private void SetAsSinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.SetSinkNode();
        }

        private void SetAsSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.SetSourceNode();
        }

        private void Redrawbutton_Click(object sender, EventArgs e)
        {
            graph.Draw();
            MaxFlowLabel.Text = "Maximum Flow: 0";
        }

        private void ResetScalebutton_Click(object sender, EventArgs e)
        {
            Graph.ResetScale();
            graph.Draw();
            MaxFlowLabel.Text = "Maximum Flow: 0";
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Activate();
            elementhost_size_offset = new Point(this.Width - elementHost1.Width, this.Height - elementHost1.Height);
            pathbox_size_offset = new Point(this.Width - groupBox2.Width, this.Height - groupBox2.Height);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState != LastWindowState) {
                LastWindowState = WindowState;
                elementHost1.Size = new System.Drawing.Size((int)(this.Width - elementhost_size_offset.X), (int)(this.Height - elementhost_size_offset.Y));
                groupBox2.Size = new System.Drawing.Size(groupBox2.Width, (int)(this.Height - pathbox_size_offset.Y));
                graph.CanvasSize = new Point(elementHost1.Width, elementHost1.Height);
                graph.BGRectResize();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S)) {
                graph.Save();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, System.Windows.Forms.Application.StartupPath + "\\MaximumFlowSimulator_HELP.chm");
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            elementHost1.Size = new System.Drawing.Size((int)(this.Width - elementhost_size_offset.X), (int)(this.Height - elementhost_size_offset.Y));
            groupBox2.Size = new System.Drawing.Size(groupBox2.Width, (int)(this.Height - pathbox_size_offset.Y));
            graph.CanvasSize = new Point(elementHost1.Width, elementHost1.Height);
            graph.BGRectResize();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.Save(true);
        }
    }
}
