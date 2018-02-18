using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HandleServerStandalone
{
    class PerfMonitor
    {
        internal PerfMonitor()
        {
            this.PlayersStats = new Dictionary<string, PlayerStats>();
            this.LockThis = new object();
        }

        internal Dictionary<string, PlayerStats> PlayersStats { get; set; }
        internal object LockThis;

        internal void AddOrUpdatePlayerNetStats(string Name, long ReadedDataByteCount, long WrittenDataByteCount)
        {
            Task.Factory.StartNew(() =>
            {
                lock (LockThis)
                {
                    if (PlayersStats.ContainsKey(Name))
                    {
                        var CurPlayer = PlayersStats[Name];
                        CurPlayer.ReadedDataByteCount += ReadedDataByteCount;
                        CurPlayer.WrittenDataByteCount += WrittenDataByteCount;
                        CurPlayer.LastUpdateTime = DateTime.Now;
                    }
                    else
                    {
                        PlayersStats.Add(Name, new PlayerStats(Name, ReadedDataByteCount, WrittenDataByteCount));
                    }
                }
            });
        }

        internal void AddOrUpdatePlayerCacheStats(string Name, int CachedCount, int UnCachedCount)
        {
            Task.Factory.StartNew(() =>
            {
                lock (LockThis)
                {
                    if (PlayersStats.ContainsKey(Name))
                    {
                        var CurPlayer = PlayersStats[Name];
                        CurPlayer.CachedCount = CachedCount;
                        CurPlayer.UnCachedCount = UnCachedCount;
                        CurPlayer.LastUpdateTime = DateTime.Now;
                    }
                }
            });
        }

        private string GetFormatedConsoleText(string RefText, string RAWText)
        {
            var RefTextPartLegths = RefText.Split('|').Select(P => P.Length).ToArray();
            var RAWTextParts = RAWText.Split('|');
            for (int i = 0; i < RefTextPartLegths.Length; i++)
            {
                if (RAWTextParts[i].Length > RefTextPartLegths[i])
                {
                    RAWTextParts[i] = RAWTextParts[i].Substring(0, RefTextPartLegths[i]);
                }
                else
                {
                    RAWTextParts[i] = RAWTextParts[i].PadRight(RefTextPartLegths[i]);
                }
            }
            return string.Join(" ", RAWTextParts);
        }

        internal void StartPerfMonitor()
        {
            long TotalCachedCount, TotalUnCachedCount, TotalReadedDataByteCount, TotalWrittenDataByteCount;
            var PerfLog = new List<string>();
            var RefText = "Name:          |Cached:        |UnCached:      |ReadedData:    |WrittenData:   ";
            while (true)
            {
                Console.Clear();
                TotalCachedCount = 0;
                TotalUnCachedCount = 0;
                TotalReadedDataByteCount = 0;
                TotalWrittenDataByteCount = 0;
                PerfLog.Add("Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                PerfLog.Add(RefText.Replace('|', ' ') + Environment.NewLine);
                lock (LockThis)
                {
                    if (PlayersStats.Count > 0)
                    {
                        var KeysToRemove = new HashSet<string>();
                        foreach (var Player in PlayersStats)
                        {
                            if (DateTime.Now < Player.Value.LastUpdateTime.AddSeconds(30))
                            {
                                var PlayerVal0 = Player.Key;
                                var PlayerVal1 = Player.Value.CachedCount;
                                var PlayerVal2 = Player.Value.UnCachedCount;
                                var PlayerVal3 = (Player.Value.ReadedDataByteCount > 0) ? Math.Round((double)Player.Value.ReadedDataByteCount / 1024, 1, MidpointRounding.AwayFromZero) : 0;
                                var PlayerVal4 = (Player.Value.WrittenDataByteCount > 0) ? Math.Round((double)Player.Value.WrittenDataByteCount / 1024, 1, MidpointRounding.AwayFromZero) : 0;
                                PerfLog.Add(GetFormatedConsoleText(RefText, string.Format("{0}|{1}|{2}|{3}KB|{4}KB", PlayerVal0, PlayerVal1, PlayerVal2, PlayerVal3, PlayerVal4)));
                                TotalCachedCount += Player.Value.CachedCount;
                                TotalUnCachedCount += Player.Value.UnCachedCount;
                                TotalReadedDataByteCount += Player.Value.ReadedDataByteCount;
                                TotalWrittenDataByteCount += Player.Value.WrittenDataByteCount;
                                Player.Value.ReadedDataByteCount = 0;
                                Player.Value.WrittenDataByteCount = 0;
                            }
                            else
                            {
                                KeysToRemove.Add(Player.Key);
                            }
                        }
                        foreach (var Key in KeysToRemove)
                        {
                            PlayersStats.Remove(Key);
                        }
                        PerfLog.Add("-".PadRight(RefText.Length, '-'));
                        var SumVal0 = TotalCachedCount;
                        var SumVal1 = (TotalCachedCount > 0) ? ((TotalCachedCount * 100) / (TotalCachedCount + TotalUnCachedCount)) : 0;
                        var SumVal2 = TotalUnCachedCount;
                        var SumVal3 = (TotalUnCachedCount > 0) ? ((TotalUnCachedCount * 100) / (TotalCachedCount + TotalUnCachedCount)) : 0;
                        var SumVal4 = (TotalReadedDataByteCount > 0) ? Math.Round((double)TotalReadedDataByteCount / 1024, 2, MidpointRounding.AwayFromZero) : 0;
                        var SumVal5 = (TotalWrittenDataByteCount > 0) ? Math.Round((double)TotalWrittenDataByteCount / 1024, 2, MidpointRounding.AwayFromZero) : 0;
                        PerfLog.Add(GetFormatedConsoleText(RefText, string.Format("Summarized|{0}({1}%)|{2}({3}%)|{4}KB|{5}KB", SumVal0, SumVal1, SumVal2, SumVal3, SumVal4, SumVal5)));
                        PerfLog.Add(Environment.NewLine + "AVG Bandwidth: " + (((TotalReadedDataByteCount + TotalWrittenDataByteCount) > 0) ? (Math.Round((double)((TotalReadedDataByteCount + TotalWrittenDataByteCount) * 8) / 61440, 2, MidpointRounding.AwayFromZero)).ToString() : "0") + "Kbit/s");
                    }
                    else
                    {
                        PerfLog.Add(Environment.NewLine + " --No data yet--");
                    }
                }
                PerfLog.Add(string.Empty);
                for (int i = 0; i < PerfLog.Count; i++)
                {
                    Console.WriteLine(PerfLog[i]);
                    try
                    {
                        var OwnPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "HandleServer_Standalone.Perf.log");
                        File.AppendAllText(OwnPath, PerfLog[i] + Environment.NewLine);
                    } catch { }
                }
                PerfLog.Clear();
                Thread.Sleep(60000);
            }
        }
    }
    internal sealed class PlayerStats
    {
        internal PlayerStats(string Name, long ReadedDataByteCount, long WrittenDataByteCount)
        {
            this.Name = Name;
            this.CachedCount = 0;
            this.UnCachedCount = 0;
            this.ReadedDataByteCount = 0;
            this.WrittenDataByteCount = 0;
            this.LastUpdateTime = DateTime.Now;
        }
        
        internal string Name { get; set; }
        internal int CachedCount { get; set; }
        internal int UnCachedCount { get; set; }
        internal long ReadedDataByteCount { get; set; }
        internal long WrittenDataByteCount { get; set; }
        internal DateTime LastUpdateTime { get; set; }
    }
}
