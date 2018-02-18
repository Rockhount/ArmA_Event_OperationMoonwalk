if ((typeName _this) == "ARRAY") then
{
	createDialog "Global_var_WeaponGUI";
	waitUntil {dialog};
	ctrlSetText [2103, "Klassenauswahl"];
	for "_i" from ((_this select 3) select 0) to ((_this select 3) select 1) do
	{
		lbAdd[2100, (Global_var_AmmoData select _i) select 0];
	};
	lbSetCurSel [2100, 0];
	Global_var_AmmoSelection = _this select 3;
}
else
{
	for "_i" from (Global_var_AmmoSelection select 0) to (Global_var_AmmoSelection select 1) do
	{
		if (((Global_var_AmmoData select _i) select 0) == _this) exitWith
		{
			Public_fnc_ManageAmmoRequests = [Player,_i];
			publicVariableServer "Public_fnc_ManageAmmoRequests";
			systemChat "Anfrage an Server geschickt. Bitte warten...";
		};
	};
};