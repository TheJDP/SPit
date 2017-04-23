using System;
using System.Net.Sockets;
using SPit.Infrastructure;
using SPit.Interfaces;

namespace SPit.Controllers
{
	public class TcpGpioController : GpioController
	{
		private readonly string ip;
		private readonly int setGpioTcpPort;

		private TcpClient setGpioTcpClient;
		private NetworkStream setGpioTcpStream;


		public TcpGpioController(string ip, int setGpioTcpPort)
		{
			this.setGpioTcpPort = setGpioTcpPort;
			this.ip = ip;

			this.InitializeSetTcpConnection();
		}

		private void InitializeSetTcpConnectionIfNeeded()
		{
			if (this.setGpioTcpClient == null || !this.setGpioTcpClient.Connected || this.setGpioTcpStream == null)
			{
				this.DestroySetTcpConnection();
				this.InitializeSetTcpConnection();
			}
		}

		private void InitializeSetTcpConnection(int retry = 3)
		{
			if (retry == 0)
			{
				return;
			}

			try
			{
				this.setGpioTcpClient = new TcpClient(this.ip, this.setGpioTcpPort);
				this.setGpioTcpStream = this.setGpioTcpClient.GetStream();
			}
			catch (Exception e)
			{
				this.DestroySetTcpConnection();

				Console.WriteLine("ERR: TcpGpioController->InitializeSetTcpConnection(Retry: " + retry + "): " + e.Message);
				System.Threading.Thread.Sleep(1000);
				this.InitializeSetTcpConnection(retry - 1); 
			}
		}

		private void DestroySetTcpConnection()
		{
			if (this.setGpioTcpStream != null)
			{
				this.setGpioTcpStream.Close();
			}
			this.setGpioTcpStream = null;

			if (this.setGpioTcpClient != null && this.setGpioTcpClient.Connected)
			{
				this.setGpioTcpClient.Close();
			}
			this.setGpioTcpClient = null;
		}

		#region Interface implementation

		public void Set(GpioPin pin, bool state)
		{
			try
			{
				this.InitializeSetTcpConnectionIfNeeded();

				lock (setGpioTcpStream)
				{
					int gpioPin = (int)pin;

					byte data = BitConverter.GetBytes(gpioPin)[0];
					if (state)
					{
						data = (byte)(data | 0x80);
					}
					else
					{
						data = (byte)(data & 0x7F);
					}

					this.setGpioTcpStream.Write(new Byte[1] { data }, 0, 1);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("ERR: TcpGpioController->Set: " + e.Message);
			}
		}

		#endregion
	}
}
