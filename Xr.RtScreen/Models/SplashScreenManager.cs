using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Xr.RtScreen.Models
{
    /// <summary>
    /// 另起一个线程跑Loading SplashScreen窗口，由主进程执行耗时操作
    /// </summary>
    public class SplashScreenManager
    {
        private Form LoadingForm;

        /// <summary>
        /// 初始化SplashScreenManager，需要传入一个Form窗体对象
        /// </summary>
        /// <param name="ParentForm">The Parent Form of LoadingForm </param>
        /// <param name="loadControl">LoadingForm To Show</param>
        public SplashScreenManager(Form LoadingForm)
        {
            this.LoadingForm = LoadingForm;
        }

        private void ShowWaitForm()
        {
            LoadingForm.BringToFront();
            LoadingForm.Activate();
            LoadingForm.ShowDialog();
        }

        /// <summary>
        /// 显示加载窗体
        /// </summary>
        public void ShowLoading()
        {
            MethodInvoker invoker = new MethodInvoker(ShowWaitForm);
            invoker.BeginInvoke(null, null);
            while (!LoadingForm.IsHandleCreated)
            {
            }
            LoadingForm.Invoke(new MethodInvoker(() =>
            {
                LoadingForm.BringToFront();
                LoadingForm.Activate();
            }));
        }

        /// <summary>
        /// 关闭loading窗体
        /// </summary>
        public void CloseWaitForm()
        {
            var err_count = 0;
            try
            {
                LoadingForm.Invoke(new MethodInvoker(() =>
                {
                    LoadingForm.Close();
                }));
            }
            catch (Exception)
            {
                var isOK = false;
                err_count++;
                while (!isOK && err_count < 20)
                {
                    try
                    {
                        isOK = true;
                        LoadingForm.Invoke(new MethodInvoker(() =>
                        {
                            LoadingForm.Close();
                        }));
                    }
                    catch (Exception)
                    {
                        isOK = false;
                        err_count++;
                    }
                }
            }
            finally
            {
            }
        }
    }
}
