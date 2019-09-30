using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using System.Collections;
using System.IO;

namespace Xr.Common
{
    /// <summary>
    /// Grid控件扩展类
    /// </summary>
    public static class XtraGridExtensions
    {
        private static PropertyInfo viewInfoProperty = null;

        /// <summary>
        /// 对于GridControl中的数据源循环执行指定的操作
        /// </summary>
        /// <typeparam name="T">Grid数据源绑定的类型</typeparam>
        /// <param name="gridControl">GridConrol控件</param>
        /// <param name="action">指定需要执行的操作</param>
        public static void ForEach<T>(this GridControl gridControl, Action<T> action)
        {
            if (action == null) return;

            var items = gridControl.DataSource as IList<T>;
            if (items == null) return;

            foreach (var item in items)
            {
                action(item);
            }
        }

        /// <summary>
        /// 获取 GridView 控件内部的 ViewInfo 属性，通过该属性可以获取 GridView 中单元格的位置等信息
        /// </summary>
        /// <param name="gridView"></param>
        /// <returns></returns>
        public static GridViewInfo GetGridViewInfo(this GridView gridView)
        {
            //ViewInfo是一个Protected属性，使用反射获取其值
            if (viewInfoProperty == null)
            {
                viewInfoProperty = typeof(GridView).GetProperty("ViewInfo", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            }

            return viewInfoProperty.GetValue(gridView, null) as GridViewInfo;
        }

        /// <summary>
        /// 将GridView中的指定列设置为序号列
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="column"></param>
        public static void SetAsIndexColumn(this GridView gridView, GridColumn column)
        {
            if (gridView == null || column == null) return;

            gridView.CustomDrawCell += (sender, e) =>
            {
                if (e.Column == column)
                {
                    var text = (e.RowHandle + 1).ToString();
                    var textSize = e.Graphics.MeasureString(text, gridView.Appearance.Row.Font);

                    e.Graphics.DrawString(text, gridView.Appearance.Row.Font, Brushes.Black,
                        e.Bounds.Left + (e.Bounds.Width - textSize.Width) / 2,
                        e.Bounds.Top + (e.Bounds.Height - textSize.Height) / 2);

                    e.Handled = true;
                }
            };
        }

        /// <summary>
        /// 显示指定列上的编辑框，使指定的列进入编辑状态
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="column"></param>
        public static void ShowEditor(this GridView gridView, GridColumn column)
        {
            if (!column.Visible) return;

            gridView.FocusedColumn = column;
            if (gridView.CanShowEditor)
            {
                gridView.ShowEditor();
                if (gridView.ActiveEditor != null) gridView.ActiveEditor.SelectAll();
            }
        }

        /// <summary>
        /// 将GridView焦点移到第一列
        /// </summary>
        public static void MoveToFirstColumn(this GridView gridView)
        {
            foreach (GridColumn col in gridView.Columns)
            {
                if (col.Visible)
                {
                    gridView.ShowEditor(col);
                    break;
                }
            }
        }

        /// <summary>
        /// 将GridView焦点移到第一个可编辑列
        /// </summary>
        /// <param name="gridView"></param>
        public static void MoveToFirstEditableColumn(this GridView gridView)
        {
            foreach (GridColumn col in gridView.Columns)
            {
                if (col.Visible && col.OptionsColumn.AllowEdit)
                {
                    gridView.ShowEditor(col);
                    break;
                }
            }
        }

        /// <summary>
        /// 将GridView焦点移到最后一列
        /// </summary>
        public static void MoveLastColumn(this GridView gridView)
        {
            for (var i = gridView.Columns.Count - 1; i >= 0; i--)
            {
                var col = gridView.Columns[i];
                if (col.Visible)
                {
                    gridView.ShowEditor(col);
                    break;
                }
            }
        }

        /// <summary>
        /// 将GridView焦点移到最后一个可编辑列
        /// </summary>
        /// <param name="gridView"></param>
        public static void MoveToLastEditableColumn(this GridView gridView)
        {
            for (var i = gridView.Columns.Count - 1; i >= 0; i--)
            {
                var col = gridView.Columns[i];
                if (col.Visible && col.OptionsColumn.AllowEdit)
                {
                    gridView.ShowEditor(col);
                    break;
                }
            }
        }

        /// <summary>
        /// 将GridView的焦点移到上一列
        /// </summary>
        /// <param name="gridView"></param>
        public static void MoveToPreviousColumn(this GridView gridView)
        {
            var col = gridView.FocusedColumn;
            if (col == null) return;

            for (var i = gridView.Columns.Count - 1; i >= 0; i--)
            {
                var c = gridView.Columns[i];
                if (c.Visible && c.VisibleIndex < col.VisibleIndex)
                {
                    gridView.ShowEditor(c);
                    break;
                }
            }
        }

        /// <summary>
        /// 将GridView的焦点移到上一个可编辑列
        /// </summary>
        /// <param name="gridView"></param>
        public static void MoveToPreviousEditableColumn(this GridView gridView)
        {
            var col = gridView.FocusedColumn;
            if (col == null) return;

            for (var i = gridView.Columns.Count - 1; i >= 0; i--)
            {
                var c = gridView.Columns[i];
                if (c.Visible && c.OptionsColumn.AllowEdit && c.VisibleIndex < col.VisibleIndex)
                {
                    gridView.ShowEditor(c);
                    break;
                }
            }
        }

        /// <summary>
        /// 将GridView的焦点移到下一列
        /// </summary>
        /// <param name="gridView"></param>
        public static void MoveToNextColumn(this GridView gridView)
        {
            var col = gridView.FocusedColumn;
            if (col == null) return;

            foreach (GridColumn c in gridView.Columns)
            {
                if (c.Visible && c.VisibleIndex > col.VisibleIndex)
                {
                    gridView.ShowEditor(c);
                    break;
                }
            }
        }

        /// <summary>
        /// 将GridView的焦点移到下一个可编辑列
        /// </summary>
        /// <param name="gridView"></param>
        public static void MoveToNextEditableColumn(this GridView gridView)
        {
            var col = gridView.FocusedColumn;
            if (col == null) return;

            foreach (GridColumn c in gridView.Columns)
            {
                if (c.Visible && c.OptionsColumn.AllowEdit && c.VisibleIndex > col.VisibleIndex)
                {
                    gridView.ShowEditor(c);
                    break;
                }
            }
        }

        /// <summary>
        /// GridView当前的焦点是否为最后一个可编辑列
        /// </summary>
        /// <param name="gridView"></param>
        /// <returns></returns>
        public static bool IsLastEditableColumn(this GridView gridView)
        {
            return IsLastEditableColumn(gridView, gridView.FocusedColumn);
        }

        /// <summary>
        /// 指定的列是否为GridView的最后一个可编辑列
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IsLastEditableColumn(this GridView gridView, GridColumn column)
        {
            if (column == null) throw new ArgumentNullException("column");

            //查找最后一个可编辑列序号
            var maxEditableIndex = 0;
            foreach (GridColumn c in gridView.Columns)
            {
                if (c.Visible && c.OptionsColumn.AllowEdit && maxEditableIndex < c.VisibleIndex)
                {
                    maxEditableIndex = c.VisibleIndex;
                }
            }

            return column.VisibleIndex >= maxEditableIndex;
        }

        /// <summary>
        /// GridView当前的焦点是否为最后一个可见列
        /// </summary>
        /// <param name="gridView"></param>
        public static bool IsLastVisibleColumn(this GridView gridView)
        {
            return IsLastVisibleColumn(gridView, gridView.FocusedColumn);
        }

        /// <summary>
        /// 指定的列是否为GridView的最后一个可见列
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="col"></param>
        public static bool IsLastVisibleColumn(this GridView gridView, GridColumn column)
        {
            if (column == null) throw new ArgumentNullException("column");

            //查找最后一个可见列的序号
            var maxVisibleIndex = 0;
            foreach (GridColumn col in gridView.Columns)
            {
                if (col.Visible && maxVisibleIndex < col.VisibleIndex)
                {
                    maxVisibleIndex = col.VisibleIndex;
                }
            }

            return column.VisibleIndex == maxVisibleIndex;
        }

        /// <summary>
        /// 添加 带默认值的行
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="gridView">表格</param>
        /// <param name="predicate">认为空行的标准</param>
        /// <param name="emptyRow">带默认值的类</param>
        public static void AddEmptyRow<T>(this GridView gridView, Func<T, bool> predicate, T emptyRow) where T: new()
        {
            var gridSource = gridView.DataSource as IList<T>;
            if (gridSource == null)
            {
                gridSource = new List<T>();
            }
            var emptycount = gridSource.Where(predicate).Count();
            if (emptycount < 1)
            {
                gridSource.Add(emptyRow);
                gridView.GridControl.DataSource = gridSource;
                gridView.RefreshData();
                gridView.MoveToFirstEditableColumn();
                gridView.FocusedRowHandle = gridView.RowCount - 1;
                gridView.ShowEditor();
            }
        }

        /// <summary>
        /// 添加 无默认值的行
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="gridView">表格</param>
        /// <param name="predicate">认为空行的标准</param>
        public static void AddEmptyRow<T>(this GridView gridView, Func<T, bool> predicate) where T : new()
        {
            gridView.AddEmptyRow<T>(predicate, new T());
        }

        /// <summary>
        /// 为GridView内增加行删除按钮
        /// </summary>
        /// <param name="gv">GridView</param>
        public static GridView AddRowDelButton(this GridView gv)
        {
            if (gv == null)
                return gv;
            RepositoryItemButtonEdit btn = new RepositoryItemButtonEdit { TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor };
            btn.Buttons.Clear();
            //下一行的AppContext.Directories.Images这个被改过
            //btn.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, new Bitmap(Path.Combine(AppContext.Directories.Images, "RowDelete-normal.png")), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), new DevExpress.Utils.SerializableAppearanceObject(), "", null, null, true));
            btn.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, new Bitmap(Path.Combine("", "RowDelete-normal.png")), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), new DevExpress.Utils.SerializableAppearanceObject(), "", null, null, true));
            gv.GridControl.RepositoryItems.Add(btn);
            GridColumn gc = new DevExpress.XtraGrid.Columns.GridColumn() { Caption = string.Empty, ColumnEdit = btn, Visible = true, Width = 20 };
            gc.OptionsColumn.AllowMove = false;
            gc.OptionsColumn.AllowSize = false;
            gc.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            gv.Columns.Add(gc);
            btn.ButtonClick += (sender, e) =>
            {
                (gv.DataSource as IList).Remove(gv.GetFocusedRow());
                gv.RefreshData();
            };
            return gv;
        }        
    }
}
