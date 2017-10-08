using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform target;
	public Vector3 targetOffset;
	public float moveSmooth = 0.1f;
	public float maxSpeed = 10f;

	public float minY = 0f;
	public float maxY = 0f;

	private Vector3 velocity;

	void Update()
	{
		Vector3 desiredPosition = target.position + targetOffset;

		Vector3 position = transform.position;
		position = Vector3.SmoothDamp(position, desiredPosition, ref velocity, moveSmooth, maxSpeed, Time.deltaTime);

		transform.position = position;
	}
}