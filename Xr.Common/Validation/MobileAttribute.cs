using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Validation
{
    /// <summary>
    /// 手机号码验证
    /// </summary>
    public class MobileAttribute : ValidateAttribute
    {
       public MobileAttribute():base()
       {}

       
       public MobileAttribute(string messageTemplate) : base(messageTemplate) { }

       public override Validator CreateValidator()
       {
           return new MobileValidator(this.MessageTemplate);
       }
    }
}
