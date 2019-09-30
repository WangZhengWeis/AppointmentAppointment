using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.Http;

namespace Xr.RtManager.Pages.cms
{
    public partial class HolidaySettingEdit : Form
    {
        public HolidaySettingEdit()
        {
            InitializeComponent();
        }
        #region 保存
        public HolidayInfoEntity holidayInfoEntity { get; set; }
        private void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!dcHolday.Validate())
                {
                    return;
                }
                dcHolday.GetValue(holidayInfoEntity);
                String param = "?" + PackReflectionEntity<HolidayInfoEntity>.GetEntityToRequestParameters(holidayInfoEntity,true);
                String url = AppContext.AppConfig.serverUrl + "cms/holiday/save" + param;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(objT["message"].ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        #region 关闭
        private void butClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion 
        #region 窗体Load事件
        private void HolidaySettingEdit_Load(object sender, EventArgs e)
        {
            dcHolday.DataType = typeof(HolidayInfoEntity);
            if (holidayInfoEntity != null)
            {
                dcHolday.SetValue(holidayInfoEntity);
            }
            else
            {
                holidayInfoEntity = new HolidayInfoEntity();
            }
        }
        #endregion 
    }
}
