using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
	public static event Action onPowerupCollected;

	[SerializeField]
	private float _speed = 3f;
	private float _destroyPos = -5.5f;



	void Start()
	{

	}


	void Update()
	{
		transform.Translate(Vector3.down * _speed * Time.deltaTime);

		if (transform.position.y <= _destroyPos)
		{
			PoolManager.Instance.DespawnPowerupTripleShot(gameObject);
		}
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			onPowerupCollected?.Invoke();
			PoolManager.Instance.DespawnPowerupTripleShot(gameObject);
		}
	}
}