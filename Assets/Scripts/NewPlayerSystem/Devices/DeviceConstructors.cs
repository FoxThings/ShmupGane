using System;
using System.Collections.Generic;
using Assets.Scripts.NewPlayerSystem.Devices.HelpDevices;
using Assets.Scripts.NewPlayerSystem.Devices.Shields;

namespace Assets.Scripts.NewPlayerSystem.Devices
{
	public delegate Device DeviceConstructor();
	
	public static class DeviceConstructors
	{
		public static readonly List<DeviceConstructor> Weapons = new List<DeviceConstructor>() {
			() => new BlasterW(), () => new SpreadW(), () => new ShotGunW(),
		};
		
		public static readonly List<DeviceConstructor> Shields = new List<DeviceConstructor>() {
			() => new Shield(),
		};
		
		public static readonly List<DeviceConstructor> Devices = new List<DeviceConstructor>() {
			() => new KineticEnergyD(), () => new LocatorD(),
		};
	}
}
