using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Validation
{
    /// <summary>
    /// 必填项验证
    /// </summary>
    public class RequiredValidator : Validator
    {
        public RequiredValidator() : base(string.Empty) { }

        public RequiredValidator(string messageTemplate) : base(messageTemplate) { }

        protected override bool ValidateCore(object target, ref string message)
        {
            var result = target != null && target.ToString() != string.Empty;
            if (!result) message = GetMessage();

            return result;
        }

        protected override string DefaultMessageTemplate
        {
            get { return "请填写相关内容"; }
        }
    }
}
