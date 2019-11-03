using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace test1
{
    public partial class Form3 : Form
    {
        string key;
        string path;
        Amazing parent;
        public Form3(string key, string path, Amazing parent)
        {
            InitializeComponent();
            this.parent = parent;
            this.path=path;
            this.key = key;
            this.CenterToScreen();
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string passwordConfirm = textBox3.Text;
            if (username.Equals("") || password.Equals("") ){
                DialogResult dr = MessageBox.Show("不允许留空，请重新输入",
                           "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (password.Equals(passwordConfirm))
            {
                FileStream fs = new FileStream(path + "/account.data",FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(username);
                sw.WriteLine(MD5.GetMD5Hash(password));
                sw.Close();
                fs.Close();
                parent.registerFreeback(username, password);
                this.Close();
            }
            else {
                DialogResult dr = MessageBox.Show("两次密码不相同！请重新输入",
                        "密码错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
