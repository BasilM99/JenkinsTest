namespace InsertIPsBatch
{
    partial class main
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
            this.btnGenerateTargetingScript = new System.Windows.Forms.Button();
            this.btnMatch = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGenerateTargetingScript
            // 
            this.btnGenerateTargetingScript.Location = new System.Drawing.Point(12, 21);
            this.btnGenerateTargetingScript.Name = "btnGenerateTargetingScript";
            this.btnGenerateTargetingScript.Size = new System.Drawing.Size(157, 23);
            this.btnGenerateTargetingScript.TabIndex = 0;
            this.btnGenerateTargetingScript.Text = "generate targeting script";
            this.btnGenerateTargetingScript.UseVisualStyleBackColor = true;
            this.btnGenerateTargetingScript.Click += new System.EventHandler(this.btnGenerateTargetingScript_Click);
            // 
            // btnMatch
            // 
            this.btnMatch.Location = new System.Drawing.Point(12, 50);
            this.btnMatch.Name = "btnMatch";
            this.btnMatch.Size = new System.Drawing.Size(157, 23);
            this.btnMatch.TabIndex = 1;
            this.btnMatch.Text = "Marge";
            this.btnMatch.UseVisualStyleBackColor = true;
            this.btnMatch.Click += new System.EventHandler(this.btnMatch_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Match";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnMatch_Click);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 126);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnMatch);
            this.Controls.Add(this.btnGenerateTargetingScript);
            this.Name = "main";
            this.Text = "main";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGenerateTargetingScript;
        private System.Windows.Forms.Button btnMatch;
        private System.Windows.Forms.Button button1;
    }
}