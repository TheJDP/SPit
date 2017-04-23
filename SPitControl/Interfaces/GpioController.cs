using System;
using SPit.Infrastructure;

namespace SPit.Interfaces
{
	public interface GpioController
	{
		void Set(GpioPin pin, bool state);
	}
}
