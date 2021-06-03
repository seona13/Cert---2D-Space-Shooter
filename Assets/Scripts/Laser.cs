using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Laser : MonoBehaviour
{
	[SerializeField]
	private float _speed = 8f;
	private float _destroyPos = 7.5f;



	void Start()
	{

	}


	void Update()
	{
		if (gameObject.CompareTag("Laser"))
		{
			transform.Translate(Vector3.up * _speed * Time.deltaTime);

			if (transform.position.y >= _destroyPos)
			{
				if (transform.parent.name == "TripleShot(Clone)")
				{
					Destroy(transform.parent.gameObject);
				}
				else
				{
					PoolManager.Instance.DespawnLaser(gameObject);
				}
			}
		}
		else if (gameObject.CompareTag("EnemyLaser"))
		{
			transform.Translate(Vector3.down * _speed * Time.deltaTime);

			if (transform.position.y <= -_destroyPos)
			{
				PoolManager.Instance.DespawnEnemyLaser(gameObject);
			}
		}
	}
}