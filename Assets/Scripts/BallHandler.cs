using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
	[SerializeField] private GameObject ballPrefab;
	[SerializeField] private Rigidbody2D pivot;
	[SerializeField	] private float delayDuration = 0.2f;
	[SerializeField] private float respawnDelay = 2f;
	
	private Rigidbody2D _currentBallRigidbody;
	private SpringJoint2D _currentBallSpringJoint;
	private Camera _mainCamera;
	private bool _isDragging;

	private void OnEnable()
	{
		EnhancedTouchSupport.Enable();
	}

	private void OnDisable()
	{
		EnhancedTouchSupport.Disable();
	}

	private void Start()
	{
		_mainCamera = Camera.main;
		
		SpawnNewBall();
	}

	private void Update()
	{
		if (_currentBallRigidbody == null)
			return;
		if (Touch.activeTouches.Count == 0)
		{
			if (_isDragging)
				LaunchBall();

			_isDragging = false;
			return;
		}

		_isDragging = true;
		_currentBallRigidbody.isKinematic = true;

		Vector2 touchPosition = new Vector2();
		
		foreach (var touch in Touch.activeTouches)
		{
			touchPosition += touch.screenPosition;
		}

		touchPosition /= Touch.activeTouches.Count;
		
		Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(touchPosition);
		
		_currentBallRigidbody.position = worldPosition;
	}

	private void SpawnNewBall()
	{
		GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

		_currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
		_currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

		_currentBallSpringJoint.connectedBody = pivot;
	}

	private void LaunchBall()
	{
		_currentBallRigidbody.isKinematic = false;
		_currentBallRigidbody = null;
		
		Invoke(nameof(DetachBall), delayDuration);
	}

	private void DetachBall()
	{
		_currentBallSpringJoint.enabled = false;
		_currentBallSpringJoint = null;
		
		Invoke(nameof(SpawnNewBall), respawnDelay);
	}
}
