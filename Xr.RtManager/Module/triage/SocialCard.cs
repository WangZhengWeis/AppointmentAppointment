using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xr.RtManager.Module.triage
{
    public class SocialCard
    {
        public string message_type;	         /* 信息类型 固定值为1，为了与返回结果信息区别 */
        public string user_id;                      /* 参保号 */
        public string company_code;         /*  单位编号*/
        public string holder_id;                /*  身份证号  */
        public string name;               /*  姓名  */
        public string sex;
        public string birthday;                   /*  出生年月 */
        public string user_type;                /*  人员类别 */
        //00-在职，10-退休，11-离休。
        //   21-机关单位离休，22-企事业单位离休，23-残疾军人离休
        public string telephone;               /*  电话号码 */
        public string blood_type;                  /*  卡种 综合--0  住院--1   特殊病种--2*/
        public string ill_history;             /* 交易时间 年4月2日2时2分2秒2  */
        public string h_ill_history;            /*  过敏史 */
        public string address1;            /* 通讯地址 */
        public string post_id;                 /* 邮政编码 */
        public string linkman;                /*  联系人 */
        public string area_code;                  /*  社保统筹年度 */
        public string cs_amount;              /*   公务员年度统筹金额统筹金额   */
        public string sb_amount;              /*   个人医疗帐户金额 */
        public string sp_amount;        /*   年度特殊病种金额 */
        public string zfu_amount;              /*   年度自付累计金额 */
        public string card;         /* 卡号 */
        public string return_msg;  //返回信息



        public SocialCard()
        {
         
        }
        public bool kougezhang(PosTranClass posTran)
        {
            StringBuilder tem = new StringBuilder();
            tem.Append(posTran.message_type);
            tem.Append(posTran.ill_code);
            tem.Append(posTran.q_amount);
            tem.Append(posTran.p_amount);
            tem.Append(posTran.f_amount);
            tem.Append(posTran.s_amount);
            tem.Append(posTran.c_amount);
            tem.Append(posTran.e_amount);
            tem.Append(posTran.yy_code);
            tem.Append(posTran.flag);
            tem.Append(posTran.datetime);
            Xr.Log4net.LogHelper.Info("个帐参数：" + tem.ToString());
            int aa = HardwareInitialClass.SendPosInfo(tem);
            Xr.Log4net.LogHelper.Info("回参：" + tem);
            string[] resultValue = tem.ToString().Split('|');
            string result = resultValue[0];
            if (result != "30") {
                posTran = null;
                StringBuilder error = new StringBuilder();
                aa = HardwareInitialClass.GetErrorMsg(error);
                Xr.Log4net.LogHelper.Info("扣个账失败原因：" + error.ToString());
                return_msg = "扣个账失败原因：" + error.ToString();
                return false;
            }
            else
            {
                string temm = "没有流水号";
                if (resultValue.Length > 1 && resultValue[1] != null)
                {
                    temm = resultValue[1];
                }
                posTran.liushuihao = temm;

                posTran.GRZHKKQYE = sb_amount;
                posTran.GRZHZF = posTran.s_amount;
                posTran.GWYTCXEYE = posTran.e_amount;
                posTran.TSBZKFQYE = sp_amount;
                posTran.JFLB = posTran.flag;
                posTran.ZFLJ = temm;
                posTran.liushuihao = temm;
                return true;
            }

            //StringBuilder tem = new StringBuilder();
            //tem.Append("1");
            //tem.Append("        ");
            //tem.Append("000000000001");
            //tem.Append("000000000000");
            //tem.Append("000000000001");
            //tem.Append("000000000002");
            //tem.Append("000000000000");
            //tem.Append("000000000000");
            //tem.Append("     ");
            //tem.Append("1");
            //tem.Append("20170307152323");
            //int aa = HardwareInitialClass.SendPosInfo(tem);
            //string result = (tem.ToString().Split('|'))[0]; ;
            //if (result != "30")
            //    posTran = null;
        }
        public bool cexiao(string liushuihao)
        {
            StringBuilder tem = new StringBuilder();
            tem.Append(liushuihao);

            int aa = HardwareInitialClass.SaleVoid(tem);
            
            if (aa != 0)
            {
                HardwareInitialClass.GetErrorMsg(tem);
                return_msg = tem + "";
                Xr.Log4net.LogHelper.Info("撤销失败：" + liushuihao);
                Xr.Log4net.LogHelper.Info("撤销失败：" + liushuihao);
                return false;
            }
            else
                return true;
        }
        public int readCard()
        {
            ////读取社保卡数据
            byte[] cardMeassageByte = new byte[225];

            int temR = HardwareInitialClass.GetPosInfo(cardMeassageByte);//等待刷卡     
            if (temR == 0)
            {
                message_type = Encoding.ASCII.GetString(cardMeassageByte, 0, 1).Trim();
                user_id = Encoding.Default.GetString(cardMeassageByte, 1, 8).Trim();
                company_code = Encoding.Default.GetString(cardMeassageByte, 9, 15).Trim();

                holder_id = Encoding.Default.GetString(cardMeassageByte, 24, 19).Trim();
                name = Encoding.Default.GetString(cardMeassageByte, 43, 10).Trim();
                if (Encoding.Default.GetString(cardMeassageByte, 53, 1).Trim() == "0")
                    sex = "1";
                else
                    sex = "2";
                user_id = Encoding.Default.GetString(cardMeassageByte, 1, 8).Trim(); //LogClass.WriteLog("user_id:" + user_id);
                birthday = Encoding.Default.GetString(cardMeassageByte, 54, 8).Trim(); //LogClass.WriteLog("birthday:" + birthday);
                user_type = Encoding.Default.GetString(cardMeassageByte, 62, 2).Trim(); //LogClass.WriteLog("user_type:" + user_type);
                telephone = Encoding.Default.GetString(cardMeassageByte, 64, 12).Trim();// LogClass.WriteLog("telephone:" + telephone);
                blood_type = Encoding.Default.GetString(cardMeassageByte, 76, 1).Trim(); //LogClass.WriteLog("blood_type:" + blood_type);
                ill_history = Encoding.Default.GetString(cardMeassageByte, 77, 14).Trim(); //LogClass.WriteLog("ill_history:" + ill_history);
                h_ill_history = Encoding.Default.GetString(cardMeassageByte, 91, 12).Trim(); //LogClass.WriteLog("h_ill_history:" + h_ill_history);
                address1 = Encoding.Default.GetString(cardMeassageByte, 103, 40).Trim(); //LogClass.WriteLog("address1:" + address1);
                post_id = Encoding.Default.GetString(cardMeassageByte, 143, 6).Trim(); //LogClass.WriteLog("post_id:" + post_id);
                linkman = Encoding.Default.GetString(cardMeassageByte, 149, 10).Trim(); //LogClass.WriteLog("linkman:" + linkman);
                area_code = Encoding.Default.GetString(cardMeassageByte, 159, 2).Trim(); //LogClass.WriteLog("area_code:" + area_code);
                cs_amount = Encoding.Default.GetString(cardMeassageByte, 161, 12).Trim(); //LogClass.WriteLog("cs_amount:" + cs_amount);
                sb_amount = Encoding.Default.GetString(cardMeassageByte, 173, 12).Trim(); //LogClass.WriteLog("sb_amount:" + sb_amount);
                sp_amount = Encoding.Default.GetString(cardMeassageByte, 185, 12).Trim(); //LogClass.WriteLog("sp_amount:" + sp_amount);
                zfu_amount = Encoding.Default.GetString(cardMeassageByte, 197, 12).Trim(); //LogClass.WriteLog("zfu_amount:" + zfu_amount);
                card = Encoding.Default.GetString(cardMeassageByte, 209, 16).Trim(); //LogClass.WriteLog("card:" + card);
                //LogClass.WriteLog("个账金额：" + sb_amount);
            }
            else
            {
                StringBuilder msg = new StringBuilder();
                HardwareInitialClass.GetErrorMsg(msg);
                return_msg = "社保读卡失败：" + temR + "|" + msg;
                //Xr.Log4net.LogHelper.Info(return_msg);
                //LogClass.WriteLog("社保读卡失败：" + temR + "|" + msg);
                int resetR = HardwareInitialClass.ResetDev();//读卡失败时，进行复位
                //Xr.Log4net.LogHelper.Info("复位结果：" + resetR);
                //LogClass.WriteLog("复位结果：" + resetR);
            }

            return temR;
        }

        public int cancelReadCard()
        {
            return HardwareInitialClass.CancelAccept();//取消刷卡     
        }
    }
    public class PosTranClass
    {
        public string message_type;	/* 信息类型 固定值为’1’ */
        public string ill_code;	/* 挂号住院编号 	*/
        public string q_amount;	/* 个人 自付金额 */
        public string p_amount;	/* 特殊病种（住院）统筹支付金额 */
        public string f_amount;	/* 个人自费金额 */
        public string s_amount;	/* 医保个帐支付额 */
        public string c_amount;        /* 公务员统筹支付金额 */
        public string e_amount;        /*  补充统筹金额 */
        public string yy_code;	/* 医院编号  */
        public string flag;		/* 交费类别，0 门诊，1 住院，2 离休门诊; 3 离休住院 ;4社保住院，5特殊门诊  */
        public string datetime;/* 交易时间:年4月2日2时2分2秒2 ;共14位 */

        public string liushuihao;

        public string NO;
        public string GRZHKKQYE;
        public string GRZHZF;
        public string GWYTCXEYE;
        public string TSBZKFQYE;
        public string JFLB;
        public string ZFLJ;



        /// <summary>
        /// 单位  元
        /// </summary>
        /// <param name="ill_code"></param>
        /// <param name="q_amount"></param>
        /// <param name="p_amount"></param>
        /// <param name="f_amount"></param>
        /// <param name="s_amount"></param>
        /// <param name="c_amount"></param>
        /// <param name="e_amount"></param>
        public PosTranClass(string ill_code, string q_amount, string p_amount, string f_amount, string s_amount, string c_amount, string e_amount)
        {
            this.message_type = "1";
            float q_amountF;
            if (!float.TryParse(q_amount, out q_amountF)) q_amountF = 0;
            else q_amountF = q_amountF * 100;

            float p_amountF;
            if (!float.TryParse(p_amount, out p_amountF)) p_amountF = 0;
            else p_amountF = p_amountF * 100;

            float f_amountF;
            if (!float.TryParse(f_amount, out f_amountF)) f_amountF = 0;
            else f_amountF = f_amountF * 100;


            float s_amountF;
            if (!float.TryParse(s_amount, out s_amountF)) s_amountF = 0;
            else s_amountF = s_amountF * 100;

            float c_amountF;
            if (!float.TryParse(c_amount, out c_amountF)) c_amountF = 0;
            else c_amountF = c_amountF * 100;

            float e_amountF;
            if (!float.TryParse(e_amount, out e_amountF)) e_amountF = 0;
            else e_amountF = e_amountF * 100;


            this.ill_code = ill_code.PadLeft(8, ' ');//分
            this.q_amount = q_amountF.ToString().PadLeft(12, '0');
            this.p_amount = p_amountF.ToString().PadLeft(12, '0');
            this.f_amount = f_amountF.ToString().PadLeft(12, '0');
            this.s_amount = s_amountF.ToString().PadLeft(12, '0');
            this.c_amount = c_amountF.ToString().PadLeft(12, '0');
            this.e_amount = e_amountF.ToString().PadLeft(12, '0');
            this.yy_code = " H003";
            this.flag = "1";
            this.datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
