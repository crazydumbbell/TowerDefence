using UnityEngine;
using CW.Common;

namespace Lean.Common
{
	/// <summary>This script moves the ball left or right and resets it if it goes out of bounds.</summary>
	[RequireComponent(typeof(Rigidbody2D))]
	[HelpURL(LeanCommon.PlusHelpUrlPrefix + "LeanPongBall")]
	[AddComponentMenu(LeanCommon.ComponentPathPrefix + "Pong Ball")]
	public class LeanPongBall : MonoBehaviour
	{
		/// <summary>Starting horizontal speed of the ball.</summary>
		public float StartSpeed { set { startSpeed = value; } get { return startSpeed; } } [SerializeField] private float startSpeed = 2.0f;

		/// <summary>Starting vertical speed of the ball.</summary>
		public float Spread { set { spread = value; } get { return spread; } } [SerializeField] private float spread = 0.5f;

		/// <summary>The acceleration per second.</summary>
		public float Acceleration { set { acceleration = value; } get { return acceleration; } } [SerializeField] private float acceleration = 1.0f;

		/// <summary>If the ball goes this many units from the center, it will reset.</summary>
		public float Bounds { set { bounds = value; } get { return bounds; } } [SerializeField] private float bounds = 18.0f;

		// The current rigidbody
		private Rigidbody2D cachedBody;

		// The current speed of the ball
		private float speed;
		
		protected virtual void Awake()
		{
			// Store the rigidbody component attached to this GameObject
			cachedBody = GetComponent<Rigidbody2D>();

			// Reset the ball
			ResetPositionAndVelocity();
		}

		protected virtual void FixedUpdate()
		{
			// Is the position out of bounds?
			if (transform.localPosition.magnitude > bounds)
			{
				ResetPositionAndVelocity();
			}

			// Increase speed value
			speed += acceleration * Time.deltaTime;

			// Reset velocity magnitude to new speed
			#if UNITY_6000_0_OR_NEWER
				cachedBody.linearVelocity = cachedBody.linearVelocity.normalized * speed;
			#else
				cachedBody.velocity = cachedBody.velocity.normalized * speed;
			#endif
		}

		private void ResetPositionAndVelocity()
		{
			// Reset position
			transform.localPosition = Vector3.zero;

			// Reset speed value
			speed = startSpeed;

			#if UNITY_6000_0_OR_NEWER
				var velocity = cachedBody.linearVelocity;
			#else
				var velocity = cachedBody.velocity;
			#endif

			// If moving right, reset velocity to the left
			if (velocity.x > 0.0f)
			{
				velocity = new Vector2(-speed, Random.Range(-spread, spread));
			}
			// If moving left, reset velocity to the right
			else
			{
				velocity = new Vector2(speed, Random.Range(-spread, spread));
			}

			#if UNITY_6000_0_OR_NEWER
				cachedBody.linearVelocity = velocity;
			#else
				cachedBody.velocity = velocity;
			#endif
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Common.Editor
{
	using UnityEditor;
	using TARGET = LeanPongBall;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class LeanPongBall_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("startSpeed", "Starting horizontal speed of the ball.");
			Draw("spread", "Starting vertical speed of the ball.");
			Draw("acceleration", "The acceleration per second.");
			Draw("bounds", "If the ball goes this many units from the center, it will reset.");
		}
	}
}
#endif