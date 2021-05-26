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
		transform.Translate(Vector3.up * _speed * Time.deltaTime);

		if (transform.position.y >= _destroyPos)
		{
			Destroy(gameObject);
		}
	}
}