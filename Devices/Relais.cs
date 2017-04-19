using System;
using SPit.Infrastructure;

namespace SPit.Devices
{
	public class Relais
	{
		public bool Enabled { get; set; }

		public Relais(GpioPin gpioPin)
		{
		}
	}
}
