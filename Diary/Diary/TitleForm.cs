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
    public partial class TitleForm : Form
    {
        public TitleForm()
        {
            InitializeComponent();
        }

        PasswordForm passwordForm = new PasswordForm();

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            passwordForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (passwordForm.Password == "")
            {
                MessageBox.Show("비밀번호 설정 필요");
                return;
            }
            
            PasswordCheckForm passwordCheckForm = new PasswordCheckForm(passwordForm.Password);
            passwordCheckForm.ShowDialog();

        }
    }
}
