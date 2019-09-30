using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Validation
{
    /// <summary>
    /// 验证器基类
    /// </summary>
    public abstract class Validator
    {
        private string messageTemplate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageTemplate">验证失败时的提示消息模板</param>
        protected Validator(string messageTemplate)
        {
            this.messageTemplate = messageTemplate;
        }

        /// <summary>
        /// 对指定的值进行验证，验证成功时返回true，失败时返回false，验证错误信息由message参数返回
        /// </summary>
        /// <param name="target">需要进行验证的对象</param>
        /// <param name="message">验证失败时的错误信息</param>
        /// <returns></returns>
        public bool Validate(object target, ref string message)
        {
            return ValidateCore(target, ref message);
        }

        protected abstract bool ValidateCore(object target, ref string message);

        /// <summary>
        /// 获取验证失败时的默认提示消息，当未指定验证失败时的提示消息时，默认返回该消息
        /// </summary>
        /// <returns></returns>
        protected virtual string DefaultMessageTemplate
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// 获取验证失败时的提示消息
        /// </summary>
        /// <returns></returns>
        protected string GetMessage()
        {
            var s = messageTemplate;
            if (string.IsNullOrEmpty(s)) s = this.DefaultMessageTemplate;

            return s;
        }
    }
}
