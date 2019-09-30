using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xr.Common.Controls
{
    /// <summary>
    /// 表示控件支持快捷键处理
    /// </summary>
    public interface ISupportHotkeys
    {
        Keys Hotkeys { get; set; }
    }
}
