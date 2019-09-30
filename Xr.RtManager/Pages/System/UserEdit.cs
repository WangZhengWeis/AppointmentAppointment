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
using System.Threading;
using System.Web;
using Xr.Common.Controls;
using DevExpress.XtraTreeList.Nodes;

namespace Xr.RtManager
{
    public partial class UserEdit : Form
    {
        public UserEdit()
        {
            InitializeComponent();
        }

        Xr.Common.Controls.OpaqueCommand cmd;
        public UserEntity user { get; set; }
        private String oldLoginName;
        String filePath = "";
        String serviceFilePath = "";
        String password;

        private void UserEdit_Load(object sender, EventArgs e)
        {
            labelControl1.Font = new Font("微软雅黑", 18, FontStyle.Regular, GraphicsUnit.Pixel);
            labelControl1.ForeColor = Color.FromArgb(255, 0, 0);
            labelControl2.Font = new Font("微软雅黑", 18, FontStyle.Regular, GraphicsUnit.Pixel);
            labelControl2.ForeColor = Color.FromArgb(255, 0, 0);
            labelControl3.Font = new Font("微软雅黑", 18, FontStyle.Regular, GraphicsUnit.Pixel);
            labelControl3.ForeColor = Color.FromArgb(255, 0, 0);
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            cmd.ShowOpaqueLayer(0f);
            richEditor1.ImagUploadUrl = AppContext.AppConfig.serverUrl;
            dcUser.DataType = typeof(UserEntity);
            richEditor1.ImagUploadUrl = AppContext.AppConfig.serverUrl;

            //获取科室数据
            String url = AppContext.AppConfig.serverUrl + "cms/dept/findAll?hospital.code=" + AppContext.AppConfig.hospitalCode;
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data2.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DeptEntity> deptList = objT["result"].ToObject<List<DeptEntity>>();
                    //DeptEntity dept = new DeptEntity();
                    //dept.code = "`";
                    //dept.name = "全院";
                    //dept.id = "`";
                    //deptList.Insert(0, dept);
                    treeList1.DataSource = deptList;
                    treeList1.KeyFieldName = "id";//设置ID  
                    treeList1.ParentFieldName = "parentId";//设置PreID   
                    treeList1.OptionsView.ShowCheckBoxes = true;  //显示多选框
                    treeList1.OptionsBehavior.AllowRecursiveNodeChecking = true; //选中父节点，子节点也会全部选中
                    treeList1.OptionsBehavior.Editable = false; //数据不可编辑
                    treeList1.ExpandAll();//展开所有节点
                    //treeDept.Properties.DataSource = deptList;
                    //treeDept.Properties.TreeList.KeyFieldName = "id";
                    //treeDept.Properties.TreeList.ParentFieldName = "parentId";
                    //treeDept.Properties.DisplayMember = "name";
                    //treeDept.Properties.ValueMember = "id";
                    ////treeDept.EditValue = "`";
                    ////这个应该是个选择多选框触发的事件
                    //treeDept.Properties.TreeList.AfterCheckNode += (s, a) =>
                    //{
                    //    a.Node.Selected = true;
                    //    GetSelectedRoleIDandName();
                    //};
                    List<RoleEntity> roleList = new List<RoleEntity>();
                    List<String> roleIdList = new List<String>();
                    String type = "0";//11：超级管理员  1：系统管理员 8：科室管理员 0：其他

                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        String data = HttpClass.httpPost(AppContext.AppConfig.serverUrl + "sys/sysRole/findAll");
                        return data;

                    }, null, (data3) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        objT = JObject.Parse(data3.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            //TODO:在这里进行判断，user是空的，会报错，后面再处理
                            roleList = objT["result"].ToObject<List<RoleEntity>>();
                            roleIdList = AppContext.Session.roleIdList;
                            List<RoleEntity> showRoleList = new List<RoleEntity>();//显示的角色
                             
                            //超级管理员只有一个 并且id为1，可以分配除超级管理员外的角色；
                            //系统管理员有多个，可以分配除超级\系统管理外的角色；
                            //科室管理员有多个，可以分配医生、护士两个角色；
                            foreach (String roleId in roleIdList)
                            {
                                if (roleId.Equals("11"))
                                {
                                    type = "11";
                                    break;
                                }
                                if (roleId.Equals("1"))
                                {
                                    type = "1";
                                    break;
                                }
                                if (roleId.Equals("8"))
                                {
                                    type = "8";
                                    break;
                                }
                            }
                            foreach (RoleEntity role in roleList)
                            {
                                if (type.Equals("11"))
                                {
                                    if (user == null)
                                    {
                                        if (!role.name.Equals("超级管理员") && !role.id.Equals("11"))
                                        {
                                            showRoleList.Add(role);
                                        }
                                    }
                                }
                                else if (type.Equals("1"))
                                {
                                    if (user == null)
                                    {
                                        if (!role.id.Equals("11") && !role.id.Equals("1"))
                                        {
                                            showRoleList.Add(role);
                                        }
                                    }
                                }
                                else if (type.Equals("8"))
                                {
                                    if (user == null)
                                    {
                                        if (role.id.Equals("9") || role.id.Equals("10"))
                                        {
                                            showRoleList.Add(role);
                                        }
                                    }
                                }
                                else
                                {
                                    //if (!role.id.Equals("11") && !role.id.Equals("1"))
                                    //    checkedListBoxControl1.Items.Add(role.id, role.name);
                                }
                            }
                            treeListRole.DataSource = showRoleList;
                            treeListRole.KeyFieldName = "id";//设置ID  
                            treeListRole.ParentFieldName = "parentId";//设置PreID   
                            treeListRole.OptionsView.ShowCheckBoxes = true;  //显示多选框
                            treeListRole.OptionsBehavior.AllowRecursiveNodeChecking = true; //选中父节点，子节点也会全部选中
                            treeListRole.OptionsBehavior.Editable = false; //数据不可编辑
                            treeListRole.ExpandAll();//展开所有节点
                        }
                        else
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                            return;
                        }

