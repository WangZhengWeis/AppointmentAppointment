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
using System.Runtime.InteropServices;
using Xr.Http;
using System.Dynamic;

namespace Xr.RtManager
{
    public partial class TestForm : UserControl
    {
        //[DllImport("user32.dll")]
        //public static extern IntPtr GetActiveWindow();

        //[DllImport("user32.dll")]
        //public static extern IntPtr SetActiveWindow(IntPtr hwnd);
        


        public TestForm()
        {
            InitializeComponent();
        }

        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        private JObject obj { get; set; }

        private void LogForm_Load(object sender, EventArgs e)
        {
            List<DeptEntity> deptLsit = AppContext.Session.deptList;
            menuTest1.KeyFieldName = "id";
            menuTest1.ParentFieldName = "parentId";
            menuTest1.DisplayMember = "name";
            menuTest1.ValueMember = "id";
            menuTest1.DataSource = deptLsit;
            menuTest1.EditValue = "23";
            DeptEntity dept = menuTest1.selectItem as DeptEntity;
        }


        private void skinButton1_Click(object sender, EventArgs e)
        { 
            cmd.ShowOpaqueLayer();
          
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
                    throw new Exception(ex.InnerException.Message);
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

        private void LogForm_Resize(object sender, EventArgs e)
        {
            //cmd.rectDisplay = this.DisplayRectangle;
        }
        


    }
}