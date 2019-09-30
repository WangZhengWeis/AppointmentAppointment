using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Web;
using Xr.Http;
using Newtonsoft.Json.Linq;

namespace Xr.Common.Controls
{
    public partial class frmAddImage : Form
    {
        public delegate void ReturnValue(string URLAddress);
        public ReturnValue _ReturnValue;
        //private FileUploader _FileUploader;
        private String _imagUploadUrl;
        public frmAddImage(string ImagUploadUrl)
        {
            InitializeComponent();
            _imagUploadUrl = ImagUploadUrl;
            this.ShowInTaskbar = false;
            this.Text = "插入图片";
            this.StartPosition = FormStartPosition.CenterParent;
            //
            //rBtn_Local.Checked = true;
            this.rBtn_URL_CheckedChanged(null, null);
        }
        #region //Events

        private void rBtn_URL_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtn_URL.Checked)
            {
                txt_URL.Enabled = true;
                txt_URL.Focus();
                btn_LocalImageUpdate.Enabled = false;
            }
            else
            {
                txt_URL.Enabled = false;
                btn_LocalImageUpdate.Enabled = true;
            }
        }

        private void btn_LocalImageUpdate_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpg (*.jpg)|*.jpg|jpeg (*.jpeg)|*.jpeg|gif (*.gif)|*.gif|所有文件 (*.*)|*.*";
            ofd.RestoreDirectory = true;
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;
            ofd.Title = "添加本地图片";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileFullName = ofd.FileName;

                txtFileName.Text = fileFullName;
            }
        }

        public Func<string,string> OnImageUpdate;

        protected  void OnBtnOkClick(object sender, EventArgs e)
        {
            //read url
            if (rBtn_URL.Checked)
            {
                if (!string.IsNullOrEmpty(txt_URL.Text.Trim()))
                {
                    this._ReturnValue(txt_URL.Text.Trim());
                    btnCancel_Click(null, null);
                }
                else
                {
                    //this.Warning("请输入您的网络图片地址");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.txtFileName.Text.Trim()))
                {
                    //this.Info("请从本地上传您的图片！");
                    return;
                }

                if ( this.OnImageUpdate != null )
                {
                    this._ReturnValue(this.OnImageUpdate(txtFileName.Text.Trim()));
                    btnCancel_Click(null, null);
                }
                //上传图片到服务器上
                string tempFileName = txtFileName.Text.Trim(); ;

                string saveFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond.ToString() + ".jpg";
                try
                {
                    string refMsg = String.Empty;
                    if (Upload(tempFileName, saveFileName, ref refMsg))
                    {
                        //this.Info("图片上传成功!");
                        string url = refMsg; // @"http://image.tianxiahotel.com/" + saveFileName;
                        _ReturnValue(url);
                        btnCancel_Click(null, null);
                    }
                    else
                    {
                        MessageBoxUtils.Hint("上传图片失败：" + refMsg, this);
                    }

                }
                catch (Exception ex)
                {
                    //this.Error("图片上传失败，请联系管理员!错误信息：\n" + ex.Message, "错误");
                    MessageBoxUtils.Hint("上传图片失败：" + ex.Message, HintMessageBoxIcon.Error, this);
                }

                //上传图片到WEB服务器上
                //string tempFileName = txtFileName.Text.Trim(); ;

                //string saveFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond.ToString() + ".jpg";
                //try
                //{
                //    _FileUploader = Singleton<FileUploader>.Instance;
                //    using (FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read))
                //    {
                //        UploadResponseInfo uploadResponseInfo = new UploadResponseInfo();
                //        string serverPath = @"\HtmlEditorPicture";
                //        if (_FileUploader.Upload(fs, saveFileName, serverPath, out uploadResponseInfo))
                //        {
                //            this.Info("图片上传成功!");

                //            string url = @"http://image.tianxiahotel.com/"
                //                + uploadResponseInfo.FileRelativePath.Replace("\\", "/");
                //            _ReturnValue(url);
                //        }
                //        else
                //        {
                //            this.Error("图片上传失败，请联系管理员", "错误");
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    this.Error("图片上传失败，请联系管理员!错误信息：\n" + ex.Message, "错误");
                //}
            }

            //base.OnBtnOkClick(sender, e);
        }

        #endregion Events

        /// <summary>
        /// 通过后台统一接口上传图片
        /// </summary>
        /// <param name="localpath"></param>
        /// <param name="saveName"></param>
        /// <param name="refStr"></param>
        /// <returns></returns>
        public bool Upload(string localpath, string saveName ,ref string refMsg)
        {
            FileInfo fileInf = new FileInfo(localpath);
            FileStream fs = fileInf.OpenRead();
            try
            {
                string requestUrl = _imagUploadUrl + "common/uploadFile";// @"http://192.168.11.43:8080/yyfz/api/common/uploadFile";//AppContext.AppConfig.severUrl + "";
                FormItemModel uploadFile = new FormItemModel() { FileName = saveName, FileContent = fs, Key = "multipartFile" };
                List<FormItemModel> formItems = new List<FormItemModel>();
                formItems.Add(uploadFile);

                string refStr = HttpClass.PostForm(requestUrl, formItems);//{"code":200,"message":"文件上传成功","result":["http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-11/2019011110115179.jpg"],"state":true}
                JObject objT = JObject.Parse(refStr);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    String serverIp = Xr.Common.Utils.Utils.StartSubString(_imagUploadUrl, 0, "/") + Xr.Common.Utils.Utils.SubBetweenString(_imagUploadUrl, "//", "/", true, true);
                    refMsg = serverIp+objT["result"][0].ToString();
                    return true;
                }
                else
                {
                    refMsg = objT["message"].ToString();
                    return false;
                }
                
            }
            catch (Exception e)
            {
                refMsg = e.Message;
                return false;
            }
            finally
            {
                fs.Close();
            }
        }
        /// <summary>
        /// 通过FTP上传文件
        /// </summary>
        /// <param name="localpath">上传文件的全路径 例@"D:\123.txt"</param>
        /// <param name="ftppath">FTP地址</param>//"192.168.1.104"
        /// /// <param name="saveName">保存文件名</param>
        /// <returns></returns>
        public bool Upload(string localpath, string ftppath,string saveName)
        {
            bool bol = false;
            try
            {
                FileInfo fileInf = new FileInfo(localpath);
                //替换符号
                ftppath = ftppath.Replace("\\", "/");
                //组合ftp上传文件路径
                string uri = "ftp://" + ftppath + "/" + saveName;

                // 根据uri创建FtpWebRequest对象
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                //reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Credentials = new NetworkCredential("testftp", "test123");
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                // 上传文件时通知服务器文件的大小
                reqFTP.ContentLength = fileInf.Length;
                // 缓冲大小设置为kb
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                // 打开一个文件流(System.IO.FileStream) 去读上传的文件
                FileStream fs = fileInf.OpenRead();
                try
                {
                    // 把上传的文件写入流
                    Stream strm = reqFTP.GetRequestStream();
                    // 每次读文件流的kb
                    contentLen = fs.Read(buff, 0, buffLength);
                    // 流内容没有结束
                    while (contentLen != 0)
                    {
                        // 把内容从file stream 写入upload stream
                        strm.Write(buff, 0, contentLen);
                        contentLen = fs.Read(buff, 0, buffLength);
                        bol = true;
                    }
                    // 关闭两个流
                    strm.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("上传文件失败，失败原因；" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("上传文件失败，失败原因；" + ex.Message);
            }
            return bol;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
