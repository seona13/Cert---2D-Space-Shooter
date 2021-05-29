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

	[Space(10)]

	[SerializeField]
	private Transform _enemyContainer;
	[SerializeField]
	private Transform _laserContainer;

	private List<GameObject> _enemyPool;
	private List<GameObject> _laserPool;



	public override void Init()
	{
		base.Init();
		_enemyPool = new List<GameObject>();
		_laserPool = new List<GameObject>();
	}


	void Start()
	{
		GenerateEnemies(_defaultPoolSize);
		GenerateLasers(_defaultPoolSize);
	}


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
}