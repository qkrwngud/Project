using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Diary
{

    public partial class Form1 : Form
    {

        Suisei.TextBoxes Tboxes = new Suisei.TextBoxes();

        string FileName = "";

        int ContentsBoxCount = 25;

        DateTime Date;


        public Form1()
        {
            InitializeComponent();

            this.Text = "테스트";
            Tboxes.SetPanel(MainTextPanel);
            Tboxes.AddTextBox(ContentsBoxCount);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void 끝내기_Click(object sender, EventArgs e)
        {
<<<<<<< Updated upstream
            Tboxes.AddTextBox(5);
=======
            
>>>>>>> Stashed changes
        }

        private void 글꼴FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog(this);
            //TitleBox.Font = fontDialog1.Font;
            MainTextPanel.Location = new Point(2, 15);
            Tboxes.SetFont(fontDialog1.Font);
        }

        private void 새로만들기_Click(object sender, EventArgs e)
        {
            TitleBox.Text = "";
            Tboxes.ResetTextBoxes(ContentsBoxCount);
        }

        private void 열기_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*";

            FileName = openFileDialog1.FileName;

            string Data = System.IO.File.ReadAllText(openFileDialog1.FileName);

        }

        private void 저장_Click(object sender, EventArgs e)
        {
            if (FileName == "")
            {
                saveFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, Tboxes.GetTextOfList().Text);
                    FileName = saveFileDialog1.FileName;
                }
            }
            else
            {
                System.IO.File.WriteAllText(FileName, Tboxes.GetTextOfList().Text);
            }
        }

        private void 다른이름으로저장_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, Tboxes.GetTextOfList().Text);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Date = dateTimePicker1.Value;
        }
    }

}

namespace Suisei
{
    public class TextBoxes
    {

<<<<<<< Updated upstream
        private List<TextBox> TextBoxList = new List<TextBox>(); // TextBox 저장하는 List
        private int CurrentCursorLoc;
=======
        private int CurrentCursorLoc = 0;
>>>>>>> Stashed changes

        private Panel panel;
        private int BoxHeight;

<<<<<<< Updated upstream
        private Font DefaultFont;
=======
        int BoxHeight = 21; // 폰트 크기가 9일때 TextBox의 기본 높이
>>>>>>> Stashed changes

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


        // TextBoxCount만큼 TextBox를 만드는 메소드
        public void AddTextBox(int TextBoxCount)
        {
            for (int i = 0; i < TextBoxCount; ++i)
            {
                TextBox Tb = new TextBox();
                Tb.Name = (panel.Controls.Count).ToString();
                Tb.Text = (panel.Controls.Count).ToString();
                Tb.Width = 800;
<<<<<<< Updated upstream
                Tb.Location = new Point(0, TextBoxList.Count * BoxHeight);
=======
                Tb.Location = new Point(0, panel.Controls.Count * BoxHeight);
>>>>>>> Stashed changes

                Tb.KeyDown += new KeyEventHandler(Text_KeyDown);
                Tb.Enter += new EventHandler(Text_Enter);

                panel.Controls.Add(Tb);
            }
<<<<<<< Updated upstream
            TextBoxResize();
=======
>>>>>>> Stashed changes
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

            for (int i = 0; i < panel.Controls.Count; ++i)
            {
                Tb.Text += panel.Controls[i].Text + "\r\n";
            }

            return Tb;
        }

<<<<<<< Updated upstream
        public void ResetTextBoxes(int TextBoxCount)
        {
            panel.Font = DefaultFont;

            for (int i = 0; i < TextBoxList.Count; ++i)
            {
                if (i >= TextBoxCount)
                {
                    panel.Controls.Remove(TextBoxList[i]);
                    TextBoxList.RemoveAt(i);

                    continue;
                }

                TextBoxList[i].Text = "";
            }

            TextBoxResize();

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


=======
        private void ReLoadTextBoxes()
        {
            Console.Clear();
            for (int i = 0; i < panel.Controls.Count; ++i)
            {
                panel.Controls[i].Location = new System.Drawing.Point(0, i * BoxHeight);
                Console.WriteLine(panel.Controls[i].Name);
            }
        }

>>>>>>> Stashed changes
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
