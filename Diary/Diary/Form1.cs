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

        string FileTItle = "";
        string FileName = "";

        int ContentsBoxCount = 10;

        Font DefaultFontData;

        string Weather = "맑음";


        public Form1()
        {
            InitializeComponent();

            this.Text = "테스트";
            Tboxes.SetPanel(MainTextPanel);
            Tboxes.SetTextBox(ContentsBoxCount);
            radioButton1.Checked = true;

            DefaultFontData = MainTextPanel.Font;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void 끝내기_Click(object sender, EventArgs e)
        {

        }

        private void 글꼴FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog(this);
            MainTextPanel.Location = new Point(2, 15);
            Tboxes.SetFont(fontDialog1.Font);
        }

        private void 새로만들기_Click(object sender, EventArgs e)
        {
            this.Text = "";
            TitleBox.Text = "";

            Tboxes.SetTextBox(ContentsBoxCount);

            radioButton1.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
            MainTextPanel.Font = DefaultFontData;

            FileName = "";
            FileTItle = "";
        }

        private void 열기_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog1.FileName;
                FileTItle = Path.GetFileNameWithoutExtension(FileName);
                this.Text = FileTItle;

                TextBox TB = new TextBox();
                TB.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);

                TitleBox.Text = TB.Lines[0];

                DateTime NewDate = Convert.ToDateTime(TB.Lines[1]);
                dateTimePicker1.Value = NewDate;

                if (TB.Lines[2] == "맑음") radioButton1.Checked = true;
                else if (TB.Lines[2] == "비") radioButton2.Checked = true;
                else radioButton3.Checked = true;

                Tboxes.SetTextBox(TB.Lines.Length - 2);

                for (int i = 0; i < TB.Lines.Length - 3; ++i)
                {
                    Tboxes.TextBoxList[i].Text = TB.Lines[i + 3];
                }
            }

        }

        private void 저장_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*";
            saveFileDialog1.FileName = FileTItle;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, SaveText().Text);

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
                System.IO.File.WriteAllText(saveFileDialog1.FileName, SaveText().Text);
                FileName = saveFileDialog1.FileName;
                FileTItle = Path.GetFileNameWithoutExtension(FileName);
                this.Text = FileTItle;
            }
        }

        private TextBox SaveText()
        {
            if (TitleBox.Text == "") return null;

            TextBox SaveTextBox = new TextBox();
            SaveTextBox.Multiline = true;

            SaveTextBox.Text = TitleBox.Text + "\r\n";

            SaveTextBox.Text += dateTimePicker1.Value + "\r\n";

            SaveTextBox.Text += Weather + "\r\n";

            SaveTextBox.Text += Tboxes.GetTextOfList().Text + "\r\n";

            return SaveTextBox;
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

        public void SetTextBox(int TextBoxCount)
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
        public TextBox GetTextOfList()
        {
            TextBox Tb = new TextBox();
            Tb.Multiline = true;
            Tb.Text = "";

            for (int i = 0; i < TextBoxList.Count; ++i)
            {
                Tb.Text += TextBoxList[i].Text + "\r\n";
            }

            return Tb;
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


        private void ReLoadTextBoxes()
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
