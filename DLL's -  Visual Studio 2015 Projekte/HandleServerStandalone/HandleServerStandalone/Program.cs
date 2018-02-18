using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HandleServerStandalone
{
    class Program
    {
        static void Main(string[] args)
        {
            var ArmABrain = new CustomClass();
            ArmABrain.CurPerfMonitor = new PerfMonitor();
            var UMissionIDPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "HandleServer_Standalone.UMissionID.txt");
            if (File.Exists(UMissionIDPath))
            {
                ArmABrain.UMissionID = File.ReadAllText(UMissionIDPath);
            }
            else
            {
                var ErrorLogPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "HandleServer_Standalone.Error.log");
                File.AppendAllText(ErrorLogPath, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + " - " + "HandleServer_Standalone.UMissionID.txt not found" + Environment.NewLine);
            }
            new Thread(() =>
            {
                ArmABrain.StartTCPMainServer();
            }).Start();
            new Thread(() =>
            {
                ArmABrain.CurPerfMonitor.StartPerfMonitor();
            }).Start();
        }
        DateTime EndDate;
    }


    internal sealed class CustomClass
    {
        private Dictionary<string, ArmAObject> DBObjects { get; set; }
        private Dictionary<string, ArmAObject> DBEnemys { get; set; }
        private Dictionary<string, ArmAPlayer> DBPlayers { get; set; }
        private Dictionary<int, ArmACity> DBCitys { get; set; }

        private object DBObjectsLock;
        private object DBEnemysLock;
        private object DBPlayersLock;
        private object DBCitysLock;
        private object LogWriterLock;

        private int CivCacheDistance { get; set; }
        private int CacheDistance { get; set; }
        private int DeleteTimeOffset { get; set; }
        private DateTime LastUpdateTime { get; set; }
        private bool Idle { get; set; }
        internal string UMissionID { private get; set; }

        internal PerfMonitor CurPerfMonitor { get; set; }

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public string RVExtension(string input)
        {
            var output = string.Empty;
            //try
            //{
                LastUpdateTime = DateTime.Now;
                Idle = false;
                var CleanInput = input.Replace("\"", string.Empty).Replace("'", string.Empty);
                if (CleanInput.StartsWith("[GetObjectsToDelete", StringComparison.OrdinalIgnoreCase))
                {
                    output = CheckOutputLength(GetObjectsToDelete());
                }
                else if (CleanInput.StartsWith("[GetCivData", StringComparison.OrdinalIgnoreCase))
                {
                    output = CheckOutputLength(GetCivData());
                }
                else if (CleanInput.StartsWith("[GetCivCacheData", StringComparison.OrdinalIgnoreCase))
                {
                    output = CheckOutputLength(GetCivCacheData());
                }
                else if (CleanInput.StartsWith("[CountEnemyInArea", StringComparison.OrdinalIgnoreCase))
                {
                    output = CheckOutputLength(CountEnemyInArea(input));
                }
                else if (CleanInput.StartsWith("[Reset", StringComparison.OrdinalIgnoreCase))
                {
                    Idle = true;
                    Reset(CleanInput);
                }
                else if (CleanInput.StartsWith("[CountPlayerInArea", StringComparison.OrdinalIgnoreCase))
                {
                    output = CheckOutputLength(CountPlayerInArea(input));
                }
                else if (CleanInput.StartsWith("[AddObjectToDelete", StringComparison.OrdinalIgnoreCase))
                {
                    AddObjectToDelete(CleanInput);
                }
                else if (CleanInput.StartsWith("[AddObjectsData", StringComparison.OrdinalIgnoreCase))
                {
                    AddObjectsData(CleanInput);
                }
                else if (CleanInput.StartsWith("[DeleteObjects", StringComparison.OrdinalIgnoreCase))
                {
                    DeleteObjects(CleanInput);
                }
                else if (CleanInput.StartsWith("[AddCivWayPos", StringComparison.OrdinalIgnoreCase))
                {
                    output = CheckOutputLength(AddCivWayPos(CleanInput));
                }
                else if (CleanInput.StartsWith("[AssignCivNames", StringComparison.OrdinalIgnoreCase))
                {
                    AssignCivNames(CleanInput);
                }
                else if (CleanInput.StartsWith("Test", StringComparison.OrdinalIgnoreCase))
                {
                    output = "Test";
                }
            //}
            //catch (Exception ex)
            //{
            //    output = '"' + "HandleServerStandalone" + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace + '"';
            //}
            return output;
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string AddCivWayPos(string INPUT)
        {
            try
            {
                var Args = GetNETObjectsWithFloat(INPUT);//[AddCivWayPos,15,[[0,0,0],...]
                var RAWCivSpawnPositions = (List<object>)Args[2];
                var NewOutput = new StringBuilder("[");
                if (!ReferenceEquals(RAWCivSpawnPositions[0], null))
                {
                    RAWCivSpawnPositions.RemoveAll(item => ReferenceEquals(item, null));
                    if (RAWCivSpawnPositions.Count > 0)
                    {
                        var CurCity = new ArmACity(Convert.ToInt32(Args[1]), new float[RAWCivSpawnPositions.Count][]);
                        lock (DBCitysLock)
                        {
                            DBCitys.Add(DBCitys.Count, CurCity);
                        }
                        Parallel.For(0, RAWCivSpawnPositions.Count, new ParallelOptions { MaxDegreeOfParallelism = RAWCivSpawnPositions.Count }, i =>
                        {
                            CurCity.CivSpawnPositions[i] = Array.ConvertAll<object, float>(((List<object>)RAWCivSpawnPositions[i]).ToArray(), Pos => (float)Pos);
                        });
                        CurCity.AddCivs();
                        foreach (var Civ in CurCity.Civs)
                        {
                            NewOutput.Append("[" + PosToString(Civ.Value.SpawnPos) + "," + PosToString(Civ.Value.GetNewWayPos(CurCity.CivSpawnPositions, CurCity.Rand)) + "," + (DBCitys.Count - 1).ToString() + "," + Civ.Key + "]" + ",");
                        }
                    }
                }
                if (NewOutput.Length > 1)
                {
                    NewOutput.Remove(NewOutput.Length - 1, 1);
                }
                NewOutput.Append("]");
                RAWCivSpawnPositions = null;
                Args = null;
                return NewOutput.ToString();//Spawnen und laufen: [[[1,1,0],[0,0,0],CityID,CivID], ...]
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
                return "[]";
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void DeleteObjects(string INPUT)
        {
            try
            {
                new Thread(() =>
                {
                    var Args = GetNETObjectsWithInt(INPUT);//['DeleteObjects',0,0:1]
                    if (!ReferenceEquals(Args[1], null) && !ReferenceEquals(Args[2], null))
                    {
                        var ObjectType = (int)Args[1];
                        var ObjectName = (string)Args[2];
                        switch (ObjectType)
                        {
                            case 0:
                                lock (DBObjectsLock)
                                {
                                    if (DBObjects.ContainsKey(ObjectName))
                                    {
                                        DBObjects.Remove(ObjectName);
                                    }
                                }
                                break;
                            case 1:
                                lock (DBEnemysLock)
                                {
                                    if (DBEnemys.ContainsKey(ObjectName))
                                    {
                                        DBEnemys.Remove(ObjectName);
                                    }
                                }
                                break;
                        }
                    }
                    Args = null;
                }).Start();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void AddObjectsData(string INPUT)
        {
            try
            {
                new Thread(() =>
                {
                    var Args = GetNETObjectsWithInt(INPUT);
                    if (!ReferenceEquals(Args[1], null) && !ReferenceEquals(Args[2], null))
                    {
                        var RAWEnemys = (List<object>)Args[1];//[[Name,Pos], ...]
                        var RAWObjects = (List<object>)Args[2];//[[Name,Pos], ...]
                        var RAWCivs = (List<object>)Args[3];//[[Name,Pos,CityIndex,CivIndex], ...]
                        RAWEnemys.RemoveAll(item => ReferenceEquals(((List<object>)item)[0], null));
                        RAWObjects.RemoveAll(item => ReferenceEquals(((List<object>)item)[0], null));
                        RAWCivs.RemoveAll(item => ReferenceEquals(((List<object>)item)[0], null));
                        if (RAWEnemys.Count > 0)
                        {
                            lock (DBEnemysLock)
                            {
                                for (int i = 0; i < RAWEnemys.Count; i++)
                                {
                                    if (DBEnemys.ContainsKey((string)((List<object>)RAWEnemys[i])[0]))
                                    {
                                        var CurEnemy = DBEnemys[(string)((List<object>)RAWEnemys[i])[0]];
                                        CurEnemy.Pos = Array.ConvertAll<object, int>((new List<object>((List<object>)((List<object>)RAWEnemys[i])[1])).ToArray(), Pos => (int)Pos);
                                        CurEnemy.LastUpdateTime = DateTime.Now;
                                    }
                                    else
                                    {
                                        DBEnemys.Add((string)((List<object>)RAWEnemys[i])[0], new ArmAObject((string)((List<object>)RAWEnemys[i])[0], Array.ConvertAll<object, int>((new List<object>((List<object>)((List<object>)RAWEnemys[i])[1])).ToArray(), Pos => (int)Pos)));
                                    }
                                }
                            }
                        }
                        if (RAWObjects.Count > 0)
                        {
                            lock (DBObjectsLock)
                            {
                                for (int i = 0; i < RAWObjects.Count; i++)
                                {
                                    if (DBObjects.ContainsKey((string)((List<object>)RAWObjects[i])[0]))
                                    {
                                        var CurObject = DBObjects[(string)((List<object>)RAWObjects[i])[0]];
                                        CurObject.Pos = Array.ConvertAll<object, int>((new List<object>((List<object>)((List<object>)RAWObjects[i])[1])).ToArray(), Pos => (int)Pos);
                                        CurObject.LastUpdateTime = DateTime.Now;
                                    }
                                    else
                                    {
                                        DBObjects.Add((string)((List<object>)RAWObjects[i])[0], new ArmAObject((string)((List<object>)RAWObjects[i])[0], Array.ConvertAll<object, int>((new List<object>((List<object>)((List<object>)RAWObjects[i])[1])).ToArray(), Pos => (int)Pos)));
                                    }
                                }
                            }
                        }
                        if (RAWCivs.Count > 0)
                        {
                            Parallel.For(0, RAWCivs.Count, new ParallelOptions { MaxDegreeOfParallelism = RAWCivs.Count }, i =>
                            {
                                lock (DBCitys[(int)((List<object>)RAWCivs[i])[2]].LockThis)
                                {
                                    if (DBCitys[(int)((List<object>)RAWCivs[i])[2]].Civs.ContainsKey(((int)((List<object>)RAWCivs[i])[3]).ToString()))
                                    {
                                        var CurCiv = DBCitys[(int)((List<object>)RAWCivs[i])[2]].Civs[((int)((List<object>)RAWCivs[i])[3]).ToString()];
                                        CurCiv.Name = (string)((List<object>)RAWCivs[i])[0];
                                        CurCiv.Pos = Array.ConvertAll<object, int>((new List<object>((List<object>)((List<object>)RAWCivs[i])[1])).ToArray(), Pos => (int)Pos);
                                        CurCiv.LastUpdateTime = DateTime.Now;
                                    }
                                }
                            });
                        }
                        RAWEnemys = null;
                        RAWObjects = null;
                        RAWCivs = null;
                    }
                    Args = null;
                }).Start();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void AddObjectToDelete(string INPUT)
        {
            try
            {
                new Thread(() =>
                {
                    var Args = GetNETObjectsWithInt(INPUT);//['AddObjectToDelete',0,0:1]
                    if (!ReferenceEquals(Args[1], null) && !ReferenceEquals(Args[2], null))
                    {
                        var ObjectType = (int)Args[1];
                        var ObjectName = (string)Args[2];
                        switch (ObjectType)
                        {
                            case 0:
                                lock (DBObjectsLock)
                                {
                                    if (DBObjects.ContainsKey(ObjectName) && !DBObjects[ObjectName].Destroyed)
                                    {
                                        DBObjects[ObjectName].DestroyedTime = DateTime.Now;
                                        DBObjects[ObjectName].Destroyed = true;
                                        DBObjects[ObjectName].LastUpdateTime = DateTime.Now;
                                    }
                                }
                                break;
                            case 1:
                                lock (DBEnemysLock)
                                {
                                    if (DBEnemys.ContainsKey(ObjectName) && !DBEnemys[ObjectName].Destroyed)
                                    {
                                        DBEnemys[ObjectName].DestroyedTime = DateTime.Now;
                                        DBEnemys[ObjectName].Destroyed = true;
                                        DBEnemys[ObjectName].LastUpdateTime = DateTime.Now;
                                    }
                                }
                                break;
                            case 2:
                                lock (DBPlayersLock)
                                {
                                    if (DBPlayers.ContainsKey(ObjectName) && !DBPlayers[ObjectName].Destroyed)
                                    {
                                        DBPlayers[ObjectName].DestroyedTime = DateTime.Now;
                                        DBPlayers[ObjectName].Destroyed = true;
                                        DBPlayers[ObjectName].LastUpdateTime = DateTime.Now;
                                    }
                                }
                                break;
                            case 3:
                                Parallel.ForEach(DBCitys, new ParallelOptions { MaxDegreeOfParallelism = DBCitys.Count }, City =>
                                {
                                    lock (City.Value.LockThis)
                                    {
                                        if (City.Value.Civs.ContainsKey(ObjectName) && !City.Value.Civs[ObjectName].Destroyed)
                                        {
                                            City.Value.Civs[ObjectName].DestroyedTime = DateTime.Now;
                                            City.Value.Civs[ObjectName].Destroyed = true;
                                            City.Value.Civs[ObjectName].LastUpdateTime = DateTime.Now;
                                        }
                                    }
                                });
                                break;
                        }
                        ObjectName = null;
                    }
                    Args = null;
                }).Start();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string CountPlayerInArea(string INPUT)
        {
            //try
            //{
                var Args = GetNETObjectsWithInt(INPUT);//["CountPlayerInArea",[[[1462,3660,0],1000,26],...]]
                if (Args.Count > 1)
                {
                    var Triggers = (List<object>)Args[1];
                    var Triggered = new int[Triggers.Count];
                    Dictionary<string, ArmAPlayer> CurDBPlayers;
                    if (!ReferenceEquals(((List<object>)Triggers)[1], null))
                    {
                        lock (DBPlayersLock)
                        {
                            CurDBPlayers = new Dictionary<string, ArmAPlayer>(DBPlayers);
                        }
                        Parallel.For(0, Triggers.Count, new ParallelOptions { MaxDegreeOfParallelism = Triggers.Count }, i =>
                        {
                            var CurTrigger = (List<object>)Triggers[i];
                            var RAWPos = Array.ConvertAll<object, int>(((List<object>)(CurTrigger[0])).ToArray(), Pos => (int)Pos);
                            var Distance = (int)CurTrigger[1];
                            var ID = (int)CurTrigger[2];
                            Triggered[i] = -1;
                            if (CurDBPlayers.Count > 0)
                            {
                                Parallel.ForEach(CurDBPlayers, new ParallelOptions { MaxDegreeOfParallelism = CurDBPlayers.Count }, Player =>
                                {
                                    if (!Player.Value.Destroyed && (((int)Math.Round(Math.Sqrt(Player.Value.Pos.Zip(RAWPos, (a, b) => (a - b) * (a - b)).Sum()), 0)) < Distance))
                                    {
                                        Triggered[i] = ID;
                                    }
                                });
                            }
                            CurTrigger = null;
                            RAWPos = null;
                        });
                        Args = null;
                        Triggers = null;
                        CurDBPlayers = null;
                        var ReturnValue = new StringBuilder("[");
                        foreach (var ID in Triggered)
                        {
                            if (ID > -1)
                            {
                                ReturnValue.Append(ID.ToString() + ",");
                            }
                        }
                        if (ReturnValue.Length > 1)
                        {
                            ReturnValue.Remove(ReturnValue.Length - 1, 1);
                        }
                        ReturnValue.Append("]");
                        return ReturnValue.ToString();
                    }
                }
                Args = null;
                return "0";
            //}
            //catch (Exception ex)
            //{
            //    WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
            //    return "0";
            //}
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string CountEnemyInArea(string INPUT)
        {
            //try
            //{
                var Args = GetNETObjectsWithInt(INPUT);
                if (!ReferenceEquals(Args[1], null) && !ReferenceEquals(Args[2], null))
                {
                    var RAWPos = Array.ConvertAll<object, int>(((List<object>)Args[1]).ToArray(), Pos => (int)Pos);
                    var Distance = (int)Args[2];
                    int EnemyInAreaCount = 0;
                    lock (DBEnemysLock)
                    {
                        if (DBEnemys.Count > 0)
                        {
                            Parallel.ForEach(DBEnemys, new ParallelOptions { MaxDegreeOfParallelism = DBEnemys.Count }, Enemy =>
                            {
                                if (!Enemy.Value.Destroyed && (((int)Math.Round(Math.Sqrt(Enemy.Value.Pos.Zip(RAWPos, (a, b) => (a - b) * (a - b)).Sum()), 0)) < Distance))
                                {
                                    Interlocked.Increment(ref EnemyInAreaCount);
                                }
                            });
                        }
                    }
                    RAWPos = null;
                    Args = null;
                    return EnemyInAreaCount.ToString();
                }
                Args = null;
                return "0";
            //}
            //catch (Exception ex)
            //{
            //    WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
            //    return "0";
            //}
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void AssignCivNames(string INPUT)
        {
            try
            {
                new Thread(() =>
                {
                    var Args = GetNETObjectsWithInt(INPUT);//[AssignCivNames,[[0:1,0,1],...]]
                    Args = (List<object>)Args[1];//[[0:1,0,1],...]
                    Args.RemoveAll(item => ReferenceEquals(((List<object>)item)[0], null));
                    if (Args.Count > 0)
                    {
                        lock (DBCitysLock)
                        {
                            Parallel.For(0, Args.Count, new ParallelOptions { MaxDegreeOfParallelism = Args.Count }, i =>
                            {
                                var CurName = (string)((List<object>)(Args[i]))[0];
                                var ID1 = (int)((List<object>)(Args[i]))[1];
                                var ID2 = (int)((List<object>)(Args[i]))[2];
                                DBCitys[ID1].Civs[ID2.ToString()].Name = CurName;
                                DBCitys[ID1].Civs[ID2.ToString()].LastUpdateTime = DateTime.Now;
                                DBCitys[ID1].Civs[ID2.ToString()].LastWalkTime = DateTime.Now;
                                DBCitys[ID1].Civs[ID2.ToString()].Pos = Array.ConvertAll(DBCitys[ID1].Civs[ID2.ToString()].SpawnPos, x => (int)Math.Round(x, 0));
                            });
                        }
                    }
                }).Start();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string GetCivData()
        {
            try
            {
                var NewOutput1 = new StringBuilder("[");
                var NewOutput2 = new StringBuilder("[");
                object LockThis1 = new object(), LockThis2 = new object();
                Parallel.ForEach(DBCitys, new ParallelOptions { MaxDegreeOfParallelism = DBCitys.Count }, City =>
                {
                    lock (City.Value.LockThis)
                    {
                        Parallel.ForEach(City.Value.Civs, new ParallelOptions { MaxDegreeOfParallelism = City.Value.Civs.Count }, Civ =>
                        {
                            if (!ReferenceEquals(Civ.Value.Name, null))
                            {
                                if (!Civ.Value.Destroyed && (DateTime.Now > Civ.Value.LastWalkTime.AddSeconds(30)))
                                {
                                    lock (LockThis1)
                                    {
                                        NewOutput1.Append("[" + PosToString(Civ.Value.GetNewWayPos(City.Value.CivSpawnPositions, City.Value.Rand)) + "," + City.Key + "," + Civ.Key + "]" + ",");
                                    }
                                    Civ.Value.LastWalkTime = DateTime.Now;
                                }
                            }
                            else
                            {
                                lock (LockThis2)
                                {
                                    NewOutput2.Append("[" + PosToString(Civ.Value.SpawnPos) + "," + PosToString(Civ.Value.GetNewWayPos(City.Value.CivSpawnPositions, City.Value.Rand)) + "," + City.Key.ToString() + "," + Civ.Key + "]" + ",");
                                }
                                Civ.Value.LastWalkTime = DateTime.Now;
                            }
                        });
                    }
                });
                if (NewOutput1.Length > 1)//Laufen: [[[0,0,0],CityID,CivID], ...]
                {
                    NewOutput1.Remove(NewOutput1.Length - 1, 1);
                }
                NewOutput1.Append("]");
                if (NewOutput2.Length > 1)//Spawnen und laufen: [[[1,1,0],[0,0,0],CityID,CivID], ...]
                {
                    NewOutput2.Remove(NewOutput2.Length - 1, 1);
                }
                NewOutput2.Append("]");
                LockThis1 = null;
                LockThis2 = null;
                return "[" + NewOutput1.ToString() + "," + NewOutput2.ToString() + "]";
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
                return "[]";
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string GetCivCacheData()
        {
            try
            {
                var NewOutput1 = new StringBuilder("[");
                var NewOutput2 = new StringBuilder("[");
                Dictionary<string, ArmAPlayer> CurDBPlayers;
                object LockThis1 = new object(), LockThis2 = new object();
                lock (DBPlayersLock)
                {
                    CurDBPlayers = new Dictionary<string, ArmAPlayer>(DBPlayers);
                }
                Parallel.ForEach(DBCitys, new ParallelOptions { MaxDegreeOfParallelism = DBCitys.Count }, City =>
                {
                    lock (City.Value.LockThis)
                    {
                        Parallel.ForEach(City.Value.Civs, new ParallelOptions { MaxDegreeOfParallelism = City.Value.Civs.Count }, Civ =>
                        {
                            if (!ReferenceEquals(Civ.Value.Name, null) && !Civ.Value.Destroyed)
                            {
                                if (CurDBPlayers.Count > 0)
                                {
                                    if (Civ.Value.Deactivated)
                                    {
                                        foreach (var Player in CurDBPlayers)
                                        {
                                            if (!Player.Value.Destroyed && (((int)Math.Round(Math.Sqrt(Civ.Value.Pos.Zip(Player.Value.Pos, (a, b) => (a - b) * (a - b)).Sum()), 0)) < CivCacheDistance))
                                            {
                                                lock (LockThis1)
                                                {
                                                    NewOutput1.Append("[" + City.Key + "," + Civ.Key + "]" + ",");
                                                }
                                                Civ.Value.Deactivated = false;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var NearPlayerCount = 0;
                                        foreach (var Player in CurDBPlayers)
                                        {
                                            if (!Player.Value.Destroyed && (((int)Math.Round(Math.Sqrt(Civ.Value.Pos.Zip(Player.Value.Pos, (a, b) => (a - b) * (a - b)).Sum()), 0)) < CivCacheDistance))
                                            {
                                                NearPlayerCount++;
                                                break;
                                            }
                                        }
                                        if (NearPlayerCount == 0)
                                        {
                                            lock (LockThis2)
                                            {
                                                NewOutput2.Append("[" + City.Key + "," + Civ.Key + "]" + ",");
                                            }
                                            Civ.Value.Deactivated = true;
                                        }
                                    }
                                }
                            }
                        });
                    }
                });
                if (NewOutput1.Length > 1)//Aktivieren: [[CityID,CivID], ...]
                {
                    NewOutput1.Remove(NewOutput1.Length - 1, 1);
                }
                NewOutput1.Append("]");
                if (NewOutput2.Length > 1)//Deaktivieren: [[CityID,CivID], ...]
                {
                    NewOutput2.Remove(NewOutput2.Length - 1, 1);
                }
                NewOutput2.Append("]");
                LockThis1 = null;
                LockThis2 = null;
                CurDBPlayers = null;
                return "[" + NewOutput1.ToString() + "," + NewOutput2.ToString() + "]";
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
                return "[]";
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string GetObjectsToDelete()
        {
            try
            {
                var NewOutput = new StringBuilder("[");
                var LockThis = new object();
                var KeysToDelete = new HashSet<string>();
                lock (DBEnemysLock)
                {
                    if (DBEnemys.Count > 0)
                    {
                        KeysToDelete.Clear();
                        Parallel.ForEach(DBEnemys, new ParallelOptions { MaxDegreeOfParallelism = DBEnemys.Count }, Enemy =>
                        {
                            if (Enemy.Value.Destroyed && (DateTime.Now > Enemy.Value.DestroyedTime.AddSeconds(DeleteTimeOffset)))
                            {
                                lock (LockThis)
                                {
                                    NewOutput.Append("'" + Enemy.Value.Name + "',");
                                    KeysToDelete.Add(Enemy.Key);
                                }
                            }
                        });
                        foreach (var Key in KeysToDelete)
                        {
                            DBEnemys.Remove(Key);
                        }
                    }
                }
                lock (DBPlayersLock)
                {
                    if (DBPlayers.Count > 0)
                    {
                        KeysToDelete.Clear();
                        Parallel.ForEach(DBPlayers, new ParallelOptions { MaxDegreeOfParallelism = DBPlayers.Count }, Player =>
                        {
                            if (Player.Value.Destroyed && (DateTime.Now > Player.Value.DestroyedTime.AddSeconds(DeleteTimeOffset)))
                            {
                                lock (LockThis)
                                {
                                    NewOutput.Append("'" + Player.Value.Name + "',");
                                    KeysToDelete.Add(Player.Key);
                                }
                            }
                        });
                        foreach (var Key in KeysToDelete)
                        {
                            DBPlayers.Remove(Key);
                        }
                    }
                }
                lock (DBObjectsLock)
                {
                    if (DBObjects.Count > 0)
                    {
                        KeysToDelete.Clear();
                        Parallel.ForEach(DBObjects, new ParallelOptions { MaxDegreeOfParallelism = DBObjects.Count }, Object =>
                        {
                            if (Object.Value.Destroyed && (DateTime.Now > Object.Value.DestroyedTime.AddSeconds(DeleteTimeOffset)))
                            {
                                lock (LockThis)
                                {
                                    NewOutput.Append("'" + Object.Value.Name + "',");
                                    KeysToDelete.Add(Object.Key);
                                }
                            }
                        });
                        foreach (var Key in KeysToDelete)
                        {
                            DBObjects.Remove(Key);
                        }
                    }
                }
                if (DBCitys.Count > 0)
                {
                    Parallel.ForEach(DBCitys, new ParallelOptions { MaxDegreeOfParallelism = DBCitys.Count }, City =>
                    {
                        lock (City.Value.LockThis)
                        {
                            if (City.Value.Civs.Count > 0)
                            {
                                Parallel.ForEach(City.Value.Civs, new ParallelOptions { MaxDegreeOfParallelism = City.Value.Civs.Count }, Civ =>
                                {
                                    if (Civ.Value.Destroyed && (DateTime.Now > Civ.Value.DestroyedTime.AddSeconds(DeleteTimeOffset)))
                                    {
                                        lock (LockThis)
                                        {
                                            NewOutput.Append("'" + Civ.Value.Name + "',");
                                        }
                                        Civ.Value.Reset();
                                    }
                                });
                            }
                        }
                    });
                }
                LockThis = null;
                if (NewOutput.Length > 1)
                {
                    NewOutput.Remove(NewOutput.Length - 1, 1);
                }
                NewOutput.Append("]");
                return NewOutput.ToString();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
                return "[]";
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Reset(string INPUT)
        {
            try
            {
                var Args = GetNETObjectsWithInt(INPUT);
                if (!ReferenceEquals(Args[1], null) && !ReferenceEquals(Args[2], null) && !ReferenceEquals(Args[3], null))
                {
                    ServicePointManager.DefaultConnectionLimit = 2000;
                    CacheDistance = (int)Args[1];
                    CivCacheDistance = (int)Args[2];
                    DeleteTimeOffset = (int)Args[3];
                    DBObjectsLock = new object();
                    DBEnemysLock = new object();
                    DBPlayersLock = new object();
                    DBCitysLock = new object();
                    LogWriterLock = new object();
                    DBPlayers = new Dictionary<string, ArmAPlayer>();
                    DBEnemys = new Dictionary<string, ArmAObject>();
                    DBObjects = new Dictionary<string, ArmAObject>();
                    DBCitys = new Dictionary<int, ArmACity>();
                    lock (CurPerfMonitor.LockThis)
                    {
                        CurPerfMonitor.PlayersStats = new Dictionary<string, PlayerStats>();
                    }
                    new Thread(() =>
                    {
                        ManagePlayerCacheData();
                    }).Start();
                    new Thread(() =>
                    {
                        GarbageCollector();
                    }).Start();
                    new Thread(() =>
                    {
                        StartTCPPlayerServer();
                    }).Start();
                    new Thread(() =>
                    {
                        StartTCPHCServer();
                    }).Start();
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private byte[] CompressString(string text)
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
        private string DecompressString(byte[] gzip, string EndpointType)
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
                    catch (Exception ex)
                    {
                        WriteErrorLog(string.Format("Message Error (Type '{0}'):{1}{2}{3}", EndpointType, ex.Message, Environment.NewLine, ex.StackTrace));
                    }
                }
            }
            return ReturnText;
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void StartTCPPlayerServer()
        {
            var TCPPlayerServer = new TcpListener(IPAddress.Any, 50100);
            TCPPlayerServer.Start();
            while (Idle || (DateTime.Now < LastUpdateTime.AddSeconds(300)))
            {
                var client = TCPPlayerServer.AcceptTcpClient();
                Task.Factory.StartNew(() =>
                {
                    ManageTCPRequest(client, "Player");
                });
            }
            if (DateTime.Now > LastUpdateTime.AddSeconds(300))
            {
                WriteErrorLog("No Server Response");
                Environment.Exit(1);
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void StartTCPHCServer()
        {
            var TCPHCServer = new TcpListener(IPAddress.Any, 50101);
            TCPHCServer.Start();
            while (Idle || (DateTime.Now < LastUpdateTime.AddSeconds(300)))
            {
                var hc = TCPHCServer.AcceptTcpClient();
                Task.Factory.StartNew(() =>
                {
                    ManageTCPRequest(hc, "HC");
                });
            }
            if (DateTime.Now > LastUpdateTime.AddSeconds(300))
            {
                WriteErrorLog("No Server Response");
                Environment.Exit(1);
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        internal void StartTCPMainServer()
        {
            var TCPMainServer = new TcpListener(IPAddress.Any, 50102);
            TCPMainServer.Start();
            while (true)
            {
                var server = TCPMainServer.AcceptTcpClient();
                Task.Factory.StartNew(() =>
                {
                    ManageTCPRequest(server, "Server");
                });
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ManageTCPRequest(TcpClient CurClient, string EndpointType)
        {
            try
            {
                using (var stream = CurClient.GetStream())
                {
                    stream.ReadTimeout = 1250;
                    stream.WriteTimeout = 1250;
                    int ReadedByteCount = 0, i = 0;
                    using (var ByteStream = new MemoryStream())
                    {
                        var buffer = new byte[CurClient.ReceiveBufferSize];
                        while (((ReadedByteCount == CurClient.ReceiveBufferSize) || (i == 0)) && (i < 35))
                        {
                            ReadedByteCount = stream.Read(buffer, 0, CurClient.ReceiveBufferSize);
                            ByteStream.Write(buffer, 0, ReadedByteCount);
                            i++;
                        }
                        if (i != 35)
                        {
                            var Message = DecompressString(ByteStream.ToArray(), EndpointType);
                            if (Message.Contains(UMissionID))
                            {
                                Message = Message.Replace(UMissionID, string.Empty);
                                if (Message.Contains("|"))
                                {
                                    var Parameter = Message.Split('|');
                                    switch (Parameter[0])
                                    {
                                        case "Test":
                                            var CurCacheData = CompressString("Test");
                                            stream.Write(CurCacheData, 0, CurCacheData.Length);
                                            break;
                                        case "PostHCData":
                                            UpdateData(Parameter[1], Parameter[2], Parameter[3], Parameter[4]);
                                            CurPerfMonitor.AddOrUpdatePlayerNetStats("All HC's", ByteStream.Length, 0);
                                            break;
                                        case "Post&GetPlayerData":
                                            var CurPlayerCacheData = CompressString(UpdatePlayerData(Parameter[1], Parameter[2]));
                                            stream.Write(CurPlayerCacheData, 0, CurPlayerCacheData.Length);
                                            CurPerfMonitor.AddOrUpdatePlayerNetStats(Parameter[1], ByteStream.Length, CurPlayerCacheData.Length);
                                            break;
                                        case "Post&GetServerData":
                                            var CurServerData = CompressString(RVExtension(Parameter[1]));
                                            stream.Write(CurServerData, 0, CurServerData.Length);
                                            CurPerfMonitor.AddOrUpdatePlayerNetStats(EndpointType, ByteStream.Length, CurServerData.Length);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    stream.Close();
                }
            }
            finally
            {
                if (!ReferenceEquals(CurClient, null))
                {
                    (CurClient as IDisposable).Dispose();
                    CurClient = null;
                }
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void UpdateData(string EnemyData, string ObjectData, string DeletionData, string ImmediateDeletionData)
        {
            try
            {
                var EnemyDataSet = EnemyData.Split(';');
                var ObjectDataSet = ObjectData.Split(';');
                var DeletionDataSet = DeletionData.Split(';');
                var ImmediateDeletionDataSet = ImmediateDeletionData.Split(';');
                var LockThis = new object();
                if (DeletionDataSet.Length > 1)
                {
                    for (int i = 0; (i + 1) < DeletionDataSet.Length; i += 2)
                    {
                        AddObjectToDelete("['a'," + DeletionDataSet[i] + "," + DeletionDataSet[i + 1] + "]");
                    };
                };
                if (ImmediateDeletionDataSet.Length > 1)
                {
                    for (int i = 0; (i + 1) < ImmediateDeletionDataSet.Length; i += 2)
                    {
                        DeleteObjects("['a'," + ImmediateDeletionDataSet[i] + "," + ImmediateDeletionDataSet[i + 1] + "]");
                    };
                };
                if (EnemyDataSet.Length > 1)
                {
                    lock (DBEnemysLock)
                    {
                        Parallel.For(0, EnemyDataSet.Length, new ParallelOptions { MaxDegreeOfParallelism = EnemyDataSet.Length }, i =>
                        //for (int i = 0; (i + 1) < EnemyDataSet.Length; i += 2)
                        {
                            if (EnemyDataSet[i].Contains(":"))
                            {
                                if (DBEnemys.ContainsKey(EnemyDataSet[i]))
                                {
                                    DBEnemys[EnemyDataSet[i]].Pos = StringToPos(EnemyDataSet[i + 1]);
                                    DBEnemys[EnemyDataSet[i]].LastUpdateTime = DateTime.Now;
                                }
                                else
                                {
                                    lock (LockThis)
                                    {
                                        DBEnemys.Add(EnemyDataSet[i], new ArmAObject(EnemyDataSet[i], StringToPos(EnemyDataSet[i + 1])));
                                    }
                                }
                            }
                        });
                    }
                }
                if (ObjectDataSet.Length > 1)
                {
                    lock (DBObjectsLock)
                    {
                        Parallel.For(0, ObjectDataSet.Length, new ParallelOptions { MaxDegreeOfParallelism = ObjectDataSet.Length }, i =>
                        //for (int i = 0; (i + 1) < ObjectDataSet.Length; i += 2)
                        {
                            if (ObjectDataSet[i].Contains(":"))
                            {
                                if (DBObjects.ContainsKey(ObjectDataSet[i]))
                                {
                                    DBObjects[ObjectDataSet[i]].Pos = StringToPos(ObjectDataSet[i + 1]);
                                    DBObjects[ObjectDataSet[i]].LastUpdateTime = DateTime.Now;
                                }
                                else
                                {
                                    lock (LockThis)
                                    {
                                        DBObjects.Add(ObjectDataSet[i], new ArmAObject(ObjectDataSet[i], StringToPos(ObjectDataSet[i + 1])));
                                    }
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string UpdatePlayerData(string PlayerName, string PlayerPos)
        {
            try
            {
                var CurPlayerPos = StringToPos(PlayerPos);
                var PlayerCacheData = new StringBuilder("|");
                var PlayerCacheDataLock = new object();
                lock (DBPlayersLock)
                {
                    if ((DBPlayers.Count > 0) && DBPlayers.ContainsKey(PlayerName))
                    {
                        PlayerCacheData.Clear();
                        var CurPlayer = DBPlayers[PlayerName];
                        CurPlayer.Pos = CurPlayerPos;
                        CurPlayer.LastUpdateTime = DateTime.Now;
                        if (CurPlayer.FarUnits.Count > 0)
                        {
                            Parallel.ForEach(CurPlayer.FarUnits, new ParallelOptions { MaxDegreeOfParallelism = CurPlayer.FarUnits.Count }, FarUnit =>
                            {
                                if (!CurPlayer.OldFarUnitNames.Contains(FarUnit))
                                {
                                    lock (PlayerCacheDataLock)
                                    {
                                        PlayerCacheData.Append("'" + FarUnit.Name + "';");
                                    }
                                }
                            });
                        }
                        if (PlayerCacheData.Length > 0)
                        {
                            PlayerCacheData.Remove(PlayerCacheData.Length - 1, 1);
                        }
                        PlayerCacheData.Append("|");
                        if (CurPlayer.NearUnits.Count > 0)
                        {
                            Parallel.ForEach(CurPlayer.NearUnits, new ParallelOptions { MaxDegreeOfParallelism = CurPlayer.NearUnits.Count }, NearUnit =>
                            {
                                if (!CurPlayer.OldNearUnitNames.Contains(NearUnit))
                                {
                                    lock (PlayerCacheDataLock)
                                    {
                                        PlayerCacheData.Append("'" + NearUnit.Name + "';");
                                    }
                                }
                            });
                        }
                        if (PlayerCacheData.ToString().EndsWith(";"))
                        {
                            PlayerCacheData.Remove(PlayerCacheData.Length - 1, 1);
                        }
                        CurPlayer.OldFarUnitNames = new HashSet<ArmAObject>(CurPlayer.FarUnits);
                        CurPlayer.OldNearUnitNames = new HashSet<ArmAObject>(CurPlayer.NearUnits);
                        CurPerfMonitor.AddOrUpdatePlayerCacheStats(CurPlayer.Name, CurPlayer.FarUnits.Count, CurPlayer.NearUnits.Count);
                    }
                    else
                    {
                        DBPlayers.Add(PlayerName, new ArmAPlayer(PlayerName, CurPlayerPos));
                    }
                }
                return PlayerCacheData.ToString();//'NewFarUnit1';...|'NewNearUnit1';...
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
                return "";
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ManagePlayerCacheData()
        {
            try
            {
                while (Idle || (DateTime.Now < LastUpdateTime.AddSeconds(20)))
                {
                    lock (DBPlayersLock)
                    {
                        if (DBPlayers.Count > 0)
                        {
                            Parallel.ForEach(DBPlayers, new ParallelOptions { MaxDegreeOfParallelism = DBPlayers.Count }, Player =>
                            {
                                Player.Value.NearUnits.Clear();
                                Player.Value.FarUnits.Clear();
                                foreach (var Player2 in DBPlayers)
                                {
                                    if (!Player.Value.Name.Equals(Player2.Value.Name))
                                    {
                                        if (((int)Math.Round(Math.Sqrt(Player.Value.Pos.Zip(Player2.Value.Pos, (a, b) => (a - b) * (a - b)).Sum()), 0)) < CacheDistance)
                                        {
                                            Player.Value.NearUnits.Add(Player2.Value);
                                        }
                                        else
                                        {
                                            Player.Value.FarUnits.Add(Player2.Value);
                                        }
                                    }
                                }
                                lock (DBObjectsLock)
                                {
                                    if (DBObjects.Count > 0)
                                    {
                                        foreach (var Object in DBObjects)
                                        {
                                            if (((int)Math.Round(Math.Sqrt(Player.Value.Pos.Zip(Object.Value.Pos, (a, b) => (a - b) * (a - b)).Sum()), 0)) < CacheDistance)
                                            {
                                                Player.Value.NearUnits.Add(Object.Value);
                                            }
                                            else
                                            {
                                                Player.Value.FarUnits.Add(Object.Value);
                                            }
                                        }
                                    }
                                }
                                lock (DBEnemysLock)
                                {
                                    if (DBEnemys.Count > 0)
                                    {
                                        foreach (var Enemy in DBEnemys)
                                        {
                                            if (((int)Math.Round(Math.Sqrt(Player.Value.Pos.Zip(Enemy.Value.Pos, (a, b) => (a - b) * (a - b)).Sum()), 0)) < CacheDistance)
                                            {
                                                Player.Value.NearUnits.Add(Enemy.Value);
                                            }
                                            else
                                            {
                                                Player.Value.FarUnits.Add(Enemy.Value);
                                            }
                                        }
                                    }
                                }
                                lock (DBCitysLock)
                                {
                                    foreach (var City in DBCitys)
                                    {
                                        lock (City.Value.LockThis)
                                        {
                                            foreach (var Civ in City.Value.Civs)
                                            {
                                                if (!ReferenceEquals(Civ.Value.Name, null))
                                                {
                                                    if (((int)Math.Round(Math.Sqrt(Player.Value.Pos.Zip(Civ.Value.Pos, (a, b) => (a - b) * (a - b)).Sum()), 0)) < CivCacheDistance)
                                                    {
                                                        Player.Value.NearUnits.Add(Civ.Value);
                                                    }
                                                    else
                                                    {
                                                        Player.Value.FarUnits.Add(Civ.Value);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            });
                        }
                    }
                    Thread.Sleep(1800);
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void GarbageCollector()
        {
            try
            {
                var LockThis = new Object();
                var KeysToDelete = new HashSet<string>();
                while (Idle || (DateTime.Now < LastUpdateTime.AddSeconds(20)))
                {
                    lock (DBPlayersLock)
                    {
                        if (DBPlayers.Count > 0)
                        {
                            KeysToDelete.Clear();
                            Parallel.ForEach(DBPlayers, new ParallelOptions { MaxDegreeOfParallelism = DBPlayers.Count }, Player =>
                            {
                                if (DateTime.Now > Player.Value.LastUpdateTime.AddSeconds(30))
                                {//Spieler löschen sich selbst im Spiel
                                    lock (LockThis)
                                    {
                                        KeysToDelete.Add(Player.Key);
                                    }
                                }
                            });
                            foreach (var Key in KeysToDelete)
                            {
                                DBPlayers.Remove(Key);
                            }
                        }
                    }
                    lock (DBObjectsLock)
                    {
                        if (DBObjects.Count > 0)
                        {
                            Parallel.ForEach(DBObjects, new ParallelOptions { MaxDegreeOfParallelism = DBObjects.Count }, Object =>
                            {
                                if (!Object.Value.Destroyed && (DateTime.Now > Object.Value.LastUpdateTime.AddSeconds(181)))
                                {
                                    Object.Value.Destroyed = true;
                                    Object.Value.DestroyedTime = Object.Value.LastUpdateTime;
                                }
                            });
                        }
                    }
                    lock (DBEnemysLock)
                    {
                        if (DBEnemys.Count > 0)
                        {
                            Parallel.ForEach(DBEnemys, new ParallelOptions { MaxDegreeOfParallelism = DBEnemys.Count }, Enemy =>
                            {
                                if (!Enemy.Value.Destroyed && (DateTime.Now > Enemy.Value.LastUpdateTime.AddSeconds(181)))
                                {
                                    Enemy.Value.Destroyed = true;
                                    Enemy.Value.DestroyedTime = Enemy.Value.LastUpdateTime;
                                }
                            });
                        }
                    }
                    lock (DBCitysLock)
                    {
                        if (DBCitys.Count > 0)
                        {
                            Parallel.ForEach(DBCitys, new ParallelOptions { MaxDegreeOfParallelism = DBCitys.Count }, City =>
                            {
                                lock (City.Value.LockThis)
                                {
                                    if (City.Value.Civs.Count > 0)
                                    {
                                        Parallel.ForEach(City.Value.Civs, new ParallelOptions { MaxDegreeOfParallelism = City.Value.Civs.Count }, Civ =>
                                        {
                                            if (!ReferenceEquals(Civ.Value.Name, null))
                                            {
                                                if (!Civ.Value.Destroyed && (DateTime.Now > Civ.Value.LastUpdateTime.AddSeconds(30)))
                                                {
                                                    Civ.Value.Destroyed = true;
                                                    Civ.Value.DestroyedTime = Civ.Value.LastUpdateTime;
                                                }
                                            }
                                        });
                                    }
                                }
                            });
                        }
                    }
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int[] StringToPos(string PosString)
        {
            try
            {
                var CurRAWPos = PosString.Remove(0, 1).Remove(PosString.Length - 2, 1).Split(',');
                return new int[3] { Convert.ToInt32(CurRAWPos[0].Split('.')[0]), Convert.ToInt32(CurRAWPos[1].Split('.')[0]), Convert.ToInt32(CurRAWPos[2].Split('.')[0]) };
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
                return new int[3] { 0, 0, 0 };
            }
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string PosToString(float[] Pos)
        {
            return ("[" + Pos[0].ToString().Replace(",", ".") + "," + Pos[1].ToString().Replace(",", ".") + "," + Pos[2].ToString().Replace(",", ".") + "]");
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private struct SnapShotStruct
        {
            public string RAWText;
            public List<object> Results;
            public float Counter;
            public bool CurrentObjectBool;
            public string CurrentObject;
            public int stage;
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private List<object> GetNETObjectsWithInt(string RAWText)
        {
            var Results = new List<object>();
            var snapshotStack = new Stack<SnapShotStruct>();
            var currentSnapshot = new SnapShotStruct();
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
                        else if (currentSnapshot.RAWText[i].Equals(']'))
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
                                else if (float.TryParse(currentSnapshot.CurrentObject.Split('.')[0], out currentSnapshot.Counter))
                                {
                                    currentSnapshot.Results.Add((int)Math.Round(currentSnapshot.Counter, 0));
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
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private List<object> GetNETObjectsWithFloat(string RAWText)
        {
            var Results = new List<object>();
            var snapshotStack = new Stack<SnapShotStruct>();
            var currentSnapshot = new SnapShotStruct();
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
                        else if (currentSnapshot.RAWText[i].Equals(']'))
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
                                else if (float.TryParse(currentSnapshot.CurrentObject.Split('.')[0], out currentSnapshot.Counter))
                                {
                                    if (currentSnapshot.CurrentObject.Split('.').Length > 1)
                                    {
                                        if (currentSnapshot.CurrentObject.Split('.')[1].Length > 4)
                                        {
                                            currentSnapshot.Counter += Convert.ToSingle("0," + currentSnapshot.CurrentObject.Split('.')[1].Substring(0, 4));
                                        }
                                        else
                                        {
                                            currentSnapshot.Counter = Convert.ToSingle(currentSnapshot.CurrentObject.Replace(".", ","));
                                        }
                                    }
                                    currentSnapshot.Results.Add((float)Math.Round(currentSnapshot.Counter, 4));
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
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        internal bool WriteErrorLog(string Error)
        {
            try
            {
                lock (LogWriterLock)
                {
                    var ErrorLogPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "HandleServer_Standalone.Error.log");
                    File.AppendAllText(ErrorLogPath, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + " - " + Error + Environment.NewLine);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        internal string CheckOutputLength(string Output)
        {
            if (Output.Length < 10239)
            {
                return Output;
            }
            else
            {
                WriteErrorLog("Output to big: " + Output);
            }
            return string.Empty;
        }
    }

    internal sealed class ArmACity
    {
        internal ArmACity(int CivCount, float[][] CivSpawnPositions)
        {
            this.CivCount = CivCount;
            this.CivSpawnPositions = CivSpawnPositions;
            this.Civs = new Dictionary<string, ArmACiv>();
            this.Rand = new Random();
            this.LockThis = new object();
        }

        internal bool AddCivs()
        {
            for (int i = 0; i < CivCount; i++)
            {
                this.Civs.Add(i.ToString(), new ArmACiv(i.ToString(), null, CivSpawnPositions[Rand.Next(0, CivSpawnPositions.Length)]));
            }
            return true;
        }
        internal Dictionary<string, ArmACiv> Civs { get; set; }
        internal int CivCount { get; set; }
        internal float[][] CivSpawnPositions { get; set; }
        internal Random Rand { get; set; }
        internal object LockThis { get; set; }
    }

    internal class ArmAObject
    {
        internal ArmAObject(string Name, int[] Pos)
        {
            this.Name = Name;
            this.Pos = Pos;
            this.Destroyed = false;
            this.DestroyedTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        internal string Name { get; set; }
        internal int[] Pos { get; set; }
        internal DateTime DestroyedTime { get; set; }
        internal bool Destroyed { get; set; }
        internal DateTime LastUpdateTime { get; set; }
    }

    internal sealed class ArmAPlayer : ArmAObject
    {
        internal ArmAPlayer(string Name, int[] Pos) : base(Name, Pos)
        {
            this.Name = Name;
            this.Pos = Pos;
            this.Destroyed = false;
            this.LockThis = new object();
            this.NearUnits = new HashSet<ArmAObject>();
            this.FarUnits = new HashSet<ArmAObject>();
            this.OldNearUnitNames = new HashSet<ArmAObject>();
            this.OldFarUnitNames = new HashSet<ArmAObject>();
        }

        internal HashSet<ArmAObject> NearUnits { get; set; }
        internal HashSet<ArmAObject> FarUnits { get; set; }
        internal HashSet<ArmAObject> OldNearUnitNames { get; set; }
        internal HashSet<ArmAObject> OldFarUnitNames { get; set; }
        internal object LockThis { get; set; }
    }

    internal sealed class ArmACiv : ArmAObject
    {
        internal ArmACiv(string Name, int[] Pos, float[] SpawnPos) : base(Name, Pos)
        {
            this.Deactivated = false;
            this.Name = null;
            this.SpawnPos = SpawnPos;
            this.Destroyed = false;
        }

        internal DateTime LastWalkTime { get; set; }
        internal float[] GetNewWayPos(float[][] WayPos, Random Rand)
        {
            return WayPos[Rand.Next(0, WayPos.Length)];
        }
        internal float[] SpawnPos { get; set; }
        internal bool Deactivated { get; set; }
        internal void Reset()
        {
            this.Deactivated = false;
            this.Name = null;
            this.Destroyed = false;
        }
    }
}
