using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
	public static event Action onPlayerCollision;

	[SerializeField]
	private float _speed = 4f;
	private float _respawnPos = -7f;



	void Start()
	{

	}


	void Update()
	{
		transform.Translate(Vector3.down * _speed * Time.deltaTime);

		if (transform.position.y <= _respawnPos)
		{
			transform.position = new Vector3(Random.Range(-9f, 9f), 7.5f, 0);
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			onPlayerCollision?.Invoke();
			PoolManager.Instance.DespawnEnemy(gameObject);
		}
		else if (other.CompareTag("Laser"))
		{
			PoolManager.Instance.DespawnLaser(other.gameObject);
			PoolManager.Instance.DespawnEnemy(gameObject);
		}
	}
}