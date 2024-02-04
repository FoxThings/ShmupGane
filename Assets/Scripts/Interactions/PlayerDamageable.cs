using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.NewPlayerSystem;
using Assets.Scripts.NewPlayerSystem.Devices.Shields;

namespace Interactions
{
    public class PlayerDamageable : Damageable
    {
        private NewPlayerController player;
        
        protected override void Start()
        {
            base.Start();
            
            player = GetComponent<NewPlayerController>();
        }

        protected override void ChangeHealth(float damage)
        {
            List<Shield> shields = player.GetModel().GetShields();
            if(shields.Any(shield => shield.DoDamage()))
            {
                return;
            }
            
            OnChangeHealth?.Invoke(0);
        }
    }
}
