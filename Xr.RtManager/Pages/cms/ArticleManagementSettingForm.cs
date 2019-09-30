using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using Xr.Http;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using Xr.RtManager.Module.cms;
using Xr.Common.Controls;
using Xr.Common;

namespace Xr.RtManager.Pages.cms
{
    public partial class ArticleManagementSettingForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        ArticleInfoEntity CickInfo;
        public ArticleManagementSettingForm()
        {
            InitializeComponent();
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(225, false);
            First();
            ArticleCategoryCollection();
            cmd.HideOpaqueLayer();
            CickInfo = new ArticleInfoEntity();
            CickInfo.id = "";
        }
        #region 类型设置
        List<FirstOne> firs;
        List<FirstOne> Two;
        public void First()
        {
            try
            {
                //类型设置
                firs = new List<FirstOne>();
                firs.Add(new FirstOne { id = "", name = "全部" });
                firs.Add(new FirstOne { id = "1", name = "医院" });
                firs.Add(new FirstOne { id = "2", name = "科室" });
                firs.Add(new FirstOne { id = "3", name = "医生" });
                this.lueType.Properties.DataSource = firs;
                lueType.Properties.DisplayMember = "name";
                lueType.Properties.ValueMember = "id";
                listoffice = new List<TreeList>();
                listoffice.Add(new TreeList { id = "", parentId = "", name = "全部" });
                treeKeshi.Properties.DataSource = listoffice;
                treeKeshi.Properties.TreeList.KeyFieldName = "id";
                treeKeshi.Properties.TreeList.ParentFieldName = "parentId";
                treeKeshi.Properties.DisplayMember = "name";
                treeKeshi.Properties.ValueMember = "id";
                treeKeshi.EditValue = "";
                Two = new List<FirstOne>();
                Two.Add(new FirstOne { id = "1", name = "医院" });
                Two.Add(new FirstOne { id = "2", name = "科室" });
                Two.Add(new FirstOne { id = "3", name = "医生" });
                this.lookUpEdit1.Properties.DataSource = Two;
                lookUpEdit1.Properties.DisplayMember = "name";
                lookUpEdit1.Properties.ValueMember = "id";
                //if (AppContext.AppConfig.deptCode != "")
                //{
                   // GetDoctorAndDepartment(AppContext.Session.deptIds);
                    //lueType.EditValue = "2";
                    //treeKeshi.EditValue = string.Join(",", from a in listoffice where a.code == AppContext.Session.deptIds select a.id);
               // }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("文章类型设置错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 文章栏目
        List<ArticleInfoEntity> list;
        dynamic Woqu;
        #region 文章类别集合
        /// <summary>
        /// 文章类别集合
        /// </summary>
        public void ArticleCategoryCollection()
        {
            try
            {
                list = new List<ArticleInfoEntity>();
                String url = AppContext.AppConfig.serverUrl + "cms/articleCategory/findAll?hospitalId=" + AppContext.Session.hospitalId;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    if (objT["result"].ToString() == "" || objT["result"].ToString() == "{}")
                    {
                        this.gc_Article.DataSource = null;
                        return;
                    }
                    Woqu = objT;
                    list = objT["result"].ToObject<List<ArticleInfoEntity>>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        switch (list[i].isUse)
                        {
                            case "0":
                                list[i].isUse = "启用";
                                break;
                            case "1":
                                list[i].isUse = "禁用";
                                break;
                        }
                    }
                    this.gc_Article.DataSource = list;
                    gc_Article.RefreshDataSource();
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception)
            {
                ArticleInfoEntity two = Newtonsoft.Json.JsonConvert.DeserializeObject<ArticleInfoEntity>(Woqu["result"].ToString());
                list.Add(two);
                for (int i = 0; i < list.Count; i++)
                {
                    switch (list[i].isUse)
                    {
                        case "0":
                            list[i].isUse = "启用";
                            break;
                        case "1":
                            list[i].isUse = "禁用";
                            break;
                    }
                }
                this.gc_Article.DataSource = list;
                gc_Article.RefreshDataSource();
            }
            finally
            {
                //list
                lookUpEdit2.Properties.DataSource = list;
                lookUpEdit2.Properties.DisplayMember = "name";
                lookUpEdit2.Properties.ValueMember = "id";
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
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].id == "" || list[i].id == null)
                    {
                        return;
                    }
                }
                list.Insert(0, new ArticleInfoEntity { createDate = "", hospitalId = "", id = "", isUse = "0", name = "", updateDate = "" });// createById = "",createByName = "",  updateById = "", updateByName = "", updateDate = "" });
                gv_Article.FocusedColumn = this.gridColumn3;
                this.gv_Article.ShowEditor(gridColumn3);
                this.gc_Article.DataSource = list;
                gc_Article.RefreshDataSource();
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("新增文章错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRow = gv_Article.GetFocusedRow() as ArticleInfoEntity;
                if (selectedRow.name == "" || selectedRow.isUse == "")
                {
                    MessageBoxUtils.Show("请填写栏目名称和选择状态", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
                String param = "";
                if (selectedRow.isUse == "0"||selectedRow.isUse == "启用")
                {
                    param = "?" + "hospitalId=" + AppContext.Session.hospitalId + "&id=" + selectedRow.id + "&name=" + selectedRow.name + "&isUse=" + "0";
                }
                else
                {
                    param = "?" + "hospitalId=" + AppContext.Session.hospitalId + "&id=" + selectedRow.id + "&name=" + selectedRow.name + "&isUse=" + "1";
                }
                String url = AppContext.AppConfig.serverUrl + "cms/articleCategory/save" + param;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    MessageBoxUtils.Hint("保存成功!", MainForm);
                    ArticleCategoryCollection();
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
                Log4net.LogHelper.Error("保存文章错误信息：" + ex.Message);
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
                //MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBoxUtils.Show("确定要删除吗?",MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm);
                if (dr == DialogResult.OK)
                {
                    String param = "?" + "id=" + CickInfo.id;
                    String url = AppContext.AppConfig.serverUrl + "cms/articleCategory/delete" + param;
                    String data = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(data);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        MessageBoxUtils.Hint("删除成功!", MainForm);
                        ArticleCategoryCollection();
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
                Log4net.LogHelper.Error("删除文章错误信息：" + ex.Message);
            }
        }
        #endregion
        #endregion
        #region 文章
        #region 获取科室信息
        #region
        List<TreeList> listoffice;
        List<HospitalInfoEntity> doctorInfoEntity;
        #endregion
        string TypeDoctor = "";
        private void lueType_EditValueChanged(object sender, EventArgs e)
        {
            switch (lueType.Text.Trim())
            {
                case "科室":
                    TypeDoctor = "";
                    GetDoctorAndDepartment(AppContext.Session.deptIds);
                    luDoctords.EditValue = "";
                    break;
                case "医生":
                    Doc = 1;
                    TypeDoctor = "全部";
                    SelectDoctor(AppContext.Session.deptIds);
                    treeKeshi.EditValue = "";
                    break;
                case "医院":
                    treeKeshi.Properties.DataSource = null;
                    luDoctords.Properties.DataSource = null;
                    break;
                case "全部":
                    TypeDoctor = "";
                    listoffice = new List<TreeList>();
                    listoffice.Add(new TreeList { id = "", parentId = "", name = "全部" });
                    treeKeshi.Properties.DataSource = listoffice;
                    treeKeshi.Properties.TreeList.KeyFieldName = "id";
                    treeKeshi.Properties.TreeList.ParentFieldName = "parentId";
                    treeKeshi.Properties.DisplayMember = "name";
                    treeKeshi.Properties.ValueMember = "id";
                    doctorInfoEntity = new List<HospitalInfoEntity>();
                    doctorInfoEntity.Add(new HospitalInfoEntity { id = "", name = "全部" });
                    luDoctords.Properties.DataSource = doctorInfoEntity;
                    luDoctords.Properties.DisplayMember = "name";
                    luDoctords.Properties.ValueMember = "id";
                    break;
            }
        }
        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            switch (lookUpEdit1.Text.Trim())
            {
                case "医院":
                    treeWKe.Properties.DataSource = null;
                    luDoctor.Properties.DataSource = null;
                    break;
                case "科室":
                    GetDoctorAndDepartment(AppContext.Session.deptIds);
                    luDoctor.Properties.DataSource = null;
                    luDoctor.Properties.DisplayMember = "name";
                    luDoctor.Properties.ValueMember = "id";
                    break;
                case "医生":
                    Doc = 2;
                    TypeDoctor = "";
                    GetDoctorAndDepartment(AppContext.Session.deptIds);
                    SelectDoctor(AppContext.Session.deptIds);
                    break;
            }
        }
        public void GetDoctorAndDepartment(string code)
        {
            try
            {
                listoffice = new List<TreeList>();
                String url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept";
                   String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    //查询科室下拉框数据
                    List<DeptEntity> deptList = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                    List<Xr.Common.Controls.Item> itemList = new List<Xr.Common.Controls.Item>();
                    foreach (DeptEntity dept in deptList)
                    {
                        Xr.Common.Controls.Item item = new Xr.Common.Controls.Item();
                        item.name = dept.name;
                        item.value = dept.id;
                        item.tag = dept.hospitalId;
                        item.parentId = dept.parentId;
                        itemList.Add(item);
                    }
                    //itemList.Insert(0, Xr.Common.Controls.Item { value = "", parentId = "", name = "全部" });
                    treeKeshi.Properties.DataSource = itemList;
                    treeKeshi.Properties.TreeList.KeyFieldName = "value";
                    treeKeshi.Properties.TreeList.ParentFieldName = "parentId";
                    treeKeshi.Properties.DisplayMember = "name";
                    treeKeshi.Properties.ValueMember = "value";
                    treeWKe.Properties.DataSource = itemList;
                    treeWKe.Properties.TreeList.KeyFieldName = "value";
                    treeWKe.Properties.TreeList.ParentFieldName = "parentId";
                    treeWKe.Properties.DisplayMember = "name";
                    treeWKe.Properties.ValueMember = "value";
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("文章中获取科室下拉框错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl1_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.ShowOpaqueLayer(225, true);
                groupBox3.Enabled = false;
                SelectInfoPage(1, pageControl1.PageSize, CickInfo.id);
                dcArticle.ClearValue();
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("文章查询错误信息：" + ex.Message);
            }
        }
        #region 查询文章
        /// <summary>
        /// 查询文章
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        public void SelectInfoPage(int pageNo, int pageSize, string categoryId)
        {
            try
            {
                #region
                switch (lueType.Text.Trim())
                {
                    case "科室":
                        if (treeKeshi.Text.Trim() == "" || treeKeshi.Text.Trim() == "全部")
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show("当类型为科室时,科室不能为空", MessageBoxButtons.OK,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                            this.gc_Atrlices.DataSource = null;
                            return;
                        }
                        break;
                    case "医生":
                        if (luDoctords.Text.Trim() == "" || luDoctords.Text.Trim() == "全部")
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show("当类型为医生时,医生不能为空", MessageBoxButtons.OK,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                            this.gc_Atrlices.DataSource = null;
                            return;
                        }
                        break;
                }
                string deptId = "";
                if (treeKeshi.Text.Trim() == "")
                {
                    deptId = "";
                }
                else
                {
                    deptId = string.Join(",", from p in listoffice where p.name == treeKeshi.Text.Trim() select p.id);
                }
                string doctorId = "";
                if (luDoctords.Text.Trim() == "")
                {
                    doctorId = "";
                }
                else
                {
                    doctorId = string.Join(",", from d in doctorInfoEntity where d.name == luDoctords.Text.Trim() select d.id);
                }
                string type = "";
                if (lueType.Text.Trim() == "")
                {
                    type = "";
                }
                else
                {
                    type = string.Join(",", from s in firs where s.name == lueType.Text.Trim() select s.id);
                }
                #endregion
                String url = AppContext.AppConfig.serverUrl + "cms/article?pageNo=" + pageNo + "&pageSize=" + pageSize + "&hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + deptId + "&doctorId=" + doctorId + "&type=" + type + "&categoryId=" + categoryId + "&title=" + txtTitle.Text.Trim();
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    if (objT["result"].ToString() == "" || objT["result"].ToString() == "{}")
                    {
                        this.gc_Atrlices.DataSource = null;
                        return;
                    }
                    this.gc_Atrlices.DataSource = objT["result"]["list"].ToObject<List<Article>>();
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
                Log4net.LogHelper.Error("查询列表错误信息：" + ex.Message);
            }
        }
        #endregion
        #endregion
        #endregion
        #region 编辑文章信息
        public Article clinicInfoEntity { get; set; }
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
                if (!dcArticle.Validate())
                {
                    return;
                }
                dcArticle.GetValue(clinicInfoEntity);
                #region
                switch (clinicInfoEntity.type)
                {
                    case "2"://科室
                        if (clinicInfoEntity.deptId == "" || clinicInfoEntity.deptId == null)
                        {
                            MessageBoxUtils.Show("请选择科室", MessageBoxButtons.OK, MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1, MainForm);
                            return;
                        }
                        break;
                    case "3"://医生
                        if (clinicInfoEntity.doctorId == "" || clinicInfoEntity.doctorId == null || clinicInfoEntity.deptId == "" || clinicInfoEntity.deptId == null)
                        {
                            MessageBoxUtils.Show("请选择科室和医生", MessageBoxButtons.OK, MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1, MainForm);
                            return;
                        }
                        break;
                }
                #endregion
                clinicInfoEntity.content = System.Web.HttpUtility.UrlEncode(clinicInfoEntity.content, Encoding.UTF8);
                String param = PackReflectionEntity<Article>.GetEntityToRequestParameters(clinicInfoEntity, true);
                String url = AppContext.AppConfig.serverUrl + "cms/article/save?";
                String data = HttpClass.httpPost(url, param);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    MessageBoxUtils.Hint("保存成功!", MainForm);
                    groupBox3.Enabled = false;
                    if (CickInfo.id != "")
                    {
                        SelectInfoPage(1, pageControl1.PageSize, CickInfo.id);
                        gv_Article.FocusedColumn = this.gridColumn5;
                    }
                    else
                    {
                        SelectInfoPage(1, pageControl1.PageSize, "");
                    }
                    dcArticle.ClearValue();
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
                Log4net.LogHelper.Error("保存文章错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 设置显示信息
        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_Article_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "isUse")
            {
                switch (e.Value.ToString())
                {
                    case "0":
                        e.DisplayText = "启用";
                        break;
                    case "1":
                        e.DisplayText = "禁用";
                        break;
                }
            }
        }
        private void gv_Atrlices_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "isUse")
            {
                switch (e.Value.ToString())
                {
                    case "0":
                        e.DisplayText = "启用";
                        break;
                    case "1":
                        e.DisplayText = "禁用";
                        break;
                }
            }
            if (e.Column.FieldName == "type")//类型：1医院文章、2科室文章、3医生文章
            {
                switch (e.Value.ToString())
                {
                    case "1":
                        e.DisplayText = "医院文章";
                        break;
                    case "2":
                        e.DisplayText = "科室文章";
                        break;
                    case "3":
                        e.DisplayText = "医生文章";
                        break;
                }
            }
        }
        #endregion
        #region 把值赋值到下面
        /// <summary>
        /// 把值赋值到下面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_Atrlices_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            clinicInfoEntity = new Article();
            var selectedRow = gv_Atrlices.GetFocusedRow() as Article;
            clinicInfoEntity = selectedRow;
            if (selectedRow == null)
                return;
            dcArticle.SetValue(selectedRow);
            groupBox3.Enabled = true;
        }
        #endregion
        #endregion
        #region 窗体Load
        /// <summary>
        /// 窗体Load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArticleManagementSettingForm_Load(object sender, EventArgs e)
        {
            dcArticle.DataType = typeof(Article);
            if (clinicInfoEntity != null)
            {
                dcArticle.SetValue(clinicInfoEntity);
            }
            else
            {
                clinicInfoEntity = new Article();
            }
        }
        #endregion
        #region 记录点击列表事件
        private void gv_Article_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            cmd.ShowOpaqueLayer(225, true);
            CickInfo = gv_Article.GetFocusedRow() as ArticleInfoEntity;
            SelectInfoPage(1, pageControl1.PageSize, CickInfo.id);
        }
        #endregion
        #region 点击科室时获取去当前科室下的医生
        int Doc = 0;
        private void treeKeshi_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                Doc = 1;
                TypeDoctor = "";
                SelectDoctor(treeKeshi.EditValue.ToString());
            }
            catch
            {
            }
        }
        private void treeWKe_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lookUpEdit1.EditValue != "2" && treeWKe.EditValue!=null)
                {
                    Doc = 2;
                    TypeDoctor = "";
                    SelectDoctor(treeWKe.EditValue.ToString());
                }
                else
                {
                    GetDoctorAndDepartment(AppContext.Session.deptIds);
                    luDoctor.Properties.DataSource = null;
                    luDoctor.Properties.DisplayMember = "name";
                    luDoctor.Properties.ValueMember = "id";
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 获取当前科室医生
        /// </summary>
        /// <param name="dept"></param>
        public void SelectDoctor(string dept)
        {
            try
            {
                doctorInfoEntity = new List<HospitalInfoEntity>();
                // 查询医生下拉框数据
                String url = "";
                if (TypeDoctor == "全部")
                {
                    url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?hospital.id=" + AppContext.Session.hospitalId + "&deptIds=" + dept; 
                }
                else
                {
                    url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + dept;
                }
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    doctorInfoEntity = objT["result"].ToObject<List<HospitalInfoEntity>>();
                    doctorInfoEntity.Insert(0, new HospitalInfoEntity { id = "", name = "全部" });
                    if (Doc == 1)
                    {
                        luDoctords.Properties.DataSource = doctorInfoEntity;
                        luDoctords.Properties.DisplayMember = "name";
                        luDoctords.Properties.ValueMember = "id";

                    }
                    else
                    {
                        luDoctor.Properties.DataSource = doctorInfoEntity;
                        luDoctor.Properties.DisplayMember = "name";
                        luDoctor.Properties.ValueMember = "id";
                    }
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("文章中获取当前科室医生错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 画边框颜色
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
        #region 编辑文章正文
        //编辑文章正文
        private void buttonControl2_Click_1(object sender, EventArgs e)
        {
            if (DianJi)
            {
                clinicInfoEntity.content = "";
                DianJi = false;
            }
            var edit = new RichEditorForm();
            edit.text = clinicInfoEntity.content;
            edit.ImagUploadUrl = AppContext.AppConfig.serverUrl;
            if (edit.ShowDialog() == DialogResult.OK)
            {
                clinicInfoEntity.content = edit.text;
            }
        }
        #endregion
        #region 新增和删除按钮
        bool DianJi = false;//判断是否新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl3_Click(object sender, EventArgs e)
        {
            try
            {
                DianJi = true;
                groupBox3.Enabled = true;
                dcArticle.ClearValue();
                int selectedHandle;
                selectedHandle = this.gv_Article.GetSelectedRows()[0];
                string id=this.gv_Article.GetRowCellValue(selectedHandle, "id").ToString();
                clinicInfoEntity = new Article();
                clinicInfoEntity.categoryId = id;
                dcArticle.SetValue(clinicInfoEntity);
                radioGroup2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                radioGroup2.SelectedIndex = 0;
                Log4net.LogHelper.Error("新增文章错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl4_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm);
                if (dr == DialogResult.OK)
                {
                    String param = "?" + "id=" + clinicInfoEntity.id;
                    String url = AppContext.AppConfig.serverUrl + "cms/article/delete" + param;
                    String data = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(data);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        MessageBoxUtils.Hint("删除成功!", MainForm);
                        groupBox3.Enabled = false;
                        if (CickInfo.id != "")
                        {
                            SelectInfoPage(1, pageControl1.PageSize, CickInfo.id);
                            gv_Article.FocusedColumn = this.gridColumn5;
                        }
                        else
                        {
                            SelectInfoPage(1, pageControl1.PageSize, "");
                        }
                        dcArticle.ClearValue();
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                      //  MessageBox.Show(objT["message"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("删除文章错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 分页跳转事件
        /// <summary>
        /// 分页跳转事件
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        private void pageControl1_Query(int CurrentPage, int PageSize)
        {
            if (CickInfo.id != "")
            {
                cmd.ShowOpaqueLayer(225, false);
                SelectInfoPage(CurrentPage, PageSize, CickInfo.id);
                gv_Article.FocusedColumn = this.gridColumn5;
            }
            else
            {
                cmd.ShowOpaqueLayer(225, false);
                SelectInfoPage(CurrentPage, PageSize, "");
            }
            //SelectInfoPage(CurrentPage,PageSize, "");
        }
        #endregion 
    }
}
