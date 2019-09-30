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
using System.IO;
using System.Net;
using Xr.Common;
using Xr.Common.Controls;
using DevExpress.XtraEditors;
using System.Threading;
using System.Web;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace Xr.RtManager.Pages.cms
{
    public partial class DoctorSettingsForm : UserControl
    {
        public DoctorSettingsForm()
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            InitializeComponent();
        }

        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;

        public DoctorInfoEntity doctorInfo { get; set; }
        public DefaultVisitEntity defaultVisit { get; set; }
        /// <summary>
        /// 出诊信息模板
        /// </summary>
        public DefaultVisitEntity defaultVisitTemplate { get; set; }

        private String hospitalId { get; set; }
        private String deptId { get; set; }

        private void DeptSettingsForm_Load(object sender, EventArgs e)
        {
            labelControl1.Font = new Font("微软雅黑", 18, FontStyle.Regular, GraphicsUnit.Pixel);
            labelControl1.ForeColor = Color.FromArgb(255, 0, 0);
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            //把这行删了，再显示分页控件，就是分页了，不过宽度不够显示分页控件
            pageControl1.PageSize = 10000;//一页一万条，不显示分页；
            dcDoctorInfo.DataType = typeof(DoctorInfoEntity);
            dcDefaultVisit.DataType = typeof(DefaultVisitEntity);
            //清除默认出诊时间模板数据
            dcDefaultVisit.ClearValue();
            //menuControl2.borderColor = Color.FromArgb(214, 214, 214);
            //cmd.ShowOpaqueLayer(0f);

            bool scFlag = false;
            foreach (FunctionEntity function in AppContext.Session.functionList)
            {
                if (function.name.Equals("医生设置删除功能"))
                {
                    scFlag = true;
                    break;
                }
            }
            btnDel.Visible = scFlag;

            bool jdFlag = false;
            foreach (FunctionEntity function in AppContext.Session.functionList)
            {
                if (function.name.Equals("医生设置启用/禁用"))
                {
                    jdFlag = true;
                    break;
                }
            }
            btn_tkz.Visible = jdFlag;
            if (!scFlag)
            {
                btn_tkz.Location = new Point(23, 50);
            }

            bool kfFlag = false;
            foreach (FunctionEntity function in AppContext.Session.functionList)
            {
                if (function.name.Equals("医生设置批量更新开放状态"))
                {
                    kfFlag = true;
                    break;
                }
            }
            btnOpenStatus.Visible = kfFlag;
            if (!scFlag && !jdFlag)
            {
                btnOpenStatus.Location = new Point(23, 50);
            }
            else if ((scFlag && !jdFlag) || (!scFlag && jdFlag))
            {
                btnOpenStatus.Location = new Point(113, 50);
            }

            if (!scFlag && !jdFlag && !kfFlag)
            {
                panel1.Height = 60;
            }

            List<DictEntity> dictList = new List<DictEntity>();
            DictEntity dict = new DictEntity();
            dict.value = "0";
            dict.label = "开放";
            dictList.Add(dict);
            dict = new DictEntity();
            dict.value = "1";
            dict.label = "不开放";
            dictList.Add(dict);
            lueIsOpen.Properties.DataSource = dictList;
            lueIsOpen.Properties.DisplayMember = "label";
            lueIsOpen.Properties.ValueMember = "value";

            //查询可操作科室
            String url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept";
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DeptEntity> deptList = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                    treeMenuControl1.KeyFieldName = "id";
                    treeMenuControl1.ParentFieldName = "parentId";
                    treeMenuControl1.DisplayMember = "name";
                    treeMenuControl1.ValueMember = "id";
                    treeMenuControl1.DataSource = deptList;
                    if (deptList.Count > 0)
                    {
                        treeMenuControl1.EditValue = deptList[0].id;
                        deptId = deptList[0].id;
                    }
                    SearchData(1, pageControl1.PageSize);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });




            //查询医院下拉框数据
            String url2 = AppContext.AppConfig.serverUrl + "cms/hospital/findAll";
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url2);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    lueHospital.Properties.DataSource = objT["result"].ToObject<List<HospitalInfoEntity>>();
                    lueHospital.Properties.DisplayMember = "name";
                    lueHospital.Properties.ValueMember = "id";
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });

            //查询状态下拉框数据
            String url3 = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=is_use";
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url3);
                return data;

            }, null, (data2) =>
            {
                JObject objT = JObject.Parse(data2.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    lueIsUse.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                    lueIsUse.Properties.DisplayMember = "label";
                    lueIsUse.Properties.ValueMember = "value";
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });

            //查询挂号类型下拉框数据
            String url4 = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=register_type";
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url4);
                return data;

            }, null, (data3) =>
            {
                JObject objT = JObject.Parse(data3.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    lueRegisterType.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                    lueRegisterType.Properties.DisplayMember = "label";
                    lueRegisterType.Properties.ValueMember = "value";
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });

            //查询性别下拉框数据
            String url5 = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=sex";
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url5);
                return data;

            }, null, (data4) =>
            {
                JObject objT = JObject.Parse(data4.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    lueSex.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                    lueSex.Properties.DisplayMember = "label";
                    lueSex.Properties.ValueMember = "value";
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });

            //查询是否显示下拉框数据
            String url6 = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=show_hide";
            this.DoWorkAsync(0, (o) =>
            {
                String  data = HttpClass.httpPost(url6);
                return data;

            }, null, (data5) =>
            {
                JObject objT = JObject.Parse(data5.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    lueIsShow.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                    lueIsShow.Properties.DisplayMember = "label";
                    lueIsShow.Properties.ValueMember = "value";
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });

            //获取默认出诊时间字典配置
            String url7 = AppContext.AppConfig.serverUrl + "cms/doctor/findDoctorVisitingDict";
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url7);
                return data;

            }, null, (data6) =>
            {
                JObject objT = JObject.Parse(data6.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    defaultVisitTemplate = objT["result"].ToObject<DefaultVisitEntity>();

                    String[] hyArr = defaultVisitTemplate.defaultSourceNumber.Split(new char[] { '-' });
                    defaultVisitTemplate.mStart = defaultVisitTemplate.defaultVisitTimeAm.Substring(0, 5);
                    defaultVisitTemplate.mEnd = defaultVisitTemplate.defaultVisitTimeAm.Substring(6, 5);
                    defaultVisitTemplate.mSubsection = defaultVisitTemplate.segmentalDuration;
                    defaultVisitTemplate.mScene = hyArr[0];
                    defaultVisitTemplate.mOpen = hyArr[1];
                    defaultVisitTemplate.mRoom = hyArr[2];
                    defaultVisitTemplate.mEmergency = hyArr[3];

                    defaultVisitTemplate.aStart = defaultVisitTemplate.defaultVisitTimePm.Substring(0, 5);
                    defaultVisitTemplate.aEnd = defaultVisitTemplate.defaultVisitTimePm.Substring(6, 5);
                    defaultVisitTemplate.aSubsection = defaultVisitTemplate.segmentalDuration;
                    defaultVisitTemplate.aScene = hyArr[0];
                    defaultVisitTemplate.aOpen = hyArr[1];
                    defaultVisitTemplate.aRoom = hyArr[2];
                    defaultVisitTemplate.aEmergency = hyArr[3];

                    defaultVisitTemplate.nStart = defaultVisitTemplate.defaultVisitTimeNight.Substring(0, 5);
                    defaultVisitTemplate.nEnd = defaultVisitTemplate.defaultVisitTimeNight.Substring(6, 5);
                    defaultVisitTemplate.nSubsection = defaultVisitTemplate.segmentalDuration;
                    defaultVisitTemplate.nScene = hyArr[0];
                    defaultVisitTemplate.nOpen = hyArr[1];
                    defaultVisitTemplate.nRoom = hyArr[2];
                    defaultVisitTemplate.nEmergency = hyArr[3];

                    defaultVisitTemplate.allStart = defaultVisitTemplate.defaultVisitTimeAllDay.Substring(0, 5);
                    defaultVisitTemplate.allEnd = defaultVisitTemplate.defaultVisitTimeAllDay.Substring(6, 5);
                    defaultVisitTemplate.allSubsection = defaultVisitTemplate.segmentalDuration;
                    defaultVisitTemplate.allScene = hyArr[0];
                    defaultVisitTemplate.allOpen = hyArr[1];
                    defaultVisitTemplate.allRoom = hyArr[2];
                    defaultVisitTemplate.allEmergency = hyArr[3];
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });
            
        }

        public void SearchData(int pageNo, int pageSize)
        {
            //缩小后宽度不够分页控件显示，所以这里不显示分页了，当前页传10000条，以后要分页的话，把这个10000条去掉就行了
            String param = "pageNo=" + pageNo + "&pageSize=" + pageSize + "&hospital.id=" + hospitalId 
                + "&dept.id=" + deptId + "&pyCode=" + tbPyCode.Text;
            String url = AppContext.AppConfig.serverUrl + "cms/doctor/list?"+param;
            this.DoWorkAsync(500, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DoctorInfoEntity> doctorList = objT["result"]["list"].ToObject<List<DoctorInfoEntity>>();
                    List<DictEntity> useDictList = lueIsUse.Properties.DataSource as List<DictEntity>;
                    foreach (DoctorInfoEntity doctor in doctorList)
                    {
                        if (doctor.isShow.Equals("1"))
                            doctor.isShow = "√";
                        else
                            doctor.isShow = "";
                        if (doctor.ignoreHoliday.Equals("1"))
                            doctor.ignoreHoliday = "√";
                        else
                            doctor.ignoreHoliday = "";
                        foreach (DictEntity dict in useDictList)
                        {
                            if (doctor.isUse.Equals(dict.value))
                                doctor.isUse = dict.label;
                        }
                        if (doctor.isOpen.Equals("0"))
                            doctor.isOpen = "开放";
                        else
                            doctor.isOpen = "不开放";
                    }

                    gcDoctor.DataSource = doctorList;
                    pageControl1.setData(int.Parse(objT["result"]["count"].ToString()),
                    int.Parse(objT["result"]["pageSize"].ToString()),
                    int.Parse(objT["result"]["pageNo"].ToString()));
                    bandedGridView1.Columns[1].OptionsColumn.AllowEdit = false;
                    bandedGridView1.Columns[2].OptionsColumn.AllowEdit = false;
                    bandedGridView1.Columns[3].OptionsColumn.AllowEdit = false;
                    bandedGridView1.Columns[4].OptionsColumn.AllowEdit = false;
                    //bandedGridView1.Columns[5].OptionsColumn.AllowEdit = false;
                    bandedGridView1.Columns[6].OptionsColumn.AllowEdit = false;
                    bandedGridView1.Columns[7].OptionsColumn.AllowEdit = false;
                    bandedGridView1.Columns[8].OptionsColumn.AllowEdit = false;
                    bandedGridView1.Columns[9].OptionsColumn.AllowEdit = false;
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });
        }

        private void pageControl1_Query(int CurrentPage, int PageSize)
        {
            SearchData(CurrentPage, PageSize);
        }



        #region 选择与上传图片
        String pictureFilePath = "";
        String pictureServiceFilePath = "";
        
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
                    pictureFilePath = fileDialog.FileName;//返回文件的完整路径   
                    pbPicture.ImageLocation = pictureFilePath; //显示本地图片
                }
            }
            catch (Exception ex)
            {
                Xr.Log4net.LogHelper.Error(ex.Message);
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (pictureFilePath != "")
            {
                string url = AppContext.AppConfig.serverUrl + "common/uploadFile";
                List<FormItemModel> lstPara = new List<FormItemModel>();
                FormItemModel model = new FormItemModel();
                // 文件
                model.Key = "multipartFile";
                int l = pictureFilePath.Length;
                int i = pictureFilePath.LastIndexOf("\\") + 2;
                model.FileName = pictureFilePath.Substring(i, l - i);
                model.FileContent = new FileStream(pictureFilePath, FileMode.Open);
                lstPara.Add(model);
                cmd.ShowOpaqueLayer();
                this.DoWorkAsync( 500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.PostForm(url, lstPara);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        WebClient web = new WebClient();
                        var bytes = web.DownloadData(AppContext.AppConfig.serverIp + objT["result"][0].ToString());
                        this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(bytes));
                        pictureServiceFilePath = objT["result"][0].ToString();
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("上传图片成功", MainForm);
                    }
                    else
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                });
            }
            else
            {
                MessageBoxUtils.Show("请选择要上传的文件", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            }
        }
        #endregion

        /// <summary>
        /// 设置出诊时间
        /// </summary>
        private void setDefaultVisit()
        {
            defaultVisit = new DefaultVisitEntity();
            String[] hyArr = defaultVisitTemplate.defaultSourceNumber.Split(new char[] { '-' });

            defaultVisit.mStart = defaultVisitTemplate.defaultVisitTimeAm.Substring(0, 5);
            defaultVisit.mEnd = defaultVisitTemplate.defaultVisitTimeAm.Substring(6, 5);
            defaultVisit.mSubsection = defaultVisitTemplate.segmentalDuration;
            defaultVisit.mScene = hyArr[0];
            defaultVisit.mOpen = hyArr[1];
            defaultVisit.mRoom = hyArr[2];
            defaultVisit.mEmergency = hyArr[3];

            defaultVisit.aStart = defaultVisitTemplate.defaultVisitTimePm.Substring(0, 5);
            defaultVisit.aEnd = defaultVisitTemplate.defaultVisitTimePm.Substring(6, 5);
            defaultVisit.aSubsection = defaultVisitTemplate.segmentalDuration;
            defaultVisit.aScene = hyArr[0];
            defaultVisit.aOpen = hyArr[1];
            defaultVisit.aRoom = hyArr[2];
            defaultVisit.aEmergency = hyArr[3];

            defaultVisit.nStart = defaultVisitTemplate.defaultVisitTimeNight.Substring(0, 5);
            defaultVisit.nEnd = defaultVisitTemplate.defaultVisitTimeNight.Substring(6, 5);
            defaultVisit.nSubsection = defaultVisitTemplate.segmentalDuration;
            defaultVisit.nScene = hyArr[0];
            defaultVisit.nOpen = hyArr[1];
            defaultVisit.nRoom = hyArr[2];
            defaultVisit.nEmergency = hyArr[3];

            defaultVisit.allStart = defaultVisitTemplate.defaultVisitTimeAllDay.Substring(0, 5);
            defaultVisit.allEnd = defaultVisitTemplate.defaultVisitTimeAllDay.Substring(6, 5);
            defaultVisit.allSubsection = defaultVisitTemplate.segmentalDuration;
            defaultVisit.allScene = hyArr[0];
            defaultVisit.allOpen = hyArr[1];
            defaultVisit.allRoom = hyArr[2];
            defaultVisit.allEmergency = hyArr[3];

            dcDefaultVisit.SetValue(defaultVisit);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {   
            //本来保存成功后清除一次就行了，不过为了预防新增或修改事件触发的过程中出现异常
            //导致面板有数据却无法编辑,导致新增的时候有默认数据，所以这里需要再清一次
            //清除医生数据
            dcDoctorInfo.ClearValue();
            pbPicture.Image = null;
            pbPicture.Refresh();
            pictureServiceFilePath = null;
            pictureFilePath = "";
            //清除排班数据
            dcDefaultVisit.ClearValue();
            tableLayoutPanel4.Enabled = false;
            pbDispose();

            setDefaultVisit();
            tableLayoutPanel4.Enabled = true;
            groupBox1.Enabled = true;
            doctorInfo = new DoctorInfoEntity();
            cbIgnoreHoliday.CheckState = CheckState.Checked;
            List<DictEntity> isShowList = lueIsShow.Properties.DataSource as List<DictEntity>;
            if (isShowList.Count > 0)
                lueIsShow.EditValue = isShowList[0].value;
            List<DictEntity> isUseList = lueIsUse.Properties.DataSource as List<DictEntity>;
            if (isUseList.Count > 0)
                lueIsUse.EditValue = isUseList[0].value;
            List<DictEntity> registerTypeList = lueRegisterType.Properties.DataSource as List<DictEntity>;
            if (registerTypeList.Count > 0)
                lueRegisterType.EditValue = registerTypeList[0].value;
            List<DictEntity> sexList = lueSex.Properties.DataSource as List<DictEntity>;
            if (sexList.Count > 0)
                lueSex.EditValue = sexList[0].value;
            lueHospital.EditValue = AppContext.Session.hospitalId;
            treeDeptId.EditValue = deptId;
            lueIsOpen.EditValue = "0";

        }


        private void btnUp_Click(object sender, EventArgs e)
        {
            //本来保存成功后清除一次就行了，不过为了预防新增或修改事件触发的过程中出现异常
            //导致面板有数据却无法编辑,导致新增的时候有默认数据，所以这里需要再清一次
            //清除医生数据
            dcDoctorInfo.ClearValue();
            pbPicture.Image = null;
            pbPicture.Refresh();
            pictureServiceFilePath = null;
            pictureFilePath = "";
            //清除排班数据
            dcDefaultVisit.ClearValue();
            tableLayoutPanel4.Enabled = false;
            pbDispose();

            doctorInfo = new DoctorInfoEntity();
            var selectedRow = bandedGridView1.GetFocusedRow() as DoctorInfoEntity;
            if (selectedRow == null)
                return;

            cmd.ShowOpaqueLayer();
            String url = AppContext.AppConfig.serverUrl + "cms/doctor/findById?id=" + selectedRow.id;
            this.DoWorkAsync(250, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    doctorInfo = objT["result"].ToObject<DoctorInfoEntity>();
                    doctorInfo.hospitalId = doctorInfo.dept.hospital.id;
                    doctorInfo.deptId = doctorInfo.dept.id;
                    if (doctorInfo.ignoreHoliday.Equals("1"))
                        cbIgnoreHoliday.CheckState = CheckState.Checked;
                    if (doctorInfo.ignoreYear.Equals("1"))
                        cbIgnoreYear.CheckState = CheckState.Checked;
                    dcDoctorInfo.SetValue(doctorInfo);
                    //显示图片
                    pictureServiceFilePath = doctorInfo.pictureUrl;
                    //医院的机子有些请求不到路径的时候，会卡的1分钟左右，所以下载图片用异步
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        if (pictureServiceFilePath != null && pictureServiceFilePath.Length > 0)
                        {
                            try
                            {
                                WebClient web = new WebClient();
                                var bytes = web.DownloadData(pictureServiceFilePath);
                                return bytes;

                            }
                            catch (Exception ex)
                            {
                                Xr.Log4net.LogHelper.Error(ex.Message);
                            }
                        }
                        return null;
                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        if (data2 != null)
                            this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(data2 as byte[]));
                    });

                    groupBox1.Enabled = true;
                    setDefaultVisit();
                    tableLayoutPanel4.Enabled = true;

                    //获取已排班信息
                    url = AppContext.AppConfig.serverUrl + "cms/doctor/findDoctorVisitingList?deptId=" + doctorInfo.dept.id + "&doctorId=" + selectedRow.id;
                    this.DoWorkAsync(250, (o) =>
                    {
                        data = HttpClass.httpPost(url);
                        return data;

                    }, null, (data2) =>
                    {
                        cmd.HideOpaqueLayer();
                        objT = JObject.Parse(data2.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            List<WorkingDayEntity> workingDayList = objT["result"].ToObject<List<WorkingDayEntity>>();
                            if (workingDayList.Count > 0)
                            {
                                setWorkingDay(workingDayList);
                            }
                            cmd.HideOpaqueLayer();
                        }
                        else
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        }
                    });
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //检验并取值
            if (!dcDoctorInfo.Validate())
            {
                return;
            }
            dcDoctorInfo.GetValue(doctorInfo);
            //加号数量默认为0
            doctorInfo.addNum = "0";

            if (cbIgnoreHoliday.CheckState == CheckState.Checked)
                doctorInfo.ignoreHoliday = "1";
            else
                doctorInfo.ignoreHoliday = "0";

            if (cbIgnoreYear.CheckState == CheckState.Checked)
                doctorInfo.ignoreYear = "1";
            else doctorInfo.ignoreYear = "0";

            //if (pictureServiceFilePath == null || pictureServiceFilePath.Length == 0)
            //{
            //    dcDoctorInfo.ShowError(pbPicture, "请先上传文件");
            //    return;
            //}
            doctorInfo.pictureUrl = pictureServiceFilePath;

            List<WorkingDayEntity> workingDayList = new List<WorkingDayEntity>();
            //获取排班信息
            int days = tabControl1.Controls.Count; //排班天数
            if (days > 0)
            {
                for (int i = 0; i < days; i++)
                {
                    TabPage tabPage = (TabPage)tabControl1.Controls[i];//周几的面板
                    String zj = tabPage.Text;
                    String week = zj.Substring(1, 1);
                    for (int period = 0; period < 4; period++)//循环上午、下午、晚上、全天
                    {
                        if (tabPage.Controls.Count == 0) break;
                        TableLayoutPanel tlp = (TableLayoutPanel)tabPage.Controls[period];//排班
                        if (tlp.Enabled)
                        {
                            CheckBox cbIsUse = (CheckBox)tlp.GetControlFromPosition(0, 1);
                            CheckBox cbAuto = (CheckBox)tlp.GetControlFromPosition(0, 2);
                            for (int r = 1; r < tlp.RowCount; r++)//行
                            {
                                WorkingDayEntity wordingDay = new WorkingDayEntity();
                                wordingDay.week = week; //周几
                                wordingDay.period = period.ToString(); //0：上午，1：下午，2：晚上 3：全天
                                if (cbIsUse.CheckState == CheckState.Checked)
                                    wordingDay.isUse = "0";
                                else
                                    wordingDay.isUse = "1";
                                if (cbAuto.CheckState == CheckState.Checked)
                                    wordingDay.autoSchedule = "0";
                                else
                                    wordingDay.autoSchedule = "1";

                                for (int c = 1; c < tlp.ColumnCount; c++)//列 
                                {
                                    TextEdit te = (TextEdit)tlp.GetControlFromPosition(c, r);
                                    if (c == 1)
                                        wordingDay.beginTime = te.Text;
                                    else if (c == 2)
                                        wordingDay.endTime = te.Text;
                                    else if (c == 3)
                                        wordingDay.numSite = te.Text;
                                    else if (c == 4)
                                        wordingDay.numOpen = te.Text;
                                    else if (c == 5)
                                        wordingDay.numClinic = te.Text;
                                    else if (c == 6)
                                        wordingDay.numYj = "0"; //应急列不需要了  传0
                                        //wordingDay.numYj = te.Text;
                                }
                                if (wordingDay.beginTime.Length != 0)
                                {
                                    workingDayList.Add(wordingDay);
                                }
                            }
                        }
                    }
                }
            }

            //先保存模板再保存默认出诊数据,新增医生时这个list传0个值
            List<WorkingDayEntity> visitingTimeList = new List<WorkingDayEntity>();
            if (doctorInfo.id != null)
            {
                WorkingDayEntity visitingTime = new WorkingDayEntity();
                visitingTime.doctorId = doctorInfo.id;
                visitingTime.period = "0";
                visitingTime.beginTime = defaultVisit.mStart;
                visitingTime.endTime = defaultVisit.mEnd;
                visitingTime.segmentalDuration = defaultVisit.mSubsection;
                visitingTime.numClinic = defaultVisit.mRoom;
                visitingTime.numOpen = defaultVisit.mOpen;
                visitingTime.numSite = defaultVisit.mScene;
                visitingTime.numYj = defaultVisit.mEmergency;
                visitingTimeList.Add(visitingTime);

                visitingTime = new WorkingDayEntity();
                visitingTime.doctorId = doctorInfo.id;
                visitingTime.period = "1";
                visitingTime.beginTime = defaultVisit.aStart;
                visitingTime.endTime = defaultVisit.aEnd;
                visitingTime.segmentalDuration = defaultVisit.aSubsection;
                visitingTime.numClinic = defaultVisit.aRoom;
                visitingTime.numOpen = defaultVisit.aOpen;
                visitingTime.numSite = defaultVisit.aScene;
                visitingTime.numYj = defaultVisit.aEmergency;
                visitingTimeList.Add(visitingTime);

                visitingTime = new WorkingDayEntity();
                visitingTime.doctorId = doctorInfo.id;
                visitingTime.period = "2";
                visitingTime.beginTime = defaultVisit.nStart;
                visitingTime.endTime = defaultVisit.nEnd;
                visitingTime.segmentalDuration = defaultVisit.nSubsection;
                visitingTime.numClinic = defaultVisit.nRoom;
                visitingTime.numOpen = defaultVisit.nOpen;
                visitingTime.numSite = defaultVisit.nScene;
                visitingTime.numYj = defaultVisit.nEmergency;
                visitingTimeList.Add(visitingTime);

                visitingTime = new WorkingDayEntity();
                visitingTime.doctorId = doctorInfo.id;
                visitingTime.period = "3";
                visitingTime.beginTime = defaultVisit.allStart;
                visitingTime.endTime = defaultVisit.allEnd;
                visitingTime.segmentalDuration = defaultVisit.allSubsection;
                visitingTime.numClinic = defaultVisit.allRoom;
                visitingTime.numOpen = defaultVisit.allOpen;
                visitingTime.numSite = defaultVisit.allScene;
                visitingTime.numYj = defaultVisit.allEmergency;
                visitingTimeList.Add(visitingTime);
            }
            
     
            String autoSchedule = "1";
            if (this.cbAuto.CheckState == CheckState.Checked)
                autoSchedule = "0";

            String workStr = Newtonsoft.Json.JsonConvert.SerializeObject(visitingTimeList);
            String param = "deptId=" + doctorInfo.deptId + "&hospitalId=" + doctorInfo.hospitalId +
                "&doctorIds=" + doctorInfo.id + "&autoSchedule=" + autoSchedule + "&workStr=" + workStr;
            String url = AppContext.AppConfig.serverUrl + "cms/doctor/saveVisitingTime?";
            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url, param);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    workStr = Newtonsoft.Json.JsonConvert.SerializeObject(workingDayList);
                    //文本编辑框的内容要转编码，不然后台获取的时候会不对
                    doctorInfo.synopsis = HttpUtility.UrlEncode(doctorInfo.synopsis, Encoding.UTF8);
                    param = PackReflectionEntity<DoctorInfoEntity>.GetEntityToRequestParameters(doctorInfo, true);
                    param += "&workStr=" + workStr;
                    //请求接口
                    url = AppContext.AppConfig.serverUrl + "cms/doctor/save?";
                    cmd.ShowOpaqueLayer();
                    this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        data = HttpClass.httpPost(url, param);
                        return data;

                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        objT = JObject.Parse(data2.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            groupBox1.Enabled = false;
                            //清除医生数据
                            dcDoctorInfo.ClearValue();
                            pbPicture.Image = null;
                            pbPicture.Refresh();
                            pictureServiceFilePath = null;
                            pictureFilePath = "";
                            //清除排班数据
                            dcDefaultVisit.ClearValue();
                            tableLayoutPanel4.Enabled = false;
                            pbDispose();
                            pageControl1_Query(pageControl1.CurrentPage, pageControl1.PageSize);
                            MessageBoxUtils.Hint("保存成功！", MainForm);
                        }
                        else
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        }
                    });
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            List<DoctorInfoEntity> doctorList = bandedGridView1.DataSource as List<DoctorInfoEntity>;
            String ids = "";
            foreach (DoctorInfoEntity selectedRow in doctorList)
            {
                if (selectedRow.check)
                {
                    ids += selectedRow.id + ",";
                }
            }

            if (ids.Length == 0) return;
            ids = ids.Substring(0, ids.Length-1);
            if (MessageBoxUtils.Show("删除医生会停诊所有排班，确定要删除吗?", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                String param = "?ids=" + ids;
                String url = AppContext.AppConfig.serverUrl + "cms/doctor/delete" + param;
                cmd.ShowOpaqueLayer();
                this.DoWorkAsync( 500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    JObject objT = JObject.Parse(r.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        SearchData(pageControl1.CurrentPage, pageControl1.PageSize);
                        MessageBoxUtils.Hint("删除成功!", MainForm);
                    }
                    else
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }
                });
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var edit = new RichEditorForm();
            edit.ImagUploadUrl = AppContext.AppConfig.serverUrl;
            edit.text = doctorInfo.synopsis;
            if (edit.ShowDialog() == DialogResult.OK)
            {
                doctorInfo.synopsis = edit.text;
            }
        }

        private void lueHospital_EditValueChanged(object sender, EventArgs e)
        {
            if (lueHospital.EditValue == null || lueHospital.EditValue.ToString().Length == 0)
            {
                treeDeptId.Properties.DataSource = null;
                return;
            }
            HospitalInfoEntity hospitalInfo = lueHospital.GetSelectedDataRow() as HospitalInfoEntity;
            //查询科室下拉框数据
            String url = AppContext.AppConfig.serverUrl + "cms/dept/findAll?hospital.code=" + hospitalInfo.code + "&deptIds=" + AppContext.Session.deptIds;
            String data = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(data);
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                List<DeptEntity> deptList = objT["result"].ToObject<List<DeptEntity>>();
                treeDeptId.Properties.DataSource = deptList;
                treeDeptId.Properties.TreeList.KeyFieldName = "id";
                treeDeptId.Properties.TreeList.ParentFieldName = "parentId";
                treeDeptId.Properties.DisplayMember = "name";
                treeDeptId.Properties.ValueMember = "id";
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
            }
        }

        private void treeMenuControl1_MenuItemClick(object sender, EventArgs e, object selectItem)
        {
            DeptEntity dept = selectItem as DeptEntity;
            hospitalId = dept.hospitalId;
            deptId = dept.id;
            cmd.ShowOpaqueLayer();
            SearchData(1, pageControl1.PageSize);
        }

        /// <summary>
        /// 清空排班控件（释放资源）
        /// </summary>
        private void pbDispose()
        {
            //清空周一的控件
            int CntControls = tabPage1.Controls.Count;
            for (int i = 0; i < CntControls; i++)
            {
                if (tabPage1.Controls[0] != null)
                    tabPage1.Controls[0].Dispose();
            }
            //清空周二的控件
            CntControls = tabPage7.Controls.Count;
            for (int i = 0; i < CntControls; i++)
            {
                if (tabPage7.Controls[0] != null)
                    tabPage7.Controls[0].Dispose();
            }
            //清空周三的控件
            CntControls = tabPage6.Controls.Count;
            for (int i = 0; i < CntControls; i++)
            {
                if (tabPage6.Controls[0] != null)
                    tabPage6.Controls[0].Dispose();
            }
            //清空周四的控件
            CntControls = tabPage5.Controls.Count;
            for (int i = 0; i < CntControls; i++)
            {
                if (tabPage5.Controls[0] != null)
                    tabPage5.Controls[0].Dispose();
            }
            //清空周五的控件
            CntControls = tabPage4.Controls.Count;
            for (int i = 0; i < CntControls; i++)
            {
                if (tabPage4.Controls[0] != null)
                    tabPage4.Controls[0].Dispose();
            }
            //清空周六的控件
            CntControls = tabPage3.Controls.Count;
            for (int i = 0; i < CntControls; i++)
            {
                if (tabPage3.Controls[0] != null)
                    tabPage3.Controls[0].Dispose();
            }
            //清空周日的控件
            CntControls = tabPage2.Controls.Count;
            for (int i = 0; i < CntControls; i++)
            {
                if (tabPage2.Controls[0] != null)
                    tabPage2.Controls[0].Dispose();
            }
        }


        private void buttonControl2_Click(object sender, EventArgs e)
        {
            //清除排班数据
            pbDispose();
            cmd.ShowOpaqueLayer();

            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                return null;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                CheckState morning = cbMorning.CheckState;
                CheckState afternoon = cbAfternoon.CheckState;
                CheckState night = cbNight.CheckState;
                CheckState allDay = cbAllAay.CheckState;
                if (morning != CheckState.Checked && afternoon != CheckState.Checked
                    && night != CheckState.Checked && allDay != CheckState.Checked)
                {
                    cmd.HideOpaqueLayer();
                    return;
                }
                //获取默认排班数据
                defaultVisit = new DefaultVisitEntity();
                dcDefaultVisit.GetValue(defaultVisit);

                //分段数量
                int rowMorningNum = 0;
                int rowAfternoonNum = 0;
                int rowNightNum = 0;
                int rowAllDayNum = 0;

                #region 计算分段数量
                //计算早上的分段数量
                if (morning == CheckState.Checked)
                {
                    if (defaultVisit.mStart.Trim().Length == 0 || defaultVisit.mEnd.Trim().Length == 0
                        || defaultVisit.mSubsection.Trim().Length == 0)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("上午的设置不能为空", MainForm);
                        return;
                    }
                    String[] startArr = defaultVisit.mStart.Split(new char[] { ':', '：' });
                    String[] endArr = defaultVisit.mEnd.Split(new char[] { ':', '：' });
                    if (startArr.Length != 2)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("上午的开始时间设置有误", MainForm);
                        return;
                    }
                    if (endArr.Length != 2)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("上午的结束时间设置有误", MainForm);
                        return;
                    }
                    DateTime d1 = new DateTime(2004, 1, 1, int.Parse(startArr[0]), int.Parse(startArr[1]), 00);
                    DateTime d2 = new DateTime(2004, 1, 1, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                    TimeSpan d3 = d2.Subtract(d1);
                    int minute = d3.Hours * 60 + d3.Minutes;
                    if (minute <= 0)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("上午结束时间不能小于或等于开始时间", MainForm);
                        return;
                    }
                    if (minute < int.Parse(defaultVisit.mSubsection))
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("上午分段时间大于总时间", MainForm);
                        return;
                    }
                    if (minute % int.Parse(defaultVisit.mSubsection) == 0)
                        rowMorningNum = minute / int.Parse(defaultVisit.mSubsection);
                    else
                        rowMorningNum = (minute / int.Parse(defaultVisit.mSubsection))+1;
                }

                //计算下午的分段数量
                if (afternoon == CheckState.Checked)
                {
                    if (defaultVisit.aStart.Trim().Length == 0 || defaultVisit.aEnd.Trim().Length == 0
                        || defaultVisit.aSubsection.Trim().Length == 0)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("下午的设置不能为空", MainForm);
                        return;
                    }
                    String[] startArr = defaultVisit.aStart.Split(new char[] { ':', '：' });
                    String[] endArr = defaultVisit.aEnd.Split(new char[] { ':', '：' });
                    if (startArr.Length != 2)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("下午的开始时间设置有误", MainForm);
                        return;
                    }
                    if (endArr.Length != 2)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("下午的结束时间设置有误", MainForm);
                        return;
                    }
                    DateTime d1 = new DateTime(2004, 1, 1, int.Parse(startArr[0]), int.Parse(startArr[1]), 00);
                    DateTime d2 = new DateTime(2004, 1, 1, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                    TimeSpan d3 = d2.Subtract(d1);
                    int minute = d3.Hours * 60 + d3.Minutes;
                    if (minute <= 0)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("下午结束时间不能小于或等于开始时间", MainForm);
                        return;
                    }
                    if (minute < int.Parse(defaultVisit.aSubsection))
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("下午分段时间大于总时间", MainForm);
                        return;
                    }
                    if(minute % int.Parse(defaultVisit.aSubsection) == 0)
                        rowAfternoonNum = minute / int.Parse(defaultVisit.aSubsection);
                    else
                        rowAfternoonNum = (minute / int.Parse(defaultVisit.aSubsection))+1;
                }

                //计算晚上的分段数量
                if (night == CheckState.Checked)
                {
                    if (defaultVisit.nStart.Trim().Length == 0 || defaultVisit.nEnd.Trim().Length == 0
                        || defaultVisit.nSubsection.Trim().Length == 0)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("晚上的设置不能为空", MainForm);
                        return;
                    }
                    String[] startArr = defaultVisit.nStart.Split(new char[] { ':', '：' });
                    String[] endArr = defaultVisit.nEnd.Split(new char[] { ':', '：' });
                    if (startArr.Length != 2)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("晚上的开始时间设置有误", MainForm);
                        return;
                    }
                    if (endArr.Length != 2)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("晚上的结束时间设置有误", MainForm);
                        return;
                    }
                    DateTime d1 = new DateTime(2004, 1, 1, int.Parse(startArr[0]), int.Parse(startArr[1]), 00);
                    DateTime d2 = new DateTime();
                    if (endArr[0].Equals("24"))
                        d2 = new DateTime(2004, 1, 2, 00, int.Parse(endArr[1]), 00);
                    if (int.Parse(endArr[0]) < int.Parse(startArr[0]))
                        d2 = new DateTime(2004, 1, 2, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                    else
                        d2 = new DateTime(2004, 1, 1, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                    TimeSpan d3 = d2.Subtract(d1);
                    int minute = d3.Hours * 60 + d3.Minutes;
                    if (minute <= 0)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("晚上结束时间不能小于或等于开始时间", MainForm);
                        return;
                    }
                    if (minute < int.Parse(defaultVisit.nSubsection))
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("晚上分段时间大于总时间", MainForm);
                        return;
                    }
                    if(minute % int.Parse(defaultVisit.nSubsection) == 0)
                        rowNightNum = minute / int.Parse(defaultVisit.nSubsection);
                    else
                        rowNightNum = (minute / int.Parse(defaultVisit.nSubsection))+1;
                }

                //计算全天的分段数量
                if (allDay == CheckState.Checked)
                {
                    if (defaultVisit.allStart.Trim().Length == 0 || defaultVisit.allEnd.Trim().Length == 0
                        || defaultVisit.allSubsection.Trim().Length == 0)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("全天的设置不能为空", MainForm);
                        return;
                    }
                    String[] startArr = defaultVisit.allStart.Split(new char[] { ':', '：' });
                    String[] endArr = defaultVisit.allEnd.Split(new char[] { ':', '：' });
                    if (startArr.Length != 2)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("全天的开始时间设置有误", MainForm);
                        return;
                    }
                    if (endArr.Length != 2)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("全天的结束时间设置有误", MainForm);
                        return;
                    }
                    DateTime d1 = new DateTime(2004, 1, 1, int.Parse(startArr[0]), int.Parse(startArr[1]), 00);
                    DateTime d2 = new DateTime();
                    if (endArr[0].Equals("24"))
                        d2 = new DateTime(2004, 1, 2, 00, int.Parse(endArr[1]), 00);
                    else
                        d2 = new DateTime(2004, 1, 1, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                    TimeSpan d3 = d2.Subtract(d1);
                    int minute = d3.Hours * 60 + d3.Minutes;
                    if (minute <= 0)
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("全天结束时间不能小于或等于开始时间", MainForm);
                        return;
                    }
                    if (minute < int.Parse(defaultVisit.nSubsection))
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Hint("全天分段时间大于总时间", MainForm);
                        return;
                    }
                    if(minute % int.Parse(defaultVisit.allSubsection) == 0)
                        rowAllDayNum = minute / int.Parse(defaultVisit.allSubsection);
                    else
                        rowAllDayNum = (minute / int.Parse(defaultVisit.allSubsection))+1;
                }
                #endregion

                #region 生成周一到周日的排班
                //生成周一到周日的排班
                for (int i = 1; i <= 7; i++)
                {
                    int blpY = 20; //TableLayoutPanel的y轴位置
                    for (int j = 0; j < 4; j++)
                    {
                        //j=0:上午 j=1:下午 j=2:晚上 j=3:全天 
                        TableLayoutPanel tlpMorning = new TableLayoutPanel();
                        int row = 0;//行数(包括标题)
                        DateTime dt1 = new DateTime();//开始时间
                        DateTime dt2 = new DateTime();//结束时间
                        String timeInterval = ""; //
                        if (j == 0)
                        {
                            if (rowMorningNum > 3) row = rowMorningNum + 1;
                            else row = 4;
                            dt1 = DateTime.Parse("2008-08-08 " + defaultVisit.mStart + ":00");
                            dt2 = dt1.AddMinutes(int.Parse(defaultVisit.mSubsection));
                            timeInterval = "上午";
                            if (morning != CheckState.Checked)
                            {
                                tlpMorning.Enabled = false;
                            }
                        }
                        else if (j == 1)
                        {
                            if (rowAfternoonNum > 3) row = rowAfternoonNum + 1;
                            else row = 4;
                            dt1 = DateTime.Parse("2008-08-08 " + defaultVisit.aStart + ":00");
                            dt2 = dt1.AddMinutes(int.Parse(defaultVisit.aSubsection));
                            timeInterval = "下午";
                            if (afternoon != CheckState.Checked)
                            {
                                tlpMorning.Enabled = false;
                            }
                        }
                        else if (j == 2)
                        {
                            if (rowNightNum > 3) row = rowNightNum + 1;
                            else row = 4;
                            dt1 = DateTime.Parse("2008-08-08 " + defaultVisit.nStart + ":00");
                            dt2 = dt1.AddMinutes(int.Parse(defaultVisit.nSubsection));
                            timeInterval = "晚上";
                            if (night != CheckState.Checked)
                            {
                                tlpMorning.Enabled = false;
                            }
                        }
                        else if (j == 3)
                        {
                            if (rowAllDayNum > 3) row = rowAllDayNum + 1;
                            else row = 4;
                            dt1 = DateTime.Parse("2008-08-08 " + defaultVisit.allStart + ":00");
                            dt2 = dt1.AddMinutes(int.Parse(defaultVisit.allSubsection));
                            timeInterval = "全天";
                            if (allDay != CheckState.Checked)
                            {
                                tlpMorning.Enabled = false;
                            }
                        }

                        tlpMorning.ColumnCount = 7;
                        tlpMorning.RowCount = row;

                        tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
                        tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
                        tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
                        tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
                        tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
                        tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
                        tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));

                        for (int n = 0; n < row; n++)
                        {
                            tlpMorning.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
                        }
                        tlpMorning.Size = new System.Drawing.Size(350, row * 30);
                        //标题栏
                        Label label = new Label();
                        label.Dock = DockStyle.Fill;
                        label.TextAlign = ContentAlignment.BottomCenter;
                        label.Font = new Font("微软雅黑", 10);
                        label.Text = "开始";
                        tlpMorning.Controls.Add(label, 1, 0);
                        label = new Label();
                        label.Dock = DockStyle.Fill;
                        label.TextAlign = ContentAlignment.BottomCenter;
                        label.Font = new Font("微软雅黑", 10);
                        label.Text = "结束";
                        tlpMorning.Controls.Add(label, 2, 0);
                        label = new Label();
                        label.Dock = DockStyle.Fill;
                        label.TextAlign = ContentAlignment.BottomCenter;
                        label.Font = new Font("微软雅黑", 10);
                        label.Text = "现场";
                        tlpMorning.Controls.Add(label, 3, 0);
                        label = new Label();
                        label.Dock = DockStyle.Fill;
                        label.TextAlign = ContentAlignment.BottomCenter;
                        label.Font = new Font("微软雅黑", 10);
                        label.Text = "预约";
                        tlpMorning.Controls.Add(label, 4, 0);
                        label = new Label();
                        label.Dock = DockStyle.Fill;
                        label.TextAlign = ContentAlignment.BottomCenter;
                        label.Font = new Font("微软雅黑", 10);
                        label.Text = "诊间";
                        tlpMorning.Controls.Add(label, 5, 0);
                        label = new Label();
                        label.Dock = DockStyle.Fill;
                        label.TextAlign = ContentAlignment.BottomCenter;
                        label.Font = new Font("微软雅黑", 10);
                        label.Text = "应急";
                        label.Visible = false; 
                        tlpMorning.Controls.Add(label, 6, 0);

                        bool teEnabled = true;//当行数小于3的时候，空白文本框需要设为不可选
                        String start = "";
                        String end = "";
                        String scene = "";
                        String open = "";
                        String room = "";
                        String emergency = "";
                        CheckState checkState = CheckState.Unchecked;
                        if (j == 0 && morning == CheckState.Checked)
                        {
                            scene = defaultVisit.mScene;
                            open = defaultVisit.mOpen;
                            room = defaultVisit.mRoom;
                            emergency = defaultVisit.mEmergency;
                            checkState = CheckState.Checked;
                        }
                        if (j == 1 && afternoon == CheckState.Checked)
                        {
                            scene = defaultVisit.aScene;
                            open = defaultVisit.aOpen;
                            room = defaultVisit.aRoom;
                            emergency = defaultVisit.aEmergency;
                            checkState = CheckState.Checked;
                        }
                        if (j == 2 && night == CheckState.Checked)
                        {
                            scene = defaultVisit.nScene;
                            open = defaultVisit.nOpen;
                            room = defaultVisit.nRoom;
                            emergency = defaultVisit.nEmergency;
                            checkState = CheckState.Checked;
                        }
                        if (j == 3 && allDay == CheckState.Checked)
                        {
                            scene = defaultVisit.allScene;
                            open = defaultVisit.allOpen;
                            room = defaultVisit.allRoom;
                            emergency = defaultVisit.allEmergency;
                            checkState = CheckState.Checked;
                        }
                        for (int r = 1; r < row; r++)
                        {
                            if (j == 0 && morning == CheckState.Checked)
                            {
                                start = dt1.ToString("HH:mm");
                                if (r == rowMorningNum)
                                    end = defaultVisit.mEnd;
                                else
                                    end = dt2.ToString("HH:mm");
                                dt1 = dt2;
                                dt2 = dt1.AddMinutes(int.Parse(defaultVisit.mSubsection));
                                if (r > rowMorningNum)
                                {
                                    start = "";
                                    end = "";
                                    scene = "";
                                    open = "";
                                    room = "";
                                    emergency = "";
                                    teEnabled = false;
                                }
                            }
                            if (j == 1 && afternoon == CheckState.Checked)
                            {
                                start = dt1.ToString("HH:mm");
                                if (r == rowAfternoonNum)
                                    end = defaultVisit.aEnd;
                                else
                                    end = dt2.ToString("HH:mm");
                                dt1 = dt2;
                                dt2 = dt1.AddMinutes(int.Parse(defaultVisit.aSubsection));
                                if (r > rowAfternoonNum)
                                {
                                    start = "";
                                    end = "";
                                    scene = "";
                                    open = "";
                                    room = "";
                                    emergency = "";
                                    teEnabled = false;
                                }
                            }
                            if (j == 2 && night == CheckState.Checked)
                            {
                                start = dt1.ToString("HH:mm");
                                if (r == rowNightNum)
                                    end = defaultVisit.nEnd;
                                else
                                    end = dt2.ToString("HH:mm");
                                dt1 = dt2;
                                dt2 = dt1.AddMinutes(int.Parse(defaultVisit.nSubsection));
                                if (r > rowNightNum)
                                {
                                    start = "";
                                    end = "";
                                    scene = "";
                                    open = "";
                                    room = "";
                                    emergency = "";
                                    teEnabled = false;
                                }
                            }
                            if (j == 3 && allDay == CheckState.Checked)
                            {
                                start = dt1.ToString("HH:mm");
                                if (r == rowAllDayNum)
                                    end = defaultVisit.allEnd;
                                else
                                    end = dt2.ToString("HH:mm");
                                dt1 = dt2;
                                dt2 = dt1.AddMinutes(int.Parse(defaultVisit.allSubsection));
                                if (r > rowAllDayNum)
                                {
                                    start = "";
                                    end = "";
                                    scene = "";
                                    open = "";
                                    room = "";
                                    emergency = "";
                                    teEnabled = false;
                                }
                            }
                            for (int c = 0; c < 7; c++)
                            {
                                if (r == 1 && c == 0)
                                {
                                    //第一行第一列
                                    CheckBox checkBox = new CheckBox();
                                    checkBox.Dock = DockStyle.Fill;
                                    checkBox.Font = new Font("微软雅黑", 10);
                                    checkBox.Text = timeInterval;
                                    checkBox.CheckState = checkState;
                                    tlpMorning.Controls.Add(checkBox, 0, 1);
                                }
                                else if (r == 2 && c == 0)
                                {
                                    //第二行第一列
                                    //需跨1行
                                    CheckBox checkBox = new CheckBox();
                                    checkBox.Dock = DockStyle.Fill;
                                    checkBox.Font = new Font("微软雅黑", 10);
                                    checkBox.ForeColor = Color.FromArgb(255, 153, 102);
                                    checkBox.Text = "自动排班";
                                    if (tlpMorning.Enabled)
                                    {
                                        checkBox.CheckState = cbAuto.CheckState;
                                    }
                                    tlpMorning.SetRowSpan(checkBox, 2);
                                    tlpMorning.Controls.Add(checkBox, c, r);
                                }
                                else if (c == 0)
                                {
                                    //不做处理
                                }
                                else
                                {
                                    if (c == 1)
                                    {
                                        //第二列
                                        TimeEdit timeEdit = new TimeEdit();
                                        timeEdit.Properties.AutoHeight = false;
                                        timeEdit.Dock = DockStyle.Fill;
                                        timeEdit.Font = new Font("微软雅黑", 10);
                                        timeEdit.Properties.DisplayFormat.FormatString = "HH:mm";
                                        timeEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                        timeEdit.Properties.EditFormat.FormatString = "HH:mm";
                                        timeEdit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                        timeEdit.Properties.EditMask = "HH:mm";
                                        timeEdit.Properties.Mask.EditMask = "HH:mm";
                                        timeEdit.EditValue = start;
                                        timeEdit.Enabled = false;
                                        tlpMorning.Controls.Add(timeEdit, c, r);
                                        //TextEdit textEdit = new TextEdit();
                                        //textEdit.Properties.AutoHeight = false;
                                        //textEdit.Dock = DockStyle.Fill;
                                        //textEdit.Font = new Font("微软雅黑", 10);
                                        //textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                        //textEdit.Text = start;
                                        //textEdit.Enabled = teEnabled;
                                        //tlpMorning.Controls.Add(textEdit, c, r);
                                    }
                                    else if (c == 2)
                                    {
                                        //第三列
                                        TimeEdit timeEdit = new TimeEdit();
                                        timeEdit.Properties.AutoHeight = false;
                                        timeEdit.Dock = DockStyle.Fill;
                                        timeEdit.Font = new Font("微软雅黑", 10);
                                        timeEdit.Properties.DisplayFormat.FormatString = "HH:mm";
                                        timeEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                        timeEdit.Properties.EditFormat.FormatString = "HH:mm";
                                        timeEdit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                        timeEdit.Properties.EditMask = "HH:mm";
                                        timeEdit.Properties.Mask.EditMask = "HH:mm";
                                        timeEdit.EditValue = end;
                                        timeEdit.Enabled = false;
                                        tlpMorning.Controls.Add(timeEdit, c, r);
                                        //TextEdit textEdit = new TextEdit();
                                        //textEdit.Properties.AutoHeight = false;
                                        //textEdit.Dock = DockStyle.Fill;
                                        //textEdit.Font = new Font("微软雅黑", 10);
                                        //textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                        //textEdit.Text = end;
                                        //textEdit.Enabled = teEnabled;
                                        //tlpMorning.Controls.Add(textEdit, c, r);
                                    }
                                    else if (c == 3)
                                    {
                                        //第四列 现场
                                        TextEdit textEdit = new TextEdit();
                                        textEdit.Properties.AutoHeight = false;
                                        textEdit.Dock = DockStyle.Fill;
                                        textEdit.Font = new Font("微软雅黑", 10);
                                        textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                        textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                        textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                                        textEdit.Properties.Mask.EditMask = "[0-9]*";
                                        textEdit.Text = scene;
                                        textEdit.Enabled = teEnabled;
                                        tlpMorning.Controls.Add(textEdit, c, r);
                                    }
                                    else if (c == 4)
                                    {
                                        //第五列 预约
                                        TextEdit textEdit = new TextEdit();
                                        textEdit.Properties.AutoHeight = false;
                                        textEdit.Dock = DockStyle.Fill;
                                        textEdit.Font = new Font("微软雅黑", 10);
                                        textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                        textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                        textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                                        textEdit.Properties.Mask.EditMask = "[0-9]*";
                                        textEdit.Text = open;
                                        textEdit.Enabled = teEnabled;
                                        tlpMorning.Controls.Add(textEdit, c, r);
                                    }
                                    else if (c == 5)
                                    {
                                        //第六列 诊间
                                        TextEdit textEdit = new TextEdit();
                                        textEdit.Properties.AutoHeight = false;
                                        textEdit.Dock = DockStyle.Fill;
                                        textEdit.Font = new Font("微软雅黑", 10);
                                        textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                        textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                        textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                                        textEdit.Properties.Mask.EditMask = "[0-9]*";
                                        textEdit.Text = room;
                                        textEdit.Enabled = teEnabled;
                                        tlpMorning.Controls.Add(textEdit, c, r);
                                    }
                                    else if (c == 6)
                                    {
                                        //第七列 应急
                                        TextEdit textEdit = new TextEdit();
                                        textEdit.Properties.AutoHeight = false;
                                        textEdit.Dock = DockStyle.Fill;
                                        textEdit.Font = new Font("微软雅黑", 10);
                                        textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                        textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                        textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                                        textEdit.Properties.Mask.EditMask = "[0-9]*";
                                        textEdit.Text = emergency;
                                        textEdit.Enabled = teEnabled;
                                        textEdit.Visible = false;
                                        tlpMorning.Controls.Add(textEdit, c, r);
                                    }
                                }
                            }
                        }
                        tlpMorning.Location = new System.Drawing.Point(0, blpY);
                        blpY += tlpMorning.Height;
                        if (i == 1) tabPage1.Controls.Add(tlpMorning);
                        if (i == 2) tabPage2.Controls.Add(tlpMorning);
                        if (i == 3) tabPage3.Controls.Add(tlpMorning);
                        if (i == 4) tabPage4.Controls.Add(tlpMorning);
                        if (i == 5) tabPage5.Controls.Add(tlpMorning);
                        if (i == 6) tabPage6.Controls.Add(tlpMorning);
                        if (i == 7) tabPage7.Controls.Add(tlpMorning);
                    }
                }
                #endregion
                cmd.HideOpaqueLayer();
            });
        }

        private void setWorkingDay(List<WorkingDayEntity> workingDayList)
        {
            //清除排班数据
            pbDispose();

            #region 生成周一到周日的排班
            //生成周一到周日的排班
            for (int i = 1; i <= 7; i++)
            {
                int blpY = 20; //TableLayoutPanel的y轴位置
                String week = "";
                switch (i)
                {
                    case 1:
                        week = "一";
                        break;
                    case 2:
                        week = "二";
                        break;
                    case 3:
                        week = "三";
                        break;
                    case 4:
                        week = "四";
                        break;
                    case 5:
                        week = "五";
                        break;
                    case 6:
                        week = "六";
                        break;
                    case 7:
                        week = "日";
                        break;
                    default: break;
                }
                for (int j = 0; j < 4; j++)
                {
                    //当前TableLayoutPanel的数量
                    List<WorkingDayEntity> wdwpList = getWorkingDayData(workingDayList, week, j.ToString());
                    //多少行数据
                    int rowNum = wdwpList.Count;
                    //j=0:上午 j=1:下午 j=2:晚上 j=3:全天
                    TableLayoutPanel tlpMorning = new TableLayoutPanel();
                    if (rowNum == 0) tlpMorning.Enabled = false;
                    int row = 0;//行数(包括标题)
                    String timeInterval = ""; //
                    if (rowNum > 3) row = rowNum + 1;
                    else row = 4;
                    CheckState checkState = CheckState.Unchecked;
                    CheckState checkAuto = CheckState.Unchecked;
                    if (rowNum==0)
                        checkState = CheckState.Unchecked;
                    else if(wdwpList[0].isUse.Equals("0"))
                        checkState = CheckState.Checked;
                    else
                        checkState = CheckState.Unchecked;
                    if (rowNum == 0)
                        checkAuto = CheckState.Unchecked;
                    else if(wdwpList[0].autoSchedule.Equals("0"))
                        checkAuto = CheckState.Checked;
                    else
                        checkAuto = CheckState.Unchecked;
                    if (j == 0)
                    {
                        timeInterval = "上午";
                    }
                    else if (j == 1)
                    {
                        timeInterval = "下午";
                    }
                    else if (j == 2)
                    {
                        timeInterval = "晚上";
                    }
                    else if (j == 3)
                    {
                        timeInterval = "全天";
                    }

                    tlpMorning.ColumnCount = 7;
                    tlpMorning.RowCount = row;

                    tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
                    tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
                    tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
                    tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
                    tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
                    tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
                    tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));

                    for (int n = 0; n < row; n++)
                    {
                        tlpMorning.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
                    }
                    tlpMorning.Size = new System.Drawing.Size(350, row * 30);
                    //标题栏
                    Label label = new Label();
                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.BottomCenter;
                    label.Font = new Font("微软雅黑", 10);
                    label.Text = "开始";
                    tlpMorning.Controls.Add(label, 1, 0);
                    label = new Label();
                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.BottomCenter;
                    label.Font = new Font("微软雅黑", 10);
                    label.Text = "结束";
                    tlpMorning.Controls.Add(label, 2, 0);
                    label = new Label();
                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.BottomCenter;
                    label.Font = new Font("微软雅黑", 10);
                    label.Text = "现场";
                    tlpMorning.Controls.Add(label, 3, 0);
                    label = new Label();
                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.BottomCenter;
                    label.Font = new Font("微软雅黑", 10);
                    label.Text = "预约";
                    tlpMorning.Controls.Add(label, 4, 0);
                    label = new Label();
                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.BottomCenter;
                    label.Font = new Font("微软雅黑", 10);
                    label.Text = "诊间";
                    tlpMorning.Controls.Add(label, 5, 0);
                    label = new Label();
                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.BottomCenter;
                    label.Font = new Font("微软雅黑", 10);
                    label.Text = "应急";
                    label.Visible = false;
                    tlpMorning.Controls.Add(label, 6, 0);

                    bool teEnabled = true;//当行数小于3的时候，空白文本框需要设为不可选
                    String start = "";
                    String end = "";
                    String scene = "";
                    String open = "";
                    String room = "";
                    String emergency = "";
                    
                    for (int r = 1; r < row; r++)
                    {
                        if (r > rowNum)
                        {
                            start = "";
                            end = "";
                            scene = "";
                            open = "";
                            room = "";
                            emergency = "";
                            teEnabled = false;
                        }
                        else
                        {
                            start = wdwpList[r - 1].beginTime;
                            end = wdwpList[r - 1].endTime;
                            scene = wdwpList[r - 1].numSite;
                            open = wdwpList[r - 1].numOpen;
                            room = wdwpList[r - 1].numClinic;
                            emergency = wdwpList[r - 1].numYj;
                            teEnabled = true;
                        }
                        for (int c = 0; c < 7; c++)
                        {
                            if (r == 1 && c == 0)
                            {
                                //第一行第一列
                                CheckBox checkBox = new CheckBox();
                                checkBox.Dock = DockStyle.Fill;
                                checkBox.Font = new Font("微软雅黑", 10);
                                checkBox.Text = timeInterval;
                                checkBox.CheckState = checkState;
                                tlpMorning.Controls.Add(checkBox, 0, 1);
                            }
                            else if (r == 2 && c == 0)
                            {
                                //第二行第一列
                                //需跨1行
                                CheckBox checkBox = new CheckBox();
                                checkBox.Dock = DockStyle.Fill;
                                checkBox.Font = new Font("微软雅黑", 10);
                                checkBox.ForeColor = Color.FromArgb(255, 153, 102);
                                checkBox.Text = "自动排班";
                                checkBox.CheckState = checkAuto;
                                tlpMorning.SetRowSpan(checkBox, 2);
                                tlpMorning.Controls.Add(checkBox, c, r);
                            }
                            else if (c == 0)
                            {
                                //不做处理
                            }
                            else
                            {
                                if (c == 1)
                                {
                                    //第二列
                                    TimeEdit timeEdit = new TimeEdit();
                                    timeEdit.Properties.AutoHeight = false;
                                    timeEdit.Dock = DockStyle.Fill;
                                    timeEdit.Font = new Font("微软雅黑", 10);
                                    timeEdit.Properties.DisplayFormat.FormatString = "HH:mm";
                                    timeEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                    timeEdit.Properties.EditFormat.FormatString = "HH:mm";
                                    timeEdit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                    timeEdit.Properties.EditMask = "HH:mm";
                                    timeEdit.Properties.Mask.EditMask = "HH:mm";
                                    timeEdit.EditValue = start;
                                    timeEdit.Enabled = false;
                                    tlpMorning.Controls.Add(timeEdit, c, r);
                                    //TextEdit textEdit = new TextEdit();
                                    //textEdit.Properties.AutoHeight = false;
                                    //textEdit.Dock = DockStyle.Fill;
                                    //textEdit.Font = new Font("微软雅黑", 10);
                                    //textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                    //textEdit.Text = start;
                                    //textEdit.Enabled = teEnabled;
                                    //tlpMorning.Controls.Add(textEdit, c, r);
                                }
                                else if (c == 2)
                                {
                                    //第三列
                                    TimeEdit timeEdit = new TimeEdit();
                                    timeEdit.Properties.AutoHeight = false;
                                    timeEdit.Dock = DockStyle.Fill;
                                    timeEdit.Font = new Font("微软雅黑", 10);
                                    timeEdit.Properties.DisplayFormat.FormatString = "HH:mm";
                                    timeEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                    timeEdit.Properties.EditFormat.FormatString = "HH:mm";
                                    timeEdit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                    timeEdit.Properties.EditMask = "HH:mm";
                                    timeEdit.Properties.Mask.EditMask = "HH:mm";
                                    timeEdit.EditValue = end;
                                    timeEdit.Enabled = false;
                                    tlpMorning.Controls.Add(timeEdit, c, r);
                                    //TextEdit textEdit = new TextEdit();
                                    //textEdit.Properties.AutoHeight = false;
                                    //textEdit.Dock = DockStyle.Fill;
                                    //textEdit.Font = new Font("微软雅黑", 10);
                                    //textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                    //textEdit.Text = end;
                                    //textEdit.Enabled = teEnabled;
                                    //tlpMorning.Controls.Add(textEdit, c, r);
                                }
                                else if (c == 3)
                                {
                                    //第四列 现场
                                    TextEdit textEdit = new TextEdit();
                                    textEdit.Properties.AutoHeight = false;
                                    textEdit.Dock = DockStyle.Fill;
                                    textEdit.Font = new Font("微软雅黑", 10);
                                    textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                    textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                    textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                                    textEdit.Properties.Mask.EditMask = "[0-9]*";
                                    textEdit.Text = scene;
                                    textEdit.Enabled = teEnabled;
                                    tlpMorning.Controls.Add(textEdit, c, r);
                                }
                                else if (c == 4)
                                {
                                    //第五列 预约
                                    TextEdit textEdit = new TextEdit();
                                    textEdit.Properties.AutoHeight = false;
                                    textEdit.Dock = DockStyle.Fill;
                                    textEdit.Font = new Font("微软雅黑", 10);
                                    textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                    textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                    textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                                    textEdit.Properties.Mask.EditMask = "[0-9]*";
                                    textEdit.Text = open;
                                    textEdit.Enabled = teEnabled;
                                    tlpMorning.Controls.Add(textEdit, c, r);
                                }
                                else if (c == 5)
                                {
                                    //第六列 诊间
                                    TextEdit textEdit = new TextEdit();
                                    textEdit.Properties.AutoHeight = false;
                                    textEdit.Dock = DockStyle.Fill;
                                    textEdit.Font = new Font("微软雅黑", 10);
                                    textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                    textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                    textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                                    textEdit.Properties.Mask.EditMask = "[0-9]*";
                                    textEdit.Text = room;
                                    textEdit.Enabled = teEnabled;
                                    tlpMorning.Controls.Add(textEdit, c, r);
                                }
                                else if (c == 6)
                                {
                                    //第七列 应急
                                    TextEdit textEdit = new TextEdit();
                                    textEdit.Properties.AutoHeight = false;
                                    textEdit.Dock = DockStyle.Fill;
                                    textEdit.Font = new Font("微软雅黑", 10);
                                    textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                    textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                    textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                                    textEdit.Properties.Mask.EditMask = "[0-9]*";
                                    textEdit.Text = emergency;
                                    textEdit.Enabled = teEnabled;
                                    textEdit.Visible = false;
                                    tlpMorning.Controls.Add(textEdit, c, r);
                                }
                            }
                        }
                    }
                    tlpMorning.Location = new System.Drawing.Point(0, blpY);
                    blpY += tlpMorning.Height;
                    if (i == 1) tabPage1.Controls.Add(tlpMorning);
                    if (i == 2) tabPage2.Controls.Add(tlpMorning);
                    if (i == 3) tabPage3.Controls.Add(tlpMorning);
                    if (i == 4) tabPage4.Controls.Add(tlpMorning);
                    if (i == 5) tabPage5.Controls.Add(tlpMorning);
                    if (i == 6) tabPage6.Controls.Add(tlpMorning);
                    if (i == 7) tabPage7.Controls.Add(tlpMorning);
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取周几上午||下午||晚上的排班数据
        /// </summary>
        /// <param name="workingDayList">排班数据</param>
        /// <param name="week">周几</param>
        /// <param name="period">0：上午，1：下午，2：晚上</param>
        /// <returns></returns>
        private List<WorkingDayEntity> getWorkingDayData(List<WorkingDayEntity> workingDayList, String week, String period)
        {
            List<WorkingDayEntity> workingDayByWeekList = new List<WorkingDayEntity>();
            foreach (WorkingDayEntity workingDay in workingDayList)
            {
                if (workingDay.week.Equals(week)&&workingDay.period.Equals(period))
                    workingDayByWeekList.Add(workingDay);
            }
            return workingDayByWeekList;
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
                    cmd.HideOpaqueLayer();
                    if (ex.InnerException != null)
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

        private void DoctorSettingsForm_Resize(object sender, EventArgs e)
        {
            float tlpWidth = this.Width * 30 / 100;
            if (tlpWidth < 400)
                tableLayoutPanel1.ColumnStyles[1].Width = tlpWidth;
            else
                tableLayoutPanel1.ColumnStyles[1].Width = 400;
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }

        private void pbPicture_Click(object sender, EventArgs e)
        {
            PictureViewer pv = new PictureViewer(pbPicture.Image);
            pv.Show();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            cmd.ShowOpaqueLayer();
            SearchData(1, pageControl1.PageSize);
        }

        private void cbAuto_CheckedChanged(object sender, EventArgs e)
        {
            int days = tabControl1.Controls.Count; //排班天数
            if (days > 0)
            {
                for (int i = 0; i < days; i++)
                {
                    TabPage tabPage = (TabPage)tabControl1.Controls[i];//周几的面板
                    for (int period = 0; period < 4; period++)//循环上午、下午、晚上、全天
                    {
                        if (tabPage.Controls.Count == 0) break;
                        TableLayoutPanel tlp = (TableLayoutPanel)tabPage.Controls[period];//排班
                        if (tlp.Enabled)
                        {
                            CheckBox cbAutoSd = (CheckBox)tlp.GetControlFromPosition(0, 2);//自动更新
                            cbAutoSd.CheckState = cbAuto.CheckState;
                        }
                    }
                }
            }
        }

        private void btn_tkz_Click(object sender, EventArgs e)
        {
            List<DoctorInfoEntity> doctorList = bandedGridView1.DataSource as List<DoctorInfoEntity>;
            String ids = "";
            foreach (DoctorInfoEntity selectedRow in doctorList)
            {
                if (selectedRow.check)
                {
                    ids += selectedRow.id + ",";
                }
            }

            if (ids.Length == 0) return;
            ids = ids.Substring(0, ids.Length - 1);
                
            cmd.ShowOpaqueLayer();
            String url = AppContext.AppConfig.serverUrl + "cms/doctor/isUse?doctorIds=" + ids;
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    dcDoctorInfo.ClearValue();
                    pbPicture.Image = null;
                    pbPicture.Refresh();
                    pictureServiceFilePath = null;
                    pictureFilePath = "";
                    //清除排班数据
                    dcDefaultVisit.ClearValue();
                    tableLayoutPanel4.Enabled = false;
                    pbDispose();
                    SearchData(1, pageControl1.PageSize);
                    MessageBoxUtils.Hint(objT["message"].ToString(), MainForm);
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void bandedGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            BandedGridHitInfo hit = bandedGridView1.CalcHitInfo(e.Location);

            if (!hit.InBandPanel)
            {
                return;
            }

            String caption = hit.Band.Caption.Trim();

            if (!caption.Equals("☐") && !caption.Equals("☑"))
            {
                return;
            }

            GridBand gb1 = hit.Band;
            if (gb1.Children.VisibleBandCount > 0) //是否有下级Band
            {
                return;
            }
            for (int i = 0; i < bandedGridView1.RowCount; i++)
            {
                Boolean check = (Boolean)this.bandedGridView1.GetRowCellValue(i, bandedGridView1.Columns["check"]);
                if (caption.Equals("☐"))
                {
                    if (!check)
                    {
                        this.bandedGridView1.SetRowCellValue(i, bandedGridView1.Columns["check"], true);
                    }
                }
                else
                {
                    if (check)
                    {
                        this.bandedGridView1.SetRowCellValue(i, bandedGridView1.Columns["check"], false);
                    }
                }
            }
            if (caption.Equals("☐"))
            {
                hit.Band.Caption = " ☑";
            }
            else
            {
                hit.Band.Caption = " ☐";
            }
        }

        private void buttonControl2_Click_1(object sender, EventArgs e)
        {
            List<DoctorInfoEntity> doctorList = bandedGridView1.DataSource as List<DoctorInfoEntity>;
            String ids = "";
            foreach (DoctorInfoEntity selectedRow in doctorList)
            {
                if (selectedRow.check)
                {
                    ids += selectedRow.id + ",";
                }
            }

            if (ids.Length == 0) return;
            ids = ids.Substring(0, ids.Length - 1);

            cmd.ShowOpaqueLayer();
            String url = AppContext.AppConfig.serverUrl + "cms/doctor/isOpen?doctorIds=" + ids;
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    dcDoctorInfo.ClearValue();
                    pbPicture.Image = null;
                    pbPicture.Refresh();
                    pictureServiceFilePath = null;
                    pictureFilePath = "";
                    //清除排班数据
                    dcDefaultVisit.ClearValue();
                    tableLayoutPanel4.Enabled = false;
                    pbDispose();
                    SearchData(1, pageControl1.PageSize);
                    MessageBoxUtils.Hint(objT["message"].ToString(), MainForm);
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void timeEdit1_EditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, "0", true);
        }

        private void timeEdit2_EditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, "0", false);
        }

        private void timeEdit3_EditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, "1", true);
        }

        private void timeEdit4_EditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, "1", false);
        }

        private void timeEdit5_EditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, "2", true);
        }

        private void timeEdit6_EditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, "2", false);
        }

        /// <summary>
        /// 限制时间
        /// </summary>
        /// <param name="sender">时间控件</param>
        /// <param name="wb">午别 0:上午 1：下午 2：晚上</param>
        /// <param name="flag">true：开始时间 false：结束时间</param>
        private void ifTime(object sender, String wb, Boolean flag)
        {
            TimeEdit te = (TimeEdit)sender;
            if (te.Text.Length > 0)
            {
                if (wb.Equals("0"))
                {
                    if (te.Text.CompareTo(defaultVisitTemplate.mStart) == -1)
                    {
                        te.EditValue = defaultVisitTemplate.mStart;
                    }
                    if (te.Text.CompareTo(defaultVisitTemplate.mEnd) == 1)
                    {
                        te.EditValue = defaultVisitTemplate.mEnd;
                    }
                }
                else if (wb.Equals("1"))
                {
                    if (te.Text.CompareTo(defaultVisitTemplate.aStart) == -1)
                    {
                        te.EditValue = defaultVisitTemplate.aStart;
                    }
                    if (te.Text.CompareTo(defaultVisitTemplate.aEnd) == 1)
                    {
                        te.EditValue = defaultVisitTemplate.aEnd;
                    }
                }
                else if (wb.Equals("2"))
                {
                    String text = "";
                    if (te.Text.CompareTo("12:00") == -1)
                    {
                        text = "2019/07/30 " + te.Text;
                    }else{
                        text = "2019/07/29 " + te.Text;
                    }
                        
                    String nStart = "2019/07/29 " + defaultVisitTemplate.nStart;
                    String nEnd = "2019/07/30 " + defaultVisitTemplate.nEnd;
                    if (text.CompareTo(nStart) == -1)
                    {
                        te.EditValue = defaultVisitTemplate.nStart;
                    }
                    if (text.CompareTo(nEnd) == 1)
                    {
                        te.EditValue = defaultVisitTemplate.nEnd;
                    }
                }
            }
        }

        private void bandedGridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.Caption == "排序号")
            {

            }
            else
            {
                //本来保存成功后清除一次就行了，不过为了预防新增或修改事件触发的过程中出现异常
                //导致面板有数据却无法编辑,导致新增的时候有默认数据，所以这里需要再清一次
                //清除医生数据
                dcDoctorInfo.ClearValue();
                pbPicture.Image = null;
                pbPicture.Refresh();
                pictureServiceFilePath = null;
                pictureFilePath = "";
                //清除排班数据
                dcDefaultVisit.ClearValue();
                tableLayoutPanel4.Enabled = false;
                pbDispose();

                doctorInfo = new DoctorInfoEntity();
                var selectedRow = bandedGridView1.GetFocusedRow() as DoctorInfoEntity;
                if (selectedRow == null)
                    return;
                cmd.ShowOpaqueLayer();
                String url = AppContext.AppConfig.serverUrl + "cms/doctor/findById?id=" + selectedRow.id;
                this.DoWorkAsync(250, (o) =>
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (data) =>
                {
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        doctorInfo = objT["result"].ToObject<DoctorInfoEntity>();
                        doctorInfo.hospitalId = doctorInfo.dept.hospital.id;
                        doctorInfo.deptId = doctorInfo.dept.id;
                        if (doctorInfo.ignoreHoliday.Equals("1"))
                            cbIgnoreHoliday.CheckState = CheckState.Checked;
                        if (doctorInfo.ignoreYear.Equals("1"))
                            cbIgnoreYear.CheckState = CheckState.Checked;
                        dcDoctorInfo.SetValue(doctorInfo);
                        //显示图片
                        pictureServiceFilePath = doctorInfo.pictureUrl;
                        //医院的机子有些请求不到路径的时候，会卡的1分钟左右，所以下载图片用异步
                        this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                        {
                            if (pictureServiceFilePath != null && pictureServiceFilePath.Length > 0)
                            {
                                try
                                {
                                    WebClient web = new WebClient();
                                    var bytes = web.DownloadData(AppContext.AppConfig.serverIp + pictureServiceFilePath);
                                    return bytes;

                                }
                                catch (Exception ex)
                                {
                                    Xr.Log4net.LogHelper.Error(ex.Message);
                                }
                            }
                            return null;
                        }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                        {
                            if (data2 != null)
                                this.pbPicture.Image = Bitmap.FromStream(new MemoryStream(data2 as byte[]));
                        });

                        groupBox1.Enabled = true;
                        setDefaultVisit();
                        tableLayoutPanel4.Enabled = true;

                        //获取默认出诊设置
                        url = AppContext.AppConfig.serverUrl + "cms/doctor/findVisitingTime?deptId=" + doctorInfo.dept.id + "&doctorId=" + selectedRow.id + "&hospitalId=" + doctorInfo.hospitalId;
                        this.DoWorkAsync(0, (o) =>
                        {
                            data = HttpClass.httpPost(url);
                            return data;

                        }, null, (data2) =>
                        {
                            objT = JObject.Parse(data2.ToString());
                            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                            {
                                List<WorkingDayEntity> workingDayList = objT["result"].ToObject<List<WorkingDayEntity>>();
                                if (workingDayList.Count > 0)
                                {
                                    DefaultVisitEntity defaultVisitNew = new DefaultVisitEntity();
                                    foreach (WorkingDayEntity wd in workingDayList)
                                    {
                                        if (wd.period.Equals("0"))
                                        {
                                            defaultVisit.mStart = wd.beginTime;
                                            defaultVisit.mEnd = wd.endTime;
                                            defaultVisit.mSubsection = wd.segmentalDuration;
                                            defaultVisit.mScene = wd.numSite;
                                            defaultVisit.mOpen = wd.numOpen;
                                            defaultVisit.mRoom = wd.numClinic;
                                            defaultVisit.mEmergency = wd.numYj;
                                        }
                                        else if (wd.period.Equals("1"))
                                        {
                                            defaultVisit.aStart = wd.beginTime;
                                            defaultVisit.aEnd = wd.endTime;
                                            defaultVisit.aSubsection = wd.segmentalDuration;
                                            defaultVisit.aScene = wd.numSite;
                                            defaultVisit.aOpen = wd.numOpen;
                                            defaultVisit.aRoom = wd.numClinic;
                                            defaultVisit.aEmergency = wd.numYj;
                                        }
                                        else if (wd.period.Equals("2"))
                                        {
                                            defaultVisit.nStart = wd.beginTime;
                                            defaultVisit.nEnd = wd.endTime;
                                            defaultVisit.nSubsection = wd.segmentalDuration;
                                            defaultVisit.nScene = wd.numSite;
                                            defaultVisit.nOpen = wd.numOpen;
                                            defaultVisit.nRoom = wd.numClinic;
                                            defaultVisit.nEmergency = wd.numYj;
                                        }
                                        else if (wd.period.Equals("3"))
                                        {
                                            defaultVisit.allStart = wd.beginTime;
                                            defaultVisit.allEnd = wd.endTime;
                                            defaultVisit.allSubsection = wd.segmentalDuration;
                                            defaultVisit.allScene = wd.numSite;
                                            defaultVisit.allOpen = wd.numOpen;
                                            defaultVisit.allRoom = wd.numClinic;
                                            defaultVisit.allEmergency = wd.numYj;
                                        }
                                    }
                                    dcDefaultVisit.SetValue(defaultVisit);
                                }
                                else
                                {
                                    setDefaultVisit();
                                }

                                //获取已排班信息
                                url = AppContext.AppConfig.serverUrl + "cms/doctor/findDoctorVisitingList?deptId=" + doctorInfo.dept.id + "&doctorId=" + selectedRow.id;
                                this.DoWorkAsync(250, (o) =>
                                {
                                    data = HttpClass.httpPost(url);
                                    return data;

                                }, null, (data3) =>
                                {
                                    cmd.HideOpaqueLayer();
                                    objT = JObject.Parse(data3.ToString());
                                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                    {
                                        List<WorkingDayEntity> workingDayList2 = objT["result"].ToObject<List<WorkingDayEntity>>();
                                        setWorkingDay(workingDayList2);
                                        cmd.HideOpaqueLayer();
                                    }
                                    else
                                    {
                                        cmd.HideOpaqueLayer();
                                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                                    }
                                });
                            }
                            else
                            {
                                cmd.HideOpaqueLayer();
                                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                            }
                        });
                    }
                    else
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }
                    gcDoctor.Focus();
                });
            }
        }

        private void btnSaveSort_Click(object sender, EventArgs e)
        {
            List<DoctorInfoEntity> doctorList = bandedGridView1.DataSource as List<DoctorInfoEntity>;
            String ids = "";
            String sorts = "";
            foreach (DoctorInfoEntity selectedRow in doctorList)
            {
                ids += selectedRow.id + ",";
                sorts += selectedRow.sort + ",";
            }

            if (ids.Length == 0) return;
            ids = ids.Substring(0, ids.Length - 1);
            if (sorts.Length == 0) return;
            sorts = sorts.Substring(0, sorts.Length - 1);

            cmd.ShowOpaqueLayer();
            String url = AppContext.AppConfig.serverUrl + "cms/doctor/saveSort?doctorIds=" + ids + "&sorts="+sorts;
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    dcDoctorInfo.ClearValue();
                    pbPicture.Image = null;
                    pbPicture.Refresh();
                    pictureServiceFilePath = null;
                    pictureFilePath = "";
                    //清除排班数据
                    dcDefaultVisit.ClearValue();
                    tableLayoutPanel4.Enabled = false;
                    pbDispose();
                    SearchData(1, pageControl1.PageSize);
                    MessageBoxUtils.Hint(objT["message"].ToString(), MainForm);
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void buttonControl2_Click_2(object sender, EventArgs e)
        {
            for (int i = 0; i < bandedGridView1.RowCount; i++)
            {
                Boolean check = (Boolean)this.bandedGridView1.GetRowCellValue(i, bandedGridView1.Columns["check"]);
                if (buttonControl2.Tag.Equals("true"))
                {
                    if (!check)
                    {
                        this.bandedGridView1.SetRowCellValue(i, bandedGridView1.Columns["check"], true);
                    }
                }
                else
                {
                    if (check)
                    {
                        this.bandedGridView1.SetRowCellValue(i, bandedGridView1.Columns["check"], false);
                    }
                }
            }
            if (buttonControl2.Tag.Equals("true"))
                buttonControl2.Tag = "false";
            else
                buttonControl2.Tag = "true";
        }

    }
}
