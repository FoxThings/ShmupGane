using UnityEngine;

namespace Assets.Scripts.NewPlayerSystem.Effects
{
	public class AutoNavigate : MonoBehaviour
	{
		public LayerMask mask;
		public float force;
		
		private GameObject target;
		private Rigidbody2D rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}
		
		private void FixedUpdate()
		{
			if(target == null)
			{
				var result = Physics2D.CircleCast(transform.position, 3, Vector2.zero, 0, mask);
				if(result)
				{
					target = result.transform.gameObject;
				}
				
				return;
			}

			Vector2 redirect = target.transform.position.x < transform.position.x ? Vector2.left : Vector2.right;
			rb.AddForce(redirect * force);
		}
	}
}
