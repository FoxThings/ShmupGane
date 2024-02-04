﻿using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Assets.Scripts.NewPlayerSystem.Devices.Shields
{
	public class Shield : Device
	{
		private readonly SpriteRenderer view;
		private readonly float maxEnergy = 60;
		private float currentEnergy = 0;
		
		private float energyInSecond = 0;
		private Sequence activeSequence;

		private bool isBroken = false;

		public Shield(GameObject obj)
		{
			this.view = obj.GetComponent<SpriteRenderer>();
			kind = ModuleKind.Shield;
		}
		
		public override float CalculateConsumption(float input)
		{
			if(activeSequence == null)
			{
				Start();
			}
			
			if(energyInSecond != input)
			{
				RestartShield();
			}
			
			energyInSecond = input;
			return input;
		}

		public bool DoDamage()
		{
			if(isBroken)
			{
				return false;
			}

			currentEnergy -= 30f;
			if(currentEnergy <= 0)
			{
				isBroken = true;
				view.enabled = false;
				currentEnergy = 0;
			}
			
			ChangeViewOpacity();
			return true;
		}
		
		private void Start()
		{
			currentEnergy = maxEnergy;
			view.enabled = true;
			ChangeViewOpacity();
		}

		private void RestartShield()
		{
			activeSequence.Pause();
			activeSequence.Kill();

			activeSequence = DOTween.Sequence();
			activeSequence
				.AppendCallback(() => currentEnergy = Mathf.Clamp(currentEnergy + energyInSecond, 0, maxEnergy))
				.AppendCallback(Update)
				.AppendInterval(1)
				.SetLoops(-1);
		}

		private void Update()
		{
			if(isBroken && currentEnergy >= maxEnergy)
			{
				isBroken = false;
				view.enabled = true;
				ChangeViewOpacity();
			}
		}

		private void ChangeViewOpacity()
		{
			var color = view.color;
			color.a = currentEnergy / maxEnergy * 0.4f;
			view.color = color;
		}
	}
}