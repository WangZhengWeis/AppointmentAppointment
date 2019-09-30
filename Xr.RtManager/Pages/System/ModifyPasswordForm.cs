using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Xr.Common;
using Xr.Http;
using Xr.RtManager.Module.System;

namespace Xr.RtManager
{
    public partial class ModifyPasswordForm : Form
    {
        public ModifyPasswordForm()
        {
            InitializeComponent();
        }

        Xr.Common.Controls.OpaqueCommand cmd;

        private void ModifyPasswordForm_Load(object sender, EventArgs e)
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            dc.DataType = typeof(ModifyPasswordEntity);
        }

        private void buttonControl1_Click(object sender, EventArgs e)
        {
            if (!dc.Validate())
            {
                return;
            }
            ModifyPasswordEntity modifyPassword = new ModifyPasswordEntity();
            dc.GetValue(modifyPassword);

            if (modifyPassword.oldPassword.Equals(modifyPassword.newPassword))
            {
                dc.ShowError(teNewPassword, "新密码不能与旧密码一样");
                return;
            }

            if (!modifyPassword.newPassword.Equals(modifyPassword.againPassword))
            {
                dc.ShowError(teAgainPassword, "密码不一致");
                return;
            }

            cmd.ShowOpaqueLayer();
            //获取用户角色，密码复杂度验证需要
            String param = "?" + PackReflectionEntity<ModifyPasswordEntity>.GetEntityToRequestParameters(modifyPassword);
            String url = AppContext.AppConfig.serverUrl + "itf/System/getUser" + param;
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    UserEntity user = objT["result"].ToObject<UserEntity>();
                    if (user.userType.Equals("1"))
                    {
                        //密码复杂度验证
                        var regex = new Regex(@"
                        (?=.*[0-9])                     #必须包含数字
                        (?=.*[a-zA-Z])                  #必须包含小写或大写字母
                        (?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
                        .{8,16}                         #至少8个字符，最多16个字符
                        ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                        if (!regex.IsMatch(modifyPassword.newPassword))
                        {
                            cmd.HideOpaqueLayer();
                            dc.ShowError(teNewPassword, "管理员密码数字+字母+符号三种组合以上,至少8位数,最多16位数");
                            return;
                        }
                    }
                    else
                    {
                        //密码复杂度验证
                        var regex = new Regex(@"
                        (?=.*[0-9])                     #必须包含数字
                        (?=.*[a-zA-Z])                  #必须包含小写或大写字母
                        (?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
                        .{6,16}                         #至少6个字符，最多16个字符
                        ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                        if (!regex.IsMatch(modifyPassword.newPassword))
                        {
                            cmd.HideOpaqueLayer();
                            dc.ShowError(teNewPassword, "密码数字+字母+符号三种组合以上,至少6位数,最多16位数");
                            return;
                        }
                    }

                    url = AppContext.AppConfig.serverUrl + "itf/System/modifyPassword" + param;
                    this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        data = HttpClass.httpPost(url);
                        return data;

                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        cmd.HideOpaqueLayer();
                        objT = JObject.Parse(data2.ToString());
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
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });
        }

        private void buttonControl2_Click(object sender, EventArgs e)
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
