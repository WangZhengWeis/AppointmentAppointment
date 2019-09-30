using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using System.IO;
using System.Drawing.Printing;
using System.ComponentModel;
using System.Windows.Threading;
using System.Net;
using System.Drawing.Imaging;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading;
using Xr.RtManager.Module.triage;

namespace Xr.RtManager
{
    class HardwareInitialClass
    {
        #region  T6接口函数
        //public class cardMessage
        //{
        //    public string message_type;	         /* 信息类型 固定值为1，为了与返回结果信息区别 */
        //    public string user_id;                      /* 参保号 */
        //    public string company_code;         /*  单位编号*/
        //    public string holder_id;                /*  身份证号  */
        //    public string name;               /*  姓名  */
        //    public string sex;

        //    public string birthday;                   /*  出生年月 */
        //    public string user_type;                /*  人员类别 */
        //    //00-在职，10-退休，11-离休。
        //    //   21-机关单位离休，22-企事业单位离休，23-残疾军人离休
        //    public string telephone;               /*  电话号码 */
        //    public string blood_type;                  /*  卡种 综合--0  住院--1   特殊病种--2*/
        //    public string ill_history;             /* 交易时间 年4月2日2时2分2秒2  */
        //    public string h_ill_history;            /*  过敏史 */
        //    public string address1;            /* 通讯地址 */
        //    public string post_id;                 /* 邮政编码 */
        //    public string linkman;                /*  联系人 */
        //    public string area_code;                  /*  社保统筹年度 */
        //    public string cs_amount;              /*   公务员年度统筹金额统筹金额   */
        //    public string sb_amount;              /*   个人医疗帐户金额 */
        //    public string sp_amount;        /*   年度特殊病种金额 */
        //    public string zfu_amount;              /*   年度自付累计金额 */
        //    public string card;         /* 卡号 */

        //    public void byteToCardMessage(byte[] cardMeassageByte,cardMessage mes)
        //    {
        //        mes.message_type = System.Text.Encoding.ASCII.GetString(cardMeassageByte, 0, 1).Trim();
        //        mes.user_id = System.Text.Encoding.Default.GetString(cardMeassageByte, 1, 8).Trim();
        //        mes.company_code = System.Text.Encoding.Default.GetString(cardMeassageByte, 9, 15).Trim();
              
        //        mes.holder_id = System.Text.Encoding.Default.GetString(cardMeassageByte, 24, 19).Trim();
        //        mes.name = System.Text.Encoding.Default.GetString(cardMeassageByte, 43, 10).Trim();
        //        if (System.Text.Encoding.Default.GetString(cardMeassageByte, 53, 1).Trim() == "0")
        //            mes.sex = "1";
        //        else
        //            mes.sex = "2";
        //        mes.user_id = System.Text.Encoding.Default.GetString(cardMeassageByte, 1, 8).Trim();
        //        mes.birthday = System.Text.Encoding.Default.GetString(cardMeassageByte, 54, 8).Trim();
        //        mes.user_type = System.Text.Encoding.Default.GetString(cardMeassageByte, 62, 2).Trim();
        //        mes.telephone = System.Text.Encoding.Default.GetString(cardMeassageByte, 64, 12).Trim();
        //        mes.blood_type = System.Text.Encoding.Default.GetString(cardMeassageByte, 76, 1).Trim();
        //        mes.ill_history = System.Text.Encoding.Default.GetString(cardMeassageByte, 77, 14).Trim();
        //        mes.h_ill_history = System.Text.Encoding.Default.GetString(cardMeassageByte, 91, 12).Trim();
        //        mes.address1 = System.Text.Encoding.Default.GetString(cardMeassageByte, 103, 40).Trim();
        //        mes.post_id = System.Text.Encoding.Default.GetString(cardMeassageByte, 143, 6).Trim();
        //        mes.linkman = System.Text.Encoding.Default.GetString(cardMeassageByte, 149, 10).Trim();
        //        mes.area_code = System.Text.Encoding.Default.GetString(cardMeassageByte, 159, 2).Trim();
        //        mes.cs_amount = System.Text.Encoding.Default.GetString(cardMeassageByte, 161, 12).Trim();
        //        mes.sb_amount = System.Text.Encoding.Default.GetString(cardMeassageByte, 173, 12).Trim();
        //        mes.sp_amount = System.Text.Encoding.Default.GetString(cardMeassageByte, 185, 12).Trim();
        //        mes.zfu_amount = System.Text.Encoding.Default.GetString(cardMeassageByte, 197, 12).Trim();
        //        mes.card = System.Text.Encoding.Default.GetString(cardMeassageByte, 209, 16).Trim();
             
        //    }
        //}

        //所有函数0成功  非0失败
        [DllImport("BACRW.dll")] //打开设备
        public static extern int OpenDevice();
        //打开设备， 读卡前要行打开设备，只需要调用一次。

        [DllImport("BACRW.dll")] //关闭设备
        public static extern int CloseDevice();
        //关闭设备， 程序退出需要调用关闭设备。

        [DllImport("BACRW.dll")] //读社保卡信息
        public static extern int GetPosInfo(Byte[] psMsg);
        //读社保卡信息，返回结果详见 <<POS与PC接口文档格式.doc>>

        [DllImport("BACRW.dll")] //打开设备
        public static extern int SendPosInfo(StringBuilder psFeeMsg);
        //社保卡扣费信息，具体参数详见 <<POS与PC接口文档格式.doc>>
//在交易成功后在返回结果”30”后面增加“|”加6位流水号，用于消费撤消。返回结果放回psFeeMsg。

        [DllImport("BACRW.dll")] //圈存
        public static extern int IcLoad(StringBuilder psMsg);
        //社保卡圈存 返回　”前余额|圈存金额|后余额”

        [DllImport("BACRW.dll")] //消费撤销
        public static extern int SaleVoid(StringBuilder psRocNo);
        //消费撤消，psRocNo = 原交易流水号6位, 从SendPosInfo成功后返回，只能撤消当天的扣费交易。

        [DllImport("BACRW.dll")] 
        public static extern int ResetDev();
//设备复位，对多次读卡不成功，可以复位处理。

        [DllImport("BACRW.dll")]
        public static extern int CancelAccept();
//取消等待插卡，系统默认等待插卡时间为30秒，调用后马上返回-1,表示原请求交易已取消。

        [DllImport("BACRW.dll")]
        public static extern int GetErrorMsg(StringBuilder psErrMsg);
//读交易失败的错误信息。
        #endregion

        #region 中山医保
        [DllImport("HNHISBridge.dll", CallingConvention = CallingConvention.StdCall)] //打开连接	HRESULT 1表示成功；-11表示系统初始化失败。
        public static extern Int32 Initialize(string svrIP, string hsBH, string odbcName, Int32 svrPort, string dbName, string username, string passwd);


        [DllImport("HNHISBridge.dll")] //释放资源
        public static extern Int32 Release();

        [DllImport("HNHISBridge.dll")] //创建一个功能调用实例。在进行一个新的功能调用前必须执行该操作，以取得调用的处理句柄。返回的句柄将成为其他功能调用的入口参数。
        public static extern IntPtr CreateInstace();

        [DllImport("HNHISBridge.dll")] //释放调用实例
        public static extern Int32 DestroyInstance(IntPtr pDataHandle);

        [DllImport("HNHISBridge.dll")] //提供功能调用的参数组，比如功能号以及其他功能的调用参数。（功能号的paramName规定为“FN”）
        public static extern Int32 SetParam(IntPtr pDataHandle, string paramName, string paramValue);


        [DllImport("HNHISBridge.dll")] //插入一个数据集,为装入数据作准备。
        public static extern Int32 InsertDataSet(IntPtr pDataHandle);

        [DllImport("HNHISBridge.dll")] //插入一行,为装入一行数据作准备。
        public static extern Int32 InsertRow(IntPtr pDataHandle);

        [DllImport("HNHISBridge.dll")] //结束当前行，把当前行插入到当前的数据集中。
        public static extern Int32 EndRow(IntPtr pDataHandle, Int32 rowID);

        [DllImport("HNHISBridge.dll")] //在当前行插入一列。
        public static extern Int32 SetField(IntPtr pDataHandle, string fieldName, string fieldValue);

        [DllImport("HNHISBridge.dll")] //结束当前数据集，把当前数据集插入到数据包中。（数据集名称规定具体请参考第三章的各功能规定）
        public static extern Int32 EndDataSet(IntPtr pDataHandle, string name);

        [DllImport("HNHISBridge.dll")] //运行调用实例。
        public static extern Int32 RunInstance(IntPtr pDataHandle);

        [DllImport("HNHISBridge.dll")] //获取错误信息
        public static extern Int32 GetSysMessage(IntPtr pDataHandle, StringBuilder pMessage, Int32 nMaxMessage);

        [DllImport("HNHISBridge.dll")] //	该接口函数用于取返回参数。
        public static extern Int32 GetParam(IntPtr pDataHandle, string paramName, StringBuilder ParamValue, Int32 nMaxValueLenth);

        [DllImport("HNHISBridge.dll", EntryPoint = "GetParam")] //	该接口函数用于取返回参数。
        public static extern Int32 GetParamS(IntPtr pDataHandle, string paramName, ref char[] ParamValue, Int32 nMaxValueLenth);

        [DllImport("HNHISBridge.dll")] //	定位返回的数据集。
        public static extern Int32 LocateDataSet(IntPtr pDataHandle, string name);

        [DllImport("HNHISBridge.dll")] //		该接口函数用于取当前数据集的数据行数。
        public static extern Int32 GetRowSize(IntPtr pDataHandle);

        [DllImport("HNHISBridge.dll")] //		该接口函数用于取当前数据集的数据列数。
        public static extern Int32 GetColSize(IntPtr pDataHandle);

        [DllImport("HNHISBridge.dll")] //	得到当前行的指定列的值。
        public static extern Int32 GetFieldValue(IntPtr pDataHandle, string name, StringBuilder value, Int32 nMaxValueLenth);

        [DllImport("HNHISBridge.dll")] //	将当前数据集的指针移到下一行。
        public static extern Int32 NextRow(IntPtr pDataHandle);

        [DllImport("HNHISBridge.dll")] //	得到当前行的实际行号。
        public static extern Int32 GetCurrentRow(IntPtr pDataHandle);
        public static string H_GetSysMessage(IntPtr ll)
        {
            StringBuilder mes=new StringBuilder();
            GetSysMessage(ll,mes,300);
            return mes.ToString();
        }
        public static Int32 H_Initialize()
        {
            string ls_svrIP = "192.168.0.22";   //前置机ＩＰ地址。
            String ls_hsBH = "H003";            //医疗机构编号。（由社保局统一分发）。
            String ls_odbcName = "yyqzj";       //客户机设置的ODBC数据源名称
            Int32 ll_port = 9990;               //Socket服务器的监听端口
            String ls_dbName = "yyqzj";         //登陆的数据库用户名,如login_数据库用户名。
            String ls_name = "yyqzj";           //系统操作员用户名。
            String ls_password = "yyqzj";       //系统操作员用户口令。


            return Initialize(ls_svrIP, ls_hsBH, ls_odbcName, ll_port, ls_dbName, ls_name, ls_password);
        }

        public static IntPtr H_CreateInstace()
        {
            return CreateInstace();
        }

        public static Int32 H_DestroyInstance(IntPtr ll_handle)
        {
            return DestroyInstance(ll_handle);
        }
        /// <summary>
        /// 查询2078单项内容
        /// </summary>
        /// <param name="ll_handle"></param>
        /// <param name="GRSXH">个人参保号</param>
        /// <param name="JZRQ">就诊日期 缴费列表happenDate YYYYMMDD</param>
        ///  <param name="return_What">返回的列名</param>
        ///  <param name="return_date">返回的参数</param>
        /// <returns>1为成功</returns>
        public static Int32 H_Function2078(string GRSXH, string JZRQ, string return_what, StringBuilder return_date)
        {
            //将单项数据传入调用实例
            if (JZRQ.IndexOf("-") >= 0) JZRQ=JZRQ.Replace("-","");
            IntPtr ll_handle = HardwareInitialClass.H_CreateInstace();
            Int32 ll_rtn;
            return_date = new StringBuilder(1024);
            Xr.Log4net.LogHelper.Info("2078GRSXH:" + GRSXH);
            Xr.Log4net.LogHelper.Info("2078JZRQ:" + JZRQ);
            ll_rtn = SetParam(ll_handle, "FN", "2078"); //设置功能号  
            ll_rtn = SetParam(ll_handle, "GRSXH", GRSXH); //
            if (JZRQ != null && JZRQ.Length > 8)
            {
                ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ.Substring(0, 8)); //
            }
            else
            {
                ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ); //
            }
            ll_rtn = SetParam(ll_handle, "JYLB", "1"); //
            ll_rtn = RunInstance(ll_handle);

