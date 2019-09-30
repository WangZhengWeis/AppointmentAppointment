using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtScreen.Models
{
    public class AutoReSizeForm
    {
        //声明结构,只记录窗体和其控件的初始位置和大小。
        public struct controlRect
        {
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        }
        public List<controlRect> oldCtrl = new List<controlRect>();
        int ctrlNo = 0;//1;
        public void controllInitializeSize(System.Windows.Forms.Control mForm)
        {
            try
            {
                controlRect cR;
                cR.Left = mForm.Left; cR.Top = mForm.Top; cR.Width = mForm.Width; cR.Height = mForm.Height;
                oldCtrl.Add(cR);
                AddControl(mForm);
            }
            catch (Exception)
            {
            }

        }
        private void AddControl(System.Windows.Forms.Control ctl)
        {
            try
            {
                foreach (System.Windows.Forms.Control c in ctl.Controls)
                {
                    controlRect objCtrl;
                    objCtrl.Left = c.Left; objCtrl.Top = c.Top; objCtrl.Width = c.Width; objCtrl.Height = c.Height;
                    oldCtrl.Add(objCtrl);
                    if (c.Controls.Count > 0)
                        AddControl(c);
                }
            }
            catch (Exception)
            {
            }

        }
        public void controlAutoSize(System.Windows.Forms.Control mForm)
        {
            try
            {
                if (ctrlNo == 0)
                {
                    controlRect cR;
                    cR.Left = 0; cR.Top = 0; cR.Width = mForm.PreferredSize.Width; cR.Height = mForm.PreferredSize.Height;
                    oldCtrl.Add(cR);
                    AddControl(mForm);
                }
                float wScale = (float)mForm.Width / (float)oldCtrl[0].Width;
                float hScale = (float)mForm.Height / (float)oldCtrl[0].Height;
                ctrlNo = 1;
                AutoScaleControl(mForm, wScale, hScale);
            }
            catch (Exception)
            {
            }

        }
        private void AutoScaleControl(System.Windows.Forms.Control ctl, float wScale, float hScale)
        {
            try
            {
                int ctrLeft0, ctrTop0, ctrWidth0, ctrHeight0;
                foreach (System.Windows.Forms.Control c in ctl.Controls)
                {
                    ctrLeft0 = oldCtrl[ctrlNo].Left;
                    ctrTop0 = oldCtrl[ctrlNo].Top;
                    ctrWidth0 = oldCtrl[ctrlNo].Width;
                    ctrHeight0 = oldCtrl[ctrlNo].Height;
                    c.Left = (int)((ctrLeft0) * wScale);
                    c.Top = (int)((ctrTop0) * hScale);
                    c.Width = (int)(ctrWidth0 * wScale);
                    c.Height = (int)(ctrHeight0 * hScale);
                    ctrlNo++;
                    if (c.Controls.Count > 0)
                        AutoScaleControl(c, wScale, hScale);
                }
            }
            catch (Exception)
            {
            }
        }
        #region 注释无用的东西
        //static float SH
        //{
        //    get
        //    {
        //        return (float)System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / Properties.Settings.Default.Y;
        //    }
        //}
        //static float SW
        //{
        //    get
        //    {
        //        return (float)System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / Properties.Settings.Default.X;
        //    }
        //}

        //public static void SetFormSize(System.Windows.Forms.Control fm)
        //{
        //    fm.Location = new System.Drawing.Point((int)(fm.Location.X * SW), (int)(fm.Location.Y * SH));
        //    fm.Size = new System.Drawing.Size((int)(fm.Size.Width * SW), (int)(fm.Size.Height * SH));
        //    fm.Font = new System.Drawing.Font(fm.Font.Name, fm.Font.Size * SH, fm.Font.Style, fm.Font.Unit, fm.Font.GdiCharSet, fm.Font.GdiVerticalFont);
        //    if (fm.Controls.Count != 0)
        //    {
        //        SetControlSize(fm);
        //    }
        //}

        //private static void SetControlSize(System.Windows.Forms.Control InitC)
        //{
        //    foreach (System.Windows.Forms.Control c in InitC.Controls)
        //    {
        //        c.Location = new System.Drawing.Point((int)(c.Location.X * SW), (int)(c.Location.Y * SH));
        //        c.Size = new System.Drawing.Size((int)(c.Size.Width * SW), (int)(c.Size.Height * SH));
        //        c.Font = new System.Drawing.Font(c.Font.Name, c.Font.Size * SH, c.Font.Style, c.Font.Unit, c.Font.GdiCharSet, c.Font.GdiVerticalFont);
        //        if (c.Controls.Count != 0)
        //        {
        //            SetControlSize(c);
        //        }
        //    }
        //}
        #endregion
    }
}
