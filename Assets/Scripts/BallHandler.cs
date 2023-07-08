using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
	[SerializeField] private Rigidbody2D currentBallRigidbody;
	[SerializeField] private SpringJoint2D currentBallSpringJoint;
	[SerializeField	] private float delayDuration = 0.5f;
	
	private Camera _mainCamera;
	private bool _isDragging;

	private void Start()
	{
		_mainCamera = Camera.main;
	}

	private void Update()
	{
		if (currentBallRigidbody == null)
			return;
		if (!Touchscreen.current.primaryTouch.press.isPressed)
		{
			if (_isDragging)
				LaunchBall();

			_isDragging = false;
			return;
		}

		_isDragging = true;
		currentBallRigidbody.isKinematic = true;
		
		Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
		Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(touchPosition);
		
		currentBallRigidbody.position = worldPosition;
	}

	private void LaunchBall()
	{
		currentBallRigidbody.isKinematic = false;
		currentBallRigidbody = null;
		
		Invoke	(nameof(DetachBall), delayDuration);
		

	}

	private void DetachBall()
	{
		currentBallSpringJoint.enabled = false;
		currentBallSpringJoint = null;
	}
}
