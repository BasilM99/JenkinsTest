namespace AdCreativeContentFormatter
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.RichTextBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.gbxAdType = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.lblSample = new System.Windows.Forms.Label();
            this.cbxSampleType = new System.Windows.Forms.ComboBox();
            this.gbxAdType.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Content";
            // 
            // txtContent
            // 
            this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent.Location = new System.Drawing.Point(57, 76);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(557, 213);
            this.txtContent.TabIndex = 1;
            this.txtContent.Text = "";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(476, 16);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 2;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // gbxAdType
            // 
            this.gbxAdType.Controls.Add(this.cbxSampleType);
            this.gbxAdType.Controls.Add(this.lblSample);
            this.gbxAdType.Controls.Add(this.btnProcess);
            this.gbxAdType.Location = new System.Drawing.Point(57, 13);
            this.gbxAdType.Name = "gbxAdType";
            this.gbxAdType.Size = new System.Drawing.Size(557, 47);
            this.gbxAdType.TabIndex = 3;
            this.gbxAdType.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Result";
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(57, 295);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(557, 213);
            this.txtResult.TabIndex = 1;
            this.txtResult.Text = "";
            // 
            // lblSample
            // 
            this.lblSample.AutoSize = true;
            this.lblSample.Location = new System.Drawing.Point(21, 20);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(41, 13);
            this.lblSample.TabIndex = 3;
            this.lblSample.Text = "Sample";
            // 
            // cbxSampleType
            // 
            this.cbxSampleType.FormattingEnabled = true;
            this.cbxSampleType.Items.AddRange(new object[] {
            "Custom",
            "Celtra Mraid",
            "Celtra Web",
            "Celtra Mraid 3",
            "Celtra Web 3",
            "Double Click - JavaScript",
            "Double Click - Simple HTML",
            "Double Click - JSONP",
            "Crisp"});
            this.cbxSampleType.Location = new System.Drawing.Point(68, 16);
            this.cbxSampleType.Name = "cbxSampleType";
            this.cbxSampleType.Size = new System.Drawing.Size(172, 21);
            this.cbxSampleType.TabIndex = 4;
            this.cbxSampleType.SelectedIndexChanged += new System.EventHandler(this.cbxSampleType_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 537);
            this.Controls.Add(this.gbxAdType);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gbxAdType.ResumeLayout(false);
            this.gbxAdType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txtContent;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.GroupBox gbxAdType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.ComboBox cbxSampleType;
        private System.Windows.Forms.Label lblSample;
    }
}

