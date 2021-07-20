using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
	public static event Action onPlayerDied;
	public static event Action onPlayerDamaged;
	public static event Action<int> onLaserFired;
	public static event Action<int> onShieldCountChanged;
	public static event Action onAmmoCollected;

	// LASER INFO
	private Vector3 _laserOffset = new Vector3(0, 1.1f, 0);
	private float _fireRate = 0.5f;
	private float _nextFire = 0;

	// MOVING AND POSITIONING
	private Vector3 _startPos = new Vector3(0, -2.5f, 0);
	private Vector3 _moveVec = Vector3.zero; // movement direction of player
	private float _screenLeft = -11f;
	private float _screenRight = 11f;
	private float _screenBottom = -4f;
	private float _playFieldTop = 0;

	// PLAYER DATA
	[Header("Player Damage")]
	[SerializeField]
	private GameObject _damageLeft;
	[SerializeField]
	private GameObject _damageRight;

	// POWER UPS
	[Header("Power Ups")]
	[SerializeField]
	private float _powerupDuration = 5f;
	private WaitForSeconds _powerupWaitDuration;
	[SerializeField]
	private GameObject _tripleShotPrefab;
	private bool _tripleShotActive;
	[SerializeField]
	private GameObject _shieldVisual;
	private bool _shieldActive;
	private int _shieldCount = 0;



	void OnEnable()
	{
		Enemy.onPlayerCollision += Damage;
		PlayerData.onUpdateLives += SetDamageIndicators;
		Powerup.onPowerupCollected += CollectPowerup;
		GameManager.onGameStart += NewGame;
	}


	void OnDisable()
	{
		Enemy.onPlayerCollision -= Damage;
		PlayerData.onUpdateLives -= SetDamageIndicators;
		Powerup.onPowerupCollected -= CollectPowerup;
		GameManager.onGameStart -= NewGame;
	}


	void Start()
	{
		_powerupWaitDuration = new WaitForSeconds(_powerupDuration);
	}


	void Update()
	{
		transform.Translate(_moveVec * PlayerData.Instance.GetMoveSpeed() * Time.deltaTime);

		RespectBounds();
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("EnemyLaser"))
		{
			Damage();
		}
	}


	public void OnMove(InputAction.CallbackContext context)
	{
		_moveVec = context.ReadValue<Vector2>();
	}


	public void OnFire(InputAction.CallbackContext context)
	{
		if (context.performed && PlayerData.Instance.GetAmmoCount() > 0 && Time.time > _nextFire)
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

			onLaserFired?.Invoke(-1);
		}
	}


	void NewGame()
	{
		gameObject.SetActive(true);
		transform.position = _startPos;
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
			_shieldCount--;
			onShieldCountChanged?.Invoke(_shieldCount);

			if (_shieldCount <= 0)
			{
				_shieldVisual.SetActive(false);
				_shieldActive = false;
			}
			return;
		}

		onPlayerDamaged?.Invoke();

		if (PlayerData.Instance.GetLives() < 1)
		{
			onPlayerDied?.Invoke();
			gameObject.SetActive(false);
		}
	}


	void SetDamageIndicators(int lives)
	{
		if (lives == 3)
		{
			_damageLeft.SetActive(false);
			_damageRight.SetActive(false);
		}
		else if (lives == 2)
		{
			_damageLeft.SetActive(true);
			_damageRight.SetActive(false);
		}
		else if (lives == 1)
		{
			_damageLeft.SetActive(true);
			_damageRight.SetActive(true);
		}
	}


	void CollectPowerup(PowerupType type)
	{
		switch (type)
		{
			case PowerupType.TripleShot:
				StartCoroutine(TripleShotActive());
				break;
			case PowerupType.Shield:
				_shieldVisual.SetActive(true);
				_shieldActive = true;
				_shieldCount = 3;
				onShieldCountChanged?.Invoke(_shieldCount);
				break;
			case PowerupType.Ammo:
				onAmmoCollected?.Invoke();
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
}