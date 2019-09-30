using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.Utils;
using DevExpress.Utils.Win;

namespace Xr.Common.Controls
{
    public class FlyoutControl : FlyoutPanel
    {
        /// <summary>
        /// 获取设置弹出面板所属控件
        /// </summary>
        public Control HostControl { get; set; }

        [DefaultValue(false)]
        public bool IsDialogModel { get; set; }

        protected override FlyoutPanelToolForm CreateToolFormCore(Control owner, FlyoutPanel content, FlyoutPanelOptions options)
        {
            return new HostFlyoutPanelToolForm(owner, content, options, this.HostControl, this);
        }
    }

    public class HostFlyoutPanelToolForm : FlyoutPanelToolForm
    {
        private HostPopupToolWindowHandler handler = null;

        public HostFlyoutPanelToolForm(Control owner, FlyoutPanel flyoutPanel, FlyoutPanelOptions options, Control hostControl, FlyoutControl flyoutControl)
            : base(owner, flyoutPanel, options)
        {
            handler.HostControl = hostControl;      //CreateHandler方法在基类的构造函数中进行了调用，因此，这个地方的handler已经创建
            handler.FlyoutControl = flyoutControl;
        }

        protected override BasePopupToolWindowHandler CreateHandler()
        {
            handler = new HostPopupToolWindowHandler(this);
            return handler;
        }
    }

    public class HostPopupToolWindowHandler : BasePopupToolWindowHandler
    {
        public HostPopupToolWindowHandler(BasePopupToolWindow toolWindow)
            : base(toolWindow)
        {

        }

        public Control HostControl { get; set; }

        public FlyoutControl FlyoutControl { get; set; }

        protected override PopupToolWindowAnimationProviderBase CreateAnimationProvider()
        {
            //(this as IPopupToolWindowAnimationSupports).AnchorType
            //目前暂只支持从左到右的弹出方式，如果需要实现其他的弹出方法，请自行实现
            //并按照AnchorType的定义，来创建不同的弹出实现类，示例如下：
            //public static PopupToolWindowAnimationProviderBase Create(IPopupToolWindowAnimationSupports info, PopupToolWindowAnimation animationType)
            //{
            //    if (animationType == PopupToolWindowAnimation.Fade)
            //    {
            //        return new PopupToolWindowFadeAnimationProvider(info);
            //    }
            //    if (animationType != PopupToolWindowAnimation.Slide)
            //    {
            //        throw new ArgumentException("animationType");
            //    }

            switch ((this as IPopupToolWindowAnimationSupports).AnchorType)
            {
                case PopupToolWindowAnchor.TopRight:
                case PopupToolWindowAnchor.TopLeft:
                case PopupToolWindowAnchor.Top:
                case PopupToolWindowAnchor.Center:
                case PopupToolWindowAnchor.Manual:
                    return new HostPopupToolWindowUpDownSlideAnimationProvider(this, this.HostControl, this.FlyoutControl);

                case PopupToolWindowAnchor.Bottom:
                    return new HostPopupToolWindowDownUpSlideAnimationProvider(this, this.HostControl, this.FlyoutControl);

                case PopupToolWindowAnchor.Left:
                    return new HostPopupToolWindowLeftRightSlideAnimationProvider(this, this.HostControl, this.FlyoutControl);

                case PopupToolWindowAnchor.Right:
                    return new HostPopupToolWindowRightLeftSlideAnimationProvider(this, this.HostControl, this.FlyoutControl);

                default:
                    throw new ArgumentException("AnchorType");
            }
        }

        /// <summary>
        /// 直接弹出时的实现方式
        /// </summary>
        public override void OnImmediateShowToolWindow()
        {
            this.CheckToolWindowLocation();

            this.ToolWindow.Owner = null;
            this.ToolWindow.TopLevel = false;
            this.ToolWindow.Parent = this.HostControl;

            //控件的默认实现是创建一个新的窗体来承载弹出控件，因此，其坐标是基于屏幕最左上角计算出来的
            //现在改成将该窗体放到用户控件中，因此，需要减掉用户控件最左上角在屏幕上的坐标来获取正确结果
            var offset = this.HostControl.PointToScreen(new Point());
            this.ToolWindow.Location = new Point(this.ToolWindow.Location.X - offset.X, this.ToolWindow.Location.Y - offset.Y);

            this.ToolWindow.Show();
            //this.AnimationProvider.OnImmediateShowToolWindow();
        }
    }

    /// <summary>
    /// 实现从左到右的弹出方式
    /// </summary>
    public class HostPopupToolWindowLeftRightSlideAnimationProvider : PopupToolWindowLeftRightSlideAnimationProvider
    {
        private Control hostControl = null;
        private FlyoutControl flyoutControl = null;

        public HostPopupToolWindowLeftRightSlideAnimationProvider(IPopupToolWindowAnimationSupports info, Control hostControl, FlyoutControl flyoutControl)
            : base(info)
        {
            this.hostControl = hostControl;
            this.flyoutControl = flyoutControl;
        }

