using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Xr.Common.Common;
using Xr.Common.Validation;

using Xr.Common.Internal;
using Xr.Common.Utils;

namespace Xr.Common
{
    /// <summary>
    /// 验证管理器，用于进行界面输入值的验证处理
    /// </summary>
    public class ValidatorManager
    {
        private ValidateErrorNotifier errorNotifier = new ValidateErrorNotifier();
        private List<ControlValidationInfo> validationInfos = new List<ControlValidationInfo>();

        /// <summary>
        /// 对于指定的控件添加验证器
        /// </summary>
        /// <param name="control">需要添加验证器的控件</param>
        /// <param name="validators">控件验证器集合</param>
        /// <returns></returns>
        public ValidatorManager Add(Control control, params Validator[] validators)
        {
            var validationInfo = validationInfos.FirstOrDefault(v => v.Control == control);
            if (validationInfo == null)
            {
                validationInfo = new ControlValidationInfo(control);
                validationInfos.Add(validationInfo);
            }

            validationInfo.Validators.AddRange(validators);

            return this;
        }

        /// <summary>
        /// 对于指定的控件添加必填项验证器
        /// </summary>
        /// <param name="controls">需要添加必填项验证器的控件集合</param>
        /// <returns></returns>
        public ValidatorManager AddRequiredValidator(params Control[] controls)
        {
            foreach (var control in controls)
            {
                Add(control, new RequiredValidator());
            }

            return this;
        }

        /// <summary>
        /// 对于指定的控件添加必填项验证器
        /// </summary>
        /// <param name="control">需要添加验证器的控件</param>
        /// <param name="messageTemplate">控件内容未填写时提示的错误信息</param>
        /// <returns></returns>
        public ValidatorManager AddRequiredValidator(Control control, string messageTemplate)
        {
            return Add(control, new RequiredValidator(messageTemplate));
        }

        /// <summary>
        /// 清除验证错误信息
        /// </summary>
        public void ClearError()
        {
            errorNotifier.ClearError();
        }

        /// <summary>
        /// 在指定的控件边上显示错误信息
        /// </summary>
        /// <param name="control"></param>
        /// <param name="message"></param>
        public void ShowError(Control control, string message)
        {
            errorNotifier.ShowError(control, message);
        }

        /// <summary>
        /// 进行控件值的验证，成功返回true，失败返回false
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            errorNotifier.ClearError();

            foreach (var validationInfo in validationInfos)
            {
                if (validationInfo.Control.Visible && validationInfo.Control.Enabled)
                {
                    if (!Validate(validationInfo))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 对指定的控件进行验证
        /// </summary>
        /// <param name="control">需要进行验证的控件</param>
        /// <returns></returns>
        public bool Validate(Control control)
        {
            errorNotifier.ClearError();
            return Validate(validationInfos.FirstOrDefault(v => v.Control == control));
        }

        private bool Validate(ControlValidationInfo validationInfo)
        {
            if (validationInfo == null) return true;

            object value = ControlValueReader.Read(validationInfo.Control, typeof(object));

            string message = string.Empty;
            foreach (var validator in validationInfo.Validators)
            {
                if (!validator.Validate(value, ref message))
                {
                    errorNotifier.ShowError(validationInfo.Control, message);
                    if (validationInfo.Control.CanFocus) validationInfo.Control.Focus();

                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// 控件验证信息
    /// </summary>
    internal class ControlValidationInfo
    {
        internal ControlValidationInfo(Control control)
        {
            this.Control = control;
            this.Validators = new List<Validator>();
        }

        /// <summary>
        /// 获取需要进行验证的控件
        /// </summary>
        public Control Control { get; private set; }

        /// <summary>
        /// 获取控件上的验证器集合
        /// </summary>
        public List<Validator> Validators { get; private set; }
    }
}
