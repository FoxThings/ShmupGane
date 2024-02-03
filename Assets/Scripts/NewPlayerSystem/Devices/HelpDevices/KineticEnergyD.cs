using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.NewPlayerSystem.Devices.HelpDevices
{
	public class KineticEnergyD : Device
	{
		private Sequence activeSequence;
		private bool isActive = false;

		public KineticEnergyD()
		{
			kind = ModuleKind.Device;
		}
		
		public override float CalculateConsumption(float input)
		{
			if(activeSequence == null)
			{
				Start();
			}

			return isActive ? -2f : 0f;
		}

		private void Start()
		{
			var player = GameObject.FindWithTag("Player");
			var rb = player.GetComponent<Rigidbody2D>();
			
			if(rb)
			{
				activeSequence = DOTween.Sequence();
				activeSequence
					.AppendCallback(() =>
					{
						var lastActive = isActive;
						
						if(rb.velocity.sqrMagnitude > 10f)
						{
							isActive = true;
						}
						else
						{
							isActive = false;
						}

						if(lastActive != isActive)
						{
							module.UpdateModel();
						}
					})
					.SetLoops(-1);
			}
		}
	}
}
