using System;
namespace SPit.Infrastructure
{
	public enum GpioPin
	{
		#region OutputPins

		HighBeamLightRelais = 20,
		LowBeamLightRelais = 21,
		StandLightRelais = 16,

		RearLightRelais = 7,
		StopLightRelais = 8,

		HornRelais = 5,
		IgnitionRelais = 12,

		FrontLeftTurnSignalLightRelais = 26,
		FrontRightTurnSignalLightRelais = 19,
		RearLeftTurnSignalLightRelais = 13,
		RearRightTurnSignalLightRelais = 6,

		#endregion

		#region InputPins

		// Input Pins
		LightSwitch0 = 17,
		LightSwitch1 = 27,
		FlashLighSwitch = 25,

		KeySwitch0 = 4,
		KeySwitch1 = 18,

		TurnSignalSwitch0 = 22,
		TurnSignalSwitch1 = 23,

		HornSwitch = 24

		#endregion
	}
}
