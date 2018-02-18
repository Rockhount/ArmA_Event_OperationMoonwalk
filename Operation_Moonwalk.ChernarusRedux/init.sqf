//Original Design by Xeno.
//Design-Adaptation by [AIG]Tartar, Rockhount.
//Story by [AIG]Atze Peng.
//Loadouts by [AIG]MightyBullet.
//Mission-Technology by Rockhount

diag_log "################################## Operation Moonwalk ##################################";


Global_fnc_AddTasks =
{
	if (_Local_var_DebugLevel != 1) then
	{
		Player createDiaryRecord ["Diary", ["Credits", "Mission-Scripts + DLL's by Rockhount.<br/>Mission-Design by [AIG]Tartar, Rockhount and Xeno.<br/>Story by [AIG]Atze Peng.<br/>Loadouts by [AIG]MightyBullet.<br/>Die Mission ist frei erfunden und hat so nicht stattgefunden.<br/><br/>Viel Glück und viel Spaß."]];
		Player createDiaryRecord ["Diary", ["Technisches", "Medical System · ACE3!<br/><br/>Persöhnliche Anpassung der Helligkeit und Gamma ist erlaubt.<br/><br/>Durch die interne Limitierung der Lichter-Anzahl in ArmA 3, kann es vorkommen, dass das Umgebungslicht der Knicklichter ab und zu erlischt.<br/><br/>Damit Knicklichter auch auf große Entfernungen sichtbar sind, muss man unter 'Esc-Optionen-Grafik-Allgemein-Licht-Dynam. Licht' auf Ultra stellen<br/><br/>Keine Nachtsichtgeräte verfügbar!<br/><br/>Die OPZ entscheidet was wann wo und wie geschieht."]];
		Player createDiaryRecord ["Diary", ["Auftrag", "Auftrag:<br/><br/>In eine aufeinander abgestimmten Luftlandeoperation werden zunächst folgende Ziele ausgeschaltet:<br/><br/>- Alpha-Squad: Absprungzone Pulkowo, Ziel Treibstofflager Rogowo<br/>- Bravo-Squad: Absprungzone Moglievka, Ziel Fahrzeugdepot Vyshnoye<br/>- Charlie-Squad: Absprungzone Guglovo, Ziel Radarstation Novy Sobor<br/>- Delta-Squad: Absprungzone südwestlich von Gorka, Ziel Artilleriestellung 'Altes Feld'<br/><br/>Danach wird gemeinsam gegen die eigentlichen Hauptmissionziele Hauptquartier und Flughafen vorgegangen.<br/><br/>Sie beginnen mit der Operation auf der U.S.S. Theodore Roosevelt, die sich gerade 'zufällig' zum testen der neuen F-22 Raptor Kampfjets in der Region befindet. Da es sich um eine Kommandoaktion handelt und ein größeres amerikanisches Militäraufkommen unbedingt vermieden werden muss, sind Sie nach dem Absprung auf sich gestellt. Sollten Sie Nachschub benötigen, müssen Sie ihn sich vor Ort selbst beschaffen. Durch die seit einem Jahr laufende UNOSOM Mission in Somalia, gibt es außerdem einen Engpass an Nachtsichtgeräten. Nach Rücksprache mit Ihrer OPZ wurde dieses Problem aber als akzeptabel eingestuft.<br/><br/>Nach erfolgreichem Abschluß der Operation übernimmt die dann überlegene FAC (Freie Armee Chernarus) und Sie verlassen in kleinen Gruppen, getarnt als Austauschstudenten, das Land in Richtung Westen.<br/><br/>Viel Erfolg!"]];
		Player createDiaryRecord ["Diary", ["Lage","Chernarus, Juli 1993<br/><br/>Lage:<br/><br/>Nach dem Zerfall der Sowjetunion bleibt die Lage unübersichtlich. Viele der ehmaligen Teilrepubliken erklären sich für unabhängig. Einige treten der GUS (Gemeinschaft unabhängiger Staaten) bei, andere wollen sich dem Einfluss Russlands entziehen und streben eine engere Anbindung an den 'Westen' an. Da Russland nicht jede Unabhängigkeitserklärung annerkennt, übt es einen starken Druck politisch, wirtschaftlich als auch militärisch auf die Satellitenstaaten seines Einflußbereichs aus.<br/><br/>Hier beginnt das Problem der freien und unabhängigen jungen Republik Chernarus.<br/><br/>Wie in vielen ehemaligen Teilrepubliken existieren auch in Chernarus noch Kasernen, Flugplätze und andere militärische Einrichtungen der Roten Armee. Da die Mehrzahl der dort stationierten Soldaten aus Russland stammt, hat die frisch gewählte demokratische Regierung von Chernarus ein Problem unabhängig und selbstbestimmt zu handeln. Nachdem der Besuch amerikanischer 'Sonderbotschafter' im Präsidentenpalast von Chernogorsk bekannt wurde, scheint eine militärische Intervention Russlands immer wahrscheinlicher. Da die FAC (Freie Armee Chernarus) nicht in der Lage ist etwas gegen die Überzahl der russlandtreuen Reste der Roten Armee zu unternehmen, wurde im Rahmen der amerikanischen 'Hilfe zur Selbsthilfe' folgender Plan entwickelt:<br/><br/>Um die militärische Dominanz zu brechen und eine russische Intervention auszuschließen, muss der unter russischem Einfluß verbliebene Flughafen als auch das russlandtreue Hauptquartier unter die Kontrolle der Streitkräfte von Chernarus gebracht werden. Um diese beiden Hauptmissionsziele zu verschleiern und um ein Maximum an Verwirrung zu stiften, werden zuvor kleinere Ziele ausgeschaltet, die im Falle einer russischen Intervention von erheblicher militärischer Bedeutung wären."]];

		Global_var_Task1 = Player createSimpleTask ["Treibstofflager."];
		Global_var_Task2 = Player createSimpleTask ["Gepanzerte Fahrzeuge."];
		Global_var_Task3 = Player createSimpleTask ["Radar."];
		Global_var_Task4 = Player createSimpleTask ["Artillerie."];
		Global_var_Task5 = Player createSimpleTask ["Russisches HQ."];
		Global_var_Task6 = Player createSimpleTask ["Luftstützpunkt."];
		for "_i" from 1 to 6 do
		{
			call compile format[
			"
				Global_var_Task%1 setTaskState 'Created';
			", _i];
		};
		Global_var_Task1 setSimpleTaskDestination getMarkerPos "fuel_dump_target";
		Global_var_Task2 setSimpleTaskDestination getMarkerPos "tanks_target";
		Global_var_Task3 setSimpleTaskDestination getMarkerPos "radar_target";
		Global_var_Task4 setSimpleTaskDestination getMarkerPos "artillery_target";
		Global_var_Task1 setSimpleTaskDescription ["Alpha, finden und zerstören Sie <marker name=""fuel_dump_target"">die Treibstoffreserven</marker> in Rogovo.","Treibstoffreserven zerstören","Treibstoffreserven zerstört"];
		Global_var_Task2 setSimpleTaskDescription ["Bravo, zerstören Sie die Fahrzeuge im <marker name=""tanks_target"">Fahrzeugdepot</marker> in der Nähe von Vyshnoye, damit sie keine Gefahr mehr für die FAC darstellen.","Fahrzeugdepot zerstören.","Fahrzeugdepot zerstört"];
		Global_var_Task3 setSimpleTaskDescription ["Charlie, zerstören Sie die Radarstellung in <marker name=""radar_target"">Novy Sobor</marker>, damit die FAC die Lufthoheit erringen kann.","Radar zerstören","Radar zerstört"];
		Global_var_Task4 setSimpleTaskDescription ["Delta, Ihre Aufgabe ist die Artillerie-Stellung <marker name=""artillery_target"">an der uns bekannten Position</marker> zu zerstören.","Artillerie-Stellung zerstören","Artillerie-Stellung zerstört"];
		Global_var_Task5 setSimpleTaskDescription ["Bündeln Sie Ihre Kräfte und greifen Sie gemeinsam <marker name=""hq_target"">das russiche HQ</marker> und die Umgebung in Stary Sobor an.","HQ angreifen","HQ gesäubert"];
		Global_var_Task6 setSimpleTaskDescription ["Unser letztes Ziel ist der <marker name=""airbase_target"">Luftstützpunkt</marker> nördlich von Vybor. Um eine Intervention Ruslands zu verhindern, muss dieser Flughafen eingenommen werden. Zerstören Sie zusätzlich alle Flugzeuge und Helikopter, die Sie dort finden können.","Luftstützpunkt angreifen","Luftstützpunkt gesäubert"];
	}
	else
	{
		Global_var_Task1 = Player createSimpleTask ["1"];
		Global_var_Task2 = Player createSimpleTask ["2"];
		Global_var_Task3 = Player createSimpleTask ["3"];
		Global_var_Task4 = Player createSimpleTask ["4"];
		Global_var_Task5 = Player createSimpleTask ["5"];
		Global_var_Task6 = Player createSimpleTask ["6"];
	};
};
if (!isServer && {hasInterface}) then
{
	private _Local_var_DebugLevel = ["DebugLevel", 0] call BIS_fnc_getParamValue;
	"hq_target" setMarkerAlphaLocal 0;
	"airbase_target" setMarkerAlphaLocal 0;
	"LZ_5" setMarkerAlphaLocal 0;
	"charlie_dir2" setMarkerAlphaLocal 0;
	if (!didJIP) then
	{
		call Global_fnc_AddTasks;
		Player linkItem "ItemMap";
	};
};

Global_var_CacheDistance = 1000;
Global_var_CreateCivDistance = 350;
Global_var_ServerIP = "127.0.0.1";
Global_var_ServerIP4HC = "127.0.0.1";
Global_var_ServerIP4Player = "127.0.0.1";
Global_var_UMissionID = "ab1c...";

[0.35] call ACRE_api_fnc_setLossModelScale;
[true] call ACRE_api_fnc_setFullDuplex;
[false] call ACRE_api_fnc_setRevealToAi;
[false] call ACRE_api_fnc_setInterference;
[true] call ACRE_api_fnc_ignoreAntennaDirection;

if !(call compile preprocessFileLineNumbers "HandleDLLInit.sqf") exitWith {};
if (isDedicated) then
{
	"HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["Reset",Global_var_CacheDistance,Global_var_CreateCivDistance,60]);
};
sleep 1;
if (!isNil "Public_fnc_ShowDLLMissing") exitWith
{
	switch (Public_fnc_ShowDLLMissing) do
	{
		case 0:
		{
			diag_log "HandleServer.dll nicht geladen oder verbunden";
			endMission "End1";
		};
		case 1:
		{
			diag_log "HandleHC.dll nicht geladen";
			endMission "End2";
		};
	};
};
if (!isServer && {hasInterface}) then
{
	private _Local_var_Result = "HandleClient" callExtension  str ["Reset",Global_var_ServerIP4Player,Global_var_UMissionID];
	if ((!isNil "_Local_var_Result") && {_Local_var_Result != ""}) then
	{
		diag_log "Es wurden falsche Parameter an die Client DLL übergeben.";
		diag_log _Local_var_Result;
		endMission "End4";
	};
};
setViewDistance 1000;
setObjectViewDistance 900;
setterraingrid 25;
enableSaving [false,false];
enableDynamicSimulationSystem false;

Global_var_LoopCount = 4;
Public_var_TaskDone = -1;
Global_var_CivilClasses = ["C_Man_casual_1_F_euro","C_Man_casual_2_F_euro","C_Man_casual_3_F_euro","C_Man_Fisherman_01_F","C_man_hunter_1_F", "C_Man_Messenger_01_F"];
execFSM "HandleAIComA3.fsm";
execFSM "HCManagerA3.fsm";
execFSM "HandleDeleteA3.fsm";

execFSM "Ammo\init.fsm";
(1200) execFSM "HandleWeaponCleanUpA3.fsm";

Global_fnc_AttackPos =
{
	Params["_Local_var_Group","_Local_var_TargetPos"];
	{
		_x doMove _Local_var_TargetPos;
		_x setSpeedMode "NORMAL";
		_x setBehaviour "COMBAT";
		_x setCombatMode "RED";
	} forEach units _Local_var_Group;
};
Global_fnc_AddWPs =
{
	Private ["_Local_var_CurWP"];
	Params ["_Local_var_Group", "_Local_var_WPPositions"];
	{
		_Local_var_CurWP = _Local_var_Group addWaypoint [_x, 0];
		_Local_var_CurWP setWaypointBehaviour "COMBAT";
		_Local_var_CurWP setWaypointCombatMode "RED";
		_Local_var_CurWP setWaypointCompletionRadius 50;
		_Local_var_CurWP setWaypointFormation "NO CHANGE";
		_Local_var_CurWP setWaypointSpeed "NORMAL";
		_Local_var_CurWP setWaypointType "MOVE";
		if (_forEachIndex == 0) then
		{
			_Local_var_Group setCurrentWaypoint _Local_var_CurWP;
		};
	} forEach _Local_var_WPPositions;
	_Local_var_CurWP = _Local_var_Group addWaypoint [_Local_var_WPPositions select 0, 0];
	_Local_var_CurWP setWaypointBehaviour "COMBAT";
	_Local_var_CurWP setWaypointCombatMode "RED";
	_Local_var_CurWP setWaypointFormation "NO CHANGE";
	_Local_var_CurWP setWaypointSpeed "NORMAL";
	_Local_var_CurWP setWaypointType "CYCLE";
};
Global_fnc_Patrol =
{
	Params ["_Local_var_Objects", "_Local_var_Pos", "_Local_var_Radius"];
	[_Local_var_Objects, _Local_var_Pos, _Local_var_Radius, 7, "MOVE", "COMBAT", "RED"] call CBA_fnc_taskPatrol;
	if ((_Local_var_Pos select 2) < -1) then
	{
		{
			(waypointPosition _x) Params ["_Local_var_CurWayPosX","_Local_var_CurWayPosY","_Local_var_CurWayPosZ"];
			_x setWaypointPosition [[_Local_var_CurWayPosX, _Local_var_CurWayPosY, _Local_var_Pos select 2], 0];
		} forEach (waypoints _Local_var_Objects);
		if ((typeName _Local_var_Objects) == "GROUP") then
		{
			{
				_x swimInDepth (_Local_var_Pos select 2);
			} forEach (units _Local_var_Objects);
		}
		else
		{
			_Local_var_Objects swimInDepth (_Local_var_Pos select 2);
		};
	};
};
Global_fnc_SpawnVehicle =
{
	Params ["_Local_var_Pos", "_Local_var_Type", "_Local_var_Side"];
	private _Local_var_Group = createGroup _Local_var_Side;
	private _Local_var_Vehicle = createVehicle [_Local_var_Type, [0,0,0], [], 0, if (_Local_var_Type isKindOf "Air") then {"FLY"} else {"NONE"}];
	_Local_var_Vehicle setPosATL _Local_var_Pos;
	private _Local_var_CurSeatCount = _Local_var_Vehicle emptyPositions "Commander";
	if (_Local_var_CurSeatCount > 0) then
	{
		private _Local_var_CurUnit = _Local_var_Group createUnit ["rhs_vdv_des_rifleman", [0,0,0], [], 0, "NONE"];
		_Local_var_CurUnit assignAsCommander _Local_var_Vehicle;
		_Local_var_CurUnit moveInCommander _Local_var_Vehicle;
		Global_var_WatchingGunners pushBack _Local_var_CurUnit;
	};
	_Local_var_CurSeatCount = _Local_var_Vehicle emptyPositions "Driver";
	if (_Local_var_CurSeatCount > 0) then
	{
		private _Local_var_CurUnit = _Local_var_Group createUnit ["rhs_vdv_des_rifleman", [0,0,0], [], 0, "NONE"];
		_Local_var_CurUnit assignAsDriver _Local_var_Vehicle;
		_Local_var_CurUnit moveInDriver _Local_var_Vehicle;
	};
	_Local_var_CurSeatCount = _Local_var_Vehicle emptyPositions "Gunner";
	if (_Local_var_CurSeatCount > 0) then
	{
		private _Local_var_CurUnit = _Local_var_Group createUnit ["rhs_vdv_des_rifleman", [0,0,0], [], 0, "NONE"];
		_Local_var_CurUnit assignAsGunner _Local_var_Vehicle;
		_Local_var_CurUnit moveInGunner _Local_var_Vehicle;
		Global_var_WatchingGunners pushBack _Local_var_CurUnit;
	};
	[_Local_var_Vehicle,_Local_var_Group]
};
if (isDedicated) then
{
	Global_var_AllTaskMessages = [];
	"Public_fnc_AddTaskID" addPublicVariableEventHandler
	{
		if !((_this select 1) in Global_var_AllTaskMessages) then
		{
			(_this select 1) spawn
			{
				Public_var_TaskDone = _this;
				Global_var_AllTaskMessages pushBack _this;
				publicVariable "Public_var_TaskDone";
			};
		};
	};
	"Public_fnc_GetJIPInfos" addPublicVariableEventHandler
	{
		(_this select 1) spawn
		{
			{
				Public_var_TaskDone = _x;
				(owner _this) publicVariableClient "Public_var_TaskDone";
			} forEach Global_var_AllTaskMessages;
		};
	};
	"Init" execFSM "HandleChemLight_New.fsm";
	Global_var_FPS = 0;
	[{Global_var_FPS = Global_var_FPS + 1;}, 0, 0] call CBA_fnc_addPerFrameHandler;
	[{diag_log format["--Server-- Einheiten: %1 DavonLokalOhneZivs: %2 Spieler: %3 Objekte: %4 FPS: %5 Schleifen: %6 Skripte: %7", count allUnits, {if ((local _x) && {side _x != civilian}) then {true} else {false}} count allUnits, count allPlayers, count vehicles, Global_var_FPS, Global_var_LoopCount,diag_activeScripts];Global_var_FPS = 0;}, 1, 0] call CBA_fnc_addPerFrameHandler;
//------------------------------------------------------------------------------------------------------------------------------
	for "_i" from 1 to 22 do
	{
		call compile format [
		"
			EnemyVehicle%1 lock 2;
			EnemyVehicle%1 setDammage 0.5;
		", _i];
	};
	for "_i" from 1 to 12 do
	{
		call compile format [
		"
			clearmagazinecargoGlobal SatchelBox%1;
			SatchelBox%1 addMagazineCargoGlobal ['CUP_PipeBomb_M', 5];
		", _i];
	};
	 //Task1
	 Global_var_Task1Hits = 0;
	{
		_x addEventHandler
		[
			"killed",
			{
				Global_var_Task1Hits = Global_var_Task1Hits + 1;
				"HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["AddObjectToDelete",0,netId (_this select 0)]);
				if (Global_var_Task1Hits == 3) then
				{
					Public_var_TaskDone = 1;
					Global_var_AllTaskMessages pushBack 1;
					publicVariable "Public_var_TaskDone";
				};
			}
		];
	} forEach [EnemyVehicle5,EnemyVehicle6,EnemyVehicle7];
	 //Task2
	 Global_var_Task2Hits = 0;
	{
		_x addEventHandler
		[
			"killed",
			{
				Global_var_Task2Hits = Global_var_Task2Hits + 1;
				"HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["AddObjectToDelete",0,netId (_this select 0)]);
				if (Global_var_Task2Hits == 4) then
				{
					Public_var_TaskDone = 2;
					Global_var_AllTaskMessages pushBack 2;
					publicVariable "Public_var_TaskDone";
				};
			}
		];
	} forEach [EnemyVehicle1,EnemyVehicle2,EnemyVehicle3,EnemyVehicle4];
	 //Task3
	EnemyVehicle8 addEventHandler
	[
		"killed",
		{
			"HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["AddObjectToDelete",0,netId (_this select 0)]);
			Public_var_TaskDone = 3;
			Global_var_AllTaskMessages pushBack 3;
			publicVariable "Public_var_TaskDone";
		}
	];
	 //Task4
	 Global_var_Task4Hits = 0;
	{
		_x addEventHandler
		[
			"killed",
			{
				Global_var_Task4Hits = Global_var_Task4Hits + 1;
				"HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["AddObjectToDelete",0,netId (_this select 0)]);
				if (Global_var_Task4Hits == 4) then
				{
					Public_var_TaskDone = 4;
					Global_var_AllTaskMessages pushBack 4;
					publicVariable "Public_var_TaskDone";
				};
			}
		];
	} forEach [EnemyVehicle9,EnemyVehicle10,EnemyVehicle11,EnemyVehicle12];
	 //Task5
	 0 spawn
	 {
		private _Local_fnc_IsClearOfEnemy =
		{
			Params ["_Local_var_Pos","_Local_var_Radius","_Local_var_CountToTrigger"];
			private _Local_var_Result = call compile ("HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["CountEnemyInArea",_Local_var_Pos,_Local_var_Radius]));
			if (typeName _Local_var_Result == "SCALAR") then
			{
				if (_Local_var_Result <= _Local_var_CountToTrigger) then
				{
					true
				}
				else
				{
					false
				}
			};
		};
		Global_var_LoopCount = Global_var_LoopCount + 1;
		waitUntil{sleep 30;!([[6176,7735,0], 700, 8] call _Local_fnc_IsClearOfEnemy)};
		sleep 600;
		waitUntil{sleep 30;[[6176,7735,0], 700, 8] call _Local_fnc_IsClearOfEnemy};
		Global_var_LoopCount = Global_var_LoopCount - 1;
		Public_var_TaskDone = 5;
		Global_var_AllTaskMessages pushBack 5;
		publicVariable "Public_var_TaskDone";
	 };
	 //Task6
	 Global_var_Task6Hits = 0;
	{
		_x addEventHandler
		[
			"killed",
			{
				Global_var_Task6Hits = Global_var_Task6Hits + 1;
				"HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["AddObjectToDelete",0,netId (_this select 0)]);
				if (Global_var_Task6Hits == 10) then
				{
					Public_var_TaskDone = 6;
					Global_var_AllTaskMessages pushBack 6;
					publicVariable "Public_var_TaskDone";
				};
			}
		];
	} forEach [EnemyVehicle13,EnemyVehicle14,EnemyVehicle15,EnemyVehicle16,EnemyVehicle17,EnemyVehicle18,EnemyVehicle19,EnemyVehicle20,EnemyVehicle21,EnemyVehicle22];
//------------------------------------------------------------------------------------------------------------------------------
	Global_fnc_CreateGroup =
	{
		Private ["_Local_var_TargetPos", "_Local_var_Radius", "_Local_var_WayPoints"];
		Params ["", "_Local_var_Pos", "_Local_var_UnitTypes"];
		switch (_this select 0) do
		{//0=Static, 1=Patrol, 2=WayPoints
			case 1:
			{
				_Local_var_TargetPos = _this select 3;
				_Local_var_Radius = _this select 4;
			};
			case 2:
			{
				_Local_var_WayPoints = _this select 3;
			};
		};
		if (count Global_var_HCIDs > 0) then
		{
			Public_fnc_HCTask = [];
			switch (_this select 0) do
			{//0=Static, 1=Patrol, 2=WayPoints
				case 0:
				{
					Public_fnc_HCTask = [0, _Local_var_Pos, east, 0.6, 0.8, _Local_var_UnitTypes, 0];
				};
				case 1:
				{
					Public_fnc_HCTask = [0, _Local_var_Pos, east, 0.6, 0.8, _Local_var_UnitTypes, 1, _Local_var_TargetPos, _Local_var_Radius];
				};
				case 2:
				{
					Public_fnc_HCTask = [0, _Local_var_Pos, east, 0.6, 0.8, _Local_var_UnitTypes, 2, _Local_var_WayPoints];
				};
			};
			(selectRandom Global_var_HCIDs) publicVariableClient "Public_fnc_HCTask";
		}
		else
		{
			private _Local_var_Group = [ _Local_var_Pos, east, _Local_var_UnitTypes, [], [],[0.6,0.8,0.7]] call BIS_fnc_spawnGroup;
			switch (_this select 0) do
			{//0=Static, 1=Patrol, 2=WayPoints
				case 0:
				{
					_Local_var_Group setBehaviour "AWARE";
					_Local_var_Group setCombatMode "RED";
					{
						_x setUnitPos "UP";
					} forEach units _Local_var_Group;
				};
				case 1:
				{
					(units _Local_var_Group) doMove _Local_var_TargetPos;
					[_Local_var_Group, _Local_var_TargetPos, _Local_var_Radius] call Global_fnc_Patrol;
				};
				case 2:
				{
					(units _Local_var_Group) doMove (_Local_var_WayPoints select 0);
					[_Local_var_Group, _Local_var_WayPoints] call Global_fnc_AddWPs;
				};
			};
			{
				_x addEventHandler ["killed", {"HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["AddObjectToDelete",1,netId (_this select 0)])}];
				_x setskill ["spotDistance", 1];
				_x setskill ["spotTime", 1];
				_x setskill ["courage", 1];
				_x setskill ["aimingAccuracy", 0.3];
				_x unlinkItem (hmd _x);
			} forEach units _Local_var_Group;
			_Local_var_Group deleteGroupWhenEmpty true;
			switch (_this select 0) do
			{//0=Static, 1=Patrol, 2=WayPoints
				case 0:
				{
					_Local_var_Group setVariable ["GroupCommands",
					"
						if (count Global_var_HCIDs > 0) then
						{
							Public_fnc_HCTask = [1, _this, 0];
							(selectRandom Global_var_HCIDs) publicVariableClient 'Public_fnc_HCTask';
						};
					", false];
				};
				case 1:
				{
					_Local_var_Group setVariable ["GroupCommands", format[
					"
						if (count Global_var_HCIDs > 0) then
						{
							Public_fnc_HCTask = [1, _this, 1, %1, %2];
							(selectRandom Global_var_HCIDs) publicVariableClient 'Public_fnc_HCTask';
						};
					", _Local_var_TargetPos, _Local_var_Radius], false];
				};
				case 2:
				{
					_Local_var_Group setVariable ["GroupCommands", format[
					"
						if (count Global_var_HCIDs > 0) then
						{
							Public_fnc_HCTask = [1, _this, 2, %1];
							(selectRandom Global_var_HCIDs) publicVariableClient 'Public_fnc_HCTask';
						};
					", _Local_var_WayPoints], false];
				};
			};
		};
	};
	Global_fnc_CreateVehicle =
	{
		Private ["_Local_var_Radius", "_Local_var_TargetPos", "_Local_var_Dir", "_Local_var_WPPositions"];
		Params ["", "_Local_var_Pos", "_Local_var_VehCount", "_Local_var_Type", "_Local_var_Delay"];
		for "_i" from 1 to _Local_var_VehCount do
		{
			if (count Global_var_HCIDs > 0) then
			{
				Public_fnc_HCTask = [];
				switch (_this select 0) do
				{//0=Static,1=Attack, 2=Patrol, 3=WayPoints
					case 0:
					{
						_Local_var_Dir = _this select 5;
						Public_fnc_HCTask = [2, _Local_var_Pos, east, _Local_var_Type, 0, _Local_var_Dir];
					};
					case 1:
					{
						_Local_var_TargetPos = _this select 5;
						Public_fnc_HCTask = [2, _Local_var_Pos, east, _Local_var_Type, 1, _Local_var_TargetPos];
					};
					case 2:
					{
						_Local_var_TargetPos = _this select 5;
						_Local_var_Radius = _this select 6;
						Public_fnc_HCTask = [2, _Local_var_Pos, east, _Local_var_Type, 2, _Local_var_TargetPos, _Local_var_Radius];
					};
					case 3:
					{
						_Local_var_WPPositions = _this select 5;
						Public_fnc_HCTask = [2, _Local_var_Pos, east, _Local_var_Type, 3, _Local_var_WPPositions];
					};
				};
				(selectRandom Global_var_HCIDs) publicVariableClient "Public_fnc_HCTask";
			}
			else
			{
				private _Local_var_Objects = [_Local_var_Pos, _Local_var_Type, EAST] call Global_fnc_SpawnVehicle;
				private _Local_var_Vehicle = _Local_var_Objects select 0;
				private _Local_var_Group = _Local_var_Objects select 1;
				_Local_var_Vehicle setVehicleLock "LOCKED";
				if (side _Local_var_Group != EAST) then
				{
					private _Local_var_NewGroup = createGroup EAST;
					(units _Local_var_Group) joinSilent _Local_var_NewGroup;
					deleteGroup _Local_var_Group;
					_Local_var_Group = _Local_var_NewGroup;
				};
				switch (_this select 0) do
				{//0=Static,1=Attack, 2=Patrol, 3=WayPoints
					case 0:
					{
						_Local_var_Dir = _this select 5;
						_Local_var_Vehicle setDir _Local_var_Dir;
						_Local_var_Vehicle setVectorUp (surfaceNormal _Local_var_Pos);
					};
					case 1:
					{
						_Local_var_TargetPos = _this select 5;
						[_Local_var_Group, _Local_var_TargetPos] call Global_fnc_AttackPos;
					};
					case 2:
					{
						_Local_var_TargetPos = _this select 5;
						_Local_var_Radius = _this select 6;
						(units _Local_var_Group) doMove _Local_var_TargetPos;
						[_Local_var_Group, _Local_var_TargetPos, _Local_var_Radius] call Global_fnc_Patrol;
					};
					case 3:
					{
						_Local_var_WPPositions = _this select 5;
						(units _Local_var_Group) doMove (_Local_var_WPPositions select 0);
						[_Local_var_Group, _Local_var_WPPositions] call Global_fnc_AddWPs;
					};
				};
				{
					_x addEventHandler ["killed", {"HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["AddObjectToDelete",1,netId (_this select 0)])}];
					_x setskill ["spotDistance", 1];
					_x setskill ["spotTime", 1];
					_x setskill ["courage", 1];
					_x setskill ["aimingAccuracy", 0.3];
					_x unlinkItem (hmd _x);
				} forEach units _Local_var_Group;
				_Local_var_Vehicle addEventHandler ["killed", {"HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["AddObjectToDelete",0,netId (_this select 0)])}];
				_Local_var_Group deleteGroupWhenEmpty true;
				switch (_this select 0) do
				{//0=Static,1=Attack, 2=Patrol, 3=WayPoints
					case 0:
					{
						_Local_var_Group setVariable ["GroupCommands",
						"
							Private ['_Local_var_Veh'];
							_Local_var_Veh = vehicle ((units _this) select 0);
							if (count Global_var_HCIDs > 0) then
							{
								Public_fnc_HCTask = [3, getPosATL _Local_var_Veh, side _this, typeOf _Local_var_Veh, damage _Local_var_Veh, getDir _Local_var_Veh, 0];
								{
									'HandleServer' callExtension (Global_var_ServerIP + '|' + Global_var_UMissionID + '|' + str ['DeleteObjects',1,netId _x]);
									deleteVehicle _x;
								} forEach units _this;
								deleteGroup _this;
								'HandleServer' callExtension (Global_var_ServerIP + '|' + Global_var_UMissionID + '|' + str ['DeleteObjects',0,netId _Local_var_Veh]);
								deleteVehicle _Local_var_Veh;
								(selectRandom Global_var_HCIDs) publicVariableClient 'Public_fnc_HCTask';
							};
						", false];
					};
					case 1:
					{
						_Local_var_Group setVariable ["GroupCommands", format[
						"
							Private ['_Local_var_Veh'];
							_Local_var_Veh = vehicle ((units _this) select 0);
							if (count Global_var_HCIDs > 0) then
							{
								Public_fnc_HCTask = [3, getPosATL _Local_var_Veh, side _this, typeOf _Local_var_Veh, damage _Local_var_Veh, getDir _Local_var_Veh, 1, vectorDir _Local_var_Veh, vectorUp _Local_var_Veh, %1];
								{
									'HandleServer' callExtension (Global_var_ServerIP + '|' + Global_var_UMissionID + '|' + str ['DeleteObjects',1,netId _x]);
									deleteVehicle _x;
								} forEach units _this;
								deleteGroup _this;
								'HandleServer' callExtension (Global_var_ServerIP + '|' + Global_var_UMissionID + '|' + str ['DeleteObjects',0,netId _Local_var_Veh]);
								deleteVehicle _Local_var_Veh;
								(selectRandom Global_var_HCIDs) publicVariableClient 'Public_fnc_HCTask';
							};
						", _Local_var_TargetPos], false];
					};
					case 2:
					{
						_Local_var_Group setVariable ["GroupCommands", format[
						"
							Private ['_Local_var_Veh'];
							_Local_var_Veh = vehicle ((units _this) select 0);
							if (count Global_var_HCIDs > 0) then
							{
								Public_fnc_HCTask = [3, getPosATL _Local_var_Veh, side _this, typeOf _Local_var_Veh, damage _Local_var_Veh, getDir _Local_var_Veh, 2, vectorDir _Local_var_Veh, vectorUp _Local_var_Veh, %1, %2];
								{
									'HandleServer' callExtension (Global_var_ServerIP + '|' + Global_var_UMissionID + '|' + str ['DeleteObjects',1,netId _x]);
									deleteVehicle _x;
								} forEach units _this;
								deleteGroup _this;
								'HandleServer' callExtension (Global_var_ServerIP + '|' + Global_var_UMissionID + '|' + str ['DeleteObjects',0,netId _Local_var_Veh]);
								deleteVehicle _Local_var_Veh;
								(selectRandom Global_var_HCIDs) publicVariableClient 'Public_fnc_HCTask';
							};
						", _Local_var_TargetPos, _Local_var_Radius], false];
					};
					case 3:
					{
						_Local_var_Group setVariable ["GroupCommands", format[
						"
							Private ['_Local_var_Veh'];
							_Local_var_Veh = vehicle ((units _this) select 0);
							if (count Global_var_HCIDs > 0) then
							{
								Public_fnc_HCTask = [3, getPosATL _Local_var_Veh, side _this, typeOf _Local_var_Veh, damage _Local_var_Veh, getDir _Local_var_Veh, 3, vectorDir _Local_var_Veh, vectorUp _Local_var_Veh, %1];
								{
									'HandleServer' callExtension (Global_var_ServerIP + '|' + Global_var_UMissionID + '|' + str ['DeleteObjects',1,netId _x]);
									deleteVehicle _x;
								} forEach units _this;
								deleteGroup _this;
								'HandleServer' callExtension (Global_var_ServerIP + '|' + Global_var_UMissionID + '|' + str ['DeleteObjects',0,netId _Local_var_Veh]);
								deleteVehicle _Local_var_Veh;
								(selectRandom Global_var_HCIDs) publicVariableClient 'Public_fnc_HCTask';
							};
						", _Local_var_WPPositions], false];
						
					};
				};
			};
			sleep _Local_var_Delay;
		};
	};
//------------------------------------------------------------------------------------------------------------------------------
	{
		removeGoggles  _x;
		removeAllWeapons _x;
		removeBackpack _x;
	} forEach [MuniGuy1, MuniGuy2];
	sleep 1;
	{
		_x enableSimulation false;
		_x allowDamage false;
	} forEach [MuniGuy1, MuniGuy2];
	{
		_x setVehicleLock "LOCKED";
		_x allowDamage false;
		_x AnimateDoor ["Door_rear_source", 1,true];
		_x animateSource ["ramp_anim",1,true];//Die Kabine der Jets ist nicht schließbar
	} forEach [Heli1,Heli2,Heli3];
	Global_var_AllTriggers = [];
	Global_var_AllTriggers pushBack [[6265,7828,0],1000,0];		//S19
	Global_var_AllTriggers pushBack [[6547,6106,0],1000,1];		//A9
	Global_var_AllTriggers pushBack [[7187,9067,0],1000,2];		//S45
	Global_var_AllTriggers pushBack [[6938,7651,0],1000,3];		//S14
	Global_var_AllTriggers pushBack [[4558,9797,0],1000,4];		//S54
	Global_var_AllTriggers pushBack [[4809,10277,0],1000,5];	//S49
	Global_var_AllTriggers pushBack [[4828,6802,0],1000,6];		//S28
	Global_var_AllTriggers pushBack [[4685,9412,0],1000,7];		//S3
	Global_var_AllTriggers pushBack [[5213,9794,0],1000,8];		//S56
	Global_var_AllTriggers pushBack [[4755,10748,0],1000,9];	//A120
	Global_var_AllTriggers pushBack [[4112,11191,0],1000,10];	//S6
	Global_var_AllTriggers pushBack [[5084,9363,0],1000,11];	//S55
	Global_var_AllTriggers pushBack [[4021,10764,0],1000,12];	//S51
	Global_var_AllTriggers pushBack [[4219,10320,0],1000,13];	//S53
	Global_var_AllTriggers pushBack [[6261,7195,0],1000,14];	//S40
	Global_var_AllTriggers pushBack [[6915,8118,0],1000,15];	//S34
	Global_var_AllTriggers pushBack [[4021,9748,0],1000,16];	//S62
	0 spawn
	{
		private _Local_fnc_DelTriggers =
		{
			{
				if ((_x select 2) in _this) then
				{
					Global_var_AllTriggers set [_forEachIndex, 1];
				};
			} forEach Global_var_AllTriggers;
			Global_var_AllTriggers = Global_var_AllTriggers - [1];
		};
		private _Local_fnc_HandleSingleSpawns =
		{
			private _Local_var_MaxCount = round (((count allPlayers) / 49)  * (count _this));
			if (_Local_var_MaxCount == 0) then
			{
				_Local_var_MaxCount = 1;
			};
			private _Local_var_Counter = 0;
			{
				if (_Local_var_Counter < _Local_var_MaxCount) then
				{
					call compile _x;
					_Local_var_Counter = _Local_var_Counter + 1;
				};
			} forEach _this;
		};
		private _Local_fnc_GetEnemyTypes =
		{
			private _Local_var_MaxGroupCount = _this;
			private _Local_var_GroupCount = round (((count allPlayers) / 49)  * _Local_var_MaxGroupCount);
			private _Local_var_RootClasses = ["rhs_vdv_machinegunner","rhs_vdv_at","rhs_vdv_grenadier_rpg","rhs_vdv_medic"];
			private _Local_var_DynamicClasses = ["rhs_vdv_grenadier","rhs_vdv_junior_sergeant","rhs_vdv_engineer","rhs_vdv_machinegunner_assistant","rhs_vdv_rifleman_asval","rhs_vdv_LAT"];
			private _Local_var_RootCount = floor (_Local_var_MaxGroupCount / 2);
			private _Local_var_ReturnList = [];
			if (_Local_var_RootCount < 1) then
			{
				_Local_var_RootCount = 1;
			};
			if (_Local_var_RootCount < 4) then
			{
				for "_i" from 1 to _Local_var_RootCount do
				{
					_Local_var_ReturnList pushBack (_Local_var_RootClasses select (_i - 1));
				};
			}
			else
			{
				_Local_var_RootCount = 3;
				_Local_var_ReturnList = _Local_var_RootClasses;
			};
			if (_Local_var_GroupCount >= (_Local_var_RootCount + 1)) then
			{
				for "_i" from (_Local_var_RootCount + 1) to _Local_var_GroupCount do
				{
					_Local_var_ReturnList pushBack (selectRandom _Local_var_DynamicClasses);
				};
			};
			_Local_var_ReturnList
		};
		Global_var_LoopCount = Global_var_LoopCount + 1;
		while {count Global_var_AllTriggers > 0} do
		{
			private _Local_var_Result = call compile ("HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["CountPlayerInArea",Global_var_AllTriggers]));
			if (typeName _Local_var_Result == "ARRAY") then
			{
				if (count _Local_var_Result > 0) then
				{
					{
						switch (_x) do
						{
							case 0://S19
							{
								[
									"[0,[6332.55,7665.66,11.07],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[6344.6,7681.9,11.08],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[6327.48,7693.09,11.09],['rhs_vdv_rifleman_alt']] call Global_fnc_CreateGroup;",
									"[0,[6345.05,7716.87,11.12],['rhs_vdv_rifleman_alt']] call Global_fnc_CreateGroup;",
									"[0,[6362.28,7749.56,0],['rhs_vdv_marksman_asval']] call Global_fnc_CreateGroup;",
									"[0,[6357.62,7758.75,4.47],['rhs_vdv_marksman_asval']] call Global_fnc_CreateGroup;",
									"[0,[6355.53,7762.69,4],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[6385.24,7773.21,4.36],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[6386.68,7775.79,4.35],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[6265.92,7818.29,0],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[6268.42,7814.42,4.39],['rhs_vdv_rifleman']] call Global_fnc_CreateGroup;",
									"[0,[6270.06,7811.85,4.46],['rhs_vdv_rifleman']] call Global_fnc_CreateGroup;",
									"[0,[6314.93,7817.38,0.35],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[6326.54,7827.46,0],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[6342.61,7778.08,0],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[6331.84,7788.53,0],['rhs_vdv_engineer']] call Global_fnc_CreateGroup;",
									"[0,[6313.43,7803.87,0.74],['rhs_vdv_LAT']] call Global_fnc_CreateGroup;",
									"[0,[6289.16,7814.49,0],['rhs_vdv_junior_sergeant']] call Global_fnc_CreateGroup;",
									"[0,[6290.84,7818.91,0],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[6283.89,7842.76,4.34],['rhs_vdv_marksman_asval']] call Global_fnc_CreateGroup;",
									"[0,[6285.82,7845.45,4.33],['rhs_vdv_marksman_asval']] call Global_fnc_CreateGroup;",
									"[0,[6353.11,7821.23,4.35],['rhs_vdv_rifleman']] call Global_fnc_CreateGroup;",
									"[0,[6354.87,7819.03,4.35],['rhs_vdv_rifleman']] call Global_fnc_CreateGroup;",
									"[0,[6313.57,7940.56,0],['rhs_vdv_rifleman']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[6364.11,7743.14,0],1,"rhs_KORD_high_MSV",0,197] call Global_fnc_CreateVehicle;
								[0,[6265.71,7828.16,0],1,"rhs_KORD_high_MSV",0,284] call Global_fnc_CreateVehicle;
								[0,[6459.66,7827.35,0],1,"rhs_KORD_high_MSV",0,139] call Global_fnc_CreateVehicle;
								[1,[6207.32,8012.78,0],6 call _Local_fnc_GetEnemyTypes,[6207.32,8012.78,0],250] call Global_fnc_CreateGroup;
								[1,[6458.92,7842.37,0],6 call _Local_fnc_GetEnemyTypes,[6458.92,7842.37,0],250] call Global_fnc_CreateGroup;
								[2,[6338.1,7801.11,0],2 call _Local_fnc_GetEnemyTypes,[[6247,7827,0],[6154,7720,0],[5948,7962,0]]] call Global_fnc_CreateGroup;
								[2,[6332.98,7689.76,0],2 call _Local_fnc_GetEnemyTypes,[[6313,7576,0],[6595,7494,0],[6195,7681,0],[6292,7811,0]]] call Global_fnc_CreateGroup;
							};
							case 1://A9
							{
								[
									"[0,[6561.81,6150.26,0],['rhs_vdv_arifleman']] call Global_fnc_CreateGroup;",
									"[0,[6563.82,6145.67,0],['rhs_vdv_engineer']] call Global_fnc_CreateGroup;",
									"[0,[6564.84,6137.99,0],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[6553.95,6137.37,0],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[6443.85,6201,0],['rhs_vdv_at']] call Global_fnc_CreateGroup;",
									"[0,[6440.71,6194.5,0],['rhs_vdv_at']] call Global_fnc_CreateGroup;",
									"[0,[6479.82,6176.04,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[6482.47,6182.66,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[6482.31,6188.96,13.01],['rhs_vdv_marksman']] call Global_fnc_CreateGroup;",
									"[0,[6547.33,6106.43,0],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[6482.31,6188.96,13],['rhs_vdv_marksman_asval']] call Global_fnc_CreateGroup;",
									"[0,[6482.47,6182.66,0],['rhs_vdv_machinegunner_assistant']] call Global_fnc_CreateGroup;",
									"[0,[6479.82,6176.04,0],['rhs_vdv_arifleman']] call Global_fnc_CreateGroup;",
									"[0,[6443.85,6201,0],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[6440.71,6194.5,0],['rhs_vdv_at']] call Global_fnc_CreateGroup;",
									"[0,[6553.95,6137.37,0],['rhs_vdv_efreitor']] call Global_fnc_CreateGroup;",
									"[0,[6564.84,6137.99,0],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[6563.82,6145.67,0],['rhs_vdv_engineer']] call Global_fnc_CreateGroup;",
									"[0,[6561.81,6150.26,0],['rhs_vdv_arifleman']] call Global_fnc_CreateGroup;",
									"[0,[6625.84,5951.04,0],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[6409.78,6236.83,0],['rhs_vdv_machinegunner_assistant']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[6613.26,5950.84,0],1,"rhs_KORD_high_MSV",0,159] call Global_fnc_CreateVehicle;
								[0,[6401.01,6230.48,0],1,"rhs_KORD_high_MSV",0,327] call Global_fnc_CreateVehicle;
								[0,[6595.49,6110.67,0],1,"rhs_KORD_high_MSV",0,353] call Global_fnc_CreateVehicle;
								[0,[6685.2,5747.85,0],1,"rhs_bmp3mera_msv",0,110] call Global_fnc_CreateVehicle;
								[1,[6585.69,6263.11,0],6 call _Local_fnc_GetEnemyTypes,[6585.69,6263.11,0],250] call Global_fnc_CreateGroup;
								[1,[6338.35,5970.51,0],6 call _Local_fnc_GetEnemyTypes,[6338.35,5970.51,0],150] call Global_fnc_CreateGroup;
							};
							case 2://S45
							{
								[
									"[0,[7215.63,9042.71,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[7214.14,9093,0.6],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[7186.41,9085.45,0],['rhs_vdv_rifleman_alt']] call Global_fnc_CreateGroup;",
									"[0,[7185.76,9076.61,0],['rhs_vdv_rifleman_alt']] call Global_fnc_CreateGroup;",
									"[0,[7228.51,9054.79,4.29],['rhs_vdv_marksman_asval']] call Global_fnc_CreateGroup;",
									"[0,[7226.11,9056.36,4.26],['rhs_vdv_marksman_asval']] call Global_fnc_CreateGroup;",
									"[0,[7237.29,9076.16,0.96],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[7199.41,9096.98,20.38],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[7189.32,9099.48,0.72],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[7174.06,9103.72,4.41],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[7176.9,9102.49,4],['rhs_vdv_rifleman']] call Global_fnc_CreateGroup;",
									"[0,[7164.57,9074.48,1.15],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[7172.36,9061.09,4.26],['rhs_vdv_LAT']] call Global_fnc_CreateGroup;",
									"[0,[7172.04,9056.92,4.25],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[7187.5,9027.71,0],['rhs_vdv_marksman_asval']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[7170.74,9095.57,0],1,"rhs_KORD_high_MSV",0,277] call Global_fnc_CreateVehicle;
								[0,[7166.99,8979.12,0],1,"rhs_KORD_high_MSV",0,103] call Global_fnc_CreateVehicle;
								[0,[7192.98,9027.37,0],1,"rhs_btr80a_msv",0,152] call Global_fnc_CreateVehicle;
								[1,[7090.83,9192.99,0],6 call _Local_fnc_GetEnemyTypes,[7090.83,9192.99,0],250] call Global_fnc_CreateGroup;
								[1,[7221.48,8803.49,0],6 call _Local_fnc_GetEnemyTypes,[7221.48,8803.49,0],250] call Global_fnc_CreateGroup;
								[1,[7187.83,9067.02,0],6 call _Local_fnc_GetEnemyTypes,[7187.83,9067.02,0],250] call Global_fnc_CreateGroup;
								[2,[7206.49,9029.36,0],3 call _Local_fnc_GetEnemyTypes,[[7246,9077,0],[7214,9118,0],[7172,9135,0],[7152,9074,0],[7190,9069,0]]] call Global_fnc_CreateGroup;
							};
							case 3://S14
							{
								[
									"[0,[6961.91,7688.54,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[7025.52,7701.05,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[7019.47,7707.61,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[6959.13,7785.84,0],['rhs_vdv_rifleman']] call Global_fnc_CreateGroup;",
									"[0,[6954.58,7793.41,0],['rhs_vdv_rifleman']] call Global_fnc_CreateGroup;",
									"[0,[6951.12,7776.85,0],['rhs_vdv_efreitor']] call Global_fnc_CreateGroup;",
									"[0,[6938.75,7760.5,0],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[6935.56,7826.04,0],['rhs_vdv_rifleman_alt']] call Global_fnc_CreateGroup;",
									"[0,[6927.25,7819.58,0.43],['rhs_vdv_rifleman']] call Global_fnc_CreateGroup;",
									"[0,[6914.22,7810.43,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[6905.13,7769.22,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[6913.56,7762.91,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[7019.21,7692.09,0.28],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[6938.51,7651.28,0],1,"rhs_KORD_high_MSV",0,240] call Global_fnc_CreateVehicle;
								[0,[7039.66,7684.1,0],1,"rhs_KORD_high_MSV",0,99] call Global_fnc_CreateVehicle;
								[0,[6935.3,7836.6,0],1,"rhs_KORD_high_MSV",0,357] call Global_fnc_CreateVehicle;
								[0,[6876.02,7754.48,0],1,"rhs_KORD_high_MSV",0,269] call Global_fnc_CreateVehicle;
								[1,[7144.56,7335.62,0],6 call _Local_fnc_GetEnemyTypes,[7144.56,7335.62,0],250] call Global_fnc_CreateGroup;
								[1,[7243.14,7697.11,0],6 call _Local_fnc_GetEnemyTypes,[7243.14,7697.11,0],250] call Global_fnc_CreateGroup;
								[2,[6915.85,7705.36,0],2 call _Local_fnc_GetEnemyTypes,[[6901,7837,0],[6937,7843,0],[7051,7684,0],[6978,7676,0]]] call Global_fnc_CreateGroup;
							};
							case 4://S54
							{
								[
									"[0,[4524.32,9906.55,2.85],['rhs_vdv_marksman']] call Global_fnc_CreateGroup;",
									"[0,[4528.9,9907.77,2.85],['rhs_vdv_machinegunner_assistant']] call Global_fnc_CreateGroup;",
									"[0,[4551.15,9876.63,0],['rhs_vdv_rifleman_asval']] call Global_fnc_CreateGroup;",
									"[0,[4546.35,9869.99,2.28],['rhs_vdv_LAT']] call Global_fnc_CreateGroup;",
									"[0,[4546.5,9867.33,2.28],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[4612.2,9655.98,12.94],['rhs_vdv_at']] call Global_fnc_CreateGroup;",
									"[0,[4611.23,9641.48,12.9],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[4639.89,9636.71,15.83],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[4633.02,9658.95,12.71],['rhs_vdv_junior_sergeant']] call Global_fnc_CreateGroup;",
									"[0,[4631.94,9634.21,8.91],['rhs_vdv_aa','rhs_vdv_engineer']] call Global_fnc_CreateGroup;",
									"[0,[4613.24,9634.09,9.03],['rhs_vdv_marksman']] call Global_fnc_CreateGroup;",
									"[0,[4633.55,9658.27,8.78],['rhs_vdv_machinegunner_assistant']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[4510.18,9910.5,0],1,"rhs_KORD_high_MSV",0,62] call Global_fnc_CreateVehicle;
								[0,[4529.77,9983.75,0],1,"rhs_btr80a_msv",0,232] call Global_fnc_CreateVehicle;
								[2,[4558.64,9797.35,0],2 call _Local_fnc_GetEnemyTypes,[[4570,9614,0],[4578,9477,0],[4842,9328,0],[4923,9475,0]]] call Global_fnc_CreateGroup;
							};
							case 5://S49
							{
								[
									"[0,[4970.74,9999.23,0],['rhs_vdv_junior_sergeant']] call Global_fnc_CreateGroup;",
									"[0,[4960.23,10008.9,0],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[4962.84,10008.3,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[4783.31,10249.2,18],['rhs_vdv_at']] call Global_fnc_CreateGroup;",
									"[0,[4784.38,10248.5,13.92],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[4782.6,10247,13.92],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[4742.27,10331.2,20.32],['rhs_vdv_junior_sergeant']] call Global_fnc_CreateGroup;",
									"[0,[4714.68,10209.7,9.12],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[4717.25,10222.3,9.12],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[4715.98,10215.2,9.11],['rhs_vdv_at']] call Global_fnc_CreateGroup;",
									"[0,[5029.6,10088.5,5.06],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[4962.49,10214.1,5.06],['rhs_vdv_at']] call Global_fnc_CreateGroup;",
									"[0,[4854.36,10565.9,5.03],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[4291.47,11325.6,5.11],['rhs_vdv_at']] call Global_fnc_CreateGroup;",
									"[0,[4696.95,10237.6,0],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[4685.05,10254.6,0],['rhs_vdv_medic']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[4987.74,10246.9,0],1,"rhs_bmp3_msv",0,43] call Global_fnc_CreateVehicle;
								[1,[4809.36,10277,0],5 call _Local_fnc_GetEnemyTypes,[4809.36,10277,0],200] call Global_fnc_CreateGroup;
								[2,[4694.63,10367.8,0],4 call _Local_fnc_GetEnemyTypes,[[4794,10188,0],[4926,9972,0],[4597,9615,0],[4361,10290,0]]] call Global_fnc_CreateGroup;
							};
							case 6://S28
							{
								[
									"[0,[4664.53,6767.31,0],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;",
									"[0,[4658.78,6771.46,13.01],['rhs_vdv_marksman_asval']] call Global_fnc_CreateGroup;",
									"[0,[4654.6,6776.86,0],['rhs_vdv_arifleman']] call Global_fnc_CreateGroup;",
									"[0,[4674.64,6763.3,0],['rhs_vdv_medic']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[4742.58,6701.14,0],1,"rhs_KORD_high_MSV",0,110] call Global_fnc_CreateVehicle;
								[0,[4678,6763.8,2.81],1,"rhs_KORD_high_MSV",0,38] call Global_fnc_CreateVehicle;
								[0,[4675.8,6825.96,0],1,"rhs_KORD_high_MSV",0,24] call Global_fnc_CreateVehicle;
								[0,[4828.85,6802.16,0],1,"rhs_KORD_high_MSV",0,87] call Global_fnc_CreateVehicle;
								[1,[4803.64,6942.57,0],6 call _Local_fnc_GetEnemyTypes,[4803.64,6942.57,0],250] call Global_fnc_CreateGroup;
								[1,[4549.14,6755.53,0],6 call _Local_fnc_GetEnemyTypes,[4549.14,6755.53,0],250] call Global_fnc_CreateGroup;
								[1,[4855.84,6615.66,0],6 call _Local_fnc_GetEnemyTypes,[4855.84,6615.66,0],250] call Global_fnc_CreateGroup;
							};
							case 7://S3
							{
								[
									"[0,[4588.23,9546.16,0.38],['rhs_vdv_rifleman_asval']] call Global_fnc_CreateGroup;",
									"[0,[4589.98,9546.86,3.38],['rhs_vdv_LAT']] call Global_fnc_CreateGroup;",
									"[0,[4602.25,9462.38,4.97],['rhs_vdv_junior_sergeant']] call Global_fnc_CreateGroup;",
									"[0,[4670.93,9419.55,5.14],['rhs_vdv_aa']] call Global_fnc_CreateGroup;",
									"[0,[4819.48,9343.94,4.93],['rhs_vdv_engineer']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[4730.56,9349.22,0],1,"rhs_btr70_vmf",0,172] call Global_fnc_CreateVehicle;
								[0,[4853.72,9354.98,0],1,"rhs_KORD_high_MSV",0,117] call Global_fnc_CreateVehicle;
								[0,[4685.13,9412.07,0],1,"rhs_KORD_high_MSV",0,217] call Global_fnc_CreateVehicle;
								[1,[4610.98,9512.92,0],6 call _Local_fnc_GetEnemyTypes,[4610.98,9512.92,0],200] call Global_fnc_CreateGroup;
							};
							case 8://S56
							{
								[
									"[0,[5210.69,9788.4,5.04],['rhs_vdv_rifleman_asval']] call Global_fnc_CreateGroup;",
									"[0,[5216.11,9785.63,0],['rhs_vdv_LAT']] call Global_fnc_CreateGroup;",
									"[0,[5216.98,9777.68,0],['rhs_vdv_junior_sergeant']] call Global_fnc_CreateGroup;",
									"[0,[5207.41,9775.13,0.39],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[5229.59,9791.21,0],1,"rhs_KORD_high_MSV",0,103] call Global_fnc_CreateVehicle;
								[1,[5284.38,9771.62,0],4 call _Local_fnc_GetEnemyTypes,[5284.38,9771.62,0],200] call Global_fnc_CreateGroup;
								[2,[5213.73,9794.58,0],2 call _Local_fnc_GetEnemyTypes,[[5059,10043,0],[4943,10261,0],[4745,10395,0]]] call Global_fnc_CreateGroup;
							};
							case 9://A120
							{
								[
									"[0,[4755.41,10763.3,4.98],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[4759.43,10762.7,0],['rhs_vdv_junior_sergeant']] call Global_fnc_CreateGroup;",
									"[0,[4763.85,10756,9.16],['rhs_vdv_grenadier']] call Global_fnc_CreateGroup;",
									"[0,[4755.8,10748.9,0.24],['rhs_vdv_machinegunner']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[4767.61,10769.4,0],1,"rhs_KORD_high_MSV",0,61] call Global_fnc_CreateVehicle;
								[1,[4560.55,10741.0],6 call _Local_fnc_GetEnemyTypes,[4560.55,10741.0],200] call Global_fnc_CreateGroup;
								[2,[4854.53,10579,0],2 call _Local_fnc_GetEnemyTypes,[[48156,10691,0],[4766,10759,0],[4702,10839,0],[4295,11330,0]]] call Global_fnc_CreateGroup;
							};
							case 10://S6
							{
								[
									"[0,[4127.4,11176.7,0],['rhs_vdv_grenadier_rpg']] call Global_fnc_CreateGroup;",
									"[0,[4116.24,11181.7,0],['rhs_vdv_medic']] call Global_fnc_CreateGroup;",
									"[0,[4109.81,11175.9,0],['rhs_vdv_junior_sergeant']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[4112.69,11191.1,0],1,"rhs_KORD_high_MSV",0,313] call Global_fnc_CreateVehicle;
								[0,[4084.82,11175.2,0],1,"rhs_btr70_msv",0,329] call Global_fnc_CreateVehicle;
								[2,[4269.88,11313.6,0],2 call _Local_fnc_GetEnemyTypes,[[4115,11184,0],[3938,11008,0],[4093,10778,0],[4401,10547,0]]] call Global_fnc_CreateGroup;
							};
							case 11://S55
							{
								[
									"[0,[5039.57,9401.55,5.19],['rhs_vdv_marksman']] call Global_fnc_CreateGroup;",
									"[0,[5184.66,9407.72,5.11],['rhs_vdv_machinegunner_assistant']] call Global_fnc_CreateGroup;"
								] call _Local_fnc_HandleSingleSpawns;
								[0,[5190.58,9414.38,0],1,"rhs_KORD_high_MSV",0,121] call Global_fnc_CreateVehicle;
								[0,[5013.65,9419.41,0.01],1,"rhs_KORD_high_MSV",0,221] call Global_fnc_CreateVehicle;
								[2,[5084.2,9363.96,0],2 call _Local_fnc_GetEnemyTypes,[[5156,9356,0],[5262,9527,0],[5221,9774,0]]] call Global_fnc_CreateGroup;
							};
							case 12://S51
							{
								[0,[4158.03,11006.9,0],1,"rhs_btr80a_vmf",0,163] call Global_fnc_CreateVehicle;
								[0,[4117.31,10711.4,20.35],["rhs_vdv_engineer"]] call Global_fnc_CreateGroup;
								[1,[4021.92,10764,0],6 call _Local_fnc_GetEnemyTypes,[4021.92,10764,0],200] call Global_fnc_CreateGroup;
							};
							case 13://S53
							{
								[1,[4296.47,10283.7,0],6 call _Local_fnc_GetEnemyTypes,[4296.47,10283.7,0],200] call Global_fnc_CreateGroup;
								[2,[4219.3,10320.8,0],2 call _Local_fnc_GetEnemyTypes,[[4404,10056,0],[4515,9901,0],[4515,9859,0]]] call Global_fnc_CreateGroup;
							};
							case 14://S40
							{
								[1,[6261.9,7195.04,0],6 call _Local_fnc_GetEnemyTypes,[6261.9,7195.04,0],250] call Global_fnc_CreateGroup;
							};
							case 15://S34
							{
								[1,[6915.34,8118.27,0],6 call _Local_fnc_GetEnemyTypes,[6915.34,8118.27,0],250] call Global_fnc_CreateGroup;
							};
							case 16://S62
							{
								[0,[4020.68,9748.01,0],1,"rhs_btr60_vmf",0,166] call Global_fnc_CreateVehicle;
							};
						};
					} forEach _Local_var_Result;
					_Local_var_Result call _Local_fnc_DelTriggers;
				};
			}
			else
			{
				diag_log _Local_var_Result;
			};
			sleep 5;
		};
		Global_var_LoopCount = Global_var_LoopCount - 1;
	};
	{
		if ((local _x) && {simulationEnabled _x}) then
		{
			_x addEventHandler ["killed", {"HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + str ["AddObjectToDelete",0,netId (_this select 0)])}];
		};
	} forEach vehicles;
	[[[2752.71,9960.14,0],300,5],[[5853.7,4808.35,0],150,3],[[5354.62,8575.39,0],250,5],[[8477.22,6684.18,0],100,2],[[7568.7,5172.12,0],300,5],[[4902.08,5633.62,0],100,4],[[5941.36,10282.2,0],250,5],[[3885.31,8889.41,0],350,6]] execFSM "HandleCivsA3.fsm";
	Global_var_LoopCount = Global_var_LoopCount + 1;
	execFSM "ACEStuff.fsm";
	addMissionEventHandler ["HandleDisconnect",{deleteVehicle (_this select 0);false}];
	setTimeMultiplier 0.1;
};
if (!isServer) then
{
	waitUntil {sleep 1;!isNull Player};
	Player setVariable ["BIS_noCoreConversations", true];
	enableRadio false;
	Player enableFatigue false;
	if (hasInterface) then
	{
		startLoadingScreen ["Mission wird initialisiert..."];
		disableUserInput true;
		Player addEventHandler ["Respawn", {(_this select 0) enableFatigue false;(_this select 0) setVariable ["BIS_noCoreConversations", true]; _Local_var_BackPack = nearestObject [_this select 0, "GroundWeaponHolder"];if (!isNull _Local_var_BackPack) then {deleteVehicle _Local_var_BackPack;};(_this select 1) spawn {sleep 60; deleteVehicle _this};removeAllWeapons Player;removeAllItems Player;removeAllAssignedItems Player;removeVest Player;removeBackpack Player;removeHeadgear Player;removeGoggles Player;}];
		"Public_var_TaskDone" addPublicVariableEventHandler
		{
			switch (_this select 1) do
			{
				case 1:
				{
					"fuel_dump_target" setMarkerColorLocal "ColorGreen";
					Global_var_Task1  setTaskState "Succeeded";
					["TaskSucceeded",["Treibstoffreserven zerstört"]] call bis_fnc_showNotification;
				};
				case 2:
				{
					"tanks_target" setMarkerColorLocal "ColorGreen";
					Global_var_Task2  setTaskState "Succeeded";
					["TaskSucceeded",["Fahrzeugdepot zerstört"]] call bis_fnc_showNotification;
				};
				case 3:
				{
					"radar_target" setMarkerColorLocal "ColorGreen";
					Global_var_Task3  setTaskState "Succeeded";
					["TaskSucceeded",["Radar zerstört"]] call bis_fnc_showNotification;
				};
				case 4:
				{
					"artillery_target" setMarkerColorLocal "ColorGreen";
					Global_var_Task4 setTaskState "Succeeded";
					["TaskSucceeded",["Artillerie-Stellungen zerstört"]] call bis_fnc_showNotification;
				};
				case 5:
				{
					if ((Player distance Heli1) < 1000) then
					{
						"LZ_3" setMarkerAlphaLocal 0;
						"charlie_dir" setMarkerAlphaLocal 0;
						"LZ_5" setMarkerAlphaLocal 1;
						"charlie_dir2" setMarkerAlphaLocal 1;
					};
					"hq_target" setMarkerColorLocal "ColorGreen";
					"airbase_target" setMarkerAlphaLocal 1;
					Global_var_Dropzone = getMarkerPos "LZ_5";
					Global_var_Task5 setTaskState "Succeeded";
					Global_var_Task6 setTaskState "Assigned";
					Global_var_Task6 setSimpleTaskDestination getMarkerPos "airbase_target";
					["TaskSucceeded",["HQ gesäubert"]] call bis_fnc_showNotification;
				};
				case 6:
				{
					"airbase_target" setMarkerColorLocal "ColorGreen";
					Global_var_Task6 setTaskState "Succeeded";
					["TaskSucceeded",["Luftstützpunkt gesäubert"]] call bis_fnc_showNotification;
				};
			};
			if ((({taskState _x == "Succeeded"} count [Global_var_Task1,Global_var_Task2,Global_var_Task3,Global_var_Task4]) == 4)  && {taskState Global_var_Task5 != "Succeeded"}) then
			{
				"hq_target" setMarkerAlphaLocal 1;
				"LZ_1" setMarkerAlphaLocal 0;
				"LZ_2" setMarkerAlphaLocal 0;
				"LZ_4" setMarkerAlphaLocal 0;
				"alpha_dir" setMarkerAlphaLocal 0;
				"bravo_dir" setMarkerAlphaLocal 0;
				"delta_dir" setMarkerAlphaLocal 0;
				Global_var_Dropzone = getMarkerPos "LZ_3";
				"charlie_dir" setMarkerTextLocal "Alpha,Bravo,Charlie,Delta";
				Global_var_Task5 setTaskState "Assigned";
				Global_var_Task5 setSimpleTaskDestination getMarkerPos "hq_target";
				["TaskAssigned",["HQ angreifen"]] call bis_fnc_showNotification;
			};
			if (({taskState _x == "Succeeded"} count [Global_var_Task1, Global_var_Task2, Global_var_Task3, Global_var_Task4, Global_var_Task5, Global_var_Task6]) == 6) then
			{
				0 spawn
				{
					sleep 60;
					[[["Mission Erfolgreich abgeschlossen!","<t align = 'center' size = '1.5'>%1</t><br/>"],  ["Gute Arbeit.","<t align = 'center' size = '1.5'>%1</t><br/>"]]] spawn BIS_fnc_typeText;
					["end5",true,15,true,false] spawn BIS_fnc_endMission;
				};
			};
		};
		Global_fnc_GetGroup =
		{
			private _Local_var_GroupID = 0;
			if ((str Player) in ["p3","p4","p5","p6","p7","p8","p9","p10","p11","p12","p13"]) then
			{
				_Local_var_GroupID = 1;
			};
			if ((str Player) in ["p14","p15","p16","p17","p18","p19","p20","p21","p22","p23","p24"]) then
			{
				_Local_var_GroupID = 2;
			};
			if ((str Player) in ["p25","p26","p27","p28","p29","p30","p31","p32","p33","p34","p35"]) then
			{
				_Local_var_GroupID = 3;
			};
			if ((str Player) in ["p36","p37","p38","p39","p40","p41","p42","p43","p44","p45","p46"]) then
			{
				_Local_var_GroupID = 4;
			};
			if ((str Player) in ["p1"]) then
			{
				_Local_var_GroupID = 6;
			};
			if ((str Player) in ["p2"]) then
			{
				_Local_var_GroupID = 7;
			};
			_Local_var_GroupID
		};
		{
			_x addEventHandler ["GetIn",
			{
				0 spawn
				{
					Public_var_MissionStarted = 0;
					sleep 3;
					waitUntil{sleep 3;!isNil "Public_var_MissionStarted"};
					if ((Vehicle Player) in [Heli1,Heli2,Heli3]) then
					{
						//SpecialFX: FadeOut, FadeIn, Text
						"Parachute" cutText ["", "BLACK OUT", 15];
						sleep 15;
						sleep 10;
						Player action ["eject", Vehicle Player];
						sleep 2;
						"Parachute" cutText ["Ein paar Stunden später", "BLACK IN", 20];
						(Global_var_Dropzone getPos [random 400, random 360]) execVM "HandleParachute.sqf";
					};
				};
			}];
		} forEach [Heli1,Heli2,Heli3];
		Player addEventHandler
		[
			"Respawn",
			{
				if (({taskState _x == "Succeeded"} count [Global_var_Task1,Global_var_Task2,Global_var_Task3,Global_var_Task4]) != 4) then
				{
					{
						_x setMarkerAlphaLocal 1;
					} forEach ["LZ_1","LZ_2","LZ_3","LZ_4","aplha_dir","bravo_dir","charlie_dir","delta_dir"];
				}
				else
				{
					if (Global_var_Task5 != "Succeeded") then
					{
						{
							_x setMarkerAlphaLocal 1;
						} forEach ["LZ_3","charlie_dir"];
					}
					else
					{
						{
							_x setMarkerAlphaLocal 1;
						} forEach ["LZ_5","charlie_dir2"];
					};
				};
				0 spawn  //Absprung
				{
					while {(Player distance Heli1) < 1000} do
					{
						waitUntil{sleep 10;(!isNull Player) && {alive Player}};
						waitUntil{sleep 5;(vehicle Player == Player) && {({(Player distance _x) < 10} count [Heli1,Heli2,Heli3]) > 0}};
						private _Local_var_AID = Player addAction ["Einsteigen", {Player moveInCargo (nearestObject [Player, "RHS_CH_47F_10"])}];
						waitUntil{sleep 3;((vehicle Player) != Player) || {({(Player distance _x) < 10} count [Heli1,Heli2,Heli3]) == 0}};
						Player removeAction _Local_var_AID;
					};
				};
			}
		];
		0 spawn //Absprung
		{
			while {(Player distance Heli1) < 1000} do
			{
				waitUntil{sleep 10;(!isNull Player) && {alive Player}};
				waitUntil{sleep 5;(vehicle Player == Player) && {({(Player distance _x) < 10} count [Heli1,Heli2,Heli3]) > 0}};
				private _Local_var_AID = Player addAction ["Einsteigen", {Player moveInCargo (nearestObject [Player, "RHS_CH_47F_10"])}];
				waitUntil{sleep 3;((vehicle Player) != Player) || {({(Player distance _x) < 10} count [Heli1,Heli2,Heli3]) == 0}};
				Player removeAction _Local_var_AID;
			};
		};
		0 spawn //Ausrüster
		{
			Private ["_Local_var_AID"];
			while {true} do
			{
				waitUntil{sleep 10;(!isNull Player) && {alive Player}};
				waitUntil{sleep 3;(Player distance MuniGuy1) < 5};
				private _Local_var_AID = Player addAction ["Ausrüstung", "Ammo\Dialog.sqf", [0,10]];
				waitUntil{sleep 3;(Player distance MuniGuy1) > 5};
				Player removeAction _Local_var_AID;
			};
		};
		0 spawn //Ausrüster
		{
			Private ["_Local_var_AID"];
			while {true} do
			{
				waitUntil{sleep 10;(!isNull Player) && {alive Player}};
				waitUntil{sleep 3;(Player distance MuniGuy2) < 5};
				private _Local_var_AID = Player addAction ["Ausrüstung", "Ammo\Dialog.sqf", [0,10]];
				waitUntil{sleep 3;(Player distance MuniGuy2) > 5};
				Player removeAction _Local_var_AID;
			};
		};
		private _Local_fnc_AddRedLight =
		{
			private _Local_var_RedLight = "#lightpoint" createVehicleLocal [0,0,0];
			_Local_var_RedLight setLightBrightness 0.3;
			_Local_var_RedLight setLightAmbient [0.5,0,0];
			_Local_var_RedLight setLightColor [0.5,0,0];
			_Local_var_RedLight lightAttachObject [_this, [0,-6,-2]]; 
		};
		private _Local_fnc_AddAmbientLight =
		{
			private _Local_var_AmbientLight ="#lightpoint" createVehicleLocal [0,0,0];
			_Local_var_AmbientLight setLightBrightness 0.05;
			_Local_var_AmbientLight setLightAmbient [255, 147, 41];
			_Local_var_AmbientLight setLightColor [255, 147, 41];
			_Local_var_AmbientLight setPosATL _this;
		};
		{
			_x call _Local_fnc_AddRedLight
		} forEach [Heli1,Heli2,Heli3];
		{
			_x call _Local_fnc_AddAmbientLight;
		} forEach [[13828.1,331.51,168.88],[13836.1,338.27,168.84],[6345.46,7784.27,0.82],[6960.45,7771.64,0.86],[4659.9,6774.36,0.92],[4962.21,10009.5,0.91],[7204.84,9065.34,0.47],[6454.04,6185.94,1.35],[13798.1,326.53,168.69],[13836.9,348.04,168.8],[4650.88,6850.85,1.46],[6918.27,7808.59,1.22],[6528.32,6110.82,4.08],[7166.65,9073.25,1.78],[6322.91,7799.26,0.86],[4719.28,10216.3,9.92]];
		execFSM "ACEStuff.fsm";
		"Init" execFSM "HandleChemLight_New.fsm";
		removeAllWeapons Player;
		removeAllItems Player;
		removeAllAssignedItems Player;
		MuniGuy1 enableSimulation false;
		MuniGuy2 enableSimulation false;
		STHud_NoSquadBarMode = true;
		for "_i" from 1 to 4 do
		{
			call compile format [
			"
				if (!isNull EnemyVehicle%1) then
				{
					EnemyVehicle%1 addEventHandler ['Explosion',
					{
						if (((damage (_this select 0)) + (_this select 1)) > 0.51) then
						{
							(_this select 0) setDamage 1;
						};
					}];
				};
			", _i];
		};
		private _Local_var_DebugLevel = ["DebugLevel", 0] call BIS_fnc_getParamValue;
		if (_Local_var_DebugLevel > 1) then
		{
			if (_Local_var_DebugLevel == 3) then
			{
				0 spawn
				{
					while {true} do
					{
						Player setPos ([5419,8358,0] getPos [300 + (random 2300), random 360]);
						if (((count allPlayers) > 2) && {(random 100) < (20 / ((count allPlayers) - 2))}) then
						{
							if ((east countside allUnits) > 10) then
							{
								{
									if (side _x == east) exitWith
									{
										_x setDamage 1;
									};
								} foreach allUnits;
							};
						};
						sleep 10;
					};
				};
			}
			else
			{
				onMapSingleClick "Player setPos _pos;";
			};
			private _Local_var_EnemyMarkers = ["EnemyMarkers", 0] call BIS_fnc_getParamValue;
			if (_Local_var_EnemyMarkers == 1) then
			{
				0 spawn
				{
					while {true} do
					{
						{
							if (_forEachIndex > 17) then
							{
								deleteMarkerLocal _x;
							};
						} forEach allMapMarkers;
						{
							if ((side _x == east) && {_x != Player}) then
							{
								_Marker = createMarkerLocal [format["Enemy%1", _forEachIndex], getPos _x];
								if (_x in Global_var_CachedUnits) then
								{
									_Marker setMarkerColorLocal "ColorGrey";
								}
								else
								{
									_Marker setMarkerColorLocal "ColorRed";
								};
								_Marker setMarkerShapeLocal "ICON";
								_Marker setMarkerBrushLocal "Solid";
								_Marker setMarkerSizeLocal [2, 2];
								_Marker setMarkerTypeLocal "mil_dot";
								_Marker setMarkerTextLocal format["%1m", round (_x distance Player)];
							};
						} forEach allUnits;
						_Marker = createMarkerLocal ["Playerxy", getPos Player];
						_Marker setMarkerColorLocal "ColorGreen";
						_Marker setMarkerShapeLocal "ICON";
						_Marker setMarkerBrushLocal "Solid";
						_Marker setMarkerSizeLocal [2, 2];
						_Marker setMarkerTypeLocal "mil_dot";
						_Marker setMarkerTextLocal name Player;
						sleep 10;
					};
				};
			};
		};
		if (_Local_var_DebugLevel == 1) then
		{
			0 spawn
			{
				while {true} do
				{
					if ((Player distance MuniGuy1) > 500) then
					{
						Player setPos ((getPos MuniGuy1) findEmptyPosition [10, 50, typeOf Player]);
					};
					sleep 10;
				};
			};
		};
		uiSleep 35;
		disableUserInput false;
		endLoadingScreen;
		if (didJIP) then
		{
			call Global_fnc_AddTasks;
		};
		switch (call Global_fnc_GetGroup) do
		{
			case 1:
			{
				Global_var_Dropzone = getMarkerPos "LZ_1";
				Global_var_Task1 setTaskState "Assigned";
			};
			case 2:
			{
				Global_var_Dropzone = getMarkerPos "LZ_2";
				Global_var_Task2 setTaskState "Assigned";
			};
			case 3:
			{
				Global_var_Dropzone = getMarkerPos "LZ_3";
				Global_var_Task3 setTaskState "Assigned";
			};
			case 4:
			{
				Global_var_Dropzone = getMarkerPos "LZ_4";
				Global_var_Task4 setTaskState "Assigned";
			};
			case 6:
			{
				Global_var_Dropzone = getMarkerPos "LZ_3";
				Global_var_Task3 setTaskState "Assigned";
			};
			case 7:
			{
				Global_var_Dropzone = getMarkerPos "LZ_2";
				Global_var_Task2 setTaskState "Assigned";
			};
		};
		startLoadingScreen ["Mission wird initialisiert..."];
		disableUserInput true;
		if (didJIP) then
		{
			Public_fnc_GetJIPInfos = Player;
			publicVariableServer "Public_fnc_GetJIPInfos";
		};
		uiSleep (5 + (random 8));
		Player allowDamage false;
		disableUserInput false;
		endLoadingScreen;
		["Operation Moonwalk", "Datum:" + str (date select 2) + "/" + str (date select 1) + "/" + str (date select 0)] spawn BIS_fnc_infoText;
		sleep 15;
		["Viel Spaß!", "Datum:" + str (date select 2) + "/" + str (date select 1) + "/" + str (date select 0)] spawn BIS_fnc_infoText;
	}
	else
	{
		Global_var_FPS = 0;
		[{Global_var_FPS = Global_var_FPS + 1;}, 0, 0] call CBA_fnc_addPerFrameHandler;
		[{diag_log format["--HC-- Einheiten: %1 DavonLokal: %2 Spieler: %3 Objekte: %4 FPS: %5 Schleifen: %6 Skripte: %7", count allUnits, {if (local _x) then {true} else {false}} count allUnits, count allPlayers, count vehicles, Global_var_FPS, Global_var_LoopCount,diag_activeScripts];Global_var_FPS = 0;}, 1, 0] call CBA_fnc_addPerFrameHandler;
	};
};
//execVM "HandleSpawnMarkers.sqf";