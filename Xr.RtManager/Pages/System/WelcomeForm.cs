using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.Common;
using Xr.Common.Controls;
using System.Threading;
using Xr.Http;
using Newtonsoft.Json.Linq;

namespace Xr.RtManager
{
    public partial class WelcomeForm : UserControl
    {
        //Xr.Common.Controls.OpaqueCommand cmd;
        private Form MainForm; //主窗体
        UpdateNoticeForm un;

        public WelcomeForm()
        {
            InitializeComponent();
            Bitmap bm = new Bitmap(Properties.Resources.welcom); //fbImage图片路径
            this.BackgroundImage = bm;//设置背景图片
            this.BackgroundImageLayout = ImageLayout.Stretch;//设置背景图片自动适应
            //textMenu1.DisplayMember = "name";
            //textMenu1.ValueMember = "id";
            //textMenu1.KeyFieldName = "id";
            //textMenu1.ParentFieldName = "parentId";
            //textMenu1.DataSource = AppContext.Session.menuList;
        }

        //private void textMenu1_MenuItemClick(object sender, EventArgs e, object selectItem)
        //{
        //    MenuEntity menu = selectItem as MenuEntity;
        //    MessageBox.Show(menu.href);
        //}

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

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            if (AppContext.AppConfig.showUpdateNotice.Equals("1"))
            {
                MainForm = (Form)this.Parent;
                String url = AppContext.AppConfig.serverUrl + "sys/clientVersion/getMaxVersion?type=0";
                //cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
                //cmd.ShowOpaqueLayer(0f);
                this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    //cmd.HideOpaqueLayer();
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        ClientVersionEntity cv = objT["result"].ToObject<ClientVersionEntity>();
                        un = new UpdateNoticeForm();
                        un.cv = cv;
                        un.TopLevel = false;//这个很重要
                        this.Controls.Add(un);//找到外面的Form，添加InnerForm
                        un.Show();//显示InnerForm
                        int x = this.Width - 625;
                        int y = this.Height - 489;
                        un.Location = new Point(x, y);
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }
                });
            }
        }

        private void WelcomeForm_Resize(object sender, EventArgs e)
        {
            if (un != null)
            {
                int x = this.Width - 625;
                int y = this.Height - 489;
                un.Location = new Point(x, y);
            }
        }
    }
}
