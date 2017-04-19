using System;
using SPit.Infrastructure;

namespace SPit.Devices
{
	public class Switch
	{
		public int PossibleStates { get; private set; }

		public event Action<int> StateChanged;

		public Switch(GpioPin gpioPin)
		{
			this.PossibleStates = 2;
		}

		public Switch(GpioPin gpioPin0, GpioPin gpioPin1)
		{
			this.PossibleStates = 4;
		}
	}
}
