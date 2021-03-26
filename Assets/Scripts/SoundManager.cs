using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource EffectsSource;
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SpecialSource;

	// Random pitch adjustment range.
    [SerializeField]
	private float LowPitchRange = .95f;
    [SerializeField]
	public float HighPitchRange = 1.05f;

	// Singleton instance.
	public static SoundManager Instance = null;
	
	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}

	// Play a single clip through the sound effects source.
	public void Play(AudioClip clip, bool loop = false)
	{
		EffectsSource.loop = loop;
		EffectsSource.clip = clip;
		EffectsSource.Play();
	}

	// Play a single clip through the music source.
	public void PlayMusic(AudioClip clip, bool loop = false, double scheduledDelay = 0)
	{
		double now = AudioSettings.dspTime;
		MusicSource.loop = loop;
		MusicSource.clip = clip;
		MusicSource.PlayScheduled(now + scheduledDelay);
	}

	public void PlaySpecial(AudioClip clip, bool loop = false, double scheduledDelay = 0)
	{
		double now = AudioSettings.dspTime;
		SpecialSource.loop = loop;
		SpecialSource.clip = clip;
		SpecialSource.PlayScheduled(now + scheduledDelay);
	}

	public void StopEffect()
	{
		EffectsSource.clip = null;
		EffectsSource.loop = false;
		EffectsSource.Stop();
	}	

	public void StopMusic()
	{
		MusicSource.clip = null;
		MusicSource.loop = false;
		MusicSource.Stop();
	}	

	public void StopSpecial()
	{
		SpecialSource.clip = null;
		SpecialSource.loop = false;
		SpecialSource.Stop();
	}	

	public void StopAll() 
	{
		EffectsSource.Stop();
		MusicSource.Stop();
		SpecialSource.Stop();
	}

	public void ScheduleTwoClips(AudioClip firstClip, AudioClip secondClip) 
	{
		double now = AudioSettings.dspTime;
		double duration = (double)firstClip.samples / firstClip.frequency;

		// intro
		SpecialSource.clip = firstClip;
		SpecialSource.loop = false;
		SpecialSource.PlayScheduled(now);

		// following loop
		MusicSource.clip = secondClip;
		MusicSource.loop = true;
		MusicSource.PlayScheduled(now + duration);
	}
}
