using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Validation
{
    /// <summary>
    /// 用于表示其修饰的属性为必填项
    /// </summary>
    public class RequiredAttribute : ValidateAttribute
    {
        public RequiredAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageTemplate">其修饰的属性未填写值时的提示信息</param>
        public RequiredAttribute(string messageTemplate) : base(messageTemplate) { }

        public override Validator CreateValidator()
        {
            return new RequiredValidator(this.MessageTemplate);
        }
    }
}
