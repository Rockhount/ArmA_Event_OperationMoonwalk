namespace RPT2Diagramm2
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ScanFileButton = new System.Windows.Forms.Button();
            this.FileSelectionText = new System.Windows.Forms.Label();
            this.FileSelectionBox = new System.Windows.Forms.ComboBox();
            this.FirstDataTypeText = new System.Windows.Forms.Label();
            this.FirstDataTypeSelectionBox = new System.Windows.Forms.ComboBox();
            this.SecondDataTypeSelectionBox = new System.Windows.Forms.ComboBox();
            this.SecondDataTypeText = new System.Windows.Forms.Label();
            this.MainChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.FileSelectionDeleteBox = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ChartImageWidthControl = new System.Windows.Forms.NumericUpDown();
            this.ChartImageHeightControl = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.SaveImageButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MainChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartImageWidthControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartImageHeightControl)).BeginInit();
            this.SuspendLayout();
            // 
            // ScanFileButton
            // 
            this.ScanFileButton.Location = new System.Drawing.Point(13, 14);
            this.ScanFileButton.Name = "ScanFileButton";
            this.ScanFileButton.Size = new System.Drawing.Size(117, 23);
            this.ScanFileButton.TabIndex = 0;
            this.ScanFileButton.Text = "Datei scannen";
            this.ScanFileButton.UseVisualStyleBackColor = true;
            this.ScanFileButton.Click += new System.EventHandler(this.ScanFileButton_Click);
            // 
            // FileSelectionText
            // 
            this.FileSelectionText.AutoSize = true;
            this.FileSelectionText.Location = new System.Drawing.Point(147, 18);
            this.FileSelectionText.Name = "FileSelectionText";
            this.FileSelectionText.Size = new System.Drawing.Size(74, 13);
            this.FileSelectionText.TabIndex = 1;
            this.FileSelectionText.Text = "Dateiauswahl:";
            // 
            // FileSelectionBox
            // 
            this.FileSelectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FileSelectionBox.Enabled = false;
            this.FileSelectionBox.FormattingEnabled = true;
            this.FileSelectionBox.Location = new System.Drawing.Point(227, 15);
            this.FileSelectionBox.Name = "FileSelectionBox";
            this.FileSelectionBox.Size = new System.Drawing.Size(178, 21);
            this.FileSelectionBox.TabIndex = 2;
            this.FileSelectionBox.SelectionChangeCommitted += new System.EventHandler(this.FileSelectionBox_SelectionChangeCommitted);
            // 
            // FirstDataTypeText
            // 
            this.FirstDataTypeText.AutoSize = true;
            this.FirstDataTypeText.Location = new System.Drawing.Point(439, 18);
            this.FirstDataTypeText.Name = "FirstDataTypeText";
            this.FirstDataTypeText.Size = new System.Drawing.Size(69, 13);
            this.FirstDataTypeText.TabIndex = 3;
            this.FirstDataTypeText.Text = "Datentyp #1:";
            // 
            // FirstDataTypeSelectionBox
            // 
            this.FirstDataTypeSelectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FirstDataTypeSelectionBox.Enabled = false;
            this.FirstDataTypeSelectionBox.FormattingEnabled = true;
            this.FirstDataTypeSelectionBox.Items.AddRange(new object[] {
            "Lokale Einheiten (Keine Zivilisten)"});
            this.FirstDataTypeSelectionBox.Location = new System.Drawing.Point(514, 15);
            this.FirstDataTypeSelectionBox.Name = "FirstDataTypeSelectionBox";
            this.FirstDataTypeSelectionBox.Size = new System.Drawing.Size(240, 21);
            this.FirstDataTypeSelectionBox.TabIndex = 4;
            this.FirstDataTypeSelectionBox.SelectionChangeCommitted += new System.EventHandler(this.FileSelectionBox_SelectionChangeCommitted);
            // 
            // SecondDataTypeSelectionBox
            // 
            this.SecondDataTypeSelectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SecondDataTypeSelectionBox.Enabled = false;
            this.SecondDataTypeSelectionBox.FormattingEnabled = true;
            this.SecondDataTypeSelectionBox.Location = new System.Drawing.Point(835, 16);
            this.SecondDataTypeSelectionBox.Name = "SecondDataTypeSelectionBox";
            this.SecondDataTypeSelectionBox.Size = new System.Drawing.Size(240, 21);
            this.SecondDataTypeSelectionBox.TabIndex = 6;
            this.SecondDataTypeSelectionBox.SelectionChangeCommitted += new System.EventHandler(this.FileSelectionBox_SelectionChangeCommitted);
            // 
            // SecondDataTypeText
            // 
            this.SecondDataTypeText.AutoSize = true;
            this.SecondDataTypeText.Location = new System.Drawing.Point(760, 19);
            this.SecondDataTypeText.Name = "SecondDataTypeText";
            this.SecondDataTypeText.Size = new System.Drawing.Size(69, 13);
            this.SecondDataTypeText.TabIndex = 5;
            this.SecondDataTypeText.Text = "Datentyp #2:";
            // 
            // MainChart
            // 
            this.MainChart.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.Text;
            chartArea1.Name = "ChartArea1";
            this.MainChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.MainChart.Legends.Add(legend1);
            this.MainChart.Location = new System.Drawing.Point(13, 42);
            this.MainChart.Name = "MainChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.MainChart.Series.Add(series1);
            this.MainChart.Size = new System.Drawing.Size(1353, 474);
            this.MainChart.TabIndex = 7;
            // 
            // FileSelectionDeleteBox
            // 
            this.FileSelectionDeleteBox.Enabled = false;
            this.FileSelectionDeleteBox.Location = new System.Drawing.Point(411, 14);
            this.FileSelectionDeleteBox.Name = "FileSelectionDeleteBox";
            this.FileSelectionDeleteBox.Size = new System.Drawing.Size(22, 23);
            this.FileSelectionDeleteBox.TabIndex = 8;
            this.FileSelectionDeleteBox.Text = "-";
            this.FileSelectionDeleteBox.UseVisualStyleBackColor = true;
            this.FileSelectionDeleteBox.Click += new System.EventHandler(this.FileSelectionDeleteBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1081, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "X:";
            // 
            // ChartImageWidthControl
            // 
            this.ChartImageWidthControl.Location = new System.Drawing.Point(1101, 16);
            this.ChartImageWidthControl.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ChartImageWidthControl.Minimum = new decimal(new int[] {
            1353,
            0,
            0,
            0});
            this.ChartImageWidthControl.Name = "ChartImageWidthControl";
            this.ChartImageWidthControl.Size = new System.Drawing.Size(59, 20);
            this.ChartImageWidthControl.TabIndex = 10;
            this.ChartImageWidthControl.Value = new decimal(new int[] {
            1353,
            0,
            0,
            0});
            this.ChartImageWidthControl.ValueChanged += new System.EventHandler(this.ChartImageWidthControl_ValueChanged);
            // 
            // ChartImageHeightControl
            // 
            this.ChartImageHeightControl.Location = new System.Drawing.Point(1191, 16);
            this.ChartImageHeightControl.Maximum = new decimal(new int[] {
            4218,
            0,
            0,
            0});
            this.ChartImageHeightControl.Minimum = new decimal(new int[] {
            474,
            0,
            0,
            0});
            this.ChartImageHeightControl.Name = "ChartImageHeightControl";
            this.ChartImageHeightControl.Size = new System.Drawing.Size(59, 20);
            this.ChartImageHeightControl.TabIndex = 12;
            this.ChartImageHeightControl.Value = new decimal(new int[] {
            474,
            0,
            0,
            0});
            this.ChartImageHeightControl.ValueChanged += new System.EventHandler(this.ChartImageHeightControl_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1171, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Y:";
            // 
            // SaveImageButton
            // 
            this.SaveImageButton.Enabled = false;
            this.SaveImageButton.Location = new System.Drawing.Point(1256, 14);
            this.SaveImageButton.Name = "SaveImageButton";
            this.SaveImageButton.Size = new System.Drawing.Size(110, 23);
            this.SaveImageButton.TabIndex = 13;
            this.SaveImageButton.Text = "Als Bild spreichern";
            this.SaveImageButton.UseVisualStyleBackColor = true;
            this.SaveImageButton.Click += new System.EventHandler(this.SaveImageButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1380, 528);
            this.Controls.Add(this.SaveImageButton);
            this.Controls.Add(this.ChartImageHeightControl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ChartImageWidthControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileSelectionDeleteBox);
            this.Controls.Add(this.MainChart);
            this.Controls.Add(this.SecondDataTypeSelectionBox);
            this.Controls.Add(this.SecondDataTypeText);
            this.Controls.Add(this.FirstDataTypeSelectionBox);
            this.Controls.Add(this.FirstDataTypeText);
            this.Controls.Add(this.FileSelectionBox);
            this.Controls.Add(this.FileSelectionText);
            this.Controls.Add(this.ScanFileButton);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1396, 567);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "RPT zu Diagramm";
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.MainChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartImageWidthControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartImageHeightControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ScanFileButton;
        private System.Windows.Forms.Label FileSelectionText;
        private System.Windows.Forms.ComboBox FileSelectionBox;
        private System.Windows.Forms.Label FirstDataTypeText;
        private System.Windows.Forms.ComboBox FirstDataTypeSelectionBox;
        private System.Windows.Forms.ComboBox SecondDataTypeSelectionBox;
        private System.Windows.Forms.Label SecondDataTypeText;
        private System.Windows.Forms.DataVisualization.Charting.Chart MainChart;
        private System.Windows.Forms.Button FileSelectionDeleteBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ChartImageWidthControl;
        private System.Windows.Forms.NumericUpDown ChartImageHeightControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SaveImageButton;
    }
}

