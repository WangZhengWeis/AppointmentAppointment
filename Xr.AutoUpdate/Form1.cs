using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace Xr.AutoUpdate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Thread t;

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "当前版本：" + ConfigurationManager.AppSettings["version"].ToString();
            //用子线程工作
            t = new System.Threading.Thread(new System.Threading.ThreadStart(StartUpdate));
            t.Start();
        }

        //开始更新
        public void StartUpdate()
        {
            Downloader downloader = new Downloader();
            downloader.onDownLoadProgress += new Downloader.dDownloadProgress(downloader_onDownLoadProgress);
            downloader.Start();
        }
        /// <summary>
        /// 同步更新UI
        /// </summary>
        /// <param name="total">总进度</param>
        /// <param name="current">当前进度</param>
        /// <param name="msg">信息</param>
        /// <param name="version">版本号</param>
        /// <param name="flag">true：正常的更新进度；false：错误信息</param>
        void downloader_onDownLoadProgress(long total, long current, String msg, String version, bool flag)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Downloader.dDownloadProgress(downloader_onDownLoadProgress), new object[] { total, current, msg, version, flag });
            }
            else
            {
                this.progressBar1.Maximum = (int)total;
                this.progressBar1.Value = (int)current;
                if (flag)
                    label1.ForeColor = Color.Black;
                else
                    label1.ForeColor = Color.Red;
                label1.Text = msg;
                if (current == 100)
                {
                    this.Close();
                    System.Environment.Exit(0);
                }
                if (version != null)
                {
                    label3.Text = "最新版本：" + version;
                }
            }
        }

        /// <summary>
        /// 下载类（您的复杂处理类）
        /// </summary>
        public class Downloader
        {
            public String FileName { get; set; }
            //委托
            public delegate void dDownloadProgress(long total, long current, String msg, String version, bool flag);
            //事件
            public event dDownloadProgress onDownLoadProgress;
            //开始模拟工作
            public void Start()
            {
                try
                {
                    int speedOfProgress = 0; //进度
                    onDownLoadProgress(100, speedOfProgress, "", null, true);

                    //System.Environment.CurrentDirectory获取和设置当前目录（即该进程从中启动的目录）的完全限定路径
                    //下载到本地的路径
                    string localPath = System.Environment.CurrentDirectory + "\\Download\\UpdatePackage";
                    //解压路径
                    string decPath = System.Environment.CurrentDirectory;

                    string serverIp = ConfigurationManager.AppSettings["serverIp"].ToString();
                    string serverUrl = ConfigurationManager.AppSettings["serverUrl"].ToString();
                    string version = ConfigurationManager.AppSettings["version"].ToString();
                    string clientVersionType = ConfigurationManager.AppSettings["clientVersionType"].ToString();
                    String url = serverUrl + "sys/clientVersion/findUpdateVersion?type=" + clientVersionType + "&version=" + version;
                    String data = httpPost(url);
                    JObject objT = JObject.Parse(data);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        List<ClientVersionEntity> cvList = objT["result"].ToObject<List<ClientVersionEntity>>();
                        //下载解压
                        if (cvList.Count > 0)
                        {
                            string zxVersion = cvList[cvList.Count() - 1].version;
                            onDownLoadProgress(100, speedOfProgress, "共需下载" + cvList.Count + "个文件", zxVersion, true);
                            int documentProgress = 90 / cvList.Count; //每个文件所拥有的进度
                            for (int i = 0; i < cvList.Count; i++)
                            {
                                String[] strArr = cvList[i].updateFilePath.Split(new char[] { '/' });
                                String fileName = strArr[strArr.Length - 1];
                                //下载
                                onDownLoadProgress(100, speedOfProgress, "共需下载" + cvList.Count + "个文件，正在下载第" + (i + 1) + "个文件:" + fileName, null, true);
                                downfile(serverIp+cvList[i].updateFilePath, fileName, localPath);
                                speedOfProgress += documentProgress / 2;
                                //解压
                                onDownLoadProgress(100, speedOfProgress, "共需下载" + cvList.Count + "个文件，第" + (i + 1) + "个文件:" + fileName + "下载成功，正在解压", null, true);
                                ZipHelper.UnpackFileRarOrZip(localPath + "/" + fileName, decPath);
                                speedOfProgress += documentProgress / 2;
                                File.Delete(localPath + "/" + fileName);
                                Thread.Sleep(1000);
                            }
                            //修改本地配置文件中的版本号
                            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                            configuration.AppSettings.Settings["version"].Value = cvList[cvList.Count - 1].version;
                            configuration.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            Thread.Sleep(1000);
                        }
                        //启动程序
                        System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
                        //设置外部程序名  
                        Info.FileName = ConfigurationManager.AppSettings["StartUpEXE"].ToString();
                        //设置外部程序工作目录为   C:\  
                        Info.WorkingDirectory = ConfigurationManager.AppSettings[@"StartUpEXE"].ToString();
                        //最小化方式启动
                        Info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        //声明一个程序类  
                        System.Diagnostics.Process Proc;
                        try
                        {
                            Proc = System.Diagnostics.Process.Start(Info);
                            System.Threading.Thread.Sleep(500);
                            Console.WriteLine();
                        }
                        catch (System.ComponentModel.Win32Exception x)
                        {
                            MessageBox.Show(x.ToString());
                        }
                        onDownLoadProgress(100, 100, "检查更新完成", null, true);
                        
                    }
                    else
                    {
                        onDownLoadProgress(100, 0, objT["message"].ToString(), null, false);
                    }
                }
                catch (Exception e)
                {
                    Log4net.LogHelper.Error(e.Message);
                    MessageBox.Show(e.Message);
                }
            }

            //下载文件1
            public static void downfile(string downloadUrl, string filename, string filepath)
            {
                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(downloadUrl);
                hwr.Timeout = 15000;
                //hwr.CookieContainer = new CookieContainer();
                HttpWebResponse hwp = (HttpWebResponse)hwr.GetResponse();
                Stream ss = hwp.GetResponseStream();
                byte[] buffer = new byte[10240];
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                FileStream fs = new FileStream(
                    string.Format(filepath + @"\" + filename),
                    FileMode.Create);
                try
                {
                    int i;
                    while ((i = ss.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fs.Write(buffer, 0, i);
                    }
                    fs.Close();
                    ss.Close();
                }
                catch (Exception e)
                {
                    Log4net.LogHelper.Error(e.Message);
                    MessageBox.Show(e.Message);
                }
            }
        }

        /// <summary>
        /// post模拟表单提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static String httpPost(String url)
        {
            string result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                System.GC.Collect();    // 请求之前 做一次垃圾回收
                System.Net.ServicePointManager.DefaultConnectionLimit = 20;

                // 打印 请求地址及参数
                Log4net.LogHelper.Info("请求数据：" + url);

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Accept = "*/*";
                request.Method = "POST";
                request.UserAgent = "Mozilla/5.0";
                //request.CookieContainer = CookieEntity.cookie;
                request.KeepAlive = false;          // 保持短链接
                request.Timeout = 1 * 60 * 1000;    // 1分钟，以防网络超时

                response = (HttpWebResponse)request.GetResponse();
                //CookieEntity.cookie = request.CookieContainer;

                stream = response.GetResponseStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                result = reader.ReadToEnd().Trim();

                // 打印响应结果
                Log4net.LogHelper.Info("响应结果：" + result);
            }
            catch (Exception e)
            {
                result = "{'state': false, 'message':'" + e.Message + "'}";
                Log4net.LogHelper.Error(e.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (response != null) response.Close();
                if (request != null) request.Abort();
            }
            return result;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
