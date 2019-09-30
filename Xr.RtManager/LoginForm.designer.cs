namespace Xr.RtManager
{
    partial class LoginForm
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
            Xr.RtManager.TTextBoxBorderRenderStyle tTextBoxBorderRenderStyle1 = new Xr.RtManager.TTextBoxBorderRenderStyle();
            Xr.RtManager.TTextBoxBorderRenderStyle tTextBoxBorderRenderStyle2 = new Xr.RtManager.TTextBoxBorderRenderStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnUp = new Xr.Common.Controls.ButtonControl();
            this.btnQuit = new Xr.Common.Controls.ButtonControl();
            this.btnLogin = new Xr.Common.Controls.ButtonControl();
            this.tbPassword = new Xr.RtManager.MyTextBox();
            this.tbLoginName = new Xr.RtManager.MyTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(70, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(70, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "密  码：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::Xr.RtManager.Properties.Resources.图片1;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(421, 112);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnUp.HoverBackColor = System.Drawing.Color.Empty;
            this.btnUp.Location = new System.Drawing.Point(276, 215);
            this.btnUp.Name = "btnUp";
            this.btnUp.SecondText = "";
            this.btnUp.Size = new System.Drawing.Size(75, 30);
            this.btnUp.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.btnUp.TabIndex = 166;
            this.btnUp.Text = "修改密码";
            this.btnUp.Click += new System.EventHandler(this.skinButton2_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnQuit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnQuit.HoverBackColor = System.Drawing.Color.Empty;
            this.btnQuit.Location = new System.Drawing.Point(175, 215);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.SecondText = "";
            this.btnQuit.Size = new System.Drawing.Size(75, 30);
            this.btnQuit.Style = Xr.Common.Controls.ButtonStyle.Del;
            this.btnQuit.TabIndex = 165;
            this.btnQuit.Text = "退出";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnLogin.HoverBackColor = System.Drawing.Color.Empty;
            this.btnLogin.Location = new System.Drawing.Point(74, 215);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.SecondText = "";
            this.btnLogin.Size = new System.Drawing.Size(75, 30);
            this.btnLogin.Style = Xr.Common.Controls.ButtonStyle.Query;
            this.btnLogin.TabIndex = 164;
            this.btnLogin.Text = "登录";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbPassword
            // 
            tTextBoxBorderRenderStyle1.LineColor = System.Drawing.Color.LightGray;
            tTextBoxBorderRenderStyle1.LineWidth = 1F;
            this.tbPassword.BorderRenderStyle = tTextBoxBorderRenderStyle1;
            this.tbPassword.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPassword.Location = new System.Drawing.Point(138, 169);
            this.tbPassword.MaxLength = 16;
            this.tbPassword.Multiline = true;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(180, 28);
            this.tbPassword.TabIndex = 145;
            this.tbPassword.TextMargin = new System.Windows.Forms.Padding(1, -1, 1, 1);
            this.tbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPassword_KeyDown);
            // 
            // tbLoginName
            // 
            tTextBoxBorderRenderStyle2.LineColor = System.Drawing.Color.LightGray;
            tTextBoxBorderRenderStyle2.LineWidth = 1F;
            this.tbLoginName.BorderRenderStyle = tTextBoxBorderRenderStyle2;
            this.tbLoginName.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbLoginName.Location = new System.Drawing.Point(138, 129);
            this.tbLoginName.Multiline = true;
            this.tbLoginName.Name = "tbLoginName";
            this.tbLoginName.Size = new System.Drawing.Size(180, 28);
            this.tbLoginName.TabIndex = 144;
            this.tbLoginName.TextMargin = new System.Windows.Forms.Padding(1, -1, 1, 1);
            this.tbLoginName.TextChanged += new System.EventHandler(this.tbLoginName_TextChanged);
            this.tbLoginName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbLoginName_KeyDown);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 261);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbLoginName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Resize += new System.EventHandler(this.LoginForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MyTextBox tbLoginName;
        private MyTextBox tbPassword;
        private Xr.Common.Controls.ButtonControl btnLogin;
        private Xr.Common.Controls.ButtonControl btnQuit;
        private Xr.Common.Controls.ButtonControl btnUp;
    }
}