using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;


namespace Xr.Common.Controls
{
    [ProvideProperty("Visible", typeof(Component))]
    public partial class RequiredMarker : Component, IExtenderProvider
    {
        private const string RequiredMarkerText = "*";
        private static readonly Font RequiredMarkerFont = new Font("微软雅黑", 18, FontStyle.Regular, GraphicsUnit.Pixel);
        private static readonly Color RequriedMarkerForeColor = Color.FromArgb(255, 0, 0);

        private bool requiredMarkerVisible = false;
        private Dictionary<Control, LabelControl> requiredMarkers = new Dictionary<Control, LabelControl>();  //用于保存控件和必填项控件的对应关系        

        private List<Control> requiredControls = new List<Control>();           //需要显示必填项标记的控件
        private List<GridColumn> requiredGridColumns = new List<GridColumn>();  //需要显示必填项标记的Grid列

        private List<GridView> includeGridViews = new List<GridView>();   //必填项Grid列对应的GridView控件

        public RequiredMarker()
        {
            InitializeComponent();
        }

        public RequiredMarker(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// 显示必填项标记
        /// </summary>
        public void ShowMarker()
        {
            requiredMarkerVisible = true;

            //在控件前显示必填项标记
            foreach (var control in requiredControls)
            {
                ShowMarker(control);
            }

            foreach (var gridView in includeGridViews)
            {
                gridView.CustomDrawColumnHeader -= new ColumnHeaderCustomDrawEventHandler(GridView_CustomDrawColumnHeader);
                gridView.CustomDrawColumnHeader += new ColumnHeaderCustomDrawEventHandler(GridView_CustomDrawColumnHeader);
            }
        }

        /// <summary>
        /// 在指定的控件前显示必填项标记
        /// </summary>
        /// <param name="control"></param>
        public void ShowMarker(Control control)
        {
            LabelControl requriedMarker = null;
            if (requiredMarkers.ContainsKey(control))
            {
                requriedMarker = requiredMarkers[control];
                requriedMarker.Visible = true;
            }
            else
            {
                requriedMarker = new LabelControl();
                requriedMarker.Parent = control.Parent;
                requriedMarker.Text = RequiredMarkerText;
                requriedMarker.Font = RequiredMarkerFont;
                requriedMarker.ForeColor = RequriedMarkerForeColor;
                requriedMarker.Location = new Point(control.Left - requriedMarker.Width, control.Top + (control.Height - requriedMarker.Height) / 2);

                //避免控件dock属性为fill时，遮挡标记的显示
                if (control.Dock == DockStyle.Fill)
                {
                    requriedMarker.Dock = DockStyle.Left;
                    requriedMarker.SendToBack();
                }

                requiredMarkers.Add(control, requriedMarker);
            }
        }

        public void HideMarker()
        {
            requiredMarkerVisible = false;

            foreach (var control in requiredControls)
            {
                HideMarker(control);
            }

            foreach (var gridView in includeGridViews)
            {
                gridView.Invalidate();
            }
        }

        public void HideMarker(Control control)
        {
            if (requiredMarkers.ContainsKey(control))
            {
                requiredMarkers[control].Visible = false;
            }
        }

        public bool CanExtend(object extendee)
        {
            return extendee is Control || extendee is GridColumn;
        }

        [DefaultValue(false)]
        public bool GetVisible(Component component)
        {
            if (component is Control)
            {
                return GetVisible(component as Control);
            }

            if (component is GridColumn)
            {
                return GetVisible(component as GridColumn);
            }

            return false;
        }

        public void SetVisible(Component component, bool value)
        {
            if (component is Control)
            {
                SetVisible(component as Control, value);
                return;
            }

            if (component is GridColumn)
            {
                SetVisible(component as GridColumn, value);
                return;
            }
        }

        private bool GetVisible(Control control)
        {
            return requiredControls.Contains(control);
        }

        private bool GetVisible(GridColumn column)
        {
            return requiredGridColumns.Contains(column);
        }

        private void SetVisible(Control control, bool value)
        {
            if (value)
            {
                if (!requiredControls.Contains(control))
                {
                    requiredControls.Add(control);
                }
            }
            else
            {
                requiredControls.Remove(control);
            }
        }

        private void SetVisible(GridColumn column, bool value)
        {
            if (value)
            {
                if (!requiredGridColumns.Contains(column))
                {
                    requiredGridColumns.Add(column);

                    //将Grid列对应的Grid添加到列表中备用
                    var gridView = column.View as GridView;
                    if (gridView != null && !includeGridViews.Contains(gridView))
                    {
                        includeGridViews.Add(gridView);
                    }
                }
            }
            else
            {
                requiredGridColumns.Remove(column);
            }
        }

        private void GridView_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (!requiredMarkerVisible) return;

            if (requiredGridColumns.Contains(e.Column))
            {
                e.Painter.DrawObject(e.Info);

                var textSize = e.Info.Graphics.MeasureString(e.Info.Caption, e.Info.Appearance.Font);

                e.Graphics.DrawString(RequiredMarkerText, RequiredMarkerFont, Brushes.Red,
                    e.Info.CaptionRect.X + Math.Max((e.Info.CaptionRect.Width - textSize.Width) / 2 - 8, 0),
                   e.Info.CaptionRect.Y + (e.Info.CaptionRect.Height - textSize.Height) / 2);

                e.Handled = true;
            }
        }
    }
}
