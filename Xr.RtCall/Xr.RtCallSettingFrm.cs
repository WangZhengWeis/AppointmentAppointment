using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.RtCall.Model;
using Xr.Http;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace Xr.RtCall
{
    public partial class SettingFrm : Form
    {
        public SettingFrm()
        {
            InitializeComponent();
            treeKeshi.EditValue = "";
            treeClinc.EditValue = "";
            treeHostile.EditValue = "";
            SettingAppConfig = false;
            //GetInforSetting();
            GetHostile();
        }
        #region 取消按钮
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        #endregion 
        #region 确定按钮
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl1_Click(object sender, EventArgs e)
        {
            //System.Environment.Exit(0);
            if (this.treeHostile.EditValue.ToString() == "")
            {
                Xr.Common.MessageBoxUtils.Show("请选择医院", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                return;
            }
            if (treeKeshi.EditValue.ToString() == "")
            {
                Xr.Common.MessageBoxUtils.Show("请选择科室", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                return;
            }
            if (treeClinc.EditValue.ToString() == "")
            {
                Xr.Common.MessageBoxUtils.Show("请选择诊室", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                return;
            }
            string WhetherToDisplay = "";
            string WhetherToAssign = "";
            string IsStart = "";
            if (radioButton1.Checked)
            {
                WhetherToAssign = "true";
            }
            else if (radioButton2.Checked)
            {
                WhetherToAssign = "false";
            }
            if (radioButton3.Checked)
            {
                WhetherToDisplay = "true";
            }
            else if (radioButton4.Checked)
            {
                WhetherToDisplay = "false";
            }
            if (radioButton5.Checked)
            {
                IsStart = "true";
            }
            else if (radioButton6.Checked)
            {
                IsStart = "false";
            }
            SaveConfigSeting(treeHostile.EditValue.ToString(), treeClinc.Text.Trim(), treeKeshi.EditValue.ToString(), "1", WhetherToDisplay, WhetherToAssign, IsStart);
            UpdateClincQuery(treeClinc.EditValue.ToString(), "1");
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件
            if (this.txtDoctorCode.Text.Trim() == "")
            {
                EncryptionClass.NewAddText("");
            }
            else
            {
                EncryptionClass.NewAddText(this.txtDoctorCode.Text.Trim());
            }
            Xr.RtCall.Model.AppContext.Load();
            Form1 ttf = new Form1();
            this.Hide();
            ttf.Show();
            //Application.ExitThread();
            //Application.Exit();
            //Application.Restart();
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        #endregion 
        public static bool SettingAppConfig { get; set; }
        #region 保存信息到本地配置文件中
        /// <summary>
        /// 保存信息到本地配置文件中
        /// </summary>
        /// <param name="hostalcode">医院编码</param>
        /// <param name="ClincName">诊室名称</param>
        /// <param name="deptCode">科室编码</param>
        /// <param name="Setting">是否第一次启动</param>
        /// <param name="WhetherToDisplay">是否显示完成就诊按钮</param>
        /// <param name="WhetherToAssign">是否给医生站赋值</param>
        /// <param name="OrdinaryOrEmergency">是普通门诊还是急诊</param>
        private void SaveConfigSeting(string hostalcode, string ClincName, string deptCode, string Setting, string WhetherToDisplay, string WhetherToAssign, string IsStart)
        {
            try
            {
                SettingAppConfig = true;
                ExeConfigurationFileMap map = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = Environment.CurrentDirectory +
                        @"\Xr.RtCall.exe.config"
                };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                config.AppSettings.Settings["hospitalCode"].Value = hostalcode;
                config.AppSettings.Settings["deptCode"].Value = deptCode;
                config.AppSettings.Settings["ClincName"].Value = ClincName;
                config.AppSettings.Settings["Setting"].Value = Setting;
                config.AppSettings.Settings["WhetherToDisplay"].Value = WhetherToDisplay;//完成就诊
                config.AppSettings.Settings["WhetherToAssign"].Value = WhetherToAssign;//赋值
                config.AppSettings.Settings["IsStart"].Value = IsStart;
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件
                Log4net.LogHelper.Info("保存配置文件内容成功：" + "医院编码：" + hostalcode + "" + "科室编码：" + deptCode + "" + "诊室名称" + ClincName + "" + "并且修改Setting值为1标识为不是第一次启动了");
            }
            catch (Exception ex)
            {
                Xr.Common.MessageBoxUtils.Show("保存配置时出错"+ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, null);
                Log4net.LogHelper.Error("保存配置文件错误信息：" + ex.Message);
            }
        }
        #endregion
        public List<dynamic> HostalList;
        public List<dynamic> DeptList;
        #region 获取医院信息
        public void GetHostile()
        {
            try
            {
                String urls = AppContext.AppConfig.serverUrl + InterfaceAddress.HostalInfo;
                String datas = HttpClass.httpPost(urls);
                JObject objTs = JObject.Parse(datas);
                if (string.Compare(objTs["state"].ToString(), "true", true) == 0)
                {
                    List<dynamic> list = objTs["result"].ToObject<List<dynamic>>();
                    HostalList = new List<dynamic>();
                    HostalList = list;
                    string code = string.Join(",",from a in list where a.name=="中山博爱医院" select a.code);
                    this.treeHostile.Properties.DataSource = list;
                    //treeHostile.Properties.TreeList.KeyFieldName = "id";
                    //treeKeshi.Properties.TreeList.ParentFieldName = "parentId";
                    //treeHostile.Properties.TreeList.KeyFieldName = "id";
                    treeHostile.Properties.DisplayMember = "name";
                    treeHostile.Properties.ValueMember = "code";
                    treeHostile.EditValue = code;
                }
                else
                {
                    Xr.Common.MessageBoxUtils.Show(objTs["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                    Log4net.LogHelper.Error("修改配置文件时错误信息：" + objTs["message"].ToString());
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("获取医院信息错误："+ex.Message);
            }
        }
        #endregion 
        #region 获取科室列表
        /// <summary>
        /// 获取科室列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeHostile_EditValueChanged(object sender, EventArgs e)
        {
            if (treeHostile.EditValue.ToString() != "")
            {
                GetInforSetting(treeHostile.EditValue.ToString());
            }
        }
        /// <summary>
        /// 获取科室列表
        /// </summary>
        public void GetInforSetting(string hospitalcode)
        {
            try
            {
                  // 查询科室数据
                String urls = AppContext.AppConfig.serverUrl + InterfaceAddress.dept + "?hospital.code=" + hospitalcode;
                String datas = HttpClass.httpPost(urls);
                JObject objTs = JObject.Parse(datas);
                if (string.Compare(objTs["state"].ToString(), "true", true) == 0)
                {
                    List<dynamic> list = objTs["result"].ToObject<List<dynamic>>();
                    DeptList = new List<dynamic>();
                    DeptList = list;
                    treeKeshi.Properties.DataSource = list;
                    treeKeshi.Properties.TreeList.KeyFieldName = "id";
                    treeKeshi.Properties.TreeList.ParentFieldName = "parentId";
                    treeKeshi.Properties.DisplayMember = "name";
                    treeKeshi.Properties.ValueMember = "code";
                    treeKeshi.EditValue = "";
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("第一次启动查询科室和诊室错误信息："+ex.Message);
            }
        }
        #endregion 
        #region 根据科室列表获取诊室列表
        /// <summary>
        /// 根据科室列表获取诊室列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeKeshi_EditValueChanged(object sender, EventArgs e)
        {
            if (treeKeshi.EditValue.ToString() != "")
            {
                string hospitalid =string.Join(",",from a in HostalList where a.code==treeHostile.EditValue select a.id);
                string deptid = string.Join(",", from s in DeptList where s.code == treeKeshi.EditValue select s.id);
                String urls = AppContext.AppConfig.serverUrl + InterfaceAddress.ClincInfo + "?hospital.id=" + hospitalid + "&dept.id=" + deptid;
                String datas = HttpClass.httpPost(urls);
                JObject objTs = JObject.Parse(datas);
                if (string.Compare(objTs["state"].ToString(), "true", true) == 0)
                {
                    List<dynamic> list = objTs["result"].ToObject<List<dynamic>>();
                    this.treeClinc.Properties.DataSource = list;
                    treeClinc.Properties.DisplayMember = "name";
                    treeClinc.Properties.ValueMember = "id";
                    treeClinc.EditValue = "";
                }
            }
        }
        #endregion 
        #region 关闭

        private void button3_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        #endregion 
        #region 查询诊室是否占用和修改占用情况
        /// <summary>
        /// 根据诊室ID查询诊室是否被占用
        /// </summary>
        /// <param name="id"></param>
        public void SelectClincIsQuery(string id)
        {
            try
            {
                String url = AppContext.AppConfig.serverUrl + "itf/triage/getClinic?id=" + id;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    String isQccery = objT["result"]["isOccupy"].ToString();
                    if (isQccery != "0")
                    {
                        //    Xr.Common.MessageBoxUtils.Show("当前诊室可用,请确认信息无误后保存", MessageBoxButtons.OK,
                        //    MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, this);
                        //}
                        //else
                        //{
                        if (Xr.Common.MessageBoxUtils.Show("当前诊室不可用,是否继续启动", MessageBoxButtons.OKCancel,
                       MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, this) == DialogResult.OK)
                        {
                            UpdateClincQuery(id, "0");
                        }
                    }
                }
                else
                {
                    Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("查询诊室占用错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 根据诊室ID修改诊室占用情况
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isOccupy">isOccupy：0未用、1占用</param>
        public void UpdateClincQuery(string id, string isOccupy)
        {
            try
            {
                String url = AppContext.AppConfig.serverUrl + "itf/triage/updateClinic?id=" + id + "&isOccupy=" + isOccupy;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    //Xr.Common.MessageBoxUtils.Show("修改诊室状态成功,请确认信息无误后保存", MessageBoxButtons.OK,
                    //  MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, this);
                }
                else
                {
                    Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("修改诊室状态错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 验证是否可用
        private void treeClinc_EditValueChanged(object sender, EventArgs e)
        {
            if (treeClinc.EditValue.ToString() != "")
            {
                //SelectClincIsQuery(treeClinc.EditValue.ToString());
            }
        }
        #endregion 
        #region 隐藏与显示
        private void radioButton6_Click(object sender, EventArgs e)
        {
            label6.Visible = true;
            txtDoctorCode.Visible = true;
        }

        private void radioButton5_Click(object sender, EventArgs e)
        {
            label6.Visible = false;
            txtDoctorCode.Visible = false;
        }
        #endregion
    }
}
