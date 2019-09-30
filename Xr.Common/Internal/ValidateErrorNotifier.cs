using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

using Xr.Common.Properties;

namespace Xr.Common.Internal
{
    internal class ValidateErrorNotifier
    {
        private ToolTipController toolTipController = null;
        private ImageList imageList = null;

        public virtual void ClearError()
        {
            this.ToolTipController.HideHint();
        }

        public virtual void ShowError(System.Windows.Forms.Control control, string message)
        {
            var args = this.ToolTipController.CreateShowArgs();
            args.ToolTip = message;
            this.toolTipController.AutoPopDelay = 3000; //设置显示时间

            //右边显示位置不够时，移到上方显示            
            var parent = control.Parent ?? control;
            var p = parent.PointToScreen(new Point(control.Right, control.Top));
            if (p.X > Screen.PrimaryScreen.WorkingArea.Width || Screen.PrimaryScreen.WorkingArea.Width - p.X < 100)
            {
                args.ToolTipLocation = ToolTipLocation.TopCenter;
            }

            this.ToolTipController.HideHint();
            this.ToolTipController.ShowHint(args, control);
        }

        public void ShowError(GridView gridView, GridColumn column, string message)
        {
            ShowError(gridView, gridView.FocusedRowHandle, column, message);
        }

        public virtual void ShowError(GridView gridView, int rowHandle, GridColumn column, string message)
        {
            try
            {
                gridView.FocusedColumn = column;
                gridView.ShowEditor();

                if (gridView.GridControl.CanFocus) gridView.GridControl.Focus();

                Application.DoEvents();

                var args = this.ToolTipController.CreateShowArgs();
                args.ToolTip = message;
                args.ToolTipLocation = ToolTipLocation.TopLeft;
                this.toolTipController.AutoPopDelay = 3000; //设置显示时间

                this.ToolTipController.HideHint();
                //this.ToolTipController.ShowHint(args, gridView.ActiveEditor);

                //获取错误单元格位置
                var gridViewInfo = gridView.GetGridViewInfo();
                var cellInfo = gridViewInfo.GetGridCellInfo(rowHandle, column);

                //计算提示框位置
                var pt = new Point(
                    cellInfo.Bounds.Location.X + 10,
                    cellInfo.Bounds.Location.Y);

                this.ToolTipController.ShowHint(args, gridView.GridControl.PointToScreen(pt));
            }
            catch
            {
                gridView.SetColumnError(column, message);
            }
        }

        private ToolTipController ToolTipController
        {
            get
            {
                EnsureToolTipController();
                return toolTipController;
            }
        }

        private void EnsureToolTipController()
        {
            if (toolTipController != null) return;

            imageList = new ImageList();
            imageList.ImageSize = new Size(24, 24);
            imageList.Images.Add("Icon", Resources.ValidateErrorIcon);

            toolTipController = new ToolTipController();
            toolTipController.CloseOnClick = DefaultBoolean.True;
            toolTipController.ShowBeak = true;
            toolTipController.ShowShadow = false;
            toolTipController.ToolTipLocation = ToolTipLocation.RightCenter;
            toolTipController.ToolTipStyle = ToolTipStyle.WindowsXP;
            toolTipController.ImageList = imageList;
            toolTipController.ImageIndex = 0;

            toolTipController.Appearance.BackColor = Color.FromArgb(251, 250, 227);
            toolTipController.Appearance.BackColor2 = Color.FromArgb(251, 250, 227);
            toolTipController.Appearance.BorderColor = Color.FromArgb(211, 207, 58);
            toolTipController.Appearance.ForeColor = Color.FromArgb(51, 51, 51);
            toolTipController.Appearance.Font = new Font("微软雅黑", 14, FontStyle.Regular, GraphicsUnit.Pixel);
        }
    }
}
