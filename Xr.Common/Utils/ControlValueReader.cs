using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Xr.Common.Common;
using Xr.Common.Controls;

namespace Xr.Common
{
    /// <summary>
    /// 用于进行控件值的读取操作
    /// </summary>
    public static class ControlValueReader
    {
        /// <summary>
        /// 读取指定控件的值
        /// </summary>
        /// <param name="control">需要进行操作的控件</param>
        /// <param name="returnType">需要返回的值类型</param>
        /// <returns></returns>
        public static object Read(Control control, Type returnType)
        {
            foreach (var reader in valueReaders)
            {
                if (reader.Key.IsAssignableFrom(control.GetType()))
                {
                    var value = reader.Value(control, returnType);

                    //对于数值类型，将null和空字符串转换成0
                    if (value == null || value.ToString() == string.Empty)
                    {
                        if (returnType == typeof(int) || returnType == typeof(decimal) || returnType == typeof(float) || returnType == typeof(double))
                        {
                            value = 0;
                        }
                    }

                    //类型不匹配，进行类型转换
                    if (value != null && !value.GetType().IsAssignableFrom(returnType))
                    {
                        if (returnType.IsEnum)  //处理枚举类型
                        {
                            value = EnumUtils.FromCode(returnType, value.ToString());
                        }
                        else
                        {
                            value = Convert.ChangeType(value, returnType);
                        }
                    }

                    return value;
                }
            }

            return null;
        }

        private delegate object ValueReader(Control control, Type returnType);

        private static Dictionary<Type, ValueReader> valueReaders = new Dictionary<Type, ValueReader>();

        static ControlValueReader()
        {
            RegisterValueReader<DateEdit>(DateEditValueReader);
            RegisterValueReader<TimeEdit>(TimeEditValueReader);
            RegisterValueReader<ComboBoxEdit>(ComboBoxEditValueReader);
            RegisterValueReader<ToggleSwitch>(ToggleSwitchValueReader);
            RegisterValueReader<SpinEdit>(SpinEditValueReader);
            
            //这里注释了两行
            //RegisterValueReader<DictCheckedComboBoxEdit>(DictCheckedComboBoxEditValueReader);
            //RegisterValueReader<DictCheckedListBoxControl>(DictCheckedListBoxControlValueReader);
            RegisterValueReader<BaseEdit>(BaseEditValueReader);
            RegisterValueReader<Control>(BaseControlValueReader);
        }

        private static object DictCheckedComboBoxEditValueReader(Control control, Type returnType)
        {
            return null;
            //return (control as DictCheckedComboBoxEdit).EditValue;
        }

        private static object DictCheckedListBoxControlValueReader(Control control, Type returnType)
        {
            return null;
            //return (control as DictCheckedListBoxControl).EditValue;
        }

        /// <summary>
        /// 获取BaseEdit及其子类的值
        /// </summary>
        /// <returns></returns>
        private static object BaseEditValueReader(Control control, Type returnType)
        {
            return (control as BaseEdit).EditValue;
        }

        /// <summary>
        /// 获取Control及其子类的值
        /// </summary>
        /// <returns></returns>
        private static object BaseControlValueReader(Control control, Type returnType)
        {
            //var valuePopertyAttr = ReflectionUtils.GetAttribute<ValuePropertyAttribute>(control.GetType(), false);
            //if (valuePopertyAttr != null)
            //{
            //    var valueProperty = control.GetType().GetProperty(valuePopertyAttr.PropertyName);
            //    if (valueProperty != null && valueProperty.CanRead)
            //    {
            //        return valueProperty.GetValue(control, null);
            //    }
            //}

            return control.Text;
        }

        private static object ComboBoxEditValueReader(Control control, Type returnType)
        {
            var value = (control as ComboBoxEdit).EditValue;
            return value != null ? value.ToString() : string.Empty;
        }

        private static object DateEditValueReader(Control control, Type returnType)
        {
            var dateEdit = control as DateEdit;

            var value = dateEdit.EditValue;
            if (value == null) return value;

            if (returnType == typeof(string))  //外界要求以string类型返回
            {
                var formatString = dateEdit.Properties.EditFormat.FormatString;
                if (string.IsNullOrEmpty(formatString)) formatString = "yyyy-MM-dd";

                return ((DateTime)value).ToString(formatString);
            }
            else
            {
                return value;
            }
        }

        private static object TimeEditValueReader(Control control, Type returnType)
        {
            var timeEdit = control as TimeEdit;

            if (returnType == typeof(string))
            {
                var formatString = timeEdit.Properties.EditFormat.FormatString;
                if (string.IsNullOrEmpty(formatString)) formatString = "HH:mm";

                return timeEdit.Time.ToString(formatString);
            }

            return timeEdit.Time;
        }

        private static object SpinEditValueReader(Control control, Type returnType)
        {
            var value = (control as SpinEdit).Value;
            if (returnType == typeof(int)) return Convert.ToInt32(value);

            return value;
        }

        private static object ToggleSwitchValueReader(Control control, Type returnType)
        {
            var toggleSwitch = control as ToggleSwitch;
            var value = toggleSwitch.IsOn;

            if (returnType == typeof(string))
            {
                return value ? toggleSwitch.Properties.OnText : toggleSwitch.Properties.OffText;
            }
            else
            {
                return value;
            }
        }

        private static void RegisterValueReader<T>(ValueReader valueReader) where T : Control
        {
            valueReaders.Add(typeof(T), valueReader);
        }
    }
}
