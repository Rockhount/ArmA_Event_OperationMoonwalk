using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RPT2Diagramm2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.DataFiles = new HashSet<DataFile>();
            this.ChangeCount = 0;
            this.LastFileDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            InitializeComponent();
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------------
        private int ChangeCount { get; set; }
        private string LastFileDir { get; set; }
        private HashSet<DataFile> DataFiles { get; set; }
//--------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            MainChart.Size = new Size(this.Width - 41, this.Height - 92);
            ChartImageWidthControl.Value = this.Width - 41;
            ChartImageHeightControl.Value = this.Height - 92;
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ScanFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Wählen Sie die eine RPT oder Log aus";
                openFileDialog.Filter = "Log Files|*.rpt;*.log";
                openFileDialog.InitialDirectory = LastFileDir;
                var objResult = openFileDialog.ShowDialog(this);
                if (objResult == DialogResult.OK)
                {
                    FileSelectionBox.Enabled = true;
                    FileSelectionDeleteBox.Enabled = true;
                    FirstDataTypeSelectionBox.Enabled = true;
                    SecondDataTypeSelectionBox.Enabled = true;
                    FirstDataTypeSelectionBox.Items.Clear();
                    SecondDataTypeSelectionBox.Items.Clear();
                    SaveImageButton.Enabled = true;
                    LastFileDir = Path.GetDirectoryName(openFileDialog.FileName);
                    var CurDataFile =  new DataFile().Construct(openFileDialog.FileName);
                    DataFiles.Add(CurDataFile);
                    FileSelectionBox.Items.Add(CurDataFile.Name);
                    FileSelectionBox.SelectedItem = CurDataFile.Name;
                    FileSelectionBox.Tag = CurDataFile;
                    foreach (var CurDataSet in CurDataFile.DataSets)
                    {
                        FirstDataTypeSelectionBox.Items.Add(CurDataSet.Name);
                        SecondDataTypeSelectionBox.Items.Add(CurDataSet.Name);
                    }
                    FirstDataTypeSelectionBox.SelectedIndex = 0;
                    SecondDataTypeSelectionBox.SelectedIndex = 1;
                    FileSelectionBox_SelectionChangeCommitted(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------------
        private void FileSelectionDeleteBox_Click(object sender, EventArgs e)
        {
            DataFiles.RemoveWhere(d => d.Name == FileSelectionBox.SelectedItem.ToString());
            FileSelectionBox.Items.Remove(FileSelectionBox.SelectedItem.ToString());
            if (FileSelectionBox.Items.Count > 0)
            {
                FileSelectionBox.SelectedItem = FileSelectionBox.Items[FileSelectionBox.Items.Count - 1];
                FileSelectionBox_SelectionChangeCommitted(null, null);
            }
            else
            {
                FileSelectionBox.Enabled = false;
                FileSelectionDeleteBox.Enabled = false;
                FirstDataTypeSelectionBox.Enabled = false;
                SecondDataTypeSelectionBox.Enabled = false;
                SaveImageButton.Enabled = false;
            }
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------------
        private void FileSelectionBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var CurDataFile = DataFiles.Where(d => d.Name == FileSelectionBox.SelectedItem.ToString()).First();
            var DataFileIsALog = CurDataFile.IsALog;
            if (!((DataFile)FileSelectionBox.Tag).Name.Equals(CurDataFile.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                FileSelectionBox.Tag = CurDataFile;
                FirstDataTypeSelectionBox.Items.Clear();
                SecondDataTypeSelectionBox.Items.Clear();
                foreach (var CurDataSet in CurDataFile.DataSets)
                {
                    FirstDataTypeSelectionBox.Items.Add(CurDataSet.Name);
                    SecondDataTypeSelectionBox.Items.Add(CurDataSet.Name);
                }
                FirstDataTypeSelectionBox.SelectedIndex = 0;
                SecondDataTypeSelectionBox.SelectedIndex = 1;
            }
            if (DataFileIsALog || !FirstDataTypeSelectionBox.SelectedItem.ToString().Equals(SecondDataTypeSelectionBox.SelectedItem.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                // Data arrays.
                var FirstData = ((DataFile)FileSelectionBox.Tag).GetDataSet(FirstDataTypeSelectionBox.SelectedItem.ToString());
                DataSet SecondData = null;
                if (!DataFileIsALog)
                {
                    SecondData = ((DataFile)FileSelectionBox.Tag).GetDataSet(SecondDataTypeSelectionBox.SelectedItem.ToString());
                }
                // Set palette.
                MainChart.Palette = ChartColorPalette.Bright;
                // Set title.
                MainChart.Titles.Clear();
                MainChart.Titles.Add("Performance");
                if (DataFileIsALog || (FirstData.AVGValue > SecondData.AVGValue))
                {
                    if (DataFileIsALog)
                    {
                        MainChart.ChartAreas[0].AxisY.Maximum = FirstData.AVGValue * 2;
                    }
                    else
                    {
                        MainChart.ChartAreas[0].AxisY.Maximum = FirstData.MaxValue;
                    }
                }
                else
                {
                    if (DataFileIsALog)
                    {
                        MainChart.ChartAreas[0].AxisY.Maximum = SecondData.AVGValue * 2;
                    }
                    else
                    {
                        MainChart.ChartAreas[0].AxisY.Maximum = SecondData.MaxValue;
                    }
                }
                MainChart.ChartAreas[0].AxisY.Minimum = 0;
                MainChart.Series.Clear();
                var CurSeries = MainChart.Series.Add(FirstDataTypeSelectionBox.SelectedItem.ToString() + ": AVG~" + FirstData.AVGValue.ToString());
                for (int i2 = 0; i2 < FirstData.DataValues.Count; i2++)
                {
                    CurSeries.Points.Add(FirstData.DataValues[i2]);
                }
                if (!DataFileIsALog)
                {
                    CurSeries = MainChart.Series.Add(SecondDataTypeSelectionBox.SelectedItem.ToString() + ": AVG~" + SecondData.AVGValue.ToString());
                    for (int i2 = 0; i2 < SecondData.DataValues.Count; i2++)
                    {
                        CurSeries.Points.Add(SecondData.DataValues[i2]);
                    }
                }
                SaveImageButton.Enabled = true;
            }
            else
            {
                SaveImageButton.Enabled = false;
            }
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------------
        private void SaveImageButton_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Wählen Sie ein Speicherort aus";
            saveFileDialog.Filter = "PNG File|*.png";
            saveFileDialog.InitialDirectory = LastFileDir;
            var objResult = saveFileDialog.ShowDialog(this);
            if (objResult == DialogResult.OK)
            {
                var DataFileIsALog = ((DataFile)FileSelectionBox.Tag).IsALog;
                if (DataFileIsALog || !FirstDataTypeSelectionBox.SelectedItem.ToString().Equals(SecondDataTypeSelectionBox.SelectedItem.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    if ((((int)ChartImageWidthControl.Value) != MainChart.Size.Width) || (((int)ChartImageHeightControl.Value) != MainChart.Size.Height))
                    {
                        var VirtualChart = new Chart();
                        VirtualChart.Visible = false;
                        VirtualChart.Width = (int)ChartImageWidthControl.Value;
                        VirtualChart.Height = (int)ChartImageHeightControl.Value;
                        VirtualChart.Palette = ChartColorPalette.Bright;
                        VirtualChart.ChartAreas.Add(new ChartArea());
                        VirtualChart.Titles.Add("Performance");
                        VirtualChart.AntiAliasing = AntiAliasingStyles.Text;
                        VirtualChart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", VirtualChart.Width / 150, FontStyle.Regular);
                        VirtualChart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", VirtualChart.Width / 150, FontStyle.Regular);
                        VirtualChart.Titles[0].Font = new Font("Arial", VirtualChart.Width / 150, FontStyle.Regular);
                        var FirstData = ((DataFile)FileSelectionBox.Tag).GetDataSet(FirstDataTypeSelectionBox.SelectedItem.ToString());
                        DataSet SecondData = null;
                        if (!DataFileIsALog)
                        {
                            SecondData = ((DataFile)FileSelectionBox.Tag).GetDataSet(SecondDataTypeSelectionBox.SelectedItem.ToString());
                        }
                        if (DataFileIsALog || (FirstData.AVGValue > SecondData.AVGValue))
                        {
                            if (DataFileIsALog)
                            {
                                VirtualChart.ChartAreas[0].AxisY.Maximum = FirstData.AVGValue * 2;
                            }
                            else
                            {
                                VirtualChart.ChartAreas[0].AxisY.Maximum = FirstData.MaxValue;
                            }
                        }
                        else
                        {
                            if (DataFileIsALog)
                            {
                                VirtualChart.ChartAreas[0].AxisY.Maximum = SecondData.AVGValue * 2;
                            }
                            else
                            {
                                VirtualChart.ChartAreas[0].AxisY.Maximum = SecondData.MaxValue;
                            }
                        }
                        VirtualChart.ChartAreas[0].AxisY.Minimum = 0;
                        VirtualChart.Series.Clear();
                        VirtualChart.Legends.Clear();
                        var CurSeries = VirtualChart.Series.Add(FirstDataTypeSelectionBox.SelectedItem.ToString() + ": AVG~" + FirstData.AVGValue.ToString());
                        VirtualChart.Legends.Add(new Legend("Legend1"));
                        VirtualChart.Legends["Legend1"].Font = new Font("Arial", VirtualChart.Width / 150, FontStyle.Regular);
                        CurSeries.Legend = "Legend1";
                        CurSeries.IsVisibleInLegend = true;
                        for (int i2 = 0; i2 < FirstData.DataValues.Count; i2++)
                        {
                            CurSeries.Points.Add(FirstData.DataValues[i2]);
                        }
                        if (!DataFileIsALog)
                        {
                            CurSeries = VirtualChart.Series.Add(SecondDataTypeSelectionBox.SelectedItem.ToString() + ": AVG~" + SecondData.AVGValue.ToString());
                            for (int i2 = 0; i2 < SecondData.DataValues.Count; i2++)
                            {
                                CurSeries.Points.Add(SecondData.DataValues[i2]);
                            }
                        }
                        VirtualChart.SaveImage(saveFileDialog.FileName, ChartImageFormat.Png);
                    }
                    else
                    {
                        MainChart.SaveImage(saveFileDialog.FileName, ChartImageFormat.Png);
                    }
                    MessageBox.Show("Bild wurde gespeichert");
                }
            }
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------------
        protected override void WndProc(ref Message m)
        {
            FormWindowState previousWindowState = this.WindowState;
            base.WndProc(ref m);
            FormWindowState currentWindowState = this.WindowState;
            ChangeCount++;
            if ((previousWindowState != currentWindowState) && ((currentWindowState == FormWindowState.Maximized) || (currentWindowState == FormWindowState.Normal)))
            {
                if (ChangeCount > 2)
                {
                    ChangeCount = 0;
                    Form1_ResizeEnd(null, null);
                }
            }
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ChartImageWidthControl_ValueChanged(object sender, EventArgs e)
        {
            if (ChartImageWidthControl.Value <= ChartImageWidthControl.Maximum)
            {
                var NewVal = Convert.ToDecimal(Convert.ToDouble(ChartImageWidthControl.Value) / 2.85);
                if (NewVal > ChartImageHeightControl.Maximum)
                {
                    ChartImageHeightControl.Value = ChartImageHeightControl.Maximum;
                }
                else if (NewVal < ChartImageHeightControl.Minimum)
                {
                    ChartImageHeightControl.Value = ChartImageHeightControl.Minimum;
                }
                else
                {
                    ChartImageHeightControl.Value = Convert.ToDecimal(Convert.ToDouble(ChartImageWidthControl.Value) / 2.85);
                }
            }
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ChartImageHeightControl_ValueChanged(object sender, EventArgs e)
        {
            if (ChartImageHeightControl.Value <= ChartImageHeightControl.Maximum)
            {
                ChartImageWidthControl.Value = Convert.ToDecimal(Convert.ToDouble(ChartImageHeightControl.Value) * 2.85);
            }
        }
    }

    sealed class DataFile
    {
        internal DataFile Construct(string FilePath)
        {
            this.Name = Path.GetFileName(FilePath);
            this.DataSets = new HashSet<DataSet>();
            var RawLines = File.ReadAllLines(FilePath);
            var LockThis = new object();
            this.IsALog = false;
            if (FilePath.EndsWith("HandleServer.dll.log", StringComparison.InvariantCultureIgnoreCase))
            {
                this.IsALog = true;
                var DataSetNames = new string[10] { "GetObjectsToDelete", "GetCivData", "GetCivCacheData", "CountEnemyInArea", "CountPlayerInArea", "AddObjectToDelete", "AddObjectsData", "DeleteObjects", "AddCivWayPos", "AssignCivNames" };
                Parallel.ForEach(DataSetNames, new ParallelOptions { MaxDegreeOfParallelism = DataSetNames.Length }, CurDataSetName =>
                {
                    var CurDataSet = new DataSet(CurDataSetName);
                    lock (LockThis)
                    {
                        DataSets.Add(CurDataSet);
                    }
                    double CurValue;
                    for (int i2 = 0; i2 < RawLines.Length; i2++)
                    {
                        CurValue = GetValue(RawLines[i2], CurDataSetName, ": ", "ms", 0);
                        if (CurValue > -1)
                        {
                            CurDataSet.DataValues.Add(CurValue);
                        }
                    }
                    if (CurDataSet.DataValues.Count > 0)
                    {
                        CurDataSet.AVGValue = Math.Round(CurDataSet.DataValues.Average(), 3, MidpointRounding.AwayFromZero);
                        CurDataSet.MaxValue = CurDataSet.DataValues.Max();
                    }
                    if (CurDataSet.MaxValue == 0)
                    {
                        DataSets.Remove(CurDataSet);
                    }
                });
            }
            else if (FilePath.EndsWith("HandleServer_Standalone.Perf.log", StringComparison.InvariantCultureIgnoreCase))
            {
                var DataSetNames = new HashSet<DataSetName>();
                DataSetNames.Add(new DataSetName("Durchschnittlich gecached", "(", "%)", 2));
                DataSetNames.Add(new DataSetName("Durchschnittlich nicht gecached", "(", "%)", 3));
                DataSetNames.Add(new DataSetName("Empfangene Datenmenge", " ", "MB", 2));
                DataSetNames.Add(new DataSetName("Gesendete Datenmenge", " ", "MB", 3));
                DataSetNames.Add(new DataSetName("Bandbreite", "AVG Bandwidth: ", "Kbit/s", 0));
                Parallel.ForEach(DataSetNames, new ParallelOptions { MaxDegreeOfParallelism = DataSetNames.Count }, CurDataSetName =>
                {
                    var CurDataSet = new DataSet(CurDataSetName.Name);
                    lock (LockThis)
                    {
                        DataSets.Add(CurDataSet);
                    }
                    double CurValue;
                    for (int i2 = 0; i2 < RawLines.Length; i2++)
                    {
                        CurValue = GetValue(RawLines[i2], CurDataSetName.SecondCut, CurDataSetName.FirstCut, CurDataSetName.SecondCut, CurDataSetName.CutType);
                        if (CurValue > -1)
                        {
                            CurDataSet.DataValues.Add(CurValue);
                        }
                    }
                    if (CurDataSet.DataValues.Count > 0)
                    {
                        CurDataSet.AVGValue = Math.Round(CurDataSet.DataValues.Average(), 3, MidpointRounding.AwayFromZero);
                        CurDataSet.MaxValue = CurDataSet.DataValues.Max();
                    }
                    if (CurDataSet.MaxValue == 0)
                    {
                        DataSets.Remove(CurDataSet);
                    }
                });
            }
            else if(FilePath.EndsWith(".rpt", StringComparison.InvariantCultureIgnoreCase))
            {
                var DataSetNames = new HashSet<DataSetName>();
                DataSetNames.Add(new DataSetName("Einheiten", "Einheiten: ", " DavonLokalOhne", 0));
                DataSetNames.Add(new DataSetName("Lokale Einheiten (Keine Zivilisten)", " DavonLokalOhneZivs: ", " Spieler: ", 0));
                DataSetNames.Add(new DataSetName("Lokale Einheiten", " DavonLokal: ", " Spieler: ", 0));
                DataSetNames.Add(new DataSetName("Spieler", " Spieler: ", " Objekte: ", 0));
                DataSetNames.Add(new DataSetName("Objekte", " Objekte: ", " FPS: ", 0));
                DataSetNames.Add(new DataSetName("FPS", " FPS: ", " Schleifen: ", 0));
                DataSetNames.Add(new DataSetName("Schleifen", " Schleifen: ", " Skripte: ", 0));
                DataSetNames.Add(new DataSetName("Skriptthreads", " Skripte: [", "]", 1));
                Parallel.ForEach(DataSetNames, new ParallelOptions { MaxDegreeOfParallelism = DataSetNames.Count }, CurDataSetName =>
                {
                    var CurDataSet = new DataSet(CurDataSetName.Name);
                    lock (LockThis)
                    {
                        DataSets.Add(CurDataSet);
                    }
                    double CurValue;
                    for (int i2 = 0; i2 < RawLines.Length; i2++)
                    {
                        CurValue = GetValue(RawLines[i2], CurDataSetName.FirstCut, CurDataSetName.FirstCut, CurDataSetName.SecondCut, CurDataSetName.CutType);
                        if (CurValue > -1)
                        {
                            CurDataSet.DataValues.Add(CurValue);
                        }
                    }
                    if (CurDataSet.DataValues.Count > 0)
                    {
                        CurDataSet.AVGValue = Math.Round(CurDataSet.DataValues.Average(), 3, MidpointRounding.AwayFromZero);
                        CurDataSet.MaxValue = CurDataSet.DataValues.Max();
                    }
                    if (CurDataSet.MaxValue == 0)
                    {
                        DataSets.Remove(CurDataSet);
                    }
                });
                if ((DataSets.Any(i => i.Name.Equals("Skriptthreads"))) && (DataSets.Any(i => i.Name.Equals("FPS"))))
                {
                    var CurCustomDataSet = new DataSet("Rechenzeit(ms) pro Skriptthread & Sekunde");
                    DataSets.Add(CurCustomDataSet);
                    var FPSSet = DataSets.First(i => i.Name.Equals("FPS"));
                    var ThreadsSet = DataSets.First(i => i.Name.Equals("Skriptthreads"));
                    Parallel.For(0, FPSSet.DataValues.Count, new ParallelOptions { MaxDegreeOfParallelism = FPSSet.DataValues.Count }, i =>
                    {
                        if ((ThreadsSet.DataValues.Count > i) && (FPSSet.DataValues[i] > 0) && (ThreadsSet.DataValues[i] > 0))
                        {
                            lock (LockThis)
                            {
                                CurCustomDataSet.DataValues.Add(Math.Round((FPSSet.DataValues[i] * 3) / ThreadsSet.DataValues[i], 1, MidpointRounding.AwayFromZero));
                            }
                        }
                    });
                    CurCustomDataSet.AVGValue = Math.Round(CurCustomDataSet.DataValues.Average(), 3, MidpointRounding.AwayFromZero);
                    CurCustomDataSet.MaxValue = CurCustomDataSet.DataValues.Max();
                    if (CurCustomDataSet.MaxValue == 0)
                    {
                        DataSets.Remove(CurCustomDataSet);
                    }
                }
            }
            return this;
        }
        private double GetValue(string RawTextLine, string Filter, string FirstCut, string SecondCut, int CutType)
        {
            double Value = -1, CurValue = 0;
            if (RawTextLine.ToLowerInvariant().Contains(Filter.ToLowerInvariant()))
            {
                string RawText;
                switch (CutType)
                {
                    case 0:
                        RawText = RawTextLine.Split(new string[1] { FirstCut }, StringSplitOptions.None)[1].Split(new string[1] { SecondCut }, StringSplitOptions.None)[0];
                        double.TryParse(RawText, out Value);
                        break;
                    case 1:
                        var RawTexts = RawTextLine.Split(new string[1] { FirstCut }, StringSplitOptions.None)[1].Split(new string[1] { SecondCut }, StringSplitOptions.None)[0].Split(',');
                        for (int i = 0; i < RawTexts.Length; i++)
                        {
                            double.TryParse(RawTexts[i], out CurValue);
                            Value += CurValue;
                        }
                        break;
                    case 2:
                        RawText = RawTextLine.Split(new string[1] { SecondCut }, StringSplitOptions.None)[0].Split(new string[1] { FirstCut }, StringSplitOptions.None)[1];
                        double.TryParse(RawText, out Value);
                        break;
                    case 3:
                        RawText = RawTextLine.Split(new string[1] { SecondCut }, StringSplitOptions.None)[1].Split(new string[1] { FirstCut }, StringSplitOptions.None)[1];
                        double.TryParse(RawText, out Value);
                        break;
                }
            }
            return Value;
        }
        internal DataSet GetDataSet(string Name)
        {
            foreach (var CurDataSet in DataSets)
            {
                if (CurDataSet.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return CurDataSet;
                }
            }
            return null;
        }
        internal string Name { get; set; }
        internal HashSet<DataSet> DataSets { get; set; }
        internal bool IsALog { get; set; }
    }
    sealed class DataSet
    {
        internal DataSet(string Name)
        {
            this.MaxValue = 0;
            this.AVGValue = 0;
            this.Name = Name;
            this.DataValues = new List<double>();
        }
        internal string Name { get; private set; }
        internal List<double> DataValues { get; set; }
        internal double MaxValue { get; set; }
        internal double AVGValue { get; set; }
    }
    sealed class DataSetName
    {
        internal DataSetName(string Name, string FirstCut, string SecondCut, int CutType)
        {
            this.Name = Name;
            this.FirstCut = FirstCut;
            this.SecondCut = SecondCut;
            this.CutType = CutType;
        }
        internal string Name { get; private set; }
        internal string FirstCut { get; private set; }
        internal string SecondCut { get; private set; }
        internal int CutType { get; private set; }
    }
}
