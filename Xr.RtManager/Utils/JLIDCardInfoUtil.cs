using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Xr.RtManager
{
    class JLIDCardInfoUtil
    {

        #region 精伦身份证读卡器
        [DllImport("Sdtapi.dll")]
        public static extern int HID_BeepLED(bool BeepON, bool LEDON, int duration);
        /// <summary>
        /// 端口初始化函数
        /// </summary>
        /// <param name="iPort">串口号<COM1...>、USB<1001></param>
        /// <returns></returns>
        [DllImport("Sdtapi.dll")]
        public static extern int InitComm(int iPort);
        /// <summary>
        /// 卡认证接口
        /// </summary>
        /// <returns></returns>
        [DllImport("Sdtapi.dll")]
        public static extern int Authenticate();
        /// <summary>
        /// 证件类型
        /// </summary>
        /// <returns></returns>
        [DllImport("Sdtapi.dll")]
        public static extern int Routon_DecideIDCardType();
        /// <summary>
        /// 读身份证信息
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Gender"></param>
        /// <param name="Folk"></param>
        /// <param name="BirthDay"></param>
        /// <param name="Code"></param>
        /// <param name="Address"></param>
        /// <param name="Agency"></param>
        /// <param name="ExpireStart"></param>
        /// <param name="ExpireEnd"></param>
        /// <returns></returns>
        [DllImport("Sdtapi.dll")]
        public static extern int ReadBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                    StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);
        /// <summary>
        /// 读取外国居留证
        /// </summary>
        /// <param name="EnName"></param>
        /// <param name="Gender"></param>
        /// <param name="Code"></param>
        /// <param name="Nation"></param>
        /// <param name="CnName"></param>
        /// <param name="BirthDay"></param>
        /// <param name="ExpireStart"></param>
        /// <param name="ExpireEnd"></param>
        /// <param name="CardVertion"></param>
        /// <param name="Agency"></param>
        /// <param name="CardType"></param>
        /// <param name="FutureItem"></param>
        /// <returns></returns>
       [DllImport("Sdtapi.dll")]
        public static extern int Routon_ReadAllForeignBaseInfos(StringBuilder EnName, StringBuilder Gender, StringBuilder Code, StringBuilder Nation,
            StringBuilder CnName, StringBuilder BirthDay, StringBuilder ExpireStart, StringBuilder ExpireEnd, StringBuilder CardVertion,
            StringBuilder Agency, StringBuilder CardType, StringBuilder FutureItem);

        /// <summary>
        /// 读港澳台居住证
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Gender"></param>
        /// <param name="FutureItem1"></param>
        /// <param name="BirthDay"></param>
        /// <param name="Address"></param>
        /// <param name="Code"></param>
        /// <param name="Agency"></param>
        /// <param name="ExpireStart"></param>
        /// <param name="ExpireEnd"></param>
        /// <param name="PassID"></param>
        /// <param name="SignCnt"></param>
        /// <param name="FutureItem2"></param>
        /// <param name="CardType"></param>
        /// <param name="FutureItem3"></param>
        [DllImport("Sdtapi.dll")]
        public static extern int Routon_ReadAllGATBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder FutureItem1, StringBuilder BirthDay,
            StringBuilder Address, StringBuilder Code, StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd, StringBuilder PassID,
            StringBuilder SignCnt, StringBuilder FutureItem2, StringBuilder CardType, StringBuilder FutureItem3);
        /// <summary>
        /// 端口关闭接口
        /// </summary>
        /// <returns></returns>
        [DllImport("Sdtapi.dll")]
        public static extern int CloseComm();

        #endregion


        

    }


}
