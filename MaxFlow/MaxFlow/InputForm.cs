using System;
using System.Windows.Forms;

namespace MaxFlow {
    public partial class InputForm : Form {
        private string output = string.Empty;

        private InputForm()
        {
            InitializeComponent();
            textBox1.Text = output;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            output = textBox1.Text;
            this.Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ChangeLabel(string newlabelstring)
        {
            label1.Text = newlabelstring;
        }

        public static string ShowForm()
        {
            InputForm form = new InputForm();
            form.ShowDialog();
            return form.output;
        }

        public static string ShowForm(string labeltext)
        {
            InputForm form = new InputForm();
            form.ChangeLabel(labeltext);
            form.ShowDialog();
            return form.output;
        }

        public static string ShowForm(IWin32Window owner)
        {
            InputForm form = new InputForm();
            form.ShowDialog(owner);
            return form.output;
        }

        public static string ShowForm(IWin32Window owner, string labeltext)
        {
            InputForm form = new InputForm();
            form.ChangeLabel(labeltext);
            form.ShowDialog(owner);
            return form.output;
        }
    }
}
