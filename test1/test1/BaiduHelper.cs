
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace test1
{
    public class BaiduHelper
    {
        private static readonly string clientId = "1cYR8pLKMWG5Q1H6Bcy46aeA"; //"百度云应用的AK";
        private static readonly string clientSecret = "4FAMtb8xyFBPV7hCsEFCtzxrmtcp67eh"; //"百度云应用的SK";
 
        /// <summary>
        ///     图片识别
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>

        public static string Ocr(string filePath) {
            return Ocr(filePath, "CHN_ENG");
        }
        //-------------------------------------------------------------------------
        public static string Ocr(string filePath,string language)
        {
            try
            {
                string img = HttpUtility.UrlEncode(GetBase64FromImage(filePath));
                string token = GetAccessToken();
                token = new Regex(
                    "\"access_token\":\"(?<token>[^\"]*?)\"",
                    RegexOptions.CultureInvariant
                    | RegexOptions.Compiled
                    ).Match(token).Groups["token"].Value.Trim();

                //string url = "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic";
                string url = "https://aip.baidubce.com/rest/2.0/ocr/v1/accurate_basic";
                var list = new List<KeyValuePair<string, string>>
                               {
                                   new KeyValuePair<string, string>("access_token", token),
                                   new KeyValuePair<string, string>("image", img),
                                   new KeyValuePair<string, string>("language_type", language)
                               };
                var data = new List<string>();
                foreach (var pair in list)
                    data.Add(pair.Key + "=" + pair.Value);
                string json = HttpPost(url, string.Join("&", data.ToArray()));
 
                var regex = new Regex(
                    "\"words\": \"(?<word>[\\s\\S]*?)\"",
                    RegexOptions.CultureInvariant
                    | RegexOptions.Compiled
                    );
                var str = new StringBuilder();
                foreach (Match match in regex.Matches(json))
                {
                    str.AppendLine(match.Groups["word"].Value.Trim() );
                }
                return str.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string Ocr(Bitmap filePath, string language)
        {
            try
            {
                string img = HttpUtility.UrlEncode(GetBase64FromImage(filePath));
                string token = GetAccessToken();
                token = new Regex(
                    "\"access_token\":\"(?<token>[^\"]*?)\"",
                    RegexOptions.CultureInvariant
                    | RegexOptions.Compiled
                    ).Match(token).Groups["token"].Value.Trim();

                string url = "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic";
                //string url = "https://aip.baidubce.com/rest/2.0/ocr/v1/accurate_basic";
                var list = new List<KeyValuePair<string, string>>
                               {
                                   new KeyValuePair<string, string>("access_token", token),
                                   new KeyValuePair<string, string>("image", img),
                                   new KeyValuePair<string, string>("language_type", language)
                               };
                var data = new List<string>();
                foreach (var pair in list)
                    data.Add(pair.Key + "=" + pair.Value);
                string json = HttpPost(url, string.Join("&", data.ToArray()));

                var regex = new Regex(
                    "\"words\": \"(?<word>[\\s\\S]*?)\"",
                    RegexOptions.CultureInvariant
                    | RegexOptions.Compiled
                    );
                var str = new StringBuilder();
                foreach (Match match in regex.Matches(json))
                {
                    str.AppendLine(match.Groups["word"].Value.Trim());
                }
                return str.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //-------------------------------------------------------------------------
        public static string GetBase64FromImage(Bitmap imagefile)
        {
            string base64String;
            try
            {
                byte[] arr;
                using (var bmp = new Bitmap(imagefile))
                {
                    using (var ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Jpeg);
                        arr = new byte[ms.Length];
                        ms.Position = 0;
                        ms.Read(arr, 0, (int)ms.Length);
                        ms.Close();
                    }
                }
                base64String = Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }
            return base64String;
        }
        //此处重载直接代码复制....
        public static string GetBase64FromImage(string imagefile)
        {
            string base64String;
            try
            {
                byte[] arr;
                using (var bmp = new Bitmap(imagefile))
                {
                    using (var ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Jpeg);
                        arr = new byte[ms.Length];
                        ms.Position = 0;
                        ms.Read(arr, 0, (int)ms.Length);
                        ms.Close();
                    }
                }
                base64String = Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }
            return base64String;
        }
        //-------------------------------------------------------------------------
        public static string GetAccessToken()
        {
            string url = "https://aip.baidubce.com/oauth/2.0/token";
            var list = new List<KeyValuePair<string, string>>
                           {
                               new KeyValuePair<string, string>("grant_type", "client_credentials"),
                               new KeyValuePair<string, string>("client_id", clientId),
                               new KeyValuePair<string, string>("client_secret", clientSecret)
                           };
            var data = new List<string>();
            foreach (var pair in list)
                data.Add(pair.Key + "=" + pair.Value);
            return HttpGet(url, string.Join("&", data.ToArray()));
        }
 
        public static string HttpGet(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url + (data == "" ? "" : "?") + data);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Stream stream = response.GetResponseStream();
                string s = null;
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding("utf-8")))
                    {
                        s = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
                return s;
            }
        }
 
        public static string HttpPost(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(data);
            Stream stream = request.GetRequestStream();
            var writer = new StreamWriter(stream, Encoding.GetEncoding("gb2312"));
            writer.Write(data);
            writer.Close();
 
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Stream res = response.GetResponseStream();
                if (res != null)
                {
                    var reader = new StreamReader(res, Encoding.GetEncoding("utf-8"));
                    string retString = reader.ReadToEnd();
                    reader.Close();
                    res.Close();
                    return retString;
                }
            }
            return "";
        }
    }
}
