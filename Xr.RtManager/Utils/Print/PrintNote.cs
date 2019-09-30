using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
//using ZXing;
//using ZXing.Common;

namespace Xr.RtManager.Utils
{
    public class PrintNote
    {

        private string HospitalName;
        private string DeptName;
        private string ClinicName;
        private string MzType;
        private string DoctorPrice;
        private string DoctorName;
        private string PatientId;
        private string QueueNum;
        private string Name;
        private string WaitingNum;
        private string Time;
        private string TipMsg;
        private string DoctorTip;
        private string RegVisitTime; //预约时间

        /// <summary>
        /// 打印参数
        /// </summary>
        /// <param name="HospitalName">医院名称</param>
        /// <param name="DeptName">科室名称</param>
        /// <param name="ClinicName">诊室名称</param>
        /// <param name="QueueNum">队列名称</param>
        /// <param name="Name">患者姓名</param>
        /// <param name="WaitingNum">等候人数</param>
        /// <param name="Time">打印时间</param>
        public PrintNote(string HospitalName, string DeptName, string ClinicName, string QueueNum, string Name, string WaitingNum, string Time, string TipMsg, string DoctorTip, string RegVisitTime)
        {
            this.HospitalName = HospitalName;
            this.DeptName = DeptName;
            this.ClinicName = ClinicName;
            this.QueueNum = QueueNum;
            this.Name = Name;
            this.WaitingNum = WaitingNum;
            this.Time = Time;
            this.TipMsg = TipMsg;
            this.DoctorTip = DoctorTip;
            this.RegVisitTime = RegVisitTime;
        }
        public PrintNote(PrintNoteEntity data)
        {
            this.HospitalName = data.HospitalName;
            this.DeptName = data.DeptName;
            this.ClinicName = data.ClinicName;
            this.MzType = data.MzType;
            this.DoctorPrice = data.DoctorPrice;
            this.DoctorName = data.DoctorName;
            this.PatientId = data.PatientId;
            this.QueueNum = data.QueueNum;
            this.Name = data.Name;
            this.WaitingNum = data.WaitingNum;
            this.Time = data.Time;
            this.TipMsg = data.TipMsg;
            this.DoctorTip = data.DoctorTip;
            this.RegVisitTime = data.RegVisitTime;
        }
        public bool Print(ref string message)
        {
            try
            {
                String zt = "微软雅黑";
                //生成条形码图片并保存
                EncodingOptions encodeOption = new EncodingOptions();
                encodeOption.Height = 50; // 必须制定高度、宽度
                encodeOption.Width = 220;
                ZXing.BarcodeWriter wr = new BarcodeWriter();
                wr.Options = encodeOption;
                wr.Format = BarcodeFormat.CODE_128; //  条形码规格：EAN13规格：12（无校验位）或13位数字
                Bitmap img = wr.Write(PatientId); // 生成图片
                Bitmap tempBmp = new Bitmap(img);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                PrintRow srow0 = new PrintRow(0, HospitalName, new Font(zt, 16), System.Drawing.Brushes.Black, 12);
                PrintRow srow1 = new PrintRow(1, "-------------------------------------------", new Font(zt, 10), System.Drawing.Brushes.Black, 30);
                if (!AppContext.AppConfig.LocalDeptName.Equals(""))
                {
                    DeptName = AppContext.AppConfig.LocalDeptName;
                }
                PrintRow srow2 = new PrintRow(2, DeptName + " - " + ClinicName, new Font(zt, 15), System.Drawing.Brushes.Black, 45);//11 
                if (DeptName.Length > 6)
                {
                    srow2 = new PrintRow(2, DeptName + "-" + ClinicName, new Font(zt, 12), System.Drawing.Brushes.Black, 45,3);
                }
                string mzStr = "";
                if (QueueNum.IndexOf("复") == -1)
                {
                    if (MzType != "1")
                    {
                        //mzStr = "-特需(" + DoctorPrice + "元)";
                        mzStr = "-特需";
                    }
                    else
                    {
                        mzStr = "-普通";
                    }
                }
                PrintRow srow3 = new PrintRow(3, QueueNum + mzStr, new Font(zt, 13), System.Drawing.Brushes.Black, 80);
                PrintRow srow4 = new PrintRow(4, "患者ID：" + PatientId, new Font(zt, 8), System.Drawing.Brushes.Black, 100, 5);
                PrintRow srow5 = new PrintRow(6, "患者姓名：" + Name, new Font(zt, 8), System.Drawing.Brushes.Black, 130, 5);
                //PrintRow srow5 = new PrintRow(4, "排队人数：" + WaitingNum + "人", new Font(zt, 8), System.Drawing.Brushes.Black, 100, 5);
                PrintRow srow6 = new PrintRow(5, "医       生：" + DoctorName, new Font(zt, 8), System.Drawing.Brushes.Black, 115, 5);
                Order tempOrders;
                //String ts1 = "预约号迟到半个小时签到此号作废";
                //String ts2 = "所有号过号3次作废";

                int tipMsgRow = 0;
                if((TipMsg.Length+1)%20==0){
                    tipMsgRow = (TipMsg.Length+1) / 18;
                }
                else
                {
                    tipMsgRow = (TipMsg.Length+1) / 18 + 1;
                }
                int doctorTipRow = 0;
                if ((DoctorTip.Length+1) % 20 == 0)
                {
                    doctorTipRow = (DoctorTip.Length+1) / 18;
                }
                else
                {
                    doctorTipRow = (DoctorTip.Length+1) / 18 + 1;
                }
                int pageHeight;
                if ( QueueNum.IndexOf("复") == -1&&(RegVisitTime != null || RegVisitTime != String.Empty))
                {
                    //PrintRow srow6 = new PrintRow(6, "就诊人：" + Name, new Font(zt, 8), System.Drawing.Brushes.Black, 130, 5);
                    PrintRow srow7 = new PrintRow(7, "预计看诊时间：" + RegVisitTime, new Font(zt, 8), System.Drawing.Brushes.Black, 130, 5);
                    PrintRow srow8 = new PrintRow(8, "分诊时间：" + Time, new Font(zt, 8), System.Drawing.Brushes.Black, 145, 5);

                    PrintRow srow9 = new PrintRow(9, "请提前到候诊区候诊,注意屏幕提示", new Font(zt, 8), System.Drawing.Brushes.Green, 160, 5);//30
                    PrintRow srow10 = new PrintRow(10, "具体时间按医生看诊情况", new Font(zt, 8), System.Drawing.Brushes.Green, 175, 5);//30
                    PrintRow srow11 = new PrintRow(11, "-------------------------------------------", new Font(zt, 8), System.Drawing.Brushes.Black, 190);//30
                    ////患者信息块
                    //PrintRow srow11 = new PrintRow(12, "姓名：" + Name, new Font(zt, 10), System.Drawing.Brushes.Black, 200);//10
                    //PrintRow srow12 = new PrintRow(13, "" , new Font(zt, 10), System.Drawing.Brushes.Black, 220, 45);//30
                    ////条形码占位50
                    //PrintRow srow13 = new PrintRow(11, "", new Font("宋体", 10), Brushes.Black, 220, tempBmp);

                    //PrintRow srow14 = new PrintRow(14, "-------------------------------------------", new Font(zt, 10), System.Drawing.Brushes.Black, 265);//共计加200
                    PrintRow srow15 = new PrintRow(15, "预约号迟到半个小时签到此号作废", new Font(zt, 8), System.Drawing.Brushes.Green, 280, 5);
                    PrintRow srow16 = new PrintRow(16, "候诊号过号3次作废", new Font(zt, 8), System.Drawing.Brushes.Green, 295, 5);
                    //PrintRow srow9 = new PrintRow(9, "", new Font("宋体", 10), System.Drawing.Brushes.Green, 245, 15);

                    if ((TipMsg != null && TipMsg.Trim().Length > 0) && (DoctorTip != null && DoctorTip.Trim().Length > 0))
                    {
                        PrintRow srow17 = new PrintRow(17, "医生候诊提醒：", new Font(zt, 8), System.Drawing.Brushes.Green, 310, 5);
                        PrintRow srow18 = new PrintRow(18, DoctorTip, new Font(zt, 8), System.Drawing.Brushes.Green, 325 , 15);
                        Log4net.LogHelper.Info(DoctorTip);
                        PrintRow srow19 = new PrintRow(19, "说明：", new Font(zt, 8), System.Drawing.Brushes.Green, 325 + doctorTipRow * 15, 5);
                        PrintRow srow20 = new PrintRow(20,  TipMsg, new Font(zt, 8), System.Drawing.Brushes.Green, 340 +doctorTipRow * 15, 15);
                        //PrintRow srow18 = new PrintRow(18, ts1, new Font(zt, 10), System.Drawing.Brushes.Green, 495 + tipMsgRow * 20 + doctorTipRow * 20);
                        //PrintRow srow19 = new PrintRow(19, ts2, new Font(zt, 10), System.Drawing.Brushes.Green, 515 + tipMsgRow * 20 + doctorTipRow * 20);
                        tempOrders = new Order(new List<PrintRow>() { srow0, srow1, srow2, srow3, srow4,srow5, srow6, srow7, srow8, srow9, srow10, srow11, srow15, srow16, srow17, srow18, srow19, srow20 });
                        pageHeight = 345 + tipMsgRow * 15 + doctorTipRow * 15;
                    }
                    else if ((TipMsg != null && TipMsg.Trim().Length > 0) && (DoctorTip == null || DoctorTip.Trim().Length == 0))
                    {
                        PrintRow srow17 = new PrintRow(17, "说明：", new Font(zt, 8), System.Drawing.Brushes.Green, 310, 5);
                        PrintRow srow18 = new PrintRow(18, TipMsg, new Font(zt, 8), System.Drawing.Brushes.Green, 325, 15);
                        //PrintRow srow16 = new PrintRow(16, "2." + TipMsg, new Font(zt, 10), System.Drawing.Brushes.Green, 495, 10);
                        //PrintRow srow17 = new PrintRow(17, ts1, new Font(zt, 10), System.Drawing.Brushes.Green, 495 + tipMsgRow * 20);
                        //PrintRow srow18 = new PrintRow(18, ts2, new Font(zt, 10), System.Drawing.Brushes.Green, 515 + tipMsgRow * 20);
                        tempOrders = new Order(new List<PrintRow>() { srow0, srow1, srow2, srow3, srow4, srow5, srow6, srow7, srow8, srow9, srow10, srow11, srow15, srow16, srow17, srow18 });
                        pageHeight = 335 + tipMsgRow * 15;
                    }
                    else if ((TipMsg == null || TipMsg.Trim().Length == 0) && (DoctorTip != null && DoctorTip.Trim().Length > 0))
                    {
                        PrintRow srow17 = new PrintRow(17, "医生候诊提醒：", new Font(zt, 8), System.Drawing.Brushes.Green, 310, 5);
                        PrintRow srow18 = new PrintRow(18, DoctorTip, new Font(zt, 8), System.Drawing.Brushes.Green, 325, 15);
                        //PrintRow srow16 = new PrintRow(16, "2." + DoctorTip, new Font(zt, 10), System.Drawing.Brushes.Green, 495, 10);
                        //PrintRow srow17 = new PrintRow(17, ts1, new Font(zt, 10), System.Drawing.Brushes.Green, 495 + doctorTipRow * 20);
                        //PrintRow srow18 = new PrintRow(18, ts2, new Font(zt, 10), System.Drawing.Brushes.Green, 515 + doctorTipRow * 20);
                        tempOrders = new Order(new List<PrintRow>() { srow0, srow1, srow2, srow3, srow4, srow5, srow6, srow7, srow8, srow9, srow10, srow11, srow15, srow16, srow17, srow18 });
                        pageHeight = 335 + doctorTipRow * 15;
                    }
                    else
                    {
                        //PrintRow srow16 = new PrintRow(16, ts1, new Font(zt, 10), System.Drawing.Brushes.Green, 495);
                        //PrintRow srow17 = new PrintRow(17, ts2, new Font(zt, 10), System.Drawing.Brushes.Green, 515);
                        tempOrders = new Order(new List<PrintRow>() { srow0, srow1, srow2, srow3, srow4, srow5, srow6, srow7, srow8, srow9, srow10, srow11, srow15, srow16 });
                        pageHeight = 315;
                    }
                }
                else
                {
                    //PrintRow srow6 = new PrintRow(6, "就诊人：" + Name, new Font(zt, 8), System.Drawing.Brushes.Black, 130, 5);
                    PrintRow srow7 = new PrintRow(7, "分诊时间：" + Time, new Font(zt, 8), System.Drawing.Brushes.Black, 130, 5);

                    PrintRow srow8 = new PrintRow(8, "请提前到候诊区候诊,注意屏幕提示", new Font(zt, 8), System.Drawing.Brushes.Green, 145, 5);//30
                    PrintRow srow9 = new PrintRow(9, "具体时间按医生看诊情况", new Font(zt, 8), System.Drawing.Brushes.Green, 160, 5);//30
                    PrintRow srow10 = new PrintRow(10, "-------------------------------------------", new Font(zt, 8), System.Drawing.Brushes.Black, 175);//30
                    ////患者信息块
                    //PrintRow srow10 = new PrintRow(12, "姓名：" + Name, new Font(zt, 10), System.Drawing.Brushes.Black, 185);//10
                    //PrintRow srow11 = new PrintRow(13, "", new Font(zt, 8), System.Drawing.Brushes.Black, 200, 5);//30
                    ////条形码占位50
                    //PrintRow srow12 = new PrintRow(11, "", new Font("宋体", 8), Brushes.Black, 200, tempBmp);

                    //PrintRow srow13 = new PrintRow(14, "-------------------------------------------", new Font(zt, 8), System.Drawing.Brushes.Black, 245);//共计加200
                    PrintRow srow14 = new PrintRow(15, "预约号迟到半个小时签到此号作废", new Font(zt, 8), System.Drawing.Brushes.Green, 255, 5);
                    PrintRow srow15 = new PrintRow(16, "候诊号过号3次作废", new Font(zt, 8), System.Drawing.Brushes.Green, 270, 5);
                    //PrintRow srow9 = new PrintRow(9, "", new Font("宋体", 10), System.Drawing.Brushes.Green, 245, 15);

                    if ((TipMsg != null && TipMsg.Trim().Length > 0) && (DoctorTip != null && DoctorTip.Trim().Length > 0))
                    {
                        PrintRow srow16 = new PrintRow(17, "医生候诊提醒：", new Font(zt, 8), System.Drawing.Brushes.Green, 285, 5);
                        PrintRow srow17 = new PrintRow(18, DoctorTip, new Font(zt, 8), System.Drawing.Brushes.Green, 300, 15);
                        Log4net.LogHelper.Info(DoctorTip);
                        PrintRow srow18 = new PrintRow(19, "说明：", new Font(zt, 8), System.Drawing.Brushes.Green, 300 + doctorTipRow * 15, 5);
                        PrintRow srow19 = new PrintRow(20, TipMsg, new Font(zt, 8), System.Drawing.Brushes.Green, 315  + doctorTipRow * 15, 15);
                        //PrintRow srow18 = new PrintRow(18, ts1, new Font(zt, 10), System.Drawing.Brushes.Green, 495 + tipMsgRow * 20 + doctorTipRow * 20);
                        //PrintRow srow19 = new PrintRow(19, ts2, new Font(zt, 10), System.Drawing.Brushes.Green, 515 + tipMsgRow * 20 + doctorTipRow * 20);
                        tempOrders = new Order(new List<PrintRow>() { srow0, srow1, srow2, srow3, srow4, srow5, srow6, srow7, srow8, srow9, srow10, srow14, srow15, srow16, srow17, srow18, srow19 });
                        pageHeight = 320 + tipMsgRow * 15 + doctorTipRow * 15;
                    }
                    else if ((TipMsg != null && TipMsg.Trim().Length > 0) && (DoctorTip == null || DoctorTip.Trim().Length == 0))
                    {
                        PrintRow srow16 = new PrintRow(17, "说明：", new Font(zt, 8), System.Drawing.Brushes.Green, 285, 5);
                        PrintRow srow17 = new PrintRow(18, TipMsg, new Font(zt, 8), System.Drawing.Brushes.Green, 300, 15);
                        //PrintRow srow16 = new PrintRow(16, "2." + TipMsg, new Font(zt, 10), System.Drawing.Brushes.Green, 495, 10);
                        //PrintRow srow17 = new PrintRow(17, ts1, new Font(zt, 10), System.Drawing.Brushes.Green, 495 + tipMsgRow * 20);
                        //PrintRow srow18 = new PrintRow(18, ts2, new Font(zt, 10), System.Drawing.Brushes.Green, 515 + tipMsgRow * 20);
                        tempOrders = new Order(new List<PrintRow>() { srow0, srow1, srow2, srow3, srow4, srow5, srow6, srow7, srow8, srow9, srow10, srow14, srow15, srow16, srow17 });
                        pageHeight = 305 + tipMsgRow * 15;
                    }
                    else if ((TipMsg == null || TipMsg.Trim().Length == 0) && (DoctorTip != null && DoctorTip.Trim().Length > 0))
                    {
                        PrintRow srow16 = new PrintRow(17, "医生候诊提醒：", new Font(zt, 8), System.Drawing.Brushes.Green, 285, 5);
                        PrintRow srow17 = new PrintRow(18, DoctorTip, new Font(zt, 8), System.Drawing.Brushes.Green, 300, 15);
                        //PrintRow srow16 = new PrintRow(16, "2." + DoctorTip, new Font(zt, 10), System.Drawing.Brushes.Green, 495, 10);
                        //PrintRow srow17 = new PrintRow(17, ts1, new Font(zt, 10), System.Drawing.Brushes.Green, 495 + doctorTipRow * 20);
                        //PrintRow srow18 = new PrintRow(18, ts2, new Font(zt, 10), System.Drawing.Brushes.Green, 515 + doctorTipRow * 20);
                        tempOrders = new Order(new List<PrintRow>() { srow0, srow1, srow2, srow3, srow4, srow5, srow6, srow7, srow8, srow9, srow10, srow14, srow15, srow16, srow17 });
                        pageHeight = 305 + doctorTipRow * 15;
                    }
                    else
                    {
                        //PrintRow srow16 = new PrintRow(16, ts1, new Font(zt, 10), System.Drawing.Brushes.Green, 495);
                        //PrintRow srow17 = new PrintRow(17, ts2, new Font(zt, 10), System.Drawing.Brushes.Green, 515);
                        tempOrders = new Order(new List<PrintRow>() { srow0, srow1, srow2, srow3, srow4, srow5, srow6, srow7, srow8, srow9, srow10, srow14, srow15 });
                        pageHeight = 290;
                    }
                }
                FormatPrintRowPosition(tempOrders);
                //PrintOrder.Print(ConfigurationManager.AppSettings["PrinterName"], tempOrders, pageHeight);

                /*
                 *                    // 1.设置条形码规格
                    EncodingOptions encodeOption = new EncodingOptions();
                    encodeOption.Height = 50; // 必须制定高度、宽度
                    encodeOption.Width = 65;
                    // 2.生成条形码图片并保存
                    ZXing.BarcodeWriter wr = new BarcodeWriter();
                    wr.Options = encodeOption;
                    wr.Format = BarcodeFormat.CODE_128; //  条形码规格
                    Bitmap img = wr.Write(PatientID); // 生成图片
                    Bitmap tempBmp = new Bitmap(img);

                    PrintRow srow0 = new PrintRow(0, "中山市博爱医院", new Font("宋体", 18), System.Drawing.Brushes.Black, -1);
                    PrintRow srow1 = new PrintRow(1, ViceTitle, new Font("宋体", 16), System.Drawing.Brushes.Black, 40);
                    PrintRow srow2 = new PrintRow(2, Typename + "号:" + SpeakNum, new Font("宋体", 14), System.Drawing.Brushes.Black, 75);
                    PrintRow srow3 = new PrintRow(3, "姓名:" + Name, new Font("宋体", 14), System.Drawing.Brushes.Black, 105);
                    //PrintRow srow4 = new PrintRow(4, "级别:" + level, new Font("宋体", 14), System.Drawing.Brushes.Black, 135);
                    PrintRow srow4 = new PrintRow(4, "编号:", new Font("宋体", 14), System.Drawing.Brushes.Black, 135, tempBmp);
                    PrintRow srow5 = new PrintRow(5, "-------------------------------------------", new Font("宋体", 10), System.Drawing.Brushes.Black, 205);
                    PrintRow srow6 = new PrintRow(6, "请您到等候休息区等待", new Font("宋体", 16), System.Drawing.Brushes.Green, 215);
                    PrintRow srow7 = new PrintRow(7, String.Format("(目前候诊人数：{0}人)", WaitSize), new Font("宋体", 16), System.Drawing.Brushes.Green, 240);
                    PrintRow srow8 = new PrintRow(8, "注意屏幕提示(过号重排)", new Font("宋体", 12), System.Drawing.Brushes.Black, 265);
                    PrintRow srow9 = new PrintRow(9, DateTime.Now.ToString(), new Font("宋体", 14), System.Drawing.Brushes.Black, 285);
                    Order tempOrders = new Order(new List<PrintRow>() { srow0, srow1, srow2, srow3, srow4, srow5, srow6, srow7, srow8, srow9 });
                    PrintOrder.Print(ConfigurationManager.AppSettings["PrinterName"],tempOrders);
                 */

                return true;
                //}
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                message = e.Message;
                return false;
            }
        }
        /// <summary>
        /// 自动计算行距
        /// </summary>
        /// <param name="order"></param>
        private void FormatPrintRowPosition(Order order)
        {
            int rowPitch = -1;
            int sum = 0;
            if (order.PrintRows[0].IsDrawBitMap)
            {
                sum = order.PrintRows[0].Bmp.Height;
            }
            else
            {
                sum =  order.PrintRows[0].DrawFont.Height;
            }
            for (int i = 1; i < order.PrintRows.Count; i++)
            {
                if (order.PrintRows[i].IsDrawBitMap)
                {

                    order.PrintRows[i].DrawHeight = sum + rowPitch;// +order.PrintRows[i - 1].Bmp.Height + rowPitch;
                    sum +=order.PrintRows[i].Bmp.Height;
                }
                else
                {
                    order.PrintRows[i].DrawHeight = sum + rowPitch;// +order.PrintRows[i - 1].DrawFont.Height + rowPitch;
                    if (order.PrintRows[i].DrawFont.Height >= 20 && GetLength(order.PrintRows[i].Context) > 18 && !order.PrintRows[i].Context.Contains("------"))
                    {
                        int rowCount = (int)Math.Ceiling((decimal)GetLength(order.PrintRows[i].Context) / 18);
                        sum +=order.PrintRows[i].DrawFont.Height * rowCount;

                    }
                    else if (order.PrintRows[i].DrawFont.Height >= 15 && GetLength(order.PrintRows[i].Context) > 32 && !order.PrintRows[i].Context.Contains("------"))
                    {
                        int rowCount = (int)Math.Ceiling((decimal)GetLength(order.PrintRows[i].Context) / 26);
                        sum +=  order.PrintRows[i].DrawFont.Height * rowCount;
                    }
                    else
                    {
                        sum += order.PrintRows[i].DrawFont.Height;
                    }
                }
            }
            PrintOrder.Print(ConfigurationManager.AppSettings["PrinterName"], order, sum);
        }
        /// <summary>   
        /// 得到字符串的长度，一个汉字算2个字符   
        /// </summary>   
        /// <param name="str">字符串</param>   
        /// <returns>返回字符串长度</returns>   
        public static int GetLength(string str)
        {
            if (str.Length == 0) return 0;

            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }

            return tempLen;
        }   
    }
    public class PrintNoteEntity
    {
        public string HospitalName { get; set; }
        public string DeptName { get; set; }
        public string ClinicName { get; set; }

        public string MzType{ get; set; }
        public string DoctorPrice{ get; set; }
        public string DoctorName{ get; set; }
        public string PatientId{ get; set; }

        public string QueueNum { get; set; }
        public string Name { get; set; }
        public string WaitingNum { get; set; }
        public string Time { get; set; }
        public string TipMsg { get; set; }
        public string DoctorTip { get; set; }
        public string RegVisitTime { get; set; }
    }
}
