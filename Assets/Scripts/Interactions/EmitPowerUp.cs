using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public class EmitPowerUp : MonoBehaviour
    {
        public List<GameObject> PowerUps;
        public int PowerUpChance;

        private void Emit()
        {
            if (Random.Range(0, 100) + 1 < PowerUpChance)
            {
                var powerUp = PowerUps[Random.Range(0, PowerUps.Count)];
                Instantiate(powerUp, transform.position, Quaternion.identity);
            }
        }

        private void Start()
        {
            if (TryGetComponent<Destroyable>(out var destroyer))
            {
                if (destroyer == null) return;
                destroyer.OnDestroyFinish += Emit;
            }
        }
    }
}