using System;
using System.Collections.Generic;
using System.Linq;

namespace Xr.RtScreen.Models
{
    /// <summary>
    /// 分页工具类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingUtil<T> : List<T>
    {
        public int DataCount { get; set; }
        public int PageCount { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage
        {
            get
            {
                return PageNo > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageNo < PageCount;
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        public PagingUtil(List<T> dataList, int pageSize, int pageNo)
        {
            PageSize = pageSize;
            PageNo = pageNo;
            DataCount = dataList.Count;
            PageCount = (int)Math.Ceiling((decimal)DataCount / pageSize);
            AddRange(dataList.Skip((pageNo - 1) * pageSize).Take(pageSize));
        }
    }
}
