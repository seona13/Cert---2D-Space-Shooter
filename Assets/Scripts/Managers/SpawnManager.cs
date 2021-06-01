using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoSingleton<SpawnManager>
{
	private bool _running;
	private bool _spawning;
	private WaitForSeconds _enemySpawnRate = new WaitForSeconds(4.5f);
	private WaitForSeconds _powerupSpawnRate = new WaitForSeconds(7f);



	private void OnEnable()
	{
		GameManager.onGameOver += OnGameOver;
		GameManager.onGameRestart += StartSpawning;
	}


	private void OnDisable()
	{
		GameManager.onGameOver -= OnGameOver;
		GameManager.onGameRestart -= StartSpawning;
	}


	void Start()
	{
		if (_running == false)
		{
			StartSpawning();
			StartCoroutine(SpawnEnemy());
			StartCoroutine(SpawnPowerup());
			_running = true;
		}
	}


	void StartSpawning()
	{
		_spawning = true;
	}


	void OnGameOver()
	{
		_spawning = false;
	}


	IEnumerator SpawnEnemy()
	{
		while (_spawning)
		{
			GameObject enemy = PoolManager.Instance.RequestEnemy();
			enemy.transform.position = new Vector3(Random.Range(-9f, 9f), 7.5f, 0);

			yield return _enemySpawnRate;
		}

		_running = false;
	}


	IEnumerator SpawnPowerup()
	{
		while (_spawning)
		{
			GameObject powerup = PoolManager.Instance.RequestPowerup();
			powerup.transform.position = new Vector3(Random.Range(-9f, 9f), 7.5f, 0);

			yield return _powerupSpawnRate;
		}

		_running = false;
	}
}