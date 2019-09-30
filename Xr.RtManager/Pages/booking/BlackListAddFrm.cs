using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json.Linq;
using DevExpress.XtraEditors;
using System.IO;
using System.Net;
using Xr.Common;
using System.Text.RegularExpressions;
using Xr.Http;

namespace Xr.RtManager
{
    public partial class BlackListAddFrm : Form
    {
        public BlackListAddFrm()
        {
            InitializeComponent();
        }

        public UserEntity user { get; set; }
        //private String oldLoginName;
        //String filePath = "";
        //String serviceFilePath = "";

        //CheckBox chkbLast;

        private void UserEdit_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txt_patientId.Text == "")
            {
                MessageBoxUtils.Hint("请填写患者ID", HintMessageBoxIcon.Error, this);
                return ;
            }
            if (txt_patientName.Text == "")
            {
                MessageBoxUtils.Hint("请填写患者姓名", HintMessageBoxIcon.Error, this);
                return ;
            }
            if (txt_phone.Text == "")
            {
                MessageBoxUtils.Hint("请填写电话", HintMessageBoxIcon.Error, this);
                return ;
            }
            if (txt_breakTimes.Text == "")
            {
                MessageBoxUtils.Hint("请填写爽约次数", HintMessageBoxIcon.Error, this);
                return ;
            }
            string sex = "1";
            if (rbt_sexM.Checked)
            {
                sex = "1";
            }
            else
            {
                sex = "0";
            }
            string isUse = "0";
            if (rbt_isUseT.Checked)
            {
                isUse = "0";
            }
            else
            { 
                isUse = "1"; 
            }
            //hospitalId=12&patientId=000693943600&patientName=曹昌平&sex=1&phone=15013369764&isUse=0&breakTimes=6
            String param = "hospitalId={0}&patientId={1}&patientName={2}&sex={3}&phone={4}&isUse={5}&breakTimes={6}";
            param = String.Format(
                param,AppContext.Session.hospitalId,
                txt_patientId.Text,
                txt_patientName.Text,
                sex,
                txt_phone.Text,
                isUse,
                txt_breakTimes.Text
                );
             
            String url = AppContext.AppConfig.serverUrl + "sch/blackList/save?" + param;
            String data = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(data);
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

       
    }
}
