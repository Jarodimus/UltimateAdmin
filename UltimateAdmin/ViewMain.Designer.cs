using UltimateAdmin.Presentation;
namespace UltimateAdmin
{
    partial class ViewMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewMain));
            this.button1 = new System.Windows.Forms.Button();
            this.SystemMgr = new UltimateAdmin.ViewSystemManager();
            this.Toolbar = new UltimateAdmin.Toolbar();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(30, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 40);
            this.button1.TabIndex = 4;
            this.button1.TabStop = false;
            this.button1.Text = "System Manager";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnSystemManager);
            // 
            // SystemMgr
            // 
            this.SystemMgr.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SystemMgr.BackColor = System.Drawing.Color.Transparent;
            this.SystemMgr.BIOSValueLabel = "";
            this.SystemMgr.BitLockerRecoveryKey = "";
            this.SystemMgr.Description = "Description";
            this.SystemMgr.Location = new System.Drawing.Point(30, 93);
            this.SystemMgr.MakeModelValueLabel = "";
            this.SystemMgr.Margin = new System.Windows.Forms.Padding(0);
            this.SystemMgr.Name = "SystemMgr";
            this.SystemMgr.OnlineStatusLabel = "Online";
            this.SystemMgr.OrganizationalUnit = "OU";
            this.SystemMgr.OSValueLabel = "";
            this.SystemMgr.Presenter = null;
            this.SystemMgr.SerialValueLabel = "";
            this.SystemMgr.Size = new System.Drawing.Size(620, 460);
            this.SystemMgr.TabIndex = 6;
            this.SystemMgr.TimeLabel = "10:51:31 AM";
            this.SystemMgr.TypeValueLabel = "";
            // 
            // Toolbar
            // 
            this.Toolbar.AppController = null;
            this.Toolbar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Toolbar.Location = new System.Drawing.Point(-4, 0);
            this.Toolbar.Name = "Toolbar";
            this.Toolbar.Size = new System.Drawing.Size(666, 28);
            this.Toolbar.TabIndex = 8;
            // 
            // ViewMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(664, 556);
            this.Controls.Add(this.Toolbar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SystemMgr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ViewMain";
            this.Text = "UltimateAdmin";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.onClose);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private ViewSystemManager SystemMgr;
        private Toolbar Toolbar;
    }
}