using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Common
{
    /// <summary>
    /// 工作进度事件参数
    /// </summary>
    public class WorkingProgressEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">当前进度值</param>
        /// <param name="maximum">最大进度值</param>
        /// <param name="description">进度说明</param>
        public WorkingProgressEventArgs(int value, int maximum, string description)
        {
            this.Value = value;
            this.Maximum = maximum;
            this.Description = description;
        }

        /// <summary>
        /// 当前进度值
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// 最大进度值
        /// </summary>
        public int Maximum { get; private set; }

        /// <summary>
        /// 进度说明
        /// </summary>
        public string Description { get; private set; }
    }
}
