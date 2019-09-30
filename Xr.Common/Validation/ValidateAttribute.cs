using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Validation
{
    /// <summary>
    /// 验证器属性基类
    /// </summary>
    public abstract class ValidateAttribute : Attribute
    {
        protected ValidateAttribute() : this(string.Empty) { }


        protected ValidateAttribute(string messageTemplate)
        {
            this.MessageTemplate = messageTemplate;
        }

        /// <summary>
        /// 获取验证失败时的错误信息
        /// </summary>
        public string MessageTemplate { get; private set; }

        /// <summary>
        /// 创建与当前属性相关联的验证器对象
        /// </summary>
        /// <returns></returns>
        public abstract Validator CreateValidator();
    }
}
