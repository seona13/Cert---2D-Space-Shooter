using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoSingleton<SpawnManager>
{
	private bool _running;
	private bool _spawning;
	private WaitForSeconds _spawnRate = new WaitForSeconds(4.5f);



	private void OnEnable()
	{
		Player.onPlayerDied += OnGameOver;
	}


	private void OnDisable()
	{
		Player.onPlayerDied -= OnGameOver;
	}


	void Start()
	{
		if (_running == false)
		{
			_spawning = true;
			StartCoroutine(SpawnEnemy());
			_running = true;
		}
	}


	IEnumerator SpawnEnemy()
	{
		while (_spawning)
		{
			GameObject enemy = PoolManager.Instance.RequestEnemy();
			enemy.transform.position = new Vector3(Random.Range(-9f, 9f), 7.5f, 0);

			yield return _spawnRate;
		}

		_running = false;
	}


	void OnGameOver()
	{
		_spawning = false;
	}
}