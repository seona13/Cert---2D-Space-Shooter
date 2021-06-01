using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Text _scoreText;
	[SerializeField]
	private Image _livesCounter;
	[SerializeField]
	private Sprite[] _livesCounterImages;

	[Space(10)]

	[SerializeField]
	private GameObject _gameOverScreen;
	[SerializeField]
	private Text _gameOverText;
	private bool _isGameOver = false;
	private WaitForSeconds _flashSpeed = new WaitForSeconds(1f);



	void OnEnable()
	{
		Player.onUpdateScore += UpdateScore;
		Player.onUpdateLives += UpdateLives;
		GameManager.onGameOver += ShowGameOver;
		GameManager.onGameRestart += HideGameOver;
	}


	void OnDisable()
	{
		Player.onUpdateScore -= UpdateScore;
		Player.onUpdateLives -= UpdateLives;
		GameManager.onGameOver -= ShowGameOver;
		GameManager.onGameRestart -= HideGameOver;
	}


	void UpdateScore(int amount)
	{
		_scoreText.text = amount.ToString();
	}


	void UpdateLives(int amount)
	{
		_livesCounter.sprite = _livesCounterImages[amount];
	}


	void ShowGameOver()
	{
		_gameOverScreen.SetActive(true);
		_isGameOver = true;
		StartCoroutine(FlashGameOverText());
	}


	void HideGameOver()
	{
		_gameOverScreen.SetActive(false);
	}


	IEnumerator FlashGameOverText()
	{
		while (_isGameOver)
		{
			yield return _flashSpeed;
			_gameOverText.gameObject.SetActive(false);
			yield return _flashSpeed;
			_gameOverText.gameObject.SetActive(true);
		}
	}
}