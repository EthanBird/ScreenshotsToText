using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace test1
{
    public partial class Amazing : Form
    {
        
        const string LASTLONGIN = "/lastlogin.sav";
        const string GETKEY = "79AE32BCA018FF00201910201458DFAC";
        string keyDirectory = "201910201458DFAC79AE32BCA018FF00";
        string realKey;
        string configPath="C:/syctest/data";
        static private Dictionary<string, string> userDic = new Dictionary<string,string>(); 

        public Amazing()
        {
            InitializeComponent();
            InitializeAccount();
            this.CenterToScreen();
        }
        private void InitializeSecurity()
        {
            //getKey
            string keyFilePath=MD5.AesEncrypt(keyDirectory, GETKEY);
            for (int i = keyFilePath.IndexOf("="); i >= 0; i = keyFilePath.IndexOf("/"))
            {
                keyFilePath=keyFilePath.Remove(i, 1);
            }
            keyFilePath=keyFilePath.Remove(keyFilePath.IndexOf("+"), 1)+".data";
            if (File.Exists(configPath + "/" + keyFilePath))
            {
                StreamReader keySr = new StreamReader(configPath + "/" + keyFilePath);
                if (keySr.Peek() == -1)
                {
                    keySr.Close();
                    StreamWriter keySw = new StreamWriter(configPath + "/" + keyFilePath);
                    String realKey = DateTime.Now.Ticks.ToString().Substring(0, 16) + "1458DFAC79AE32BC";
                    keySw.WriteLine(MD5.AesEncrypt(realKey, keyDirectory)) ;
                    keySw.Close();
                }
                else
                {
                    realKey = MD5.AesDecrypt(keySr.ReadLine(), keyDirectory);
                }
                keySr.Close();
            }
            else {
                StreamWriter keySw = new StreamWriter(configPath + "/" + keyFilePath);
                String realKey = DateTime.Now.Ticks.ToString().Substring(0, 16) + "1458DFAC79AE32BC";
                keySw.WriteLine(MD5.AesEncrypt(realKey, keyDirectory));
                keySw.Close();
            }
        }
        private void InitializeAccount(){
            Boolean di1 =  Directory.Exists("D:/syctest/data");
            Boolean di2 =  Directory.Exists("E:/syctest/data");
            if (!di1&&!di2)
            {
                DriveInfo[] driveInfos = DriveInfo.GetDrives();
                foreach(DriveInfo d in driveInfos){
                    if (d.Name.Contains("D"))
                    {
                        this.configPath = @"D:/syctest/data";
                    }
                    else if (d.Name.Contains("E")) {
                        this.configPath = @"E:/syctest/data";
                    }
                    
                }
                Directory.CreateDirectory(configPath);
                StreamWriter sw = new StreamWriter(configPath + "/directory.config");
                sw.WriteLine(configPath);
                sw.Close();
            }
            else
            {   
                StreamReader sr;
                if(di1){
                    sr = new StreamReader(@"D:/syctest/data/directory.config");
                } else {
                    sr = new StreamReader(@"E:/syctest/data/directory.config");
                }
                this.configPath = sr.ReadLine();
                sr.Close();
            }
            /*****************/
            InitializeSecurity();
            //getLoginData
            
            if (File.Exists(configPath + LASTLONGIN)) {
                StreamReader lastLonginReader;
                lastLonginReader = new StreamReader(configPath + LASTLONGIN);
                if (lastLonginReader.Peek() != -1)
                {
                    textBox1.Text = MD5.AesDecrypt(lastLonginReader.ReadLine(), realKey);
                    textBox2.Text = MD5.AesDecrypt(lastLonginReader.ReadLine(), realKey);
                    checkBox1.Checked = MD5.AesDecrypt(lastLonginReader.ReadLine(), realKey).Equals("T");

                }
                lastLonginReader.Close();
            }



            getAccountData();
            
            
        }
       private void getAccountData(){
         if (File.Exists(configPath + "/account.data")) {
                StreamReader configPathSr = new StreamReader(configPath + "/account.data");
                while (configPathSr.Peek() != -1)
                {
                    try
                    {
                        userDic.Add(configPathSr.ReadLine(), configPathSr.ReadLine());
                    }
                    catch (Exception e){
                        Console.Write(e);
                    }
                    
                }
                configPathSr.Close();
            }
        }
        private void Amazing_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.CenterToScreen();
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String username = textBox1.Text;
            String password = textBox2.Text;
            if (userDic.ContainsKey(username))
            {
                if (MD5.GetMD5Hash(password).Equals(userDic[username]))
                {
                    FileStream fs ;
                    if (File.Exists(configPath + LASTLONGIN))
                    {
                        fs = new FileStream(configPath + LASTLONGIN, FileMode.Truncate);
                    }
                    else {
                        fs = new FileStream(configPath + LASTLONGIN, FileMode.CreateNew);
                    }
                    StreamWriter loginPasswordSw = new StreamWriter(fs);
                    if (checkBox1.Checked)
                    {
                        loginPasswordSw.WriteLine(MD5.AesEncrypt(username, realKey));
                        loginPasswordSw.WriteLine(MD5.AesEncrypt(password, realKey));
                        loginPasswordSw.WriteLine(MD5.AesEncrypt("T", realKey));
                    }
                    loginPasswordSw.Close();
                    fs.Close();  
                    Form2 form2 = new Form2(this);
                    form2.Show();
                    this.Hide();
                }
                else
                {
                    DialogResult dr = MessageBox.Show("密码错误！请重新输入。",
                        "密码错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox2.Text = null;
                }
            } 
            else {
                DialogResult dr = MessageBox.Show("帐号不存在！请重新输入。",
                    "帐号不存在", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
                
                   
        }

      
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 form3 = new Form3(realKey,configPath,this);
            form3.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //TODO
        }

        public void registerFreeback(string username,string password) {
            textBox1.Text = username;
            textBox2.Text = password;
            getAccountData();
        }
    }
}
