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
    public partial class ClinicSettingEdit : Form
    {
        public ClinicSettingEdit()
        {
            InitializeComponent();
        }
        #region 关闭
        private void butClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion 
        #region 保存
        public ClinicInfoEntity clinicInfoEntity { get; set; }
        private void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!dcClinic.Validate())
                {
                    return;
                }
                dcClinic.GetValue(clinicInfoEntity);
                String param = "?" + PackReflectionEntity<ClinicInfoEntity>.GetEntityToRequestParameters(clinicInfoEntity,true);
                String url = AppContext.AppConfig.serverUrl + "cms/clinic/save" + param;
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
        #region 窗体Load事件
        private void ClinicSettingEdit_Load(object sender, EventArgs e)
        {
            dcClinic.DataType = typeof(ClinicInfoEntity);
            if (clinicInfoEntity != null)
            {
                dcClinic.SetValue(clinicInfoEntity);
            }
            else
            {
                clinicInfoEntity = new ClinicInfoEntity();
            }
        }
        #endregion 
    }
}
