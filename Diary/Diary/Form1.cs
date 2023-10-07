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

        public Form1()
        {
            InitializeComponent();

            this.Text = "테스트";
            Tboxes.SetPanel(MainTextPanel);
            Tboxes.AddTextBoxes(50);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void 끝내기_Click(object sender, EventArgs e)
        {
            textBox1.Text = Tboxes.GetTextBoxes().Text;
            
        }

        private void 글꼴FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog(this);
            //TitleBox.Font = fontDialog1.Font;
            Tboxes.SetFont(fontDialog1.Font);

        }
    }


}

namespace Suisei
{
    public class TextBoxes
    {

        private List<TextBox> TextBoxList = new List<TextBox>(); // TextBox 저장하는 List
        private int CurrentCursorLoc = 0;

        private Panel panel;

        int BoxHeight = 20; // 폰트 크기가 9일때 TextBox의 기본 높이

        public TextBoxes()
        {

        }

        public void SetPanel(Panel MainTextPanel)
        {
            panel = MainTextPanel;
        }

        // 매개변수 만큼 TextBox를 만드는 메소드
        public void AddTextBoxes(int TextBoxCount)
        {
            for (int i = 0; i < TextBoxCount; ++i)
            {
                TextBox Tb = new TextBox();
                Tb.Name = (TextBoxList.Count).ToString();
                Tb.Text = (TextBoxList.Count).ToString();
                Tb.Width = 800;
                Tb.KeyDown += new KeyEventHandler(Text_KeyDown);

                TextBoxList.Add(Tb);
                panel.Controls.Add(Tb);
            }

            ReLoadTextBoxes();
        }

        // 폰트를 받아와서 적용시키는 메소드
        public void SetFont (Font Font)
        {
            panel.Font = Font;

            BoxHeight = TextBoxList[0].Size.Height;

            ReLoadTextBoxes();

        }

        // TextBoxList의 원소인 TextBox들의 Text를 한개의 TextBox에 옮겨서 반환
        // 내용을 파일에 저장할때 사용하는 메소드
        public TextBox GetTextBoxes()
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

        private void Text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CurrentCursorLoc >= TextBoxList.Count - 1)
                {
                    AddTextBoxes(5);
                }
                ++CurrentCursorLoc;
                Console.WriteLine(panel.Controls.Count);
                TextBoxList[CurrentCursorLoc].Focus();
            }
        }

        private void ReLoadTextBoxes()
        {
            for (int i = 0; i < TextBoxList.Count; ++i)
            {
                TextBoxList[i].Location = new System.Drawing.Point(0, i * BoxHeight);
            }
        }

    }
}
