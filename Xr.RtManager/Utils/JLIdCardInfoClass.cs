using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Xr.RtManager
{
    class JLIdCardInfoClass
    {

        public StringBuilder enName;          // 英文姓名	121	字母
        public StringBuilder cnName;           // 姓名	31	汉字
        public StringBuilder Gender;            // 性别	3	汉字
        public StringBuilder Folk;             // 民族/国籍	40	汉字/字母     
        public StringBuilder Code;             // 公民身份号码 19	数字/数字+字母       
        public StringBuilder BirthDay;         // 出生日期	9	CCYYMMDD
        public StringBuilder Address;         // 住址	71	汉字和数字
        public StringBuilder Agency;           // 签发机关	31	汉字
        public StringBuilder ExpireStart;      // 有效期起始日期	9	CCYYMMDD
        public StringBuilder ExpireEnd;        // 有效期截止日期	9	CCYYMMDD 有效期为长期的表示为汉字“长期”

        public StringBuilder PassID;           // 通行证号
        public StringBuilder CardType;          // 证件类型
        public StringBuilder SignCnt;           // 签发次数
        public StringBuilder FutureItem1;      // 指向港澳台居住证第一块预留区信息
        public StringBuilder FutureItem2;      // 指向港澳台居住证第二块预留区信息
        public StringBuilder FutureItem3;      // 指向港澳台居住证第三块预留区信息


        public JLIdCardInfoClass()
        {
            enName = new StringBuilder(121);          // 英文姓名	121	字母
            cnName = new StringBuilder(31);           // 姓名	31	汉字
            Gender = new StringBuilder(3);            // 性别	3	汉字
            Folk = new StringBuilder(40);             // 民族/国籍	40	汉字/字母     
            Code = new StringBuilder(20);             // 公民身份号码 19	数字/数字+字母       
            BirthDay = new StringBuilder(10);         // 出生日期	9	CCYYMMDD
            Address = new StringBuilder(255);         // 住址	71	汉字和数字
            Agency = new StringBuilder(31);           // 签发机关	31	汉字
            ExpireStart = new StringBuilder(10);      // 有效期起始日期	9	CCYYMMDD
            ExpireEnd = new StringBuilder(10);        // 有效期截止日期	9	CCYYMMDD 有效期为长期的表示为汉字“长期”

            PassID = new StringBuilder(19);           // 通行证号
            CardType = new StringBuilder(2);          // 证件类型
            SignCnt = new StringBuilder(5);           // 签发次数
            FutureItem1 = new StringBuilder(10);      // 指向港澳台居住证第一块预留区信息
            FutureItem2 = new StringBuilder(10);      // 指向港澳台居住证第二块预留区信息
            FutureItem3 = new StringBuilder(10);      // 指向港澳台居住证第三块预留区信息
        }
        /// <summary>
        /// 停止读取标志
        /// </summary>
        public static bool CancelFlag = false;
        /// <summary>
        /// 获取卡信息
        /// </summary>
        /// <param name="info">卡信息</param>
        /// <returns>读卡结果：0失败、1成功</returns>
        public static JLIdCardInfoClass getCardInfo()
        {
            CancelFlag = false;
            int result = 0;
            JLIdCardInfoClass info = null;
            int ret = JLIDCardInfoUtil.InitComm(1001);//USB接口,  1-16则为串口号
            if (ret == 1)
            {
                while (result == 0 )
                {
                    if (!CancelFlag)
                    {
                        ret = JLIDCardInfoUtil.Authenticate();
                        if (ret == 1)
                        {
                            ret = JLIDCardInfoUtil.Routon_DecideIDCardType();
                            if (ret == 100)
                            {//身份证卡
                                info = new JLIdCardInfoClass();
                                //读取身份证信息
                                ret = JLIDCardInfoUtil.ReadBaseInfos(info.cnName, info.Gender, info.Folk, info.BirthDay, info.Code, info.Address, info.Agency,
                                                    info.ExpireStart, info.ExpireEnd);
                                if (ret == 1)
                                {
                                    info.CardType = new StringBuilder("01");
                                    result = 1;
                                    JLIDCardInfoUtil.HID_BeepLED(true, false, 0);//发出滴声响，说明读取成功
                                }
                            }
                            else if (ret == 101)
                            {//外国人居留证
                                info = new JLIdCardInfoClass();
                                //读取外国居留证
                                ret = JLIDCardInfoUtil.Routon_ReadAllForeignBaseInfos(info.enName, info.Gender, info.Code, info.Folk, info.cnName, info.BirthDay,
                                                               info.ExpireStart, info.ExpireEnd, info.FutureItem1, info.Agency, info.CardType, info.FutureItem2);
                                if (ret == 1)
                                {
                                    info.CardType = new StringBuilder("07");
                                    result = 1;
                                    JLIDCardInfoUtil.HID_BeepLED(true, false, 0);//发出滴声响，说明读取成功
                                }
                            }
                            else if (ret == 102)
                            {//港澳台居住证
                                info = new JLIdCardInfoClass();
                                //读港澳台居住证
                                ret = JLIDCardInfoUtil.Routon_ReadAllGATBaseInfos(info.cnName, info.Gender, info.FutureItem1, info.BirthDay, info.Address, info.Code,
                                    info.Agency, info.ExpireStart, info.ExpireEnd, info.Code, info.SignCnt, info.FutureItem2, info.CardType, info.FutureItem3);
                                if (ret == 1)
                                {
                                    info.CardType = new StringBuilder("06");
                                    result = 1;
                                    JLIDCardInfoUtil.HID_BeepLED(true, false, 0);//发出滴声响，说明读取成功
                                }
                            }
                        }
                    }
                    else
                    {
                        result = 1;
                    }
                }
            }
            JLIDCardInfoUtil.CloseComm();
            return info;

        }
    
    }
}
