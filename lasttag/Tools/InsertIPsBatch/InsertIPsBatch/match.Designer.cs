namespace InsertIPsBatch
{
    partial class Match
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
            this.ofdSource = new System.Windows.Forms.OpenFileDialog();
            this.sfdFound = new System.Windows.Forms.SaveFileDialog();
            this.sfdNotFound = new System.Windows.Forms.SaveFileDialog();
            this.btnSource = new System.Windows.Forms.Button();
            this.btnFound = new System.Windows.Forms.Button();
            this.btnNotFound = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblFound = new System.Windows.Forms.Label();
            this.lblNotFound = new System.Windows.Forms.Label();
            this.btnMainSource = new System.Windows.Forms.Button();
            this.lblMainSource = new System.Windows.Forms.Label();
            this.ofdMainSource = new System.Windows.Forms.OpenFileDialog();
            this.pgbProcess = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // ofdSource
            // 
            this.ofdSource.FileName = "Source.csv";
            // 
            // sfdFound
            // 
            this.sfdFound.FileName = "Found.csv";
            // 
            // sfdNotFound
            // 
            this.sfdNotFound.FileName = "NotFound.csv";
            // 
            // btnSource
            // 
            this.btnSource.Location = new System.Drawing.Point(12, 41);
            this.btnSource.Name = "btnSource";
            this.btnSource.Size = new System.Drawing.Size(75, 23);
            this.btnSource.TabIndex = 0;
            this.btnSource.Text = "Source";
            this.btnSource.UseVisualStyleBackColor = true;
            this.btnSource.Click += new System.EventHandler(this.btnSource_Click);
            // 
            // btnFound
            // 
            this.btnFound.Location = new System.Drawing.Point(12, 71);
            this.btnFound.Name = "btnFound";
            this.btnFound.Size = new System.Drawing.Size(75, 23);
            this.btnFound.TabIndex = 1;
            this.btnFound.Text = "Found";
            this.btnFound.UseVisualStyleBackColor = true;
            this.btnFound.Click += new System.EventHandler(this.btnFound_Click);
            // 
            // btnNotFound
            // 
            this.btnNotFound.Location = new System.Drawing.Point(12, 101);
            this.btnNotFound.Name = "btnNotFound";
            this.btnNotFound.Size = new System.Drawing.Size(75, 23);
            this.btnNotFound.TabIndex = 2;
            this.btnNotFound.Text = "Not Found";
            this.btnNotFound.UseVisualStyleBackColor = true;
            this.btnNotFound.Click += new System.EventHandler(this.btnNotFound_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(12, 147);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 3;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(91, 46);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(0, 13);
            this.lblSource.TabIndex = 4;
            // 
            // lblFound
            // 
            this.lblFound.AutoSize = true;
            this.lblFound.Location = new System.Drawing.Point(91, 76);
            this.lblFound.Name = "lblFound";
            this.lblFound.Size = new System.Drawing.Size(0, 13);
            this.lblFound.TabIndex = 5;
            // 
            // lblNotFound
            // 
            this.lblNotFound.AutoSize = true;
            this.lblNotFound.Location = new System.Drawing.Point(91, 106);
            this.lblNotFound.Name = "lblNotFound";
            this.lblNotFound.Size = new System.Drawing.Size(0, 13);
            this.lblNotFound.TabIndex = 6;
            // 
            // btnMainSource
            // 
            this.btnMainSource.Location = new System.Drawing.Point(12, 12);
            this.btnMainSource.Name = "btnMainSource";
            this.btnMainSource.Size = new System.Drawing.Size(75, 23);
            this.btnMainSource.TabIndex = 0;
            this.btnMainSource.Text = "Main Source";
            this.btnMainSource.UseVisualStyleBackColor = true;
            this.btnMainSource.Click += new System.EventHandler(this.btnMainSource_Click);
            // 
            // lblMainSource
            // 
            this.lblMainSource.AutoSize = true;
            this.lblMainSource.Location = new System.Drawing.Point(91, 17);
            this.lblMainSource.Name = "lblMainSource";
            this.lblMainSource.Size = new System.Drawing.Size(0, 13);
            this.lblMainSource.TabIndex = 4;
            // 
            // ofdMainSource
            // 
            this.ofdMainSource.FileName = "MainSource.csv";
            // 
            // pgbProcess
            // 
            this.pgbProcess.Location = new System.Drawing.Point(13, 131);
            this.pgbProcess.Name = "pgbProcess";
            this.pgbProcess.Size = new System.Drawing.Size(595, 10);
            this.pgbProcess.TabIndex = 7;
            // 
            // Match
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 249);
            this.Controls.Add(this.pgbProcess);
            this.Controls.Add(this.lblNotFound);
            this.Controls.Add(this.lblFound);
            this.Controls.Add(this.lblMainSource);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnNotFound);
            this.Controls.Add(this.btnFound);
            this.Controls.Add(this.btnMainSource);
            this.Controls.Add(this.btnSource);
            this.Name = "Match";
            this.Text = "Match";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdSource;
        private System.Windows.Forms.SaveFileDialog sfdFound;
        private System.Windows.Forms.SaveFileDialog sfdNotFound;
        private System.Windows.Forms.Button btnSource;
        private System.Windows.Forms.Button btnFound;
        private System.Windows.Forms.Button btnNotFound;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblFound;
        private System.Windows.Forms.Label lblNotFound;
        private System.Windows.Forms.Button btnMainSource;
        private System.Windows.Forms.Label lblMainSource;
        private System.Windows.Forms.OpenFileDialog ofdMainSource;
        private System.Windows.Forms.ProgressBar pgbProcess;
    }
}