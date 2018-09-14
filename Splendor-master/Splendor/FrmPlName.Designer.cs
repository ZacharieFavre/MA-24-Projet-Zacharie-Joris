namespace Splendor
{
    partial class FrmPlName
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxName = new System.Windows.Forms.TextBox();
            this.cmdValidateName = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nom du Joueur";
            // 
            // txtBoxName
            // 
            this.txtBoxName.Location = new System.Drawing.Point(16, 46);
            this.txtBoxName.Name = "txtBoxName";
            this.txtBoxName.Size = new System.Drawing.Size(145, 20);
            this.txtBoxName.TabIndex = 1;
            // 
            // cmdValidateName
            // 
            this.cmdValidateName.Location = new System.Drawing.Point(13, 81);
            this.cmdValidateName.Name = "cmdValidateName";
            this.cmdValidateName.Size = new System.Drawing.Size(75, 23);
            this.cmdValidateName.TabIndex = 2;
            this.cmdValidateName.Text = "Valider";
            this.cmdValidateName.UseVisualStyleBackColor = true;
            this.cmdValidateName.Click += new System.EventHandler(this.cmdValidateName_Click);
            // 
            // FrmPlName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 116);
            this.Controls.Add(this.cmdValidateName);
            this.Controls.Add(this.txtBoxName);
            this.Controls.Add(this.label1);
            this.Name = "FrmPlName";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxName;
        private System.Windows.Forms.Button cmdValidateName;
    }
}