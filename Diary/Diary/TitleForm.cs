using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Diary
{
    public partial class TitleForm : Form
    {
        public TitleForm()
        {
            InitializeComponent();
        }

        public PasswordForm passwordForm;
        public Form1 form;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            form = new Form1("일반");
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            passwordForm = new PasswordForm("비밀번호설정");
            passwordForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            form = new Form1("비밀");
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*"; // 필터

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                TextBox Tb = new TextBox();
                Tb.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);

                if (Tb.Lines[0] == "비밀")
                { 
                    passwordForm = new PasswordForm("비밀번호확인");
                    passwordForm.SetDiaryPath(openFileDialog1.FileName);
                    passwordForm.ShowDialog();
                }
            }
        }

        public void OpenDiary()
        {
            Console.WriteLine("dddddddddddddddddd");
        }

    }
}
