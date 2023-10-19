using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diary
{
    public partial class PasswordCheckForm : Form
    {

        private string Password = "";
        public PasswordCheckForm(string Path)
        {
            InitializeComponent();

            Password = System.IO.File.ReadAllText(Path);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Password == textBox1.Text)
            {
                Form1 form1 = new Form1();
                form1.ShowDialog();
                Close();
            }
        }
    }
}
