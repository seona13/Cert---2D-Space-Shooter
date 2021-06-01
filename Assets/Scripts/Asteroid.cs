using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asteroid : MonoBehaviour
{
	public static event Action onStartSpawning;

	[SerializeField]
	private float _rotationSpeed = 3f;
	[SerializeField]
	private GameObject _explosionPrefab;



	void OnEnable()
	{
		GetComponent<Collider2D>().enabled = true;
	}


	void Update()
	{
		transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Laser")
		{
			PoolManager.Instance.DespawnLaser(other.gameObject);
			GetComponent<Collider2D>().enabled = false;
			Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
			onStartSpawning?.Invoke();
			Destroy(gameObject, 0.5f);
		}
	}
}