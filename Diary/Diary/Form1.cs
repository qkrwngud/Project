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

        int ContentsBoxCount = 10; // 텍스트 박스 갯수

        Font DefaultFontData; // 기본 폰트

        string Weather = "맑음"; // 날씨


        public Form1()
        {
            InitializeComponent();

            this.Text = "테스트";
            Tboxes.SetPanel(MainTextPanel);
            Tboxes.MakeBoxes(ContentsBoxCount);

            DefaultFontData = MainTextPanel.Font;

        }

        private void 끝내기_Click(object sender, EventArgs e)
        {
            Close();
        }

        // 글꼴 설정
        private void 글꼴FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog(this);
            MainTextPanel.Location = new Point(2, 15); // panel의 위치 조정
            Tboxes.SetFont(fontDialog1.Font); // 내용의 폰트 설정
        }

        private void 새로만들기_Click(object sender, EventArgs e) // 사용자가 설정한 모든 값을 초기화
        {
            this.Text = "";
            TitleBox.Text = ""; // 제목 초기화

            Tboxes.SetFont(DefaultFontData); // 폰트 초기화
            Tboxes.MakeBoxes(ContentsBoxCount); // 기존 박스 삭제 및 박스 생성

            dateTimePicker1.Value = DateTime.Now; // 날짜를 현재로 맞춤

            FileName = "";
            FileTItle = "";
        }

        public void 열기_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*"; // 필터

            if (openFileDialog1.ShowDialog() == DialogResult.OK) // 
            {
                FileName = openFileDialog1.FileName; // 경로 저장
                FileTItle = Path.GetFileNameWithoutExtension(FileName); // 이름 저장
                this.Text = FileTItle;

                OpenFile(FileName);
            }

        }

        private void 저장_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*"; // 필터
            saveFileDialog1.FileName = FileTItle; // 처음 저장 이름을 FileTitle로 함

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, GetTextforSave()); // 파일 저장

                // 파일 이름이 비었으면 파일 이름, 경로, 제목 설정
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

            if (saveFileDialog1.ShowDialog() == DialogResult.OK) // saveFileDialog를 열었을때 저장을 누르면 DialogResult.Ok가 나온다
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, GetTextforSave());
                FileName = saveFileDialog1.FileName;
                FileTItle = Path.GetFileNameWithoutExtension(FileName);
                this.Text = FileTItle;
            }
        }

        private string GetTextforSave() // 텍스트 박스의 값들을 저장을 위한 문자열로 묶음
        {
            if (TitleBox.Text == "") return null;

            TextBox SaveTextBox = new TextBox();
            SaveTextBox.Multiline = true;

            SaveTextBox.Text = TitleBox.Text + "\r\n";

            SaveTextBox.Text += dateTimePicker1.Value + "\r\n";

            SaveTextBox.Text += Weather + "\r\n";

            SaveTextBox.Text += Tboxes.GetContents() + "\r\n";

            return SaveTextBox.Text;
        }

        

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Weather = "맑음";
            radioButton1.Checked = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Weather = "비";
            radioButton2.Checked = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Weather = "눈";
            radioButton3.Checked = true;
        }

        public void OpenFile(string filename) // 파일 경로를 받아 파일 경로의 데이터를 받아옴
        {

            FileName = filename; // 경로 저장
            FileTItle = Path.GetFileNameWithoutExtension(filename); // 이름 저장
            this.Text = FileTItle;

            TextBox TB = new TextBox();
            TB.Text = System.IO.File.ReadAllText(filename);


            // 할거 파일을 열때 비밀일기라면 PasswordForm을 비밀번호확인으로 열어서 
            // 확인을 눌렸을때 비밀번호가 동일하면 파일을 연다

            TitleBox.Text = TB.Lines[0]; // 0번째는 제목

            // 1번째는 날짜
            DateTime NewDate = Convert.ToDateTime(TB.Lines[1]);
            dateTimePicker1.Value = NewDate;

            // 2번재는 날씨
            if (TB.Lines[2] == "맑음") radioButton1.Checked = true;
            else if (TB.Lines[2] == "비") radioButton2.Checked = true;
            else radioButton3.Checked = true;

            // 박스 재생성
            Tboxes.MakeBoxes(TB.Lines.Length - 2);

            // 3번째 줄부터 내용에 채움
            for (int i = 0; i < TB.Lines.Length - 3; ++i) // 반복문 0부터 TB.Lines.Length -3 미만 까지 돌림
            {
                Tboxes.TextBoxList[i].Text = TB.Lines[i + 3]; // Tboxes의 박스 마다 내용을 채움
            }
            Tboxes.TextBoxResize();
        }
            
    }

}

