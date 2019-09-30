using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace Xr.RtManager.Utils
{
    class BackgroundWorkerUtil
    {

        public delegate void ComparisonDoWork(object sender, DoWorkEventArgs e);
        public delegate void ComparisonRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e);
        public delegate void ComparisonProgressChanged(object sender, ProgressChangedEventArgs e);

        /// <summary>
        /// BackgroundWorker异步处理
        /// </summary>
        /// <param name="bw_DoWork">异步执行方法</param>
        /// <param name="bw_RunWorkerCompleted">异步完成后执行的方法</param>
        /// <param name="bw_ProgressChanged">进度条方法</param>
        /// <param name="isCancel">是否允许取消异步操作</param>
        public static void start_run(ComparisonDoWork bw_DoWork, ComparisonRunWorkerCompleted bw_RunWorkerCompleted, ComparisonProgressChanged bw_ProgressChanged, Boolean isCancel)
        {
            // BackgroundWorker对象
            BackgroundWorker bw = new BackgroundWorker();
            // 执行：执行指定的事件
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            // 进度：执行过程中进行的事件
            if (bw_ProgressChanged!=null)
            {
                bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged); 
                bw.WorkerReportsProgress = true; // 是否报告进度更新
            }
            // 完成：执行完成后需要进行的事件
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            // 声明是否可以取消
            bw.WorkerSupportsCancellation = isCancel;

            // 启动：可以传递参数，在DoWork事件中用e.Argument接收参数
            bw.RunWorkerAsync(); 
        }

    }
}
