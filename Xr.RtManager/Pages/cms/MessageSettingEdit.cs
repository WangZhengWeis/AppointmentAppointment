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
    public partial class MessageSettingEdit : Form
    {
        public MessageSettingEdit()
        {
            InitializeComponent();
        }
        #region 关闭
        /// <summary>
        /// 关闭
        /// </summary>
        public MessageInfoEntity messageInfoEntity { set; get; }
        private void butClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion
        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!dcMessage.Validate())
                {
                    return;
                }
                dcMessage.GetValue(messageInfoEntity);
                String param = "?" + PackReflectionEntity<MessageInfoEntity>.GetEntityToRequestParameters(messageInfoEntity,true);
                String url = AppContext.AppConfig.serverUrl + "cms/messageTemplate/save" + param;
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
        private void MessageSettingEdit_Load(object sender, EventArgs e)
        {
            dcMessage.DataType = typeof(MessageInfoEntity);
            if (messageInfoEntity != null)
            {
                dcMessage.SetValue(messageInfoEntity);
            }
            else
            {
                messageInfoEntity = new MessageInfoEntity();
            }
        }
        #endregion
    }
}
