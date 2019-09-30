using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace Xr.RtManager.Utils
{
    public static class PrintOrder
    {
        private static PrintDocument printDocument;

        private static Order printOrder;

        private static object lockObj;

   
        /// <summary>
        /// 设置打印纸高 
        /// </summary>
        public static int pageHeight = 350;

        /// <summary>
        /// 设置打印纸宽
        /// </summary>
        public static int pageWidth = 200;//300

        public static StringFormat sf;
       
        static PrintOrder()
        {
            sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            lockObj = new object();
            printDocument = new System.Drawing.Printing.PrintDocument();
            printDocument.BeginPrint += PrintDocument_BeginPrint;
            printDocument.EndPrint += PrintDocument_EndPrint;
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private static void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                Draw(e.Graphics, printOrder);
            }
            catch 
            {

            }
        }

        private static void PrintDocument_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private static void PrintDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }


        /// <summary>
        /// 绘制打印的过程
        /// </summary>
        /// <param name="g">打印机g</param>
        /// <param name="order">订单</param>
        private static void Draw(Graphics g, Order order)
        {
            List<PrintRow> tempList = order.PrintRows.OrderBy(p => p.PrintIndex).ToList();
            for (int i = 0; i < tempList.Count; i++)
            {
                Rectangle drawRect = new Rectangle(0, tempList[i].DrawHeight, pageWidth, tempList[i].DrawFont.Height);
                if (tempList[i].IsDrawBitMap)
                {
                    SizeF tempSf = g.MeasureString(tempList[i].Context, tempList[i].DrawFont);
                    int startX = (pageWidth - (int)tempSf.Width - tempList[i].Bmp.Width) / 2;

                    g.DrawString(tempList[i].Context, tempList[i].DrawFont, tempList[i].DrawBrush, startX, tempList[i].DrawHeight);// tempList[i].DrawFont,  tempList[i].DrawBrush,startX,tempList[i].DrawHeight
                    //这里绘制图片 这个需要根据图片的大小进行细微的调整 主要是高度
                    g.DrawImage(tempList[i].Bmp, tempSf.Width + startX, tempList[i].DrawHeight);
                    float a = tempSf.Width + startX;
                }
                else
                {
                    if (tempList[i].Alignment != null)
                        g.DrawString(tempList[i].Context, tempList[i].DrawFont, tempList[i].DrawBrush, drawRect, tempList[i].Alignment);//tempList[i].Context, tempList[i].DrawFont, tempList[i].DrawBrush, drawRect, sf
                    else
                    {
                        if (tempList[i].MarginLeft!=0)
                        {
                            drawRect = new Rectangle(tempList[i].MarginLeft, tempList[i].DrawHeight, pageWidth - tempList[i].MarginLeft, tempList[i].DrawFont.Height * 10);
                            sf.Alignment = StringAlignment.Near;
                            sf.LineAlignment = StringAlignment.Near;
                            g.DrawString(tempList[i].Context, tempList[i].DrawFont, tempList[i].DrawBrush, drawRect, sf);//tempList[i].Context, tempList[i].DrawFont, tempList[i].DrawBrush, drawRect, sf
                        }
                        else
                        {
                            sf.Alignment = StringAlignment.Center;
                            g.DrawString(tempList[i].Context, tempList[i].DrawFont, tempList[i].DrawBrush, drawRect, sf);//tempList[i].Context, tempList[i].DrawFont, tempList[i].DrawBrush, drawRect, sf
                        }
                    }
                }
            }
        }

        public static Bitmap GetBmp(Order order)
        {
            Bitmap bmp = new Bitmap(pageWidth, pageHeight);
            Graphics g = Graphics.FromImage(bmp);
            Draw(g, order);
            g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
            return bmp;
        }

        public static void Print(string printName, Order order, int height)
        {
            lock (lockObj)
            {
                try
                {
                    printOrder = order;
                    printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custom", pageWidth, height);//指定纸张大小
                    printDocument.PrinterSettings.PrinterName = printName;
                    printDocument.Print();

                    //printDocument.DocumentName = order.HeardInfo.OrderNum;
                }
                catch
                {

                }
            }
        }


        public static void Print(Order order)
        {
            lock (lockObj)
            {
                try
                {
                    printOrder = order;
                    printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custom", pageWidth, pageHeight);
                    printDocument.Print();
                }
                catch 
                {
                    // MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 将CM转换成打印像素
        /// </summary>
        /// <param name="cmValue"></param>
        /// <returns></returns>
        public static int ConvertCmToPrintPixel(float cmValue)
        {
            return PrinterUnitConvert.Convert((int)(cmValue * 100), PrinterUnit.TenthsOfAMillimeter, PrinterUnit.Display);
        }
    }
}
