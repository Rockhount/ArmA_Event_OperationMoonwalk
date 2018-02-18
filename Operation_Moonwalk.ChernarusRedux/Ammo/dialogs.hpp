class Global_var_WeaponGUI
{
	idd=-1;
	movingenable=false;
	class controls
	{
		class Global_var_WeaponGUI_Frame: RscFrame
		{
			idc = -1;
			x = 0.365937 * safezoneW + safezoneX;
			y = 0.379 * safezoneH + safezoneY;
			w = 0.170156 * safezoneW;
			h = 0.143 * safezoneH;
		};
		class Global_var_WeaponGUI_Background: Box
		{
			idc = -1;
			x = 0.365937 * safezoneW + safezoneX;
			y = 0.379 * safezoneH + safezoneY;
			w = 0.170156 * safezoneW;
			h = 0.143 * safezoneH;
		};
		class Global_var_WeaponGUI_Text: RscText
		{
			idc = -1;
			text = "Waffenauswahl";
			x = 0.365937 * safezoneW + safezoneX;
			y = 0.39 * safezoneH + safezoneY;
			w = 0.0773437 * safezoneW;
			h = 0.022 * safezoneH;
		};
		class Global_var_WeaponGUI_Combo: RscCombo
		{
			idc = 2100;
			x = 0.371094 * safezoneW + safezoneX;
			y = 0.445 * safezoneH + safezoneY;
			w = 0.159844 * safezoneW;
			h = 0.022 * safezoneH;
		};
		class Global_var_WeaponGUI_Button_OK: RscButton
		{
			idc = 2101;
			text = "OK";
			x = 0.371094 * safezoneW + safezoneX;
			y = 0.489 * safezoneH + safezoneY;
			w = 0.0773437 * safezoneW;
			h = 0.022 * safezoneH;
			action = "(lbText [2100, lbCurSel 2100]) execVM ""Ammo\Dialog.sqf"";closeDialog 1;";
		};
		class Global_var_WeaponGUI_Button_Cancel: RscButton
		{
			idc = 2102;
			text = "Abbruch";
			x = 0.453594 * safezoneW + safezoneX;
			y = 0.489 * safezoneH + safezoneY;
			w = 0.0773437 * safezoneW;
			h = 0.022 * safezoneH;
			action = "closeDialog 2;";
		};
	};
};