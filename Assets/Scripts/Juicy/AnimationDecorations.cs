using UnityEngine;

namespace Juicy
{
    public class AnimationDecorations : MonoBehaviour
    {
        public void EmitRippleEffect()
        {
            if (Camera.main != null)
                Camera.main.GetComponent<RippleEffect>()
                    .Emit(Camera.main.WorldToViewportPoint(transform.position));
        }
    }
}
