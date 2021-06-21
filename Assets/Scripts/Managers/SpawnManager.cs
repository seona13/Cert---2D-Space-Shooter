using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoSingleton<SpawnManager>
{
	public static event Action onWaveEnd;
	public static event Action onWaveStart;

	private bool _running;
	private bool _spawning;
	private WaitForSeconds _enemySpawnRate = new WaitForSeconds(4.5f);
	private WaitForSeconds _powerupSpawnRate = new WaitForSeconds(7f);

	[SerializeField]
	private List<Wave> _waves;
	[SerializeField]
	private int _waveCount = 0;
	private int _spawnCount = 0;
	private int _killCount = 0;



	void OnEnable()
	{
		GameManager.onGameOver += OnGameOver;
		UIManager.onCloseUpgrades += OnLevelStart;
	}


	void OnDisable()
	{
		GameManager.onGameOver -= OnGameOver;
		UIManager.onCloseUpgrades += OnLevelStart;
	}


	void Start()
	{
	}


	void OnLevelStart()
	{
		if (_running == false)
		{
			_spawning = true;
			_enemySpawnRate = new WaitForSeconds(_waves[_waveCount].spawnRate);
			StartCoroutine(SpawnEnemy());
			StartCoroutine(SpawnPowerup());
			_running = true;
		}
	}


	void OnGameOver()
	{
		_spawning = false;
	}


	IEnumerator SpawnEnemy()
	{
		while (_spawning && _spawnCount < _waves[_waveCount].enemyCount)
		{
			GameObject enemy = PoolManager.Instance.RequestEnemy();
			enemy.transform.position = new Vector3(Random.Range(-9f, 9f), 7.5f, 0);
			_spawnCount++;

			yield return _enemySpawnRate;
		}

		_running = false;
	}


	public void DespawnEnemy(Enemy enemy)
	{
		_killCount++;

		if (_killCount >= _waves[_waveCount].enemyCount)
		{
			_spawning = false;
			onWaveEnd?.Invoke();
			_waveCount++;
			onWaveStart?.Invoke();
			_spawnCount = 0;
			_killCount = 0;
		}
	}


	IEnumerator SpawnPowerup()
	{
		while (_spawning && _spawnCount < _waves[_waveCount].enemyCount)
		{
			GameObject powerup = PoolManager.Instance.RequestPowerup();
			powerup.transform.position = new Vector3(Random.Range(-9f, 9f), 7.5f, 0);

			yield return _powerupSpawnRate;
		}

		_running = false;
	}
}



[System.Serializable]
public class Wave
{
	public int enemyCount;
	public float spawnRate;
}