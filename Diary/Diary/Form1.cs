using System;
using System.IO;
using System.Windows.Forms;

namespace Diary
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            TitleBox.Focus();

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;

        }

        string FilePath = "";
        string FileName = "";
        string Weather = "";


        /* 할거
         * 
         * 폰트 가져오기
         * 내용 가져오기
         * 주석 달기
         * 버튼마다 이벤트 만들어주기
         * 일반일기랑 비밀일기 분할
         * 저장 조건 및 열기 조건 추가
         * 
         */


        private void 열기OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "텍스트문서(*.txt)|*.txt";
            openFileDialog1.Title = "열기";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FilePath = openFileDialog1.FileName;
                FileName = Path.GetFileNameWithoutExtension(FilePath);

                TextBox openFileData = new TextBox();
                openFileData.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);

                SetFileData(openFileData);

                this.Text = FileName;
            }

        }

        private void 저장SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FilePath == "")
            {
                saveFileDialog1.Filter = "텍스트문서(*.txt)|*.txt";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FilePath = saveFileDialog1.FileName;
                    FileName = Path.GetFileNameWithoutExtension(FilePath);
                    saveFileDialog1.FileName = FileName;

                    TextBox SaveDate = new TextBox();
                    SaveDate.Text =
                    TitleBox.Text + "\r\n" +
                    Weather + "\r\n" +
                    dateTimePicker1.Value.ToString() + "\r\n" +
                    MainTextBox.Font.ToString() + "\r\n" +
                    MainTextBox.Text;

                    System.IO.File.WriteAllText(FilePath, SaveDate.Text);

                    this.Text = FileName;
                }
            }
        }

        private void 다른이름으로저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "텍스트문서(*.txt)|*.txt";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FilePath = saveFileDialog1.FileName;
                FileName = Path.GetFileNameWithoutExtension(FilePath);

                TextBox SaveDate = new TextBox();
                SaveDate.Text = 
                    TitleBox.Text + "\r\n" + 
                    Weather + "\r\n" +
                    dateTimePicker1.Value.ToString() + "\r\n" + 
                    MainTextBox.Font.ToString() + "\r\n" + 
                    MainTextBox.Text;

                System.IO.File.WriteAllText(FilePath, SaveDate.Text);

                this.Text = FileName;
            }
        }

        private void 끝내기XToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 글꼴FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            MainTextBox.Font = fontDialog1.Font;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Weather = "Sunny";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Weather = "Rainy";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Weather = "Snow";
        }

        private void SetFileData(TextBox openFileData)
        {
            TitleBox.Text = openFileData.Lines[0].ToString();

            // 날씨 설정
            {
                string Weather = openFileData.Lines[1].ToString();

                if (Weather == "Sunny") radioButton1.Checked = true;
                else if (Weather == "Rainy") radioButton2.Checked = true;
                else if (Weather == "Snow") radioButton3.Checked = true;

            }

            // 날짜 설정
            {
                string Date = openFileData.Lines[2].ToString();

                int Year = Int32.Parse(Date.Substring(0, 4));
                int Month = Int32.Parse(Date.Substring(5, 2));
                int Day = Int32.Parse(Date.Substring(8, 2));

                dateTimePicker1.Value = new DateTime(Year, Month, Day);
            }

            // 글꼴
            {

            }
        }


        private void 새로만들기_Click(object sender, EventArgs e)
        {
            TitleBox.Focus();

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;

            MainTextBox.Text = "";
            TitleBox.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }
    }

}
