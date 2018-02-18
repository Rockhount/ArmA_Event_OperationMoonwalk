if (hasInterface && canSuspend) then
{
	private _Local_var_Exit = false;
	private _Local_var_Pos = if ((!isNil "_this") && {typeName _this == "ARRAY"}) then {_this} else {_Local_var_Exit = true;[0,0,0]};
	//Error handling
	if (_Local_var_Exit) exitWith
	{
		diag_log "HandleParachute Error: Wrong parameters";
	};
	//Jump
	if (!isNull (unitBackpack player)) then
	{
		removeBackpack Player;
	};
	Player addBackpack "B_Parachute";
	Player setPosATL (_Local_var_Pos vectorAdd [0,0,3000]);
	Player allowDamage false;
	waitUntil {sleep 1;(isTouchingGround (Vehicle Player)) || {((getPos Player) select 2) < 1}};
	Player action ["eject", Vehicle Player];
	Player allowDamage true;
	sleep 3;
	if (!isNil "Public_fnc_ManageAmmoRequests") then
	{
		publicVariableServer "Public_fnc_ManageAmmoRequests";
	}
	else
	{
		removeBackpack Player;
	};
	{
		_x setMarkerAlphaLocal 0;
	} forEach ["LZ_1","LZ_2","LZ_3","LZ_4","LZ_5","alpha_dir","bravo_dir","charlie_dir","delta_dir","charlie_dir2"];
};