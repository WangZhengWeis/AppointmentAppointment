using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Validation
{
    /// <summary>
    /// 邮箱验证并且不为空
    /// </summary>
    public class MailboxAttribute : ValidateAttribute
    {
       public MailboxAttribute():base()
       {}


       public MailboxAttribute(string messageTemplate) : base(messageTemplate) { }

       public override Validator CreateValidator()
       {
           return new MailboxValidator(this.MessageTemplate);
       }
    }
}
