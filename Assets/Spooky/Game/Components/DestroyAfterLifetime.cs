using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
	public float lifetime = 0f;

	void Update()
	{
		if (lifetime > 0f)
		{
			lifetime -= Time.deltaTime;
			if (lifetime <= 0f)
			{
				Destroy(gameObject);
			}
		}
	}
}