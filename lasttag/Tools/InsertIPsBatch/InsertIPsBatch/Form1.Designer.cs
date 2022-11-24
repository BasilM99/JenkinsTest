namespace InsertIPsBatch
{
    partial class Form1
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnProcess = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lblSource = new System.Windows.Forms.Label();
            this.btnSource = new System.Windows.Forms.Button();
            this.btnDest = new System.Windows.Forms.Button();
            this.lblDest = new System.Windows.Forms.Label();
            this.cbxIsCIDR = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(23, 112);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 0;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(104, 35);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(0, 13);
            this.lblSource.TabIndex = 1;
            // 
            // btnSource
            // 
            this.btnSource.Location = new System.Drawing.Point(23, 30);
            this.btnSource.Name = "btnSource";
            this.btnSource.Size = new System.Drawing.Size(75, 23);
            this.btnSource.TabIndex = 2;
            this.btnSource.Text = "Source";
            this.btnSource.UseVisualStyleBackColor = true;
            this.btnSource.Click += new System.EventHandler(this.btnSource_Click);
            // 
            // btnDest
            // 
            this.btnDest.Location = new System.Drawing.Point(23, 59);
            this.btnDest.Name = "btnDest";
            this.btnDest.Size = new System.Drawing.Size(75, 23);
            this.btnDest.TabIndex = 4;
            this.btnDest.Text = "Destination";
            this.btnDest.UseVisualStyleBackColor = true;
            this.btnDest.Click += new System.EventHandler(this.btnDest_Click);
            // 
            // lblDest
            // 
            this.lblDest.AutoSize = true;
            this.lblDest.Location = new System.Drawing.Point(104, 64);
            this.lblDest.Name = "lblDest";
            this.lblDest.Size = new System.Drawing.Size(0, 13);
            this.lblDest.TabIndex = 3;
            // 
            // cbxIsCIDR
            // 
            this.cbxIsCIDR.AutoSize = true;
            this.cbxIsCIDR.Location = new System.Drawing.Point(23, 7);
            this.cbxIsCIDR.Name = "cbxIsCIDR";
            this.cbxIsCIDR.Size = new System.Drawing.Size(63, 17);
            this.cbxIsCIDR.TabIndex = 5;
            this.cbxIsCIDR.Text = "Is CIDR";
            this.cbxIsCIDR.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 151);
            this.Controls.Add(this.cbxIsCIDR);
            this.Controls.Add(this.btnDest);
            this.Controls.Add(this.lblDest);
            this.Controls.Add(this.btnSource);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.btnProcess);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Button btnSource;
        private System.Windows.Forms.Button btnDest;
        private System.Windows.Forms.Label lblDest;
        private System.Windows.Forms.CheckBox cbxIsCIDR;
    }
}

