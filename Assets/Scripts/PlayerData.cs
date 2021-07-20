using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerData : MonoSingleton<PlayerData>
{
	public static event Action<int> onUpdateLives;
	public static event Action<int> onUpdateScore;
	public static event Action<int> onUpdatePoints;
	public static event Action<int> onUpdateLivesCost;
	public static event Action<int> onUpdateAmmoCount;
	public static event Action<int> onUpdateMaxAmmo;

	private int _maxLives = 3;
	private int _currentLives;
	private int _livesCost;
	private int _score;
	private int _upgradePoints;
	private int _maxAmmo;
	private int _ammoCount;
	private float _moveSpeed;



	void OnEnable()
	{
		GameManager.onGameStart += NewGame;
		Player.onPlayerDamaged += PlayerDamaged;
		Player.onLaserFired += ChangeAmmoCount;
		Player.onAmmoCollected += AmmoCollected;
		Enemy.onEnemyDied += EnemyDied;
		UIManager.onRepairPlayer += PlayerRepaired;
		SkillEvents.onSkillBought += SkillBought;
	}


	void OnDisable()
	{
		GameManager.onGameStart -= NewGame;
		Player.onPlayerDamaged -= PlayerDamaged;
		Player.onLaserFired -= ChangeAmmoCount;
		Player.onAmmoCollected -= AmmoCollected;
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

		ChangeMaxAmmo(15);
		ChangeAmmoCount(_maxAmmo);
		_moveSpeed = 5f;
	}


	void EnemyDied(int amount)
	{
		UpdateScore(amount);
		UpdatePoints(amount);
	}


	void SkillBought(Skill skill, int amount)
	{
		UpdatePoints(-amount);

		foreach (SkillModifiers modifiers in skill.skillModifiers)
		{
			switch (modifiers.modifier)
			{
				case SkillModifiers.ModifierType.Speed:
					_moveSpeed += modifiers.amount;
					break;
				case SkillModifiers.ModifierType.Ammo:
					ChangeMaxAmmo((int)modifiers.amount);
					break;
				default:
					Debug.LogWarning("PlayerData::SkillBought -- Unknown skill modifier type");
					break;
			}
		}
	}


	public float GetMoveSpeed()
	{
		return _moveSpeed;
	}


	#region Ammo
	public int GetMaxAmmo()
	{
		return _maxAmmo;
	}


	void ChangeMaxAmmo(int amount)
	{
		_maxAmmo += amount;
		onUpdateMaxAmmo?.Invoke(_maxAmmo);
	}


	public int GetAmmoCount()
	{
		return _ammoCount;
	}


	void ChangeAmmoCount(int amount)
	{
		_ammoCount += amount;
		_ammoCount = Mathf.Clamp(_ammoCount, 0, _maxAmmo);
		onUpdateAmmoCount?.Invoke(_ammoCount);
	}


	void AmmoCollected()
	{
		ChangeAmmoCount(_maxAmmo);
	}
	#endregion


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