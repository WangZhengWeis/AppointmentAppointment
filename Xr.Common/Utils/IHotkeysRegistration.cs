using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Utils
{
    /// <summary>
    /// 用于向外界进行快捷键注册
    /// </summary>
    public interface IHotkeysRegistration
    {
        /// <summary>
        /// 注册快捷键
        /// </summary>
        /// <param name="hotkeys"></param>
        void RegisterHotkeys(HotkeysManager hotkeys);
    }
}
