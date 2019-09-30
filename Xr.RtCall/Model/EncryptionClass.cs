using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xr.RtCall.Model
{
    /// <summary>
    /// 获取指定文件中的内容
    /// </summary>
   public static class EncryptionClass
    {
       public static void NewAddText(string code)
       {
           if (!File.Exists(System.Windows.Forms.Application.StartupPath + "\\doctorCode.txt"))
           {
               FileStream fs1 = new FileStream(System.Windows.Forms.Application.StartupPath + "\\doctorCode.txt", FileMode.Create, FileAccess.Write);//创建写入文件
               StreamWriter sw = new StreamWriter(fs1);
               sw.WriteLine(code);//开始写入值
               sw.Close();
               fs1.Close();
           }
           else
           {
               FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + "\\doctorCode.txt", FileMode.Open, FileAccess.Write);
               StreamWriter sr = new StreamWriter(fs);
               sr.WriteLine(code);//开始写入值
               sr.Close();
               fs.Close();
           }
       }
       /// <summary>
       /// 指定文件下的内容
       /// </summary>
       /// <param name="logPass">文件地址</param>
       /// <returns></returns>
       public static string UserOrPassWordInfor(string logPass)
        {
            string str = "";
            //string logPass = System.Windows.Forms.Application.StartupPath + "\\hispassword.txt";
            using (System.IO.FileStream fsRead = new System.IO.FileStream(logPass, System.IO.FileMode.Open))
            {
                int fsLen = (int)fsRead.Length;
                byte[] heByte = new byte[fsLen];
                int r = fsRead.Read(heByte, 0, heByte.Length);
                str = System.Text.Encoding.UTF8.GetString(heByte);
            }
            return str;
        }
    }
}
