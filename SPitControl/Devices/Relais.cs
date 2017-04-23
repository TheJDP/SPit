using System;
using SPit.Infrastructure;
using SPit.Interfaces;

namespace SPit.Devices
{
	public class Relais
	{
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
				this.gpioController.Set(this.gpioPin, this.enabled);
			}
		}

		private readonly GpioPin gpioPin;
		private readonly GpioController gpioController;
		private bool enabled;

		public Relais(GpioController gpioController, GpioPin gpioPin)
		{
			this.gpioController = gpioController;
			this.gpioPin = gpioPin;

			this.gpioController.Set(this.gpioPin, false);
		}
	}
}
