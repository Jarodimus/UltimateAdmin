namespace UltimateAdmin
{
    partial class ViewAddOU
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
            this.ouTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.exampleLabel = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.enterAliasLabel = new System.Windows.Forms.Label();
            this.enterAliasValue = new System.Windows.Forms.TextBox();
            this.OUGroupLabel = new System.Windows.Forms.Label();
            this.OUGroupValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ouTextBox
            // 
            this.ouTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ouTextBox.Location = new System.Drawing.Point(35, 107);
            this.ouTextBox.Name = "ouTextBox";
            this.ouTextBox.Size = new System.Drawing.Size(337, 25);
            this.ouTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Add or Modify an OU Path.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Example: ";
            // 
            // exampleLabel
            // 
            this.exampleLabel.AutoSize = true;
            this.exampleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exampleLabel.Location = new System.Drawing.Point(32, 90);
            this.exampleLabel.Name = "exampleLabel";
            this.exampleLabel.Size = new System.Drawing.Size(286, 15);
            this.exampleLabel.TabIndex = 3;
            this.exampleLabel.Text = "ou=Users,ou=,ou=us,dc=corp,dc=footlocker,dc=net";
            // 
            // addButton
            // 
            this.addButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addButton.Location = new System.Drawing.Point(216, 196);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 25);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.OnAddClick);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(297, 196);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 25);
            this.button2.TabIndex = 5;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnCloseClick);
            // 
            // enterAliasLabel
            // 
            this.enterAliasLabel.AutoSize = true;
            this.enterAliasLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterAliasLabel.Location = new System.Drawing.Point(32, 144);
            this.enterAliasLabel.Name = "enterAliasLabel";
            this.enterAliasLabel.Size = new System.Drawing.Size(261, 17);
            this.enterAliasLabel.TabIndex = 6;
            this.enterAliasLabel.Text = "Enter a 1-5 letter alias for the selected OU. ";
            // 
            // enterAliasValue
            // 
            this.enterAliasValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterAliasValue.Location = new System.Drawing.Point(35, 166);
            this.enterAliasValue.Name = "enterAliasValue";
            this.enterAliasValue.Size = new System.Drawing.Size(92, 25);
            this.enterAliasValue.TabIndex = 7;
            // 
            // OUGroupLabel
            // 
            this.OUGroupLabel.AutoSize = true;
            this.OUGroupLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OUGroupLabel.Location = new System.Drawing.Point(32, 44);
            this.OUGroupLabel.Name = "OUGroupLabel";
            this.OUGroupLabel.Size = new System.Drawing.Size(71, 17);
            this.OUGroupLabel.TabIndex = 8;
            this.OUGroupLabel.Text = "OU Group:";
            // 
            // OUGroupValue
            // 
            this.OUGroupValue.AutoSize = true;
            this.OUGroupValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OUGroupValue.Location = new System.Drawing.Point(103, 44);
            this.OUGroupValue.Name = "OUGroupValue";
            this.OUGroupValue.Size = new System.Drawing.Size(77, 17);
            this.OUGroupValue.TabIndex = 9;
            this.OUGroupValue.Text = "Groupname";
            // 
            // ViewAddOU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 236);
            this.Controls.Add(this.OUGroupValue);
            this.Controls.Add(this.OUGroupLabel);
            this.Controls.Add(this.enterAliasValue);
            this.Controls.Add(this.enterAliasLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.exampleLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ouTextBox);
            this.Name = "ViewAddOU";
            this.ShowIcon = false;
            this.Text = "Add User OU Path";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ouTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label exampleLabel;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label enterAliasLabel;
        private System.Windows.Forms.TextBox enterAliasValue;
        private System.Windows.Forms.Label OUGroupLabel;
        private System.Windows.Forms.Label OUGroupValue;
    }
}