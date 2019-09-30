using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList.Nodes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using Xr.Http;
using Xr.RtScreen.Models;

namespace Xr.RtScreen
{
    public partial class SettingFrm : Form
    {
        public SettingFrm()
        {
            InitializeComponent();
            treeClinc.EditValue = string.Empty;
            treeKeshi.EditValue = string.Empty;
            treeHostile.EditValue = string.Empty;
            GetHostile();
        }
        private String keyId;
        private String keyName;
        private List<string> lstCheckedKeyID = new List<string>();
        private List<string> lstCheckedKeyName = new List<string>();
        /// <summary>
        /// 获取选择的集合
        /// </summary>
        private void GetSelectedRoleIDandName()
        {
            lstCheckedKeyID.Clear();
            lstCheckedKeyName.Clear();
            if (treeKeshi.Properties.TreeList.Nodes.Count > 0)
            {
                foreach (TreeListNode root in treeKeshi.Properties.TreeList.Nodes)
                {
                    GetCheckedKeyID(root);
                }
            }
            keyId = string.Empty;
            keyName = string.Empty;
            foreach (string id in lstCheckedKeyID)
            {
                keyId += id + ",";
            }
            if (keyId.Length > 0)
            {
                keyId = keyId.Substring(0, keyId.Length - 1);
            }
            foreach (string name in lstCheckedKeyName)
            {
                keyName += name + ",";
            }
            if (keyName.Length > 0)
            {
                keyName = keyName.Substring(0, keyName.Length - 1);
            }
        }
        /// <summary>
        /// 获取选择状态的数据主键ID集合
        /// </summary>
        /// <param name="parentNode">父级节点</param>
        private void GetCheckedKeyID(TreeListNode parentNode)
        {
            if (parentNode.CheckState != CheckState.Unchecked)
            {
                var dept = treeKeshi.Properties.TreeList.GetDataRecordByNode(parentNode) as DeptListDic;
                if (dept != null)
                {
                    var KeyFieldName = dept.code;
                    var DisplayMember = dept.name;
                    if (!lstCheckedKeyID.Contains(KeyFieldName))
                    {
                        lstCheckedKeyID.Add(KeyFieldName);
                    }
                    if (!lstCheckedKeyName.Contains(DisplayMember))
                    {
                        lstCheckedKeyName.Add(DisplayMember);
                    }
                }
            }
            if (parentNode.Nodes.Count == 0)
            {
                return;
            }
            foreach (TreeListNode node in parentNode.Nodes)
            {
                if (node.CheckState != CheckState.Unchecked)
                {
                    var dept = treeKeshi.Properties.TreeList.GetDataRecordByNode(node) as DeptListDic;
                    if (dept != null)
                    {
                        var KeyFieldName = dept.code;
                        var DisplayMember = dept.name;
                        lstCheckedKeyID.Add(KeyFieldName);
                        lstCheckedKeyName.Add(DisplayMember);
                    }
                }
                GetCheckedKeyID(node);
            }
        }
        private void txtDoctors_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
        {
            e.DisplayText = keyName;
        }
        /// <summary>
        /// 默认选中事件
        /// </summary>
        /// <param name="rid"></param>
        private void DefaultChecked(string rid)
        {
            if (rid == null)
            {
                return;
            }
            var deptList = treeKeshi.Properties.DataSource as List<DeptListDic>;
            var deptedList = new List<DeptListDic>();
            var arr = rid.Split(',');
            foreach (String id in arr)
            {
                foreach (DeptListDic dept in deptList)
                {
                    if (dept.code.Equals(id))
                    {
                        deptedList.Add(dept);
                        continue;
                    }
                }
            }
            if (treeKeshi.Properties.TreeList.Nodes.Count > 0)
            {
                foreach (TreeListNode nd in treeKeshi.Properties.TreeList.Nodes)
                {
                    for (var i = 0; i < deptedList.Count; i++)
                    {
                        var checkedkeyid = deptedList[i].code;
                        if (treeKeshi.Properties.TreeList.FindNodeByKeyID(checkedkeyid) != null)
                        {
                            treeKeshi.Properties.TreeList.FindNodeByKeyID(checkedkeyid).Checked = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl1_Click(object sender, EventArgs e)
        {
            if (treeHostile.EditValue.ToString() == string.Empty)
            {
                Xr.Common.MessageBoxUtils.Show("请选择医院", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                return;
            }
            if (treeKeshi.EditValue.ToString() == string.Empty)
            {
                Xr.Common.MessageBoxUtils.Show("请选择科室", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                return;
            }
            SaveConfigSeting(treeHostile.EditValue.ToString(), treeClinc.Text.Trim(), keyId, UpStard, "1");
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            Xr.RtScreen.Models.AppContext.Load();
            var ttf = new Form1();
            Hide();
            ttf.ShowDialog();
        }
        /// <summary>
        /// 保存信息到本地配置文件中
        /// </summary>
        private void SaveConfigSeting(string hostalCode, string ClincName, string deptCode, string StartupScreen, string Setting)
        {
            try
            {
                var map = new ExeConfigurationFileMap()
                { ExeConfigFilename = Environment.CurrentDirectory +
                        @"\Xr.RtScreen.exe.config" };
                var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                config.AppSettings.Settings["hospitalCode"].Value = hostalCode;
                config.AppSettings.Settings["deptCode"].Value = deptCode;
                config.AppSettings.Settings["clinicName"].Value = ClincName;
                config.AppSettings.Settings["StartupScreen"].Value = StartupScreen;
                config.AppSettings.Settings["Setting"].Value = Setting;
                config.Save(ConfigurationSaveMode.Full);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                Log4net.LogHelper.Info("保存配置文件内容成功：" + "医院编码：" + hostalCode + "科室编码：" + deptCode + "诊室名称" + ClincName + "并且修改Setting值为1标识为不是第一次启动了");
            }
            catch (Exception ex)
            {
                Xr.Common.MessageBoxUtils.Show("保存配置时出错" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, null);
                Log4net.LogHelper.Error("保存配置文件错误信息：" + ex.Message);
            }
        }
        public List<dynamic> HostalList;
        public List<DeptListDic> DeptList;
        public void GetHostile()
        {
            try
            {
                var urls = AppContext.AppConfig.serverUrl + InterfaceAddress.hostalInfo;
                var datas = HttpClass.httpPost(urls);
                var objTs = JObject.Parse(datas);
                if (string.Compare(objTs["state"].ToString(), "true", true) == 0)
                {
                    var list = objTs["result"].ToObject<List<dynamic>>();
                    HostalList = new List<dynamic>();
                    HostalList = list;
                    treeHostile.Properties.DataSource = list;
                    treeHostile.Properties.DisplayMember = "name";
                    treeHostile.Properties.ValueMember = "code";
                    treeHostile.EditValue = string.Empty;
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
                Log4net.LogHelper.Error("获取医院信息错误：" + ex.Message);
            }
        }
        /// <summary>
        /// 科室列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeHostile_EditValueChanged(object sender, EventArgs e)
        {
            if (treeHostile.EditValue.ToString() != string.Empty)
            {
                GetInforSetting(treeHostile.EditValue.ToString());
            }
        }
        /// <summary>
        /// 获取科室列表
        /// </summary>
        public void GetInforSetting(string hostalcode)
        {
            try
            {
                var urls = AppContext.AppConfig.serverUrl + InterfaceAddress.dept + "?hospital.code=" + hostalcode;
                var datas = HttpClass.httpPost(urls);
                var objTs = JObject.Parse(datas);
                if (string.Compare(objTs["state"].ToString(), "true", true) == 0)
                {
                    var list = objTs["result"].ToObject<List<DeptListDic>>();
                    DeptList = new List<DeptListDic>();
                    DeptList = list;
                    treeKeshi.Properties.DataSource = list;
                    treeKeshi.Properties.TreeList.KeyFieldName = "id";
                    treeKeshi.Properties.TreeList.ParentFieldName = "parentId";
                    treeKeshi.Properties.DisplayMember = "name";
                    treeKeshi.Properties.ValueMember = "code";
                    treeKeshi.EditValue = string.Empty;
                    treeKeshi.Properties.TreeList.AfterCheckNode += (s, a) =>
                    {
                        a.Node.Selected = true;
                        GetSelectedRoleIDandName();
                    };
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("第一次启动查询科室和诊室错误信息：" + ex.Message);
            }
        }
        public class DeptListDic
        {
            public string id { get; set; }
            public string parentId { get; set; }
            public string name { get; set; }
            public string code { get; set; }
        }
        /// <summary>
        /// 诊室列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeKeshi_EditValueChanged(object sender, EventArgs e)
        {
            if (treeKeshi.EditValue.ToString() != string.Empty)
            {
                var hospitalid = string.Join(",", from a in HostalList
                                                  where a.code == treeHostile.EditValue
                                                  select a.id);
                var deptid = string.Join(",", from s in DeptList
                                              where s.code == treeKeshi.EditValue
                                              select s.id);
                var urls = AppContext.AppConfig.serverUrl + InterfaceAddress.ClincInfo + "?hospital.id=" + hospitalid + "&dept.id=" + deptid;
                var datas = HttpClass.httpPost(urls);
                var objTs = JObject.Parse(datas);
                if (string.Compare(objTs["state"].ToString(), "true", true) == 0)
                {
                    var list = objTs["result"].ToObject<List<dynamic>>();
                    treeClinc.Properties.DataSource = list;
                    treeClinc.Properties.DisplayMember = "name";
                    treeClinc.Properties.ValueMember = "id";
                    treeClinc.EditValue = string.Empty;
                }
            }
        }
        private string UpStard = "1";
        private void Postoperative_Properties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Postoperative.EditValue.ToString() == "1" || Postoperative.EditValue.ToString() == "4")
            {
                label5.Visible = false;
                treeClinc.Visible = false;
            }
            else
            {
                label5.Visible = true;
                treeClinc.Visible = true;
            }
            UpStard = Postoperative.EditValue.ToString();
        }
    }
}
