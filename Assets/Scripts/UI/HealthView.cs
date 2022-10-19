using Interactions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthView : MonoBehaviour
    {
        public Image Mask;

        private float originalSize;
        private Damageable player;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
            originalSize = Mask.rectTransform.rect.width;

            player.OnChangeHealth += health => { SetValue(health / player.MaxHealth); };
        }

        private void SetValue(float value)
        {
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize * value);
        }
    }
}