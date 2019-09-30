using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using Xr.Common;
using Xr.Common.Controls;
using System.Threading;
using System.Web;

namespace Xr.RtManager.Pages.cms
{
    public partial class HospitalSettingsForm : UserControl
    {
        public HospitalSettingsForm()
        {
            InitializeComponent();
        }

        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public HospitalInfoEntity hospitalInfo { get; set; }

        private void HospitalSettingsForm_Load(object sender, EventArgs e)
        {
            //labelControl1.Font = new Font("微软雅黑", 18, FontStyle.Regular, GraphicsUnit.Pixel);
            //labelControl1.ForeColor = Color.FromArgb(255, 0, 0);
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(0f);
            dcHospitalInfo.DataType = typeof(HospitalInfoEntity);
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);

            bool scFlag = false;
            foreach (FunctionEntity function in AppContext.Session.functionList)
            {
                if (function.name.Equals("医院设置删除功能"))
                {
                    scFlag = true;
                    break;
                }
            }
            btnDel.Visible = scFlag;

            //查询医院类型下拉框数据
            String url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=hospital_type";
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(r.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    lueHospitalType.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                    lueHospitalType.Properties.DisplayMember = "label";
                    lueHospitalType.Properties.ValueMember = "value";

                    //查询状态下拉框数据
                    String url2 = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=is_use";
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        String data = HttpClass.httpPost(url2);
                        return data;

                    }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        objT = JObject.Parse(data.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            lueIsUse.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                            lueIsUse.Properties.DisplayMember = "label";
                            lueIsUse.Properties.ValueMember = "value";
                                
                            SearchData(1, pageControl1.PageSize);
                        }
                        else
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                            return;
                        }
                    });
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });
        }

        public void SearchData(int pageNo, int pageSize)
        {
            String url = AppContext.AppConfig.serverUrl + "cms/hospital/list?pageNo="+pageNo + "&pageSize=" +pageSize;
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(r.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<HospitalInfoEntity> list = objT["result"]["list"].ToObject<List<HospitalInfoEntity>>();
                    List<DictEntity> HospitalTypeDictList = lueHospitalType.Properties.DataSource as List<DictEntity>;
                    List<DictEntity> useDictList = lueIsUse.Properties.DataSource as List<DictEntity>;
                    foreach (HospitalInfoEntity entity in list)
                    {
                        foreach (DictEntity dict in useDictList)
                        {
                            if (entity.isUse.Equals(dict.value))
                                entity.isUse = dict.label;
                        }

                        foreach (DictEntity dict in HospitalTypeDictList)
                        {
                            if (entity.hospitalType.Equals(dict.value))
                                entity.hospitalType = dict.label;
                        }
                    }
                    gcHospitalInfo.DataSource = list;
                    pageControl1.setData(int.Parse(objT["result"]["count"].ToString()),
                    int.Parse(objT["result"]["pageSize"].ToString()),
                    int.Parse(objT["result"]["pageNo"].ToString()));
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void pageControl1_Query(int CurrentPage, int PageSize)
        {
            cmd.ShowOpaqueLayer(225, true);
            SearchData(CurrentPage, PageSize);
        }



        #region 选择与上传图片
        String logoFilePath = "";
        String logoServiceFilePath = "";
        
        private void buttonControl1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                fileDialog.Title = "请选择文件";
                fileDialog.Filter = "所有文件(*txt*)|*.*"; //设置要选择的文件的类型
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    logoFilePath = fileDialog.FileName;//返回文件的完整路径   
                    pbLogo.ImageLocation = logoFilePath; //显示本地图片
                }
            }
            catch (Exception ex)
            {
                Xr.Log4net.LogHelper.Error(ex.Message);
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (logoFilePath != "")
            {
                string url = AppContext.AppConfig.serverUrl + "common/uploadFile";
                List<FormItemModel> lstPara = new List<FormItemModel>();
                FormItemModel model = new FormItemModel();
                // 文件
                model.Key = "multipartFile";
                int l = logoFilePath.Length;
                int i = logoFilePath.LastIndexOf("\\") + 2;
                model.FileName = logoFilePath.Substring(i, l - i);
                model.FileContent = new FileStream(logoFilePath, FileMode.Open);
                lstPara.Add(model);
                cmd.ShowOpaqueLayer(225, true);
                this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.PostForm(url, lstPara);
                    return data;

                }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    cmd.HideOpaqueLayer();
                    JObject objT = JObject.Parse(r.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        WebClient web = new WebClient();
                        var bytes = web.DownloadData(AppContext.AppConfig.serverIp + objT["result"][0].ToString());
                        this.pbLogo.Image = Bitmap.FromStream(new MemoryStream(bytes));
                        logoServiceFilePath = objT["result"][0].ToString();
                        MessageBoxUtils.Hint("上传图片成功", MainForm);
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                });
            }
            else
            {
                MessageBoxUtils.Show("请选择要上传的文件", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            }
        }

        
        String pictureFilePath = "";
        String pictureServiceFilePath = "";
        private void buttonControl3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                fileDialog.Title = "请选择文件";
                fileDialog.Filter = "所有文件(*txt*)|*.*"; //设置要选择的文件的类型
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureFilePath = fileDialog.FileName;//返回文件的完整路径   
                    pbPicture.ImageLocation = pictureFilePath; //显示本地图片
                }
            }
            catch (Exception ex)
            {
                Xr.Log4net.LogHelper.Error(ex.Message);
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            }
        }

        private void buttonControl2_Click(object sender, EventArgs e)
        {
            if (pictureFilePath != "")
            {
                string url = AppContext.AppConfig.serverUrl + "common/uploadFile";
                List<FormItemModel> lstPara = new List<FormItemModel>();
                FormItemModel model = new FormItemModel();
                // 文件
                model.Key = "multipartFile";
                int l = pictureFilePath.Length;
                int i = pictureFilePath.LastIndexOf("\\") + 2;
                model.FileName = pictureFilePath.Substring(i, l - i);
                model.FileContent = new FileStream(pictureFilePath, FileMode.Open);
                lstPara.Add(model);
                cmd.ShowOpaqueLayer(225, true);
                this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.PostForm(url, lstPara);
                    return data;

                }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    cmd.HideOpaqueLayer();
                    JObject objT = JObject.Parse(r.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        WebClient web = new WebClient();
                        var bytes = web.DownloadData(AppContext.AppConfig.serverIp + objT["result"][0].ToString());
                        this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(bytes));
                        pictureServiceFilePath = objT["result"][0].ToString();
                        MessageBoxUtils.Hint("上传图片成功", MainForm);
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                });
            }
            else
            {
                MessageBoxUtils.Show("请选择要上传的文件", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            }
        }
        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //清除值
            dcHospitalInfo.ClearValue();
            pbLogo.Image = null;
            pbLogo.Refresh();
            logoServiceFilePath = null;
            pbPicture.Image = null;
            pbPicture.Refresh();
            pictureServiceFilePath = null;
            logoFilePath = "";
            pictureFilePath = "";

            groupBox1.Enabled = true;
            hospitalInfo = new HospitalInfoEntity();
            List<DictEntity> isUseList = lueIsUse.Properties.DataSource as List<DictEntity>;
            if (isUseList.Count > 0)
                lueIsUse.EditValue = isUseList[0].value;
            List<DictEntity> hospitalTypeList = lueHospitalType.Properties.DataSource as List<DictEntity>;
            if (hospitalTypeList.Count > 0)
                lueHospitalType.EditValue = hospitalTypeList[0].value;
        }


        private void btnUp_Click(object sender, EventArgs e)
        {
            //清除值
            dcHospitalInfo.ClearValue();
            pbLogo.Image = null;
            pbLogo.Refresh();
            logoServiceFilePath = null;
            pbPicture.Image = null;
            pbPicture.Refresh();
            pictureServiceFilePath = null;
            logoFilePath = null;
            pictureFilePath = null;
            logoFilePath = null;
            pictureFilePath = null;

            hospitalInfo = new HospitalInfoEntity();
            var selectedRow = gridView1.GetFocusedRow() as HospitalInfoEntity;
            if (selectedRow == null)
                return;
            cmd.ShowOpaqueLayer();
            String url = AppContext.AppConfig.serverUrl + "cms/hospital/findById?id=" + selectedRow.id;
            

            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(r.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    hospitalInfo = objT["result"].ToObject<HospitalInfoEntity>();
                    dcHospitalInfo.SetValue(hospitalInfo);
                    //显示图片
                    logoServiceFilePath = hospitalInfo.logoUrl;
                    pictureServiceFilePath = hospitalInfo.pictureUrl;

                    //医院的机子有些请求不到路径的时候，会卡的1分钟左右，所以下载图片用异步
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        if (logoServiceFilePath != null && logoServiceFilePath.Length > 0)
                        {
                            try
                            {
                                WebClient web = new WebClient();
                                var bytes = web.DownloadData(AppContext.AppConfig.serverIp+logoServiceFilePath);
                                return bytes;
                                
                            }
                            catch (Exception ex)
                            {
                                Xr.Log4net.LogHelper.Error(ex.Message);
                            }
                        }
                        return null;
                    }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        if (data != null)
                            this.pbLogo.Image = Bitmap.FromStream(new MemoryStream(data as byte[]));
                    });

                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        if (pictureServiceFilePath != null && pictureServiceFilePath.Length > 0)
                        {
                            try
                            {
                                WebClient web = new WebClient();
                                var bytes = web.DownloadData(AppContext.AppConfig.serverIp+pictureServiceFilePath);
                                this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(bytes));
                                return bytes;
                            }
                            catch (Exception ex)
                            {
                                Xr.Log4net.LogHelper.Error(ex.Message);
                            }
                        }
                        return null;
                    }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        if (data != null)
                            this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(data as byte[]));
                    });

                    groupBox1.Enabled = true;
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //检验并取值
            if (!dcHospitalInfo.Validate())
            {
                return;
            }
            dcHospitalInfo.GetValue(hospitalInfo);
            if (logoServiceFilePath != null && logoServiceFilePath.Length != 0)
            {
                hospitalInfo.logoUrl = logoServiceFilePath;
            }
            
            if (pictureServiceFilePath != null && pictureServiceFilePath.Length != 0)
            {
                hospitalInfo.pictureUrl = pictureServiceFilePath;
            }
            
            //文本编辑框的内容要转编码，不然后台获取的时候会不对
            hospitalInfo.information = HttpUtility.UrlEncode(hospitalInfo.information, Encoding.UTF8);
            //请求接口
            String param = PackReflectionEntity<HospitalInfoEntity>.GetEntityToRequestParameters(hospitalInfo, true);
            String url = AppContext.AppConfig.serverUrl + "cms/hospital/save?";

            cmd.ShowOpaqueLayer(225, true);
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url, param);
                return data;

            }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(r.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    pageControl1_Query(pageControl1.CurrentPage, pageControl1.PageSize);
                    groupBox1.Enabled = false;
                    //清除值
                    dcHospitalInfo.ClearValue();
                    pbLogo.Image = null;
                    pbLogo.Refresh();
                    logoServiceFilePath = null;
                    pbPicture.Image = null;
                    pbPicture.Refresh();
                    pictureServiceFilePath = null;
                    logoFilePath = "";
                    pictureFilePath = "";
                    MessageBoxUtils.Hint("保存成功！", MainForm);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as HospitalInfoEntity;
            if (selectedRow == null)
                return;

            if (MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                String param = "?id=" + selectedRow.id;
                String url = AppContext.AppConfig.serverUrl + "cms/hospital/delete" + param;
                cmd.ShowOpaqueLayer(225, true);
                this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    JObject objT = JObject.Parse(r.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        SearchData(pageControl1.CurrentPage, pageControl1.PageSize);
                        MessageBoxUtils.Hint("删除成功!", MainForm);
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }
                });
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var edit = new RichEditorForm();
            edit.ImagUploadUrl = AppContext.AppConfig.serverUrl;
            edit.text = hospitalInfo.information;
            if (edit.ShowDialog() == DialogResult.OK)
            {
                hospitalInfo.information = edit.text;
            }
        }

        /// <summary>
        /// 多线程异步后台处理某些耗时的数据，不会卡死界面
        /// </summary>
        /// <param name="time">线程延迟多少</param>
        /// <param name="workFunc">Func委托，包装耗时处理（不含UI界面处理），示例：(o)=>{ 具体耗时逻辑; return 处理的结果数据 }</param>
        /// <param name="funcArg">Func委托参数，用于跨线程传递给耗时处理逻辑所需要的对象，示例：String对象、JObject对象或DataTable等任何一个值</param>
        /// <param name="workCompleted">Action委托，包装耗时处理完成后，下步操作（一般是更新界面的数据或UI控件），示列：(r)=>{ datagirdview1.DataSource=r; }</param>
        protected void DoWorkAsync(int time, Func<object, object> workFunc, object funcArg = null, Action<object> workCompleted = null)
        {
            var bgWorkder = new BackgroundWorker();


            //Form loadingForm = null;
            //System.Windows.Forms.Control loadingPan = null;
            bgWorkder.WorkerReportsProgress = true;
            bgWorkder.ProgressChanged += (s, arg) =>
            {
                if (arg.ProgressPercentage > 1) return;

            };

            bgWorkder.RunWorkerCompleted += (s, arg) =>
            {

                try
                {
                    bgWorkder.Dispose();

                    if (workCompleted != null)
                    {
                        workCompleted(arg.Result);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message!=null)
                        throw new Exception(ex.InnerException.Message);
                    else
                        throw new Exception(ex.Message);
                }
            };

            bgWorkder.DoWork += (s, arg) =>
            {
                bgWorkder.ReportProgress(1);
                var result = workFunc(arg.Argument);
                arg.Result = result;
                bgWorkder.ReportProgress(100);
                Thread.Sleep(time);
            };

            bgWorkder.RunWorkerAsync(funcArg);
        }

        private void pbLogo_Click(object sender, EventArgs e)
        {
            PictureViewer pv = new PictureViewer(pbLogo.Image);
            pv.Show();
            //PictureViewer pv = new PictureViewer();
            //if (logoServiceFilePath != null && logoServiceFilePath.Length != 0)
            //{
                
            //    pv.imgPathStr = logoServiceFilePath;
            //    pv.Show();
            //}
            //else if (logoFilePath != null && logoFilePath.Length != 0)
            //{
            //    pv.imgPathStr = logoFilePath;
            //    pv.Show();
            //}
        }

        private void pbPicture_Click(object sender, EventArgs e)
        {
            PictureViewer pv = new PictureViewer(pbPicture.Image);
            pv.Show();
            //PictureViewer pv = new PictureViewer();
            //if (pictureServiceFilePath != null && pictureServiceFilePath.Length != 0)
            //{

            //    pv.imgPathStr = logoServiceFilePath;
            //    pv.Show();
            //}
            //else if (pictureFilePath != null && pictureFilePath.Length != 0)
            //{
            //    pv.imgPathStr = logoFilePath;
            //    pv.Show();
            //}
        }

        private void HospitalSettingsForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }

        private void borderPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as HospitalInfoEntity;
            if (selectedRow == null)
                return;

            //清除值
            dcHospitalInfo.ClearValue();
            pbLogo.Image = null;
            pbLogo.Refresh();
            logoServiceFilePath = null;
            pbPicture.Image = null;
            pbPicture.Refresh();
            pictureServiceFilePath = null;
            logoFilePath = null;
            pictureFilePath = null;
            logoFilePath = null;
            pictureFilePath = null;

            hospitalInfo = new HospitalInfoEntity();
            cmd.ShowOpaqueLayer();
            String url = AppContext.AppConfig.serverUrl + "cms/hospital/findById?id=" + selectedRow.id;


            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(r.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    hospitalInfo = objT["result"].ToObject<HospitalInfoEntity>();
                    dcHospitalInfo.SetValue(hospitalInfo);
                    //显示图片
                    logoServiceFilePath = hospitalInfo.logoUrl;
                    pictureServiceFilePath = hospitalInfo.pictureUrl;

                    //医院的机子有些请求不到路径的时候，会卡的1分钟左右，所以下载图片用异步
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        if (logoServiceFilePath != null && logoServiceFilePath.Length > 0)
                        {
                            try
                            {
                                WebClient web = new WebClient();
                                Log4net.LogHelper.Info("1");
                                var bytes = web.DownloadData(logoServiceFilePath);
                                return bytes;

                            }
                            catch (Exception ex)
                            {
                                Log4net.LogHelper.Info("2");
                                Xr.Log4net.LogHelper.Error(ex.Message);
                            }
                        }
                        return null;
                    }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        if (data != null)
                            this.pbLogo.Image = Bitmap.FromStream(new MemoryStream(data as byte[]));
                    });

                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        if (pictureServiceFilePath != null && pictureServiceFilePath.Length > 0)
                        {
                            try
                            {
                                WebClient web = new WebClient();
                                Log4net.LogHelper.Info("3");
                                var bytes = web.DownloadData(pictureServiceFilePath);
                                this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(bytes));
                                return bytes;
                            }
                            catch (Exception ex)
                            {
                                Log4net.LogHelper.Info("4");
                                Xr.Log4net.LogHelper.Error(ex.Message);
                            }
                        }
                        return null;
                    }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        if (data != null)
                            this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(data as byte[]));
                    });

                    groupBox1.Enabled = true;
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

    }
}
