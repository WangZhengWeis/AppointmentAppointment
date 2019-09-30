using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.Http;
using Newtonsoft.Json.Linq;
using Xr.Common;
using System.Threading;

namespace Xr.RtManager.Pages.cms
{
    public partial class MessageManagement : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public MessageManagement()
        {
            InitializeComponent();
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(225, false);
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            MessageContentTemplateList(1,pageControl1.PageSize);
            TemplateType();
            Thread.Sleep(500);
        }
        #region 测试
        public List<MessageInfoEntity> Date = new List<MessageInfoEntity>();
        #endregion 
        #region 消息内容模版列表
        /// <summary>
        /// 消息内容模版列表
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="hospitalId"></param>
        public void MessageContentTemplateList(int pageNo, int pageSize)
        {
            try
            {
                String url = AppContext.AppConfig.serverUrl + "cms/messageTemplate/list?pageNo=" + pageNo + "&pageSize=" + pageSize + "&hospital.id=" + AppContext.Session.hospitalId;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    this.gc_Message.DataSource = objT["result"]["list"].ToObject<List<MessageInfoEntity>>();
                    pageControl1.setData(int.Parse(objT["result"]["count"].ToString()),
                    int.Parse(objT["result"]["pageSize"].ToString()),
                    int.Parse(objT["result"]["pageNo"].ToString()));
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    cmd.HideOpaqueLayer();
                }
            }
            catch (Exception ex)
            {
                cmd.HideOpaqueLayer();
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("获取消息内容模版列表错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 查询消息模板详情
        /// <summary>
        /// 查询消息模板详情
        /// </summary>
        /// <param name="id">id</param>
        public void QueryMessageTemplateDetails(string id)
        {
            try
            {
                String url = AppContext.AppConfig.serverUrl + "api/cms/messageTemplate/findById?id=" + id;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("查询消息模板详情错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 获取指定医院下消息模板列表
        /// <summary>
        /// 获取指定医院下消息模板列表
        /// </summary>
        /// <param name="id">医院ID</param>
        public void GetSpecifiedHospital(string id)
        {
            try
            {
                String url = AppContext.AppConfig.serverUrl + "api/cms/messageTemplate/findAll?hospital.id=" + id;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("获取指定医院下消息模板列表错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Enabled = true;
                dcMessage.ClearValue();
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("消息模板列表新增错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 修改

        /// <summary>
        /// 单击替代修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gc_Message_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_Message_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                var selectedRow = gv_Message.GetFocusedRow() as MessageInfoEntity;
                if (selectedRow == null)
                    return;
                groupBox1.Enabled = true;
                dcMessage.SetValue(selectedRow);
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("消息模板列表修改错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRow = gv_Message.GetFocusedRow() as MessageInfoEntity;
                if (selectedRow == null)
                    return;
                //MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm);

                if (dr == DialogResult.OK)
                {
                    String param = "?id=" + selectedRow.id;
                    String url = AppContext.AppConfig.serverUrl + "cms/messageTemplate/delete" + param;
                    String data = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(data);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        Xr.Common.MessageBoxUtils.Hint("删除成功", MainForm);
                        MessageContentTemplateList(1, pageControl1.PageSize);
                        this.dcMessage.ClearValue();
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("消息模板列表删除错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 保存
        public MessageInfoEntity messageInfoEntity { set; get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butContronl_Click(object sender, EventArgs e)
        {
            try
            {
                if (!dcMessage.Validate())
                {
                    return;
                }
                dcMessage.GetValue(messageInfoEntity);
                String param = "?" + PackReflectionEntity<MessageInfoEntity>.GetEntityToRequestParameters(messageInfoEntity, true);
                String url = AppContext.AppConfig.serverUrl + "cms/messageTemplate/save" + param;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    MessageBoxUtils.Hint("保存成功!", MainForm);
                    dcMessage.ClearValue();
                    MessageContentTemplateList(1, pageControl1.PageSize);
                    groupBox1.Enabled = false;
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("消息模板列表保存错误信息：" + ex.Message);
            }
        }
#endregion 
        #region 获取消息内容模板类型
        List<DictEntity> listentity;
        public void TemplateType()
        {
            try
            {
                //查询状态下拉框数据
                String url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=message_template";
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    listentity = new List<DictEntity>();
                    listentity = objT["result"].ToObject<List<DictEntity>>();
                    lueType.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                    lueType.Properties.DisplayMember = "label";
                    lueType.Properties.ValueMember = "value";
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("获取消息内容模板类型错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 窗体Load事件
        private void MessageManagement_Load(object sender, EventArgs e)
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
        #region 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_Message_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName=="type")
            {
                e.DisplayText = string.Join(",", from a in listentity where a.value == e.Value.ToString() select a.label);
            }
        }
        #endregion 
        #region 
        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            //GroupBox gBox = (GroupBox)sender;
            //var vSize = e.Graphics.MeasureString(gBox.Text, gBox.Font);
            //e.Graphics.DrawLine(Pens.Gray, 1, vSize.Height / 2, 8, vSize.Height / 2);
            //e.Graphics.DrawLine(Pens.Gray, vSize.Width + 8, vSize.Height / 2, gBox.Width - 2, vSize.Height / 2);
            //e.Graphics.DrawLine(Pens.Gray, 1, vSize.Height / 2, 1, gBox.Height - 2);
            //e.Graphics.DrawLine(Pens.Gray, 1, gBox.Height - 2, gBox.Width - 2, gBox.Height - 2);
            //e.Graphics.DrawLine(Pens.Gray, gBox.Width - 2, vSize.Height / 2, gBox.Width - 2, gBox.Height - 2);
        }
        #endregion 
        #region 分页跳转事件
        private void pageControl1_Query(int CurrentPage, int PageSize)
        {
            cmd.ShowOpaqueLayer(225, false);
            MessageContentTemplateList(CurrentPage, PageSize);
        }
        #endregion 
    }
}
