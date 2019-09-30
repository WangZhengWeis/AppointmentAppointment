using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xr.Common.Validation;
using Xr.Common.Common;

namespace Xr.Common.Validation
{
    public class IdCardNoValidator : Validator
    {
        public IdCardNoValidator(string messageTemplate) : base(messageTemplate) { }

        protected override string DefaultMessageTemplate
        {
            get { return "无效的身份证号码"; }
        }

        protected override bool ValidateCore(object target, ref string message)
        {
            //未填写身份证号，认为验证成功，防止出现身份证号为非必填项时，这里验证无法通过的情况出现
            if (target == null) return true;
            if (target.ToString() == string.Empty) return true;

            if (!new IdCardNoParser(target.ToString()).IsValid)
            {
                message = GetMessage();
                return false;
            }

            return true;
        }
    }
}
