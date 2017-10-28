namespace UltimateAdmin
{
    partial class ViewLoggedInUser
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
            this.domainNameLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.noUsersLoggedOnLabel = new System.Windows.Forms.Label();
            this.nonDomainUserNameLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // domainNameLabel
            // 
            this.domainNameLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.domainNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.domainNameLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainNameLabel.ForeColor = System.Drawing.Color.Blue;
            this.domainNameLabel.Location = new System.Drawing.Point(0, 0);
            this.domainNameLabel.Name = "domainNameLabel";
            this.domainNameLabel.Size = new System.Drawing.Size(279, 42);
            this.domainNameLabel.TabIndex = 0;
            this.domainNameLabel.Text = "CORP\\u381522a";
            this.domainNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(102, 80);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnOKClick);
            // 
            // noUsersLoggedOnLabel
            // 
            this.noUsersLoggedOnLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noUsersLoggedOnLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noUsersLoggedOnLabel.Location = new System.Drawing.Point(0, 0);
            this.noUsersLoggedOnLabel.Name = "noUsersLoggedOnLabel";
            this.noUsersLoggedOnLabel.Size = new System.Drawing.Size(279, 42);
            this.noUsersLoggedOnLabel.TabIndex = 2;
            this.noUsersLoggedOnLabel.Text = "No users are currently logged on.";
            this.noUsersLoggedOnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nonDomainUserNameLabel
            // 
            this.nonDomainUserNameLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.nonDomainUserNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nonDomainUserNameLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nonDomainUserNameLabel.ForeColor = System.Drawing.Color.Black;
            this.nonDomainUserNameLabel.Location = new System.Drawing.Point(0, 0);
            this.nonDomainUserNameLabel.Name = "nonDomainUserNameLabel";
            this.nonDomainUserNameLabel.Size = new System.Drawing.Size(279, 42);
            this.nonDomainUserNameLabel.TabIndex = 3;
            this.nonDomainUserNameLabel.Text = "CORP\\u381522a";
            this.nonDomainUserNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.nonDomainUserNameLabel);
            this.panel1.Controls.Add(this.domainNameLabel);
            this.panel1.Controls.Add(this.noUsersLoggedOnLabel);
            this.panel1.Location = new System.Drawing.Point(0, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(279, 42);
            this.panel1.TabIndex = 4;
            // 
            // ViewLoggedInUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 115);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ViewLoggedInUser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Logged on to:";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label domainNameLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label noUsersLoggedOnLabel;
        private System.Windows.Forms.Label nonDomainUserNameLabel;
        private System.Windows.Forms.Panel panel1;
    }
}