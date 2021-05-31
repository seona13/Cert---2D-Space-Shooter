using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoSingleton<PoolManager>
{
	[SerializeField]
	private int _defaultPoolSize = 10;
	[SerializeField]
	private GameObject _enemyPrefab;
	[SerializeField]
	private GameObject _laserPrefab;
	[SerializeField]
	private GameObject[] _powerupPrefabs;

	[Space(10)]

	[SerializeField]
	private Transform _enemyContainer;
	[SerializeField]
	private Transform _laserContainer;
	[SerializeField]
	private Transform _powerupContainer;

	private List<GameObject> _enemyPool;
	private List<GameObject> _laserPool;
	private List<GameObject> _powerupPool;

	private int _powerupCounter = 0;



	public override void Init()
	{
		base.Init();
		_enemyPool = new List<GameObject>();
		_laserPool = new List<GameObject>();
		_powerupPool = new List<GameObject>();
	}


	void Start()
	{
		GenerateEnemies(_defaultPoolSize);
		GenerateLasers(_defaultPoolSize);
		GeneratePowerups(_defaultPoolSize);
	}


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


	#region Lasers
	List<GameObject> GenerateLasers(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject laser = Instantiate(_laserPrefab);
			laser.transform.parent = _laserContainer;
			laser.SetActive(false);

			_laserPool.Add(laser);
		}

		return _laserPool;
	}


	public GameObject RequestLaser()
	{
		foreach (GameObject laser in _laserPool)
		{
			if (laser.activeInHierarchy == false)
			{
				laser.SetActive(true);
				return laser;
			}
		}

		GameObject newLaser = Instantiate(_laserPrefab);
		newLaser.transform.parent = _laserContainer;
		_laserPool.Add(newLaser);

		return newLaser;
	}


	public void DespawnLaser(GameObject laser)
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