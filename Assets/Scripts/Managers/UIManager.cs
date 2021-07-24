using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
	public static event Action onCloseUpgrades;
	public static event Action onRepairPlayer;
	public static event Action<bool> onUpgradeScreenToggle;

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
	private Text _ammoCounter;
	[SerializeField]
	private Text _maxAmmo;

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
	[SerializeField]
	private Text _upgradePointsText;
	[SerializeField]
	private Button _repairButton;
	[SerializeField]
	private Text _repairCostText;



	void OnEnable()
	{
		PlayerData.onUpdateLives += UpdateLives;
		PlayerData.onUpdateScore += UpdateScore;
		PlayerData.onUpdatePoints += UpdatePoints;
		PlayerData.onUpdateLivesCost += UpdateLivesCost;
		PlayerData.onUpdateAmmoCount += UpdateAmmoCount;
		PlayerData.onUpdateMaxAmmo += UpdateMaxAmmo;
		Player.onShieldCountChanged += UpdateShieldCount;
		GameManager.onGameOver += ShowGameOver;
		GameManager.onGameStart += HideGameOver;
		SpawnManager.onWaveEnd += ShowUpgradeScreen;
	}


	void OnDisable()
	{
		PlayerData.onUpdateLives -= UpdateLives;
		PlayerData.onUpdateScore -= UpdateScore;
		PlayerData.onUpdatePoints -= UpdatePoints;
		PlayerData.onUpdateLivesCost -= UpdateLivesCost;
		PlayerData.onUpdateAmmoCount -= UpdateAmmoCount;
		PlayerData.onUpdateMaxAmmo -= UpdateMaxAmmo;
		Player.onShieldCountChanged -= UpdateShieldCount;
		GameManager.onGameOver -= ShowGameOver;
		GameManager.onGameStart -= HideGameOver;
		SpawnManager.onWaveEnd -= ShowUpgradeScreen;
	}


	void Start()
	{
		ShowUpgradeScreen();
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


	void UpdateAmmoCount(int amount)
	{
		_ammoCounter.text = amount.ToString();
	}


	void UpdateMaxAmmo(int amount)
	{
		_maxAmmo.text = amount.ToString();
	}


	#region Game Over
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
	#endregion


	#region Upgrade Screen
	void ShowUpgradeScreen()
	{
		if (_isGameOver == false)
		{
			_upgradeScreen.SetActive(true);
			onUpgradeScreenToggle?.Invoke(false);

			if (PlayerData.Instance.GetLives() >= 3)
			{
				_repairButton.interactable = false;
			}
			else
			{
				_repairButton.interactable = true;
			}
		}
	}


	public void HideUpgradeScreen()
	{
		_upgradeScreen.SetActive(false);
		onUpgradeScreenToggle?.Invoke(true);
		onCloseUpgrades?.Invoke();
	}


	void UpdatePoints(int points)
	{
		_upgradePointsText.text = points.ToString();
	}


	void UpdateLivesCost(int amount)
	{
		_repairCostText.text = amount + " points";
	}


	public void RepairPlayer()
	{
		onRepairPlayer?.Invoke();

		if (PlayerData.Instance.GetLives() >= 3)
		{
			_repairButton.interactable = false;
		}
	}
	#endregion
}