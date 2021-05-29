using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
	// LASER INFO
	[SerializeField]
	private GameObject _laserPrefab;
	private Vector3 _laserOffset = new Vector3(0, 0.7f, 0);
	private float _fireRate = 0.5f;
	private float _nextFire = 0;

	// MOVING AND POSITIONING
	[SerializeField]
	private float _moveSpeed = 10f;
	private Vector3 _startPos = new Vector3(0, -2.5f, 0);
	private Vector3 _moveVec = Vector3.zero; // movement direction of player
	private float _screenLeft = -11f;
	private float _screenRight = 11f;
	private float _screenBottom = -4f;
	private float _playFieldTop = 0;

	// PLAYER DATA
	[SerializeField]
	private int _lives = 3;



	void OnEnable()
	{
		Enemy.onPlayerCollision += Damage;
	}


	void OnDisable()
	{
		Enemy.onPlayerCollision -= Damage;
	}


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
			GameObject laser = PoolManager.Instance.RequestLaser();
			laser.transform.position = transform.position + _laserOffset;
		}
	}


	void RespectBounds()
	{
		float newX = transform.position.x;

		if (transform.position.x >= _screenRight)
		{
			newX = _screenLeft;
		}
		else if (transform.position.x <= _screenLeft)
		{
			newX = _screenRight;
		}

		float newY = Mathf.Clamp(transform.position.y, _screenBottom, _playFieldTop);

		if (newX != transform.position.x || newY != transform.position.y)
		{
			transform.position = new Vector3(newX, newY, 0);
		}
	}


	void Damage()
	{
		_lives--;

		if (_lives < 1)
		{
			Destroy(gameObject);
		}
	}
}