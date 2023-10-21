using Suisei;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Diary
{

    public partial class Form1 : Form
    {

        Suisei.TextBoxes Tboxes = new Suisei.TextBoxes();

        string FileTItle = ""; // 저장 제목
        string FileName = ""; // 파일 경로

        string DiaryType = "일반";

        int ContentsBoxCount = 10; // 텍스트 박스 갯수

        Font DefaultFontData; // 기본 폰트

        string Weather = "맑음"; // 날씨


        public Form1(string NewDiaryType)
        {
            InitializeComponent();

            this.Text = "테스트";
            Tboxes.SetPanel(MainTextPanel);
            Tboxes.InitializeBox(ContentsBoxCount);
            radioButton1.Checked = true;

            DefaultFontData = MainTextPanel.Font;

            DiaryType = NewDiaryType;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void 끝내기_Click(object sender, EventArgs e)
        {

        }

        // 글꼴 설정
        private void 글꼴FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog(this);
            MainTextPanel.Location = new Point(2, 15); // panel의 위치 조정
            Tboxes.SetFont(fontDialog1.Font); // 내용의 폰트 설정
        }

        private void 새로만들기_Click(object sender, EventArgs e)
        {
            this.Text = "";
            TitleBox.Text = ""; // 제목 초기화

            Tboxes.SetFont(DefaultFontData); // 폰트 초기화
            Tboxes.InitializeBox(ContentsBoxCount); // 기존 박스 삭제 및 박스 생성

            radioButton1.Checked = true; // 맑음으로 설정
            dateTimePicker1.Value = DateTime.Now; // 날짜를 현재로 맞춤

            FileName = "";
            FileTItle = "";
        }

        private void 열기_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*"; // 필터

            if (openFileDialog1.ShowDialog() == DialogResult.OK) // 
            {
                FileName = openFileDialog1.FileName; // 경로 저장
                FileTItle = Path.GetFileNameWithoutExtension(FileName); // 이름 저장
                this.Text = FileTItle;

                TextBox TB = new TextBox();
                TB.Text = System.IO.File.ReadAllText(openFileDialog1.FileName); // 파일에 있는 내용을 TB에 저장

                // 할거 파일을 열때 비밀일기라면 PasswordForm을 비밀번호확인으로 열어서 
                // 확인을 눌렸을때 비밀번호가 동일하면 파일을 연다

                if (TB.Lines[0] == "비밀")
                {
                    PasswordForm passwordForm = new PasswordForm("비밀번호확인");
                    passwordForm.ShowDialog();
                }

                TitleBox.Text = TB.Lines[1]; // 0번째는 제목

                // 1번째는 날짜
                DateTime NewDate = Convert.ToDateTime(TB.Lines[2]);
                dateTimePicker1.Value = NewDate;

                // 2번재는 날씨
                if (TB.Lines[3] == "맑음") radioButton1.Checked = true;
                else if (TB.Lines[3] == "비") radioButton2.Checked = true;
                else radioButton3.Checked = true;

                // 박스 재생성
                Tboxes.InitializeBox(TB.Lines.Length - 3);

                // 3번째 줄부터 내용에 채움
                for (int i = 0; i < TB.Lines.Length - 4; ++i) // 반복문 0부터 TB.Lines.Length -3 미만 까지 돌림
                {
                    Tboxes.TextBoxList[i].Text = TB.Lines[i + 4]; // Tboxes의 박스 마다 내용을 채움
                }
            }

        }

        private void 저장_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*"; // 필터
            saveFileDialog1.FileName = FileTItle; // 처음 저장 이름을 FileTitle로 함

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, GetTextforSave()); // 

                if (FileName == "")
                {
                    FileName = saveFileDialog1.FileName;
                    FileTItle = Path.GetFileNameWithoutExtension(FileName);
                    this.Text = FileTItle;
                }

            }
        }

        private void 다른이름으로저장_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, GetTextforSave());
                FileName = saveFileDialog1.FileName;
                FileTItle = Path.GetFileNameWithoutExtension(FileName);
                this.Text = FileTItle;
            }
        }

        private string GetTextforSave()
        {
            if (TitleBox.Text == "") return null;

            TextBox SaveTextBox = new TextBox();
            SaveTextBox.Multiline = true;

            SaveTextBox.Text = DiaryType + "\r\n";

            SaveTextBox.Text += TitleBox.Text + "\r\n";

            SaveTextBox.Text += dateTimePicker1.Value + "\r\n";

            SaveTextBox.Text += Weather + "\r\n";

            SaveTextBox.Text += Tboxes.GetContents() + "\r\n";

            return SaveTextBox.Text;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Weather = "맑음";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Weather = "비";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Weather = "눈";
        }

    }

}

