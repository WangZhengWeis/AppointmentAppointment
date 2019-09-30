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

namespace Xr.RtManager.Pages.scheduling
{
    public partial class BatchSchedulingForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;

        public BatchSchedulingForm()
        {
            InitializeComponent();
        }

        private void BatchSchedulingForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(0f);
            //设置科室列表
            String url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept";
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DeptEntity> deptList = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                    //List<DeptEntity> deptList = AppContext.Session.deptList;
                    treeMenuControl1.KeyFieldName = "id";
                    treeMenuControl1.ParentFieldName = "parentId";
                    treeMenuControl1.DisplayMember = "name";
                    treeMenuControl1.ValueMember = "id";
                    treeMenuControl1.DataSource = deptList;
                    if (deptList.Count > 0)
                        treeMenuControl1.EditValue = deptList[0].id;

                    //查询日期下拉框数据
                    url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/findWeeks";
                    this.DoWorkAsync( 0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        data = HttpClass.httpPost(url);
                        return data;

                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        objT = JObject.Parse(data2.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            List<DictEntity> dictList = new List<DictEntity>();
                            for (int i = 0; i < objT["result"].Count(); i++)
                            {
                                DictEntity dict = new DictEntity();
                                dict.value = i + "";
                                dict.label = objT["result"][i].ToString();
                                dictList.Add(dict);
                            }
                            lueDate.Properties.DataSource = dictList;
                            lueDate.Properties.DisplayMember = "label";
                            lueDate.Properties.ValueMember = "value";
                            lueDate.EditValue = "0";

                            DateTime dt = DateTime.Parse(lueDate.Text + " 00:00:00");
                            monday.Caption = dt.ToString("u").Substring(5, 5) + "(一)";
                            dt = dt.AddDays(1);
                            tuesday.Caption = dt.ToString("u").Substring(5, 5) + "(二)";
                            dt = dt.AddDays(1);
                            wednesday.Caption = dt.ToString("u").Substring(5, 5) + "(三)";
                            dt = dt.AddDays(1);
                            thursday.Caption = dt.ToString("u").Substring(5, 5) + "(四)";
                            dt = dt.AddDays(1);
                            friday.Caption = dt.ToString("u").Substring(5, 5) + "(五)";
                            dt = dt.AddDays(1);
                            saturday.Caption = dt.ToString("u").Substring(5, 5) + "(六)";
                            dt = dt.AddDays(1);
                            sunday.Caption = dt.ToString("u").Substring(5, 5) + "(日)";

                            columShowHide("", "", "", "");

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
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });
        }

        private void btnTswk_Click(object sender, EventArgs e)
        {
            int i = int.Parse(lueDate.EditValue.ToString());
            List<DictEntity> dictList = lueDate.Properties.DataSource as List<DictEntity>;
            if (i > 0 && i <= dictList.Count()-1)
            {
                i--;
                lueDate.EditValue = i.ToString();
            }
        }

        private void btnNxvWk_Click(object sender, EventArgs e)
        {
            int i = int.Parse(lueDate.EditValue.ToString());
            List<DictEntity> dictList = lueDate.Properties.DataSource as List<DictEntity>;
            if (i >= 0 && i < dictList.Count() - 1)
            {
                i++;
                lueDate.EditValue = i.ToString();
            }
        }

        private void lueDate_EditValueChanged(object sender, EventArgs e)
        {
            SearchData();
        }

        private void treeMenuControl1_MenuItemClick(object sender, EventArgs e, object selectItem)
        {
            SearchData();
        }

        public void SearchData()
        {
            if (treeMenuControl1.EditValue != null && lueDate.Text.Length > 0)
            {
                //检查是否是节假日
                List<String> holidayList = new List<String>();
                DateTime dtEnd = DateTime.Parse(lueDate.Text + " 00:00:00").AddDays(6);
                String param = "hospitalId=" + AppContext.Session.hospitalId + "&beginDate=" + lueDate.Text + "&endDate=" + dtEnd.ToString("u").Substring(0, 10);
                String url = AppContext.AppConfig.serverUrl + "cms/holiday/checkHolidayForDate?" + param;
                cmd.ShowOpaqueLayer();
                this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        holidayList = objT["result"].ToObject<List<String>>();

                        //修改表格背景色
                        DateTime dt = DateTime.Parse(lueDate.Text + " 00:00:00");
                        monday.Caption = dt.ToString("u").Substring(5, 5) + "(一)";
                        if (ifListContainStr(holidayList, dt.ToString("u").Substring(0, 10)))
                        {
                            monday.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            mondayMorning.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            mondayAfternoon.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            mondayNight.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            mondayAllDay.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                        }
                        else
                        {
                            monday.AppearanceHeader.BackColor = SystemColors.Control;
                            mondayMorning.AppearanceHeader.BackColor = SystemColors.Control;
                            mondayAfternoon.AppearanceHeader.BackColor = SystemColors.Control;
                            mondayNight.AppearanceHeader.BackColor = SystemColors.Control;
                            mondayAllDay.AppearanceHeader.BackColor = SystemColors.Control;
                        }
                        dt = dt.AddDays(1);
                        tuesday.Caption = dt.ToString("u").Substring(5, 5) + "(二)";
                        if (ifListContainStr(holidayList, dt.ToString("u").Substring(0, 10)))
                        {
                            tuesday.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            tuesdayMorning.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            tuesdayAfternoon.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            tuesdayNight.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            tuesdayAllDay.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                        }
                        else
                        {
                            tuesday.AppearanceHeader.BackColor = SystemColors.Control;
                            tuesdayMorning.AppearanceHeader.BackColor = SystemColors.Control;
                            tuesdayAfternoon.AppearanceHeader.BackColor = SystemColors.Control;
                            tuesdayNight.AppearanceHeader.BackColor = SystemColors.Control;
                            tuesdayAllDay.AppearanceHeader.BackColor = SystemColors.Control;
                        }
                        dt = dt.AddDays(1);
                        wednesday.Caption = dt.ToString("u").Substring(5, 5) + "(三)";
                        if (ifListContainStr(holidayList, dt.ToString("u").Substring(0, 10)))
                        {
                            wednesday.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            wednesdayMorning.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            wednesdayAfternoon.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            wednesdayNight.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            wednesdayAllDay.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                        }
                        else
                        {
                            wednesday.AppearanceHeader.BackColor = SystemColors.Control;
                            wednesdayMorning.AppearanceHeader.BackColor = SystemColors.Control;
                            wednesdayAfternoon.AppearanceHeader.BackColor = SystemColors.Control;
                            wednesdayNight.AppearanceHeader.BackColor = SystemColors.Control;
                            wednesdayAllDay.AppearanceHeader.BackColor = SystemColors.Control;
                        }
                        dt = dt.AddDays(1);
                        thursday.Caption = dt.ToString("u").Substring(5, 5) + "(四)";
                        if (ifListContainStr(holidayList, dt.ToString("u").Substring(0, 10)))
                        {
                            thursday.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            thursdayMorning.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            thursdayAfternoon.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            thursdayNight.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            thursdayAllDay.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                        }
                        else
                        {
                            thursday.AppearanceHeader.BackColor = SystemColors.Control;
                            thursdayMorning.AppearanceHeader.BackColor = SystemColors.Control;
                            thursdayAfternoon.AppearanceHeader.BackColor = SystemColors.Control;
                            thursdayNight.AppearanceHeader.BackColor = SystemColors.Control;
                            thursdayAllDay.AppearanceHeader.BackColor = SystemColors.Control;
                        }
                        dt = dt.AddDays(1);
                        friday.Caption = dt.ToString("u").Substring(5, 5) + "(五)";
                        if (ifListContainStr(holidayList, dt.ToString("u").Substring(0, 10)))
                        {
                            friday.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            fridayMorning.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            fridayAfternoon.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            fridayNight.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            fridayAllDay.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                        }
                        else
                        {
                            friday.AppearanceHeader.BackColor = SystemColors.Control;
                            fridayMorning.AppearanceHeader.BackColor = SystemColors.Control;
                            fridayAfternoon.AppearanceHeader.BackColor = SystemColors.Control;
                            fridayNight.AppearanceHeader.BackColor = SystemColors.Control;
                            fridayAllDay.AppearanceHeader.BackColor = SystemColors.Control;
                        }
                        dt = dt.AddDays(1);
                        saturday.Caption = dt.ToString("u").Substring(5, 5) + "(六)";
                        if (ifListContainStr(holidayList, dt.ToString("u").Substring(0, 10)))
                        {
                            saturday.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            saturdayMorning.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            saturdayAfternoon.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            saturdayNight.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            saturdayAllDay.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                        }
                        else
                        {
                            saturday.AppearanceHeader.BackColor = SystemColors.Control;
                            saturdayMorning.AppearanceHeader.BackColor = SystemColors.Control;
                            saturdayAfternoon.AppearanceHeader.BackColor = SystemColors.Control;
                            saturdayNight.AppearanceHeader.BackColor = SystemColors.Control;
                            saturdayAllDay.AppearanceHeader.BackColor = SystemColors.Control;
                        }
                        dt = dt.AddDays(1);
                        sunday.Caption = dt.ToString("u").Substring(5, 5) + "(日)";
                        if (ifListContainStr(holidayList, dt.ToString("u").Substring(0, 10)))
                        {
                            sunday.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            sundayMorning.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            sundayAfternoon.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            sundayNight.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                            sundayAllDay.AppearanceHeader.BackColor = Color.FromArgb(255, 204, 255);
                        }
                        else
                        {
                            sunday.AppearanceHeader.BackColor = SystemColors.Control;
                            sundayMorning.AppearanceHeader.BackColor = SystemColors.Control;
                            sundayAfternoon.AppearanceHeader.BackColor = SystemColors.Control;
                            sundayNight.AppearanceHeader.BackColor = SystemColors.Control;
                            sundayAllDay.AppearanceHeader.BackColor = SystemColors.Control;
                        }

                        List<DoctorVSEntity> DVSList = new List<DoctorVSEntity>();//表格数据
                        List<SchedulingEntity> pbList = null;//后台返回的排班数据
                        //获取排班数据
                        param = "hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + treeMenuControl1.EditValue + "&date=" + lueDate.Text;
                        url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/findByDeptAndDate?" + param;
                        this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                        {
                            data = HttpClass.httpPost(url);
                            return data;

                        }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                        {
                            objT = JObject.Parse(data2.ToString());
                            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                            {
                                pbList = objT["result"].ToObject<List<SchedulingEntity>>();

                                //获取医生列表
                                //param = "pageNo=1&pageSize=10000&hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + treeMenuControl1.EditValue;
                                param = "hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + treeMenuControl1.EditValue;
                                url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?" + param;
                                this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                                {
                                    data = HttpClass.httpPost(url);
                                    return data;

                                }, null, (data3) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                                {
                                    objT = JObject.Parse(data3.ToString());
                                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                    {
                                       // List<DoctorInfoEntity> doctorList = objT["result"]["list"].ToObject<List<DoctorInfoEntity>>();
                                        List<DoctorInfoEntity> doctorList = objT["result"].ToObject<List<DoctorInfoEntity>>();
                                        if (doctorList.Count == 0) DVSList.Clear();
                                        foreach (DoctorInfoEntity doctor in doctorList)
                                        {
                                            DoctorVSEntity DVS = new DoctorVSEntity();
                                            DVS.doctorId = doctor.id;
                                            DVS.doctorName = doctor.name;
                                            dt = DateTime.Parse(lueDate.Text + " 00:00:00");
                                            for (int i = 0; i < 7; i++)
                                            {
                                                String date = dt.ToString("u").Substring(0, 10);
                                                for (int j = 0; j < 4; j++)
                                                {
                                                    SchedulingEntity scheduling = getSchedulingData(pbList, date, doctor.id, j.ToString());
                                                    String hideValue = "";
                                                    String value = "口";
                                                    if (scheduling != null)
                                                    {
                                                        if (scheduling.mzType.Equals("1"))
                                                        {
                                                            value = "√";
                                                        }
                                                        else
                                                        {
                                                            value = "特√";
                                                        }
                                                        hideValue = scheduling.doctorId;
                                                    }

                                                    if (i == 0 && j == 0)
                                                    {
                                                        DVS.mondayMorning = value;
                                                        DVS.mondayMorningValue = hideValue;
                                                    }
                                                    else if (i == 0 && j == 1)
                                                    {
                                                        DVS.mondayAfternoon = value;
                                                        DVS.mondayAfternoonValue = hideValue;
                                                    }
                                                    else if (i == 0 && j == 2)
                                                    {
                                                        DVS.mondayNight = value;
                                                        DVS.mondayNightValue = hideValue;
                                                    }
                                                    else if (i == 0 && j == 3)
                                                    {
                                                        DVS.mondayAllAay = value;
                                                        DVS.mondayAllAayValue = hideValue;
                                                    }
                                                    else if (i == 1 && j == 0)
                                                    {
                                                        DVS.tuesdayMorning = value;
                                                        DVS.tuesdayMorningValue = hideValue;
                                                    }
                                                    else if (i == 1 && j == 1)
                                                    {
                                                        DVS.tuesdayAfternoon = value;
                                                        DVS.tuesdayAfternoonValue = hideValue;
                                                    }
                                                    else if (i == 1 && j == 2)
                                                    {
                                                        DVS.tuesdayNight = value;
                                                        DVS.tuesdayNightValue = hideValue;
                                                    }
                                                    else if (i == 1 && j == 3)
                                                    {
                                                        DVS.tuesdayAllAay = value;
                                                        DVS.tuesdayAllAayValue = hideValue;
                                                    }
                                                    else if (i == 2 && j == 0)
                                                    {
                                                        DVS.wednesdayMorning = value;
                                                        DVS.wednesdayMorningValue = hideValue;
                                                    }
                                                    else if (i == 2 && j == 1)
                                                    {
                                                        DVS.wednesdayAfternoon = value;
                                                        DVS.wednesdayAfternoonValue = hideValue;
                                                    }
                                                    else if (i == 2 && j == 2)
                                                    {
                                                        DVS.wednesdayNight = value;
                                                        DVS.wednesdayNightValue = hideValue;
                                                    }
                                                    else if (i == 2 && j == 3)
                                                    {
                                                        DVS.wednesdayAllAay = value;
                                                        DVS.wednesdayAllAayValue = hideValue;
                                                    }
                                                    else if (i == 3 && j == 0)
                                                    {
                                                        DVS.thursdayMorning = value;
                                                        DVS.thursdayMorningValue = hideValue;
                                                    }
                                                    else if (i == 3 && j == 1)
                                                    {
                                                        DVS.thursdayAfternoon = value;
                                                        DVS.thursdayAfternoonValue = hideValue;
                                                    }
                                                    else if (i == 3 && j == 2)
                                                    {
                                                        DVS.thursdayNight = value;
                                                        DVS.thursdayNightValue = hideValue;
                                                    }
                                                    else if (i == 3 && j == 3)
                                                    {
                                                        DVS.thursdayAllAay = value;
                                                        DVS.thursdayAllAayValue = hideValue;
                                                    }
                                                    else if (i == 4 && j == 0)
                                                    {
                                                        DVS.fridayMorning = value;
                                                        DVS.fridayMorningValue = hideValue;
                                                    }
                                                    else if (i == 4 && j == 1)
                                                    {
                                                        DVS.fridayAfternoon = value;
                                                        DVS.fridayAfternoonValue = hideValue;
                                                    }
                                                    else if (i == 4 && j == 2)
                                                    {
                                                        DVS.fridayNight = value;
                                                        DVS.fridayNightValue = hideValue;
                                                    }
                                                    else if (i == 4 && j == 3)
                                                    {
                                                        DVS.fridayAllAay = value;
                                                        DVS.fridayAllAayValue = hideValue;
                                                    }
                                                    else if (i == 5 && j == 0)
                                                    {
                                                        DVS.saturdayMorning = value;
                                                        DVS.saturdayMorningValue = hideValue;
                                                    }
                                                    else if (i == 5 && j == 1)
                                                    {
                                                        DVS.saturdayAfternoon = value;
                                                        DVS.saturdayAfternoonValue = hideValue;
                                                    }
                                                    else if (i == 5 && j == 2)
                                                    {
                                                        DVS.saturdayNight = value;
                                                        DVS.saturdayNightValue = hideValue;
                                                    }
                                                    else if (i == 5 && j == 3)
                                                    {
                                                        DVS.saturdayAllAay = value;
                                                        DVS.saturdayAllAayValue = hideValue;
                                                    }
                                                    else if (i == 6 && j == 0)
                                                    {
                                                        DVS.sundayMorning = value;
                                                        DVS.sundayMorningValue = hideValue;
                                                    }
                                                    else if (i == 6 && j == 1)
                                                    {
                                                        DVS.sundayAfternoon = value;
                                                        DVS.sundayAfternoonValue = hideValue;
                                                    }
                                                    else if (i == 6 && j == 2)
                                                    {
                                                        DVS.sundayNight = value;
                                                        DVS.sundayNightValue = hideValue;
                                                    }
                                                    else if (i == 6 && j == 3)
                                                    {
                                                        DVS.sundayAllAay = value;
                                                        DVS.sundayAllAayValue = hideValue;
                                                    }
                                                }
                                                dt = dt.AddDays(1);
                                            }
                                            DVSList.Add(DVS);
                                        }
                                        gcDoctor.DataSource = DVSList;
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
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                });
            }
        }

        /// <summary>
        /// 判断List<String>是否含有某个字符串
        /// </summary>
        /// <param name="holidayList"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool ifListContainStr(List<String> holidayList, String dt)
        {
            foreach (String holiday in holidayList)
            {
                if(holiday.Equals(dt))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据医生id、日期、时段查询是否与该排班
        /// </summary>
        /// <param name="schedulingList"></param>
        /// <param name="date"></param>
        /// <param name="doctorId"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        private SchedulingEntity getSchedulingData(List<SchedulingEntity> schedulingList, String date, 
            String doctorId, String period)
        {
            foreach (SchedulingEntity scheduling in schedulingList)
            {
                if (scheduling.doctorId.Equals(doctorId) && scheduling.date.Equals(date) 
                    && scheduling.period.Equals(period))
                {
                    return scheduling;
                }
            }
            return null;
        }

        private void bandedGridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.CellValue == null) return;
            DateTime dt = DateTime.ParseExact(lueDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            String zj = e.Column.Caption.Substring(0,2);//周几
            if (zj.Equals("名称"))
            {
                return;
            }
            else if (zj.Equals("周一"))
            {
            }
            else if (zj.Equals("周二"))
            {
                dt = dt.AddDays(1);
            }
            else if (zj.Equals("周三"))
            {
                dt = dt.AddDays(2);
            }
            else if (zj.Equals("周四"))
            {
                dt = dt.AddDays(3);
            }
            else if (zj.Equals("周五"))
            {
                dt = dt.AddDays(4);
            }
            else if (zj.Equals("周六"))
            {
                dt = dt.AddDays(5);
            }
            else if (zj.Equals("周日"))
            {
                dt = dt.AddDays(6);
            }
            if (DateTime.Compare(dt, DateTime.Today) < 0) //判断日期大小
            {
                MessageBoxUtils.Hint("过去的日期不能排班", HintMessageBoxIcon.Error, MainForm);
                return;
            }

            String ts = "";
            if(e.Column.Caption.Equals("周一上午")||e.Column.Caption.Equals("周一下午")||e.Column.Caption.Equals("周一晚上")){
                string strName = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "mondayAllAay");
                if (strName.Equals("√") || strName.Equals("特√"))
                {
                    ts = "选中了全天就不能选择上午、下午、晚上";
                }
            }else if(e.Column.Caption.Equals("周二上午")||e.Column.Caption.Equals("周二下午")||e.Column.Caption.Equals("周二晚上")){
                string strName = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "tuesdayAllAay");
                if (strName.Equals("√") || strName.Equals("特√"))
                {
                    ts = "选中了全天就不能选择上午、下午、晚上";
                }
            }else if(e.Column.Caption.Equals("周三上午")||e.Column.Caption.Equals("周三下午")||e.Column.Caption.Equals("周三晚上")){
                string strName = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "wednesdayAllAay");
                if (strName.Equals("√") || strName.Equals("特√"))
                {
                    ts = "选中了全天就不能选择上午、下午、晚上";
                }
            }else if(e.Column.Caption.Equals("周四上午")||e.Column.Caption.Equals("周四下午")||e.Column.Caption.Equals("周四晚上")){
                string strName = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "thursdayAllAay");
                if (strName.Equals("√") || strName.Equals("特√"))
                {
                    ts = "选中了全天就不能选择上午、下午、晚上";
                }
            }else if(e.Column.Caption.Equals("周五上午")||e.Column.Caption.Equals("周五下午")||e.Column.Caption.Equals("周五晚上")){
                string strName = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "fridayAllAay");
                if (strName.Equals("√") || strName.Equals("特√"))
                {
                    ts = "选中了全天就不能选择上午、下午、晚上";
                }
            }else if(e.Column.Caption.Equals("周六上午")||e.Column.Caption.Equals("周六下午")||e.Column.Caption.Equals("周六晚上")){
                string strName = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "saturdayAllAay");
                if (strName.Equals("√") || strName.Equals("特√"))
                {
                    ts = "选中了全天就不能选择上午、下午、晚上";
                }
            }else if(e.Column.Caption.Equals("周日上午")||e.Column.Caption.Equals("周日下午")||e.Column.Caption.Equals("周日晚上")){
                string strName = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "sundayAllAay");
                if(strName.Equals("√") || strName.Equals("特√")){
                    ts = "选中了全天就不能选择上午、下午、晚上";
                }
            }else if(e.Column.Caption.Equals("周一全天")){
                string morning = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "mondayMorning");
                string afternoon = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "mondayAfternoon");
                string night = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "mondayNight");
                if (morning.Equals("√") || afternoon.Equals("√") || night.Equals("√")
                    || morning.Equals("特√") || afternoon.Equals("特√") || night.Equals("特√"))
                {
                    ts = "选中了上午、下午、晚上就不能选择全天";
                }
            }else if(e.Column.Caption.Equals("周二全天")){
                string morning = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "tuesdayMorning");
                string afternoon = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "tuesdayAfternoon");
                string night = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "tuesdayNight");
                if (morning.Equals("√") || afternoon.Equals("√") || night.Equals("√")
                    || morning.Equals("特√") || afternoon.Equals("特√") || night.Equals("特√"))
                {
                    ts = "选中了上午、下午、晚上就不能选择全天";
                }
            }else if(e.Column.Caption.Equals("周三全天")){
                string morning = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "wednesdayMorning");
                string afternoon = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "wednesdayAfternoon");
                string night = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "wednesdayNight");
                if (morning.Equals("√") || afternoon.Equals("√") || night.Equals("√")
                    || morning.Equals("特√") || afternoon.Equals("特√") || night.Equals("特√"))
                {
                    ts = "选中了上午、下午、晚上就不能选择全天";
                }
            }else if(e.Column.Caption.Equals("周四全天")){
                string morning = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "thursdayMorning");
                string afternoon = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "thursdayAfternoon");
                string night = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "thursdayNight");
                if (morning.Equals("√") || afternoon.Equals("√") || night.Equals("√")
                    || morning.Equals("特√") || afternoon.Equals("特√") || night.Equals("特√"))
                {
                    ts = "选中了上午、下午、晚上就不能选择全天";
                }
            }else if(e.Column.Caption.Equals("周五全天")){
                string morning = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "fridayMorning");
                string afternoon = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "fridayAfternoon");
                string night = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "fridayNight");
                if (morning.Equals("√") || afternoon.Equals("√") || night.Equals("√")
                    || morning.Equals("特√") || afternoon.Equals("特√") || night.Equals("特√"))
                {
                    ts = "选中了上午、下午、晚上就不能选择全天";
                }
            }else if(e.Column.Caption.Equals("周六全天")){
                string morning = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "saturdayMorning");
                string afternoon = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "saturdayAfternoon");
                string night = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "saturdayNight");
                if (morning.Equals("√") || afternoon.Equals("√") || night.Equals("√")
                    || morning.Equals("特√") || afternoon.Equals("特√") || night.Equals("特√"))
                {
                    ts = "选中了上午、下午、晚上就不能选择全天";
                }
            }else if(e.Column.Caption.Equals("周日全天")){
                string morning = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "sundayMorning");
                string afternoon = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "sundayAfternoon");
                string night = bandedGridView1.GetRowCellDisplayText(e.RowHandle, "sundayNight");
                if (morning.Equals("√") || afternoon.Equals("√") || night.Equals("√")
                    || morning.Equals("特√") || afternoon.Equals("特√") || night.Equals("特√"))
                {
                    ts = "选中了上午、下午、晚上就不能选择全天";
                }
            }
            if(ts.Length>0){
                MessageBoxUtils.Hint(ts, HintMessageBoxIcon.Error, MainForm);
                return;
            }

            String hideValue = "";
            if (e.Column.Caption.Equals("周一上午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "mondayMorningValue").ToString();
            }
            else if (e.Column.Caption.Equals("周一下午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "mondayAfternoonValue").ToString();
            }
            else if (e.Column.Caption.Equals("周一晚上"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "mondayNightValue").ToString();
            }
            else if (e.Column.Caption.Equals("周一全天"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "mondayAllAayValue").ToString();
            }
            else if (e.Column.Caption.Equals("周二上午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "tuesdayMorningValue").ToString();
            }
            else if (e.Column.Caption.Equals("周二下午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "tuesdayAfternoonValue").ToString();
            }
            else if (e.Column.Caption.Equals("周二晚上"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "tuesdayNightValue").ToString();
            }
            else if (e.Column.Caption.Equals("周二全天"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "tuesdayAllAayValue").ToString();
            }
            else if (e.Column.Caption.Equals("周三上午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "wednesdayMorningValue").ToString();
            }
            else if (e.Column.Caption.Equals("周三下午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "wednesdayAfternoonValue").ToString();
            }
            else if (e.Column.Caption.Equals("周三晚上"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "wednesdayNightValue").ToString();
            }
            else if (e.Column.Caption.Equals("周三全天"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "wednesdayAllAayValue").ToString();
            }
            else if (e.Column.Caption.Equals("周四上午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "thursdayMorningValue").ToString();
            }
            else if (e.Column.Caption.Equals("周四下午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "thursdayAfternoonValue").ToString();
            }
            else if (e.Column.Caption.Equals("周四晚上"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "thursdayNightValue").ToString();
            }
            else if (e.Column.Caption.Equals("周四全天"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "thursdayAllAayValue").ToString();
            }
            else if (e.Column.Caption.Equals("周五上午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "fridayMorningValue").ToString();
            }
            else if (e.Column.Caption.Equals("周五下午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "fridayAfternoonValue").ToString();
            }
            else if (e.Column.Caption.Equals("周五晚上"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "fridayNightValue").ToString();
            }
            else if (e.Column.Caption.Equals("周五全天"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "fridayAllAayValue").ToString();
            }
            else if (e.Column.Caption.Equals("周六上午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "saturdayMorningValue").ToString();
            }
            else if (e.Column.Caption.Equals("周六下午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "saturdayAfternoonValue").ToString();
            }
            else if (e.Column.Caption.Equals("周六晚上"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "saturdayNightValue").ToString();
            }
            else if (e.Column.Caption.Equals("周六全天"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "saturdayAllAayValue").ToString();
            }
            else if (e.Column.Caption.Equals("周日上午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "sundayMorningValue").ToString();
            }
            else if (e.Column.Caption.Equals("周日下午"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "sundayAfternoonValue").ToString();
            }
            else if (e.Column.Caption.Equals("周日晚上"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "sundayNightValue").ToString();
            }
            else if (e.Column.Caption.Equals("周日全天"))
            {
                hideValue = bandedGridView1.GetRowCellValue(e.RowHandle, "sundayAllAayValue").ToString();
            }
            //hideValue：表格中获取的值，用于判断
            //hideValue2：判断后需要赋给表格的值


            if (hideValue.Length > 0 && (e.CellValue.Equals("√") || e.CellValue.Equals("特√")))
            {
                String wb = e.Column.Caption.Substring(2, 2);//周几
                if (wb.Equals("上午"))
                {
                    wb = "0";
                }
                else if (wb.Equals("下午"))
                {
                    wb = "1";
                }
                else if (wb.Equals("晚上"))
                {
                    wb = "2";
                }
                else if (wb.Equals("全天"))
                {
                    wb = "3";
                }
                //已有排班的取消勾选需要弹窗确认是否勾选
                String param = "hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + treeMenuControl1.EditValue
    + "&doctorId=" + bandedGridView1.GetRowCellValue(e.RowHandle, "doctorId").ToString() + "&workDate=" + dt.ToString("yyyy-MM-dd") + "&period=" + wb;
                String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/getRegisterNum?" + param;
                cmd.ShowOpaqueLayer();
                this.DoWorkAsync(300, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        cmd.HideOpaqueLayer();
                        if (MessageBoxUtils.Show("当前已预约人数:" + objT["result"].ToString() + "人,确定要停诊吗?", MessageBoxButtons.OKCancel,
MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
                        {
                            updateValue(e, hideValue);
                            return;
                        }
                    }
                    else
                    {
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                        return;
                    }
                });
            }
            else
            {
                updateValue(e, hideValue);
            }
        }

        /// <summary>
        /// 修改表格中的值
        /// </summary>
        /// <param name="e"></param>
        /// <param name="hideValue"></param>
        private void updateValue(DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e, String hideValue)
        {
            if (e.CellValue.Equals("√"))
            {
                bandedGridView1.SetRowCellValue(e.RowHandle, e.Column.FieldName, "口");
            }
            else if (e.CellValue.Equals("特√"))
            {
                bandedGridView1.SetRowCellValue(e.RowHandle, e.Column.FieldName, "□");
            }
            else if (e.CellValue.Equals("口"))
            {
                bandedGridView1.SetRowCellValue(e.RowHandle, e.Column.FieldName, "√");
            }
            else if (e.CellValue.Equals("□"))
            {
                bandedGridView1.SetRowCellValue(e.RowHandle, e.Column.FieldName, "特√");
            }
        }

        //TODO:保存批量排班数据方法代码量有可优化空间
        private void btnSave_Click(object sender, EventArgs e)
        {
            List<SchedulingSubEntity> schedulingSubList = new List<SchedulingSubEntity>();
            List<DoctorVSEntity> DVSList = gcDoctor.DataSource as List<DoctorVSEntity>;
            if (DVSList == null || DVSList.Count == 0) return;

            #region 获取排班数据
            for (int i = 0; i < DVSList.Count; i++)
            {
                DoctorVSEntity doctor = DVSList[i];
                DateTime dt = DateTime.Parse(lueDate.Text + " 00:00:00");
                for (int c = 0; c < 28; c++ )
                {
                    SchedulingSubEntity schedulingSub = new SchedulingSubEntity();
                    schedulingSub.doctorId = doctor.doctorId;
                    
                    if(c==0||c==1||c==2||c==3){
                        schedulingSub.workDate = dt.ToString("u").Substring(0, 10);
                        schedulingSub.week = "一";
                    } 
                    else if (c == 4 || c == 5 || c == 6 || c == 7){
                        schedulingSub.workDate = dt.AddDays(1).ToString("u").Substring(0, 10);
                        schedulingSub.week = "二";
                    }
                    else if (c == 8 || c == 9 || c == 10 || c == 11){
                        schedulingSub.workDate = dt.AddDays(2).ToString("u").Substring(0, 10);
                        schedulingSub.week = "三";
                    }
                    else if (c == 12 || c == 13 || c == 14 || c == 15){
                        schedulingSub.workDate = dt.AddDays(3).ToString("u").Substring(0, 10);
                        schedulingSub.week = "四";
                    }
                    else if (c == 16 || c == 17 || c == 18 || c == 19){
                        schedulingSub.workDate = dt.AddDays(4).ToString("u").Substring(0, 10);
                        schedulingSub.week = "五";
                    }
                    else if (c == 20 || c == 21 || c == 22 || c == 23){
                        schedulingSub.workDate = dt.AddDays(5).ToString("u").Substring(0, 10);
                        schedulingSub.week = "六";
                    }
                    else if (c == 24 || c == 25 || c == 26 || c == 27)
                    {
                        schedulingSub.workDate = dt.AddDays(6).ToString("u").Substring(0, 10);
                        schedulingSub.week = "日";
                    }

                    if (c == 0)
                    {
                        schedulingSub.period = "0";
                        if (doctor.mondayMorning.Equals("√") || doctor.mondayMorning.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.mondayMorning.Equals("特√") || doctor.mondayMorning.Equals("□")) schedulingSub.mzType = "2";
                        
                        if ((doctor.mondayMorning.Equals("口") && doctor.mondayMorningValue.Length == 0))
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.mondayMorning.Equals("√") || doctor.mondayMorning.Equals("特√")) && doctor.mondayMorningValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.mondayMorning.Equals("口") || doctor.mondayMorning.Equals("□")) && doctor.mondayMorningValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.mondayMorning.Equals("√") && doctor.mondayMorningValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 1)
                    {
                        schedulingSub.period = "1";
                        if (doctor.mondayAfternoon.Equals("√") || doctor.mondayAfternoon.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.mondayAfternoon.Equals("特√") || doctor.mondayAfternoon.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.mondayAfternoon.Equals("口") && doctor.mondayAfternoonValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.mondayAfternoon.Equals("√") || doctor.mondayAfternoon.Equals("特√")) && doctor.mondayAfternoonValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.mondayAfternoon.Equals("口") || doctor.mondayAfternoon.Equals("□")) && doctor.mondayAfternoonValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.mondayAfternoon.Equals("√") && doctor.mondayAfternoonValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 2)
                    {
                        schedulingSub.period = "2";
                        if (doctor.mondayNight.Equals("√") || doctor.mondayNight.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.mondayNight.Equals("特√") || doctor.mondayNight.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.mondayNight.Equals("口") && doctor.mondayNightValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.mondayNight.Equals("√") || doctor.mondayNight.Equals("特√")) && doctor.mondayNightValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.mondayNight.Equals("口") || doctor.mondayNight.Equals("□")) && doctor.mondayNightValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.mondayNight.Equals("√") && doctor.mondayNightValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 3)
                    {
                        schedulingSub.period = "3";
                        if (doctor.mondayAllAay.Equals("√") || doctor.mondayAllAay.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.mondayAllAay.Equals("特√") || doctor.mondayAllAay.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.mondayAllAay.Equals("口") && doctor.mondayAllAayValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.mondayAllAay.Equals("√") || doctor.mondayAllAay.Equals("特√")) && doctor.mondayAllAayValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.mondayAllAay.Equals("口") || doctor.mondayAllAay.Equals("□")) && doctor.mondayAllAayValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.mondayAllAay.Equals("√") && doctor.mondayAllAayValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 4)
                    {
                        schedulingSub.period = "0";
                        if (doctor.tuesdayMorning.Equals("√") || doctor.tuesdayMorning.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.tuesdayMorning.Equals("特√") || doctor.tuesdayMorning.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.tuesdayMorning.Equals("口") && doctor.tuesdayMorningValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.tuesdayMorning.Equals("√") || doctor.tuesdayMorning.Equals("特√")) && doctor.tuesdayMorningValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.tuesdayMorning.Equals("口") || doctor.tuesdayMorning.Equals("□")) && doctor.tuesdayMorningValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.tuesdayMorning.Equals("√") && doctor.tuesdayMorningValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 5)
                    {
                        schedulingSub.period = "1";
                        if (doctor.tuesdayMorning.Equals("√") || doctor.tuesdayMorning.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.tuesdayMorning.Equals("特√") || doctor.tuesdayMorning.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.tuesdayAfternoon.Equals("口") && doctor.tuesdayAfternoonValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.tuesdayAfternoon.Equals("√") || doctor.tuesdayAfternoon.Equals("特√")) && doctor.tuesdayAfternoonValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.tuesdayAfternoon.Equals("口") || doctor.tuesdayAfternoon.Equals("□")) && doctor.tuesdayAfternoonValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.tuesdayAfternoon.Equals("√") && doctor.tuesdayAfternoonValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 6)
                    {
                        schedulingSub.period = "2";
                        if (doctor.tuesdayNight.Equals("√") || doctor.tuesdayNight.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.tuesdayNight.Equals("特√") || doctor.tuesdayNight.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.tuesdayNight.Equals("口") && doctor.tuesdayNightValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.tuesdayNight.Equals("√") || doctor.tuesdayNight.Equals("特√")) && doctor.tuesdayNightValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.tuesdayNight.Equals("口") || doctor.tuesdayNight.Equals("□")) && doctor.tuesdayNightValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.tuesdayNight.Equals("√") && doctor.tuesdayNightValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                            
                        }
                    }
                    else if (c == 7)
                    {
                        schedulingSub.period = "3";
                        if (doctor.tuesdayAllAay.Equals("√") || doctor.tuesdayAllAay.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.tuesdayAllAay.Equals("特√") || doctor.tuesdayAllAay.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.tuesdayAllAay.Equals("口") && doctor.tuesdayAllAayValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.tuesdayAllAay.Equals("√") || doctor.tuesdayAllAay.Equals("特√")) && doctor.tuesdayAllAayValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.tuesdayAllAay.Equals("口") || doctor.tuesdayAllAay.Equals("□")) && doctor.tuesdayAllAayValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.tuesdayAllAay.Equals("√") && doctor.tuesdayAllAayValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 8)
                    {
                        schedulingSub.period = "0";
                        if (doctor.wednesdayMorning.Equals("√") || doctor.wednesdayMorning.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.wednesdayMorning.Equals("特√") || doctor.wednesdayMorning.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.wednesdayMorning.Equals("口") && doctor.wednesdayMorningValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.wednesdayMorning.Equals("√") || doctor.wednesdayMorning.Equals("特√")) && doctor.wednesdayMorningValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.wednesdayMorning.Equals("口") || doctor.wednesdayMorning.Equals("□")) && doctor.wednesdayMorningValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.wednesdayMorning.Equals("√") && doctor.wednesdayMorningValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 9)
                    {
                        schedulingSub.period = "1";
                        if (doctor.wednesdayAfternoon.Equals("√") || doctor.wednesdayAfternoon.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.wednesdayAfternoon.Equals("特√") || doctor.wednesdayAfternoon.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.wednesdayAfternoon.Equals("口") && doctor.wednesdayAfternoonValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.wednesdayAfternoon.Equals("√") || doctor.wednesdayAfternoon.Equals("特√")) && doctor.wednesdayAfternoonValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.wednesdayAfternoon.Equals("口") || doctor.wednesdayAfternoon.Equals("□")) && doctor.wednesdayAfternoonValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.wednesdayAfternoon.Equals("√") && doctor.wednesdayAfternoonValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 10)
                    {
                        schedulingSub.period = "2";
                        if (doctor.wednesdayNight.Equals("√") || doctor.wednesdayNight.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.wednesdayNight.Equals("特√") || doctor.wednesdayNight.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.wednesdayNight.Equals("口") && doctor.wednesdayNightValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.wednesdayNight.Equals("√") || doctor.wednesdayNight.Equals("特√")) && doctor.wednesdayNightValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.wednesdayNight.Equals("口") || doctor.wednesdayNight.Equals("□")) && doctor.wednesdayNightValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.wednesdayNight.Equals("√") && doctor.wednesdayNightValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 11)
                    {
                        schedulingSub.period = "3";
                        if (doctor.wednesdayAllAay.Equals("√") || doctor.wednesdayAllAay.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.wednesdayAllAay.Equals("特√") || doctor.wednesdayAllAay.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.wednesdayAllAay.Equals("口") && doctor.wednesdayAllAayValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.wednesdayAllAay.Equals("√") || doctor.wednesdayAllAay.Equals("特√")) && doctor.wednesdayAllAayValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.wednesdayAllAay.Equals("口") || doctor.wednesdayAllAay.Equals("□")) && doctor.wednesdayAllAayValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.wednesdayAllAay.Equals("√") && doctor.wednesdayAllAayValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 12)
                    {
                        schedulingSub.period = "0";
                        if (doctor.thursdayMorning.Equals("√") || doctor.thursdayMorning.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.thursdayMorning.Equals("特√") || doctor.thursdayMorning.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.thursdayMorning.Equals("口") && doctor.thursdayMorningValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.thursdayMorning.Equals("√") || doctor.thursdayMorning.Equals("特√")) && doctor.thursdayMorningValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.thursdayMorning.Equals("口") || doctor.thursdayMorning.Equals("□")) && doctor.thursdayMorningValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.thursdayMorning.Equals("√") && doctor.thursdayMorningValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 13)
                    {
                        schedulingSub.period = "1";
                        if (doctor.thursdayAfternoon.Equals("√") || doctor.thursdayAfternoon.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.thursdayAfternoon.Equals("特√") || doctor.thursdayAfternoon.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.thursdayAfternoon.Equals("口") && doctor.thursdayAfternoonValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.thursdayAfternoon.Equals("√") || doctor.thursdayAfternoon.Equals("特√")) && doctor.thursdayAfternoonValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.thursdayAfternoon.Equals("口") || doctor.thursdayAfternoon.Equals("□")) && doctor.thursdayAfternoonValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.thursdayAfternoon.Equals("√") && doctor.thursdayAfternoonValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 14)
                    {
                        schedulingSub.period = "2";
                        if (doctor.thursdayNight.Equals("√") || doctor.thursdayNight.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.thursdayNight.Equals("特√") || doctor.thursdayNight.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.thursdayNight.Equals("口") && doctor.thursdayNightValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.thursdayNight.Equals("√") || doctor.thursdayNight.Equals("特√")) && doctor.thursdayNightValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.thursdayNight.Equals("口") || doctor.thursdayNight.Equals("□")) && doctor.thursdayNightValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.thursdayNight.Equals("√") && doctor.thursdayNightValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 15)
                    {
                        schedulingSub.period = "3";
                        if (doctor.thursdayAllAay.Equals("√") || doctor.thursdayAllAay.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.thursdayAllAay.Equals("特√") || doctor.thursdayAllAay.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.thursdayAllAay.Equals("口") && doctor.thursdayAllAayValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.thursdayAllAay.Equals("√") || doctor.thursdayAllAay.Equals("特√")) && doctor.thursdayAllAayValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.thursdayAllAay.Equals("口") || doctor.thursdayAllAay.Equals("□")) && doctor.thursdayAllAayValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.thursdayAllAay.Equals("√") && doctor.thursdayAllAayValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 16)
                    {
                        schedulingSub.period = "0";
                        if (doctor.fridayMorning.Equals("√") || doctor.fridayMorning.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.fridayMorning.Equals("特√") || doctor.fridayMorning.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.fridayMorning.Equals("口") && doctor.fridayMorningValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.fridayMorning.Equals("√") || doctor.fridayMorning.Equals("特√")) && doctor.fridayMorningValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.fridayMorning.Equals("口") || doctor.fridayMorning.Equals("□")) && doctor.fridayMorningValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.fridayMorning.Equals("√") && doctor.fridayMorningValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 17)
                    {
                        schedulingSub.period = "1";
                        if (doctor.fridayAfternoon.Equals("√") || doctor.fridayAfternoon.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.fridayAfternoon.Equals("特√") || doctor.fridayAfternoon.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.fridayAfternoon.Equals("口") && doctor.fridayAfternoonValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.fridayAfternoon.Equals("√") || doctor.fridayAfternoon.Equals("特√")) && doctor.fridayAfternoonValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.fridayAfternoon.Equals("口") || doctor.fridayAfternoon.Equals("□")) && doctor.fridayAfternoonValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.fridayAfternoon.Equals("√") && doctor.fridayAfternoonValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 18)
                    {
                        schedulingSub.period = "2";
                        if (doctor.fridayNight.Equals("√") || doctor.fridayNight.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.fridayNight.Equals("特√") || doctor.fridayNight.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.fridayNight.Equals("口") && doctor.fridayNightValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.fridayNight.Equals("√") || doctor.fridayNight.Equals("特√")) && doctor.fridayNightValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.fridayNight.Equals("口") || doctor.fridayNight.Equals("□")) && doctor.fridayNightValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.fridayNight.Equals("√") && doctor.fridayNightValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 19)
                    {
                        schedulingSub.period = "3";
                        if (doctor.fridayAllAay.Equals("√") || doctor.fridayAllAay.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.fridayAllAay.Equals("特√") || doctor.fridayAllAay.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.fridayAllAay.Equals("口") && doctor.fridayAllAayValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.fridayAllAay.Equals("√") || doctor.fridayAllAay.Equals("特√")) && doctor.fridayAllAayValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.fridayAllAay.Equals("口") || doctor.fridayAllAay.Equals("□")) && doctor.fridayAllAayValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.fridayAllAay.Equals("√") && doctor.fridayAllAayValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 20)
                    {
                        schedulingSub.period = "0";
                        if (doctor.saturdayMorning.Equals("√") || doctor.saturdayMorning.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.saturdayMorning.Equals("特√") || doctor.saturdayMorning.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.saturdayMorning.Equals("口") && doctor.saturdayMorningValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.saturdayMorning.Equals("√") || doctor.saturdayMorning.Equals("特√")) && doctor.saturdayMorningValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.saturdayMorning.Equals("口") || doctor.saturdayMorning.Equals("□")) && doctor.saturdayMorningValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.saturdayMorning.Equals("√") && doctor.saturdayMorningValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 21)
                    {
                        schedulingSub.period = "1";
                        if (doctor.saturdayAfternoon.Equals("√") || doctor.saturdayAfternoon.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.saturdayAfternoon.Equals("特√") || doctor.saturdayAfternoon.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.saturdayAfternoon.Equals("口") && doctor.saturdayAfternoonValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.saturdayAfternoon.Equals("√") || doctor.saturdayAfternoon.Equals("特√")) && doctor.saturdayAfternoonValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.saturdayAfternoon.Equals("口") || doctor.saturdayAfternoon.Equals("□")) && doctor.saturdayAfternoonValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.saturdayAfternoon.Equals("√") && doctor.saturdayAfternoonValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 22)
                    {
                        schedulingSub.period = "2";
                        if (doctor.saturdayNight.Equals("√") || doctor.saturdayNight.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.saturdayNight.Equals("特√") || doctor.saturdayNight.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.saturdayNight.Equals("口") && doctor.saturdayNightValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.saturdayNight.Equals("√") || doctor.saturdayNight.Equals("特√")) && doctor.saturdayNightValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.saturdayNight.Equals("口") || doctor.saturdayNight.Equals("□")) && doctor.saturdayNightValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.saturdayNight.Equals("√") && doctor.saturdayNightValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 23)
                    {
                        schedulingSub.period = "3";
                        if (doctor.saturdayAllAay.Equals("√") || doctor.saturdayAllAay.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.saturdayAllAay.Equals("特√") || doctor.saturdayAllAay.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.saturdayAllAay.Equals("口") && doctor.saturdayAllAayValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.saturdayAllAay.Equals("√") || doctor.saturdayAllAay.Equals("特√")) && doctor.saturdayAllAayValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.saturdayAllAay.Equals("口") || doctor.saturdayAllAay.Equals("□")) && doctor.saturdayAllAayValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.saturdayAllAay.Equals("√") && doctor.saturdayAllAayValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 24)
                    {
                        schedulingSub.period = "0";
                        if (doctor.sundayMorning.Equals("√") || doctor.sundayMorning.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.sundayMorning.Equals("特√") || doctor.sundayMorning.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.sundayMorning.Equals("口") && doctor.sundayMorningValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.sundayMorning.Equals("√") || doctor.sundayMorning.Equals("特√")) && doctor.sundayMorningValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.sundayMorning.Equals("口") || doctor.sundayMorning.Equals("□")) && doctor.sundayMorningValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.sundayMorning.Equals("√") && doctor.sundayMorningValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 25)
                    {
                        schedulingSub.period = "1";
                        if (doctor.sundayAfternoon.Equals("√") || doctor.sundayAfternoon.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.sundayAfternoon.Equals("特√") || doctor.sundayAfternoon.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.sundayAfternoon.Equals("口") && doctor.sundayAfternoonValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.sundayAfternoon.Equals("√") || doctor.sundayAfternoon.Equals("特√")) && doctor.sundayAfternoonValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.sundayAfternoon.Equals("口") || doctor.sundayAfternoon.Equals("□")) && doctor.sundayAfternoonValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.sundayAfternoon.Equals("√") && doctor.sundayAfternoonValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 26)
                    {
                        schedulingSub.period = "2";
                        if (doctor.sundayNight.Equals("√") || doctor.sundayNight.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.sundayNight.Equals("特√") || doctor.sundayNight.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.sundayNight.Equals("口") && doctor.sundayNightValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.sundayNight.Equals("√") || doctor.sundayNight.Equals("特√")) && doctor.sundayNightValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.sundayNight.Equals("口") || doctor.sundayNight.Equals("□")) && doctor.sundayNightValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.sundayNight.Equals("√") && doctor.sundayNightValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    else if (c == 27)
                    {
                        schedulingSub.period = "3";
                        if (doctor.sundayAllAay.Equals("√") || doctor.sundayAllAay.Equals("口")) schedulingSub.mzType = "1";
                        else if (doctor.sundayAllAay.Equals("特√") || doctor.sundayAllAay.Equals("□")) schedulingSub.mzType = "2";
                        if (doctor.sundayAllAay.Equals("口") && doctor.sundayAllAayValue.Length == 0)
                        {
                            //口+id长度0=没有排班，无需处理(没有点击过，或者点击了又取消了)
                            schedulingSub = null;
                        }
                        else if ((doctor.sundayAllAay.Equals("√") || doctor.sundayAllAay.Equals("特√")) && doctor.sundayAllAayValue.Length != 0)
                        {
                            //√或者特√+id长度不为0=有排班，无需处理(默认有排班，没改过，或者取消了又选中)
                            schedulingSub = null;
                        }
                        else if ((doctor.sundayAllAay.Equals("口") || doctor.sundayAllAay.Equals("□")) && doctor.sundayAllAayValue.Length != 0)
                        {
                            //口或□+id长度不为0=停诊，组装数据
                            schedulingSub.isPlan = "false";
                        }
                        else if (doctor.sundayAllAay.Equals("√") && doctor.sundayAllAayValue.Length == 0)
                        {
                            //√+id长度为0=新增排班，组装数据
                            schedulingSub.isPlan = "true";
                        }
                    }
                    if (schedulingSub!=null)
                        schedulingSubList.Add(schedulingSub);
                }
            }
            #endregion
            if (schedulingSubList.Count == 0)
            {
                MessageBoxUtils.Show("没有修改过批量排班数据，不需要保存", MessageBoxButtons.OK, MainForm);
                return;
            }
            String scheduSets = Newtonsoft.Json.JsonConvert.SerializeObject(schedulingSubList);
            String param = "hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + treeMenuControl1.EditValue + "&scheduSets=" + scheduSets;
            //String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/saveToMany?";

            var edit = new WorkNumVerificationEdit();
            edit.param = param;
            if (edit.ShowDialog() == DialogResult.OK)
            {
                Thread.Sleep(300);
                SearchData();
                MessageBoxUtils.Hint("保存成功!", MainForm);
            }


            //cmd.ShowOpaqueLayer(0.56f, "正在提交数据中");
            //this.DoWorkAsync( 500 ,(o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            //{
            //    String data = HttpClass.httpPost(url, param, 10);
            //    return data;

            //}, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            //{
            //    JObject objT = JObject.Parse(r.ToString());
            //    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            //    {
            //        SearchData();
            //        MessageBoxUtils.Hint("保存成功!", MainForm);
            //    }
            //    else
            //    {
            //        cmd.HideOpaqueLayer();
            //        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            //    }
            //});

            //String data = HttpClass.httpPost(url, param, 10);
            //JObject objT = JObject.Parse(data);
            //if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            //{
            //    MessageBoxUtils.Hint("保存成功!");
            //}
            //else
            //{
            //    MessageBox.Show(objT["message"].ToString());
            //}
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

        private void BatchSchedulingForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
            if (gcDoctor.Width < 1080)
            {
                mondayMorning.Caption = "上";
                mondayAfternoon.Caption = "下";
                mondayNight.Caption = "晚";
                mondayAllDay.Caption = "全";

                tuesdayMorning.Caption = "上";
                tuesdayAfternoon.Caption = "下";
                tuesdayNight.Caption = "晚";
                tuesdayAllDay.Caption = "全";

                wednesdayMorning.Caption = "上";
                wednesdayAfternoon.Caption = "下";
                wednesdayNight.Caption = "晚";
                wednesdayAllDay.Caption = "全";

                thursdayMorning.Caption = "上";
                thursdayAfternoon.Caption = "下";
                thursdayNight.Caption = "晚";
                thursdayAllDay.Caption = "全";

                fridayMorning.Caption = "上";
                fridayAfternoon.Caption = "下";
                fridayNight.Caption = "晚";
                fridayAllDay.Caption = "全";

                saturdayMorning.Caption = "上";
                saturdayAfternoon.Caption = "下";
                saturdayNight.Caption = "晚";
                saturdayAllDay.Caption = "全";

                sundayMorning.Caption = "上";
                sundayAfternoon.Caption = "下";
                sundayNight.Caption = "晚";
                sundayAllDay.Caption = "全";
            }
            else
            {
                mondayMorning.Caption = "上午";
                mondayAfternoon.Caption = "下午";
                mondayNight.Caption = "晚上";
                mondayAllDay.Caption = "全天";

                tuesdayMorning.Caption = "上午";
                tuesdayAfternoon.Caption = "下午";
                tuesdayNight.Caption = "晚上";
                tuesdayAllDay.Caption = "全天";

                wednesdayMorning.Caption = "上午";
                wednesdayAfternoon.Caption = "下午";
                wednesdayNight.Caption = "晚上";
                wednesdayAllDay.Caption = "全天";

                thursdayMorning.Caption = "上午";
                thursdayAfternoon.Caption = "下午";
                thursdayNight.Caption = "晚上";
                thursdayAllDay.Caption = "全天";

                fridayMorning.Caption = "上午";
                fridayAfternoon.Caption = "下午";
                fridayNight.Caption = "晚上";
                fridayAllDay.Caption = "全天";

                saturdayMorning.Caption = "上午";
                saturdayAfternoon.Caption = "下午";
                saturdayNight.Caption = "晚上";
                saturdayAllDay.Caption = "全天";

                sundayMorning.Caption = "上午";
                sundayAfternoon.Caption = "下午";
                sundayNight.Caption = "晚上";
                sundayAllDay.Caption = "全天";
            }
        }

        private void bandedGridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            Color color = e.Appearance.BackColor;
            if (e.Column.FieldName == "mondayMorning" || e.Column.FieldName == "mondayAfternoon"
                || e.Column.FieldName == "mondayNight" || e.Column.FieldName == "mondayAllAay"
                 || e.Column.FieldName == "wednesdayMorning" || e.Column.FieldName == "wednesdayAfternoon"
                 || e.Column.FieldName == "wednesdayNight" || e.Column.FieldName == "wednesdayAllAay"
                 || e.Column.FieldName == "fridayMorning" || e.Column.FieldName == "fridayAfternoon"
                 || e.Column.FieldName == "fridayNight" || e.Column.FieldName == "fridayAllAay"
                 || e.Column.FieldName == "sundayMorning" || e.Column.FieldName == "sundayAfternoon"
                 || e.Column.FieldName == "sundayNight" || e.Column.FieldName == "sundayAllAay")
            {
                color = Color.FromArgb(199, 234, 252);                    
            }
            if ((string)e.CellValue == "特√")  //条件  e.CellValue 为object类型
                color = Color.FromArgb(255, 34, 167);
            e.Appearance.BackColor = color;
        }

        private void bandedGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            e.ErrorText = "只能输入正整数！";
            e.Valid = false;
            return;
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            columShowHide("",null,null,null);
        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            columShowHide(null, "", null, null);
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            columShowHide(null, null, "", null);
        }

        private void checkBox4_CheckStateChanged(object sender, EventArgs e)
        {
            columShowHide(null, null, null, "");
        }
        
        /// <summary>
        /// 显示或隐藏午别对应的列
        /// </summary>
        /// <param name="m">不为null即为显示或隐藏上午的列，为null则上午的列不做处理</param>
        /// <param name="a">不为null即为显示或隐藏下午的列，为null则下午的列不做处理</param>
        /// <param name="n">不为null即为显示或隐藏晚上的列，为null则晚上的列不做处理</param>
        /// <param name="all">不为null即为显示或隐藏全天的列，为null则全天的列不做处理</param>
        private void columShowHide(String m, String a, String n, String all)
        {
            if (m != null)
            {
                if (checkBox1.CheckState == CheckState.Checked)
                {
                    gridColumn2.Visible = true;
                    gridColumn6.Visible = true;
                    gridColumn10.Visible = true;
                    gridColumn14.Visible = true;
                    gridColumn18.Visible = true;
                    gridColumn22.Visible = true;
                    gridColumn26.Visible = true;
                }
                else
                {
                    gridColumn2.Visible = false;
                    gridColumn6.Visible = false;
                    gridColumn10.Visible = false;
                    gridColumn14.Visible = false;
                    gridColumn18.Visible = false;
                    gridColumn22.Visible = false;
                    gridColumn26.Visible = false;
                }
            }
            if (a != null)
            {
                if (checkBox2.CheckState == CheckState.Checked)
                {
                    gridColumn3.Visible = true;
                    gridColumn7.Visible = true;
                    gridColumn11.Visible = true;
                    gridColumn15.Visible = true;
                    gridColumn19.Visible = true;
                    gridColumn23.Visible = true;
                    gridColumn27.Visible = true;
                }
                else
                {
                    gridColumn3.Visible = false;
                    gridColumn7.Visible = false;
                    gridColumn11.Visible = false;
                    gridColumn15.Visible = false;
                    gridColumn19.Visible = false;
                    gridColumn23.Visible = false;
                    gridColumn27.Visible = false;
                }
            }
            if (n != null)
            {
                if (checkBox3.CheckState == CheckState.Checked)
                {
                    gridColumn4.Visible = true;
                    gridColumn8.Visible = true;
                    gridColumn12.Visible = true;
                    gridColumn16.Visible = true;
                    gridColumn20.Visible = true;
                    gridColumn24.Visible = true;
                    gridColumn28.Visible = true;
                }
                else
                {
                    gridColumn4.Visible = false;
                    gridColumn8.Visible = false;
                    gridColumn12.Visible = false;
                    gridColumn16.Visible = false;
                    gridColumn20.Visible = false;
                    gridColumn24.Visible = false;
                    gridColumn28.Visible = false;
                }
            }
            if (all != null)
            {
                if (checkBox4.CheckState == CheckState.Checked)
                {
                    gridColumn5.Visible = true;
                    gridColumn9.Visible = true;
                    gridColumn13.Visible = true;
                    gridColumn17.Visible = true;
                    gridColumn21.Visible = true;
                    gridColumn25.Visible = true;
                    gridColumn29.Visible = true;
                }
                else
                {
                    gridColumn5.Visible = false;
                    gridColumn9.Visible = false;
                    gridColumn13.Visible = false;
                    gridColumn17.Visible = false;
                    gridColumn21.Visible = false;
                    gridColumn25.Visible = false;
                    gridColumn29.Visible = false;
                }
            }
        }
    }
}
