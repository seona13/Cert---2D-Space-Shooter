using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoSingleton<PoolManager>
{
	[SerializeField]
	private int _defaultPoolSize = 10;
	[SerializeField]
	private GameObject _playerLaserPrefab;
	[SerializeField]
	private GameObject _enemyPrefab;
	[SerializeField]
	private GameObject _enemyLaserPrefab;
	[SerializeField]
	private GameObject[] _powerupPrefabs;

	[Space(10)]

	[SerializeField]
	private Transform _enemyContainer;
	[SerializeField]
	private Transform _laserContainer;
	[SerializeField]
	private Transform _powerupContainer;

	private List<GameObject> _playerLaserPool;
	private List<GameObject> _enemyPool;
	private List<GameObject> _enemyLaserPool;
	private List<GameObject> _powerupPool;

	private int _powerupCounter = 0;



	public override void Init()
	{
		base.Init();
		_playerLaserPool = new List<GameObject>();
		_enemyPool = new List<GameObject>();
		_enemyLaserPool = new List<GameObject>();
		_powerupPool = new List<GameObject>();
	}


	void Start()
	{
		GenerateLasers(_defaultPoolSize);
		GenerateEnemies(_defaultPoolSize);
		GenerateEnemyLasers(_defaultPoolSize);
		GeneratePowerups(_defaultPoolSize * 2);
	}


	#region Lasers
	List<GameObject> GenerateLasers(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject laser = Instantiate(_playerLaserPrefab);
			laser.transform.parent = _laserContainer;
			laser.SetActive(false);

			_playerLaserPool.Add(laser);
		}

		return _playerLaserPool;
	}


	public GameObject RequestLaser()
	{
		foreach (GameObject laser in _playerLaserPool)
		{
			if (laser.activeInHierarchy == false)
			{
				laser.SetActive(true);
				return laser;
			}
		}

		GameObject newLaser = Instantiate(_playerLaserPrefab);
		newLaser.transform.parent = _laserContainer;
		_playerLaserPool.Add(newLaser);

		return newLaser;
	}


	public void DespawnLaser(GameObject laser)
	{
		laser.SetActive(false);
	}
	#endregion


	#region Enemies
	List<GameObject> GenerateEnemies(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject enemy = Instantiate(_enemyPrefab);
			enemy.transform.parent = _enemyContainer;
			enemy.SetActive(false);

			_enemyPool.Add(enemy);
		}

		return _enemyPool;
	}


	public GameObject RequestEnemy()
	{
		foreach (GameObject enemy in _enemyPool)
		{
			if (enemy.activeInHierarchy == false)
			{
				enemy.SetActive(true);
				return enemy;
			}
		}

		GameObject newEnemy = Instantiate(_enemyPrefab);
		newEnemy.transform.parent = _enemyContainer;
		_enemyPool.Add(newEnemy);

		return newEnemy;
	}


	public void DespawnEnemy(GameObject enemy)
	{
		enemy.SetActive(false);
	}
	#endregion


	#region Enemy Lasers
	List<GameObject> GenerateEnemyLasers(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject laser = Instantiate(_enemyLaserPrefab);
			laser.transform.parent = _laserContainer;
			laser.SetActive(false);

			_enemyLaserPool.Add(laser);
		}

		return _enemyLaserPool;
	}


	public GameObject RequestEnemyLaser()
	{
		foreach (GameObject laser in _enemyLaserPool)
		{
			if (laser.activeInHierarchy == false)
			{
				laser.SetActive(true);
				return laser;
			}
		}

		GameObject newLaser = Instantiate(_enemyLaserPrefab);
		newLaser.transform.parent = _laserContainer;
		_enemyLaserPool.Add(newLaser);

		return newLaser;
	}


	public void DespawnEnemyLaser(GameObject laser)
	{
		laser.SetActive(false);
	}
	#endregion


	#region Powerups
	List<GameObject> GeneratePowerups(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject powerup = Instantiate(_powerupPrefabs[Random.Range(0, _powerupPrefabs.Length)]);
			powerup.transform.parent = _powerupContainer;
			powerup.SetActive(false);

			_powerupPool.Add(powerup);
		}

		return _powerupPool;
	}


	public GameObject RequestPowerup()
	{
		if (_powerupCounter < _powerupPool.Count)
		{
			if (_powerupPool[_powerupCounter].activeInHierarchy == false)
			{
				GameObject powerup = _powerupPool[_powerupCounter];
				powerup.SetActive(true);
				_powerupCounter++;
				return powerup;
			}
		}

		_powerupCounter = 0;
		GameObject firstPowerup = _powerupPool[_powerupCounter];
		firstPowerup.SetActive(true);
		return firstPowerup;
	}


	public void DespawnPowerup(GameObject powerup)
	{
		powerup.SetActive(false);
	}
	#endregion
}