namespace Xr.Common.Controls
{
    partial class RichEditorForm
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
            this.panelEx1 = new Xr.Common.Controls.PanelEx();
            this.richEditor1 = new Xr.Common.Controls.RichEditor();
            this.btnCancel = new Xr.Common.Controls.ButtonControl();
            this.btnSave = new Xr.Common.Controls.ButtonControl();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.BorderColor = System.Drawing.Color.Green;
            this.panelEx1.BorderSize = 1;
            this.panelEx1.BorderStyleBottom = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.BorderStyleLeft = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.BorderStyleRight = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.BorderStyleTop = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.Controls.Add(this.richEditor1);
            this.panelEx1.Location = new System.Drawing.Point(12, 12);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx1.Size = new System.Drawing.Size(752, 442);
            this.panelEx1.TabIndex = 3;
            // 
            // richEditor1
            // 
            this.richEditor1.BorderColor = System.Drawing.Color.Black;
            this.richEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditor1.HeightLightBolorColor = System.Drawing.Color.Black;
            this.richEditor1.ImagUploadUrl = null;
            this.richEditor1.Location = new System.Drawing.Point(1, 1);
            this.richEditor1.Name = "richEditor1";
            this.richEditor1.Size = new System.Drawing.Size(750, 440);
            this.richEditor1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnCancel.HoverBackColor = System.Drawing.Color.Empty;
            this.btnCancel.Location = new System.Drawing.Point(689, 469);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.Style = Xr.Common.Controls.ButtonStyle.Return;
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnSave.HoverBackColor = System.Drawing.Color.Empty;
            this.btnSave.Location = new System.Drawing.Point(598, 469);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "确定";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // RichEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 511);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RichEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑信息";
            this.Load += new System.EventHandler(this.RichEditorForm_Load);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ButtonControl btnSave;
        private ButtonControl btnCancel;
        private PanelEx panelEx1;
        private RichEditor richEditor1;
    }
}