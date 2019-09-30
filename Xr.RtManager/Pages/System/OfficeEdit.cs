using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Xr.Http;
using System.Threading;
using Xr.Common;

namespace Xr.RtManager
{
    public partial class OfficeEdit : Form
    {
        public OfficeEdit()
        {
            InitializeComponent();
        }

        Xr.Common.Controls.OpaqueCommand cmd;
        public OfficeEntity office { get; set; }

        private void OfficeEdit_Load(object sender, EventArgs e)
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            cmd.ShowOpaqueLayer(0f);
            dcOffice.DataType = typeof(OfficeEntity);

            //获取下拉框数据
            String url = AppContext.AppConfig.serverUrl + "sys/sysOffice/getDropDownData";
            this.DoWorkAsync(250, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    treeParent.Properties.DataSource = objT["result"]["officeList"].ToObject<List<OfficeEntity>>();
                    treeParent.Properties.TreeList.KeyFieldName = "id";//设置ID  
                    treeParent.Properties.TreeList.ParentFieldName = "parentId";//设置PreID   
                    treeParent.Properties.DisplayMember = "name";  //要在树里展示的
                    treeParent.Properties.ValueMember = "id";    //对应的value
                    treeParent.EditValue = "2";

                    treeArea.Properties.DataSource = objT["result"]["areaList"].ToObject<List<OfficeEntity>>();
                    treeArea.Properties.TreeList.KeyFieldName = "id";//设置ID  
                    treeArea.Properties.TreeList.ParentFieldName = "parentId";//设置PreID   
                    treeArea.Properties.DisplayMember = "name";  //要在树里展示的
                    treeArea.Properties.ValueMember = "id";    //对应的value
                    treeArea.EditValue = "2";

                    lueType.Properties.DataSource = objT["result"]["typeDictList"].ToObject<List<OfficeEntity>>();
                    lueType.Properties.DisplayMember = "name";
                    lueType.Properties.ValueMember = "code";
                    if (objT["result"]["typeDictList"].Count() > 0)
                        lueType.EditValue = objT["result"]["typeDictList"][0]["code"].ToString();

                    lueGrade.Properties.DataSource = objT["result"]["gradeDictList"].ToObject<List<OfficeEntity>>();
                    lueGrade.Properties.DisplayMember = "name";
                    lueGrade.Properties.ValueMember = "code";
                    if (objT["result"]["gradeDictList"].Count() > 0)
                        lueGrade.EditValue = objT["result"]["gradeDictList"][0]["code"].ToString();

                    if (office != null)
                    {
                        if (office.id != null)
                        {
                            url = AppContext.AppConfig.serverUrl + "sys/sysOffice/getOffice?officeId=" + office.id;
                            this.DoWorkAsync(250, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                            {
                                data = HttpClass.httpPost(url);
                                return data;

                            }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                            {
                                objT = JObject.Parse(data2.ToString());
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    office = objT["result"].ToObject<OfficeEntity>();
                                    dcOffice.SetValue(office);
                                    teName.Focus();
                                    cmd.HideOpaqueLayer();
                                }
                                else
                                {
                                    cmd.HideOpaqueLayer();
                                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, 
                                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                                }
                            });
                        }
                        else
                        {
                            dcOffice.SetValue(office);
                            teName.Focus();
                            cmd.HideOpaqueLayer();
                        }
                    }
                    else
                    {
                        office = new OfficeEntity();
                        cmd.HideOpaqueLayer();
                    }
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!dcOffice.Validate())
            {
                return;
            }
            dcOffice.GetValue(office);
            String param = "?" + PackReflectionEntity<OfficeEntity>.GetEntityToRequestParameters(office, true);
            String url = AppContext.AppConfig.serverUrl + "sys/sysOffice/save" + param;
            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                cmd.HideOpaqueLayer();
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
                    cmd.HideOpaqueLayer();
                    if (ex.InnerException != null)
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
       
    }
}
