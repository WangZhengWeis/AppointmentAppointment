namespace Xr.RtManager.Pages.triage
{
    partial class RegisterRecordForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.reservationInfoPanel4 = new Xr.Common.Controls.ReservationInfoPanel();
            this.reservationInfoPanel3 = new Xr.Common.Controls.ReservationInfoPanel();
            this.reservationInfoPanel2 = new Xr.Common.Controls.ReservationInfoPanel();
            this.reservationInfoPanel1 = new Xr.Common.Controls.ReservationInfoPanel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.reservationInfoPanel4);
            this.panel1.Controls.Add(this.reservationInfoPanel3);
            this.panel1.Controls.Add(this.reservationInfoPanel2);
            this.panel1.Controls.Add(this.reservationInfoPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(668, 361);
            this.panel1.TabIndex = 0;
            // 
            // reservationInfoPanel4
            // 
            this.reservationInfoPanel4.BackColor = System.Drawing.Color.Transparent;
            this.reservationInfoPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.reservationInfoPanel4.Location = new System.Drawing.Point(0, 294);
            this.reservationInfoPanel4.Name = "reservationInfoPanel4";
            this.reservationInfoPanel4.obj = null;
            this.reservationInfoPanel4.Padding = new System.Windows.Forms.Padding(2);
            this.reservationInfoPanel4.Size = new System.Drawing.Size(651, 98);
            this.reservationInfoPanel4.TabIndex = 3;
            // 
            // reservationInfoPanel3
            // 
            this.reservationInfoPanel3.BackColor = System.Drawing.Color.Transparent;
            this.reservationInfoPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.reservationInfoPanel3.Location = new System.Drawing.Point(0, 196);
            this.reservationInfoPanel3.Name = "reservationInfoPanel3";
            this.reservationInfoPanel3.obj = null;
            this.reservationInfoPanel3.Padding = new System.Windows.Forms.Padding(2);
            this.reservationInfoPanel3.Size = new System.Drawing.Size(651, 98);
            this.reservationInfoPanel3.TabIndex = 2;
            // 
            // reservationInfoPanel2
            // 
            this.reservationInfoPanel2.BackColor = System.Drawing.Color.Transparent;
            this.reservationInfoPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.reservationInfoPanel2.Location = new System.Drawing.Point(0, 98);
            this.reservationInfoPanel2.Name = "reservationInfoPanel2";
            this.reservationInfoPanel2.obj = null;
            this.reservationInfoPanel2.Padding = new System.Windows.Forms.Padding(2);
            this.reservationInfoPanel2.Size = new System.Drawing.Size(651, 98);
            this.reservationInfoPanel2.TabIndex = 1;
            // 
            // reservationInfoPanel1
            // 
            this.reservationInfoPanel1.BackColor = System.Drawing.Color.Transparent;
            this.reservationInfoPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.reservationInfoPanel1.Location = new System.Drawing.Point(0, 0);
            this.reservationInfoPanel1.Name = "reservationInfoPanel1";
            this.reservationInfoPanel1.obj = null;
            this.reservationInfoPanel1.Padding = new System.Windows.Forms.Padding(2);
            this.reservationInfoPanel1.Size = new System.Drawing.Size(651, 98);
            this.reservationInfoPanel1.TabIndex = 0;
            // 
            // RegisterRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 381);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterRecordForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "预约记录";
            this.Load += new System.EventHandler(this.RegisterRecordForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Xr.Common.Controls.ReservationInfoPanel reservationInfoPanel4;
        private Xr.Common.Controls.ReservationInfoPanel reservationInfoPanel3;
        private Xr.Common.Controls.ReservationInfoPanel reservationInfoPanel2;
        private Xr.Common.Controls.ReservationInfoPanel reservationInfoPanel1;

    }
}