namespace Suisei
{

    public class TextBoxes
    {

        public List<TextBox> TextBoxList = new List<TextBox>(); // TextBox 저장하는 List
        private int CurrentCursorLoc = 0; // 커서 위치

        private Panel panel; // Form1에서 받아온 Panel을 넣음

        private Font DefaultFont; // 기본 폰트
        int BoxHeight = 21; // 폰트 크기가 9일때 TextBox의 기본 높이

        public TextBoxes() // 생성자
        {
            CurrentCursorLoc= 0;
            BoxHeight = 21;
            panel = null;
            DefaultFont = null;
        }

        public void SetPanel(Panel MainTextPanel) // 판넬 세팅
        {
            panel = MainTextPanel;
            DefaultFont = panel.Font;
        }

        public void MakeBoxes(int TextBoxCount) // TextBoxCount만큼 박스 생성
        {
            panel.Controls.Clear();

            foreach (TextBox Tb in TextBoxList) // TextBoxList의 값 소멸
            {
                Tb.Dispose();
            }

            TextBoxList.Clear(); // 텍스트 박스 리스트 초기화

            // 박스 생성
            for (int i = 0; i < TextBoxCount; ++i)
            {
                TextBox Tb = new TextBox();
                Tb.Name = (panel.Controls.Count).ToString(); // 이름 설정
                Tb.Width = 800; // 너비
                Tb.Location = new Point(0, TextBoxList.Count * BoxHeight); // 위치

                // 키 이벤트
                Tb.KeyDown += new KeyEventHandler(Text_KeyDown);

                Tb.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                // TextBoxList와 panel에 추가
                TextBoxList.Add(Tb);
                panel.Controls.Add(Tb);

            }
            TextBoxResize(); // 박스 크기 재설정
        }

        public void AddTextBox(int TextBoxCount) // TextBoxCount만큼 박스 추가
        {
            // MakeBoxes와 동일
            for (int i = 0; i < TextBoxCount; ++i)
            {
                TextBox Tb = new TextBox();
                Tb.Name = (panel.Controls.Count).ToString();
                Tb.Width = 800;
                Tb.Location = new Point(0, TextBoxList.Count * BoxHeight);
                Tb.KeyDown += new KeyEventHandler(Text_KeyDown);
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

        // TextBoxList의 TextBox의 위치 재설정
        public void TextBoxResize()
        {
            int ScrollValue = panel.VerticalScroll.Value; // 현재 스크롤바 위치

            // TextBoxList의 박스 크기를 재설정
            for (int i = 1; i < TextBoxList.Count; ++i)
            {
                TextBoxList[i].Location = new Point(0, i * TextBoxList[0].Height - ScrollValue);
            }
        }

        private void Text_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter, Down(↓)키를 눌렀을때 커서가 아래로 내려감
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                // 현재 커서 위치와 panel의 요소갯수 비교
                if (CurrentCursorLoc >= panel.Controls.Count - 1)
                {
                    // 현재 커서 위치와 panel의 요소갯수가 같으면 박스를 5개 추가
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
            // Backspace, Up(↑)키를 눌렀을때 커서가 위로 올라감
            else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Up)
            {
                // 커서 위치가 0이하면 커서를 안올림
                if (CurrentCursorLoc <= 0)
                {
                    return;
                }

                // 현재 커서가 있는 위치의 TextBox가 비어있거나 Up(↑)키를 누르면 커서를 위로 올림
                if (TextBoxList[CurrentCursorLoc].Text == "" || e.KeyCode == Keys.Up)
                {
                    --CurrentCursorLoc;
                    TextBoxList[CurrentCursorLoc].Focus();
                }
            }
            
        }
    }
}
