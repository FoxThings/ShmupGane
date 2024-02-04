using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.NewPlayerSystem;
using Assets.Scripts.NewPlayerSystem.Devices;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

namespace UI
{
	public class NewDevicePanel : MonoBehaviour
	{
		public GameObject panel;
		public GameObject currentDeviceIco;
		public Text currentDescription;

		public Sprite WeaponS;
		public Sprite DeviceS;
		public Sprite ShieldS;

		public List<Button> buttons;
		
		public NewPlayerController player;
		
		private Action activeCallback = null;
		private Device currentDevice;
		
		public void ActivateWeaponChangeProcess(Action callback, bool isStart)
		{
			activeCallback = callback;
			Time.timeScale = 0;

			UpdateButtons();
			ProcessNewDevice(GenerateNewDevice(isStart));
			panel.SetActive(true);
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

		private Device GenerateNewDevice(bool isStart)
		{
			List<DeviceConstructor> sample = DeviceConstructors.Weapons
				.Concat(DeviceConstructors.Devices)
				.Concat(DeviceConstructors.Shields)
				.ToList();

			if(isStart)
			{
				sample = sample.Concat(DeviceConstructors.Weapons).ToList();
			}

			return sample[Random.Range(0, sample.Count)].Invoke();
		}

		private void ProcessNewDevice(Device device)
		{
			currentDescription.text = device.description;
			currentDevice = device;
			
			currentDeviceIco.GetComponent<Image>().sprite = device.kind switch {
				ModuleKind.Device => DeviceS,
				ModuleKind.Weapon => WeaponS,
				_ => ShieldS
			};
		}

		private void Exit()
		{
			Time.timeScale = 1;
			panel.SetActive(false);
			
			activeCallback?.Invoke();
			activeCallback = null;
		}
		
		private void UpdateButtons()
		{
			List<Module> modules = player.GetModel().GetAllModules();
			for(int i = 1; i < modules.Count; ++i)
			{
				if(modules[i].IsAttached())
				{
					Button b = buttons[i - 1];
					var c = b.colors;
					c.normalColor = Color.black;
					c.pressedColor = Color.black;
					c.highlightedColor = Color.black;

					b.interactable = false;
				}
			}
		}
	}
}
