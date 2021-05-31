using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PowerupType { TripleShot, Speed, Shield }

public class Powerup : MonoBehaviour
{
	public static event Action<PowerupType> onPowerupCollected;

	[SerializeField]
	private PowerupType _type;
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
			PoolManager.Instance.DespawnPowerup(gameObject);
		}
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			onPowerupCollected?.Invoke(_type);
			PoolManager.Instance.DespawnPowerup(gameObject);
		}
	}
}