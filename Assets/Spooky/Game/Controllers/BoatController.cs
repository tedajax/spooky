using UnityEngine;
using System;

[Serializable]
public class BoatData
{
	public float thrustPower;
}
	
public class BoatController : MonoBehaviour
{
	public BoatData data;

	private Vector3 thrustForce = Vector3.zero;

	private Rigidbody rigidBody;

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		float thrustInput = Input.GetAxis("Thrust");

		thrustForce = transform.forward * thrustInput * data.thrustPower;
	}
	
	void FixedUpdate()
	{
		rigidBody.AddForce(thrustForce, ForceMode.Force);
	}
}