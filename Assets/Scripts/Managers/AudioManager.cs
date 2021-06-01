using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
	private AudioSource _source;
	[SerializeField]
	private AudioClip _laser;
	[SerializeField]
	[Range(0, 1)]
	private float _laserVolume = 0.5f;
	[SerializeField]
	private AudioClip _explosion;
	[SerializeField]
	[Range(0, 1)]
	private float _explosionVolume = 0.5f;
	[SerializeField]
	private AudioClip _powerup;
	[SerializeField]
	[Range(0, 1)]
	private float _powerupVolume = 0.5f;


	private void OnEnable()
	{
		Player.onLaserFired += PlayLaser;
		Powerup.onPowerupCollected += PlayPowerup;
		Explosion.onExplosion += PlayExplosion;
		Enemy.onEnemyDied += PlayExplosion;
	}

	private void OnDisable()
	{
		Player.onLaserFired -= PlayLaser;
		Powerup.onPowerupCollected -= PlayPowerup;
		Explosion.onExplosion -= PlayExplosion;
		Enemy.onEnemyDied -= PlayExplosion;
	}


	void Start()
	{
		DontDestroyOnLoad(this);

		_source = GetComponent<AudioSource>();
		if (_source == null)
		{
			Debug.LogError("AudioManager is missing AudioSource component.");
		}
	}


	void PlayLaser()
	{
		_source.clip = _laser;
		_source.volume = _laserVolume;
		_source.Play();
	}


	void PlayExplosion()
	{
		_source.clip = _explosion;
		_source.volume = _explosionVolume;
		_source.Play();
	}


	void PlayExplosion(int amount = 0)
	{
		PlayExplosion();
	}


	void PlayPowerup(PowerupType type = PowerupType.Shield)
	{
		_source.clip = _powerup;
		_source.volume = _powerupVolume;
		_source.Play();
	}
}