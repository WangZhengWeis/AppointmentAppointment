using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Xr.Http.RestSharp;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;
using Xr.RtCall.Model;

namespace Xr.RtCall.pages
{
    public partial class RtIntersectionAppointmentFrm : UserControl
    {
        public SynchronizationContext _context;
        int selectLue = 0;
        Patient patients;
        public RtIntersectionAppointmentFrm(Patient patient)
        {
            InitializeComponent();
            patients = new Patient();
            patients = patient;
            label14.Text = patient.cardNo;
            lab_patientName.Text = patient.patientName;
            GetCardType();//获取卡类型
            GetDoctorName();
            //DoctorScheduling();//获取医生排班日期
            selectLue = Convert.ToInt32(patient.cardType);
            _context = SynchronizationContext.Current;
        }
        #region 设置卡类型
        /// <summary>
        /// 获取卡类型
        /// </summary>
        public void GetCardType()
        {
            try
            {
                //卡类型：1患者ID、2诊疗卡、3社保卡、4身份证、5健康卡、6健康虚拟卡
                List<CardType> listCard = new List<CardType>();
                listCard.Add(new CardType { value = "1", label = "患者ID" });
                listCard.Add(new CardType { value = "2", label = "诊疗卡" });
                listCard.Add(new CardType { value = "3", label = "社保卡" });
                listCard.Add(new CardType { value = "4", label = "身份证" });
                listCard.Add(new CardType { value = "5", label = "健康卡" });
                listCard.Add(new CardType { value = "6", label = "健康虚拟卡" });
                lueIsUse.Properties.DataSource = listCard;
                lueIsUse.Properties.DisplayMember = "label";
                lueIsUse.Properties.ValueMember = "value";
                lueIsUse.EditValue = patients.cardType;
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("获取卡类型错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 医生排班日期
       
        /// <summary>
        ///  医生排班日期
        /// </summary>
        public void DoctorScheduling(string doctorId)
        {
            try
            {
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("hospitalId", HelperClass.hospitalId);
                prament.Add("deptId", HelperClass.deptId);//科室主键
                prament.Add("doctorId", doctorId);//医生主键
                prament.Add("type", "1");//类型：0公开预约号源、1诊间预约号源
                Xr.RtCall.Model.RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.findByDeptAndDoctor, prament, Method.POST,
               result =>
               {
                   switch (result.ResponseStatus)
                   {
                       case ResponseStatus.Completed:
                           if (result.StatusCode == System.Net.HttpStatusCode.OK)
                           {
                               Log4net.LogHelper.Info("请求结果：" + string.Join(",", result.Data.ToArray()));
                               JObject objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                               if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                               {
                                   //更新日历
                                   List<Dictionary<int, String>> dcs = new List<Dictionary<int, String>>();
                                   List<AvaDateEntity> list = objT["result"].ToObject<List<AvaDateEntity>>();
                                   Dictionary<int, String> dc1 = new Dictionary<int, String>();
                                   if (list.Count == 0)
                                   {
                                       _context.Send((s) => Xr.Common.MessageBoxUtils.Show("当前选择医生没有排班日期", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,Form1.pCurrentWin), null);
                                       _context.Send((s) => reservationCalendar1.ChangeValidDate(dcs), null);
                                       return;
                                   }
                                   string Month = System.DateTime.Now.ToString("MM");//list[0].month;
                                   foreach (var item in list)
                                   {
                                       if (item.month != Month)
                                       {
                                           dcs.Add(dc1);
                                           dc1 = new Dictionary<int, String>();
                                           Month = item.month;
                                       }
                                       if (item.syNum > 0)
                                       {
                                           dc1.Add(Int32.Parse(item.day), System.DateTime.Now.ToString());
                                       }
                                       else
                                       {
                                           dc1.Add(Int32.Parse(item.day), "约满");
                                       }
                                   }
                                   dcs.Add(dc1);
                                   _context.Send((s) => reservationCalendar1.ChangeValidDate(dcs), null);
                               }
                               else
                               {
                                   _context.Send((s) => Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,Form1.pCurrentWin), null);
                               }
                           }
                           break;
                   }
               });
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("获取医生排班号源错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 日期排班号源
        List<string> list;
        //dynamic listNum;
        /// <summary>
        /// 日期排班号源
        /// </summary>
        /// <param name="time">日期</param>
        public void TimeScheduling(string time)
        {
            try
            {
                list = new List<string>();
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("hospitalId", HelperClass.hospitalId);//医院主键
                prament.Add("deptId", HelperClass.deptId);//科室主键
                prament.Add("doctorId", HelperClass.doctorId);//医生主键
                prament.Add("workDate", time);//排班日期
                prament.Add("type", "1");//类型：0公开预约号源、1诊间预约号源
                Xr.RtCall.Model.RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.findTimeNum, prament, Method.POST,
                result =>
                {
                    #region
                    switch (result.ResponseStatus)
                    {
                        case ResponseStatus.Completed:
                            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                Log4net.LogHelper.Info("请求结果：" + string.Join(",", result.Data.ToArray()));
                                JObject objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    List<TimeNum> timenum = objT["result"].ToObject<List<TimeNum>>();
                                    List<Xr.Common.Controls.Item> listitem = new List<Common.Controls.Item>();
                                    foreach (var item in timenum)
                                    {
                                        Xr.Common.Controls.Item it = new Common.Controls.Item();
                                        it.name = item.beginTime + "-" + item.endTime + "" + "<" + item.num + ">";
                                        it.value = item.id;
                                        it.tag = item.beginTime + "-" + item.endTime + "#" + item.mzType;
                                        it.parentId = item.id;
                                        if (item.mzType == "2")//特需门诊显红色
                                        {
                                            it.spcialBColor = "Red";
                                        }
                                        listitem.Add(it);
                                    }
                                    _context.Send((s) => this.menuList.setDataSource(listitem), null);
                                }
                                else
                                {
                                 _context.Send((s) =>Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,Form1.pCurrentWin),null);
                                }
                            }
                            break;
                    }
                    #endregion
                });
            }
            catch (Exception ex)
            {
              Log4net.LogHelper.Error("获取日期排班号源错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 科室下的医生
        List<DoctorScheduling> listScheduling;
        public void GetDoctorName()
        {
            try
            {
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("pageNo", "1");
                prament.Add("pageSize", "1000");//暂时没有分页就一页传大点
                prament.Add("hospital.id", HelperClass.hospitalId);//医院主键
                prament.Add("dept.id", HelperClass.deptId);//科室主键
                Xr.RtCall.Model.RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.DoctorName, prament, Method.POST,
                result =>
                {
                    #region
                    switch (result.ResponseStatus)
                    {
                        case ResponseStatus.Completed:
                            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                Log4net.LogHelper.Info("请求结果：" + string.Join(",", result.Data.ToArray()));
                                JObject objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    listScheduling = objT["result"]["list"].ToObject<List<DoctorScheduling>>();
                                    if (Convert.ToBoolean(AppContext.AppConfig.IsDoctorClinic))
                                    {
                                        listScheduling = (from docotr in listScheduling where docotr.id == HelperClass.doctorId select docotr).ToList();
                                    }
                                    List<string> listName = new List<string>();
                                    List<Xr.Common.Controls.Item> listitem = new List<Common.Controls.Item>();
                                    foreach (var item in listScheduling)
                                    {
                                        Xr.Common.Controls.Item it = new Common.Controls.Item();
                                        it.name = item.name;
                                        it.value = item.id;
                                        it.tag = item.id;
                                        it.parentId = "";
                                        listitem.Add(it);
                                    }
                                    _context.Send((s) => this.menuDoctor.setDataSource(listitem), null);
                                    _context.Send((s) => menuDoctor.EditValue(HelperClass.doctorId), null);
                                }
                                else
                                {
                                    _context.Send((s) => Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,Form1.pCurrentWin), null);
                                }
                            }
                            break;
                    }
                    #endregion
                });
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("叫号获取医生错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 返回
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butReturn_Click(object sender, EventArgs e)
        {
            try
            {
                Form1.pCurrentWin.panel_MainFrm.Controls.Clear();
                RtCallPeationFrm rtcpf = new RtCallPeationFrm();
                rtcpf.Dock = DockStyle.Fill;
                Form1.pCurrentWin.panel_MainFrm.Controls.Add(rtcpf);
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("返回按钮错误信息："+ex.Message);
            }
        }
        #endregion 
        #region  确认预约
        /// <summary>
        /// 确认预约
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButYes_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> prament = new Dictionary<string, string>();
                #region
                prament.Add("scheduPlanId", YuYueId);//排班记录主键
                prament.Add("patientId", patients.patientId);//患者主键
                prament.Add("patientName", patients.patientName);//患者姓名
                prament.Add("cardType", patients.cardType);//卡类型
                prament.Add("cardNo", label14.Text.Trim());//卡号
                prament.Add("tempPhone", patients.telPhone);//手机号
                prament.Add("note", "");//备注
                prament.Add("visitType", "1");//就诊类别：0.初诊 ，1.复诊  VisitCategory.EditValue.ToString()
                prament.Add("addressType", AddressCategory.EditValue.ToString());//地址类别：0市内、1市外  
                prament.Add("isShfz", Postoperative.EditValue.ToString());//术后复诊：0是、1否
                prament.Add("isYwzz", Foreign.EditValue.ToString());//院外转诊：0是、1否
                prament.Add("isCyfz", Discharged.EditValue.ToString());//出院复诊：0是、1否
                prament.Add("isMxb", Chronic.EditValue.ToString());//慢性病：0是、1否
                prament.Add("registerWay", "1");//预约途径：1诊间、2自助机、3公众号、4卫计局平台、5官网
                #endregion
                #region
                Xr.RtCall.Model.RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.confirmBooking, prament, Method.POST,
                result =>
                {
                    switch (result.ResponseStatus)
                    {
                        case ResponseStatus.Completed:
                            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                Log4net.LogHelper.Info("请求结果：" + string.Join(",", result.Data.ToArray()));
                                JObject objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    _context.Send((s) => Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, Form1.pCurrentWin), null);
                                    _context.Send((s) => butReturn_Click(sender, e), null);
                                }
                                else
                                {
                                    _context.Send((s) => Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, Form1.pCurrentWin), null);
                                }
                            }
                            break;
                    }
                });
                #endregion
            }
            catch (Exception ex)
            {
                Xr.Common.MessageBoxUtils.Show("确认预约错误信息" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, Form1.pCurrentWin);
                Log4net.LogHelper.Error("确认预约错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 日期控件
        /// <summary>
        /// 点击日期控件
        /// </summary>
        /// <param name="SelectedDate"></param>
        private void reservationCalendar1_SelectDate(DateTime SelectedDate)
        {
            label8.Text = SelectedDate.ToString("yyyy-MM-dd");
            TimeScheduling(SelectedDate.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// 给控件赋可选择值
        /// </summary>
        /// <param name="SelectedMonth"></param>
        private void reservationCalendar1_ChangeMonth(DateTime SelectedMonth)
        {
            label8.Text = "";
            label13.Text = "";
            this.menuList.setDataSources(null, true,"");
        }
        #endregion 
        #region 点击菜单控件
        public String YuYueId { get; set; }
        /// <summary>
        /// 点击菜单控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuList_MenuItemClick(object sender, EventArgs e)
        {
            Label label = null;
            if (typeof(Label).IsInstanceOfType(sender))
            {
                label = (Label)sender;
            }
            else
            {
                Xr.Common.Controls.PanelEx panelEx = (Xr.Common.Controls.PanelEx)sender;
                label = (Label)panelEx.Controls[0];
            }
            label13.Text = label.Text.Trim().Substring(0, 11);
            YuYueId = label.Name.ToString();
            if (label.Tag.ToString().Split('#')[1] == "2")
            {
                lab_mz_typeStr.Text = "特需门诊";
            }
            else
            {
                lab_mz_typeStr.Text = "普通门诊";
            }
        }
        #endregion 
        #region 排班医生点击事件
        public String DoctorId { get; set; }
        private void menuDoctor_MenuItemClick(object sender, EventArgs e)
        {
            Label label = null;
            if (typeof(Label).IsInstanceOfType(sender))
            {
                label = (Label)sender;
            }
            else
            {
                Xr.Common.Controls.PanelEx panelEx = (Xr.Common.Controls.PanelEx)sender;
                label = (Label)panelEx.Controls[0];
            }
            DoctorId = label.Tag.ToString();
            YuYueId = "";
            label13.Text = "";
            List<Xr.Common.Controls.Item> item = new List<Common.Controls.Item>();
            menuList.setDataSource(item);
            DoctorScheduling(label.Tag.ToString());
        }
        #endregion 
    }
}
