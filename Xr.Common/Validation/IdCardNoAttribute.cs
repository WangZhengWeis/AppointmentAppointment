using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Validation
{
    /// <summary>
    /// 表示其修饰的属性需要进行身份证号验证
    /// </summary>
    public class IdCardNoAttribute : ValidateAttribute
    {
        public IdCardNoAttribute() : base() { }

        public IdCardNoAttribute(string messageTemplate) : base(messageTemplate) { }

        public override Validator CreateValidator()
        {
            return new IdCardNoValidator(this.MessageTemplate);
        }
    }
}
