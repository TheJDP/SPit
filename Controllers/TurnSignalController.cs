using System;
using System.Threading;
using System.Threading.Tasks;
using SPit.Devices;

namespace SPit.Controllers
{
	public class TurnSignalController
	{
		public enum TurnSignalType
		{ 
			None,
			Left,
			Right,
			All
		}

		public event Action<TurnSignalType> TurnSignalTypeChanged;

		private int turnSignalOnMilliseconds = 700;
		private int turnSignalOffMilliseconds = 500;

		private Switch turnSignalSwitch;
		private Relais frontLeftTurnSignalRelais;
		private Relais frontRightTurnSignalRelais;
		private Relais rearLeftTurnSignalRelais;
		private Relais rearRightTurnSignalRelais;

		private TurnSignalType turnSignalType;
		private Object turnToken;

		public TurnSignalController(Switch turnSignalSwitch, 
		                            Relais frontLeftTurnSignalRelais, 
		                            Relais frontRightTurnSignalRelais, 
		                            Relais rearLeftTurnSignalRelais, 
		                            Relais rearRightTurnSignalRelais)
		{
			this.rearRightTurnSignalRelais = rearRightTurnSignalRelais;
			this.rearLeftTurnSignalRelais = rearLeftTurnSignalRelais;
			this.frontRightTurnSignalRelais = frontRightTurnSignalRelais;
			this.frontLeftTurnSignalRelais = frontLeftTurnSignalRelais;
			this.turnSignalSwitch = turnSignalSwitch;

			this.InitializeSwitchEvents();
			this.TurnSignalTypeChanged += OnTurnSignalTypeChanged;
		}

		private void InitializeSwitchEvents()
		{
			if (this.turnSignalSwitch.PossibleStates == 2)
			{
				this.turnSignalSwitch.StateChanged += this.OnTurnSignalSwitchStateChanged2State;
			}
			else if (this.turnSignalSwitch.PossibleStates == 4)
			{
				this.turnSignalSwitch.StateChanged += this.OnTurnSignalSwitchStateChanged4State;
			}
		}

		private void OnTurnSignalSwitchStateChanged2State(int state)
		{
			switch (state)
			{
				case 0:
					if (this.turnSignalType != TurnSignalType.None)
					{
						this.turnSignalType = TurnSignalType.None;
						this.TurnSignalTypeChanged(this.turnSignalType);
					}
					break;
				case 1:
					if (this.turnSignalType != TurnSignalType.All)
					{
						this.turnSignalType = TurnSignalType.All;
						this.TurnSignalTypeChanged(this.turnSignalType);
					}
					break;
			}
		}

		private void OnTurnSignalSwitchStateChanged4State(int state)
		{	
			switch (state)
			{
				case 0:
					if (this.turnSignalType != TurnSignalType.None)
					{
						this.turnSignalType = TurnSignalType.None;
						this.TurnSignalTypeChanged(this.turnSignalType);
					}
					break;
				case 1:
					if (this.turnSignalType != TurnSignalType.Left)
					{
						this.turnSignalType = TurnSignalType.Left;
						this.TurnSignalTypeChanged(this.turnSignalType);
					}
					break;
				case 2:
					if(this.turnSignalType != TurnSignalType.Right)
					{
						this.turnSignalType = TurnSignalType.Right;
						this.TurnSignalTypeChanged(this.turnSignalType);
					}
					break;
				case 3:
					if (this.turnSignalType != TurnSignalType.All)
					{
						this.turnSignalType = TurnSignalType.All;
						this.TurnSignalTypeChanged(this.turnSignalType);
					}
					break;
			}
		}

		private void OnTurnSignalTypeChanged(TurnSignalType localTurnSignalType)
		{
			this.frontLeftTurnSignalRelais.Enabled = false;
			this.frontRightTurnSignalRelais.Enabled = false;
			this.rearLeftTurnSignalRelais.Enabled = false;
			this.rearRightTurnSignalRelais.Enabled = false;

			if (localTurnSignalType == TurnSignalType.None)
			{
				return;
			}

			Task.Factory.StartNew(() =>
			{
				Object localTurnToken = new Object();
				this.turnToken = localTurnToken;
				while (true)
				{
					if (localTurnToken != this.turnToken || this.turnSignalType == TurnSignalType.None)
					{
						return;
					}

					if (localTurnSignalType == TurnSignalType.All)
					{
						this.frontLeftTurnSignalRelais.Enabled = true;
						this.frontRightTurnSignalRelais.Enabled = true;
						this.rearLeftTurnSignalRelais.Enabled = true;
						this.rearRightTurnSignalRelais.Enabled = true;
					}
					else if (localTurnSignalType == TurnSignalType.Left)
					{
						this.frontLeftTurnSignalRelais.Enabled = true;
						this.rearLeftTurnSignalRelais.Enabled = true;
					}
					else if (localTurnSignalType == TurnSignalType.Right)
					{ 
						this.frontRightTurnSignalRelais.Enabled = true;
						this.rearRightTurnSignalRelais.Enabled = true;
					}

					Thread.Sleep(this.turnSignalOnMilliseconds);

					if (localTurnToken != this.turnToken || this.turnSignalType == TurnSignalType.None)
					{
						return;
					}

					if (localTurnSignalType == TurnSignalType.All)
					{
						this.frontLeftTurnSignalRelais.Enabled = false;
						this.frontRightTurnSignalRelais.Enabled = false;
						this.rearLeftTurnSignalRelais.Enabled = false;
						this.rearRightTurnSignalRelais.Enabled = false;
					}
					else if (localTurnSignalType == TurnSignalType.Left)
					{
						this.frontLeftTurnSignalRelais.Enabled = false;
						this.rearLeftTurnSignalRelais.Enabled = false;
					}
					else if (localTurnSignalType == TurnSignalType.Right)
					{
						this.frontRightTurnSignalRelais.Enabled = false;
						this.rearRightTurnSignalRelais.Enabled = false;
					}

					Thread.Sleep(this.turnSignalOffMilliseconds);
				}
			});
		}
	}
}
