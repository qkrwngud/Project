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

        public Form1 form;

        private void button1_Click(object sender, EventArgs e)
        {
            form = new Form1();
            form.ShowDialog();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "텍스트 문서(*.txt)|*.txt|모든파일|*.*"; // 필터

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                form = new Form1();
                form.OpenFile(openFileDialog1.FileName);
                form.ShowDialog();
            }
        }
    }
}
