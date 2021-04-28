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

namespace PDF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
      
            ITextWrapper.ImageToPdf(textBox1.Text, "d:\\", "test.pdf");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var f = new DirectoryInfo(@"C:\Users\ninespc\Downloads\Documents");
            List<string> files = new List<string>();
            foreach (var file in f.GetFiles())
            {
                files.Add(file.FullName);
            }
            ITextWrapper.MergeFiles(files.ToArray(), "d:\\mergetest\\", "out.pdf");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ITextWrapper.Split(@"D:\mergetest",
                @"D:\mergetest",
                @"\out.pdf",2);
        }
    }
}
