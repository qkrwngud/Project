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

        string FormType = "비밀번호설정";

        public PasswordForm(string NewFormType)
        {
            InitializeComponent();

            FormType = NewFormType;

        }

        public string PasswordFilePath = Application.StartupPath + @"\Password.txt";

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("비어있음");
                return;
            }

            if (FormType == "비밀번호설정")
            {
                System.IO.File.WriteAllText(PasswordFilePath, textBox1.Text);
                Close();
            }
            else if (FormType == "비밀번호확인")
            {
                string Password = System.IO.File.ReadAllText(PasswordFilePath);

                if (textBox1.Text == Password)
                {
                    Form1 form = new Form1("비밀");
                    form.ShowDialog();
                    this.Close();
                }
            }
        }
    }
}
