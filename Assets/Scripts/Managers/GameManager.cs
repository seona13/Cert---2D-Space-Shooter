using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
	public static event Action onGameOver;
	public static event Action onGameRestart;

	[SerializeField]
	private bool _isGameOver;
	[SerializeField]
	private GameObject _player;



	private void OnEnable()
	{
		Player.onPlayerDied += GameOver;
	}


	private void OnDisable()
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
			onGameRestart?.Invoke();
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