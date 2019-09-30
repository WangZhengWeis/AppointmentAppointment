using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Xr.Common.Controls;

namespace Xr.Common.Utils
{
    /// <summary>
    /// 快捷键管理
    /// </summary>
    public class HotkeysManager
    {
        private List<HotkeysInfo> hotkeysItems = new List<HotkeysInfo>();

        /// <summary>
        /// 清空所有的快捷键
        /// </summary>
        public void Clear()
        {
            hotkeysItems.Clear();
        }

        /// <summary>
        /// 触发指定快捷键对应的操作，指定的快捷键有对应操作时，返回true，否则返回false
        /// </summary>
        /// <param name="keys">快捷键键位</param>
        /// <returns></returns>
        public bool Handle(Keys keys, ref bool suppressKeyPress)
        {
            foreach (var item in hotkeysItems)
            {
                if (item.Keys == keys)
                {
                    item.Action();
                    suppressKeyPress = item.SuppressKeyPress;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 注册快捷键
        /// </summary>
        /// <param name="hotkeysRegistration">注册快捷键接口</param>
        /// <returns></returns>
        public HotkeysManager Register(IHotkeysRegistration hotkeysRegistration)
        {
            if (hotkeysRegistration != null) hotkeysRegistration.RegisterHotkeys(this);

            return this;
        }

        /// <summary>
        /// 注册快捷键
        /// </summary>
        /// <param name="keys">快捷键键位</param>
        /// <param name="description">快捷键说明</param>
        /// <param name="action">快捷键对应操作</param>
        public HotkeysManager Register(Keys keys, string description, Action action)
        {
            return Register(keys, description, false, action);
        }

        /// <summary>
        /// 注册快捷键
        /// </summary>
        /// <param name="keys">快捷键键位</param>
        /// <param name="description">快捷键说明</param>
        /// <param name="suppressKeyPress">执行快捷键后，是否取消按键输入</param>
        /// <param name="action">快捷键对应操作</param>
        public HotkeysManager Register(Keys keys, string description, bool suppressKeyPress, Action action)
        {
            hotkeysItems.Insert(0, new HotkeysInfo()
            {
                Keys = keys,
                Action = action,
                SuppressKeyPress = suppressKeyPress,
                Description = description
            });

            return this;
        }

        /// <summary>
        /// 获取当前已经注册的快捷键数量
        /// </summary>
        internal int Count
        {
            get { return hotkeysItems.Count; }
        }

        /// <summary>
        /// 从控件中发现并注册快捷键
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        internal HotkeysManager Register(System.Windows.Forms.Control control)
        {
            //查找并注册按钮快捷键
            var button = control as IButtonControl;
            var supportHotkeys = control as ISupportHotkeys;
            if (button != null && supportHotkeys != null && supportHotkeys.Hotkeys != Keys.None)
            {
                Register(supportHotkeys.Hotkeys, control.Text, button.PerformClick);
            }

            if (control is IHotkeysRegistration)
            {
                Register(control as IHotkeysRegistration);
            }

            //从子控件中查找
            foreach (var c in control.Controls)
            {
                if (c is System.Windows.Forms.Control) Register(c as System.Windows.Forms.Control);
            }

            return this;
        }

        private class HotkeysInfo
        {
            public Keys Keys { get; set; }

            public Action Action { get; set; }

            public bool SuppressKeyPress { get; set; }

            public string Description { get; set; }
        }
    }
}
