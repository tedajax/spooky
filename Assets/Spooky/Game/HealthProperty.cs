using UnityEngine;

[System.Serializable]
public class HealthData
{
	public float maxHealth;
	public float damagedInvulnTime;
}

public class HealthProperty
{
	private HealthData data;

	public float MaxHealth { get { return data.maxHealth; } }
	public float Health { get; private set; }

	public bool IsDead { get { return Health == 0f; } }

	public delegate void HealthHandler(HealthProperty health);

	public event HealthHandler onDamageTaken;
	public event HealthHandler onDeath;

	private float invulnTimer = 0f;

	public HealthProperty(HealthData data)
	{
		this.data = data;

		Health = MaxHealth;
	}

	public void Update(float dt)
	{
		if (invulnTimer > 0f)
		{
			invulnTimer -= dt;
		}
	}

	public void Damage(float amount)
	{
		if (IsDead)
		{
			return;
		}

		if (invulnTimer > 0f)
		{
			return;
		}

		Health -= amount;
		invulnTimer = data.damagedInvulnTime;

		if (onDamageTaken != null)
		{
			onDamageTaken(this);
		}

		if (Health <= 0f)
		{
			Health = 0f;

			if (onDeath != null)
			{
				onDeath(this);
			}
		}
	}
}