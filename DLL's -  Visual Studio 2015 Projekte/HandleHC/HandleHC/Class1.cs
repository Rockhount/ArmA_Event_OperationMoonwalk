using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Text;

namespace HandleHC
{
    public sealed class CustomClass
    {
        #if WIN64
        [RGiesecke.DllExport.DllExport("RVExtensionVersion", CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]
        #else
        [RGiesecke.DllExport.DllExport("_RVExtensionVersion@8", CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]
        #endif
        public static void RVExtensionVersion(string output, int outputsize)
        {
            output = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #if WIN64
        [RGiesecke.DllExport.DllExport("RVExtension", CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]
        #else
        [RGiesecke.DllExport.DllExport("_RVExtension@12", CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]
        #endif
        public static void RVExtension(StringBuilder output, int outputsize, string input)
        {
            try
            {
                DateTime EndDate;
                DateTime.TryParse("15" + "." + "2" + "." + "2018", out EndDate); //<----- Hier Ablaufdatum des Events + 1 Tag eintragen, damit die DLL nur für das vorgesehene Event benutzt werden kann
                if (EndDate >= DateTime.Now)
                {
                    var CleanInput = input.Replace("\"", string.Empty).Replace("'", string.Empty);
                    if (CleanInput.StartsWith("[PostHCData", StringComparison.OrdinalIgnoreCase))
                    {
                        var Args = GetNETObjects(CleanInput);
                        if (!ReferenceEquals(Args[1], null) && !ReferenceEquals(Args[2], null) && !ReferenceEquals(Args[3], null))
                        {
                            var ServerIP = (string)Args[1];
                            var UMissionID = (string)Args[2];
                            var RAWData = (List<object>)Args[3];
                            var EnemyData = (List<object>)RAWData[0];//[[0:1,[0,0,0]],...]
                            var ObjectData = (List<object>)RAWData[1];//[[0:1,[0,0,0]],...]
                            var DeletionData = (List<object>)RAWData[2];//[[0,0:1],[1,0:2],...]
                            var ImmediateDeletionData = (List<object>)RAWData[3];//[[0,0:1],[1,0:2],...]
                            var EnemyTransferText = new StringBuilder();
                            var ObjectTransferText = new StringBuilder();
                            var DeletionTransferText = new StringBuilder();
                            var ImmediateDeletionTransferText = new StringBuilder();
                            List<object> CurDataSet;
                            for (int i = 0; i < EnemyData.Count; i++)
                            {
                                CurDataSet = (List<object>)EnemyData[i];
                                if (!ReferenceEquals(CurDataSet[0], null) && !ReferenceEquals(CurDataSet[1], null))
                                {
                                    EnemyTransferText.Append((string)CurDataSet[0] + ";" + PosToString((List<object>)CurDataSet[1]) + ";");
                                }
                            }
                            if (EnemyTransferText.Length > 0)
                            {
                                EnemyTransferText.Remove(EnemyTransferText.Length - 1, 1);
                            }
                            for (int i = 0; i < ObjectData.Count; i++)
                            {
                                CurDataSet = (List<object>)ObjectData[i];
                                if (!ReferenceEquals(CurDataSet[0], null) && !ReferenceEquals(CurDataSet[1], null))
                                {
                                    ObjectTransferText.Append((string)CurDataSet[0] + ";" + PosToString((List<object>)CurDataSet[1]) + ";");
                                }
                            }
                            if (ObjectTransferText.Length > 0)
                            {
                                ObjectTransferText.Remove(ObjectTransferText.Length - 1, 1);
                            }
                            for (int i = 0; i < DeletionData.Count; i++)
                            {
                                CurDataSet = (List<object>)DeletionData[i];
                                if (!ReferenceEquals(CurDataSet[0], null) && !ReferenceEquals(CurDataSet[1], null))
                                {
                                    DeletionTransferText.Append(((int)CurDataSet[0]).ToString() + ";" + (string)CurDataSet[1] + ";");
                                }
                            }
                            if (DeletionTransferText.Length > 0)
                            {
                                DeletionTransferText.Remove(DeletionTransferText.Length - 1, 1);
                            }
                            for (int i = 0; i < ImmediateDeletionData.Count; i++)
                            {
                                CurDataSet = (List<object>)ImmediateDeletionData[i];
                                if (!ReferenceEquals(CurDataSet[0], null) && !ReferenceEquals(CurDataSet[1], null))
                                {
                                    ImmediateDeletionTransferText.Append(((int)CurDataSet[0]).ToString() + ";" + (string)CurDataSet[1] + ";");
                                }
                            }
                            if (ImmediateDeletionTransferText.Length > 0)
                            {
                                ImmediateDeletionTransferText.Remove(ImmediateDeletionTransferText.Length - 1, 1);
                            }
                            using (var CurClient = new TcpClient(ServerIP, 50101))
                            {
                                using (var CurStream = CurClient.GetStream())
                                {
                                    CurStream.ReadTimeout = 1250;
                                    CurStream.WriteTimeout = 1250;
                                    var HCData = CompressString(UMissionID + "PostHCData|" + EnemyTransferText.ToString() + "|" + ObjectTransferText.ToString() + "|" + DeletionTransferText.ToString() + "|" + ImmediateDeletionTransferText.ToString());
                                    CurStream.Write(HCData, 0, HCData.Length);
                                    EnemyTransferText = null;
                                    ObjectTransferText = null;
                                    RAWData = null;
                                    EnemyData = null;
                                    ObjectData = null;
                                    CurStream.Close();
                                }
                                CurClient.Close();
                            }
                        }
                    }
                    else if (CleanInput.StartsWith("[Test", StringComparison.OrdinalIgnoreCase))
                    {
                        var Args = GetNETObjects(CleanInput);
                        if (!ReferenceEquals(Args[1], null) && !ReferenceEquals(Args[2], null))
                        {
                            var ServerIP = (string)Args[1];
                            var UMissionID = (string)Args[2];
                            DateTime CurEndDate;
                            DateTime.TryParse("15" + "." + "2" + "." + "2018", out CurEndDate); //<----- Hier Ablaufdatum des Events + 1 Tag eintragen, damit die DLL nur für das vorgesehene Event benutzt werden kann
                            if (CurEndDate >= DateTime.Now)
                            {
                                try
                                {
                                    using (var CurClient = new TcpClient(ServerIP, 50101))
                                    {
                                        using (var CurStream = CurClient.GetStream())
                                        {
                                            CurStream.ReadTimeout = 1250;
                                            CurStream.WriteTimeout = 1250;
                                            var TestData = CompressString(UMissionID + "Test|");
                                            CurStream.Write(TestData, 0, TestData.Length);
                                            int ReadedByteCount = 0, i = 0;
                                            using (var ByteStream = new MemoryStream())
                                            {
                                                var buffer = new byte[CurClient.ReceiveBufferSize];
                                                while (((ReadedByteCount == CurClient.ReceiveBufferSize) || (i == 0)) && (i < 35))
                                                {
                                                    ReadedByteCount = CurStream.Read(buffer, 0, CurClient.ReceiveBufferSize);
                                                    ByteStream.Write(buffer, 0, ReadedByteCount);
                                                    i++;
                                                }
                                                if (i != 35)
                                                {
                                                    var Message = DecompressString(ByteStream.ToArray());
                                                    if (Message.Equals("Test"))
                                                    {
                                                        output.Append("Test");
                                                    }
                                                }
                                                CurStream.Close();
                                            }
                                        }
                                        CurClient.Close();
                                    }
                                }
                                catch { }
                            }
                            else
                            {
                                throw new InvalidDateException(EndDate);
                            }
                        }
                    }
                }
                else
                {
                    throw new InvalidDateException(EndDate);
                }
                output.Append(string.Empty);
            }
            catch (Exception ex)
            {
                output.Append('"' + "HandleHC.dll" + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace + '"');
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static string PosToString(List<object> Pos)
        {
            return ("[" + ((int)Pos[0]).ToString() + "," + ((int)Pos[1]).ToString() + "," + ((int)Pos[2]).ToString() + "]");
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static byte[] CompressString(string text)
        {
            var raw = Encoding.UTF8.GetBytes(text);
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                raw = memory.ToArray();
            }
            return raw;
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static string DecompressString(byte[] gzip)
        {
            var ReturnText = string.Empty;
            using (var stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    int count = 1;
                    while (count > 0)
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    buffer = null;
                    try
                    {
                        ReturnText = Encoding.UTF8.GetString(memory.ToArray());
                    }
                    catch { }
                }
            }
            return ReturnText;
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private struct SnapShotStruct
        {
            public string RAWText;
            public List<object> Results;
            public int Counter;
            public bool CurrentObjectBool;
            public string CurrentObject;
            public int stage;
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static List<object> GetNETObjects(string RAWText)
        {
            var Results = new List<object>();
            Stack<SnapShotStruct> snapshotStack = new Stack<SnapShotStruct>();
            SnapShotStruct currentSnapshot = new SnapShotStruct();
            currentSnapshot.RAWText = RAWText;
            currentSnapshot.Results = Results;
            currentSnapshot.Counter = 0;
            currentSnapshot.CurrentObjectBool = false;
            currentSnapshot.CurrentObject = string.Empty;
            currentSnapshot.stage = 0;
            snapshotStack.Push(currentSnapshot);
            while (snapshotStack.Count > 0)
            {
                currentSnapshot = snapshotStack.Pop();
                if (currentSnapshot.stage == 0)
                {
                    currentSnapshot.stage = 1;
                    if (currentSnapshot.RAWText.StartsWith("["))
                    {
                        currentSnapshot.RAWText = currentSnapshot.RAWText.Remove(0, 1);
                        currentSnapshot.RAWText = currentSnapshot.RAWText.Remove(currentSnapshot.RAWText.Length - 1, 1);
                    }
                    for (int i = 0; i < currentSnapshot.RAWText.Length; i++)
                    {
                        if (currentSnapshot.RAWText[i].Equals('['))
                        {
                            currentSnapshot.Counter++;
                        }
                        if (currentSnapshot.RAWText[i].Equals(']'))
                        {
                            currentSnapshot.Counter--;
                        }
                        if (!currentSnapshot.RAWText[i].Equals(',') || (currentSnapshot.Counter > 0))
                        {
                            currentSnapshot.CurrentObject += currentSnapshot.RAWText[i];
                        }
                        if ((currentSnapshot.Counter == 0))
                        {
                            if (currentSnapshot.RAWText[i].Equals(',') || (i == (currentSnapshot.RAWText.Length - 1)))
                            {
                                if (currentSnapshot.CurrentObject.Contains("["))
                                {
                                    currentSnapshot.Results.Add(new List<object>());
                                    SnapShotStruct newSnapshot;
                                    newSnapshot.RAWText = currentSnapshot.CurrentObject;
                                    newSnapshot.Results = (List<object>)currentSnapshot.Results[currentSnapshot.Results.Count - 1];
                                    newSnapshot.Counter = 0;
                                    newSnapshot.CurrentObjectBool = false;
                                    newSnapshot.CurrentObject = string.Empty;
                                    newSnapshot.stage = 0;
                                    snapshotStack.Push(newSnapshot);
                                }
                                else if ((currentSnapshot.CurrentObject.Split('.').Length < 3) && (int.TryParse(currentSnapshot.CurrentObject.Split('.')[0], out currentSnapshot.Counter)))
                                {
                                    currentSnapshot.Results.Add(currentSnapshot.Counter);
                                    currentSnapshot.Counter = 0;
                                }
                                else if (bool.TryParse(currentSnapshot.CurrentObject, out currentSnapshot.CurrentObjectBool))
                                {
                                    currentSnapshot.Results.Add(currentSnapshot.CurrentObjectBool);
                                    currentSnapshot.CurrentObjectBool = false;
                                }
                                else if (!currentSnapshot.CurrentObject.Equals("<null>", StringComparison.Ordinal) && !currentSnapshot.CurrentObject.Equals("<NULL-object>", StringComparison.Ordinal) && !currentSnapshot.CurrentObject.Equals("any", StringComparison.Ordinal))
                                {
                                    currentSnapshot.Results.Add(currentSnapshot.CurrentObject);
                                }
                                else
                                {
                                    currentSnapshot.Results.Add(null);
                                }
                                currentSnapshot.CurrentObject = string.Empty;
                            }
                        }
                    }
                }
            }
            return Results;
        }
    }
    [Serializable]
    class InvalidDateException : Exception
    {
        public InvalidDateException()
        { }
        public InvalidDateException(DateTime date) : base(String.Format("Invalid date: {0}", date.ToString("dd.MM.yyyy")))
        { }
    }
}