        public override void OnShowToolWindowCore()
        {
            base.ToolWindow.Owner = null;
            base.ToolWindow.TopLevel = false;
            base.ToolWindow.Parent = hostControl;

            if (flyoutControl.IsDialogModel)
            {
                this.CalcCheckPoints();
                base.ToolWindow.Location = base.StartPt;
                base.ToolWindow.Size = this.CalcTargetFormSize();
                this.OnStartAnimation();

                // base.ToolWindow.Parent.Controls.Remove(base.ToolWindow);
                //base.ToolWindow.ParentForm.Controls.Remove(base.ToolWindow);
                base.ToolWindow.Opacity = 0.001;

                base.ToolWindow.Show();
            }
            else
            {
                base.OnShowToolWindowCore();
            }
        }

        public override Point CalcTargetFormLocation()
        {
            var point = base.CalcTargetFormLocation();
            var offset = hostControl.PointToScreen(new Point());
            return new Point(point.X - offset.X, point.Y - offset.Y);
        }
    }


    public class HostPopupToolWindowRightLeftSlideAnimationProvider : PopupToolWindowRightLeftSlideAnimationProvider
    {
        private Control hostControl = null;
        private FlyoutControl flyoutControl = null;

        public HostPopupToolWindowRightLeftSlideAnimationProvider(IPopupToolWindowAnimationSupports info, Control hostControl, FlyoutControl flyoutControl)
            : base(info)
        {
            this.hostControl = hostControl;
            this.flyoutControl = flyoutControl;
        }

        public override void OnShowToolWindowCore()
        {

            base.ToolWindow.Owner = null;
            base.ToolWindow.TopLevel = false;
            base.ToolWindow.Parent = hostControl;
            if (flyoutControl.IsDialogModel)
            {
                this.CalcCheckPoints();
                base.ToolWindow.Location = base.StartPt;
                base.ToolWindow.Size = this.CalcTargetFormSize();
                this.OnStartAnimation();

                // base.ToolWindow.Parent.Controls.Remove(base.ToolWindow);
                //base.ToolWindow.ParentForm.Controls.Remove(base.ToolWindow);
                base.ToolWindow.Opacity = 0.001;

                base.ToolWindow.Show();
            }
            else
            {
                base.OnShowToolWindowCore();
            }
        }

        public override Point CalcTargetFormLocation()
        {
            var point = base.CalcTargetFormLocation();
            var offset = hostControl.PointToScreen(new Point());
            return new Point(point.X - offset.X, point.Y - offset.Y);
        }
    }

    public class HostPopupToolWindowDownUpSlideAnimationProvider : PopupToolWindowDownUpSlideAnimationProvider
    {
        private Control hostControl = null;
        private FlyoutControl flyoutControl = null;

        public HostPopupToolWindowDownUpSlideAnimationProvider(IPopupToolWindowAnimationSupports info, Control hostControl, FlyoutControl flyoutControl)
            : base(info)
        {
            this.hostControl = hostControl;
            this.flyoutControl = flyoutControl;
        }

        public override void OnShowToolWindowCore()
        {

            base.ToolWindow.Owner = null;
            base.ToolWindow.TopLevel = false;
            base.ToolWindow.Parent = hostControl;
            if (flyoutControl.IsDialogModel)
            {
                this.CalcCheckPoints();
                base.ToolWindow.Location = base.StartPt;
                base.ToolWindow.Size = this.CalcTargetFormSize();
                this.OnStartAnimation();

                // base.ToolWindow.Parent.Controls.Remove(base.ToolWindow);
                //base.ToolWindow.ParentForm.Controls.Remove(base.ToolWindow);
                base.ToolWindow.Opacity = 0.001;

                base.ToolWindow.Show();
            }
            else
            {
                base.OnShowToolWindowCore();
            }
        }

        public override Point CalcTargetFormLocation()
        {
            var point = base.CalcTargetFormLocation();
            var offset = hostControl.PointToScreen(new Point());
            return new Point(point.X - offset.X, point.Y - offset.Y);
        }
    }

    public class HostPopupToolWindowUpDownSlideAnimationProvider : PopupToolWindowUpDownSlideAnimationProvider
    {
        private Control hostControl = null;
        private FlyoutControl flyoutControl = null;

        public HostPopupToolWindowUpDownSlideAnimationProvider(IPopupToolWindowAnimationSupports info, Control hostControl, FlyoutControl flyoutControl)
            : base(info)
        {
            this.hostControl = hostControl;
            this.flyoutControl = flyoutControl;
        }

        public override void OnShowToolWindowCore()
        {
            base.ToolWindow.Owner = null;
            base.ToolWindow.TopLevel = false;
            base.ToolWindow.Parent = hostControl;
            if (flyoutControl.IsDialogModel)
            {
                this.CalcCheckPoints();
                base.ToolWindow.Location = base.StartPt;
                base.ToolWindow.Size = this.CalcTargetFormSize();
                this.OnStartAnimation();

                // base.ToolWindow.Parent.Controls.Remove(base.ToolWindow);
                //base.ToolWindow.ParentForm.Controls.Remove(base.ToolWindow);
                base.ToolWindow.Opacity = 0.001;

                base.ToolWindow.Show();
            }
            else
            {
                base.OnShowToolWindowCore();
            }
        }

        public override Point CalcTargetFormLocation()
        {
            var point = base.CalcTargetFormLocation();
            var offset = hostControl.PointToScreen(new Point());
            return new Point(point.X - offset.X, point.Y - offset.Y);
        }
    }
}
