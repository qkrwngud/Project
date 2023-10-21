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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1("일반");
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PasswordForm passwordForm = new PasswordForm("비밀번호설정");
            passwordForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1("비밀");
            form.ShowDialog();
        }
    }
}
