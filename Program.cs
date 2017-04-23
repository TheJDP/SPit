using System;
using System.Net.Sockets;
using SPit.Controllers;
using SPit.Devices;
using SPit.Infrastructure;
using SPit.Interfaces;

namespace SPit
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			GpioController gpioController = new TcpGpioController("192.168.178.45", 10000);

			Switch turnSignalSwitch = new Switch(GpioPin.TurnSignalSwitch0, GpioPin.TurnSignalSwitch1);

			Relais frontLightRelais = new Relais(gpioController, GpioPin.StandLightRelais);

			Relais frontLeftTurnSignalLightRelais = new Relais(gpioController, GpioPin.FrontLeftTurnSignalLightRelais);
			Relais frontRightTurnSignalLightRelais = new Relais(gpioController, GpioPin.FrontRightTurnSignalLightRelais);
			Relais rearLeftTurnSignalLightRelais = new Relais(gpioController, GpioPin.RearLeftTurnSignalLightRelais);
			Relais rearRightTurnSignalLightRelais = new Relais(gpioController, GpioPin.RearRightTurnSignalLightRelais);

			TurnSignalController tsc = new TurnSignalController(turnSignalSwitch, 
			                                                    frontLeftTurnSignalLightRelais, 
			                                                    frontRightTurnSignalLightRelais, 
			                                                    rearLeftTurnSignalLightRelais, 
			                                                    rearRightTurnSignalLightRelais);
			
			// For testing ->
			while (true)
			{
				ConsoleKeyInfo key = System.Console.ReadKey();
				switch (key.Key)
				{
					case ConsoleKey.Q:
						turnSignalSwitch.StateChanged(0);
						break;
					case ConsoleKey.W:
						turnSignalSwitch.StateChanged(1);
						break;
					case ConsoleKey.E:
						turnSignalSwitch.StateChanged(2);
						break;
					case ConsoleKey.R:
						turnSignalSwitch.StateChanged(3);
						break;
					case ConsoleKey.Escape:
						return;
				}
			}
		}
	}
}
