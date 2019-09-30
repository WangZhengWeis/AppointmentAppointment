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
    public partial class RoleDistributionEdit : Form
    {
        public RoleDistributionEdit()
        {
            InitializeComponent();
        }

        Xr.Common.Controls.OpaqueCommand cmd;
        public String id { get; set; }
        public String roleId;
        public String msg;
        private List<OfficeEntity> officeList = null;
        private List<UserEntity> userList = new List<UserEntity>();

        private void RoleDistributionEdit_Load(object sender, EventArgs e)
        {
            treeList1.OptionsBehavior.Editable = false;   //treelist不可编辑
            treeList2.OptionsBehavior.Editable = false;   //treelist不可编辑
            treeList3.OptionsBehavior.Editable = false;   //treelist不可编辑

            treeList2.KeyFieldName = "id";//设置ID  
            treeList3.KeyFieldName = "id";//设置ID 

            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            cmd.ShowOpaqueLayer(0f);

            String url = AppContext.AppConfig.serverUrl + "sys/sysOffice/findAllJson";
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    officeList = objT["result"].ToObject<List<OfficeEntity>>();
                    treeList1.DataSource = officeList;
                    treeList1.KeyFieldName = "id";//设置ID  
                    treeList1.ParentFieldName = "parentId";//设置PreID   

                    url = AppContext.AppConfig.serverUrl + "sys/sysRole/assign?roleId=" + roleId;
                    this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        data = HttpClass.httpPost(url);
                        return data;

                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        objT = JObject.Parse(data2.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            userList = objT["result"].ToObject<List<UserEntity>>();
                            treeList3.DataSource = userList;
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
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });
            
            
        }

        private void treeList1_Click(object sender, EventArgs e)
        {
            if (treeList1.Nodes.Count != 0)
            {
                if (treeList1.FocusedNode == null) return;
                var str = treeList1.FocusedNode.GetValue("id");
                if (str == null) return;
                foreach (OfficeEntity office in officeList)
                {
                    if (office.id.Equals(str))
                        treeList2.DataSource = office.userList;
                }
            }
        }

        private void treeList2_Click(object sender, EventArgs e)
        {
            UserEntity user = treeList2.GetDataRecordByNode(treeList2.FocusedNode) as UserEntity;
            bool flag = true;
            foreach (UserEntity entity in userList)
            {
                if (entity.id.Equals(user.id))
                {
                    flag = false;
                    return;
                }
            }
            if (flag)
            {
                userList.Add(user);
                treeList3.DataSource = userList;
                treeList3.RefreshDataSource();
            }
        }

        private void treeList3_Click(object sender, EventArgs e)
        {
            UserEntity user = treeList3.GetDataRecordByNode(treeList3.FocusedNode) as UserEntity;
            if (user == null) return;
            userList.Remove(user);
            treeList3.DataSource = userList;
            treeList3.RefreshDataSource();
            //treeList3.DeleteNode(treeList3.FocusedNode);
        }

        private void btnScavenging_Click(object sender, EventArgs e)
        {
            userList.Clear();
            treeList3.DataSource = userList;
            treeList3.RefreshDataSource();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String ids = "";
            foreach(UserEntity user in userList){
                ids = ids + user.id + ",";
            }
            ids.Substring(0, ids.Length-1);
            String url = AppContext.AppConfig.serverUrl + "sys/sysRole/assignrole?roleId=" + roleId + "&&ids=" + ids;
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
                    msg = objT["message"].ToString();
                    DialogResult = DialogResult.OK;
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
