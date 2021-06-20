using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
	public static event Action onCloseUpgrades;

	[SerializeField]
	private Text _scoreText;
	[SerializeField]
	private Image _livesCounter;
	[SerializeField]
	private Sprite[] _livesCounterImages;

	[Space(10)]

	[SerializeField]
	private Image _shieldCounter;
	[SerializeField]
	private Sprite[] _shieldCounterImages;

	[Space(10)]

	[SerializeField]
	private GameObject _gameOverScreen;
	[SerializeField]
	private Text _gameOverText;
	private bool _isGameOver = false;
	private WaitForSeconds _flashSpeed = new WaitForSeconds(1f);

	[Space(10)]

	[SerializeField]
	private GameObject _upgradeScreen;



	void OnEnable()
	{
		Player.onUpdateScore += UpdateScore;
		Player.onUpdateLives += UpdateLives;
		Player.onShieldCountChanged += UpdateShieldCount;
		GameManager.onGameOver += ShowGameOver;
		GameManager.onGameRestart += HideGameOver;
		SpawnManager.onWaveEnd += ShowUpgradeScreen;
	}


	void OnDisable()
	{
		Player.onUpdateScore -= UpdateScore;
		Player.onUpdateLives -= UpdateLives;
		Player.onShieldCountChanged -= UpdateShieldCount;
		GameManager.onGameOver -= ShowGameOver;
		GameManager.onGameRestart -= HideGameOver;
		SpawnManager.onWaveEnd -= ShowUpgradeScreen;
	}


	void UpdateScore(int amount)
	{
		_scoreText.text = amount.ToString();
	}


	void UpdateLives(int amount)
	{
		_livesCounter.sprite = _livesCounterImages[amount];
	}


	void UpdateShieldCount(int amount)
	{
		_shieldCounter.sprite = _shieldCounterImages[amount];
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


	void ShowUpgradeScreen()
	{
		_upgradeScreen.SetActive(true);
	}


	public void HideUpgradeScreen()
	{
		_upgradeScreen.SetActive(false);
		onCloseUpgrades?.Invoke();
	}
}