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
using Xr.RtManager.Module.cms;

namespace Xr.RtManager.Pages.cms
{
    public partial class ClinicSettingForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        
        public ClinicSettingForm()
        {
            InitializeComponent();
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(225, false);
            GetKeShiList();
            SearchData(1, pageControl1.PageSize, AppContext.Session.hospitalId, DeptId);
            cmd.HideOpaqueLayer();
            deptId = "";
        }
        #region 查询科室列表
        public String DeptId { get; set; }
        public void GetKeShiList()
        {
            try
            {
                String url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept?";
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DeptEntity> list = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                    List<Xr.Common.Controls.Item> itemList = new List<Xr.Common.Controls.Item>();
                    foreach (DeptEntity dept in list)
                    {
                        Xr.Common.Controls.Item item = new Xr.Common.Controls.Item();
                        item.name = dept.name;
                        item.value = dept.id;
                        item.tag = dept.hospitalId;
                        item.parentId = dept.parentId;
                        itemList.Add(item);
                    }
                    treeMenuControl1.KeyFieldName = "id";
                    treeMenuControl1.ParentFieldName = "parentId";
                    treeMenuControl1.DisplayMember = "name";
                    treeMenuControl1.ValueMember = "id";
                    this.treeMenuControl1.DataSource = list;
                    treeMenuControl1.EditValue = list[0].id;
                    DeptId = list[0].id;
                    treeMenuControl1.selectText = list[0].name;
                    treeWKe.Properties.DataSource = itemList;
                    treeWKe.Properties.TreeList.KeyFieldName = "value";
                    treeWKe.Properties.TreeList.ParentFieldName = "parentId";
                    treeWKe.Properties.DisplayMember = "name";
                    treeWKe.Properties.ValueMember = "value";
                }
                //List<DeptEntity> deptList = AppContext.Session.deptList;
                //List<Xr.Common.Controls.Item> itemList = new List<Xr.Common.Controls.Item>();
                //foreach (DeptEntity dept in deptList)
                //{
                //    Xr.Common.Controls.Item item = new Xr.Common.Controls.Item();
                //    item.name = dept.name;
                //    item.value = dept.id;
                //    item.tag = dept.hospitalId;
                //    item.parentId = dept.parentId;
                //    itemList.Add(item);
                //}
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("诊室设置下查询科室列表信息错误：" + ex.Message);
            }
        }
        #endregion 
        #region 科室列表
        public List<ClinicInfoEntity> Date = new List<ClinicInfoEntity>();
        #endregion 
        #region 查询列表
        List<ClinicInfoEntity> clinicInfo;
        public void SearchData(int pageNo, int pageSize, string hospitalId, string deptId)
        {
            try
            {
                clinicInfo = new List<ClinicInfoEntity>();
                String url = AppContext.AppConfig.serverUrl + "cms/clinic/list?pageNo=" + pageNo + "&pageSize=" + pageSize + "&hospital.id=" + hospitalId + "&dept.id=" +deptId;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    if (objT["result"].ToString() == "" || objT["result"].ToString() == "{}")
                    {
                        this.gc_Clinic.DataSource = null;
                        return;
                    }
                    clinicInfo = objT["result"]["list"].ToObject<List<ClinicInfoEntity>>();
                    for (int i = 0; i < clinicInfo.Count; i++)//Convert.ToInt32(objT["result"]["count"])
                    {
                        //String name = objT["result"]["list"][i]["dept"]["name"].ToString();
                        //string asss = treeMenuControl1.EditValue;
                        clinicInfo[i].deptname = treeMenuControl1.selectText;//string.Join(",",from a  in AppContext.Session.deptList where a.id==asss select a.name);
                    }
                    this.gc_Clinic.DataSource = clinicInfo;
                   
                    pageControl1.setData(int.Parse(objT["result"]["count"].ToString()),
                    int.Parse(objT["result"]["pageSize"].ToString()),
                    int.Parse(objT["result"]["pageNo"].ToString()));
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                cmd.HideOpaqueLayer();
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("查询科室列表错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 编号或者编码查询诊室信息详情
        /// <summary>
        /// 查询诊室信息详情
        /// </summary>
        /// <param name="code"></param>
        public void SelectInforList(string deptId)
        {
            SearchData(1, pageControl1.PageSize, AppContext.Session.hospitalId, deptId);
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
                groupBox3.Enabled = true;
                dcClinc.ClearValue();
               // radioGroup2.SelectedIndex = 0;
                var selectedRow =new ClinicInfoEntity();
                selectedRow.deptId = treeMenuControl1.EditValue;
                selectedRow.isUse="0";
                selectedRow.isOccupy = "0";
                dcClinc.SetValue(selectedRow);

            }
            catch (Exception ex)
            {
                //radioGroup2.SelectedIndex = 0;
                var selectedRow = new ClinicInfoEntity();
                selectedRow.deptId = deptId;
                selectedRow.isUse = "0";
                dcClinc.SetValue(selectedRow);
                Log4net.LogHelper.Error("诊室新增错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRow = this.gv_Clinic.GetFocusedRow() as ClinicInfoEntity;
                if (selectedRow == null)
                    return;
                selectedRow.deptId = treeMenuControl1.EditValue;
                dcClinc.SetValue(selectedRow);
                groupBox3.Enabled = true;
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("诊室修改错误信息：" + ex.Message);
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
                var selectedRow = gv_Clinic.GetFocusedRow() as ClinicInfoEntity;
                if (selectedRow == null)
                    return;
                DialogResult dr = MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm);
                if (dr == DialogResult.OK)
                {
                    String param = "?id=" + selectedRow.id;
                    String url = AppContext.AppConfig.serverUrl + "cms/clinic/delete" + param;
                    String data = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(data);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        Xr.Common.MessageBoxUtils.Hint("删除成功", MainForm);
                        if (deptId != "")
                        {
                            SearchData(1, pageControl1.PageSize, AppContext.Session.hospitalId, deptId);
                        }
                        else
                        {
                            SearchData(1, pageControl1.PageSize, AppContext.Session.hospitalId, AppContext.Session.deptList[0].id);
                        }
                        this.dcClinc.ClearValue();
                        //SearchData(1, pageControl1.PageSize, AppContext.Session.hospitalId, AppContext.Session.deptId);
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("诊室删除错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 编号或者编码查询诊室信息或者查询所有信息
        private void butSelect_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.ShowOpaqueLayer(255, true);
                if (deptId == "")
                {
                    treeMenuControl1.EditValue = AppContext.Session.deptList[0].id;
                    deptId = AppContext.Session.deptList[0].id;
                }
                SelectInforList(deptId);
                this.groupBox3.Enabled = false;
                dcClinc.ClearValue();
            }
            catch
            {
                
            }           
        }
        #endregion
        #region 科室列表点击事件 
        string deptId = "";
        private void treeMenuControl1_MenuItemClick(object sender, EventArgs e, object selectItem)
        {
            DeptEntity dept = selectItem as DeptEntity;
            string hospitalId = dept.hospitalId;
            deptId = dept.id;
            SearchData(1, pageControl1.PageSize, hospitalId, deptId);
        }
        #endregion 
        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butContronl_Click(object sender, EventArgs e)
        {
            try
            {
                if (!dcClinc.Validate())
                {
                    return;
                }
                dcClinc.GetValue(clinicInfoEntity);
                String param = "?" + PackReflectionEntity<ClinicInfoEntity>.GetEntityToRequestParameters(clinicInfoEntity, true);
                String url = AppContext.AppConfig.serverUrl + "cms/clinic/save" + param;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    MessageBoxUtils.Hint("保存成功!", MainForm);
                    if (deptId != "")
                    {
                        SearchData(1, pageControl1.PageSize, AppContext.Session.hospitalId, deptId);
                    }
                    else
                    {
                        SearchData(1, pageControl1.PageSize, AppContext.Session.hospitalId, AppContext.Session.deptList[0].id);
                    }
                    groupBox3.Enabled = false;
                    dcClinc.ClearValue();
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("诊室保存错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 辅助控件
        #region 控件判断
        #region 显示格式
        private void gv_Clinic_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "isUse")
            {
                switch (e.Value.ToString().Trim())
                {
                    case "0":
                        e.DisplayText = "启用";//
                        break;
                    case "1":
                        e.DisplayText = "禁用";//
                        break;
                }
            }//isOccupy
            if (e.Column.FieldName == "isOccupy")
            {
                switch (e.Value.ToString().Trim())
                {
                    case "0":
                        e.DisplayText = "未占用";//
                        break;
                    case "1":
                        e.DisplayText = "占用";//
                        break;
                }
            }
        }
        #endregion 
        #endregion
        #region 窗体Load事件
        public ClinicInfoEntity clinicInfoEntity { get; set; }
        private void ClinicSettingForm_Load(object sender, EventArgs e)
        {
            dcClinc.DataType = typeof(ClinicInfoEntity);
            if (clinicInfoEntity != null)
            {
                dcClinc.SetValue(clinicInfoEntity);
            }
            else
            {
                clinicInfoEntity = new ClinicInfoEntity();
            }
        }
        #endregion 
        #region 加粗GroupBox线条颜色
        /// <summary>
        /// 画边框线条颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
           // GroupBox gBox = (GroupBox)sender;
           // //e.Graphics.Clear(gBox.BackColor);
           ////e.Graphics.DrawString(gBox.Text, gBox.Font, Brushes.Gray, 10, 1);
           // var vSize = e.Graphics.MeasureString(gBox.Text, gBox.Font);
           // e.Graphics.DrawLine(Pens.Gray, 1, vSize.Height / 2, 8, vSize.Height / 2);
           // e.Graphics.DrawLine(Pens.Gray, vSize.Width + 8, vSize.Height / 2, gBox.Width - 2, vSize.Height / 2);
           // e.Graphics.DrawLine(Pens.Gray, 1, vSize.Height / 2, 1, gBox.Height - 2);
           // e.Graphics.DrawLine(Pens.Gray, 1, gBox.Height - 2, gBox.Width - 2, gBox.Height - 2);
           // e.Graphics.DrawLine(Pens.Gray, gBox.Width - 2, vSize.Height / 2, gBox.Width - 2, gBox.Height - 2);
        }
        #endregion 
        #region 分页跳转事件
        private void pageControl1_Query(int CurrentPage, int PageSize)
        {
            if (deptId != "")
            {
                cmd.ShowOpaqueLayer(225, false);
                SearchData(CurrentPage, PageSize, AppContext.Session.hospitalId, deptId);
            }
            else
            {
                cmd.ShowOpaqueLayer(225, false);
                SearchData(CurrentPage, PageSize, AppContext.Session.hospitalId, AppContext.Session.deptList[0].id);
            }
          //  SearchData(CurrentPage,PageSize, AppContext.Session.hospitalId, AppContext.Session.deptId);
        }
        #endregion 

        private void gv_Clinic_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                var selectedRow = this.gv_Clinic.GetFocusedRow() as ClinicInfoEntity;
                if (selectedRow == null)
                    return;
                selectedRow.deptId = treeMenuControl1.EditValue;
                dcClinc.SetValue(selectedRow);
                groupBox3.Enabled = true;
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("诊室修改错误信息：" + ex.Message);
            }
        }
        #endregion
    }
}
