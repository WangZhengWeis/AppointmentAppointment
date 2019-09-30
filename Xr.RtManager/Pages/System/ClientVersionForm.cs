using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using Xr.Common;
using Xr.Http;
using Xr.Common.Controls;

namespace Xr.RtManager
{
    public partial class ClientVersionForm : UserControl
    {
        public ClientVersionForm()
        {
            InitializeComponent();
        }

        private Form MainForm;
        private OpaqueCommand cmd;
        private JObject obj { get; set; }

        private void UserForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)Parent;
            pageControl1.MainForm = MainForm;
            cmd = new OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(0f);
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            SearchData(true, 1, pageControl1.PageSize);
        }

        public void SearchData(bool flag, int pageNo, int pageSize)
        {
            var param = "?title=" + tbTitle.Text
                + "&&version=" + tbVersion.Text + "&&pageNo=" + pageNo
                + "&&pageSize=" + pageSize;
            var url = AppContext.AppConfig.serverUrl + "sys/clientVersion/list" + param;

            DoWorkAsync(0, (o) =>
            {
                var data = HttpClass.httpPost(url);
                return data;

            }, null, (r) =>
            {
                cmd.HideOpaqueLayer();
                var objT = JObject.Parse(r.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    gcDict.DataSource = objT["result"][0]["list"].ToObject<List<ClientVersionEntity>>();
                    pageControl1.setData(int.Parse(objT["result"][0]["count"].ToString()),
                    int.Parse(objT["result"][0]["pageSize"].ToString()),
                    int.Parse(objT["result"][0]["pageNo"].ToString()));
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            cmd.ShowOpaqueLayer(255, true);
            SearchData(false, 1, pageControl1.PageSize);
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            var edit = new ClientVersionEdit();
            if (edit.ShowDialog() == DialogResult.OK)
            {
                Thread.Sleep(300);
                cmd.ShowOpaqueLayer(255, true);
                SearchData(true, 1, pageControl1.PageSize);
                MessageBoxUtils.Hint("保存成功!", MainForm);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as ClientVersionEntity;
            if (selectedRow == null)
            {
                return;
            }
            if (MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                cmd.ShowOpaqueLayer(225, true);
                var param = "?id=" + selectedRow.id;
                var url = AppContext.AppConfig.serverUrl + "sys/clientVersion/delete" + param;
                DoWorkAsync(500, (o) =>
                {
                    var data = HttpClass.httpPost(url);
                    return data;

                }, null, (r) =>
                {
                    var objT = JObject.Parse(r.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        SearchData(false, pageControl1.CurrentPage, pageControl1.PageSize);
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

        private void btnUp_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as ClientVersionEntity;
            if (selectedRow == null)
            {
                return;
            }
            var edit = new ClientVersionEdit();
            edit.clientVersion = selectedRow;
            edit.Text = "版本修改";
            if (edit.ShowDialog() == DialogResult.OK)
            {
                MessageBoxUtils.Hint("修改成功!", MainForm);
                DoWorkAsync(500, (o) =>
                {
                    Thread.Sleep(2700);
                    return null;

                }, null, (r) =>
                {
                    cmd.ShowOpaqueLayer(255, true);
                    SearchData(true, pageControl1.CurrentPage, pageControl1.PageSize);
                });
            }
        }

        private void pageControl1_Query(int CurrentPage, int pageSize)
        {
            cmd.ShowOpaqueLayer(255, true);
            SearchData(false, CurrentPage, pageSize);
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


            bgWorkder.WorkerReportsProgress = true;
            bgWorkder.ProgressChanged += (s, arg) =>
            {
                if (arg.ProgressPercentage > 1)
                {
                    return;
                }
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
                    {
                        throw new Exception(ex.InnerException.Message);
                    }
                    else
                    {
                        throw new Exception(ex.Message);
                    }
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

        private void buttonControl5_Click(object sender, EventArgs e)
        {
            var param = "?title=" + tbTitle.Text
            + "&&version=" + tbVersion.Text + "&&pageNo=" + 1
            + "&&pageSize=" + 10;
            var url = AppContext.AppConfig.serverUrl + "sys/clientVersion/list" + param;
            cmd.ShowOpaqueLayer();
            DoWorkAsync(5000, (o) =>
            {
                var data = HttpClass.httpPost(url);
                return data;

            }, null, (r) =>
            {
                cmd.HideOpaqueLayer();
                var objT = JObject.Parse(r.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    gcDict.DataSource = objT["result"][0]["list"].ToObject<List<ClientVersionEntity>>();
                    pageControl1.setData(int.Parse(objT["result"][0]["count"].ToString()),
                    int.Parse(objT["result"][0]["pageSize"].ToString()),
                    int.Parse(objT["result"][0]["pageNo"].ToString()));
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void ClientVersionForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = DisplayRectangle;
        }

        private void buttonControl5_Click_1(object sender, EventArgs e)
        {
            var ss = "时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n时代大厦\r\n盛世嫡妃士大夫士大夫时段范的发生的仕达发生的发生的发生的\r\n水电费第三方时段";
            MessageBoxUtils.Show(ss, MessageBoxButtons.OK, this);
        }

        private void gcDict_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as ClientVersionEntity;
            if (selectedRow == null)
            {
                return;
            }
            var edit = new ClientVersionEdit();
            edit.clientVersion = selectedRow;
            edit.Text = "版本修改";
            if (edit.ShowDialog() == DialogResult.OK)
            {
                MessageBoxUtils.Hint("修改成功!", MainForm);
                DoWorkAsync(500, (o) =>
                {
                    Thread.Sleep(2700);
                    return null;

                }, null, (r) =>
                {
                    cmd.ShowOpaqueLayer(255, true);
                    SearchData(true, pageControl1.CurrentPage, pageControl1.PageSize);
                });
            }
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.Caption == "操作")
            {
                var selectedRow = gridView1.GetFocusedRow() as ClientVersionEntity;
                if (selectedRow == null)
                {
                    return;
                }
                if (MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
                {
                    cmd.ShowOpaqueLayer(225, true);
                    var param = "?id=" + selectedRow.id;
                    var url = AppContext.AppConfig.serverUrl + "sys/clientVersion/delete" + param;
                    DoWorkAsync(500, (o) =>
                    {
                        var data = HttpClass.httpPost(url);
                        return data;

                    }, null, (r) =>
                    {
                        var objT = JObject.Parse(r.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            SearchData(false, pageControl1.CurrentPage, pageControl1.PageSize);
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
            else
            {
                var selectedRow = gridView1.GetFocusedRow() as ClientVersionEntity;
                if (selectedRow == null)
                {
                    return;
                }
                var edit = new ClientVersionEdit();
                edit.clientVersion = selectedRow;
                edit.Text = "版本修改";
                if (edit.ShowDialog() == DialogResult.OK)
                {
                    MessageBoxUtils.Hint("修改成功!", MainForm);
                    DoWorkAsync(500, (o) =>
                    {
                        Thread.Sleep(2700);
                        return null;

                    }, null, (r) =>
                    {
                        cmd.ShowOpaqueLayer(255, true);
                        SearchData(true, pageControl1.CurrentPage, pageControl1.PageSize);
                    });
                }
            }
        }
    }
}