            ll_rtn = GetParam(ll_handle, return_what.ToString(), return_date, 1024); //返回值
            HardwareInitialClass.H_DestroyInstance(ll_handle);
            return ll_rtn;

        }

        public static Int32 H_Function1005HosipitalBoAi(string GRSXH,  StringBuilder result)
        {
            //将单项数据传入调用实例
            IntPtr ll_handle = HardwareInitialClass.H_CreateInstace();
            result.Append("0");
            Int32 ll_rtn;
            ll_rtn = SetParam(ll_handle, "FN", "1005"); //设置功能号  
            ll_rtn = SetParam(ll_handle, "GRSXH", GRSXH); //

            ll_rtn = RunInstance(ll_handle);

            ll_rtn = LocateDataSet(ll_handle, "CJRYMXCX");
            //取数据集的数据行数
            Int32 ll_rowsize = GetRowSize(ll_handle);

            if (ll_rowsize > 0)
            {
                StringBuilder CJDDYY = new StringBuilder();
                for (int i = 0; i < ll_rowsize; i++)
                {
                    GetFieldValue(ll_handle, " CJDDYY ", CJDDYY, 50);
                    if (CJDDYY.ToString().IndexOf("博爱") >= 0)
                    {
                        result.Append("1");
                        break;
                    }
                }
            }
            HardwareInitialClass.DestroyInstance(ll_handle);
            return ll_rtn;

        }

        public static Int32 H_Function2085(string GRSXH, string JZRQ, string return_what, StringBuilder return_date)
        {
            //将单项数据传入调用实例
            Int32 ll_rtn;
            IntPtr ll_handle = HardwareInitialClass.H_CreateInstace();
            ll_rtn = SetParam(ll_handle, "FN", "2085"); //设置功能号  
            ll_rtn = SetParam(ll_handle, "GRSXH", GRSXH); //
            if (JZRQ != null && JZRQ.Length > 8)
            {
                ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ.Substring(0, 8)); //
            }
            else
            {
                ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ); //
            }
            ll_rtn = RunInstance(ll_handle);

            ll_rtn = GetParam(ll_handle, return_what.ToString(), return_date, 1024); //返回值
            HardwareInitialClass.H_DestroyInstance(ll_handle);
            return ll_rtn;

        }

        public static Int32 H_Function2091(string GRSXH, string JZRQ, string return_what, StringBuilder return_date)
        {
            //将单项数据传入调用实例
            IntPtr ll_handle = HardwareInitialClass.H_CreateInstace();
            Int32 ll_rtn;
            ll_rtn = SetParam(ll_handle, "FN", "2091"); //设置功能号  
            ll_rtn = SetParam(ll_handle, "GRSXH", GRSXH); //
            if (JZRQ != null && JZRQ.Length > 8)
            {
                ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ.Substring(0, 8)); //
            }
            else
            {
                ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ); //
            }
            
            ll_rtn = RunInstance(ll_handle);

            ll_rtn = GetParam(ll_handle, return_what.ToString(), return_date, 1024); //返回值
            HardwareInitialClass.H_DestroyInstance(ll_handle);
            return ll_rtn;

        }

        //普通结算
        public static Int32 H_Function2071(string JZRQ, string ZYH, string JZZD,
            string JZZDGJDM,string ZZYSXM,
            SocialCard cadMes, List<JObject> date, StringBuilder gezhang, StringBuilder xianjin, decimal sb_amount, StringBuilder GWYTCZF, JObject sameNoListReturn, bool isSure, StringBuilder wrMes)
        {
            //将单项数据传入调用实例
            IntPtr ll_handle = HardwareInitialClass.H_CreateInstace();
            Int32 ll_rtn;
            ll_rtn = SetParam(ll_handle, "FN", "2071"); //设置功能号  
            ll_rtn = SetParam(ll_handle, "GMSFHM", cadMes.holder_id); //
            if (isSure)
                ll_rtn = SetParam(ll_handle, "JSBZ", "1"); //
            else
                ll_rtn = SetParam(ll_handle, "JSBZ", "2"); //
            ll_rtn = SetParam(ll_handle, "GRSXH", cadMes.user_id); //
            ll_rtn = SetParam(ll_handle, "JZLB", cadMes.blood_type); //
            ll_rtn = SetParam(ll_handle, "ZYH", ZYH); //
            ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ); //

            StringBuilder BZLB = new StringBuilder();
            H_Function2091(cadMes.user_id, JZRQ, "DJBZ", BZLB);
            ll_rtn = SetParam(ll_handle, "BZLB", BZLB.ToString()); //

            ll_rtn = SetParam(ll_handle, "JZZD", JZZD); //
            ll_rtn = SetParam(ll_handle, "JZZDGJDM", JZZDGJDM); //
            ll_rtn = SetParam(ll_handle, "ZZYSXM", ZZYSXM); //
            ll_rtn = SetParam(ll_handle, "YBZHYE", sb_amount.ToString()); //
            ll_rtn = SetParam(ll_handle, "GRZFLJ", ""); //
            ll_rtn = SetParam(ll_handle, "ND", cadMes.area_code); //
            ll_rtn = SetParam(ll_handle, "RYLB", cadMes.user_type); //
            ll_rtn = SetParam(ll_handle, "KH", cadMes.card); //
            ll_rtn = SetParam(ll_handle, "JYM", cadMes.linkman); //
            ll_rtn = SetParam(ll_handle, "PSAM", ""); //
           
            ll_rtn =	InsertDataSet(ll_handle);

            for (int i = 0; i < date.Count; i++)
            {
                ll_rtn = InsertRow(ll_handle);    //创建一行
                string XMXH = date[i]["XMXH"].ToString();
                string KZRQ=date[i]["KZRQ"].ToString().Replace("-", "").Replace(":", "").Replace(" ",""); 
                string YYXMBH = date[i]["YYXMBH"].ToString();
                string YYXMMC = date[i]["YYXMMC"].ToString();
                string YZLB = date[i]["YZLB"].ToString();
                string JG = date[i]["JG"].ToString();
                string MCYL = date[i]["MCYL"].ToString();
                string JE = date[i]["JE"].ToString();
                string XZYYBZ = date[i]["XZYYBZ"].ToString();
              

                ll_rtn = SetField(ll_handle, "XMXH",XMXH );
                ll_rtn = SetField(ll_handle, "KZRQ", KZRQ);
                ll_rtn = SetField(ll_handle, "TZRQ", "");
                ll_rtn = SetField(ll_handle, "YYXMBH", YYXMBH);
                ll_rtn = SetField(ll_handle, "YYXMMC", YYXMMC);
                ll_rtn = SetField(ll_handle, "SPMC", "");
                ll_rtn = SetField(ll_handle, "YPGG", "");
                ll_rtn = SetField(ll_handle, "YPJX", "");
                ll_rtn = SetField(ll_handle, "YPYF", "");
                ll_rtn = SetField(ll_handle, "YPJL", "");
                ll_rtn = SetField(ll_handle, "YZLB",YZLB );
                ll_rtn = SetField(ll_handle, "JG", JG);
              
                ll_rtn = SetField(ll_handle, "MCYL",MCYL );
                ll_rtn = SetField(ll_handle, "JE",JE );
                ll_rtn = SetField(ll_handle, "YPLY", "");
                ll_rtn = SetField(ll_handle, "TSYPBZ", "");
                ll_rtn = SetField(ll_handle, "XZYYBZ", XZYYBZ);
                ll_rtn = SetField(ll_handle, "SJGYYY", "");

                ll_rtn = SetField(ll_handle, "DLGG", "");
                ll_rtn = SetField(ll_handle, "DWGG", "");
                ll_rtn = SetField(ll_handle, "SYTS", "");
                ll_rtn = SetField(ll_handle, "YJJYPBM", "");
                ll_rtn = SetField(ll_handle, "YYCLZCZMC", "");
                ll_rtn = SetField(ll_handle, "YYCLSYJZCH", "");
                ll_rtn = SetField(ll_handle, "TXBZ", "");
                ll_rtn = SetField(ll_handle, "BZ1", "");
                ll_rtn = SetField(ll_handle, "BZ2", "");
                ll_rtn = SetField(ll_handle, "BZ3", "");
               
                ll_rtn = EndRow(ll_handle, i+1);     //结束当前行
            }
            ll_rtn = EndDataSet(ll_handle, "MZCFXMDRML");  //结束当前数据集，赋予数据集的名称
        
            ll_rtn = RunInstance(ll_handle);
            if (ll_rtn < 0)
            {
                wrMes.Append("                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              ");
                GetSysMessage(ll_handle, wrMes, 300);
                return ll_rtn;
            }
           
            
            //出参 
            StringBuilder GZZFUJE = new StringBuilder(111);
            StringBuilder GZZFEJE = new StringBuilder(111);

            StringBuilder XJZFUJE = new StringBuilder(111);
            StringBuilder XJZFEJE = new StringBuilder(111);

            StringBuilder MZYFTCZF = new StringBuilder(111);
            StringBuilder MZDBTCZF = new StringBuilder(111);

            StringBuilder GWYTCZF_T = new StringBuilder(111);

             GetParam(ll_handle, "GZZFUJE", GZZFUJE, 1024); //返回值
             GetParam(ll_handle, "GZZFEJE", GZZFEJE, 1024); //返回值
             GetParam(ll_handle, "XJZFUJE", XJZFUJE, 1024); //返回值
             GetParam(ll_handle, "XJZFEJE", XJZFEJE, 1024); //返回值
              GetParam(ll_handle, "MZYFTCZF", MZYFTCZF, 1024); //返回值
              GetParam(ll_handle, "MZDBTCZF", MZDBTCZF, 1024); //返回值
              GetParam(ll_handle, "GWYTCZF", GWYTCZF_T, 1024); //返回值

             if (MZYFTCZF.ToString() == "") MZYFTCZF.Append("0");
             if (MZDBTCZF.ToString() == "") MZDBTCZF.Append("0");
             if (XJZFUJE.ToString() == "") XJZFUJE.Append("0");
             if (XJZFEJE.ToString() == "") XJZFEJE.Append("0");
             if (GZZFEJE.ToString() == "") GZZFEJE.Append("0");
             if (GZZFUJE.ToString() == "") GZZFUJE.Append("0");
             if (GWYTCZF.ToString() == "") GWYTCZF.Append("0");

            gezhang.Append((float.Parse(GZZFUJE.ToString()) + float.Parse(GZZFEJE.ToString())).ToString());
            xianjin.Append((float.Parse(XJZFUJE.ToString()) + float.Parse(XJZFEJE.ToString())-float.Parse(MZDBTCZF.ToString())-float.Parse(MZYFTCZF.ToString())).ToString())  ;

            StringBuilder tP=new StringBuilder(1024);
              GetParam(ll_handle, "FHZ", tP, 110); //返回值
             sameNoListReturn.Add("FHZ", tP.ToString());
              GetParam(ll_handle, "MSG", tP, 110); //返回值
             sameNoListReturn.Add("MSG", tP.ToString());
              GetParam(ll_handle, "JZJLH", tP, 110); //返回值
             sameNoListReturn.Add("JZJLH", tP.ToString());
              GetParam(ll_handle, "JSYWH", tP, 110); //返回值
             sameNoListReturn.Add("JSYWH", tP.ToString());
              GetParam(ll_handle, "YLFYZE", tP, 110); //返回值
             sameNoListReturn.Add("YLFYZE", tP.ToString());
              GetParam(ll_handle, "GZZFUJE", tP, 110); //返回值
             sameNoListReturn.Add("GZZFUJE", tP.ToString());
              GetParam(ll_handle, "GZZFEJE", tP, 110); //返回值
             sameNoListReturn.Add("GZZFEJE", tP.ToString());
              GetParam(ll_handle, "TCZFJE", tP, 110); //返回值
             sameNoListReturn.Add("TCZFJE", tP.ToString());
              GetParam(ll_handle, "GWYTCZF", tP, 110); //返回值
             sameNoListReturn.Add("GWYTCZF", tP.ToString());
              GetParam(ll_handle, "XJZFUJE", tP, 110); //返回值
             sameNoListReturn.Add("XJZFUJE", tP.ToString());
              GetParam(ll_handle, "XJZFEJE", tP, 110); //返回值
             sameNoListReturn.Add("XJZFEJE", tP.ToString());
              GetParam(ll_handle, "JSRQ", tP, 110); //返回值
             sameNoListReturn.Add("JSRQ", tP.ToString());
              GetParam(ll_handle, "RYLB", tP, 110); //返回值
             sameNoListReturn.Add("RYLB", tP.ToString());
              GetParam(ll_handle, "BCYLTCZF", tP, 110); //返回值
             sameNoListReturn.Add("BCYLTCZF", tP.ToString());
              GetParam(ll_handle, "JZLB", tP, 110); //返回值
             sameNoListReturn.Add("JZLB", tP.ToString());
              GetParam(ll_handle, "CWYY", tP, 110); //返回值
             sameNoListReturn.Add("CWYY", tP.ToString());
              GetParam(ll_handle, "YFDXLB", tP, 110); //返回值
             sameNoListReturn.Add("YFDXLB", tP.ToString());
              GetParam(ll_handle, "MZYFTCZF", tP, 110); //返回值
             sameNoListReturn.Add("MZYFTCZF", tP.ToString());
              GetParam(ll_handle, "MZYFLJJE", tP, 110); //返回值
             sameNoListReturn.Add("MZYFLJJE", tP.ToString());

              GetParam(ll_handle, "DBDXLB", tP, 110); //返回值
             sameNoListReturn.Add("DBDXLB", tP.ToString());
              GetParam(ll_handle, "MZDBTCZF", tP, 110); //返回值
             sameNoListReturn.Add("MZDBTCZF", tP.ToString());
              GetParam(ll_handle, "MZDBLJJE", tP, 110); //返回值
             sameNoListReturn.Add("MZDBLJJE", tP.ToString());

              LocateDataSet(ll_handle, "MZFYJSXMMX1");
            //取数据集的数据行数
            Int32 ll_rowsize1 = GetRowSize(ll_handle);
            //设置返回的数据列变量，并分配存储空间     
            //根据对应返回数据集的数据行数按列取出列数据
          
            if (ll_rowsize1 > 0)
            {
                JArray ja = new JArray();
                for (int i = 0; i < ll_rowsize1; i++)
                {
                    JObject joRow = new JObject();
                    StringBuilder tem = new StringBuilder();
                    GetFieldValue(ll_handle, "JSXM ", tem, 50);
                    joRow.Add("JSXM",tem.ToString());
                    GetFieldValue(ll_handle, "FYXJ ", tem, 50);
                    joRow.Add("FYXJ", tem.ToString());
                    GetFieldValue(ll_handle, "YBFY", tem, 50);
                    joRow.Add("YBFY", tem.ToString());
                    GetFieldValue(ll_handle, "ZFJE", tem, 50);
                    joRow.Add("ZFJE", tem.ToString());
                    ja.Add(joRow);
                     NextRow(ll_handle);
                }
                sameNoListReturn.Add("MZFYJSXMMX1", ja);
            }




             LocateDataSet(ll_handle, "MZCFXMMX");
            //取数据集的数据行数
            Int32 ll_rowsize2 = GetRowSize(ll_handle);
            //设置返回的数据列变量，并分配存储空间
          
            //根据对应返回数据集的数据行数按列取出列数据
            if (ll_rowsize2 > 0)
            {
                JArray ja = new JArray();
                for (int i = 0; i < ll_rowsize2; i++)
                {
                    JObject joRow = new JObject();
                    StringBuilder tem = new StringBuilder();
                    GetFieldValue(ll_handle, " JZJLH ", tem, 50);
                    joRow.Add("JZJLH", tem.ToString());
                    GetFieldValue(ll_handle, " XMXH ", tem, 50);
                    joRow.Add("XMXH", tem.ToString());
                    GetFieldValue(ll_handle, " KZRQ ", tem, 50);
                    joRow.Add("KZRQ", tem.ToString());
                    GetFieldValue(ll_handle, " TZRQ ", tem, 50);
                    joRow.Add("TZRQ", tem.ToString());
                    GetFieldValue(ll_handle, " XMBH ", tem, 50);
                    joRow.Add("XMBH", tem.ToString());
                    GetFieldValue(ll_handle, " YYXMBH ", tem, 50);
                    joRow.Add("YYXMBH", tem.ToString());
                    GetFieldValue(ll_handle, " XMMC ", tem, 50);
                    joRow.Add("XMMC", tem.ToString());
                    GetFieldValue(ll_handle, " SPMC ", tem, 50);
                    joRow.Add("SPMC", tem.ToString());
                    GetFieldValue(ll_handle, " FLDM ", tem, 50);
                    joRow.Add("FLDM", tem.ToString());
                    GetFieldValue(ll_handle, " YPGG ", tem, 50);
                    joRow.Add("YPGG", tem.ToString());
                    GetFieldValue(ll_handle, " YPJX ", tem, 50);
                    joRow.Add("YPJX", tem.ToString());
                    GetFieldValue(ll_handle, " YPYF ", tem, 50);
                    joRow.Add("YPYF", tem.ToString());
                    GetFieldValue(ll_handle, " YPJL ", tem, 50);
                    joRow.Add("YPJL", tem.ToString());
                    GetFieldValue(ll_handle, " YZLB ", tem, 50);
                    joRow.Add("YZLB", tem.ToString());
                    GetFieldValue(ll_handle, " JG ", tem, 50);
                    joRow.Add("JG", tem.ToString());
                    GetFieldValue(ll_handle, " MCYL ", tem, 50);
                    joRow.Add("MCYL", tem.ToString());
                    GetFieldValue(ll_handle, " ZFBL ", tem, 50);
                    joRow.Add("ZFBL", tem.ToString());
                    GetFieldValue(ll_handle, " SQZFBL ", tem, 50);
                    joRow.Add("SQZFBL", tem.ToString());
                    GetFieldValue(ll_handle, " XJJE ", tem, 50);
                    joRow.Add("XJJE", tem.ToString());
                    GetFieldValue(ll_handle, " JE ", tem, 50);
                    joRow.Add("JE", tem.ToString());
                    GetFieldValue(ll_handle, " YPLY ", tem, 50);
                    joRow.Add("YPLY", tem.ToString());
                    GetFieldValue(ll_handle, " TSYPBZ ", tem, 50);
                    joRow.Add("TSYPBZ", tem.ToString());
                    GetFieldValue(ll_handle, " XZYYBZ ", tem, 50);
                    joRow.Add("XZYYBZ", tem.ToString());
                    GetFieldValue(ll_handle, " SJGYYY ", tem, 50);
                    joRow.Add("SJGYYY", tem.ToString());
                    GetFieldValue(ll_handle, " DLGG ", tem, 50);
                    joRow.Add("DLGG", tem.ToString());
                    GetFieldValue(ll_handle, " DWGG ", tem, 50);
                    joRow.Add("DWGG", tem.ToString());
                    GetFieldValue(ll_handle, " SYTS ", tem, 50);
                    joRow.Add("SYTS", tem.ToString());
                    GetFieldValue(ll_handle, " YJJYPBM ", tem, 50);
                    joRow.Add("YJJYPBM", tem.ToString());
                    GetFieldValue(ll_handle, " YYCLZCZMC ", tem, 50);
                    joRow.Add("YYCLZCZMC", tem.ToString());
                    GetFieldValue(ll_handle, " YYCLSYJZCH ", tem, 50);
                    joRow.Add("YYCLSYJZCH", tem.ToString());
                    GetFieldValue(ll_handle, " TXBZ ", tem, 50);
                    joRow.Add("TXBZ", tem.ToString());
                    GetFieldValue(ll_handle, " BZ1 ", tem, 50);
                    joRow.Add("BZ1", tem.ToString());
                    GetFieldValue(ll_handle, " BZ2 ", tem, 50);
                    joRow.Add("BZ2", tem.ToString());
                    GetFieldValue(ll_handle, " BZ3 ", tem, 50);
                    joRow.Add("BZ3", tem.ToString());

                    ja.Add(joRow);
                     NextRow(ll_handle);
                }
                sameNoListReturn.Add("MZCFXMMX", ja);
            }
               
            HardwareInitialClass.H_DestroyInstance(ll_handle);
            return ll_rtn;

        }

        //结算pos确认
        public static bool H_Function2026(string JZJLH, string JSYWH)
        {
            //将单项数据传入调用实例
            IntPtr ll_handle = HardwareInitialClass.H_CreateInstace();
            Int32 ll_rtn;
            ll_rtn = SetParam(ll_handle, "FN", "2026"); //设置功能号  
            ll_rtn = SetParam(ll_handle, "JZJLH", JZJLH); //
            ll_rtn = SetParam(ll_handle, "JSYWH", JSYWH); //
            ll_rtn = RunInstance(ll_handle);
            StringBuilder return_date = new StringBuilder(50);
            ll_rtn = GetParam(ll_handle, "FHZ ", return_date, 1024); //返回值
            HardwareInitialClass.H_DestroyInstance(ll_handle);

            if (return_date.ToString() == "1") return true;
            else return false;  

        }

        //工伤结算
        public static Int32 H_Function3049(string ZYH,
            string JZRQ, string JZZD, string JZZDGJDM, string ZZYSXM,
            SocialCard cadMes, JToken date, StringBuilder GRZFJE, StringBuilder TCZFJE, StringBuilder ZFEIJE, JObject sameNoListReturn, bool isSure, StringBuilder wrMes)
        {
            IntPtr ll_handle = HardwareInitialClass.H_CreateInstace();
            //将单项数据传入调用实例
            Int32 ll_rtn;
            ll_rtn = SetParam(ll_handle, "FN", "3049"); //设置功能号 
 
            if (isSure)
                ll_rtn = SetParam(ll_handle, "JSBZ", "1"); //
            else
                ll_rtn = SetParam(ll_handle, "JSBZ", "2"); //
            ll_rtn = SetParam(ll_handle, "GMSFHM", cadMes.holder_id); //

            ll_rtn = SetParam(ll_handle, "GRSXH", cadMes.user_id); //


            ll_rtn = SetParam(ll_handle, "JZLB", cadMes.blood_type); //&&
            ll_rtn = SetParam(ll_handle, "ZYH", ZYH); //&&

            ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ); //

            StringBuilder BZLB=new StringBuilder(111);
            H_Function2091(cadMes.user_id, JZRQ, "DJBZ", BZLB);
            ll_rtn = SetParam(ll_handle, "BZLB", BZLB.ToString()); //

            ll_rtn = SetParam(ll_handle, "JZZD", JZZD); //
            ll_rtn = SetParam(ll_handle, "JZZDGJDM", JZZDGJDM); //
            ll_rtn = SetParam(ll_handle, "ZZYSXM", ZZYSXM); //
            ll_rtn = SetParam(ll_handle, "ND", cadMes.area_code); //
            ll_rtn = SetParam(ll_handle, "RYLB", cadMes.user_type); //
            ll_rtn = SetParam(ll_handle, "KH", cadMes.card); //
            ll_rtn = SetParam(ll_handle, "JYM", cadMes.linkman);
            ll_rtn = InsertDataSet(ll_handle);

            for (int i = 0; i < date.Count(); i++)
            {
                ll_rtn = InsertRow(ll_handle);    //创建一行
                ll_rtn = SetField(ll_handle, "XMXH", date[i]["XMXH"].ToString());
                ll_rtn = SetField(ll_handle, "KZRQ", date[i]["KZRQ"].ToString());
                ll_rtn = SetField(ll_handle, "TZRQ", date[i]["TZRQ"].ToString());

                ll_rtn = SetField(ll_handle, "YYXMBH", date[i]["YYXMBH"].ToString());
                ll_rtn = SetField(ll_handle, "YYXMMC", date[i]["YYXMMC"].ToString());
                ll_rtn = SetField(ll_handle, "SPMC", date[i]["SPMC"].ToString());

                ll_rtn = SetField(ll_handle, "YPGG", date[i]["YPGG"].ToString());
                ll_rtn = SetField(ll_handle, "YPJX", date[i]["YPJX"].ToString());
                ll_rtn = SetField(ll_handle, "YPYF", date[i]["YPYF"].ToString());
                ll_rtn = SetField(ll_handle, "YPJL", date[i]["YPJL"].ToString());

                ll_rtn = SetField(ll_handle, "YZLB", date[i]["YZLB"].ToString());
                ll_rtn = SetField(ll_handle, "JG", date[i]["JG"].ToString());
                ll_rtn = SetField(ll_handle, "MCYL", date[i]["MCYL"].ToString());
                ll_rtn = SetField(ll_handle, "JE", date[i]["JE"].ToString());
                ll_rtn = SetField(ll_handle, "YPLY", date[i]["YPLY"].ToString());
                ll_rtn = SetField(ll_handle, "TSYPBZ", date[i]["TSYPBZ"].ToString());

                ll_rtn = SetField(ll_handle, "XZYYBZ", date[i]["XZYYBZ"].ToString());
                ll_rtn = SetField(ll_handle, "SJGYYY", date[i]["SJGYYY"].ToString());

                ll_rtn = SetField(ll_handle, "DLGG", date[i]["DLGG"].ToString());
                ll_rtn = SetField(ll_handle, "DWGG", date[i]["DWGG"].ToString());
                ll_rtn = SetField(ll_handle, "SYTS", date[i]["SYTS"].ToString());
                ll_rtn = SetField(ll_handle, "YJJYPBM", date[i]["YJJYPBM"].ToString());

                ll_rtn = SetField(ll_handle, "YYCLZCZMC", date[i]["YYCLZCZMC"].ToString());
                ll_rtn = SetField(ll_handle, "YYCLSYJZCH", date[i]["YYCLSYJZCH"].ToString());
                ll_rtn = SetField(ll_handle, "TXBZ", date[i]["TXBZ"].ToString());
                ll_rtn = SetField(ll_handle, "BZ1", date[i]["BZ1"].ToString());
                ll_rtn = SetField(ll_handle, "BZ2", date[i]["BZ2"].ToString());
                ll_rtn = SetField(ll_handle, "BZ3", date[i]["BZ3"].ToString());

                ll_rtn = EndRow(ll_handle, i + 1);     //结束当前行
            }
            ll_rtn = EndDataSet(ll_handle, "GSMZCFXMDRML");  //结束当前数据集，赋予数据集的名称

            ll_rtn = RunInstance(ll_handle);
            if (ll_rtn < 0)
            {
                GetSysMessage(ll_handle, wrMes, 35);
                return ll_rtn;
            }

             GetParam(ll_handle, "GRZFJE", GRZFJE, 1024); //返回值
             GetParam(ll_handle, "TCZFJE ", TCZFJE, 1024); //返回值
             GetParam(ll_handle, "ZFEIJE", ZFEIJE, 1024); //返回值
            if (GRZFJE.ToString() == "") GRZFJE.Append("0");
            if (TCZFJE.ToString() == "") TCZFJE.Append("0");
            if (ZFEIJE.ToString() == "") ZFEIJE.Append("0");




            StringBuilder tP = new StringBuilder(1024);
             GetParam(ll_handle, "FHZ", tP, 110); //返回值
            sameNoListReturn.Add("FHZ", tP.ToString());
             GetParam(ll_handle, "MSG", tP, 110); //返回值
            sameNoListReturn.Add("MSG", tP.ToString());
             GetParam(ll_handle, "JSYWH", tP, 110); //返回值
            sameNoListReturn.Add("JSYWH", tP.ToString());
             GetParam(ll_handle, "YLFYZE", tP, 110); //返回值
            sameNoListReturn.Add("YLFYZE", tP.ToString());
             GetParam(ll_handle, "GZZFUJE", tP, 110); //返回值
            sameNoListReturn.Add("GZZFUJE", tP.ToString());
             GetParam(ll_handle, "GZZFEJE", tP, 110); //返回值
            sameNoListReturn.Add("GZZFEJE", tP.ToString());
             GetParam(ll_handle, "TCZFJE", tP, 110); //返回值
            sameNoListReturn.Add("TCZFJE", tP.ToString());
             GetParam(ll_handle, "XJZFUJE", tP, 110); //返回值
            sameNoListReturn.Add("XJZFUJE", tP.ToString());
             GetParam(ll_handle, "XJZFEJE", tP, 110); //返回值
            sameNoListReturn.Add("XJZFEJE", tP.ToString());
             GetParam(ll_handle, "DEJSBZ", tP, 110); //返回值
            sameNoListReturn.Add("DEJSBZ", tP.ToString());

             GetParam(ll_handle, "JSQZYTCED", tP, 110); //返回值
            sameNoListReturn.Add("JSQZYTCED", tP.ToString());
             GetParam(ll_handle, "JSRQ", tP, 110); //返回值
            sameNoListReturn.Add("JSRQ", tP.ToString());
             GetParam(ll_handle, "RYLB", tP, 110); //返回值
            sameNoListReturn.Add("RYLB", tP.ToString());
             GetParam(ll_handle, "JZJLH", tP, 110); //返回值
            sameNoListReturn.Add("JZJLH", tP.ToString());
             GetParam(ll_handle, "SYSXBZ", tP, 110); //返回值
            sameNoListReturn.Add("SYSXBZ", tP.ToString());
            //取数据集的数据行数
             LocateDataSet(ll_handle, "FYJSXMMX");
            Int32 ll_rowsize1 = GetRowSize(ll_handle);
            //设置返回的数据列变量，并分配存储空间     
            //根据对应返回数据集的数据行数按列取出列数据


            if (ll_rowsize1 > 0)
            {
                JArray ja = new JArray();
                for (int i = 0; i < ll_rowsize1; i++)
                {
                    JObject joRow = new JObject();
                    StringBuilder tem = new StringBuilder();
                    GetFieldValue(ll_handle, "JSXM ", tem, 50);
                    joRow.Add("JSXM", tem.ToString());
                    GetFieldValue(ll_handle, "FYXJ", tem, 50);
                    joRow.Add("FYXJ", tem.ToString());
                    GetFieldValue(ll_handle, "YBFY", tem, 50);
                    joRow.Add("YBFY", tem.ToString());
                    GetFieldValue(ll_handle, "ZFJE", tem, 50);
                    joRow.Add("ZFJE", tem.ToString());
                    ja.Add(joRow);
                     NextRow(ll_handle);
                }
                sameNoListReturn.Add("FYJSXMMX", ja);
            }




             LocateDataSet(ll_handle, "JHSYCFXMMX");
            //取数据集的数据行数
            Int32 ll_rowsize2 = GetRowSize(ll_handle);
            //设置返回的数据列变量，并分配存储空间

            //根据对应返回数据集的数据行数按列取出列数据
            if (ll_rowsize2 > 0)
            {
                JArray ja = new JArray();
                for (int i = 0; i < ll_rowsize2; i++)
                {
                    JObject joRow = new JObject();
                    StringBuilder tem = new StringBuilder();
                    GetFieldValue(ll_handle, " JZJLH ", tem, 50);
                    joRow.Add("JZJLH", tem.ToString());
                    GetFieldValue(ll_handle, " XMXH ", tem, 50);
                    joRow.Add("XMXH", tem.ToString());
                    GetFieldValue(ll_handle, " KZRQ ", tem, 50);
                    joRow.Add("KZRQ", tem.ToString());
                    GetFieldValue(ll_handle, " TZRQ ", tem, 50);
                    joRow.Add("TZRQ", tem.ToString());
                    GetFieldValue(ll_handle, " XMBH ", tem, 50);
                    joRow.Add("XMBH", tem.ToString());
                    GetFieldValue(ll_handle, " YYXMBH ", tem, 50);
                    joRow.Add("YYXMBH", tem.ToString());
                    GetFieldValue(ll_handle, " XMMC ", tem, 50);
                    joRow.Add("XMMC", tem.ToString());
                    GetFieldValue(ll_handle, " SPMC ", tem, 50);
                    joRow.Add("SPMC", tem.ToString());
                    GetFieldValue(ll_handle, " FLDM ", tem, 50);
                    joRow.Add("FLDM", tem.ToString());
                    GetFieldValue(ll_handle, " YPGG ", tem, 50);
                    joRow.Add("YPGG", tem.ToString());
                    GetFieldValue(ll_handle, " YPJX ", tem, 50);
                    joRow.Add("YPJX", tem.ToString());
                    GetFieldValue(ll_handle, " YPYF ", tem, 50);
                    joRow.Add("YPYF", tem.ToString());
                    GetFieldValue(ll_handle, " YPJL ", tem, 50);
                    joRow.Add("YPJL", tem.ToString());
                    GetFieldValue(ll_handle, " YZLB ", tem, 50);
                    joRow.Add("YZLB", tem.ToString());
                    GetFieldValue(ll_handle, " JG ", tem, 50);
                    joRow.Add("JG", tem.ToString());
                    GetFieldValue(ll_handle, " MCYL ", tem, 50);
                    joRow.Add("MCYL", tem.ToString());
                    GetFieldValue(ll_handle, " ZFBL ", tem, 50);
                    joRow.Add("ZFBL", tem.ToString());
                    GetFieldValue(ll_handle, " SQZFBL ", tem, 50);
                    joRow.Add("SQZFBL", tem.ToString());
                    GetFieldValue(ll_handle, " XJJE ", tem, 50);
                    joRow.Add("XJJE", tem.ToString());
                    GetFieldValue(ll_handle, " JE ", tem, 50);
                    joRow.Add("JE", tem.ToString());
                    GetFieldValue(ll_handle, " YPLY ", tem, 50);
                    joRow.Add("YPLY", tem.ToString());
                    GetFieldValue(ll_handle, " TSYPBZ ", tem, 50);
                    joRow.Add("TSYPBZ", tem.ToString());
                    GetFieldValue(ll_handle, " XZYYBZ ", tem, 50);
                    joRow.Add("XZYYBZ", tem.ToString());
                    GetFieldValue(ll_handle, " SJGYYY ", tem, 50);
                    joRow.Add("SJGYYY", tem.ToString());
                    GetFieldValue(ll_handle, " DLGG ", tem, 50);
                    joRow.Add("DLGG", tem.ToString());
                    GetFieldValue(ll_handle, " DWGG ", tem, 50);
                    joRow.Add("DWGG", tem.ToString());
                    GetFieldValue(ll_handle, " SYTS ", tem, 50);
                    joRow.Add("SYTS", tem.ToString());
                    GetFieldValue(ll_handle, " YJJYPBM ", tem, 50);
                    joRow.Add("YJJYPBM", tem.ToString());
                    GetFieldValue(ll_handle, " YYCLZCZMC ", tem, 50);
                    joRow.Add("YYCLZCZMC", tem.ToString());
                    GetFieldValue(ll_handle, " YYCLSYJZCH ", tem, 50);
                    joRow.Add("YYCLSYJZCH", tem.ToString());
                    GetFieldValue(ll_handle, " TXBZ ", tem, 50);
                    joRow.Add("TXBZ", tem.ToString());
                    GetFieldValue(ll_handle, " BZ1 ", tem, 50);
                    joRow.Add("BZ1", tem.ToString());
                    GetFieldValue(ll_handle, " BZ2 ", tem, 50);
                    joRow.Add("BZ2", tem.ToString());
                    GetFieldValue(ll_handle, " BZ3 ", tem, 50);
                    joRow.Add("BZ3", tem.ToString());

                    ja.Add(joRow);
                     NextRow(ll_handle);
                }
                sameNoListReturn.Add("JHSYCFXMMX", ja);
            }

            HardwareInitialClass.H_DestroyInstance(ll_handle);
            return ll_rtn;

        }

        //计生结算
        public static Int32 H_Function2143(string ZYH,
            string JZRQ, string JZZD, string JZZDGJDM, string ZZYSXM, string JHSYSS1, string JHSYSS2, string JHSYSS3, string PSAM, string HYZS, string SFECJS,
            SocialCard cadMes, JToken date, StringBuilder gezhang, StringBuilder xianjin,JObject sameNoListReturn, bool isSure, StringBuilder wrMes)
        {
            IntPtr ll_handle = HardwareInitialClass.H_CreateInstace();
            //将单项数据传入调用实例
            Int32 ll_rtn;
            ll_rtn = SetParam(ll_handle, "FN", "2143"); //设置功能号 

            if (isSure)
                ll_rtn = SetParam(ll_handle, "JSBZ", "1"); //
            else
                ll_rtn = SetParam(ll_handle, "JSBZ", "2"); //
            ll_rtn = SetParam(ll_handle, "GMSFHM", cadMes.holder_id); //

            ll_rtn = SetParam(ll_handle, "GRSXH", cadMes.user_id); //


            ll_rtn = SetParam(ll_handle, "JZLB", cadMes.blood_type); //&&
            ll_rtn = SetParam(ll_handle, "ZYH", ZYH); //&&

            ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ); //

            //StringBuilder BZLB = new StringBuilder();
            //H_Function2091(cadMes.user_id, JZRQ, "DJBZ", BZLB);
            //ll_rtn = SetParam(ll_handle, "BZLB", BZLB.ToString()); //

            ll_rtn = SetParam(ll_handle, "JZZD", JZZD); //

            ll_rtn = SetParam(ll_handle, "JHSYSS1", JHSYSS1); //
            ll_rtn = SetParam(ll_handle, "JHSYSS2", JHSYSS2); //
            ll_rtn = SetParam(ll_handle, "JHSYSS3", JHSYSS3); //

            ll_rtn = SetParam(ll_handle, "JZZDGJDM", JZZDGJDM); //

            ll_rtn = SetParam(ll_handle, "ZZYSXM", ZZYSXM); //
            ll_rtn = SetParam(ll_handle, "YBZHYE", cadMes.sb_amount); //
            ll_rtn = SetParam(ll_handle, "GRZFLJ", cadMes.zfu_amount); //

            ll_rtn = SetParam(ll_handle, "ND", cadMes.area_code); //
            ll_rtn = SetParam(ll_handle, "RYLB", cadMes.user_type); //
            ll_rtn = SetParam(ll_handle, "KH", cadMes.card); //
            ll_rtn = SetParam(ll_handle, "JYM", cadMes.linkman);
            ll_rtn = SetParam(ll_handle, "PSAM", PSAM);
            ll_rtn = SetParam(ll_handle, "HYZS", HYZS);
            ll_rtn = SetParam(ll_handle, "SFECJS", SFECJS);
            ll_rtn = InsertDataSet(ll_handle);

            for (int i = 0; i < date.Count(); i++)
            {
                ll_rtn = InsertRow(ll_handle);    //创建一行
                ll_rtn = SetField(ll_handle, "XMXH", date[i]["XMXH"].ToString());
                ll_rtn = SetField(ll_handle, "KZRQ", date[i]["KZRQ"].ToString());
                ll_rtn = SetField(ll_handle, "TZRQ", date[i]["TZRQ"].ToString());

                ll_rtn = SetField(ll_handle, "YYXMBH", date[i]["YYXMBH"].ToString());
                ll_rtn = SetField(ll_handle, "YYXMMC", date[i]["YYXMMC"].ToString());
                ll_rtn = SetField(ll_handle, "SPMC", date[i]["SPMC"].ToString());

                ll_rtn = SetField(ll_handle, "YPGG", date[i]["YPGG"].ToString());
                ll_rtn = SetField(ll_handle, "YPJX", date[i]["YPJX"].ToString());
                ll_rtn = SetField(ll_handle, "YPYF", date[i]["YPYF"].ToString());
                ll_rtn = SetField(ll_handle, "YPJL", date[i]["YPJL"].ToString());

                ll_rtn = SetField(ll_handle, "YZLB", date[i]["YZLB"].ToString());
                ll_rtn = SetField(ll_handle, "JG", date[i]["JG"].ToString());
                ll_rtn = SetField(ll_handle, "MCYL", date[i]["MCYL"].ToString());
                ll_rtn = SetField(ll_handle, "JE", date[i]["JE"].ToString());
                ll_rtn = SetField(ll_handle, "YPLY", date[i]["YPLY"].ToString());
                ll_rtn = SetField(ll_handle, "TSYPBZ", date[i]["TSYPBZ"].ToString());

                ll_rtn = SetField(ll_handle, "XZYYBZ", date[i]["XZYYBZ"].ToString());
                ll_rtn = SetField(ll_handle, "SJGYYY", date[i]["SJGYYY"].ToString());

                ll_rtn = SetField(ll_handle, "DLGG", date[i]["DLGG"].ToString());
                ll_rtn = SetField(ll_handle, "DWGG", date[i]["DWGG"].ToString());
                ll_rtn = SetField(ll_handle, "SYTS", date[i]["SYTS"].ToString());
                ll_rtn = SetField(ll_handle, "YJJYPBM", date[i]["YJJYPBM"].ToString());

                ll_rtn = SetField(ll_handle, "YYCLZCZMC", date[i]["YYCLZCZMC"].ToString());
                ll_rtn = SetField(ll_handle, "YYCLSYJZCH", date[i]["YYCLSYJZCH"].ToString());
                ll_rtn = SetField(ll_handle, "TXBZ", date[i]["TXBZ"].ToString());
                ll_rtn = SetField(ll_handle, "BZ1", date[i]["BZ1"].ToString());
                ll_rtn = SetField(ll_handle, "BZ2", date[i]["BZ2"].ToString());
                ll_rtn = SetField(ll_handle, "BZ3", date[i]["BZ3"].ToString());

                ll_rtn = EndRow(ll_handle, i + 1);     //结束当前行
            }
            ll_rtn = EndDataSet(ll_handle, "JHSYCFXMDRML");  //结束当前数据集，赋予数据集的名称

            ll_rtn = RunInstance(ll_handle);
            if (ll_rtn < 0)
            {
                GetSysMessage(ll_handle, wrMes, 35);
                return ll_rtn;
            }

            StringBuilder GZZFUJE = new StringBuilder(111);
            StringBuilder GZZFEJE = new StringBuilder(111);

            StringBuilder XJZFUJE = new StringBuilder(111);
            StringBuilder XJZFEJE = new StringBuilder(111);

          

            //gezhang.Append(float.Parse(GZZFEJE.ToString()) + float.Parse(GZZFUJE.ToString()));
            //xianjin.Append(float.Parse(XJZFUJE.ToString()) + float.Parse(XJZFEJE.ToString()));

             GetParam(ll_handle, "GZZFUJE", GZZFUJE, 1024); //返回值
             GetParam(ll_handle, "GZZFEJE", GZZFEJE, 1024); //返回值
             GetParam(ll_handle, "XJZFUJE", XJZFUJE, 1024); //返回值
             GetParam(ll_handle, "XJZFEJE", XJZFEJE, 1024); //返回值

            if (XJZFUJE.ToString() == "") XJZFUJE.Append("0");
            if (XJZFEJE.ToString() == "") XJZFEJE.Append("0");
            if (GZZFEJE.ToString() == "") GZZFEJE.Append("0");
            if (GZZFUJE.ToString() == "") GZZFUJE.Append("0");

            gezhang.Append((float.Parse(GZZFUJE.ToString()) + float.Parse(GZZFEJE.ToString())).ToString());
            xianjin.Append((float.Parse(XJZFUJE.ToString()) + float.Parse(XJZFEJE.ToString())));

            StringBuilder tP = new StringBuilder(1024);
             GetParam(ll_handle, "FHZ", tP, 110); //返回值
            sameNoListReturn.Add("FHZ", tP.ToString());
             GetParam(ll_handle, "MSG", tP, 110); //返回值
            sameNoListReturn.Add("MSG", tP.ToString());
             GetParam(ll_handle, "JSYWH", tP, 110); //返回值
            sameNoListReturn.Add("JSYWH", tP.ToString());
             GetParam(ll_handle, "YLFYZE", tP, 110); //返回值
            sameNoListReturn.Add("YLFYZE", tP.ToString());
             GetParam(ll_handle, "GRZFJE", tP, 110); //返回值
            sameNoListReturn.Add("GRZFJE", tP.ToString());
             GetParam(ll_handle, "TCZFJE ", tP, 110); //返回值
            sameNoListReturn.Add("TCZFJE", tP.ToString());
             GetParam(ll_handle, "ZFEIJE", tP, 110); //返回值
            sameNoListReturn.Add("ZFEIJE", tP.ToString());
             GetParam(ll_handle, "DEJSBZ", tP, 110); //返回值
            sameNoListReturn.Add("DEJSBZ", tP.ToString());
             GetParam(ll_handle, "JSRQ", tP, 110); //返回值
            sameNoListReturn.Add("JSRQ", tP.ToString());
             GetParam(ll_handle, "JZJLH", tP, 110); //返回值
            sameNoListReturn.Add("JZJLH", tP.ToString());


            //取数据集的数据行数
             LocateDataSet(ll_handle, "FYJSXMMX");
            Int32 ll_rowsize1 = GetRowSize(ll_handle);
            //设置返回的数据列变量，并分配存储空间     
            //根据对应返回数据集的数据行数按列取出列数据


            if (ll_rowsize1 > 0)
            {
                JArray ja = new JArray();
                for (int i = 0; i < ll_rowsize1; i++)
                {
                    JObject joRow = new JObject();
                    StringBuilder tem = new StringBuilder();
                    GetFieldValue(ll_handle, "JSXM ", tem, 50);
                    joRow.Add("JSXM", tem.ToString());
                    GetFieldValue(ll_handle, "FYXJ", tem, 50);
                    joRow.Add("FYXJ", tem.ToString());
                    GetFieldValue(ll_handle, "YBFY", tem, 50);
                    joRow.Add("YBFY", tem.ToString());
                    GetFieldValue(ll_handle, "ZFJE", tem, 50);
                    joRow.Add("ZFJE", tem.ToString());
                    ja.Add(joRow);
                     NextRow(ll_handle);
                }
                sameNoListReturn.Add("FYJSXMMX", ja);
            }




             LocateDataSet(ll_handle, "GSMZCFXMMX");
            //取数据集的数据行数
            Int32 ll_rowsize2 = GetRowSize(ll_handle);
            //设置返回的数据列变量，并分配存储空间

            //根据对应返回数据集的数据行数按列取出列数据
            if (ll_rowsize2 > 0)
            {
                JArray ja = new JArray();
                for (int i = 0; i < ll_rowsize2; i++)
                {
                    JObject joRow = new JObject();
                    StringBuilder tem = new StringBuilder();
                    GetFieldValue(ll_handle, " JZJLH ", tem, 50);
                    joRow.Add("JZJLH", tem.ToString());
                    GetFieldValue(ll_handle, " XMXH ", tem, 50);
                    joRow.Add("XMXH", tem.ToString());
                    GetFieldValue(ll_handle, " KZRQ ", tem, 50);
                    joRow.Add("KZRQ", tem.ToString());
                    GetFieldValue(ll_handle, " TZRQ ", tem, 50);
                    joRow.Add("TZRQ", tem.ToString());
                    GetFieldValue(ll_handle, " XMBH ", tem, 50);
                    joRow.Add("XMBH", tem.ToString());
                    GetFieldValue(ll_handle, " YYXMBH ", tem, 50);
                    joRow.Add("YYXMBH", tem.ToString());
                    GetFieldValue(ll_handle, " XMMC ", tem, 50);
                    joRow.Add("XMMC", tem.ToString());
                    GetFieldValue(ll_handle, " SPMC ", tem, 50);
                    joRow.Add("SPMC", tem.ToString());
                    GetFieldValue(ll_handle, " FLDM ", tem, 50);
                    joRow.Add("FLDM", tem.ToString());
                    GetFieldValue(ll_handle, " YPGG ", tem, 50);
                    joRow.Add("YPGG", tem.ToString());
                    GetFieldValue(ll_handle, " YPJX ", tem, 50);
                    joRow.Add("YPJX", tem.ToString());
                    GetFieldValue(ll_handle, " YPYF ", tem, 50);
                    joRow.Add("YPYF", tem.ToString());
                    GetFieldValue(ll_handle, " YPJL ", tem, 50);
                    joRow.Add("YPJL", tem.ToString());
                    GetFieldValue(ll_handle, " YZLB ", tem, 50);
                    joRow.Add("YZLB", tem.ToString());
                    GetFieldValue(ll_handle, " JG ", tem, 50);
                    joRow.Add("JG", tem.ToString());
                    GetFieldValue(ll_handle, " MCYL ", tem, 50);
                    joRow.Add("MCYL", tem.ToString());
                    GetFieldValue(ll_handle, " ZFBL ", tem, 50);
                    joRow.Add("ZFBL", tem.ToString());
                    GetFieldValue(ll_handle, " SQZFBL ", tem, 50);
                    joRow.Add("SQZFBL", tem.ToString());
                    GetFieldValue(ll_handle, " XJJE ", tem, 50);
                    joRow.Add("XJJE", tem.ToString());
                    GetFieldValue(ll_handle, " JE ", tem, 50);
                    joRow.Add("JE", tem.ToString());
                    GetFieldValue(ll_handle, " YPLY ", tem, 50);
                    joRow.Add("YPLY", tem.ToString());
                    GetFieldValue(ll_handle, " TSYPBZ ", tem, 50);
                    joRow.Add("TSYPBZ", tem.ToString());
                    GetFieldValue(ll_handle, " XZYYBZ ", tem, 50);
                    joRow.Add("XZYYBZ", tem.ToString());
                    GetFieldValue(ll_handle, " SJGYYY ", tem, 50);
                    joRow.Add("SJGYYY", tem.ToString());
                    GetFieldValue(ll_handle, " DLGG ", tem, 50);
                    joRow.Add("DLGG", tem.ToString());
                    GetFieldValue(ll_handle, " DWGG ", tem, 50);
                    joRow.Add("DWGG", tem.ToString());
                    GetFieldValue(ll_handle, " SYTS ", tem, 50);
                    joRow.Add("SYTS", tem.ToString());
                    GetFieldValue(ll_handle, " YJJYPBM ", tem, 50);
                    joRow.Add("YJJYPBM", tem.ToString());
                    GetFieldValue(ll_handle, " YYCLZCZMC ", tem, 50);
                    joRow.Add("YYCLZCZMC", tem.ToString());
                    GetFieldValue(ll_handle, " YYCLSYJZCH ", tem, 50);
                    joRow.Add("YYCLSYJZCH", tem.ToString());
                    GetFieldValue(ll_handle, " TXBZ ", tem, 50);
                    joRow.Add("TXBZ", tem.ToString());
                    GetFieldValue(ll_handle, " BZ1 ", tem, 50);
                    joRow.Add("BZ1", tem.ToString());
                    GetFieldValue(ll_handle, " BZ2 ", tem, 50);
                    joRow.Add("BZ2", tem.ToString());
                    GetFieldValue(ll_handle, " BZ3 ", tem, 50);
                    joRow.Add("BZ3", tem.ToString());

                    ja.Add(joRow);
                     NextRow(ll_handle);
                }
                sameNoListReturn.Add("GSMZCFXMMX", ja);
            }


            HardwareInitialClass.H_DestroyInstance(ll_handle);
            return ll_rtn;

        }


        //特殊病种
        public static Int32 H_Function2052(string ZYH,
            string JZRQ,string JZZD, string JZZDGJDM, string ZZYSXM,string PASM,
            SocialCard cadMes, JToken date, StringBuilder gezhang, StringBuilder xianjin, JObject sameNoListReturn, bool isSure, StringBuilder wrMes)
        {
            IntPtr ll_handle = HardwareInitialClass.H_CreateInstace();
            //将单项数据传入调用实例
            Int32 ll_rtn;
            ll_rtn = SetParam(ll_handle, "FN", "2052"); //设置功能号 

            if (isSure)
                ll_rtn = SetParam(ll_handle, "JSBZ", "1"); //
            else
                ll_rtn = SetParam(ll_handle, "JSBZ", "2"); //
            ll_rtn = SetParam(ll_handle, "GMSFHM", cadMes.holder_id); //

            ll_rtn = SetParam(ll_handle, "GRSXH", cadMes.user_id); //


            ll_rtn = SetParam(ll_handle, "JZLB", "71"); //&&
            ll_rtn = SetParam(ll_handle, "ZYH", ZYH); //&&

            ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ); //

            StringBuilder BZLB = new StringBuilder();
            H_Function2091(cadMes.user_id, JZRQ, "DJBZ", BZLB);
            ll_rtn = SetParam(ll_handle, "BZLB", BZLB.ToString()); //

            ll_rtn = SetParam(ll_handle, "JZZD", JZZD); //
            ll_rtn = SetParam(ll_handle, "JZZDGJDM", JZZDGJDM); //
            ll_rtn = SetParam(ll_handle, "ZZYSXM", ZZYSXM); //

            ll_rtn = SetParam(ll_handle, "GWYTCXE", cadMes.cs_amount); //
            ll_rtn = SetParam(ll_handle, "TSMZTCXE", cadMes.sp_amount); //
            ll_rtn = SetParam(ll_handle, "YBZHYE", cadMes.sb_amount); //
            ll_rtn = SetParam(ll_handle, "GRZFLJ", cadMes.zfu_amount); //

            ll_rtn = SetParam(ll_handle, "ND", cadMes.area_code); //
            ll_rtn = SetParam(ll_handle, "RYLB", cadMes.user_type); //
            ll_rtn = SetParam(ll_handle, "KH", cadMes.card); //
            ll_rtn = SetParam(ll_handle, "JYM", cadMes.linkman);
            ll_rtn = SetParam(ll_handle, "PASM", cadMes.linkman);
            ll_rtn = InsertDataSet(ll_handle);
            Xr.Log4net.LogHelper.Info("进入TDMZCFXMDRML入参");
            for (int i = 0; i < date.Count(); i++)
            {
                ll_rtn = InsertRow(ll_handle);    //创建一行
                ll_rtn = SetField(ll_handle, "XMXH", date[i]["XMXH"].ToString());
                ll_rtn = SetField(ll_handle, "KZRQ", date[i]["KZRQ"].ToString());
                ll_rtn = SetField(ll_handle, "TZRQ", date[i]["TZRQ"].ToString());

                ll_rtn = SetField(ll_handle, "YYXMBH", date[i]["YYXMBH"].ToString());
                ll_rtn = SetField(ll_handle, "YYXMMC", date[i]["YYXMMC"].ToString());
                ll_rtn = SetField(ll_handle, "SPMC", date[i]["SPMC"].ToString());

                ll_rtn = SetField(ll_handle, "YPGG", date[i]["YPGG"].ToString());

                ll_rtn = SetField(ll_handle, "YPJX", date[i]["YPJX"].ToString());
                ll_rtn = SetField(ll_handle, "YPYF", date[i]["YPYF"].ToString());

                ll_rtn = SetField(ll_handle, "YPJL", date[i]["YPJL"].ToString());

                ll_rtn = SetField(ll_handle, "YZLB", date[i]["YZLB"].ToString());
                ll_rtn = SetField(ll_handle, "JG", date[i]["JG"].ToString());
                ll_rtn = SetField(ll_handle, "MCYL", date[i]["MCYL"].ToString());
                ll_rtn = SetField(ll_handle, "JE", date[i]["JE"].ToString());
                ll_rtn = SetField(ll_handle, "YPLY", date[i]["YPLY"].ToString());
                ll_rtn = SetField(ll_handle, "TSYPBZ", date[i]["TSYPBZ"].ToString());

                ll_rtn = SetField(ll_handle, "XZYYBZ", date[i]["XZYYBZ"].ToString());
                ll_rtn = SetField(ll_handle, "SJGYYY", date[i]["SJGYYY"].ToString());

                ll_rtn = SetField(ll_handle, "DLGG", date[i]["DLGG"].ToString());
                ll_rtn = SetField(ll_handle, "DWGG", date[i]["DWGG"].ToString());
                ll_rtn = SetField(ll_handle, "SYTS", date[i]["SYTS"].ToString());
                ll_rtn = SetField(ll_handle, "YJJYPBM", date[i]["YJJYPBM"].ToString());

                ll_rtn = SetField(ll_handle, "YYCLZCZMC", date[i]["YYCLZCZMC"].ToString());
                ll_rtn = SetField(ll_handle, "YYCLSYJZCH", date[i]["YYCLSYJZCH"].ToString());
                ll_rtn = SetField(ll_handle, "TXBZ", date[i]["TXBZ"].ToString());
                ll_rtn = SetField(ll_handle, "BZ1", date[i]["BZ1"].ToString());
                ll_rtn = SetField(ll_handle, "BZ2", date[i]["BZ2"].ToString());
                ll_rtn = SetField(ll_handle, "BZ3", date[i]["BZ3"].ToString());

                ll_rtn = EndRow(ll_handle, i + 1);     //结束当前行
            }
            ll_rtn = EndDataSet(ll_handle, "TDMZCFXMDRML");  //结束当前数据集，赋予数据集的名称

            ll_rtn = RunInstance(ll_handle);
            Xr.Log4net.LogHelper.Info("执行状态：" + ll_rtn);
            if (ll_rtn < 0)
            {
                GetSysMessage(ll_handle, wrMes, 35);
                return ll_rtn;
            }

            StringBuilder GZZFUJE = new StringBuilder(100) ;
            StringBuilder GZZFEJE = new StringBuilder(100);
            StringBuilder XJZFUJE = new StringBuilder(100);
            StringBuilder XJZFEJE = new StringBuilder(100);
            StringBuilder MZDBTCZF = new StringBuilder(100);
            StringBuilder MZYFTCZF = new StringBuilder(100);

            GetParam(ll_handle, "GZZFUJE",  GZZFUJE, 110); //返回值
            GetParam(ll_handle, "GZZFEJE",  GZZFEJE, 110); //返回值
            GetParam(ll_handle, "XJZFUJE",  XJZFUJE, 110); //返回值
            GetParam(ll_handle, "XJZFEJE",  XJZFEJE, 110); //返回值

            GetParam(ll_handle, "MZDBTCZF",  MZDBTCZF, 110); //返回值
            GetParam(ll_handle, "MZYFTCZF",  MZYFTCZF, 110); //返回值

            if (MZYFTCZF.ToString() == "") MZYFTCZF.Append("0");
            if (MZDBTCZF.ToString() == "") MZDBTCZF.Append("0");
            if (XJZFUJE.ToString() == "") XJZFUJE.Append("0");
            if (XJZFEJE.ToString() == "") XJZFEJE.Append("0");
            if (GZZFEJE.ToString() == "") GZZFEJE.Append("0");
            if (GZZFUJE.ToString() == "") GZZFUJE.Append("0");

            Xr.Log4net.LogHelper.Info("MZYFTCZF：" + MZYFTCZF);
            Xr.Log4net.LogHelper.Info("MZDBTCZF：" + MZDBTCZF);
            Xr.Log4net.LogHelper.Info("XJZFUJE：" + XJZFUJE);
            Xr.Log4net.LogHelper.Info("XJZFEJE：" + GZZFUJE);
            Xr.Log4net.LogHelper.Info("GZZFEJE：" + GZZFEJE);
            Xr.Log4net.LogHelper.Info("GZZFUJE：" + GZZFUJE);
            gezhang.Append((float.Parse(GZZFUJE.ToString()) + float.Parse(GZZFEJE.ToString())).ToString());
            xianjin.Append((float.Parse(XJZFUJE.ToString()) + float.Parse(XJZFEJE.ToString()) - float.Parse(MZDBTCZF.ToString()) - float.Parse(MZYFTCZF.ToString())).ToString());
  

            StringBuilder tP = new StringBuilder(1024);
            GetParam(ll_handle, "FHZ", tP, 1024); //返回值
            sameNoListReturn.Add("FHZ", tP.ToString());
            GetParam(ll_handle, "MSG", tP, 1024); //返回值
            sameNoListReturn.Add("MSG", tP.ToString());
             GetParam(ll_handle, "JSYWH", tP, 1024); //返回值
             sameNoListReturn.Add("JSYWH", tP.ToString()); 
             GetParam(ll_handle, "YLFYZE", tP, 1024); //返回值
             sameNoListReturn.Add("YLFYZE", tP.ToString()); 
             GetParam(ll_handle, "GZZFUJE", tP, 1024); //返回值
             sameNoListReturn.Add("GZZFUJE", tP.ToString());  
             GetParam(ll_handle, "GZZFEJE ", tP, 1024); //返回值
             sameNoListReturn.Add("GZZFEJE", tP.ToString()); 
             GetParam(ll_handle, "TCZFJE", tP, 1024); //返回值
             sameNoListReturn.Add("TCZFJE", tP.ToString()); 
             GetParam(ll_handle, "GWYTCZF", tP, 1024); //返回值
             sameNoListReturn.Add("GWYTCZF", tP.ToString()); 
             GetParam(ll_handle, "XJZFUJE", tP, 1024); //返回值
             sameNoListReturn.Add("XJZFUJE", tP.ToString());  
             GetParam(ll_handle, "XJZFEJE", tP, 1024); //返回值
             sameNoListReturn.Add("XJZFEJE", tP.ToString());  

             GetParam(ll_handle, "DEJSBZ", tP, 1024); //返回值
             sameNoListReturn.Add("DEJSBZ", tP.ToString()); 
             GetParam(ll_handle, "JSQZYTCED", tP, 1024); //返回值
             sameNoListReturn.Add("JSQZYTCED", tP.ToString()); 
             GetParam(ll_handle, "JSRQ", tP, 1024); //返回值
             sameNoListReturn.Add("JSRQ", tP.ToString()); 
             GetParam(ll_handle, "RYLB", tP, 1024); //返回值
             sameNoListReturn.Add("RYLB", tP.ToString()); 
             GetParam(ll_handle, "BCYLTCZF", tP, 1024); //返回值
             sameNoListReturn.Add("BCYLTCZF", tP.ToString()); 
             GetParam(ll_handle, "JZJLH", tP, 1024); //返回值
             sameNoListReturn.Add("JZJLH", tP.ToString());  
             GetParam(ll_handle, "JBYLBZ", tP, 1024); //返回值
             sameNoListReturn.Add("JBYLBZ", tP.ToString()); 
             GetParam(ll_handle, "BCXSBZ", tP, 1024); //返回值
             sameNoListReturn.Add("BCXSBZ", tP.ToString()); 
             GetParam(ll_handle, "GWYXSBZ", tP, 1024); //返回值
             sameNoListReturn.Add("GWYXSBZ", tP.ToString());  
             GetParam(ll_handle, "DBTCZF", tP, 1024); //返回值
             sameNoListReturn.Add("DBTCZF", tP.ToString());  
             GetParam(ll_handle, "DBDXLB", tP, 1024); //返回值
             sameNoListReturn.Add("DBDXLB", tP.ToString()); 
             GetParam(ll_handle, "MZDBTCZF", tP, 1024); //返回值
             sameNoListReturn.Add("MZDBTCZF", tP.ToString());  
             GetParam(ll_handle, "MZDBLJJE", tP, 1024); //返回值
             sameNoListReturn.Add("MZDBLJJE", tP.ToString()); 
             GetParam(ll_handle, "YFDXLB", tP, 1024); //返回值
             sameNoListReturn.Add("YFDXLB", tP.ToString()); 
             GetParam(ll_handle, "MZYFTCZF", tP, 1024); //返回值
             sameNoListReturn.Add("MZYFTCZF", tP.ToString());  
             GetParam(ll_handle, "MZYFLJJE", tP, 1024); //返回值
             sameNoListReturn.Add("MZYFLJJE", tP.ToString()); 
            //取数据集的数据行数
             LocateDataSet(ll_handle, "TDMZCFXMMX");
            Int32 ll_rowsize1 = GetRowSize(ll_handle);
            Xr.Log4net.LogHelper.Info("TDMZCFXMMX行数：" + ll_rowsize1);
            //设置返回的数据列变量，并分配存储空间     
            //根据对应返回数据集的数据行数按列取出列数据


            if (ll_rowsize1 > 0)
            {
                JArray ja = new JArray();
                for (int i = 0; i < ll_rowsize1; i++)
                {
                    JObject joRow = new JObject();
                    StringBuilder tem = new StringBuilder();
                    GetFieldValue(ll_handle, "JZJLH ", tem, 50);
                    joRow.Add("JZJLH", tem.ToString());
                    GetFieldValue(ll_handle, "XMXH", tem, 50);
                    joRow.Add("XMXH", tem.ToString());
                    GetFieldValue(ll_handle, "KZRQ", tem, 50);
                    joRow.Add("KZRQ", tem.ToString());
                    GetFieldValue(ll_handle, "TZRQ", tem, 50);
                    joRow.Add("TZRQ", tem.ToString());
                    GetFieldValue(ll_handle, "XMBH ", tem, 50);
                    joRow.Add("XMBH", tem.ToString());
                    GetFieldValue(ll_handle, "YYXMBH", tem, 50);
                    joRow.Add("YYXMBH", tem.ToString());
                    GetFieldValue(ll_handle, "XMMC", tem, 50);
                    joRow.Add("XMMC", tem.ToString());
                    GetFieldValue(ll_handle, "SPMC", tem, 50);
                    joRow.Add("SPMC", tem.ToString());
                    GetFieldValue(ll_handle, "FLDM ", tem, 50);
                    joRow.Add("FLDM", tem.ToString());
                    GetFieldValue(ll_handle, "YPGG", tem, 50);
                    joRow.Add("YPGG", tem.ToString());
                    GetFieldValue(ll_handle, "YPJX", tem, 50);
                    joRow.Add("YPJX", tem.ToString());
                    GetFieldValue(ll_handle, "YPYF", tem, 50);
                    joRow.Add("YPYF", tem.ToString());
                    GetFieldValue(ll_handle, "YPJL ", tem, 50);
                    joRow.Add("YPJL", tem.ToString());
                    GetFieldValue(ll_handle, "YZLB", tem, 50);
                    joRow.Add("YZLB", tem.ToString());
                    GetFieldValue(ll_handle, "JG", tem, 50);
                    joRow.Add("JG", tem.ToString());
                    GetFieldValue(ll_handle, "MCYL", tem, 50);
                    joRow.Add("MCYL", tem.ToString());
                    GetFieldValue(ll_handle, "ZFBL ", tem, 50);
                    joRow.Add("ZFBL", tem.ToString());
                    GetFieldValue(ll_handle, "XJJE", tem, 50);
                    joRow.Add("XJJE", tem.ToString());
                    GetFieldValue(ll_handle, "JE", tem, 50);
                    joRow.Add("JE", tem.ToString());
                    GetFieldValue(ll_handle, "YPLY", tem, 50);
                    joRow.Add("YPLY", tem.ToString());
                    GetFieldValue(ll_handle, "TSYPBZ ", tem, 50);
                    joRow.Add("TSYPBZ", tem.ToString());
                    GetFieldValue(ll_handle, "XZYYBZ", tem, 50);
                    joRow.Add("XZYYBZ", tem.ToString());
                    GetFieldValue(ll_handle, "SJGYYY", tem, 50);
                    joRow.Add("SJGYYY", tem.ToString());
                    GetFieldValue(ll_handle, "DLGG", tem, 50);
                    joRow.Add("DLGG", tem.ToString());
                    GetFieldValue(ll_handle, "DWGG ", tem, 50);
                    joRow.Add("DWGG", tem.ToString());
                    GetFieldValue(ll_handle, "SYTS", tem, 50);
                    joRow.Add("SYTS", tem.ToString());
                    GetFieldValue(ll_handle, "YJJYPBM", tem, 50);
                    joRow.Add("YJJYPBM", tem.ToString());
                    GetFieldValue(ll_handle, "YYCLZCZMC", tem, 50);
                    joRow.Add("YYCLZCZMC", tem.ToString());

                    GetFieldValue(ll_handle, "YYCLSYJZCH", tem, 50);
                    joRow.Add("YYCLSYJZCH", tem.ToString());
                    GetFieldValue(ll_handle, "TXBZ", tem, 50);
                    joRow.Add("TXBZ", tem.ToString());
                    GetFieldValue(ll_handle, "BZ1", tem, 50);
                    joRow.Add("BZ1", tem.ToString());
                    GetFieldValue(ll_handle, "BZ2", tem, 50);
                    joRow.Add("BZ2", tem.ToString());
                    GetFieldValue(ll_handle, "BZ3", tem, 50);
                    joRow.Add("BZ3", tem.ToString());

                    ja.Add(joRow);
                     NextRow(ll_handle);
                }
                sameNoListReturn.Add("MZCFXMMX", ja);
            }




             LocateDataSet(ll_handle, "FYJSXMMX");
            //取数据集的数据行数
            Int32 ll_rowsize2 = GetRowSize(ll_handle);
            Xr.Log4net.LogHelper.Info("FYJSXMMX行数：" + ll_rowsize2);
            //设置返回的数据列变量，并分配存储空间

            //根据对应返回数据集的数据行数按列取出列数据
            if (ll_rowsize2 > 0)
            {
                JArray ja = new JArray();
                for (int i = 0; i < ll_rowsize2; i++)
                {
                    JObject joRow = new JObject();
                    StringBuilder tem = new StringBuilder();
                    GetFieldValue(ll_handle, "JSXM ", tem, 50);
                    joRow.Add("JSXM", tem.ToString());
                    GetFieldValue(ll_handle, "FYXJ  ", tem, 50);
                    joRow.Add("FYXJ ", tem.ToString());
                    GetFieldValue(ll_handle, "YBFY ", tem, 50);
                    joRow.Add("YBFY", tem.ToString());
                    GetFieldValue(ll_handle, "ZFJE", tem, 50);
                    joRow.Add("ZFJE", tem.ToString());

                    ja.Add(joRow);
                     NextRow(ll_handle);
                }
                sameNoListReturn.Add("MZFYJSXMMX1", ja);
            }


            HardwareInitialClass.H_DestroyInstance(ll_handle);
            return ll_rtn;

        }


        //产检结算
        public static Int32 H_Function2142(string ZYH,
            string JZRQ, string CJLB, string JZZD, string JZZDGJDM, string ZZYSXM, string PASM,
            SocialCard cadMes, JToken date, StringBuilder gezhang, StringBuilder xianjin,JObject sameNoListReturn, bool isSure, StringBuilder wrMes)
        {
            IntPtr ll_handle = HardwareInitialClass.H_CreateInstace();
            //将单项数据传入调用实例
            Int32 ll_rtn;
            ll_rtn = SetParam(ll_handle, "FN", "2142"); //设置功能号 

            if (isSure)
                ll_rtn = SetParam(ll_handle, "JSBZ", "1"); //
            else
                ll_rtn = SetParam(ll_handle, "JSBZ", "2"); //

            ll_rtn = SetParam(ll_handle, "GMSFHM", cadMes.holder_id); //

            ll_rtn = SetParam(ll_handle, "GRSXH", cadMes.user_id); //


            ll_rtn = SetParam(ll_handle, "JZLB", cadMes.blood_type); //&&
            ll_rtn = SetParam(ll_handle, "ZYH", ZYH); //&&

            ll_rtn = SetParam(ll_handle, "JZRQ", JZRQ); //
            ll_rtn = SetParam(ll_handle, "CJLB", CJLB); //
            
            //StringBuilder BZLB = new StringBuilder();
            //H_Function2091(cadMes.user_id, JZRQ, "DJBZ", BZLB);
            //ll_rtn = SetParam(ll_handle, "BZLB", BZLB.ToString()); //

            ll_rtn = SetParam(ll_handle, "JZZD", JZZD); //
            ll_rtn = SetParam(ll_handle, "JZZDGJDM", JZZDGJDM); //
            ll_rtn = SetParam(ll_handle, "ZZYSXM", ZZYSXM); //


            ll_rtn = SetParam(ll_handle, "YBZHYE", cadMes.sb_amount); //
            ll_rtn = SetParam(ll_handle, "GRZFLJ", cadMes.zfu_amount); //

            //ll_rtn = SetParam(ll_handle, "GWYTCXE", cadMes.cs_amount); //
            //ll_rtn = SetParam(ll_handle, "TSMZTCXE", cadMes.sp_amount); //

            ll_rtn = SetParam(ll_handle, "ND", cadMes.area_code); //
            ll_rtn = SetParam(ll_handle, "RYLB", cadMes.user_type); //
            ll_rtn = SetParam(ll_handle, "KH", cadMes.card); //
            ll_rtn = SetParam(ll_handle, "JYM", cadMes.linkman);
            ll_rtn = SetParam(ll_handle, "PASM", cadMes.linkman);
            ll_rtn = InsertDataSet(ll_handle);

            for (int i = 0; i < date.Count(); i++)
            {
                ll_rtn = InsertRow(ll_handle);    //创建一行
                ll_rtn = SetField(ll_handle, "XMXH", date[i]["XMXH"].ToString());
                ll_rtn = SetField(ll_handle, "KZRQ", date[i]["KZRQ"].ToString());
                ll_rtn = SetField(ll_handle, "TZRQ", date[i]["TZRQ"].ToString());

                ll_rtn = SetField(ll_handle, "YYXMBH", date[i]["YYXMBH"].ToString());
                ll_rtn = SetField(ll_handle, "YYXMMC", date[i]["YYXMMC"].ToString());
                ll_rtn = SetField(ll_handle, "SPMC", date[i]["SPMC"].ToString());

                ll_rtn = SetField(ll_handle, "YPGG", date[i]["YPGG"].ToString());
                ll_rtn = SetField(ll_handle, "YPJX", date[i]["YPJX"].ToString());
                ll_rtn = SetField(ll_handle, "YPYF", date[i]["YPYF"].ToString());
                ll_rtn = SetField(ll_handle, "YPJL", date[i]["YPJL"].ToString());

                ll_rtn = SetField(ll_handle, "YZLB", date[i]["YZLB"].ToString());
                ll_rtn = SetField(ll_handle, "JG", date[i]["JG"].ToString());
                ll_rtn = SetField(ll_handle, "MCYL", date[i]["MCYL"].ToString());
                ll_rtn = SetField(ll_handle, "JE", date[i]["JE"].ToString());
                ll_rtn = SetField(ll_handle, "YPLY", date[i]["YPLY"].ToString());
                ll_rtn = SetField(ll_handle, "TSYPBZ", date[i]["TSYPBZ"].ToString());

                ll_rtn = SetField(ll_handle, "XZYYBZ", date[i]["XZYYBZ"].ToString());
                ll_rtn = SetField(ll_handle, "SJGYYY", date[i]["SJGYYY"].ToString());

                ll_rtn = SetField(ll_handle, "DLGG", date[i]["DLGG"].ToString());
                ll_rtn = SetField(ll_handle, "DWGG", date[i]["DWGG"].ToString());
                ll_rtn = SetField(ll_handle, "SYTS", date[i]["SYTS"].ToString());
                ll_rtn = SetField(ll_handle, "YJJYPBM", date[i]["YJJYPBM"].ToString());

                ll_rtn = SetField(ll_handle, "YYCLZCZMC", date[i]["YYCLZCZMC"].ToString());
                ll_rtn = SetField(ll_handle, "YYCLSYJZCH", date[i]["YYCLSYJZCH"].ToString());
                ll_rtn = SetField(ll_handle, "TXBZ", date[i]["TXBZ"].ToString());
                ll_rtn = SetField(ll_handle, "BZ1", date[i]["BZ1"].ToString());
                ll_rtn = SetField(ll_handle, "BZ2", date[i]["BZ2"].ToString());
                ll_rtn = SetField(ll_handle, "BZ3", date[i]["BZ3"].ToString());

                ll_rtn = EndRow(ll_handle, i + 1);     //结束当前行
            }
            ll_rtn = EndDataSet(ll_handle, "GSMZCFXMDRML");  //结束当前数据集，赋予数据集的名称

            ll_rtn = RunInstance(ll_handle);
            if (ll_rtn < 0)
            {
                GetSysMessage(ll_handle, wrMes, 35);
                return ll_rtn;
            }

            StringBuilder GZZFUJE = new StringBuilder(110);
            StringBuilder GZZFEJE = new StringBuilder(111);
            StringBuilder XJZFUJE = new StringBuilder(111);
            StringBuilder XJZFEJE = new StringBuilder(111);
            StringBuilder MZDBTCZF = new StringBuilder(111);
            StringBuilder MZYFTCZF = new StringBuilder(111);

             GetParam(ll_handle, "GZZFUJE", GZZFUJE, 1024); //返回值
             GetParam(ll_handle, "GZZFEJE", GZZFEJE, 1024); //返回值
             GetParam(ll_handle, "XJZFUJE", XJZFUJE, 1024); //返回值
             GetParam(ll_handle, "XJZFEJE", XJZFEJE, 1024); //返回值

            // GetParam(ll_handle, "MZDBTCZF", MZDBTCZF, 1024); //返回值
            // GetParam(ll_handle, "MZYFTCZF", MZYFTCZF, 1024); //返回值


            gezhang.Append((float.Parse(GZZFUJE.ToString()) + float.Parse(GZZFEJE.ToString())).ToString());
            xianjin.Append((float.Parse(XJZFUJE.ToString()) + float.Parse(XJZFEJE.ToString())).ToString());

            StringBuilder tP = new StringBuilder(1024);
             GetParam(ll_handle, "FHZ", tP, 110); //返回值
            sameNoListReturn.Add("FHZ", tP.ToString());
             GetParam(ll_handle, "MSG", tP, 110); //返回值
            sameNoListReturn.Add("MSG", tP.ToString());
             GetParam(ll_handle, "JSYWH", tP, 110); //返回值
            sameNoListReturn.Add("JSYWH", tP.ToString());
             GetParam(ll_handle, "YLFYZE", tP, 110); //返回值
            sameNoListReturn.Add("YLFYZE", tP.ToString());
             GetParam(ll_handle, "GZZFUJE", tP, 110); //返回值
            sameNoListReturn.Add("GZZFUJE", tP.ToString());
             GetParam(ll_handle, "GZZFEJE ", tP, 110); //返回值
            sameNoListReturn.Add("GZZFEJE", tP.ToString());
             GetParam(ll_handle, "TCZFJE", tP, 110); //返回值
            sameNoListReturn.Add("TCZFJE", tP.ToString());
             GetParam(ll_handle, "GWYTCZF", tP, 110); //返回值
            sameNoListReturn.Add("GWYTCZF", tP.ToString());
             GetParam(ll_handle, "XJZFUJE", tP, 110); //返回值
            sameNoListReturn.Add("XJZFUJE", tP.ToString());
             GetParam(ll_handle, "XJZFEJE", tP, 110); //返回值
            sameNoListReturn.Add("XJZFEJE", tP.ToString());

             GetParam(ll_handle, "DEJSBZ", tP, 110); //返回值
            sameNoListReturn.Add("DEJSBZ", tP.ToString());
             GetParam(ll_handle, "JSQZYTCED", tP, 110); //返回值
            sameNoListReturn.Add("JSQZYTCED", tP.ToString());
             GetParam(ll_handle, "JSRQ", tP, 110); //返回值
            sameNoListReturn.Add("JSRQ", tP.ToString());
             GetParam(ll_handle, "RYLB", tP, 110); //返回值
            sameNoListReturn.Add("RYLB", tP.ToString());
             GetParam(ll_handle, "JZJLH", tP, 110); //返回值
            sameNoListReturn.Add("JZJLH", tP.ToString());
             GetParam(ll_handle, "SYSXBZ", tP, 110); //返回值
            sameNoListReturn.Add("SYSXBZ", tP.ToString());
          
            //取数据集的数据行数
             LocateDataSet(ll_handle, "CQJCCFXMMX");
            Int32 ll_rowsize1 = GetRowSize(ll_handle);
            //设置返回的数据列变量，并分配存储空间     
            //根据对应返回数据集的数据行数按列取出列数据


            if (ll_rowsize1 > 0)
            {
                JArray ja = new JArray();
                for (int i = 0; i < ll_rowsize1; i++)
                {
                    JObject joRow = new JObject();
                    StringBuilder tem = new StringBuilder();
                    GetFieldValue(ll_handle, "JZJLH ", tem, 50);
                    joRow.Add("JZJLH", tem.ToString());
                    GetFieldValue(ll_handle, "XMXH", tem, 50);
                    joRow.Add("XMXH", tem.ToString());
                    GetFieldValue(ll_handle, "KZRQ", tem, 50);
                    joRow.Add("KZRQ", tem.ToString());
                    GetFieldValue(ll_handle, "TZRQ", tem, 50);
                    joRow.Add("TZRQ", tem.ToString());
                    GetFieldValue(ll_handle, "XMBH ", tem, 50);
                    joRow.Add("XMBH", tem.ToString());
                    GetFieldValue(ll_handle, "YYXMBH", tem, 50);
                    joRow.Add("YYXMBH", tem.ToString());
                    GetFieldValue(ll_handle, "XMMC", tem, 50);
                    joRow.Add("XMMC", tem.ToString());
                    GetFieldValue(ll_handle, "SPMC", tem, 50);
                    joRow.Add("SPMC", tem.ToString());
                    GetFieldValue(ll_handle, "FLDM ", tem, 50);
                    joRow.Add("FLDM", tem.ToString());
                    GetFieldValue(ll_handle, "YPGG", tem, 50);
                    joRow.Add("YPGG", tem.ToString());
                    GetFieldValue(ll_handle, "YPJX", tem, 50);
                    joRow.Add("YPJX", tem.ToString());
                    GetFieldValue(ll_handle, "YPYF", tem, 50);
                    joRow.Add("YPYF", tem.ToString());
                    GetFieldValue(ll_handle, "YPJL ", tem, 50);
                    joRow.Add("YPJL", tem.ToString());
                    GetFieldValue(ll_handle, "YZLB", tem, 50);
                    joRow.Add("YZLB", tem.ToString());
                    GetFieldValue(ll_handle, "JG", tem, 50);
                    joRow.Add("JG", tem.ToString());
                    GetFieldValue(ll_handle, "MCYL", tem, 50);
                    joRow.Add("MCYL", tem.ToString());
                    GetFieldValue(ll_handle, "ZFBL ", tem, 50);
                    joRow.Add("ZFBL", tem.ToString());
                    GetFieldValue(ll_handle, "XJJE", tem, 50);
                    joRow.Add("XJJE", tem.ToString());
                    GetFieldValue(ll_handle, "JE", tem, 50);
                    joRow.Add("JE", tem.ToString());
                    GetFieldValue(ll_handle, "YPLY", tem, 50);
                    joRow.Add("YPLY", tem.ToString());
                    GetFieldValue(ll_handle, "TSYPBZ ", tem, 50);
                    joRow.Add("TSYPBZ", tem.ToString());
                    GetFieldValue(ll_handle, "XZYYBZ", tem, 50);
                    joRow.Add("XZYYBZ", tem.ToString());
                    GetFieldValue(ll_handle, "SJGYYY", tem, 50);
                    joRow.Add("SJGYYY", tem.ToString());
                    GetFieldValue(ll_handle, "DLGG", tem, 50);
                    joRow.Add("DLGG", tem.ToString());
                    GetFieldValue(ll_handle, "DWGG ", tem, 50);
                    joRow.Add("DWGG", tem.ToString());
                    GetFieldValue(ll_handle, "SYTS", tem, 50);
                    joRow.Add("SYTS", tem.ToString());
                    GetFieldValue(ll_handle, "YJJYPBM", tem, 50);
                    joRow.Add("YJJYPBM", tem.ToString());
                    GetFieldValue(ll_handle, "YYCLZCZMC", tem, 50);
                    joRow.Add("YYCLZCZMC", tem.ToString());

                    GetFieldValue(ll_handle, "YYCLSYJZCH", tem, 50);
                    joRow.Add("YYCLSYJZCH", tem.ToString());
                    GetFieldValue(ll_handle, "TXBZ", tem, 50);
                    joRow.Add("TXBZ", tem.ToString());
                    GetFieldValue(ll_handle, "BZ1", tem, 50);
                    joRow.Add("BZ1", tem.ToString());
                    GetFieldValue(ll_handle, "BZ2", tem, 50);
                    joRow.Add("BZ2", tem.ToString());
                    GetFieldValue(ll_handle, "BZ3", tem, 50);
                    joRow.Add("BZ3", tem.ToString());
                    ja.Add(joRow);
                     NextRow(ll_handle);
                }
                sameNoListReturn.Add("CQJCCFXMMX", ja);
            }




             LocateDataSet(ll_handle, "FYJSXMMX");
            //取数据集的数据行数
            Int32 ll_rowsize2 = GetRowSize(ll_handle);
            //设置返回的数据列变量，并分配存储空间

            //根据对应返回数据集的数据行数按列取出列数据
            if (ll_rowsize2 > 0)
            {
                JArray ja = new JArray();
                for (int i = 0; i < ll_rowsize2; i++)
                {
                    JObject joRow = new JObject();
                    StringBuilder tem = new StringBuilder();
                    GetFieldValue(ll_handle, "JSXM ", tem, 50);
                    joRow.Add("JSXM", tem.ToString());
                    GetFieldValue(ll_handle, "FYXJ  ", tem, 50);
                    joRow.Add("FYXJ ", tem.ToString());
                    GetFieldValue(ll_handle, "YBFY ", tem, 50);
                    joRow.Add("YBFY", tem.ToString());
                    GetFieldValue(ll_handle, "ZFJE", tem, 50);
                    joRow.Add("ZFJE", tem.ToString());

                    ja.Add(joRow);
                     NextRow(ll_handle);
                }
                sameNoListReturn.Add("FYJSXMMX", ja);
            }

            HardwareInitialClass.H_DestroyInstance(ll_handle);
            return ll_rtn;

        }
        #endregion

        #region 身份证读卡器

       
        //[DllImport("cardapi3.dll")]
        //public static extern Int32 OpenCardReader(Int32 lPort, UInt32 ulFlag, UInt32 ulBaudRate);


        //[DllImport("cardapi3.dll")]
        //public static extern Int32 GetPersonMsgW(ref PERSONINFOW pInfo, string pszImageFile);

        //[DllImport("cardapi3.dll")]
        //public static extern Int32 CloseCardReader();

        //[DllImport("cardapi3.dll")]
        //public static extern void GetErrorTextW(StringBuilder pszBuffer, UInt32 dwBufLen);

        //[DllImport("cardapi3.dll")]
        //public static extern Int32 GetCardReaderStatus(long lPort, UInt32 ulBaudRate);

        //[DllImport("cardapi3.dll")]
        //public static extern Int32 ResetCardReader();
        [DllImport("Sdtapi.dll")]
        public static extern int HID_BeepLED(bool BeepON, bool LEDON, int duration);

        [DllImport("Sdtapi.dll")]
        public static extern int InitComm(int iPort);
        [DllImport("Sdtapi.dll")]
        public static extern int Authenticate();
        [DllImport("Sdtapi.dll")]
        public static extern int ReadBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                    StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);
        [DllImport("Sdtapi.dll")]
        public static extern int CloseComm();
        [DllImport("Sdtapi.dll")]
        public static extern int ReadBaseMsg(byte[] pMsg, ref int len);
        [DllImport("Sdtapi.dll")]
        public static extern int ReadBaseMsgW(byte[] pMsg, ref int len);

        #endregion

        #region 杉德交易接口

        [DllImport("SandLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sand_Trans(StringBuilder TransUp, StringBuilder TransDown);

        [DllImport("SandLib.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sand_Operator(StringBuilder set,StringBuilder BuffOut);

        [DllImport("SandLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Sand_TestPos();

        #endregion

        #region 天腾银联发卡器K100
          
        [DllImport("M100A_DLL.dll")]
        public static extern IntPtr M100A_CommOpen(string Port);
        //        功能:	打开串口，默认的波特率“9600, n, 8, 1”
        //参数:	[in]*Port			要打开的串口，例如打开com1，则*Port 存储”com1”
        //返回值:	正确返回串口的句柄；错误=0

        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_CommClose(IntPtr ComHandle);
        //[3] int __stdcall M100A_CommClose(HANDLE ComHandle);
        //功能：	关闭当前打开的串口
        //参数：	[in]ComHandle		要关闭的串口的句柄
        //返回值：	正确=0，错误=非0

        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_GetSysVersion(IntPtr ComHandle, StringBuilder strVersion);


        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_SetCommBaud(IntPtr ComHandle, bool bHasMac_Addr, byte Mac_Addr, byte _Baud, StringBuilder RecrodInfo);

        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_Reset(IntPtr ComHandle, bool bHasMac_Addr, Byte Mac_Addr, Byte _PM, Byte[] _VerCode, char[] RecordInfo);
        //功能：	M100A复位命令
        //参数：	[in]ComHandle		已经打开的串口的句柄
        //        [in]bHasMac_Addr	是否为多机通讯版本(使用方式请参文档前部“接口函数公有的参数说明“)
        //        [in]MacAddr		机器的地址，有效取值（0到15）
        //        [in]_PM			复位选项
        //                        =0x30，初始化读卡器,有卡弹卡（不上传版本信息）
        //                        =0x31，初始化读卡器,有卡回收（不上传版本信息）
        //                        =0x32，初始化读卡器,有卡停在读磁卡位置（不上传版本信息）
        //                        =0x33，初始化读卡器，有卡停在读IC卡位置（不上传版本信息）
        //                        =0x34，初始化读卡器，不动作(上传版本信息)
        //        [out]_Vercode		存储的是机器的版本信息，如“TTCE_M100A_V*.**”
        //        [out]RecrodInfo		存储该条命令的通讯记录
        //返回值：	正确=0，错误=非0



        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_CheckCardPosition(IntPtr ComHandle, bool bHasMac_Addr, Byte Mac_Addr,  Byte[] CardStates,  char[] RecordInfo);
        //功能：	读取卡片在机器里的位置
        //参数：	[in]ComHandle		已经打开的串口的句柄
        //        [in]bHasMac_Addr	是否为多机通讯版本(使用方式请参文档前部“接口函数公有的参数说明“)
        //        [in]MacAddr		机器的地址，有效取值（0到15）
        //        [out]CardStates[2]	CardStates[0]，表示通道卡片位置，具体含义如下
        //                =0x30：通道无卡
        //=0x31：读磁卡位置有卡
        //=0x32：IC卡位置有卡
        //=0x33：前端夹卡位置有卡
        //=0x34：前端不夹卡位置有卡
        //=0x35：卡不在标准位置(标准位置指的是上面5个位置
        //（0x30~0x34）.当卡不在标准位置时，可以通过移动卡片
        //命令将卡移动到标准位置)
        //                        CardStates[1]，表示卡箱卡片状态
        //=0x30:卡箱无卡
        //                =0x31:卡箱卡片不足,提醒需要加卡
        //=0x32:卡箱卡片足够
        //        [out]RecrodInfo		存储该条命令的通讯记录
        //返回值：	正确=0，错误=非0

        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_EnterCard(IntPtr ComHandle, bool bHasMac_Addr, byte Mac_Addr, byte EnterType, char[] RecordInfo);
        //功能：	进卡设置，为立即返回方式，卡片一旦进入，则命令失效，卡片是否到具体位置，必须通过“读
        //取卡片在机器里的位置”命令来判断
        //参数：	[in]ComHandle		已经打开的串口的句柄
        //        [in]bHasMac_Addr	是否为多机通讯版本(使用方式请参文档前部“接口函数公有的参数说明“)
        //        [in]MacAddr		机器的地址，有效取值（0到15）
        //        [in]EnterType		具体有效值如下
        //                        0x30: 禁止进卡(将取消先前设置好的进卡指令)
        //                        0x31: 使能进卡，进卡后停卡在读磁卡位置
        //                        0x32: 使能进卡，进卡后停卡在读IC卡位置
        //                        0x33: 使能进卡，进卡后将卡回收到回收箱
        //                        0x34: 使能进卡，进卡后停卡在前端夹卡位置
        //                        0x35: 使能进卡，进卡后将卡弹出
        //        [out]RecrodInfo		存储该条命令的通讯记录
        //返回值：	正确=0，错误=非0
        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_ClearMagCardData(IntPtr ComHandle, bool bHasMac_Addr, Byte Mac_Addr, char[] RecordInfo);
            //功能：	清空存储在机器缓存中的磁卡信息
            //参数：	[in]ComHandle		已经打开的串口的句柄
            //        [in]bHasMac_Addr	是否为多机通讯版本(使用方式请参文档前部“接口函数公有的参数说明“)
            //        [in]MacAddr		机器的地址，有效取值（0到15）
            //        [out]RecrodInfo		存储该条命令的通讯记录
            //返回值：	正确=0，错误=非0
        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_MoveCard(IntPtr ComHandle, bool bHasMac_Addr, Byte Mac_Addr, Byte _PM, char[] RecordInfo);
        //功能：	卡片传动指令
        //参数：	[in]ComHandle		已经打开的串口的句柄
        //        [in]bHasMac_Addr	是否为多机通讯版本(使用方式请参文档前部“接口函数公有的参数说明“)
        //        [in]MacAddr		机器的地址，有效取值（0到15）
        //        [in]_PM			卡片传动的选项，有效值如下：
        //                        0x30: 将卡片传动到读磁卡位置
        //                        0x31: 将卡片传动到IC卡位置
        //                        0x32: 将卡片传动到前端夹卡位置
        //                        0x33: 将卡片弹出
        //                        0x34: 将卡片回收到回收箱
        //        [out]RecrodInfo		存储该条命令的通讯记录
        //返回值：	正确=0，错误=非0
        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_Eot(IntPtr ComHandle, bool bHasMac_Addr, byte Mac_Addr, char[] RecordInfo);
        //功能：	向机器发送EOT，取消命令
        //参数：	[in]ComHandle		已经打开的串口的句柄
        //        [in]bHasMac_Addr	是否为多机通讯版本(使用方式请参文档前部“接口函数公有的参数说明“)
        //        [in]MacAddr		机器的地址，有效取值（0到15）
        //        [out]RecrodInfo		存储该条命令的通讯记录
        //返回值：	正确=0，错误=非0
        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_ReadMagcardDecode(IntPtr ComHandle, bool bHasMac_Addr, byte Mac_Addr, byte _track, ref int _DataLen, Byte[] _BlockData, char[] RecordInfo);
        //功能：	读磁卡解码数据
        //参数：	[in]ComHandle		已经打开的串口的句柄
        //        [in]bHasMac_Addr	是否为多机通讯版本(使用方式请参文档前部“接口函数公有的参数说明“)
        //        [in]MacAddr		机器的地址，有效取值（0到15）
        //        [in]_track			要读取的轨道，具体含义如下
        //                        0x30: 读ISO第一轨数据
        //                        0x31: 读ISO第二轨数据
        //                        0x32: 读ISO第三轨数据
        //                        0x33: 读ISO第一二轨数据
        //                        0x34: 读ISO第一三轨数据
        //                        0x35: 读ISO第二三轨数据
        //                        0x36: 读ISO全三轨数据
        //        [out]_DataLen		存储返回的磁卡数据包的长度（_BlockData[]的长度）
        //        [out]_BlockData[]	存储返回的磁卡数据包（数据包格式说明见下）
        //        [out]RecrodInfo		存储该条命令的通讯记录
        //返回值：	正确=0，错误=非0
         [DllImport("M100A_DLL.dll")]
        public static extern int M100A_ReadMagcardUNDecode(IntPtr ComHandle, bool bHasMac_Addr, byte Mac_Addr, byte _track, ref Int32 _DataLen, Byte[] _BlockData, char[] RecordInfo);
        //功能：	读磁卡未解码数据
        //参数：	[in]ComHandle		已经打开的串口的句柄
        //        [in]bHasMac_Addr	是否为多机通讯版本(使用方式请参文档前部“接口函数公有的参数说明“)
        //        [in]MacAddr		机器的地址，有效取值（0到15）
        //        [in]_track			要读取的轨道，具体含义如下
        //                        0x30: 读ISO第一轨数据
        //                        0x31: 读ISO第二轨数据
        //                        0x32: 读ISO第三轨数据
        //                        0x33: 读ISO第一二轨数据
        //                        0x34: 读ISO第一三轨数据
        //                        0x35: 读ISO第二三轨数据
        //                        0x36: 读ISO全三轨数据
        //        [out]_DataLen		存储返回的磁卡数据包的长度（_BlockData[]的长度）
        //        [out]_BlockData[]	存储返回的磁卡数据包（数据包格式说明见下）
        //        [out]RecrodInfo		存储该条命令的通讯记录
        //返回值：	正确=0，错误=非0

        [DllImport("M100A_DLL.dll")]
        public static extern int M100A_AutoTestICCard(IntPtr ComHandle, bool bHasMac_Addr, Byte Mac_Addr, Byte[] _IcCardType, char[] RecordInfo);
        #endregion

        #region 磁卡读卡器
        //打开串口
        [DllImport("CRT_310.dll")]
        private static extern UInt32 CommOpen(string port);

        //复位读卡机//0x30=不弹卡 0x31=前端弹卡 0x32=后端弹卡
        [DllImport("CRT_310.dll")]

        private static extern int CRT310_Reset(UInt32 ComHandle, byte _Eject);
        //从读卡机读取状态信息
        [DllImport("CRT_310.dll")]
        private static extern int CRT310_GetStatus(UInt32 ComHandle, ref byte _CardStatus, ref byte _frontStatus, ref byte _RearStatus);
        //读磁轨数据
        [DllImport("CRT_310.dll")]
        private static extern int MC_ReadTrack(UInt32 ComHandle, byte _Mode, byte _track, ref int _TrackDataLen, byte[] _TrackData);
        
        //移动卡
        [DllImport("CRT_310.dll")]
        private static extern int CRT310_MovePosition(UInt32 ComHandle, byte _Position);
        //进卡控制
        [DllImport("CRT_310.dll")]
        private static extern int CRT310_CardSetting(UInt32 ComHandle, byte _CardIn, byte _EnableBackIn);
 
        //关闭串口
        [DllImport("CRT_310.dll")]
        private static extern int CommClose(UInt32 ComHandle);




        public static UInt32 CRT_310CommOpen()
        {
            return CommOpen(ConfigurationManager.AppSettings["CRT_310PORT"].ToString());
        }
        public static int CRT_310Reset(UInt32 ComHandle)
        {
            return CRT310_Reset(ComHandle, 0x01);
        }
        public static int CRT_301GetStatus(UInt32 ComHandle, ref string CardStatus, ref string FrontSetting, ref string RearSetting)
        {
            byte _CardStatus, _frontSetting, _RearSetting;
            _CardStatus = 0;
            _frontSetting = 0;
            _RearSetting = 0;
            int i = CRT310_GetStatus(ComHandle, ref _CardStatus, ref _frontSetting, ref _RearSetting);
            if (i == 0)
            {
                switch (_CardStatus)
                {
                    case 70:
                        CardStatus = "There is long card in the reader";
                        break;
                    case 72:
                        CardStatus = "There is card at the front side";
                        break;
                    case 73:
                        CardStatus = "There is card at the front card-hold position";
                        break;
                    case 74:
                        CardStatus = "There is card in the reader";
                        break;
                    case 75:
                        CardStatus = "There is card at IC card operation position";
                        break;
                    case 76:
                        CardStatus = "There is card at the rear card-hold position";
                        break;
                    case 77:
                        CardStatus = "There is card at the rear side";
                        break;
                    case 78:
                        CardStatus = "No Card In The Reader";
                        break;
                }
                switch (_frontSetting)
                {
                    case 73:
                        FrontSetting = "Permit magnetic cards in only from front";
                        break;
                    case 74:
                        FrontSetting = "Permit all cards in from front";
                        break;
                    case 75:
                        FrontSetting = "Permit card-in by mag signal";
                        break;
                    case 78:
                        FrontSetting = "Prohibit the cards in from front";
                        break;
                }
                switch (_RearSetting)
                {
                    case 74:
                        RearSetting = "Permit all cards in from front";
                        break;
                    case 78:
                        RearSetting = "Prohibit the cards in from front";
                        break;
                }
                //MessageBox.Show("Card Status:  " + Sb1 + "\r\n" + "frontSetting:  " + Sb2 + "\r\n" + "_RearSetting:  " + Sb3, "Status");
                return i ;
            }
            else 
            {
                //PrintErrorMessage();
                return i;
            }
        }
        public static int CRT_310ReadTrack(UInt32 ComHandle,ref string Track1Data,ref string Track2Data,ref string Track3Data)
        {
            int _TrackDataLen = 0;
            byte[] _TrackData = new byte[500];
            int refInt=-1;
            refInt= MC_ReadTrack(ComHandle, 0x30, 0x37, ref _TrackDataLen, _TrackData);
             if (refInt == 0)
                {
                    int n;
                    int weizhi1 = 0;
                    int weizhi2 = 0; int weizhi3 = 0;
                    string Tra1Buf = "";
                    string Tra2Buf = "";
                    string Tra3Buf = "";

                    for (n = 0; n < _TrackDataLen; n++)
                    {
                        if (_TrackData[n] == 31)
                        {
                            weizhi1 = n;
                            break;
                        }
                    }
                    for (n = weizhi1 + 1; n < _TrackDataLen; n++)
                    {
                        if (_TrackData[n] == 31)
                        {
                            weizhi2 = n;
                            break;
                        }
                    }
                    for (n = weizhi2 + 1; n < _TrackDataLen; n++)
                    {
                        if (_TrackData[n] == 31)
                        {
                            weizhi3 = n;
                            break;
                        }
                    }
                    switch (_TrackData[weizhi1 + 1])
                    {
                        case 89:
                            for (n = weizhi1 + 2; n < weizhi2 ; n++)
                            {
                                //Tra1Buf += _TrackData[n].ToString(); 
                                Tra1Buf += (char)_TrackData[n];
                            }
                            Track1Data = Tra1Buf;
                            break;
                        case 78:
                            Track1Data = "Read/Parity Error" + Environment.NewLine;
                            switch (_TrackData[weizhi1 + 2])
                            {
                                case 225:
                                    Track1Data= Track1Data + "No start bits (STX)";
                                    break;
                                case 226:
                                    Track1Data = Track1Data + "No stop bits (ETX)";
                                    break;
                                case 227:
                                    Track1Data = Track1Data + "Byte Parity Error(Parity))";
                                    break;
                                case 228:
                                    Track1Data = Track1Data + "Parity Bit Error(LRC)";
                                    break;
                                case 229:
                                    Track1Data = Track1Data + "Card Track Data is Blank";
                                    break;
                            }
                            break;
                        case 69:
                            Track1Data = "No Read for this Track" + Environment.NewLine + "Card Track Data is 0xE0";
                            break;
                        case 0:
                            Track1Data = "No Read Operation" + Environment.NewLine + "Card Track Data is 0x00";
                            break;
                    }
                    switch (_TrackData[weizhi2 + 1])
                    {
                        case 89:
                            for (n = weizhi2 + 2; n < weizhi3 ; n++)
                            {
                                //Tra1Buf += _TrackData[n].ToString(); 
                                Tra2Buf += (char)_TrackData[n];
                            }
                            Track2Data = Tra2Buf;
                            break;
                        case 78:
                            Track2Data = "Read/Parity Error" + Environment.NewLine;
                            switch (_TrackData[weizhi2 + 2])
                            {
                                case 225:
                                    Track2Data = Track2Data + "No start bits (STX)";
                                    break;
                                case 226:
                                    Track2Data = Track2Data + "No stop bits (ETX)";
                                    break;
                                case 227:
                                    Track2Data = Track2Data + "Byte Parity Error(Parity))";
                                    break;
                                case 228:
                                    Track2Data = Track2Data + "Parity Bit Error(LRC)";
                                    break;
                                case 229:
                                    Track2Data = Track2Data + "Card Track Data is Blank";
                                    break;
                            }
                            break;
                        case 69:
                            Track2Data = "No Read for this Track" + Environment.NewLine + "Card Track Data is 0xE0";
                            break;
                        case 0:
                            Track2Data = "No Read Operation" + Environment.NewLine + "Card Track Data is 0x00";
                            break;
                    }
                    switch (_TrackData[weizhi3 + 1])
                    {
                        case 89:
                            for (n = weizhi3 + 2; n < _TrackDataLen ; n++)
                            {
                                //Tra1Buf += _TrackData[n].ToString(); 
                                Tra3Buf += (char)_TrackData[n];
                            }
                            Track3Data = Tra3Buf;
                            break;
                        case 78:
                            Track3Data = "Read/Parity Error" + Environment.NewLine;
                            switch (_TrackData[weizhi3 + 2])
                            {
                                case 225:
                                    Track3Data = Track3Data + "No start bits (STX)";
                                    break;
                                case 226:
                                    Track3Data = Track3Data + "No stop bits (ETX)";
                                    break;
                                case 227:
                                    Track3Data = Track3Data + "Byte Parity Error(Parity))";
                                    break;
                                case 228:
                                    Track3Data = Track3Data + "Parity Bit Error(LRC)";
                                    break;
                                case 229:
                                    Track3Data = Track3Data + "Card Track Data is Blank";
                                    break;
                            }
                            break;
                        case 69:
                            Track3Data = "No Read for this Track" + Environment.NewLine + "Card Track Data is 0xE0";
                            break;
                        case 0:
                            Track3Data = "No Read Operation" + Environment.NewLine + "Card Track Data is 0x00";
                            break;
                    }
                    //MessageBox.Show("Mag-Card Read OK", "Mag-Card Read");
                }
            return refInt;
        }
        public static int CRT310MovePosition(UInt32 ComHandle)
        {
            return CRT310_MovePosition(ComHandle, 0x01);// 0x01前端移出 0x06 后端移出
        }
        public static int CRT310CardSetting(UInt32 ComHandle)
        {
            //_CardIn;    0x1=Prohibit the cards in    0x2=Card in by mag signal and  switch  0x3=Permit all cards in  0x4=Card in by mag signal      0x3=Permit all cards in               0x4=Card in by mag signal
            //_EnableBackIn;      0x0=Prohibit the cards in    0x1=Permit all cards in。
            int i = CRT310_CardSetting(ComHandle, 0x1, 0x1);

            /*if (i == 0)
            {
                MessageBox.Show("Enter Control OK", "Enter Control");
            }
            else if (i == -1)
            {
               // PrintErrorMessage();
            }
            else
            {
                MessageBox.Show("unknow Error", "Caution");
            }
             */
            return i;
        }
        public static int CRT310CommClose(UInt32 ComHandle)
        {
            return CommClose(ComHandle);
        }


        #endregion
    }
}
