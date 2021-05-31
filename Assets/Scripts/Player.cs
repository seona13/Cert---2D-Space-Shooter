using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
	public static event Action onPlayerDied;

	// LASER INFO
	private Vector3 _laserOffset = new Vector3(0, 1.1f, 0);
	private float _fireRate = 0.5f;
	private float _nextFire = 0;

	// POWER UPS
	[SerializeField]
	private float _powerupDuration = 5f;
	private WaitForSeconds _powerupWaitDuration;
	[SerializeField]
	private GameObject _tripleShotPrefab;
	[SerializeField]
	private bool _tripleShotActive;
	[SerializeField]
	private float _speedMultiplier = 3f;
	[SerializeField]
	private bool _speedActive;
	[SerializeField]
	private GameObject _shieldVisual;
	[SerializeField]
	private bool _shieldActive;

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
		Powerup.onPowerupCollected += CollectPowerup;
	}


	void OnDisable()
	{
		Enemy.onPlayerCollision -= Damage;
		Powerup.onPowerupCollected -= CollectPowerup;
	}


	void Start()
	{
		transform.position = _startPos;
		_powerupWaitDuration = new WaitForSeconds(_powerupDuration);
	}


	void Update()
	{
		if (_speedActive)
		{
			transform.Translate(_moveVec * _moveSpeed * _speedMultiplier * Time.deltaTime);
		}
		else
		{
			transform.Translate(_moveVec * _moveSpeed * Time.deltaTime);
		}

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

			if (_tripleShotActive)
			{
				GameObject shot = Instantiate(_tripleShotPrefab);
				shot.transform.position = transform.position;
			}
			else
			{
				GameObject laser = PoolManager.Instance.RequestLaser();
				laser.transform.position = transform.position + _laserOffset;
			}
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
		if (_shieldActive)
		{
			_shieldVisual.SetActive(false);
			_shieldActive = false;
			return;
		}

		_lives--;

		if (_lives < 1)
		{
			onPlayerDied?.Invoke();
			Destroy(gameObject);
		}
	}


	void CollectPowerup(PowerupType type)
	{
		switch (type)
		{
			case PowerupType.TripleShot:
				StartCoroutine(TripleShotActive());
				break;
			case PowerupType.Speed:
				StartCoroutine(SpeedActive());
				break;
			case PowerupType.Shield:
				_shieldVisual.SetActive(true);
				_shieldActive = true;
				break;
			default:
				Debug.LogWarning("Player::CollectPowerup -- Unknown powerup type detected");
				break;
		}
	}


	IEnumerator TripleShotActive()
	{
		_tripleShotActive = true;
		yield return _powerupWaitDuration;
		_tripleShotActive = false;
	}


	IEnumerator SpeedActive()
	{
		_speedActive = true;
		yield return _powerupWaitDuration;
		_speedActive = false;
	}
}