using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Xr.Common.Common
{
    /// <summary>
    /// 图形绘制辅助单元
    /// </summary>
    public static class DrawingUtils
    {
        /// <summary>
        /// 创建圆角矩形
        /// </summary>
        /// <param name="rect">矩形尺寸</param>
        /// <param name="radius">圆角半径</param>
        /// <returns></returns>
        public static GraphicsPath GetGraphicsPath(Rectangle rect, int radius)
        {
            return GetGraphicsPath(rect, radius, radius, radius, radius);
        }

        /// <summary>
        /// 创建圆角矩形
        /// </summary>
        /// <param name="rect">矩形尺寸</param>
        /// <param name="topLeftRadius">左上角圆角半径</param>
        /// <param name="topRightRadius">右上角圆角半径</param>
        /// <param name="bottomRightRadius">右下角圆角半径</param>
        /// <param name="bottomLeftRadius">左下角圆角半径</param>
        /// <returns></returns>
        public static GraphicsPath GetGraphicsPath(Rectangle rect, int topLeftRadius, int topRightRadius, int bottomRightRadius, int bottomLeftRadius)
        {
            var path = new GraphicsPath();

            //左上角圆角
            if (topLeftRadius > 0)
            {
                path.AddArc(rect.Left, rect.Top, topLeftRadius * 2, topLeftRadius * 2, 180, 90);
            }

            //上方直线
            path.AddLine(rect.Left + topLeftRadius, rect.Top, rect.Right - topRightRadius, rect.Top);

            //右上角圆角
            if (topRightRadius > 0)
            {
                path.AddArc(rect.Right - 2 * topRightRadius, rect.Top, topRightRadius * 2, topRightRadius * 2, 270, 90);
            }

            //右侧直线
            path.AddLine(rect.Right, rect.Top + topRightRadius, rect.Right, rect.Bottom - bottomRightRadius);

            //右下角圆角
            if (bottomRightRadius > 0)
            {
                path.AddArc(rect.Right - 2 * bottomRightRadius, rect.Bottom - 2 * bottomRightRadius, bottomRightRadius * 2, bottomRightRadius * 2, 0, 90);
            }

            //下方直线
            path.AddLine(rect.Right - bottomRightRadius, rect.Bottom, rect.Left + bottomLeftRadius, rect.Bottom);

            //左下圆角
            if (bottomLeftRadius > 0)
            {
                path.AddArc(rect.Left, rect.Bottom - bottomLeftRadius * 2, bottomLeftRadius * 2, bottomLeftRadius * 2, 90, 90);
            }

            path.CloseFigure();
            return path;
        }
    }
}
