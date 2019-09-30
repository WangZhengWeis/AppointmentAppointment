using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

///Auto:wzw
///Time:2019-01-07
///文本框内容从下往上循环滚动
namespace Xr.RtScreen.RtUserContronl
{
    [
    ToolboxBitmapAttribute(typeof(ScrollingTexts), "ScrollingTexts.bmp"),
    DefaultEvent("TextClicked")
    ]
    public class ScrollingTexts : System.Windows.Forms.Control
    {
        private Timer timer;
        private string text = "Text";
        private float staticTextPos = 0;
        private float yPos = 0;
        private ScrollDirection scrollDirection = ScrollDirection.RightToLeft;
        private ScrollDirection currentDirection = ScrollDirection.LeftToRight;
        private VerticleTextPosition verticleTextPosition = VerticleTextPosition.Center;
        private int scrollPixelDistance = 1;
        private bool showBorder = true;
        private bool stopScrollOnMouseOver = false;
        private bool scrollOn = true;
        private Brush foregroundBrush = null;
        private Brush backgroundBrush = null;
        private Color borderColor = Color.Black;
        public ScrollingTexts()
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
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Tick += new EventHandler(Tick);
        }
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
            this.Name = "ScrollingTexts";
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

        protected override void OnSizeChanged(EventArgs e)
        {
            p = new PointF(0, ClientSize.Height);
            base.OnSizeChanged(e);
        }

        private PointF p;
        public void DrawScrollingText(Graphics canvas)
        {
            canvas.SmoothingMode = SmoothingMode.HighQuality;
            canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
            var stringSize = canvas.MeasureString(text, Font);
            if (scrollOn)
            {
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
            if (BackgroundImage != null)
            {
                canvas.DrawImage(BackgroundImage, ClientRectangle);
            }
            p = new PointF(0, p.Y - scrollPixelDistance);
            var textRows = GetStringRows(canvas, Font, text, Size.Width);
            var strDraw = string.Empty;
            foreach (string str in textRows)
            {
                strDraw += str + "\n";
            }
            stringSize = canvas.MeasureString(strDraw, Font);
            if (p.Y <= -1 * stringSize.Height)
            {
                p = new PointF(0, ClientSize.Height);
            }
            if (foregroundBrush == null)
            {
                using (Brush tempForeBrush = new System.Drawing.SolidBrush(ForeColor))
                {
                    canvas.DrawString(strDraw, Font, tempForeBrush, p);
                }
            }
            else
            {
                canvas.DrawString(strDraw, Font, foregroundBrush, p);
            }
        }

        /// <summary>
        /// 绘制文本自动换行（超出截断）
        /// </summary>
        /// <param name="graphic">绘图图面</param>
        /// <param name="font">字体</param>
        /// <param name="text">文本</param>
        /// <param name="recangle">绘制范围</param>
        private void DrawStringWrap(Graphics graphic, Font font, string text, Rectangle recangle)
        {
            var textRows = GetStringRows(graphic, font, text, recangle.Width);
            var rowHeight = (int)(Math.Ceiling(graphic.MeasureString("测试", font).Height));
            var maxRowCount = recangle.Height / rowHeight;
            var drawRowCount = (maxRowCount < textRows.Count) ? maxRowCount : textRows.Count;
            var top = (recangle.Height - rowHeight * drawRowCount) / 2;
            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            for (var i = 0; i < drawRowCount; i++)
            {
                var fontRectanle = new Rectangle(recangle.Left, top + rowHeight * i, recangle.Width, rowHeight);
                graphic.DrawString(textRows[i], font, new SolidBrush(Color.Black), fontRectanle, sf);
            }
        }

        /// <summary>
        /// 将文本分行
        /// </summary>
        /// <param name="graphic">绘图图面</param>
        /// <param name="font">字体</param>
        /// <param name="text">文本</param>
        /// <param name="width">行宽</param>
        /// <returns></returns>
        private List<string> GetStringRows(Graphics graphic, Font font, string text, int width)
        {
            var RowBeginIndex = 0;
            var rowEndIndex = 0;
            var textLength = text.Length;
            var textRows = new List<string>();
            for (var index = 0; index < textLength; index++)
            {
                rowEndIndex = index;
                if (index == textLength - 1)
                {
                    textRows.Add(text.Substring(RowBeginIndex));
                }
                else
                {
                    if (rowEndIndex + 1 < text.Length && text.Substring(rowEndIndex, 2) == "\r\n")
                    {
                        textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                        rowEndIndex = index += 2;
                        RowBeginIndex = rowEndIndex;
                    }
                    else
                    {
                        if (graphic.MeasureString(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex + 1), font).Width > width)
                        {
                            textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                            RowBeginIndex = rowEndIndex;
                        }
                    }
                }
            }

            return textRows;
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
        CategoryAttribute("Behavior"),
        Description("Indicates whether the control is enabled")
        ]
        public new bool Enabled
        {
            set
            {
                timer.Enabled = value;
                base.Enabled = value;
            }

            get
            {
                return base.Enabled;
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
}
