using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common
{
    /// <summary>
    /// 表示在Zebone公用类库中触发的异常
    /// </summary>
    [Serializable]
    public class ZeboneException : Exception
    {
        public ZeboneException() { }
        public ZeboneException(string message) : base(message) { }
        public ZeboneException(string message, Exception inner) : base(message, inner) { }
        protected ZeboneException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
