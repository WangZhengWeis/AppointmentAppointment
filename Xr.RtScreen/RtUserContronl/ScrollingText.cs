using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

///Auto:wzw
///Time:2019-01-07
///文本内容从左往右或者从右往左滚动
namespace Xr.RtScreen.RtUserContronl
{
    /// <summary>
    /// 自定义控件（控件中的数据从右向左滚动）
    /// Summary description for ScrollingTextControl.
    /// </summary>
    [
    ToolboxBitmapAttribute(typeof(ScrollingText), "ScrollingText.bmp"),
    DefaultEvent("TextClicked")
    ]
    public class ScrollingText : System.Windows.Forms.Control
    {
        private Timer timer;
        public  string text = "Text";
        public float staticTextPos = 0;
        private float yPos = 0;
        private ScrollDirection scrollDirection = ScrollDirection.RightToLeft;
        private ScrollDirection currentDirection = ScrollDirection.LeftToRight;
        private VerticleTextPosition verticleTextPosition = VerticleTextPosition.Center;
        private int scrollPixelDistance = 2;
        private bool showBorder = true;
        private bool stopScrollOnMouseOver = false;
        private bool scrollOn = true;
        private Brush foregroundBrush = null;
        private Brush backgroundBrush = null;
        private Color borderColor = Color.Black;
        private RectangleF lastKnownRect;
        public ScrollingText()
        {
            InitializeComponent();
            var v = System.Environment.Version;
            if (v.Major < 2)
            {
                SetStyle(ControlStyles.DoubleBuffer, true);
            }
            else
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            }
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
            timer = new Timer();
            timer.Interval = 10;
            timer.Enabled = true;
            timer.Tick += new EventHandler(Tick);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (foregroundBrush != null)
                {
                    foregroundBrush.Dispose();
                }
                if (backgroundBrush != null)
                {
                    backgroundBrush.Dispose();
                }
                if (timer != null)
                {
                    timer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Name = "ScrollingText";
            this.Size = new System.Drawing.Size(216, 40);
            this.Click += new System.EventHandler(this.ScrollingText_Click);
        }

