using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace test1
{
    public partial class Cutter : Form
    {
        // 是否开始截图
        private bool isCatchStart = false;

        // 截图起点
        private Point startPoint;
        // 矩形起点
        private int rectX;
        private int rectY;
        // 矩形宽高
        private int width;
        private int height;

        // 确认按钮
        private Button OK_btn = null;
        Form2 parent;
        // 截图窗口构造
        public Cutter(Form2 parent) : base()
        {
            InitializeComponent();

            // 最大化截图窗口并隐藏边框
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;

            // 双缓冲
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
                           ControlStyles.AllPaintingInWmPaint,
                           true);
            this.UpdateStyles();

            // 鼠标样式
            Cursor = Cursors.Cross;
            this.parent = parent;
        }

        // 控件初始化
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Cutter
            // 
            this.ClientSize = new System.Drawing.Size(334, 242);
            this.Name = "Cutter";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Cutter_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Cutter_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Cutter_MouseUp);
            this.ResumeLayout(false);

        }

        #region 鼠标事件
        // 鼠标按下
        private void Cutter_MouseDown(object sender, MouseEventArgs e)
        {
            // 如果按下左键且还没开始开始截图，记录截图起点
            if (e.Button == MouseButtons.Left)
            {
                if (!isCatchStart)
                {
                    isCatchStart = true;

                    startPoint = new Point(e.X, e.Y);
                }
            }
        }
        // 鼠标移动
        private void Cutter_MouseMove(object sender, MouseEventArgs e)
        {
            if (isCatchStart)
            {
                // 初始化矩形区域
                rectX = Math.Min(startPoint.X, e.X);
                rectY = Math.Min(startPoint.Y, e.Y);
                width = Math.Abs(e.X - startPoint.X);
                height = Math.Abs(e.Y - startPoint.Y);

                Rectangle rect = new Rectangle(rectX, rectY, width, height);
                Pen pen = new Pen(Color.Red, 1);

                Invalidate();
                Update();
                Graphics g = this.CreateGraphics();
                g.DrawRectangle(pen, rect);
            }
        }
        // 鼠标松开
        private void Cutter_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isCatchStart = false;

                Update();

                // 保存图片到图片框
                Bitmap bmp = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(BackgroundImage, new Rectangle(0, 0, width, height), new Rectangle(rectX, rectY, width, height), GraphicsUnit.Pixel);
                
                this.Hide();
                Form2.catchBmp = bmp;
                parent.Ocr();
                // 确定按钮
                /*
                OK_btn = new Button();
                OK_btn.Location = new Point(e.X, e.Y);
                OK_btn.Size = new Size(100, 50);
                OK_btn.Text = "确认！";
                OK_btn.Click += (sende, ee) => DialogResult = DialogResult.OK;
                Controls.Add(OK_btn);
                Update();
               
                // 绘制矩形区域
                Rectangle rect = new Rectangle(rectX, rectY, width, height);
                Pen pen = new Pen(Color.Red, 5);
                g = this.CreateGraphics();
                g.DrawRectangle(pen, rect);
                  */
            }
        }
        #endregion
    }
}
