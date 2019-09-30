using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Validation
{
    public class MailboxValidator : Validator
    {
        public MailboxValidator(string messageTemplate) : base(messageTemplate) { }

        protected override string DefaultMessageTemplate
        {
            get { return "请输入有效的邮箱号码！"; }
        }
        protected override bool ValidateCore(object target, ref string message)
        {
            if (target == null) return true;
            if (target.ToString() == string.Empty) return true;
            if (!System.Text.RegularExpressions.Regex.IsMatch(target.ToString(), @"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z0-9]{2,6}$"))
            {
                message = GetMessage();
                return false;
            }
            return true;
        }
    }
}
