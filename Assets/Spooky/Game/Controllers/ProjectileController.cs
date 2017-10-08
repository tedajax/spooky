
using System;
using UnityEngine;

[Serializable]
public class ProjectileData
{
	public float speed;
	public float lifespan;
	public GameObject impactPrefab;
}

public class ProjectileController : GameBehavior
{
	public ProjectileData data;

	float lifeTimer;
	Vector2 direction;

	public override void Awake()
	{
		base.Awake();
		lifeTimer = data.lifespan;
	}

	public void Init(Vector2 direction)
	{
		this.direction = direction;
	}

	void Update()
	{
		Vector2 position = transform.position;
		position += direction * data.speed * Time.deltaTime;
		transform.position = position;

		if (lifeTimer > 0f)
		{
			lifeTimer -= Time.deltaTime;
			if (lifeTimer <= 0f)
			{
				destroy();
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		GameObject targetObj = collision.gameObject;

		GameBehaviorHub hub = targetObj.GetComponent<GameBehaviorHub>();
		if (hub != null)
		{
			hub.SendEvent(new DamageEvent() { damage = 1 }, this);
		}

		destroy();
	}

	void destroy()
	{
		if (data.impactPrefab != null)
		{
			Instantiate(data.impactPrefab, transform.position, Quaternion.identity);
		}

		Destroy(gameObject);
	}
}
