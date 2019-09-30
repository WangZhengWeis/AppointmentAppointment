using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using Xr.Common.Controls;

namespace Xr.Common.Utils
{
    /// <summary>
    /// 用于将值写入到控件中
    /// </summary>
    public static class ControlValueWriter
    {
        /// <summary>
        /// 将值写入指定的控件
        /// </summary>
        /// <param name="control">需要写入值的控件</param>
        /// <param name="value">需要写入的值</param>
        public static void Write(Control control, object value)
        {
            foreach (var writer in valueWriters)
            {
                if (writer.Key.IsAssignableFrom(control.GetType()))
                {
                    writer.Value(control, value);
                    break;
                }
            }
        }

        private delegate void ValueWriter(Control control, object value);

        private static Dictionary<Type, ValueWriter> valueWriters = new Dictionary<Type, ValueWriter>();

        static ControlValueWriter()
        {
            RegisterValueWriter<DateEdit>(DateEditValueWriter);
            RegisterValueWriter<TimeEdit>(TimeEditValueWriter);
            RegisterValueWriter<ToggleSwitch>(ToggleSwitchValueWriter);
            //RegisterValueWriter<DictCheckedComboBoxEdit>(DictCheckedComboBoxEditValueWriter);
            RegisterValueWriter<BaseEdit>(BaseEditValueWriter);
            //RegisterValueWriter<DictCheckedListBoxControl>(DictCheckedListBoxControlValueWriter);
            RegisterValueWriter<Control>(BaseControlValueWriter);
            
        }

        private static void DictCheckedComboBoxEditValueWriter(Control control, object value)
        {
            //DictCheckedComboBoxEdit baseEdit = control as DictCheckedComboBoxEdit;
            //if (baseEdit == null) return;

            //baseEdit.EditValue = value;
        }

        private static void DictCheckedListBoxControlValueWriter(Control control, object value)
        {
            //DictCheckedListBoxControl baseEdit = control as DictCheckedListBoxControl;
            //if (baseEdit == null) return;

            //baseEdit.EditValue = (value==null?null:value.ToString());
        }

        /// <summary>
        /// 设置BaseEdit及其子类的值
        /// </summary>
        private static void BaseEditValueWriter(Control control, object value)
        {
            BaseEdit baseEdit = control as BaseEdit;
            if (baseEdit == null) return;

            baseEdit.EditValue = value;
        }

        /// <summary>
        /// 设置Control及其子类的值
        /// </summary>
        private static void BaseControlValueWriter(Control control, object value)
        {
            //var valuePopertyAttr = ReflectionUtils.GetAttribute<ValuePropertyAttribute>(control.GetType(), false);
            //if (valuePopertyAttr != null)
            //{
            //    var valueProperty = control.GetType().GetProperty(valuePopertyAttr.PropertyName);
            //    if (valueProperty != null && valueProperty.CanWrite)
            //    {
            //        valueProperty.SetValue(control, value, null);
            //    }
            //}

            if (value != null)
            {
                if (value.GetType() == typeof(DateTime))
                {
                    control.Text = ((DateTime)value).ToString("yyyy-MM-dd");
                }
                else
                {
                    control.Text = value.ToString();
                }
            }
            else
            {
                control.Text = string.Empty;
            }
        }

        private static void DateEditValueWriter(Control control, object value)
        {
            DateEdit dateEdit = control as DateEdit;

            if (value != null && value.GetType() == typeof(string))  //外界传入的类型为string类型，转换成日期类型
            {
                DateTime dt;
                if (DateTime.TryParse(value as string, out dt))
                {
                    dateEdit.EditValue = dt;
                }
                else
                {
                    dateEdit.EditValue = null;
                }
            }
            else
            {
                dateEdit.EditValue = value;
            }
        }

        private static void TimeEditValueWriter(Control control, object value)
        {
            var timeEdit = control as TimeEdit;

            if (value != null && value.GetType() == typeof(string))
            {
                var s = value.ToString();
                if (s.Length == 5) timeEdit.EditValue = DateTime.ParseExact(s, "HH:mm", null);
                else if (s.Length == 8) timeEdit.EditValue = DateTime.ParseExact(s, "HH:mm:ss", null);
            }
            else
            {
                timeEdit.EditValue = value;
            }
        }

        private static void ToggleSwitchValueWriter(Control control, object value)
        {
            ToggleSwitch toggleSwitch = control as ToggleSwitch;

            if (value.GetType() == typeof(string))
            {
                toggleSwitch.EditValue = string.Compare(value.ToString(), toggleSwitch.Properties.OnText, true) == 0;
            }
            else
            {
                toggleSwitch.EditValue = value;
            }
        }

        private static void RegisterValueWriter<T>(ValueWriter valueWriter) where T : Control
        {
            valueWriters.Add(typeof(T), valueWriter);
        }
    }
}
