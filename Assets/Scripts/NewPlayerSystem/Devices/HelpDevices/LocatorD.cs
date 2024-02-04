using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.NewPlayerSystem.Devices.HelpDevices
{
	public class LocatorD : Device
	{
		private bool isActive = false;

		public LocatorD()
		{
			kind = ModuleKind.Device;
			description = "Локатор\nСледующее оружие в цепи получает эффект автоматичекой наводки на врагов";
		}
		
		public override float CalculateConsumption(float input)
		{
			if(!isActive)
			{
				Start();
			}

			return input / 2;
		}

		private void Start()
		{
			isActive = true;

			var result = module.GetNeighbourDevices()
				.Where(device => device.kind == ModuleKind.Weapon)
				.Select(device => device as Weapon);

			foreach (var weapon in result)
			{
				if(weapon != null)
				{
					weapon.bulletObj = Resources.Load<GameObject>("NavigableBullet");
				}
			}
		}
	}
}
