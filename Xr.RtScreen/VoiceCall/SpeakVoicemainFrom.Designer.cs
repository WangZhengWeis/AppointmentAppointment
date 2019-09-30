namespace Xr.RtScreen.VoiceCall
{
    partial class SpeakVoicemainFrom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpeakVoicemainFrom));
            this.txt_log = new System.Windows.Forms.TextBox();
            this.lab_lasttime = new System.Windows.Forms.Label();
            this.lab_startime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.lab_failedCount = new System.Windows.Forms.Label();
            this.lab_succeedCount = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // txt_log
            // 
            this.txt_log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_log.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_log.Location = new System.Drawing.Point(4, 34);
            this.txt_log.Multiline = true;
            this.txt_log.Name = "txt_log";
            this.txt_log.ReadOnly = true;
            this.txt_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_log.Size = new System.Drawing.Size(714, 297);
            this.txt_log.TabIndex = 19;
            // 
            // lab_lasttime
            // 
            this.lab_lasttime.AutoSize = true;
            this.lab_lasttime.Location = new System.Drawing.Point(346, 9);
            this.lab_lasttime.Name = "lab_lasttime";
            this.lab_lasttime.Size = new System.Drawing.Size(65, 12);
            this.lab_lasttime.TabIndex = 16;
            this.lab_lasttime.Text = "最后时间：";
            // 
            // lab_startime
            // 
            this.lab_startime.AutoSize = true;
            this.lab_startime.Location = new System.Drawing.Point(159, 9);
            this.lab_startime.Name = "lab_startime";
            this.lab_startime.Size = new System.Drawing.Size(65, 12);
            this.lab_startime.TabIndex = 15;
            this.lab_startime.Text = "开始时间：";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "开始播放（暂停中）";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lab_failedCount
            // 
            this.lab_failedCount.AutoSize = true;
            this.lab_failedCount.Location = new System.Drawing.Point(627, 9);
            this.lab_failedCount.Name = "lab_failedCount";
            this.lab_failedCount.Size = new System.Drawing.Size(65, 12);
            this.lab_failedCount.TabIndex = 18;
            this.lab_failedCount.Text = "失败次数：";
            // 
            // lab_succeedCount
            // 
            this.lab_succeedCount.AutoSize = true;
            this.lab_succeedCount.Location = new System.Drawing.Point(529, 9);
            this.lab_succeedCount.Name = "lab_succeedCount";
            this.lab_succeedCount.Size = new System.Drawing.Size(65, 12);
            this.lab_succeedCount.TabIndex = 17;
            this.lab_succeedCount.Text = "正常次数：";
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // SpeakVoicemainFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 335);
            this.Controls.Add(this.txt_log);
            this.Controls.Add(this.lab_lasttime);
            this.Controls.Add(this.lab_startime);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lab_failedCount);
            this.Controls.Add(this.lab_succeedCount);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpeakVoicemainFrom";
            this.Text = "播放语音";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpeakVoicemainFrom_FormClosing);
            this.Load += new System.EventHandler(this.SpeakVoicemainFrom_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_log;
        private System.Windows.Forms.Label lab_lasttime;
        private System.Windows.Forms.Label lab_startime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lab_failedCount;
        private System.Windows.Forms.Label lab_succeedCount;
        private System.Windows.Forms.Timer timer2;
    }
}