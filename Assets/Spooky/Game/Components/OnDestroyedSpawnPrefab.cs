using UnityEngine;

public class OnDestroyedSpawnPrefab : GameBehavior
{
	public GameObject prefab = null;
	public bool inheritRotation = false;

	public override void Awake()
	{
		base.Awake();

		Hub.Subscribe(this);
	}
}