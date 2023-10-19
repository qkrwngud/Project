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
    public partial class PasswordForm : Form
    {
        public PasswordForm()
        {
            InitializeComponent();
        }

        public string Password = @"C:\Users\ai_student\Desktop\Project\Diary\Diary\Password.txt";


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("비어있음");
                return;
            }

            System.IO.File.WriteAllText(Password, textBox1.Text);
            Close();
        }

    }
}
