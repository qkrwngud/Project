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

        private delegate void CloseDelegate();
        CloseDelegate closeDelegate;

        public PasswordForm(TitleForm NewTitleForm, string NewFormType)
        {
            InitializeComponent();

            FormType = NewFormType;

            closeDelegate = new CloseDelegate(NewTitleForm.OpenDiary);

        }
        public PasswordForm(string NewFormType)
        {
            InitializeComponent();

            FormType = NewFormType;
        }

        string DiaryPath = "";

        public void SetDiaryPath(string NewDiaryPath)
        {
            DiaryPath = NewDiaryPath;
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
                    closeDelegate();
                }
            }
        }
    }
}
