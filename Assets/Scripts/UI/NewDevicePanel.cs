using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.NewPlayerSystem;
using Assets.Scripts.NewPlayerSystem.Devices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
	public class NewDevicePanel : MonoBehaviour
	{
		public GameObject panel;
		public Image currentDeviceIco;
		public TMP_Text currentDescription;
		
		public NewPlayerController player;
		
		private Action activeCallback = null;
		private Device currentDevice;
		
		public void ActivateWeaponChangeProcess(Action callback)
		{
			activeCallback = callback;
			Time.timeScale = 0;
			panel.SetActive(true);
			
			ProcessNewDevice(GenerateNewDevice());
		}

		public void ProcessButtonClick(int index)
		{
			if(player.AddDevice(index, currentDevice))
			{
				Exit();
			}
		}

		public void ProcessCancelButtonClick()
		{
			Exit();
		}
		
		private Device GenerateNewDevice()
		{
			List<DeviceConstructor> sample = DeviceConstructors.Weapons
				.Concat(DeviceConstructors.Devices)
				.Concat(DeviceConstructors.Shields)
				.ToList();

			return sample[Random.Range(0, sample.Count)].Invoke();
		}

		private void ProcessNewDevice(Device device)
		{
			currentDescription.text = device.description;
			currentDevice = device;
		}

		private void Exit()
		{
			Time.timeScale = 1;
			panel.SetActive(false);
			
			activeCallback?.Invoke();
			activeCallback = null;
		}
	}
}