                        if (user != null)
                        {
                            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                            {
                                String data = HttpClass.httpPost(AppContext.AppConfig.serverUrl + "sys/sysUser/getUser?userId=" + user.id);
                                return data;

                            }, null, (data4) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                            {
                                cmd.HideOpaqueLayer();
                                List<RoleEntity> showRoleList = new List<RoleEntity>();//显示的角色
                                objT = JObject.Parse(data4.ToString());
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    user = objT["result"].ToObject<UserEntity>();

                                    foreach (RoleEntity role in roleList)
                                    {
                                        
                                        if (type.Equals("11"))
                                        {
                                            if (user != null)
                                            {
                                                bool isAdd = false;
                                                String[] idsArr = user.roleIds.Split(',');
                                                foreach (string roleId in idsArr)
                                                {
                                                    if (roleId.Equals("11"))
                                                    {
                                                        isAdd = true;
                                                        break;
                                                    }
                                                }
                                                if (!isAdd)
                                                {
                                                    if (!role.name.Equals("超级管理员") && !role.id.Equals("11"))
                                                    {
                                                        showRoleList.Add(role);
                                                    }
                                                }
                                                else
                                                {
                                                    showRoleList.Add(role);
                                                }
                                            }
                                        }
                                        else if (type.Equals("1"))
                                        {
                                            if (user != null)
                                            {
                                                bool isAdd = false;
                                                String[] idsArr = user.roleIds.Split(',');
                                                foreach (string roleId in idsArr)
                                                {
                                                    if (roleId.Equals("1"))
                                                    {
                                                        isAdd = true;
                                                        break;
                                                    }
                                                }
                                                if (isAdd)
                                                {
                                                    if (!role.id.Equals("11"))
                                                    {
                                                        showRoleList.Add(role);
                                                    }
                                                }
                                                else
                                                {
                                                    if (!role.id.Equals("11") && !role.id.Equals("1"))
                                                    {
                                                        showRoleList.Add(role);
                                                    }
                                                }
                                            }
                                        }
                                        else if (type.Equals("8"))
                                        {
                                            if (user != null)
                                            {
                                                bool isAdd = false;
                                                String[] idsArr = user.roleIds.Split(',');
                                                foreach (string roleId in idsArr)
                                                {
                                                    if (roleId.Equals("8"))
                                                    {
                                                        isAdd = true;
                                                        break;
                                                    }
                                                }
                                                if (isAdd)
                                                {
                                                    if (role.id.Equals("8") || role.id.Equals("9") || role.id.Equals("10"))
                                                    {
                                                        showRoleList.Add(role);
                                                    }
                                                }
                                                else
                                                {
                                                    if (role.id.Equals("9") || role.id.Equals("10"))
                                                    {
                                                        showRoleList.Add(role);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (role.id.Equals("9") || role.id.Equals("10"))
                                                {
                                                    showRoleList.Add(role);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (user != null)
                                            {
                                                foreach (String roleId in roleIdList)
                                                {
                                                    if (!roleId.Equals("11")&&!roleId.Equals("1")&&!roleId.Equals("8"))
                                                    {
                                                        if (role.id.Equals(roleId))
                                                        {
                                                            showRoleList.Add(role);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    dcUser.SetValue(user);
                                    treeListRole.DataSource = showRoleList;//显示角色列表
                                    //DefaultChecked(user.deptIds);//默认选择所属科室
                                    String[] deptArray = user.deptIds.Split(',');
                                    for (int i = 0; i < deptArray.Count(); i++)
                                    {
                                        SetMenuNodeChecked(treeList1, deptArray[i], treeList1.Nodes);
                                    }
                                    GetSelectedRoleIDandName();
                                    treeDept.RefreshEditValue();

                                    //修改的时候密码显示为空
                                    password = user.password;
                                    tePassword.Text = "";
                                    oldLoginName = user.loginName;
                                    //设置多选框选择
                                    String[] strArr = user.roleIds.Split(',');
                                    foreach (string str in strArr)
                                    {
                                        for (int i = 0; i < showRoleList.Count; i++)
                                        {
                                            if (str.Equals(showRoleList[i].id))
                                            {
                                                SetRoleNodeChecked(treeListRole, str, treeListRole.Nodes);
                                                break;
                                            }
                                        }
                                    }
                                    richEditor1.LoadText(user.remarks);

                                    //显示图片
                                    if (user.imgPath != null && user.imgPath.Length > 0)
                                    {
                                        try
                                        {
                                            WebClient web = new WebClient();
                                            var bytes = web.DownloadData(AppContext.AppConfig.serverIp + user.imgPath);
                                            this.pictureBox1.Image = Bitmap.FromStream(new MemoryStream(bytes));
                                        }
                                        catch (Exception ex)
                                        {
                                            Xr.Log4net.LogHelper.Error(ex.Message);
                                        }
                                    }
                                    //WebClient web = new WebClient();
                                    //var bytes = web.DownloadData("http://127.0.0.1:8080/dzkb/uploadFileDir/user_1/2018-12-19/asuo_Splash_0.jpg");
                                    //this.pictureBox1.Image = Bitmap.FromStream(new MemoryStream(bytes));
                                }
                                else
                                {
                                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                                }
                            });
                        }
                        else
                        {
                            user = new UserEntity();
                            cmd.HideOpaqueLayer();
                        }
                    });
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
            });
        }

        private List<String> lstCheckedDeptID = new List<String>();//菜单ID集合
        private List<String> listCheckedRoleID = new List<String>();//角色ID集合

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!dcUser.Validate())
            {
                return;
            }
            if (user.id == null)
            {
                if (tePassword.Text.Length == 0)
                {
                    dcUser.ShowError(tePassword, "密码不能为空");
                    return;
                }
            }
            if (user.id == null || (user.id != null && !user.password.Equals(tePassword.Text) && tePassword.Text.Length != 0))
            {
                if (radioGroup1.EditValue.Equals("1"))
                {
                    //密码复杂度验证
                    var regex = new Regex(@"
                        (?=.*[0-9])                     #必须包含数字
                        (?=.*[a-zA-Z])                  #必须包含小写或大写字母
                        (?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
                        .{8,16}                         #至少8个字符，最多16个字符
                        ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                    if (!regex.IsMatch(tePassword.Text))
                    {
                        dcUser.ShowError(tePassword, "密码数字+字母+符号三种组合以上,至少8位数,最多16位数");
                        return;
                    }
                }
                else
                {
                    //密码复杂度验证
                    var regex = new Regex(@"
                        (?=.*[0-9])                     #必须包含数字
                        (?=.*[a-zA-Z])                  #必须包含小写或大写字母
                        (?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
                        .{6,16}                         #至少6个字符，最多16个字符
                        ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                    if (!regex.IsMatch(tePassword.Text))
                    {
                        dcUser.ShowError(tePassword, "密码数字+字母+符号三种组合以上,至少6位数,最多16位数");
                        return;
                    }
                }
            }
            if (user.id == null)
            {
                if (!tePassword.Text.Equals(tePassword2.Text))
                {
                    dcUser.ShowError(tePassword2, "密码不一致");
                    return;
                }
            }
            else
            {
                if (tePassword2.Text.Length > 0)
                {
                    if (!tePassword.Text.Equals(tePassword2.Text))
                    {
                        dcUser.ShowError(tePassword2, "密码不一致");
                        return;
                    }
                }
            }
            
            dcUser.GetValue(user);
            if (user.id != null && tePassword.Text.Length == 0)
            {
                user.password = password;
            }
            //编辑框的内容要进行转码，不然后台获取的数据会异常缺失数据
            user.remarks = HttpUtility.UrlEncode(richEditor1.InnerHtml, Encoding.UTF8);
            user.imgPath = serviceFilePath;

            //获取选中的角色
            this.listCheckedRoleID.Clear();
            if (treeListRole.Nodes.Count > 0)
            {
                foreach (TreeListNode root in treeListRole.Nodes)
                {
                    if (root.CheckState == CheckState.Checked || root.CheckState == CheckState.Indeterminate)
                    {
                        RoleEntity role = treeListRole.GetDataRecordByNode(root) as RoleEntity;
                        if (role != null)
                        {
                            listCheckedRoleID.Add(role.id);
                        }
                    }
                    GetCheckedID(treeListRole, root, listCheckedRoleID);
                }
            }

            string RoleIdStr = string.Empty;
            foreach (String id in listCheckedRoleID)
            {
                RoleIdStr += id + ",";
            }
            if (RoleIdStr.Length != 0)
                RoleIdStr = RoleIdStr.Substring(0, RoleIdStr.Length - 1);
            else { dcUser.ShowError(treeListRole, "用户角色至少选一个"); return; }
            user.roleIds = RoleIdStr;
            


            //获取选中的菜单权限
            this.lstCheckedDeptID.Clear();
            if (treeList1.Nodes.Count > 0)
            {
                foreach (TreeListNode root in treeList1.Nodes)
                {
                    if (root.CheckState == CheckState.Checked)// || root.CheckState == CheckState.Indeterminate)
                    {
                        DeptEntity dept = treeList1.GetDataRecordByNode(root) as DeptEntity;
                        if (dept != null)
                        {
                            lstCheckedDeptID.Add(dept.id);
                        }
                    }
                    GetCheckedID(treeList1, root, lstCheckedDeptID);
                }
            }

            string idStr = string.Empty;
            foreach (String id in lstCheckedDeptID)
            {
                idStr += id + ",";
            }
            if (idStr.Length != 0)
                idStr = idStr.Substring(0, idStr.Length - 1);
            user.deptIds = idStr;

            String param = PackReflectionEntity<UserEntity>.GetEntityToRequestParameters(user, true);
            if (oldLoginName != null)
            {
                param = param + "&&oldLoginName=" + oldLoginName;
            }
            String url = AppContext.AppConfig.serverUrl + "sys/sysUser/save?";
            cmd.ShowOpaqueLayer(255, true);
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url, param);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }



        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                fileDialog.Title = "请选择文件";
                fileDialog.Filter = "所有文件(*txt*)|*.*"; //设置要选择的文件的类型
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = fileDialog.FileName;//返回文件的完整路径   
                    pictureBox1.ImageLocation = filePath; //显示本地图片
                }
            }
            catch (Exception ex)
            {
                Xr.Log4net.LogHelper.Error(ex.Message);
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, this);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (filePath != "")
            {
                string url = AppContext.AppConfig.serverUrl + "common/uploadFile";
                List<FormItemModel> lstPara = new List<FormItemModel>();
                FormItemModel model = new FormItemModel();
                // 文件
                model.Key = "multipartFile";
                int l = filePath.Length;
                int i = filePath.LastIndexOf("\\") + 2;
                model.FileName = filePath.Substring(i, l - i);
                model.FileContent = new FileStream(filePath, FileMode.Open);
                lstPara.Add(model);

                cmd.ShowOpaqueLayer(255, true);
                this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.PostForm(url, lstPara);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    cmd.HideOpaqueLayer();
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        WebClient web = new WebClient();
                        var bytes = web.DownloadData(AppContext.AppConfig.serverIp + objT["result"][0].ToString());
                        this.pictureBox1.Image = Bitmap.FromStream(new MemoryStream(bytes));
                        serviceFilePath = objT["result"][0].ToString();
                        MessageBoxUtils.Hint("上传图片成功", this);
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                        return;
                    }
                });
            }
            else
            {
                MessageBoxUtils.Show("请选择要上传的文件", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
            }
        }

        /// <summary>
        /// 多线程异步后台处理某些耗时的数据，不会卡死界面
        /// </summary>
        /// <param name="time">线程延迟多少</param>
        /// <param name="workFunc">Func委托，包装耗时处理（不含UI界面处理），示例：(o)=>{ 具体耗时逻辑; return 处理的结果数据 }</param>
        /// <param name="funcArg">Func委托参数，用于跨线程传递给耗时处理逻辑所需要的对象，示例：String对象、JObject对象或DataTable等任何一个值</param>
        /// <param name="workCompleted">Action委托，包装耗时处理完成后，下步操作（一般是更新界面的数据或UI控件），示列：(r)=>{ datagirdview1.DataSource=r; }</param>
        protected void DoWorkAsync(int time, Func<object, object> workFunc, object funcArg = null, Action<object> workCompleted = null)
        {
            var bgWorkder = new BackgroundWorker();


            //Form loadingForm = null;
            //System.Windows.Forms.Control loadingPan = null;
            bgWorkder.WorkerReportsProgress = true;
            bgWorkder.ProgressChanged += (s, arg) =>
            {
                if (arg.ProgressPercentage > 1) return;

            };

            bgWorkder.RunWorkerCompleted += (s, arg) =>
            {

                try
                {
                    bgWorkder.Dispose();

                    if (workCompleted != null)
                    {
                        workCompleted(arg.Result);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException!=null)
                        throw new Exception(ex.InnerException.Message);
                    else
                        throw new Exception(ex.Message);
                }
            };

            bgWorkder.DoWork += (s, arg) =>
            {
                bgWorkder.ReportProgress(1);
                var result = workFunc(arg.Argument);
                arg.Result = result;
                bgWorkder.ReportProgress(100);
                Thread.Sleep(time);
            };

            bgWorkder.RunWorkerAsync(funcArg);
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureViewer pv = new PictureViewer(pictureBox1.Image);
            pv.Show();
        }

        #region 树状多选下拉框相应的事件

        String keyId;//选中的id字符串(1,2,3)
        String keyName;//选中的显示值字符串(科室1,科室2,科室3)

        private List<int> lstCheckedKeyID = new List<int>();//选择局ID集合
        private List<string> lstCheckedKeyName = new List<string>();//选择局Name集合

        private void GetSelectedRoleIDandName()
        {
            this.lstCheckedKeyID.Clear();
            this.lstCheckedKeyName.Clear();

            if (treeDept.Properties.TreeList.Nodes.Count > 0)
            {
                foreach (TreeListNode root in treeDept.Properties.TreeList.Nodes)
                {
                    GetCheckedKeyID(root);
                }
            }
            keyId= "";
            keyName= "";
            foreach (int id in lstCheckedKeyID)
            {
                keyId += id + ",";
            }
            if (keyId.Length > 0)
                keyId = keyId.Substring(0, keyId.Length - 1);

            foreach (string name in lstCheckedKeyName)
            {
                keyName += name + ",";
            }
            if (keyName.Length > 0)
                keyName = keyName.Substring(0, keyName.Length-1);
        }

        /// <summary>
        /// 获取选择状态的数据主键ID集合
        /// </summary>
        /// <param name="parentNode">父级节点</param>
        private void GetCheckedKeyID(TreeListNode parentNode)
        {
            if (parentNode.CheckState != CheckState.Unchecked)
            {
                DeptEntity dept = treeDept.Properties.TreeList.GetDataRecordByNode(parentNode) as DeptEntity;
                if (dept != null)
                {
                    int KeyFieldName = int.Parse(dept.id);
                    string DisplayMember = dept.name;
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
                return;//递归终止
            }
            foreach (TreeListNode node in parentNode.Nodes)
            {
                if (node.CheckState != CheckState.Unchecked)
                {
                    DeptEntity dept = treeDept.Properties.TreeList.GetDataRecordByNode(node) as DeptEntity;
                    if (dept != null)
                    {
                        int KeyFieldName = int.Parse(dept.id);
                        string DisplayMember = dept.name;
                        lstCheckedKeyID.Add(KeyFieldName);
                        lstCheckedKeyName.Add(DisplayMember);
                    }
                }
                GetCheckedKeyID(node);
            }

        }

        //下拉框关闭修改文本框的值
        private void treeDept_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            e.DisplayText = keyName;
        }

        /// <summary>
        /// 默认选中事件
        /// </summary>
        /// <param name="rid"></param>
        private void DefaultChecked(string rid)
        {
            if (rid == null) return;
            List<DeptEntity> deptList = treeDept.Properties.DataSource as List<DeptEntity>;
            List<DeptEntity> deptedList = new List<DeptEntity>();
            String[] arr = rid.Split(',');
            foreach (String id in arr)
            {
                foreach (DeptEntity dept in deptList)
                {
                    if (dept.id.Equals(id))
                    {
                        deptedList.Add(dept);
                        continue;
                    }
                }
            }

            if (treeDept.Properties.TreeList.Nodes.Count > 0)
            {
                foreach (TreeListNode nd in treeDept.Properties.TreeList.Nodes)
                {
                    for (int i = 0; i < deptedList.Count; i++)
                    {
                        String checkedkeyid = deptedList[i].id;
                        if (treeDept.Properties.TreeList.FindNodeByKeyID(checkedkeyid) != null)
                        {
                            treeDept.Properties.TreeList.FindNodeByKeyID(checkedkeyid).Checked = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取选择状态的数据主键ID集合
        /// </summary>
        /// <param name="parentNode">父级节点</param>
        private void GetCheckedID(DevExpress.XtraTreeList.TreeList treeList, TreeListNode parentNode, List<String> strList)
        {
            if (parentNode.Nodes.Count == 0)
            {
                return;//递归终止
            }

            foreach (TreeListNode node in parentNode.Nodes)
            {
                if (node.CheckState == CheckState.Checked || node.CheckState == CheckState.Indeterminate)
                {
                    DeptEntity menu = treeList.GetDataRecordByNode(node) as DeptEntity;
                    if (menu != null)
                    {
                        strList.Add(menu.id);
                    }


                }
                GetCheckedID(treeList, node, strList);
            }
        }


        /// <summary>
        /// 设置菜单CheckBox选中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        private void SetMenuNodeChecked(DevExpress.XtraTreeList.TreeList treeList, String key, TreeListNodes node)
        {
            foreach (TreeListNode childeNode in node)
            {
                DeptEntity data = treeList.GetDataRecordByNode(childeNode) as DeptEntity;
                if (data.id == key)
                {
                    childeNode.Checked = true;
                }
                if (childeNode.HasChildren)
                {
                    SetMenuNodeChecked(treeList, key, childeNode.Nodes);
                }
            }
        }

        /// <summary>
        /// 设置RoleCheckBox选中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        private void SetRoleNodeChecked(DevExpress.XtraTreeList.TreeList treeList, String key, TreeListNodes node)
        {
            foreach (TreeListNode childeNode in node)
            {
                RoleEntity data = treeList.GetDataRecordByNode(childeNode) as RoleEntity;
                if (data.id == key)
                {
                    childeNode.Checked = true;
                }
                if (childeNode.HasChildren)
                {
                    SetMenuNodeChecked(treeList, key, childeNode.Nodes);
                }
            }
        }

        #endregion

    }
}
