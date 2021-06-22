using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
	public static event Action onPlayerCollision;
	public static event Action<int> onEnemyDied;

	[SerializeField]
	private float _speed = 4f;
	private bool _moving = true;

	[SerializeField]
	private int _killValue = 10;
	private float _respawnPos = -7f;
	private Vector3 _laserOffset = new Vector3(0, -1.1f, 0);
	private bool _canFire = true;

	private Animator _anim;
	private Collider2D _collider;
	private WaitForSeconds _deathDelay = new WaitForSeconds(2.35f);



	private void Awake()
	{
		_anim = GetComponent<Animator>();
		if (_anim == null)
		{
			Debug.LogError("Enemy missing Animator component.");
		}
		_anim.keepAnimatorControllerStateOnDisable = false;

		_collider = GetComponent<Collider2D>();
		if (_collider == null)
		{
			Debug.LogError("Enemy missing Collider component");
		}
	}


	private void OnEnable()
	{
		_moving = true;
		_anim.SetTrigger("EnemyResurrected");
		_collider.enabled = true;
		_canFire = true;
	}


	void Start()
	{
		StartCoroutine(FireLaserRoutine());
	}


	void Update()
	{
		if (_moving)
		{
			transform.Translate(Vector3.down * _speed * Time.deltaTime);

			if (transform.position.y <= _respawnPos)
			{
				transform.position = new Vector3(Random.Range(-9f, 9f), 7.5f, 0);
			}
		}
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			onPlayerCollision?.Invoke();
			onEnemyDied?.Invoke(_killValue / 2);
		}
		else if (other.CompareTag("Laser"))
		{
			onEnemyDied?.Invoke(_killValue);
			PoolManager.Instance.DespawnLaser(other.gameObject);
		}

		if (other.CompareTag("EnemyLaser") == false)
		{
			StartCoroutine(EnemyDied());
		}
	}


	IEnumerator FireLaserRoutine()
	{
		if (_canFire)
		{
			GameObject laser = PoolManager.Instance.RequestEnemyLaser();
			laser.transform.position = transform.position + _laserOffset;
		}
		yield return new WaitForSeconds(Random.Range(2f, 5f));
	}


	IEnumerator EnemyDied()
	{
		_collider.enabled = false;
		_moving = false;
		_canFire = false;
		_anim.SetTrigger("EnemyDied");
		yield return _deathDelay;
		SpawnManager.Instance.DespawnEnemy(this);
		PoolManager.Instance.DespawnEnemy(gameObject);
	}
}