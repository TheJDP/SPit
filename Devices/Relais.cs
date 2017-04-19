using System;
using SPit.Infrastructure;

namespace SPit.Devices
{
	public class Relais
	{
		private bool enabled;
		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				if (this.enabled == value)
				{
					return;
				}

				this.enabled = value;
				Console.WriteLine("GPIO Pin " + this.gpioPin + " Output: " + this.enabled);
			}
		}

		readonly GpioPin gpioPin;

		public Relais(GpioPin gpioPin)
		{
			this.gpioPin = gpioPin;
		}
	}
}
