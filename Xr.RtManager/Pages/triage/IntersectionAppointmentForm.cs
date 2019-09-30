using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Xr.Http;
using Xr.Common;
using Xr.RtManager.Module.triage;
using RestSharp;
using System.Threading;

namespace Xr.RtManager.Pages.triage
{
    public partial class IntersectionAppointmentForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public SynchronizationContext _context;
        public IntersectionAppointmentForm()
        {
            InitializeComponent();
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(225, false);
            MainForm = (Form)this.Parent;
            _context = SynchronizationContext.Current;
            Patientid = "";
            getLuesInfo();
            GetClincInfo();
            cmd.HideOpaqueLayer();
            this.btn_reservation.Enabled = false;
        }
        #region 获取卡类型
        /// <summary>
        /// 下拉框数据
        /// </summary>
        public List<DictEntity> ListCardType;
        /// <summary>
        /// 获取卡类型
        /// </summary>
        void getLuesInfo()
        {
            //卡类型下拉框数据
            String param = "type={0}";
            param = String.Format(param, "card_type");

            String url = String.Empty;
            url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?" + param;
            JObject objT = new JObject();
            objT = JObject.Parse(HttpClass.httpPost(url));
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                //List<Dic> list = objT["result"].ToObject<List<Dic>>();
                ListCardType = new List<DictEntity>();
                List<DictEntity> list = new List<DictEntity>();
                list.Add(new DictEntity { label = "", value = " " });
                list.AddRange(objT["result"].ToObject<List<DictEntity>>());
                ListCardType = list;
                lueCardType.Properties.DataSource = list;
                lueCardType.Properties.DisplayMember = "label";
                lueCardType.Properties.ValueMember = "value";
                lueCardType.EditValue = 1;

                lueCardTypeQuery.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                lueCardTypeQuery.Properties.DisplayMember = "label";
                lueCardTypeQuery.Properties.ValueMember = "value";
                lueCardTypeQuery.EditValue = 2;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
            }
        }
        #endregion
        #region 获取可挂号科室
        public void GetClincInfo()
        {
            Dictionary<string, string> prament = new Dictionary<string, string>();
            String url = String.Empty;
            url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept";
            String jsonStr = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(jsonStr);
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
                treeMenuControl1.DataSource = list;
                treeMenuControl1.EditValue = list[0].id;
                SelectDeptid = list[0].id;
                GetDoctorName(AppContext.Session.hospitalId, list[0].id);
            }
        }
        #endregion
        #region 科室下的医生
        List<DoctorScheduling> listScheduling;
        public void GetDoctorName(string hospitalid, string deptid)
        {
            cmd.ShowOpaqueLayer(225, false);
            try
            {
                Dictionary<string, string> prament = new Dictionary<string, string>();
                //prament.Add("pageNo", "1");
                //prament.Add("pageSize", "1000");//暂时没有分页就一页传大点
                prament.Add("hospital.id", hospitalid);//医院主键
                prament.Add("dept.id", deptid);//科室主键
                prament.Add("isOpen", "0");
                String url = String.Empty;
                String param = "";
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?" + param;
                String jsonStr = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(jsonStr);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    listScheduling = objT["result"].ToObject<List<DoctorScheduling>>();
                    List<string> listName = new List<string>();
                    List<Xr.Common.Controls.Item> listitem = new List<Xr.Common.Controls.Item>();
                    foreach (var item in listScheduling)
                    {
                        Xr.Common.Controls.Item it = new Xr.Common.Controls.Item();
                        it.name = item.name;
                        it.value = item.id;
                        it.tag = item.id;
                        it.parentId = "";
                        listitem.Add(it);
                    }
                    this.mcDoctor.setDataSource(listitem);
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
                #region
                //RestSharpHelper.ReturnResult<List<string>>("cms/doctor/findAll", prament, Method.POST,
                //result =>
                //{
                //    #region
                //    switch (result.ResponseStatus)
                //    {
                //        case ResponseStatus.Completed:
                //            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                //            {
                //                Log4net.LogHelper.Info("请求结果：" + string.Join(",", result.Data.ToArray()));
                //                JObject objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                //                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                //                {
                //                    listScheduling = objT["result"]["list"].ToObject<List<DoctorScheduling>>();
                //                    List<string> listName = new List<string>();
                //                    List<Xr.Common.Controls.Item> listitem = new List<Xr.Common.Controls.Item>();
                //                    foreach (var item in listScheduling)
                //                    {
                //                        Xr.Common.Controls.Item it = new Xr.Common.Controls.Item();
                //                        it.name = item.name;
                //                        it.value = item.id;
                //                        it.tag = item.id;
                //                        it.parentId = "";
                //                        listitem.Add(it);
                //                    }
                //                    _context.Send((s) => this.mcDoctor.setDataSource(listitem), null);
                //                    _context.Send((s) => cmd.HideOpaqueLayer(), null);
                //                }
                //                else
                //                {
                //                    _context.Send((s) => cmd.HideOpaqueLayer(), null);
                //                    _context.Send((s) => Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm), null);
                //                }
                //            }
                //            break;
                //    }
                //    #endregion
                //});
                #endregion
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("诊间预约获取医生错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 医生可挂号日期

        /// <summary>
        ///  医生排班日期
        /// </summary>
        public void DoctorSchedulings(string hospitalId,string deptId, string doctorId)
        {
            try
            {
                cmd.ShowOpaqueLayer(225, false);
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("hospitalId", hospitalId);
                prament.Add("deptId", deptId);//科室主键
                prament.Add("doctorId", doctorId);//医生主键
                prament.Add("type", "1");//类型：0公开预约号源、1诊间预约号源
                RestSharpHelper.ReturnResult<List<string>>("itf/booking/findByDeptAndDoctor", prament, Method.POST,
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
                                   List<Xr.RtManager.Pages.triage.SpotBookingForm.AvaDateEntity> list = objT["result"].ToObject<List<Xr.RtManager.Pages.triage.SpotBookingForm.AvaDateEntity>>();
                                   Dictionary<int, String> dc1 = new Dictionary<int, String>();
                                   if (list.Count == 0)
                                   {
                                       _context.Send((s) => cmd.HideOpaqueLayer(), null);
                                       _context.Send((s) => Xr.Common.MessageBoxUtils.Show("当前选择医生没有排班日期", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MainForm), null);
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
                                   _context.Send((s) => cmd.HideOpaqueLayer(), null);
                               }
                               else
                               {
                                   _context.Send((s) => cmd.HideOpaqueLayer(), null);
                                   _context.Send((s) => Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm), null);
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
        public void TimeScheduling(string hospitalId,string deptId,string doctorId, string time)
        {
            try
            {
                cmd.ShowOpaqueLayer(225, false);
                list = new List<string>();
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("hospitalId", hospitalId);//医院主键
                prament.Add("deptId", deptId);//科室主键
                prament.Add("doctorId", doctorId);//医生主键
                prament.Add("workDate", time);//排班日期
                prament.Add("type", "1");//类型：0公开预约号源、1诊间预约号源
                RestSharpHelper.ReturnResult<List<string>>("itf/booking/findTimeNum", prament, Method.POST,
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
                                    List<Xr.Common.Controls.Item> listitem = new List<Xr.Common.Controls.Item>();
                                    foreach (var item in timenum)
                                    {
                                        Xr.Common.Controls.Item it = new Xr.Common.Controls.Item();
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
                                    _context.Send((s) => this.mcTimeSpan.setDataSource(listitem), null);
                                    _context.Send((s) => cmd.HideOpaqueLayer(), null);
                                }
                                else
                                {
                                    _context.Send((s) => cmd.HideOpaqueLayer(), null);
                                    _context.Send((s) => Xr.Common.MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm), null);
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
        #region 帮助类
        /// <summary>
        /// 时间段
        /// </summary>
        public class TimeNum
        {
            public string id { get; set; }
            public string period { get; set; }
            public string periodName { get; set; }
            public string beginTime { get; set; }
            public string endTime { get; set; }
            public string num { get; set; }
            public String mzType { get; set; }
        }
        public class DoctorScheduling
        {
            public String id { get; set; }
            public String name { get; set; }
        }
        public String CardID { get; set; }
        public String CardType { get; set; }
        #endregion
        #region 读诊疗卡
        /// <summary>
        /// 读诊疗卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_zlk_Click(object sender, EventArgs e)
        {
            lueCardTypeQuery.EditValue = "2";
            if (txt_cardNoQuery.Text != String.Empty)
            {
               string CardID = txt_cardNoQuery.Text.Trim();
                GetPatientInfo(CardID, "2");
                txt_cardNoQuery.Text = String.Empty;
            }
            txt_cardNoQuery.Focus();
            lueCardTypeQuery.EditValue = 1;
        }
        #endregion
        #region 查询患者信息
        public string Patientid { get; set; }
        public void GetPatientInfo(string cardId,string cardType)
        {
            try
            {
                cmd.ShowOpaqueLayer(225, false);
                if (cardId != String.Empty)
                {
                    String param = "";
                    Dictionary<string, string> prament = new Dictionary<string, string>();
                    prament.Add("cardNo", cardId);
                    prament.Add("cardType", cardType);
                    if (prament.Count != 0)
                    {
                        param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                    }
                    String url = AppContext.AppConfig.serverUrl + "patmi/findPatMiByTyptAndCardNo?" + param;
                    String jsonStr = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(jsonStr);

                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        Patientid = objT["result"]["patientId"].ToString();
                        lab_patientName.Text = objT["result"]["patientName"].ToString();
                        //lab_name.Text = objT["result"]["age"].ToString();
                        lab_tel.Text = objT["result"]["phone"].ToString();

                        if (objT["result"]["sex"].ToString() == "男")
                        {
                            rBtn_male.Checked = true;
                        }
                        else
                        {
                            rBtn_female.Checked = true;
                        }
                        lueCardType.EditValue = CardType;
                        if (CardType == "1")//患者ID
                        {
                            lab_cardID.Text = objT["result"]["patientId"].ToString();
                        }
                        else if (CardType == "2")//诊疗卡
                        {
                            lab_cardID.Text = objT["result"]["zlk"].ToString();
                        }
                        else if (CardType == "3")//社保卡
                        {
                            lab_cardID.Text = objT["result"]["sbk"].ToString();
                        }
                        else if (CardType == "4")//身份证
                        {
                            lab_cardID.Text = objT["result"]["sfz"].ToString();
                        }
                        else if (CardType == "5")//健康卡
                        {
                            lab_cardID.Text = objT["result"]["jkt"].ToString();
                        }
                        else if (CardType == "6")//健康虚拟卡
                        {
                            lab_cardID.Text = objT["result"]["jktXnk"].ToString();
                        }
                        cmd.HideOpaqueLayer();
                    }
                    else
                    {
                        cmd.HideOpaqueLayer();
                        if (objT["message"].ToString() == "未匹配到患者信息")
                        {
                            MessageBoxUtils.Show("没有查询到基本信息，请去办卡", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        }
                        else
                        {
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        }
                    }
                    CardID = String.Empty;
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("诊间预约查询患者错误信息："+ex.Message);
            }
        }
        #endregion
        #region 确认预约
        /// <summary>
        /// 确认预约
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reservation_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.ShowOpaqueLayer(225, false);
                Dictionary<string, string> prament = new Dictionary<string, string>();
                #region
                prament.Add("scheduPlanId", YuYueId);//排班记录主键
                prament.Add("patientId", Patientid);//患者主键
                prament.Add("patientName", lab_patientName.Text.Trim());//患者姓名
                prament.Add("cardType", lueCardType.EditValue.ToString());//卡类型
                prament.Add("cardNo", lab_cardID.Text.Trim());//卡号
                prament.Add("tempPhone", lab_tel.Text.Trim());//手机号
                prament.Add("note", "");//备注
                if (rBtn_visitType0.Checked)//就诊类别：0.初诊 ，1.复诊
                {
                    prament.Add("visitType", "0");
                }
                else
                {
                    prament.Add("visitType", "1");
                }
                if (rBtn_addressType0.Checked)//地址类别：0市内、1市外  
                {
                    prament.Add("addressType", "0");
                }
                else
                {
                    prament.Add("addressType", "1");
                }
                if (rBtn_isShfzF.Checked)//术后复诊：0是、1否
                {
                    prament.Add("isShfz", "1");
                }
                else
                {
                    prament.Add("isShfz", "0");
                }
                if (rBtn_isYwzzF.Checked)//外院转诊：0是、1否
                {
                    prament.Add("isYwzz", "1");
                }
                else
                {
                    prament.Add("isYwzz", "0");
                }
                if (rBtn_isCyfzF.Checked)//出院复诊：0是、1否
                {
                    prament.Add("isCyfz", "1");
                }
                else
                {
                    prament.Add("isCyfz", "0");
                }
                if (rBtn_isMxbF.Checked)//是否慢性病：0是、1否
                {
                    prament.Add("isMxb", "1");
                }
                else
                {
                    prament.Add("isMxb", "0");
                }
                prament.Add("registerWay", "1");//预约途径：1诊间、2自助机、3公众号、4卫计局平台、5官网
                #endregion
                #region
                RestSharpHelper.ReturnResult<List<string>>("itf/booking/confirmBooking", prament, Method.POST,
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
                                    _context.Send((s) => ClearReservationUIInfo(), null);
                                    _context.Send((s) => cmd.HideOpaqueLayer(), null);
                                    _context.Send((s) => Xr.Common.MessageBoxUtils.Hint(objT["message"].ToString(), MainForm), null);
                                    _context.Send((s) => TimeScheduling(AppContext.Session.hospitalId, SelectDeptid, DoctorId, GetDateTime), null);
                                    _context.Send((s) =>  GetDoctorPatientInfo(SelectDeptid, DoctorId, GetDateTime, GetDateTime), null);
                                }
                                else
                                {
                                    _context.Send((s) => ClearReservationUIInfo(), null);
                                    _context.Send((s) => cmd.HideOpaqueLayer(), null);
                                    _context.Send((s) => Xr.Common.MessageBoxUtils.Hint(objT["message"].ToString(), HintMessageBoxIcon.Error, MainForm), null);
                                }
                            }
                            break;
                    }
                });
                #endregion
            }
            catch (Exception ex)
            {
                _context.Send((s) => cmd.HideOpaqueLayer(), null);
                Xr.Common.MessageBoxUtils.Show("确认预约错误信息" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,MainForm);
                Log4net.LogHelper.Error("确认预约错误信息：" + ex.Message);
            }
        }
        void ClearReservationUIInfo()
        {
            lab_patientName.Text = String.Empty;
            lab_cardID.Text = String.Empty;
            lueCardType.ItemIndex = 0;
            lueCardType.Enabled = false;
            lab_tel.Text = String.Empty;
            Patientid = String.Empty;
            lueNote.Text = "";
            lab_reservationDate.Text = String.Empty;
            lab_mz_typeStr.Text = String.Empty;
            lab_timespan.Text = String.Empty;
            //reservationCalendar1.ChangeValidDate(new List<Dictionary<int, String>>());
            //mcTimeSpan.setDataSource(new List<Xr.Common.Controls.Item>());
            //gc_patient.DataSource = null;
            this.btn_reservation.Enabled = false;
        }
        #endregion
        #region 文本框的回车和读卡

        private void txt_cardNoQuery_Enter_1(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate
            {
                (sender as TextBox).SelectAll();
            });
        }
        private void txt_cardNoQuery_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == (Keys.Control | Keys.V))
            //{
            //    if (Clipboard.ContainsText())
            //    {
            //        try
            //        {
            //            Convert.ToInt64(Clipboard.GetText());  //检查是否数字
            //            //((TextBox)sender).SelectedText = Clipboard.GetText().Trim(); //Ctrl+V 粘贴  
            //            //光标处插入
            //            string s = txt_cardNoQuery.Text.Trim();
            //            int idx = txt_cardNoQuery.SelectionStart;
            //            s = s.Insert(idx, Clipboard.GetText());

            //            txt_cardNoQuery.Text = s;
            //            txt_cardNoQuery.SelectionStart = idx + Clipboard.GetText().Length;
            //            txt_cardNoQuery.Focus();

            //        }
            //        catch (Exception)
            //        {
            //            e.Handled = true;
            //            //throw;
            //        }
            //    }
            //}
            ////允许Ctrl+A实现全选
            //else if (e.KeyData == (Keys.Control | Keys.A))
            //{
            //    ((TextBox)sender).SelectAll();
            //}
            ////允许Ctrl+C实现复制
            //else if (e.KeyData == (Keys.Control | Keys.C))
            //{
            //    if (txt_cardNoQuery.SelectedText != "")
            //        Clipboard.SetDataObject(txt_cardNoQuery.SelectedText);
            //}
            if (txt_cardNoQuery.Text.Trim() != String.Empty)
            {
                if (e.KeyCode == Keys.Control || e.KeyCode == Keys.Enter)
                {
                    CardType = lueCardTypeQuery.EditValue.ToString();
                    CardID = txt_cardNoQuery.Text.Trim();
                    GetPatientInfo(CardID, lueCardTypeQuery.EditValue.ToString());
                    txt_cardNoQuery.Text = String.Empty;
                    //gc_patient.Focus();
                    txt_cardNoQuery.Focus();
                }
            }
        }
        /// <summary>
        /// 读社保卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_readSocialCard_Click(object sender, EventArgs e)
        {
            cmd.IsShowCancelBtn = true;
            cmd.ShowOpaqueLayer(0.56f, "正在读取...");
            SocialCard carMes = new SocialCard();
            carMes.readCard();
            if (carMes.message_type == "1")
            {
                carMes.cancelReadCard();
                GetPatientInfo(carMes.user_id, "3");
            }
        }
        /// <summary>
        /// 读身份证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_readIdcard_Click(object sender, EventArgs e)
        {
            cmd.IsShowCancelBtn = true;
            cmd.ShowOpaqueLayer(0.56f, "正在读取...");
            JLIdCardInfoClass idCardInfo = JLIdCardInfoClass.getCardInfo();
            if (idCardInfo != null)
            {
                CardID = idCardInfo.Code.ToString();
                GetPatientInfo(CardID, "4");
                JLIdCardInfoClass.CancelFlag = true;
            }
        }
        #endregion
        #region 科室列表点击事件
        /// <summary>
        /// 选中的科室ID
        /// </summary>
        public String SelectDeptid { get; set; }
        /// <summary>
        /// 科室列表点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="selectItem"></param>
        private void treeMenuControl1_MenuItemClick(object sender, EventArgs e, object selectItem)
        {
            DeptEntity dept = selectItem as DeptEntity;
            SelectDeptid = dept.id;
            ClearReservationUIInfo();
            List<Dictionary<int, String>> dcs = new List<Dictionary<int, String>>();
            reservationCalendar1.ChangeValidDate(dcs);
            this.mcTimeSpan.setDataSources(null, true, "");
            GetDoctorName(AppContext.Session.hospitalId, SelectDeptid);
        }
        #endregion
        #region 医生列表点击事件
        /// <summary>
        /// 选中的医生ID
        /// </summary>
        public String DoctorId { get; set; }
        /// <summary>
        /// 记录ID
        /// </summary>
        public String YuYueId { get; set; } 
        private void mcDoctor_MenuItemClick(object sender, EventArgs e)
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
            lab_timespan.Text = "";
            lab_reservationDate.Text = "";
            List<Xr.Common.Controls.Item> item = new List<Xr.Common.Controls.Item>();
            this.mcTimeSpan.setDataSource(item);
            DoctorSchedulings(AppContext.Session.hospitalId, SelectDeptid,label.Tag.ToString());
            gc_patient.DataSource = null;
        }
        #endregion
        #region  日期控件点击事件
        public string GetDateTime { get; set; }
        private void reservationCalendar1_SelectDate(DateTime SelectedDate)
        {
            lab_reservationDate.Text = SelectedDate.ToString("yyyy-MM-dd");
            lab_timespan.Text = "";
            GetDateTime = SelectedDate.ToString("yyyy-MM-dd");
            TimeScheduling(AppContext.Session.hospitalId,SelectDeptid,DoctorId,SelectedDate.ToString("yyyy-MM-dd"));
            gc_patient.DataSource = null;
            GetDoctorPatientInfo(SelectDeptid, DoctorId, SelectedDate.ToString("yyyy-MM-dd"), SelectedDate.ToString("yyyy-MM-dd"));
        }

        private void reservationCalendar1_ChangeMonth(DateTime SelectedMonth)
        {
            lab_reservationDate.Text = "";
            lab_timespan.Text = "";
            this.mcTimeSpan.setDataSources(null, true, "");
        }
        #endregion
        #region 号源列表点击事件
        private void mcTimeSpan_MenuItemClick(object sender, EventArgs e)
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
            lab_timespan.Text = label.Text.Trim().Substring(0, 11);
            YuYueId = label.Name.ToString();
            if (label.Tag.ToString().Split('#')[1] == "2")
            {
                lab_mz_typeStr.Text = "特需门诊";
            }
            else
            {
                lab_mz_typeStr.Text = "普通门诊";
            }
            this.btn_reservation.Enabled = true;
        }
        #endregion
        #region 该医生预约名单
        public void GetDoctorPatientInfo(string deptId, string doctorId, string beginDate, string endDate)
        {
            try
            {
                 String param = "";
                //获取候诊患者列表
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("hospitalId", AppContext.Session.hospitalId);
                prament.Add("deptId", deptId);
                prament.Add("doctorId", doctorId);
                prament.Add("beginDate", beginDate);
                prament.Add("endDate", endDate);
                prament.Add("registerWay", "1");
                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/doctorScheduRegister/findRegister?" + param;
                String jsonStr = HttpClass.httpPost(url);

                JObject objT = JObject.Parse(jsonStr);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<PatientListEntity> list = objT["result"].ToObject<List<PatientListEntity>>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].cardType.ToString() != "")
                        {
                            list[i].cardType = ListCardType.FirstOrDefault(s => s.value == list[i].cardType).label;
                        }
                    }
                    gc_patient.DataSource = list;
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("该医生预约名单错误信息："+ex.Message);
            }
        }
        /// <summary>
        ///  患者列表实体
        /// </summary>
        public class PatientListEntity
        {
            /// <summary>
            /// 预约主键
            /// </summary>
            public String id { get; set; }
            public String cardNo { get; set; }
            public String cardType { get; set; }
            public String patientId { get; set; }
            /// <summary>
            /// 患者姓名
            /// </summary>
            public String patientName { get; set; }
            /// <summary>
            /// 性别
            /// </summary>
            public String sex { get; set; }
            /// <summary>
            /// 预约就诊日期
            /// </summary>
            public String workDate { get; set; }
            /// <summary>
            /// 周几：周一、周二...
            /// </summary>
            public String week { get; set; }
            /// <summary>
            /// 时间，号源所在的时间段beginTime-endTime,如：08:00-08:30
            /// </summary>
            public String time { get; set; }
            /// <summary>
            /// 科室名称
            /// </summary>
            public String deptName { get; set; }
            /// <summary>
            /// 医生姓名
            /// </summary>
            public String doctorName { get; set; }
            /// <summary>
            /// 预约状态
            /// </summary>
            public String status { get; set; }


            /// <summary>
            /// 预约途径
            /// </summary>
            public String registerWay { get; set; }
            /// <summary>
            /// 就诊类别,初诊、复诊
            /// </summary>
            public String visitType { get; set; }
            /// <summary>
            /// 术后复诊
            /// </summary>
            public String isShfz { get; set; }
            /// <summary>
            /// 出院复诊
            /// </summary>
            public String isCyfz { get; set; }
            /// <summary>
            /// 院外转诊
            /// </summary>
            public String isYwzz { get; set; }
            /// <summary>
            /// 就诊时间
            /// </summary>
            public String registerTime { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public String note { get; set; }
            /// <summary>
            /// 年龄：30岁1月
            /// </summary>
            public String age { get; set; }
            /// <summary>
            /// 联系电话
            /// </summary>
            public String tempPhone { get; set; }
            /// <summary>
            /// 联系地址
            /// </summary>
            public String address { get; set; }
            /// <summary>
            /// 预约状态Txt
            /// </summary>
            public String statusTxt { get; set; }
            /// <summary>
            /// 预约途径TxT
            /// </summary>
            public String registerWayTxt { get; set; }
            private String _visitTypeTxt;
            /// <summary>
            /// 就诊类别TxT,就诊类别：0.初诊 ，1.复诊
            /// </summary>
            public String visitTypeTxt
            {
                get
                {
                    if (_visitTypeTxt == "0")
                    {
                        return "初诊";
                    }
                    else
                    {
                        return "复诊";
                    }
                }

                set
                {
                    _visitTypeTxt = value;
                }
            }
        }
        #endregion
        #region 取消预约
        private void 加急ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBoxUtils.Show("确定为该患者取消预约吗?", MessageBoxButtons.OKCancel,
               MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                //PatientListEntity
                var selectedRow = this.gv_patient.GetFocusedRow() as PatientListEntity;
                if (selectedRow == null)
                    return;
                String param = "";
                //请求取消预约
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("registerId", selectedRow.id);
                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/doctorScheduRegister/cancelBooking?" + param;
                String jsonStr = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(jsonStr);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    MessageBoxUtils.Hint(objT["message"].ToString(), MainForm);
                    GetDoctorPatientInfo(SelectDeptid, DoctorId, GetDateTime, GetDateTime);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
                ////WorkType = AsynchronousWorks.CancelWaiting;
                //if (RegisterId != null && RegisterId != String.Empty)
                //    Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.CancelReservation, Argument = new String[] { RegisterId } });
                //else
                //    MessageBoxUtils.Hint("该患者尚未分诊", HintMessageBoxIcon.Error, MainForm);
            }
        }
        #endregion
    }
}
