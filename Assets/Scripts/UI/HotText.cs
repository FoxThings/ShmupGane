using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HotText : MonoBehaviour
    {
        public TMP_Text Text;
        private static readonly int GlowPower = Shader.PropertyToID("_GlowPower");

        public void ShowText(string text, float timeout)
        {
            Text.text = text;
            var sequence = DOTween.Sequence();
            sequence.Append(Text.DOFade(0, 0));
            sequence.AppendCallback(() => gameObject.SetActive(true));
            sequence.Append(Text.DOFade(1, 1));
            sequence.AppendCallback(DoAnim);
            sequence.AppendInterval(timeout);
            sequence.Append(Text.DOFade(0, 1));
            sequence.AppendCallback(() => gameObject.SetActive(false));
        }

        private void DoAnim()
        {
            var mat = Text.fontSharedMaterial;
            mat.SetFloat(GlowPower, 0.25f);
            DOTween.To(() => mat.GetFloat(GlowPower), (value) => mat.SetFloat(GlowPower, value), 0.1f, 0.5f)
                .SetEase(Ease.InOutSine).SetLoops(-1).SetLink(gameObject, LinkBehaviour.KillOnDisable);
        }
    
    }
}
