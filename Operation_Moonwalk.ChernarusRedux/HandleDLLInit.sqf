private _Local_fnc_ShowDLLMissing =
{
	Params ["_Local_var_ResultText", "_Local_var_ResultVal"];
	switch (_Local_var_ResultVal) do
	{
		case 0:
		{
			Public_fnc_ShowDLLMissing = 0;
			publicVariable "Public_fnc_ShowDLLMissing";
		};
		case 1:
		{
			Public_fnc_ShowDLLMissing = 1;
			publicVariable "Public_fnc_ShowDLLMissing";
			endMission "End2";
		};
		case 2:
		{
			endMission "End3";
		};
	};
	false
};
private _Local_fnc_ProcessResult =
{
	Params ["_Local_var_Result", "_Local_var_ResultText", "_Local_var_ResultVal"];
	private _Local_var_ReturnVal = true;
	if (isNil "_Local_var_Result") then
	{
		_Local_var_ReturnVal = [_Local_var_ResultText, _Local_var_ResultVal] call _Local_fnc_ShowDLLMissing;
	}
	else
	{
		if (_Local_var_Result != "Test") then
		{
			_Local_var_ReturnVal = [_Local_var_ResultText, _Local_var_ResultVal] call _Local_fnc_ShowDLLMissing;
		};
	};
	_Local_var_ReturnVal
};
_Local_var_Result = "";
_Local_var_ResultText = "";
_Local_var_ResultVal = -1;
if (hasInterface) then
{
	_Local_var_Result = "HandleClient" callExtension str ["Test",Global_var_ServerIP4Player,Global_var_UMissionID];
	_Local_var_ResultText = "HandleClient.dll nicht verbunden";
	_Local_var_ResultVal = 2;
}
else
{
	if (isServer) then
	{
		_Local_var_Result = "HandleServer" callExtension (Global_var_ServerIP + "|" + Global_var_UMissionID + "|" + "Test");
		_Local_var_ResultText = "HandleServer.dll nicht verbunden";
		_Local_var_ResultVal = 0;
	}
	else
	{
		_Local_var_Result = "HandleHC" callExtension str ["Test",Global_var_ServerIP4HC,Global_var_UMissionID];
		_Local_var_ResultText = "HandleHC.dll nicht verbunden";
		_Local_var_ResultVal = 1;
	};
};
[_Local_var_Result, _Local_var_ResultText, _Local_var_ResultVal] call _Local_fnc_ProcessResult