        private void Tick(object sender, EventArgs e)
        {
            var refreshRect = new RectangleF(0, 0, Size.Width, Size.Height);
            var updateRegion = new Region(refreshRect);
            Invalidate(updateRegion);
            Update();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            DrawScrollingText(pe.Graphics);
            base.OnPaint(pe);
        }
        public void DrawScrollingText(Graphics canvas)
        {
            canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            canvas.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            var stringSize = canvas.MeasureString(text, Font);
            if (scrollOn)
            {
                CalcTextPosition(stringSize);
            }
            if (backgroundBrush != null)
            {
                canvas.FillRectangle(backgroundBrush, 0, 0, ClientSize.Width, ClientSize.Height);
            }
            else
            {
                canvas.Clear(BackColor);
            }
            if (showBorder)
            {
                using (var borderPen = new Pen(borderColor))
                {
                    canvas.DrawRectangle(borderPen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
                }
            }
            if (foregroundBrush == null)
            {
                using (Brush tempForeBrush = new System.Drawing.SolidBrush(ForeColor))
                {
                    canvas.DrawString(text, Font, tempForeBrush, staticTextPos, yPos);
                }
            }
            else
            {
                canvas.DrawString(text, Font, foregroundBrush, staticTextPos, yPos);
            }
            lastKnownRect = new RectangleF(staticTextPos, yPos, stringSize.Width, stringSize.Height);
            EnableTextLink(lastKnownRect);
        }

        private void CalcTextPosition(SizeF stringSize)
        {
            switch (scrollDirection)
            {
                case ScrollDirection.RightToLeft:
                    if (staticTextPos < (-1 * (stringSize.Width)))
                    {
                        staticTextPos = ClientSize.Width - 1;
                    }
                    else
                    {
                        staticTextPos -= scrollPixelDistance;
                    }
                    break;
                case ScrollDirection.LeftToRight:
                    if (staticTextPos > ClientSize.Width)
                    {
                        staticTextPos = -1 * stringSize.Width;
                    }
                    else
                    {
                        staticTextPos += scrollPixelDistance;
                    }
                    break;
                case ScrollDirection.Bouncing:
                    if (currentDirection == ScrollDirection.RightToLeft)
                    {
                        if (staticTextPos < 0)
                        {
                            currentDirection = ScrollDirection.LeftToRight;
                        }
                        else
                        {
                            staticTextPos -= scrollPixelDistance;
                        }
                    }
                    else
                    {
                        if (currentDirection == ScrollDirection.LeftToRight)
                        {
                            if (staticTextPos > ClientSize.Width - stringSize.Width)
                            {
                                currentDirection = ScrollDirection.RightToLeft;
                            }
                            else
                            {
                                staticTextPos += scrollPixelDistance;
                            }
                        }
                    }
                    break;
            }
            switch (verticleTextPosition)
            {
                case VerticleTextPosition.Top:
                    yPos = 2;
                    break;
                case VerticleTextPosition.Center:
                    yPos = (ClientSize.Height / 2) - (stringSize.Height / 2);
                    break;
                case VerticleTextPosition.Botom:
                    yPos = ClientSize.Height - stringSize.Height;
                    break;
            }
        }

        private void EnableTextLink(RectangleF textRect)
        {
            var curPt = PointToClient(Cursor.Position);
            if (textRect.Contains(curPt))
            {
                if (stopScrollOnMouseOver)
                {
                    scrollOn = false;
                }
                Cursor = Cursors.Hand;
            }
            else
            {
                scrollOn = true;

                Cursor = Cursors.Default;
            }
        }

        private void ScrollingText_Click(object sender, System.EventArgs e)
        {
            if (Cursor == Cursors.Hand)
            {
                OnTextClicked(this, new EventArgs());
            }
        }

        public delegate void TextClickEventHandler(object sender, EventArgs args);
        public event TextClickEventHandler TextClicked;

        private void OnTextClicked(object sender, EventArgs args)
        {
            if (TextClicked != null)
            {
                TextClicked(sender, args);
            }
        }


        [
        Browsable(true),
        CategoryAttribute("Scrolling Text"),
        Description("The timer interval that determines how often the control is repainted")
        ]
        public int TextScrollSpeed
        {
            set
            {
                timer.Interval = value;
            }
            get
            {
                return timer.Interval;
            }
        }

        [
        Browsable(true),
        CategoryAttribute("Scrolling Text"),
        Description("How many pixels will the text be moved per Paint")
        ]
        public int TextScrollDistance
        {
            set
            {
                scrollPixelDistance = value;
            }
            get
            {
                return scrollPixelDistance;
            }
        }

        [
        Browsable(true),
        CategoryAttribute("Scrolling Text"),
        Description("The text that will scroll accros the control")
        ]
        public string ScrollText
        {
            set
            {
                text = value;
                Invalidate();
                Update();
            }
            get
            {
                return text;
            }
        }
        [
        Browsable(true),
        CategoryAttribute("Scrolling Text"),
        Description("What direction the text will scroll: Left to Right, Right to Left, or Bouncing")
        ]
        public ScrollDirection ScrollDirection
        {
            set
            {
                scrollDirection = value;
            }
            get
            {
                return scrollDirection;
            }
        }

        [
        Browsable(true),
        CategoryAttribute("Scrolling Text"),
        Description("The verticle alignment of the text")
        ]
        public VerticleTextPosition VerticleTextPosition
        {
            set
            {
                verticleTextPosition = value;
            }
            get
            {
                return verticleTextPosition;
            }
        }

        [
        Browsable(true),
        CategoryAttribute("Scrolling Text"),
        Description("Turns the border on or off")
        ]
        public bool ShowBorder
        {
            set
            {
                showBorder = value;
            }
            get
            {
                return showBorder;
            }
        }

        [
        Browsable(true),
        CategoryAttribute("Scrolling Text"),
        Description("The color of the border")
        ]
        public Color BorderColor
        {
            set
            {
                borderColor = value;
            }
            get
            {
                return borderColor;
            }
        }

        [
        Browsable(true),
        CategoryAttribute("Scrolling Text"),
        Description("Determines if the text will stop scrolling if the user's mouse moves over the text")
        ]
        public bool StopScrollOnMouseOver
        {
            set
            {
                stopScrollOnMouseOver = value;
            }
            get
            {
                return stopScrollOnMouseOver;
            }
        }

        [
        Browsable(false)
        ]
        public Brush ForegroundBrush
        {
            set
            {
                foregroundBrush = value;
            }
            get
            {
                return foregroundBrush;
            }
        }

        [
        ReadOnly(true)
        ]
        public Brush BackgroundBrush
        {
            set
            {
                backgroundBrush = value;
            }
            get
            {
                return backgroundBrush;
            }
        }
    }



    public enum ScrollDirection
    {
        RightToLeft
        ,
        LeftToRight
        ,
        Bouncing
    }

    public enum VerticleTextPosition
    {
        Top
        ,
        Center
        ,
        Botom
    }
}
