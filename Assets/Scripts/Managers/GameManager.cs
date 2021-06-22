using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
	public static event Action onGameOver;
	public static event Action onGameStart;

	[SerializeField]
	private bool _isGameOver;
	[SerializeField]
	private GameObject _player;



	void OnEnable()
	{
		Player.onPlayerDied += GameOver;

		onGameStart?.Invoke();
	}


	void OnDisable()
	{
		Player.onPlayerDied -= GameOver;
	}


	void GameOver()
	{
		_isGameOver = true;
		onGameOver?.Invoke();
	}


	public void OnRestart()
	{
		if (_isGameOver)
		{
			_isGameOver = false;
			_player.SetActive(true);
			onGameStart?.Invoke();
		}
	}


	public void OnQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}