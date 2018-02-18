using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HandleClient
{
    public sealed class CustomClass
    {
        private static HashSet<string> OldNearUnits { get; set; }
        private static HashSet<string> OldFarUnits { get; set; }
        private static HashSet<string> NearUnits { get; set; }
        private static HashSet<string> FarUnits { get; set; }
        private static object UnitsLock { get; set; }
        private static bool AlreadyStarted { get; set; }
        private static string PlayerName { get; set; }
        private static string PlayerPos { get; set; }
        private static string ServerIP { get; set; }
        private static string UMissionID { get; set; }
        private static DateTime LastUpdateTime { get; set; }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
                LastUpdateTime = DateTime.Now;
                var CleanInput = input.Replace("\"", string.Empty).Replace("'", string.Empty);
                if (CleanInput.StartsWith("[GetCacheData", StringComparison.OrdinalIgnoreCase))
                {
                    var Args = GetNETObjects(CleanInput);
                    if (!ReferenceEquals(Args[1], null) && !ReferenceEquals(Args[2], null) && !ReferenceEquals(Args[3], null))
                    {
                        PlayerName = (string)Args[1];
                        PlayerPos = PosToString((List<object>)Args[2]);
                        ServerIP = (string)Args[3];
                        var NewOutput = new StringBuilder("[[");
                        lock (UnitsLock)
                        {
                            foreach (string FarUnit in FarUnits)
                            {
                                if (!OldFarUnits.Contains(FarUnit))
                                {
                                    NewOutput.Append(FarUnit + ",");
                                }
                            }
                        }
                        if (NewOutput.Length > 2)
                        {
                            NewOutput.Remove(NewOutput.Length - 1, 1);
                        }
                        NewOutput.Append("],[");
                        lock (UnitsLock)
                        {
                            foreach (string NearUnit in NearUnits)
                            {
                                if (!OldNearUnits.Contains(NearUnit))
                                {
                                    NewOutput.Append(NearUnit + ",");
                                }
                            }
                        }
                        if (!NewOutput.ToString().EndsWith("["))
                        {
                            NewOutput.Remove(NewOutput.Length - 1, 1);
                        }
                        NewOutput.Append("]]");
                        var NewFinalOutput = NewOutput.ToString();
                        while (NewFinalOutput.Contains("[,") || NewFinalOutput.Contains(",]") || NewFinalOutput.Contains(",,"))
                        {
                            NewFinalOutput = NewFinalOutput.Replace("[,", "[").Replace(",]", "]").Replace(",,", ",");
                        }
                        output.Append(NewFinalOutput);
                        lock (UnitsLock)
                        {
                            OldFarUnits = new HashSet<string>(FarUnits);
                            OldNearUnits = new HashSet<string>(NearUnits);
                        }
                    }
                }
                else if (CleanInput.StartsWith("[Reset", StringComparison.OrdinalIgnoreCase))
                {
                    var Args = GetNETObjects(CleanInput);
                    if (!ReferenceEquals(Args[1], null) && !ReferenceEquals(Args[2], null))
                    {
                        ServerIP = (string)Args[1];
                        UMissionID = (string)Args[2];
                        DateTime EndDate;
                        DateTime.TryParseExact("15" + "." + "2" + "." + "2018", "dd.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out EndDate); //<----- Hier Ablaufdatum des Events + 1 Tag eintragen, damit die DLL nur für das vorgesehene Event benutzt werden kann
                        if (EndDate >= DateTime.Now)
                        {
                            if (ReferenceEquals(AlreadyStarted, null) || !AlreadyStarted)
                            {
                                NearUnits = new HashSet<string>();
                                FarUnits = new HashSet<string>();
                                OldNearUnits = new HashSet<string>();
                                OldFarUnits = new HashSet<string>();
                                UnitsLock = new object();
                                AlreadyStarted = true;
                                new Thread(() =>
                                {
                                    ManageCommunication();
                                }).Start();
                            }
                            else
                            {
                                Environment.Exit(0);
                            }
                        }
                        else
                        {
                            throw new InvalidDateException(EndDate);
                        }
                    }
                }
                else if (CleanInput.StartsWith("[Test", StringComparison.OrdinalIgnoreCase))
                {
                    var Args = GetNETObjects(CleanInput);
                    if (!ReferenceEquals(Args[1], null) && !ReferenceEquals(Args[2], null))
                    {
                        ServerIP = (string)Args[1];
                        UMissionID = (string)Args[2];
                        DateTime EndDate;
                        DateTime.TryParseExact("15" + "." + "2" + "." + "2018", "dd.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out EndDate); //<----- Hier Ablaufdatum des Events + 1 Tag eintragen, damit die DLL nur für das vorgesehene Event benutzt werden kann
                        if (EndDate >= DateTime.Now)
                        {
                            try
                            {
                                using (var CurClient = new TcpClient(ServerIP, 50100))
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
                            } catch { }
                        }
                        else
                        {
                            throw new InvalidDateException(EndDate);
                        }
                    }
                }
                output.Append(string.Empty);
            }
            catch (Exception ex)
            {
                output.Append('"' + "HandleClient.dll" + "\r\n" + ex.Message + "\r\n" + ex.StackTrace + '"');
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static bool ManageCommunication()
        {
            while (DateTime.Now < LastUpdateTime.AddSeconds(300))
            {
                try
                {
                    if (!ReferenceEquals(PlayerPos, null))
                    {
                        using (var CurClient = new TcpClient(ServerIP, 50100))
                        {
                            using (var CurStream = CurClient.GetStream())
                            {
                                CurStream.ReadTimeout = 1250;
                                CurStream.WriteTimeout = 1250;
                                var PlayerData = CompressString(UMissionID + "Post&GetPlayerData|" + PlayerName + "|" + PlayerPos);
                                CurStream.Write(PlayerData, 0, PlayerData.Length);
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
                                        if (Message.Contains("|"))
                                        {
                                            var Parameter = Message.Split('|');
                                            var NewFarUnits = Parameter[0].Split(';');
                                            var NewNearUnits = Parameter[1].Split(';');
                                            lock (UnitsLock)
                                            {
                                                for (int i2 = 0; i2 < NewNearUnits.Length; i2++)
                                                {
                                                    FarUnits.Remove(NewNearUnits[i2]);
                                                    NearUnits.Add(NewNearUnits[i2]);
                                                }
                                                for (int i2 = 0; i2 < NewFarUnits.Length; i2++)
                                                {
                                                    NearUnits.Remove(NewFarUnits[i2]);
                                                    FarUnits.Add(NewFarUnits[i2]);
                                                }
                                            }
                                        }
                                    }
                                    CurStream.Close();
                                }
                            }
                            CurClient.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
                }
                Thread.Sleep(2800);
            }
            Environment.Exit(0);
            return true;
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
        private static string PosToString(List<object> Pos)
        {
            return ("[" + Pos[0].ToString() + "," + Pos[1].ToString() + "," + Pos[2].ToString() + "]");
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static bool WriteErrorLog(string Error)
        {
            try
            {
                var OwnPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "HandleClient.Error.log");
                File.AppendAllText(OwnPath, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + " - " + Error + Environment.NewLine);
            }
            catch
            {
                return false;
            }
            return true;
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
