using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Xr.Common;
using Xr.Http;

namespace Xr.RtManager
{
    public partial class OfficeForm : UserControl
    {
        public OfficeForm()
        {
            InitializeComponent();
        }

        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        private JObject obj { get; set; }

        private void OfficeForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(0f);
            SearchData();
        }

        private void SearchData()
        {
            String url = AppContext.AppConfig.serverUrl + "sys/sysOffice/findAll";
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    treeList1.DataSource = objT["result"].ToObject<List<OfficeEntity>>();
                    treeList1.KeyFieldName = "id";//设置ID  
                    treeList1.ParentFieldName = "parentId";//设置PreID 
                    treeList1.ExpandAll();
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            cmd.ShowOpaqueLayer();
            SearchData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var edit = new OfficeEdit();
            if (edit.ShowDialog() == DialogResult.OK)
            {
                Thread.Sleep(300);
                cmd.ShowOpaqueLayer();
                SearchData();
                MessageBoxUtils.Hint("保存成功!", MainForm);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            String id = Convert.ToString(treeList1.FocusedNode.GetValue("id"));
            var selectedRow = new OfficeEntity();
            if (id == null)
                return;
            selectedRow.id = id;
            var form = new OfficeEdit();
            form.office = selectedRow;
            form.Text = "机构修改";
            if (form.ShowDialog() == DialogResult.OK)
            {
                Thread.Sleep(300);
                cmd.ShowOpaqueLayer();
                SearchData();
                MessageBoxUtils.Hint("修改成功!", MainForm);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (treeList1.FocusedNode == null) return;
            String id = Convert.ToString(treeList1.FocusedNode.GetValue("id"));
            if (id == null)
                return;

            if (MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                String url = AppContext.AppConfig.serverUrl + "sys/sysOffice/delete?id=" + id;
                cmd.ShowOpaqueLayer();
                this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        SearchData();
                        MessageBoxUtils.Hint("删除成功!", MainForm);
                    }
                    else
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }
                });
            }
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var selectedRow = new OfficeEntity();
            selectedRow.parentId = Convert.ToString(treeList1.FocusedNode.GetValue("id"));
            selectedRow.areaId = Convert.ToString(treeList1.FocusedNode.GetValue("areaId"));
            selectedRow.type = Convert.ToString(treeList1.FocusedNode.GetValue("type"));
            selectedRow.grade = Convert.ToString(treeList1.FocusedNode.GetValue("grade"));
            var form = new OfficeEdit();
            form.office = selectedRow;
            if (form.ShowDialog() == DialogResult.OK)
            {
                Thread.Sleep(300);
                cmd.ShowOpaqueLayer();
                SearchData();
                MessageBoxUtils.Hint("添加成功!", MainForm);
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

        private void OfficeForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }

        private void treeList1_Click(object sender, EventArgs e)
        {
            if (treeList1.FocusedColumn.Caption != "操作")
            {
                String id = Convert.ToString(treeList1.FocusedNode.GetValue("id"));
                var selectedRow = new OfficeEntity();
                if (id == null)
                    return;
                selectedRow.id = id;
                var form = new OfficeEdit();
                form.office = selectedRow;
                form.Text = "机构修改";
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Thread.Sleep(300);
                    cmd.ShowOpaqueLayer();
                    SearchData();
                    MessageBoxUtils.Hint("修改成功!", MainForm);
                }
            }
        }

        private void repositoryItemButtonEdit2_Click(object sender, EventArgs e)
        {
            if (treeList1.FocusedNode == null) return;
            String id = Convert.ToString(treeList1.FocusedNode.GetValue("id"));
            if (id == null)
                return;

            if (MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                String url = AppContext.AppConfig.serverUrl + "sys/sysOffice/delete?id=" + id;
                cmd.ShowOpaqueLayer();
                this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        SearchData();
                        MessageBoxUtils.Hint("删除成功!", MainForm);
                    }
                    else
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }
                });
            }
        }


    }
}
