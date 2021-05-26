using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
	[SerializeField]
	private GameObject _laserPrefab;
	private Vector3 _laserOffset = new Vector3(0, 0.7f, 0);
	[SerializeField]
	private float _moveSpeed = 10f;
	private Vector3 _startPos = new Vector3(0, -2.5f, 0);
	private Vector3 _moveVec = Vector3.zero; // movement direction of player
	[SerializeField]
	private float _fireRate = 0.5f;
	private float _nextFire = 0;



	void Start()
	{
		transform.position = _startPos;
	}


	void Update()
	{
		transform.Translate(_moveVec * _moveSpeed * Time.deltaTime);
		RespectBounds();
	}


	public void OnMove(InputValue input)
	{
		Vector2 inputVec = input.Get<Vector2>();

		_moveVec = new Vector3(inputVec.x, inputVec.y, 0);
	}


	public void OnFire()
	{
		if (Time.time > _nextFire)
		{
			_nextFire = Time.time + _fireRate;
			Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
		}
	}


	void RespectBounds()
	{
		float newX = transform.position.x;

		if (transform.position.x >= 11f)
		{
			newX = -11f;
		}
		else if (transform.position.x <= -11f)
		{
			newX = 11f;
		}

		float newY = Mathf.Clamp(transform.position.y, -4f, 0);

		if (newX != transform.position.x || newY != transform.position.y)
		{
			transform.position = new Vector3(newX, newY, 0);
		}
	}
}