namespace Suisei
{

    public class TextBoxes
    {

        public List<TextBox> TextBoxList = new List<TextBox>(); // TextBox 저장하는 List
        private int CurrentCursorLoc = 0;

        private Panel panel;

        private Font DefaultFont;
        int BoxHeight = 21; // 폰트 크기가 9일때 TextBox의 기본 높이

        public TextBoxes()
        {
            CurrentCursorLoc= 0;
            BoxHeight = 21;
            panel = null;
            DefaultFont = null;
        }

        public void SetPanel(Panel MainTextPanel)
        {
            panel = MainTextPanel;
            DefaultFont = panel.Font;
        }

        public void InitializeBox(int TextBoxCount)
        {
            panel.Controls.Clear();

            foreach (TextBox Tb in TextBoxList)
            {
                Tb.Dispose();
            }
            TextBoxList.Clear();

            for (int i = 0; i < TextBoxCount; ++i)
            {
                TextBox Tb = new TextBox();
                Tb.Name = (panel.Controls.Count).ToString();
                Tb.Width = 800;
                Tb.Location = new Point(0, TextBoxList.Count * BoxHeight);
                Tb.KeyDown += new KeyEventHandler(Text_KeyDown);
                Tb.Enter += new EventHandler(Text_Enter);
                Tb.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                TextBoxList.Add(Tb);
                panel.Controls.Add(Tb);
            }
            TextBoxResize();
        }

        // TextBoxCount만큼 TextBox를 만드는 메소드
        public void AddTextBox(int TextBoxCount)
        {
            for (int i = 0; i < TextBoxCount; ++i)
            {
                TextBox Tb = new TextBox();
                Tb.Name = (panel.Controls.Count).ToString();
                Tb.Width = 800;
                Tb.Location = new Point(0, TextBoxList.Count * BoxHeight);
                Tb.KeyDown += new KeyEventHandler(Text_KeyDown);
                Tb.Enter += new EventHandler(Text_Enter);
                Tb.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                TextBoxList.Add(Tb);
                panel.Controls.Add(Tb);
            }
            TextBoxResize();
        }

        // 폰트를 받아와서 적용시키는 메소드
        public void SetFont (Font Font)
        {
            panel.Font = Font;

            BoxHeight = panel.Controls[0].Size.Height;



            TextBoxResize();

        }

        // 내용을 파일에 저장할때 사용하는 메소드
        // TextBoxList의 원소인 TextBox들의 Text를 한개의 TextBox에 옮겨서 반환
        public string GetContents()
        {
            TextBox Tb = new TextBox();
            Tb.Multiline = true;
            Tb.Text = "";

            for (int i = 0; i < TextBoxList.Count; ++i)
            {
                Tb.Text += TextBoxList[i].Text + "\r\n";
            }

            return Tb.Text;
        }

        // TextBox 높이 재설정
        private void TextBoxResize()
        {

            int ScrollValue = panel.VerticalScroll.Value;

            for (int i = 1; i < TextBoxList.Count; ++i)
            {
                TextBoxList[i].Location = new Point(0, i * TextBoxList[0].Height - ScrollValue);
            }
        }


        private void ReLoadBoxSize()
        {
            Console.Clear();
            for (int i = 0; i < panel.Controls.Count; ++i)
            {
                panel.Controls[i].Location = new System.Drawing.Point(0, i * BoxHeight);
                Console.WriteLine(panel.Controls[i].Name);
            }
        }

        private void Text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                if (CurrentCursorLoc >= panel.Controls.Count - 1)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        AddTextBox(5);
                    }
                    else
                    {
                        return;
                    }
                }
                ++CurrentCursorLoc;
                panel.Controls[CurrentCursorLoc].Focus();
            }
            else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Up)
            {
                if (CurrentCursorLoc <= 0)
                {
                    return;
                }

                if (TextBoxList[CurrentCursorLoc].Text == "" || e.KeyCode == Keys.Up)
                {
                    --CurrentCursorLoc;
                    TextBoxList[CurrentCursorLoc].Focus();
                }
            }
            
        }

        private void Text_Enter(object sender, EventArgs e)
        {
            TextBox Tb = (TextBox)sender;
            CurrentCursorLoc = Int32.Parse(Tb.Name);
        }
    }
}
