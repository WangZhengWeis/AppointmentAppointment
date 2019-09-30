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
    public partial class DeptSettingsForm : UserControl
    {
        public DeptSettingsForm()
        {
            InitializeComponent();
        }

        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public DeptInfoEntity deptInfo { get; set; }

        /// <summary>
        /// 修改排序前的数据源，用于比较，没有改过的不需要提交
        /// </summary>
        private List<DeptInfoEntity> originalDeptList { get; set; }

        private void DeptSettingsForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(0f);
            dcDeptInfo.DataType = typeof(DeptInfoEntity);

            bool scFlag = false;
            foreach (FunctionEntity function in AppContext.Session.functionList)
            {
                if (function.name.Equals("科室设置删除功能"))
                {
                    scFlag = true;
                    break;
                }
            }
            btnDel.Visible = scFlag;

            List<DictEntity> dictList = new List<DictEntity>();
            DictEntity dict = new DictEntity();
            dict.value = "0";
            dict.label = "开放";
            dictList.Add(dict);
            dict = new DictEntity();
            dict.value = "1";
            dict.label = "不开放";
            dictList.Add(dict);
            lueIsOpen.Properties.DataSource = dictList;
            lueIsOpen.Properties.DisplayMember = "label";
            lueIsOpen.Properties.ValueMember = "value";

            dictList = new List<DictEntity>();
            dict = new DictEntity();
            dict.value = "0";
            dict.label = "是";
            dictList.Add(dict);
            dict = new DictEntity();
            dict.value = "1";
            dict.label = "否";
            dictList.Add(dict);
            lueIsPush.Properties.DataSource = dictList;
            lueIsPush.Properties.DisplayMember = "label";
            lueIsPush.Properties.ValueMember = "value";

            lueIsBook.Properties.DataSource = dictList;
            lueIsBook.Properties.DisplayMember = "label";
            lueIsBook.Properties.ValueMember = "value";
            
            //查询医院下拉框数据
            String url = AppContext.AppConfig.serverUrl + "cms/hospital/findAll";
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    lueHospital.Properties.DataSource = objT["result"].ToObject<List<HospitalInfoEntity>>();
                    lueHospital.Properties.DisplayMember = "name";
                    lueHospital.Properties.ValueMember = "id";

                    //查询宣传显示下拉框数据
                    url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=show_hide";
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        data = HttpClass.httpPost(url);
                        return data;

                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        objT = JObject.Parse(data2.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            lueIsShow.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                            lueIsShow.Properties.DisplayMember = "label";
                            lueIsShow.Properties.ValueMember = "value";

                            //查询状态下拉框数据
                            url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=is_use";
                            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                            {
                                data = HttpClass.httpPost(url);
                                return data;

                            }, null, (data3) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                            {
                                objT = JObject.Parse(data3.ToString());
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    lueIsUse.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                                    lueIsUse.Properties.DisplayMember = "label";
                                    lueIsUse.Properties.ValueMember = "value";

                                    SearchData(1, 10000);
                                }
                                else
                                {
                                    cmd.HideOpaqueLayer();
                                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                                    return;
                                }
                            });
                        }
                        else
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                            return;
                        }
                    });
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });  
        }

        public void SearchData(int pageNo, int pageSize)
        {
            String param = "pageNo=" + pageNo + "&pageSize=" + pageSize + "&hospital.code=" + AppContext.AppConfig.hospitalCode + "&deptIds=" + AppContext.Session.deptIds;
            String url = AppContext.AppConfig.serverUrl + "cms/dept/list?"+param;
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DeptInfoEntity> listDeptInfo = objT["result"]["list"].ToObject<List<DeptInfoEntity>>();
                    for (int i = 0; i < listDeptInfo.Count; i++)
                    {
                        switch (listDeptInfo[i].isOpen)
                        {
                            case "0":
                                listDeptInfo[i].isOpen="开放";
                                break;
                            case "1":
                                listDeptInfo[i].isOpen = "不开放";
                                break;
                        }
                    }
                    treeList1.DataSource = listDeptInfo; //objT["result"]["list"].ToObject<List<DeptInfoEntity>>();
                    treeList1.KeyFieldName = "id";//设置ID  
                    treeList1.ParentFieldName = "superiorId";//设置PreID   
                    treeList1.ExpandAll();
                    pageControl1.setData(int.Parse(objT["result"]["count"].ToString()),
                    int.Parse(objT["result"]["pageSize"].ToString()),
                    int.Parse(objT["result"]["pageNo"].ToString()));
                    originalDeptList = Xr.Common.Utils.Utils.Clone<List<DeptInfoEntity>>(listDeptInfo);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
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
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MainForm);
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

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    cmd.HideOpaqueLayer();
                    JObject objT = JObject.Parse(data.ToString());
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
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, 
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                });
            }
            else
            {
                MessageBoxUtils.Show("请选择要上传的文件", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
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
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MainForm);
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

                cmd.ShowOpaqueLayer();
                this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.PostForm(url, lstPara);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    cmd.HideOpaqueLayer();
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        WebClient web = new WebClient();
                        var bytes = web.DownloadData(AppContext.AppConfig.serverIp + objT["result"][0].ToString());
                        this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(bytes));
                        logoServiceFilePath = objT["result"][0].ToString();
                        MessageBoxUtils.Hint("上传图片成功", MainForm);
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                });
            }
            else
            {
                MessageBoxUtils.Show("请选择要上传的文件", MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MainForm);
            }
        }
        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //清除值
            dcDeptInfo.ClearValue();
            pbLogo.Image = null;
            pbLogo.Refresh();
            logoServiceFilePath = null;
            pbPicture.Image = null;
            pbPicture.Refresh();
            pictureServiceFilePath = null;
            logoFilePath = "";
            pictureFilePath = "";

            groupBox1.Enabled = true;
            deptInfo = new DeptInfoEntity();
            List<DictEntity> isShowList = lueIsShow.Properties.DataSource as List<DictEntity>;
            if (isShowList.Count > 0)
                lueIsShow.EditValue = isShowList[0].value;
            List<DictEntity> isUseList = lueIsUse.Properties.DataSource as List<DictEntity>;
            if(isUseList.Count>0)
                lueIsUse.EditValue = isUseList[0].value;
            lueHospital.EditValue = AppContext.Session.hospitalId;
            lueIsOpen.EditValue = "0";
            lueIsPush.EditValue = "0";
            lueIsBook.EditValue = "0";
        }


        private void btnUp_Click(object sender, EventArgs e)
        {
            //清除值
            dcDeptInfo.ClearValue();
            pbLogo.Image = null;
            pbLogo.Refresh();
            logoServiceFilePath = null;
            pbPicture.Image = null;
            pbPicture.Refresh();
            pictureServiceFilePath = null;
            logoFilePath = "";
            pictureFilePath = "";

            deptInfo = new DeptInfoEntity();
            String id = Convert.ToString(treeList1.FocusedNode.GetValue("id"));
            if (id == null)
                return;
            String url = AppContext.AppConfig.serverUrl + "cms/dept/findById?id=" + id;

            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    deptInfo = objT["result"].ToObject<DeptInfoEntity>();
                    deptInfo.hospitalId = deptInfo.hospital.id;
                    if (deptInfo.parent != null)
                        deptInfo.parentId = deptInfo.parent.id;
                    dcDeptInfo.SetValue(deptInfo);
                    if (deptInfo.parent == null) //没有父级元素的时候，默认选中"无"选项
                        treeParentId.EditValue = "0";
                    //显示图片
                    logoServiceFilePath = deptInfo.logoUrl;
                    pictureServiceFilePath = deptInfo.pictureUrl;
                    //医院的机子有些请求不到路径的时候，会卡的1分钟左右，所以下载图片用异步
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        if (logoServiceFilePath != null && logoServiceFilePath.Length > 0)
                        {
                            try
                            {
                                WebClient web = new WebClient();
                                var bytes = web.DownloadData(logoServiceFilePath);
                                return bytes;

                            }
                            catch (Exception ex)
                            {
                                Xr.Log4net.LogHelper.Error(ex.Message);
                            }
                        }
                        return null;
                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        if (data2 != null)
                            this.pbLogo.Image = Bitmap.FromStream(new MemoryStream(data2 as byte[]));
                    });

                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        if (pictureServiceFilePath != null && pictureServiceFilePath.Length > 0)
                        {
                            try
                            {
                                WebClient web = new WebClient();
                                var bytes = web.DownloadData(pictureServiceFilePath);
                                this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(bytes));
                                return bytes;
                            }
                            catch (Exception ex)
                            {
                                Xr.Log4net.LogHelper.Error(ex.Message);
                            }
                        }
                        return null;
                    }, null, (data3) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        if (data3 != null)
                            this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(data3 as byte[]));
                    });
                    groupBox1.Enabled = true;
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //检验并取值
            if (!dcDeptInfo.Validate())
            {
                return;
            }
            dcDeptInfo.GetValue(deptInfo);

            if (logoServiceFilePath != null && logoServiceFilePath.Length != 0)
            {
                deptInfo.logoUrl = logoServiceFilePath;
            }
            
            if (pictureServiceFilePath != null && pictureServiceFilePath.Length != 0)
            {
                deptInfo.pictureUrl = pictureServiceFilePath;
            }
            //文本编辑框的内容要转编码，不然后台获取的时候会不对
            deptInfo.synopsis = HttpUtility.UrlEncode(deptInfo.synopsis, Encoding.UTF8);
            //请求接口
            String param = PackReflectionEntity<DeptInfoEntity>.GetEntityToRequestParameters(deptInfo, true);
            String url = AppContext.AppConfig.serverUrl + "cms/dept/save?";

            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url, param);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    pageControl1_Query(pageControl1.CurrentPage, pageControl1.PageSize);
                    groupBox1.Enabled = false;
                    //清除值
                    dcDeptInfo.ClearValue();
                    pbLogo.Image = null;
                    pbLogo.Refresh();
                    logoServiceFilePath = null;
                    pbPicture.Image = null;
                    pbPicture.Refresh();
                    pictureServiceFilePath = null;
                    logoFilePath = "";
                    pictureFilePath = "";
                    MessageBoxUtils.Hint("保存成功!", MainForm);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }); 
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            String id = Convert.ToString(treeList1.FocusedNode.GetValue("id"));
            if (id == null)
                return;

            if (MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                String param = "?id=" + id;
                String url = AppContext.AppConfig.serverUrl + "cms/dept/delete" + param;

                cmd.ShowOpaqueLayer();
                this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
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
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }
                });
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var edit = new RichEditorForm();
            edit.text = deptInfo.synopsis;
            edit.ImagUploadUrl = AppContext.AppConfig.serverUrl;
            if (edit.ShowDialog() == DialogResult.OK)
            {
                deptInfo.synopsis = edit.text;
            }
        }

        private void lueHospital_EditValueChanged(object sender, EventArgs e)
        {
            if (lueHospital.EditValue == null || lueHospital.EditValue.ToString().Length == 0)
            {
                treeParentId.Properties.DataSource = null;
                return;
            }
            HospitalInfoEntity hospitalInfo = lueHospital.GetSelectedDataRow() as HospitalInfoEntity;
            //查询上级科室下拉框数据
            String url = AppContext.AppConfig.serverUrl + "cms/dept/findAll?hospital.code=" + hospitalInfo.code;// +"&deptIds=" + AppContext.Session.deptIds;
            String data = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(data);
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                List<DeptEntity> deptLsit = objT["result"].ToObject<List<DeptEntity>>();
                DeptEntity dept = new DeptEntity();
                dept.id = "0";
                dept.name = "无";
                deptLsit.Insert(0, dept);
                
                treeParentId.Properties.DataSource = deptLsit;
                treeParentId.Properties.TreeList.KeyFieldName = "id";
                treeParentId.Properties.TreeList.ParentFieldName = "parentId";
                treeParentId.Properties.DisplayMember = "name";
                treeParentId.Properties.ValueMember = "id";
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
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
                    if(ex.InnerException!=null)
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
        }

        private void pbPicture_Click(object sender, EventArgs e)
        {
            PictureViewer pv = new PictureViewer(pbPicture.Image);
            pv.Show();
        }

        private void DeptSettingsForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (treeList1.FocusedColumn == null) return;
            if (treeList1.FocusedNode == null) return;
            if (treeList1.FocusedColumn.Caption == "排序号" || treeList1.FocusedColumn.Caption == "打印顺序") return;
            String id = Convert.ToString(treeList1.FocusedNode.GetValue("id"));
            if (id == null)
                return;

            //清除值
            dcDeptInfo.ClearValue();
            pbLogo.Image = null;
            pbLogo.Refresh();
            logoServiceFilePath = null;
            pbPicture.Image = null;
            pbPicture.Refresh();
            pictureServiceFilePath = null;
            logoFilePath = "";
            pictureFilePath = "";

            deptInfo = new DeptInfoEntity();
            String url = AppContext.AppConfig.serverUrl + "cms/dept/findById?id=" + id;

            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    deptInfo = objT["result"].ToObject<DeptInfoEntity>();
                    deptInfo.hospitalId = deptInfo.hospital.id;
                    if (deptInfo.parent != null)
                        deptInfo.parentId = deptInfo.parent.id;
                    dcDeptInfo.SetValue(deptInfo);
                    if (deptInfo.parent == null) //没有父级元素的时候，默认选中"无"选项
                        treeParentId.EditValue = "0";
                    //显示图片
                    logoServiceFilePath = deptInfo.logoUrl;
                    pictureServiceFilePath = deptInfo.pictureUrl;
                    //医院的机子有些请求不到路径的时候，会卡的1分钟左右，所以下载图片用异步
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        if (logoServiceFilePath != null && logoServiceFilePath.Length > 0)
                        {
                            try
                            {
                                WebClient web = new WebClient();
                                var bytes = web.DownloadData(AppContext.AppConfig.serverIp + logoServiceFilePath);
                                return bytes;

                            }
                            catch (Exception ex)
                            {
                                Xr.Log4net.LogHelper.Error(ex.Message);
                            }
                        }
                        return null;
                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        if (data2 != null)
                            this.pbLogo.Image = Bitmap.FromStream(new MemoryStream(data2 as byte[]));
                    });

                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        if (pictureServiceFilePath != null && pictureServiceFilePath.Length > 0)
                        {
                            try
                            {
                                WebClient web = new WebClient();
                                var bytes = web.DownloadData(AppContext.AppConfig.serverIp + pictureServiceFilePath);
                                this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(bytes));
                                return bytes;
                            }
                            catch (Exception ex)
                            {
                                Xr.Log4net.LogHelper.Error(ex.Message);
                            }
                        }
                        return null;
                    }, null, (data3) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        if (data3 != null)
                            this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(data3 as byte[]));
                    });
                    groupBox1.Enabled = true;
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void btnSaveSort_Click(object sender, EventArgs e)
        {
            List<DeptInfoEntity> deptList = treeList1.DataSource as List<DeptInfoEntity>;
            String ids = "";
            String showSorts = "";
            String printSorts = "";
            foreach(DeptInfoEntity dept in deptList){
                foreach(DeptInfoEntity originalDept in originalDeptList){
                    if (dept.id.Equals(originalDept.id))
                    {
                        if (!dept.showSort.Equals(originalDept.showSort) || !dept.printSort.Equals(originalDept.printSort))
                        {
                            ids += dept.id + ",";
                            showSorts += dept.showSort + ",";
                            printSorts += dept.printSort + ",";
                        }
                        break;
                    }
                }
            }

            if (ids.Length == 0) return;
            ids = ids.Substring(0, ids.Length - 1);
            if (showSorts.Length == 0) return;
            showSorts = showSorts.Substring(0, showSorts.Length - 1);
            if (printSorts.Length == 0) return;
            printSorts = printSorts.Substring(0, printSorts.Length - 1);

            cmd.ShowOpaqueLayer();
            String url = AppContext.AppConfig.serverUrl + "cms/dept/saveSort?deptIds=" + ids + "&showSorts=" + showSorts + "&printSorts=" + printSorts;
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    groupBox1.Enabled = false;
                    //清除值
                    dcDeptInfo.ClearValue();
                    pbLogo.Image = null;
                    pbLogo.Refresh();
                    logoServiceFilePath = null;
                    pbPicture.Image = null;
                    pbPicture.Refresh();
                    pictureServiceFilePath = null;
                    logoFilePath = "";
                    pictureFilePath = "";

                    pageControl1_Query(pageControl1.CurrentPage, pageControl1.PageSize);
                    MessageBoxUtils.Hint(objT["message"].ToString(), MainForm);
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }
    
    }
}
