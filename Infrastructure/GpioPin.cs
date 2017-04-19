using System;
namespace SPit.Infrastructure
{
	public enum GpioPin
	{
		// Ouput Pins
		HighBeamLightRelais = 1,
		LowBeamLightRelais = 2,
		FrontLightRelais = 3,

		RearLightRelais = 4,
		StopLightRelais = 5,

		FrontLeftTurnSignalLightRelais = 6,
		FrontRightTurnSignalLightRelais = 7,
		RearLeftTurnSignalLightRelais = 8,
		RearRightTurnSignalLightRelais = 9,

		HornRelais = 10,

		// Input Pins
		BeamLighSwitch = 11,

		KeySwitch0 = 12,
		KeySwitch1 = 13,


		TurnSignalSwitch0 = 14,
		TurnSignalSwitch1 = 15,

		HornSwitch = 16
	}
}
