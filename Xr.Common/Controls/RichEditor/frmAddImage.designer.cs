using System.Windows.Forms;
namespace Xr.Common.Controls
{
    partial class frmAddImage
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
            this.rBtn_URL = new System.Windows.Forms.RadioButton();
            this.rBtn_Local = new System.Windows.Forms.RadioButton();
            this.txt_URL = new System.Windows.Forms.TextBox();
            this.txtFileName = new System.Windows.Forms.Label();
            this.btnCancel = new Xr.Common.Controls.ButtonControl();
            this.btnOK = new Xr.Common.Controls.ButtonControl();
            this.btn_LocalImageUpdate = new Xr.Common.Controls.ButtonControl();
            this.SuspendLayout();
            // 
            // rBtn_URL
            // 
            this.rBtn_URL.AutoSize = true;
            this.rBtn_URL.Location = new System.Drawing.Point(12, 9);
            this.rBtn_URL.MinimumSize = new System.Drawing.Size(22, 22);
            this.rBtn_URL.Name = "rBtn_URL";
            this.rBtn_URL.Size = new System.Drawing.Size(137, 22);
            this.rBtn_URL.TabIndex = 3;
            this.rBtn_URL.Text = "URL地址（网络图片）";
            this.rBtn_URL.UseVisualStyleBackColor = true;
            this.rBtn_URL.CheckedChanged += new System.EventHandler(this.rBtn_URL_CheckedChanged);
            // 
            // rBtn_Local
            // 
            this.rBtn_Local.AutoSize = true;
            this.rBtn_Local.Checked = true;
            this.rBtn_Local.Location = new System.Drawing.Point(12, 67);
            this.rBtn_Local.MinimumSize = new System.Drawing.Size(22, 22);
            this.rBtn_Local.Name = "rBtn_Local";
            this.rBtn_Local.Size = new System.Drawing.Size(71, 22);
            this.rBtn_Local.TabIndex = 2;
            this.rBtn_Local.TabStop = true;
            this.rBtn_Local.Text = "本地图片";
            this.rBtn_Local.UseVisualStyleBackColor = true;
            // 
            // txt_URL
            // 
            this.txt_URL.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_URL.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_URL.Location = new System.Drawing.Point(36, 36);
            this.txt_URL.Name = "txt_URL";
            this.txt_URL.Size = new System.Drawing.Size(412, 21);
            this.txt_URL.TabIndex = 0;
            this.txt_URL.Text = "http://";
            // 
            // txtFileName
            // 
            this.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFileName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFileName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFileName.Location = new System.Drawing.Point(151, 95);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(297, 21);
            this.txtFileName.TabIndex = 2;
            this.txtFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnCancel.HoverBackColor = System.Drawing.Color.Empty;
            this.btnCancel.Location = new System.Drawing.Point(247, 148);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 25);
            this.btnCancel.Style = Xr.Common.Controls.ButtonStyle.Return;
            this.btnCancel.TabIndex = 5;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnOK.HoverBackColor = System.Drawing.Color.Empty;
            this.btnOK.Location = new System.Drawing.Point(96, 148);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 25);
            this.btnOK.Style = Xr.Common.Controls.ButtonStyle.Green;
            this.btnOK.TabIndex = 4;
            this.btnOK.TabStop = false;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.OnBtnOkClick);
            // 
            // btn_LocalImageUpdate
            // 
            this.btn_LocalImageUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btn_LocalImageUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btn_LocalImageUpdate.HoverBackColor = System.Drawing.Color.Empty;
            this.btn_LocalImageUpdate.Location = new System.Drawing.Point(36, 95);
            this.btn_LocalImageUpdate.Name = "btn_LocalImageUpdate";
            this.btn_LocalImageUpdate.Size = new System.Drawing.Size(108, 21);
            this.btn_LocalImageUpdate.Style = Xr.Common.Controls.ButtonStyle.Green;
            this.btn_LocalImageUpdate.TabIndex = 1;
            this.btn_LocalImageUpdate.TabStop = false;
            this.btn_LocalImageUpdate.Text = "上传本地图片...";
            this.btn_LocalImageUpdate.Click += new System.EventHandler(this.btn_LocalImageUpdate_Click);
            // 
            // frmAddImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(455, 194);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.btn_LocalImageUpdate);
            this.Controls.Add(this.txt_URL);
            this.Controls.Add(this.rBtn_Local);
            this.Controls.Add(this.rBtn_URL);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddImage";
            this.Text = "添加图片";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioButton rBtn_Local;
        private RadioButton rBtn_URL;
        private TextBox txt_URL;
        private Xr.Common.Controls.ButtonControl btn_LocalImageUpdate;
        private Label txtFileName;
        private Xr.Common.Controls.ButtonControl btnOK;
        private Xr.Common.Controls.ButtonControl btnCancel;

    }
}