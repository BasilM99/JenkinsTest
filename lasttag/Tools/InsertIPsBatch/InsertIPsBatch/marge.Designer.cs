namespace InsertIPsBatch
{
    partial class marge
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
            this.btnDest = new System.Windows.Forms.Button();
            this.lblDest = new System.Windows.Forms.Label();
            this.btnSource = new System.Windows.Forms.Button();
            this.lblSource = new System.Windows.Forms.Label();
            this.btnSource2 = new System.Windows.Forms.Button();
            this.lblsource2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(12, 94);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 5;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnDest
            // 
            this.btnDest.Location = new System.Drawing.Point(12, 65);
            this.btnDest.Name = "btnDest";
            this.btnDest.Size = new System.Drawing.Size(75, 23);
            this.btnDest.TabIndex = 9;
            this.btnDest.Text = "Destination";
            this.btnDest.UseVisualStyleBackColor = true;
            this.btnDest.Click += new System.EventHandler(this.btnDest_Click);
            // 
            // lblDest
            // 
            this.lblDest.AutoSize = true;
            this.lblDest.Location = new System.Drawing.Point(93, 70);
            this.lblDest.Name = "lblDest";
            this.lblDest.Size = new System.Drawing.Size(0, 13);
            this.lblDest.TabIndex = 8;
            // 
            // btnSource
            // 
            this.btnSource.Location = new System.Drawing.Point(12, 12);
            this.btnSource.Name = "btnSource";
            this.btnSource.Size = new System.Drawing.Size(75, 23);
            this.btnSource.TabIndex = 7;
            this.btnSource.Text = "Source";
            this.btnSource.UseVisualStyleBackColor = true;
            this.btnSource.Click += new System.EventHandler(this.btnSource_Click);
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(93, 17);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(0, 13);
            this.lblSource.TabIndex = 6;
            // 
            // btnSource2
            // 
            this.btnSource2.Location = new System.Drawing.Point(12, 36);
            this.btnSource2.Name = "btnSource2";
            this.btnSource2.Size = new System.Drawing.Size(75, 23);
            this.btnSource2.TabIndex = 11;
            this.btnSource2.Text = "Source";
            this.btnSource2.UseVisualStyleBackColor = true;
            this.btnSource2.Click += new System.EventHandler(this.btnSource2_Click);
            // 
            // lblsource2
            // 
            this.lblsource2.AutoSize = true;
            this.lblsource2.Location = new System.Drawing.Point(93, 41);
            this.lblsource2.Name = "lblsource2";
            this.lblsource2.Size = new System.Drawing.Size(0, 13);
            this.lblsource2.TabIndex = 10;
            // 
            // marge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 126);
            this.Controls.Add(this.btnSource2);
            this.Controls.Add(this.lblsource2);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnDest);
            this.Controls.Add(this.lblDest);
            this.Controls.Add(this.btnSource);
            this.Controls.Add(this.lblSource);
            this.Name = "marge";
            this.Text = "marge";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnDest;
        private System.Windows.Forms.Label lblDest;
        private System.Windows.Forms.Button btnSource;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Button btnSource2;
        private System.Windows.Forms.Label lblsource2;
    }
}