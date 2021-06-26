using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData : MonoSingleton<PlayerData>
{
	public static event Action<int> onUpdateLives;
	public static event Action<int> onUpdateScore;
	public static event Action<int> onUpdatePoints;
	public static event Action<int> onUpdateLivesCost;

	private int _maxLives = 3;
	private int _currentLives;
	private int _livesCost;
	private int _score;
	private int _upgradePoints;



	void OnEnable()
	{
		GameManager.onGameStart += NewGame;
		Player.onPlayerDamaged += PlayerDamaged;
		Enemy.onEnemyDied += EnemyDied;
		UIManager.onRepairPlayer += PlayerRepaired;
		SkillEvents.onSkillBought += SkillBought;
	}


	void OnDisable()
	{
		GameManager.onGameStart -= NewGame;
		Player.onPlayerDamaged -= PlayerDamaged;
		Enemy.onEnemyDied -= EnemyDied;
		UIManager.onRepairPlayer -= PlayerRepaired;
		SkillEvents.onSkillBought -= SkillBought;
	}


	void NewGame()
	{
		_currentLives = _maxLives;
		onUpdateLives?.Invoke(_currentLives);

		_score = 0;
		onUpdateScore?.Invoke(_score);

		_upgradePoints = 50;
		onUpdatePoints?.Invoke(_upgradePoints);

		_livesCost = 10;
		onUpdateLivesCost?.Invoke(_livesCost);
	}


	void EnemyDied(int amount)
	{
		UpdateScore(amount);
		UpdatePoints(amount);
	}


	void SkillBought(int amount)
	{
		UpdatePoints(-amount);
	}


	#region Lives
	public int GetLives()
	{
		return _currentLives;
	}


	public int GetLivesCost()
	{
		return _livesCost;
	}


	void UpdateLives(int change)
	{
		_currentLives += change;
		onUpdateLives?.Invoke(_currentLives);
	}


	void PlayerDamaged()
	{
		UpdateLives(-1);
	}


	void PlayerRepaired()
	{
		UpdateLives(1);

		UpdatePoints(-_livesCost);

		_livesCost *= 2;
		onUpdateLivesCost(_livesCost);
	}
	#endregion


	#region Score
	public int GetScore()
	{
		return _score;
	}


	void UpdateScore(int amount)
	{
		_score += amount;
		onUpdateScore?.Invoke(_score);
	}
	#endregion


	#region Upgrade Points
	public int GetUpgradePoints()
	{
		return _upgradePoints;
	}


	void UpdatePoints(int amount)
	{
		_upgradePoints += amount;
		onUpdatePoints?.Invoke(_upgradePoints);
	}
	#endregion
}