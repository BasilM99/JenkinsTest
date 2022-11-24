namespace AdFalconScriptGenerator
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
            this.txtApps = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOperators = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAccountId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPageNumber = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPageSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxSummaryBy = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtOrderColumn = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtOrderType = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCulture = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnGetSql = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.cbxLayout = new System.Windows.Forms.ComboBox();
            this.cbxSubReportType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cbxType = new System.Windows.Forms.ComboBox();
            this.rbReport = new System.Windows.Forms.RadioButton();
            this.rbChart = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.txtThreshold = new System.Windows.Forms.TextBox();
            this.txtIds = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ids";
            // 
            // txtApps
            // 
            this.txtApps.Location = new System.Drawing.Point(85, 48);
            this.txtApps.Name = "txtApps";
            this.txtApps.Size = new System.Drawing.Size(375, 20);
            this.txtApps.TabIndex = 1;
            this.txtApps.Text = "20806,21917,21412,16362,20402";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Filter By";
            // 
            // txtOperators
            // 
            this.txtOperators.Location = new System.Drawing.Point(191, 74);
            this.txtOperators.Name = "txtOperators";
            this.txtOperators.Size = new System.Drawing.Size(269, 20);
            this.txtOperators.TabIndex = 1;
            this.txtOperators.Text = "282,443,254,444";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "From Date";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Location = new System.Drawing.Point(85, 99);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(200, 20);
            this.dtpFromDate.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "To Date";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Location = new System.Drawing.Point(85, 125);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(200, 20);
            this.dtpToDate.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Account Id";
            // 
            // txtAccountId
            // 
            this.txtAccountId.Location = new System.Drawing.Point(85, 151);
            this.txtAccountId.Name = "txtAccountId";
            this.txtAccountId.Size = new System.Drawing.Size(90, 20);
            this.txtAccountId.TabIndex = 1;
            this.txtAccountId.Text = "20709";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Page";
            // 
            // txtPageNumber
            // 
            this.txtPageNumber.Location = new System.Drawing.Point(85, 183);
            this.txtPageNumber.Name = "txtPageNumber";
            this.txtPageNumber.Size = new System.Drawing.Size(90, 20);
            this.txtPageNumber.TabIndex = 1;
            this.txtPageNumber.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Page Size";
            // 
            // txtPageSize
            // 
            this.txtPageSize.Location = new System.Drawing.Point(85, 209);
            this.txtPageSize.Name = "txtPageSize";
            this.txtPageSize.Size = new System.Drawing.Size(90, 20);
            this.txtPageSize.TabIndex = 1;
            this.txtPageSize.Text = "10";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 238);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Summary By";
            // 
            // cbxSummaryBy
            // 
            this.cbxSummaryBy.FormattingEnabled = true;
            this.cbxSummaryBy.Items.AddRange(new object[] {
            "Hour",
            "Day",
            "Week",
            "Month"});
            this.cbxSummaryBy.Location = new System.Drawing.Point(85, 235);
            this.cbxSummaryBy.Name = "cbxSummaryBy";
            this.cbxSummaryBy.Size = new System.Drawing.Size(121, 21);
            this.cbxSummaryBy.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(249, 183);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Order Column";
            // 
            // txtOrderColumn
            // 
            this.txtOrderColumn.Location = new System.Drawing.Point(321, 180);
            this.txtOrderColumn.Name = "txtOrderColumn";
            this.txtOrderColumn.Size = new System.Drawing.Size(90, 20);
            this.txtOrderColumn.TabIndex = 1;
            this.txtOrderColumn.Text = "Date";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(249, 209);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Order Type";
            // 
            // txtOrderType
            // 
            this.txtOrderType.Location = new System.Drawing.Point(321, 206);
            this.txtOrderType.Name = "txtOrderType";
            this.txtOrderType.Size = new System.Drawing.Size(90, 20);
            this.txtOrderType.TabIndex = 1;
            this.txtOrderType.Text = "asc";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(249, 235);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Layout";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 265);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(42, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Culture";
            // 
            // txtCulture
            // 
            this.txtCulture.Location = new System.Drawing.Point(85, 262);
            this.txtCulture.Name = "txtCulture";
            this.txtCulture.Size = new System.Drawing.Size(90, 20);
            this.txtCulture.TabIndex = 1;
            this.txtCulture.Text = "en-US";
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtResult.Location = new System.Drawing.Point(0, 399);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(577, 211);
            this.txtResult.TabIndex = 5;
            // 
            // btnGetSql
            // 
            this.btnGetSql.Location = new System.Drawing.Point(247, 260);
            this.btnGetSql.Name = "btnGetSql";
            this.btnGetSql.Size = new System.Drawing.Size(75, 23);
            this.btnGetSql.TabIndex = 6;
            this.btnGetSql.Text = "Get SQL";
            this.btnGetSql.UseVisualStyleBackColor = true;
            this.btnGetSql.Click += new System.EventHandler(this.btnGetSql_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(328, 260);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(55, 23);
            this.btnCopy.TabIndex = 6;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // cbxLayout
            // 
            this.cbxLayout.FormattingEnabled = true;
            this.cbxLayout.Items.AddRange(new object[] {
            "Summary",
            "Detailed"});
            this.cbxLayout.Location = new System.Drawing.Point(321, 231);
            this.cbxLayout.Name = "cbxLayout";
            this.cbxLayout.Size = new System.Drawing.Size(90, 21);
            this.cbxLayout.TabIndex = 7;
            // 
            // cbxSubReportType
            // 
            this.cbxSubReportType.FormattingEnabled = true;
            this.cbxSubReportType.Items.AddRange(new object[] {
            "Hour",
            "Operator",
            "Platform",
            "Manufacturer",
            "GeoLocation"});
            this.cbxSubReportType.Location = new System.Drawing.Point(85, 72);
            this.cbxSubReportType.Name = "cbxSubReportType";
            this.cbxSubReportType.Size = new System.Drawing.Size(100, 21);
            this.cbxSubReportType.TabIndex = 9;
            this.cbxSubReportType.SelectedIndexChanged += new System.EventHandler(this.cbxSubReportType_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Type";
            // 
            // cbxType
            // 
            this.cbxType.FormattingEnabled = true;
            this.cbxType.Items.AddRange(new object[] {
            "App/Site",
            "Campaign",
            "AdGroup",
            "Ad"});
            this.cbxType.Location = new System.Drawing.Point(85, 21);
            this.cbxType.Name = "cbxType";
            this.cbxType.Size = new System.Drawing.Size(100, 21);
            this.cbxType.TabIndex = 9;
            this.cbxType.SelectedIndexChanged += new System.EventHandler(this.cbxType_SelectedIndexChanged);
            // 
            // rbReport
            // 
            this.rbReport.AutoSize = true;
            this.rbReport.Checked = true;
            this.rbReport.Location = new System.Drawing.Point(213, 12);
            this.rbReport.Name = "rbReport";
            this.rbReport.Size = new System.Drawing.Size(58, 17);
            this.rbReport.TabIndex = 10;
            this.rbReport.TabStop = true;
            this.rbReport.Text = "Report";
            this.rbReport.UseVisualStyleBackColor = true;
            // 
            // rbChart
            // 
            this.rbChart.AutoSize = true;
            this.rbChart.Location = new System.Drawing.Point(270, 12);
            this.rbChart.Name = "rbChart";
            this.rbChart.Size = new System.Drawing.Size(52, 17);
            this.rbChart.TabIndex = 11;
            this.rbChart.Text = "Chart";
            this.rbChart.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(179, 156);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(54, 13);
            this.label16.TabIndex = 12;
            this.label16.Text = "Threshold";
            // 
            // txtThreshold
            // 
            this.txtThreshold.Location = new System.Drawing.Point(232, 152);
            this.txtThreshold.Name = "txtThreshold";
            this.txtThreshold.Size = new System.Drawing.Size(39, 20);
            this.txtThreshold.TabIndex = 13;
            this.txtThreshold.Text = "25";
            // 
            // txtIds
            // 
            this.txtIds.Location = new System.Drawing.Point(85, 289);
            this.txtIds.Multiline = true;
            this.txtIds.Name = "txtIds";
            this.txtIds.Size = new System.Drawing.Size(480, 104);
            this.txtIds.TabIndex = 15;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(13, 292);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(22, 13);
            this.label17.TabIndex = 14;
            this.label17.Text = "Ids";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 610);
            this.Controls.Add(this.txtIds);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtThreshold);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.rbChart);
            this.Controls.Add(this.rbReport);
            this.Controls.Add(this.cbxType);
            this.Controls.Add(this.cbxSubReportType);
            this.Controls.Add(this.cbxLayout);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnGetSql);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.cbxSummaryBy);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtCulture);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtOrderType);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtOrderColumn);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtPageSize);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtPageNumber);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtAccountId);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtOperators);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtApps);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtApps;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOperators;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAccountId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPageNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPageSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbxSummaryBy;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtOrderColumn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtOrderType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCulture;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnGetSql;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.ComboBox cbxLayout;
        private System.Windows.Forms.ComboBox cbxSubReportType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbxType;
        private System.Windows.Forms.RadioButton rbReport;
        private System.Windows.Forms.RadioButton rbChart;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtThreshold;
        private System.Windows.Forms.TextBox txtIds;
        private System.Windows.Forms.Label label17;
    }
}