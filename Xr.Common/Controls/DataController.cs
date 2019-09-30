using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Xr.Common.Common;
using Xr.Common.Validation;
using Xr.Common.Utils;

namespace Xr.Common.Controls
{
    /// <summary>
    /// 数据控制器组件，用于对控件进行数据绑定、验证等操作
    /// </summary>
    [ProvideProperty("DataMember", typeof(Control))]
    public partial class DataController : Component, IExtenderProvider
    {
        private List<DataBindingItem> dataBindingItems = new List<DataBindingItem>();
        private ValidatorManager validatorManager = new ValidatorManager();
        private Type dataType = null;

        public DataController()
        {
            InitializeComponent();
        }

        public DataController(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// 获取或设置必填项标记控件
        /// </summary>
        [DefaultValue(null)]
        public RequiredMarker RequiredMarker { get; set; }

        /// <summary>
        /// 获取或设置数据控制器对应的数据类型
        /// </summary>
        [DefaultValue(null)]
        [Browsable(false)]
        public Type DataType
        {
            get { return dataType; }
            set
            {
                if (dataType != value)
                {
                    dataType = value;
                    ShowRequiredMarker();
                    AddValidators();
                }
            }
        }

        /// <summary>
        /// 获取或设置是否将空字符串转换成null
        /// </summary>
        [DefaultValue(false)]
        [Description("是否将空字符串转换成null")]
        public bool ConvertEmptyStringToNull { get; set; }

        public bool CanExtend(object extendee)
        {
            return extendee is Control;
        }

        /// <summary>
        /// 设置与控件进行绑定的数据成员
        /// </summary>
        /// <param name="control">需要进行绑定的控件</param>
        /// <param name="dataMember">数据成员</param>
        public void SetDataMember(Control control, string dataMember)
        {
            var dataBindingItem = dataBindingItems.FirstOrDefault(item => item.Control == control);

            if (string.IsNullOrEmpty(dataMember))
            {
                if (dataBindingItem != null)
                {
                    dataBindingItems.Remove(dataBindingItem);
                }
            }
            else
            {
                if (dataBindingItem == null)
                {
                    dataBindingItem = new DataBindingItem();
                    dataBindingItem.Control = control;

                    dataBindingItems.Add(dataBindingItem);
                }

                dataBindingItem.DataMember = dataMember;
            }
        }

        /// <summary>
        /// 获取与控件进行绑定的数据成员
        /// </summary>
        /// <param name="control">需要获取绑定的控件</param>
        /// <returns></returns>
        [DefaultValue("")]
        public string GetDataMember(Control control)
        {
            var dataBindingItem = dataBindingItems.FirstOrDefault(item => item.Control == control);
            return dataBindingItem != null ? dataBindingItem.DataMember : string.Empty;
        }

        /// <summary>
        /// 在指定的控件上显示错误信息
        /// </summary>
        /// <param name="control">需要显示错误信息的控件</param>
        /// <param name="message">需要显示的错误信息</param>
        public void ShowError(Control control, string message)
        {
            validatorManager.ShowError(control, message);
        }

        /// <summary>
        /// 进行数据验证操作，验证成功返回true，失败返回false
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            return validatorManager.Validate();
        }

        /// <summary>
        /// 从控件中获取指定对象的属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void GetValue<T>(T value)
        {
            foreach (var property in typeof(T).GetProperties().Where(p => p.CanWrite))
            {
                var dataBindingItem = dataBindingItems.FirstOrDefault(item => string.Compare(item.DataMember, property.Name, true) == 0);
                if (dataBindingItem != null)
                {
                    var returnType = property.PropertyType;
                    if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Nullable<>)) returnType = returnType.GetGenericArguments()[0];  //处理可空类型

                    var controlValue = ControlValueReader.Read(dataBindingItem.Control, returnType);

                    //将空字符串转换成null
                    if (this.ConvertEmptyStringToNull && controlValue != null && property.PropertyType == typeof(string) && controlValue.ToString() == string.Empty)
                    {
                        controlValue = null;
                    }

                    property.SetValue(value, controlValue, null);
                }
            }
        }

        /// <summary>
        /// 将指定对象的属性值设置到控件中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void SetValue<T>(T value)
        {
            if (value == null)
            {
                ClearValue();
                return;
            }

            foreach (var property in typeof(T).GetProperties().Where(p => p.CanRead))
            {
                foreach (var dataBindingItem in dataBindingItems.Where(item => string.Compare(item.DataMember, property.Name, true) == 0))
                {
                    ControlValueWriter.Write(dataBindingItem.Control, property.GetValue(value, null));
                }
            }
        }

        /// <summary>
        /// 清空控件中的值
        /// </summary>
        public void ClearValue()
        {
            foreach (var dataBindingItem in dataBindingItems)
            {
                ControlValueWriter.Write(dataBindingItem.Control, null);
            }
        }

        /// <summary>
        /// 将控制器中的数据与指定对象中的数据进行比较，如果数据发生改变，返回true，否则返回false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">需要进行比较的对象</param>
        /// <returns></returns>
        public bool CheckChanged<T>(T value)
        {
            foreach (var property in typeof(T).GetProperties().Where(p => p.CanRead))
            {
                foreach (var dataBindingItem in dataBindingItems.Where(item => string.Compare(item.DataMember, property.Name, true) == 0))
                {
                    var controlValue = ControlValueReader.Read(dataBindingItem.Control, property.PropertyType);
                    var propertyValue = property.GetValue(value, null);

                    if (controlValue != null && !controlValue.Equals(propertyValue)) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 从指定的类型中发现必填项，并显示必填项标记
        /// </summary>
        /// <param name="objectType"></param>
        private void ShowRequiredMarker()
        {
            if (this.DataType == null) return;

            if (this.RequiredMarker == null) this.RequiredMarker = new RequiredMarker();

            foreach (var dataBindingItem in dataBindingItems)
            {
                var property = this.DataType.GetProperty(dataBindingItem.DataMember);  //查询数据成员对应的属性名称
                if (property != null)
                {
                    if (ReflectionUtils.GetAttribute<RequiredAttribute>(property, true) != null && !this.RequiredMarker.GetVisible(dataBindingItem.Control))
                    {
                        this.RequiredMarker.SetVisible(dataBindingItem.Control, true);
                    }
                }
            }

            this.RequiredMarker.ShowMarker();
        }

        /// <summary>
        /// 从指定的类型中发现并添加验证器
        /// </summary>
        /// <param name="objectType"></param>
        private void AddValidators()
        {
            if (this.DataType == null) return;

            foreach (var property in this.DataType.GetProperties().Where(p => p.CanWrite))
            {
                var validatorAttributes = ReflectionUtils.GetAttributes<ValidateAttribute>(property, true);
                if (validatorAttributes.Length > 0)
                {
                    foreach (var dataBindingItem in dataBindingItems.Where(item => string.Compare(item.DataMember, property.Name, true) == 0))
                    {
                        validatorManager.Add(dataBindingItem.Control, validatorAttributes.Select(v => v.CreateValidator()).ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// 数据绑定项信息
        /// </summary>
        private class DataBindingItem
        {
            /// <summary>
            /// 获取或设置需要进行绑定的控件
            /// </summary>
            public Control Control { get; set; }

            /// <summary>
            /// 获取或设置绑定的数据成员名称
            /// </summary>
            public string DataMember { get; set; }
        }
    }
}
