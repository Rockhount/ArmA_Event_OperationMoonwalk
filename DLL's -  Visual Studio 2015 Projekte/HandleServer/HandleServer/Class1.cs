using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HandleServer
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
                    output.Append(string.Empty);
                    if (input.Contains("|"))
                    {
                        var PerfTimer = new Stopwatch();
                        PerfTimer.Start();
                        ServicePointManager.DefaultConnectionLimit = 100;
                        var ServerIP = (input.Split('|')[0]).Replace("\"", string.Empty).Replace("'", string.Empty);
                        var UMissionID = (input.Split('|')[1]).Replace("\"", string.Empty).Replace("'", string.Empty);
                        input = input.Split('|').Last();
                        var MethodName = input.Split(',').First();
                        var MethodLowName = MethodName.ToLowerInvariant();
                        var ReturningMethodNames = new string[7] { "getobjectstodelete", "getcivdata", "getcivcachedata", "countenemyinarea", "countplayerinarea", "addcivwaypos", "test" };
                        using (var CurClient = new TcpClient(ServerIP, 50102))
                        {
                            using (var CurStream = CurClient.GetStream())
                            {
                                CurStream.ReadTimeout = 1250;
                                CurStream.WriteTimeout = 1250;
                                var ServerData = CompressString(UMissionID + "Post&GetServerData|" + input);
                                CurStream.Write(ServerData, 0, ServerData.Length);
                                if (ReturningMethodNames.Any(w => MethodLowName.Contains(w)))
                                {
                                    int BytesReaded = 0, i = 0;
                                    using (var ByteStream = new MemoryStream())
                                    {
                                        var buffer = new byte[CurClient.ReceiveBufferSize];
                                        while (((BytesReaded == CurClient.ReceiveBufferSize) || (i == 0)) && (i < 35))
                                        {
                                            BytesReaded = CurStream.Read(buffer, 0, CurClient.ReceiveBufferSize);
                                            ByteStream.Write(buffer, 0, BytesReaded);
                                            i++;
                                        }
                                        if (i != 35)
                                        {
                                            output.Append(DecompressString(ByteStream.ToArray()));
                                        }
                                    }
                                }
                                CurStream.Close();
                            }
                            CurClient.Close();
                        }
                        DebugInfo(PerfTimer, MethodName);
                    }
                }
                else
                {
                    throw new InvalidDateException(EndDate);
                }
            }
            catch (Exception ex)
            {
                output.Append('"' + "HandleServer.dll" + "\r\n" + ex.Message + "\r\n" + ex.StackTrace + '"');
            }
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
        private static bool DebugInfo(Stopwatch PerfTimer, string MethodName)
        {
            PerfTimer.Stop();
            WriteErrorLog("Perf " + MethodName + ": " + PerfTimer.Elapsed.TotalMilliseconds.ToString() + "ms");
            PerfTimer = null;
            return true;
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static bool WriteErrorLog(string Error)
        {
            try
            {
                var OwnPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "HandleServer.dll.log");
                File.AppendAllText(OwnPath, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + " - " + Error + Environment.NewLine);
            }
            catch
            {
                return false;
            }
            return true;
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
