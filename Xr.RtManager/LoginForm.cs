using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json.Linq;
//using BedsideSettle.Pages;
using Xr.RtManager.Utils;
using System.Drawing.Drawing2D;
using Xr.Http;
using System.Threading;
using Xr.Common;

namespace Xr.RtManager
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            tbLoginName.Focus();
            tbLoginName.Select(tbLoginName.TextLength, 0); // 光标移动到最后
        }

        Xr.Common.Controls.OpaqueCommand cmd;

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbLoginName.Text.Trim().Length == 0 && tbPassword.Text.Trim().Length == 0)
            {
                return;
            }
            cmd.ShowOpaqueLayer(1f);
            String param = "username=" + tbLoginName.Text + "&password=" + tbPassword.Text;
            String url = AppContext.AppConfig.serverUrl + "login?" + param;
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.loginPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    //将数据存到session
                    AppContext.Session.userName = tbLoginName.Text; //登录名
                    AppContext.Session.password = tbPassword.Text;  //密码
                    AppContext.Session.name = objT["result"]["name"].ToString(); //用户姓名
                    AppContext.Session.officeId = objT["result"]["officeId"].ToString(); //机构id
                    AppContext.Session.menuList = objT["result"]["menuList"].ToObject<List<MenuEntity>>(); //菜单列表
                    AppContext.Session.functionList = objT["result"]["functionList"].ToObject<List<FunctionEntity>>(); //功能列表
                    AppContext.Session.UserId = objT["result"]["id"].ToString(); //用户id
                    AppContext.Session.userType = objT["result"]["userType"].ToString(); 
                    AppContext.Session.loginDate = objT["result"]["loginDate"].ToString(); //登录时间，目前没作用
                    AppContext.Session.deptIds = objT["result"]["deptIds"].ToString();
                    AppContext.Session.deptNames = objT["result"]["deptNames"].ToString();
                    AppContext.Session.deptList = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                    AppContext.Session.roleIdList = objT["result"]["roleIdList"].ToObject<List<String>>();
                    //获取医院id
                    param = "code=" + AppContext.AppConfig.hospitalCode;
                    url = AppContext.AppConfig.serverUrl + "cms/hospital/findByCode?" + param;
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        data = HttpClass.httpPost(url);
                        return data;

                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        objT = JObject.Parse(data2.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            HospitalInfoEntity hospitalInfo = objT["result"].ToObject<HospitalInfoEntity>();
                            AppContext.Session.hospitalId = hospitalInfo.id;
                            cmd.HideOpaqueLayer();
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            tbPassword.Text = "";
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                        }
                    });
                }
                else
                {
                    tbPassword.Text = "";
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
            //这是最彻底的退出方式，不管什么线程都被强制退出，把程序结束的很干净。 
            System.Environment.Exit(0); 
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            ModifyPasswordForm form = new ModifyPasswordForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                MessageBoxUtils.Hint("修改密码成功!", this);
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

        private void tbLoginName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//如果输入的是回车键  
            {
                e.Handled = true;
                tbPassword.Focus();
                tbPassword.Select(tbPassword.TextLength, 0); // 光标移动到最后
            }  
        }

        /// <summary>
        /// 清除换行符，使用回车键切到密码行会使账号框触发回车换行，导致看不到账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbLoginName_TextChanged(object sender, EventArgs e)
        {
            tbLoginName.Text = tbLoginName.Text.Trim().Replace("\r\n", "");
        }

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//如果输入的是回车键  
            {
                this.btnLogin_Click(sender, e);//触发button事件  
            }  
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            if(cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(this);
            cmd.rectDisplay = this.DisplayRectangle;
        }
    }
}
