using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
	[SerializeField] private Rigidbody2D currentBallRigidbody;
	
	private Camera _mainCamera;

	private void Start()
	{
		_mainCamera = Camera.main;
	}

	private void Update()
	{
		if (!Touchscreen.current.primaryTouch.press.isPressed)
		{
			currentBallRigidbody.isKinematic = false;
			return;
		}
		
		currentBallRigidbody.isKinematic = true;
		
		Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
		Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(touchPosition);
		
		currentBallRigidbody.position = worldPosition;
	}
}
