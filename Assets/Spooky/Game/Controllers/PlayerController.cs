using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerControllerData
{
	public float normalSpeed;
	public float acceleration;

	public GunData boneThrowerData;
}

[Serializable]
public class GunData
{
	public GameObject projectilePrefab;
	public float fireInterval;
	public AudioClip fireSoundClip;
}

public class PlayerController : GameBehavior
{
	public PlayerControllerData data;
	public Transform bulletSpawnPoint;

	private GameEventSystem events { get { return GameSystems.Instance.Events; } }

	private float speed = 0f;
	private float fireTimer = 0f;

	public override void Awake()
	{
		base.Awake();
		speed = data.normalSpeed;
	}

	void Update()
	{
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

		Vector2 position = transform.position;
		position += input * speed * Time.deltaTime;
		transform.position = position;

		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetButton("Fire1"))
		{
			fireTimer -= Time.deltaTime;
			if (fireTimer <= 0f)
			{
				fireTimer += data.boneThrowerData.fireInterval;
				GameObject obj = Instantiate(data.boneThrowerData.projectilePrefab, bulletSpawnPoint.position, Quaternion.identity);
				var projectile = obj.GetComponent<ProjectileController>();

				Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

				projectile.Init(direction);
			}
		}
		else
		{
			fireTimer -= Time.deltaTime;
			fireTimer = Mathf.Max(fireTimer, 0f);
		}
	}
}