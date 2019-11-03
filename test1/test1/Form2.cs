using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace test1
{
    public partial class Form2 : Form
    {
        // 截图窗口
        Cutter cutter = null;
        // 截得的图片
        public static Bitmap catchBmp = null;

        Amazing parent;
        string[] LUANGUAGE = { "CHN_ENG", "ENG", "POR", "FRE", "GER", "ITA", "SPA", "RUS", "JAP", "KOR" };
        int languageTag = 0;
        
        public Form2(Amazing parent)
        {
            InitializeComponent();
            this.parent = parent;
            this.CenterToScreen();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text) {
                case "中英文": 
                case "中文": languageTag = 0;break;
                case "英文": languageTag = 1;break;
                case "葡萄牙语": languageTag = 2; break;
                case "法语": languageTag = 3; break;
                case "德语": languageTag = 4; break;
                case "意大利语": languageTag = 5; break;
                case "西班牙语": languageTag = 6; break;
                case "俄语": languageTag = 7; break;
                case "日语": languageTag = 8; break;
                case "韩语": languageTag = 9; break;
            }
            //textBox1.Text = LUANGUAGE[languageTag];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = "正在识别...";
            label1.Visible = true;
            textBox1.Text = BaiduHelper.Ocr("E:/syctest/screen/20191020.jpg",LUANGUAGE[languageTag]);
            label1.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals("") && textBox1.Text != null){
            
                Clipboard.SetText(textBox1.Text);
        
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 新建一个截图窗口
            cutter = new Cutter(this);

            // 隐藏原窗口
            Hide();
            Thread.Sleep(200);

            // 设置截图窗口的背景图片
            Bitmap bmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(bmp.Width, bmp.Height));
            cutter.BackgroundImage = bmp;

            // 显示原窗口
            Show();

            // 显示截图窗口
            cutter.WindowState = FormWindowState.Maximized;
            cutter.ShowDialog();


        }
        public void Ocr()
        {
            label1.Text = "正在识别...";
            label1.Visible = true;
            textBox1.Text = BaiduHelper.Ocr(catchBmp, LUANGUAGE[languageTag]);
            label1.Visible = false;
        }
    }
}
