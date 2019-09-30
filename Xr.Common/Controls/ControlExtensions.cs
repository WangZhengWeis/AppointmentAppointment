using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Xr.Common.Controls
{
    /// <summary>
    /// Control扩展类
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// 检查控件是否处于设计状态
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool IsDesignMode(this Component component)
        {
            if (component.Site != null && component.Site.DesignMode) return true;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
            if (Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV")) return true;

            return false;
        }

        /// <summary>
        /// 设置控件的双缓冲属性
        /// </summary>
        /// <param name="control">需要设置双缓冲的控件</param>
        /// <param name="value">是否启用双缓冲</param>
        public static void SetDoubleBuffered(this Control control, bool value)
        {
            var property = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null) property.SetValue(control, value, null);
        }
    }
}
