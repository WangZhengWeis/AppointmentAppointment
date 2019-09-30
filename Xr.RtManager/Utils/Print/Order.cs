using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtManager.Utils
{
    public class Order
    {
        private List<PrintRow> printRows;

        public List<PrintRow> PrintRows
        {
            get { return printRows; }
            set { printRows = value; }
        }

        public Order(List<PrintRow> rows)
        {
            this.printRows = rows;
        }
    }
}
