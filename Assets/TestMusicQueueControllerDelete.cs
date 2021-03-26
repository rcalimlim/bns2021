using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMusicQueueControllerDelete : MonoBehaviour
{
    [SerializeField] private AudioClip intro;
    [SerializeField] private AudioClip loop;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.Instance.ScheduleTwoClips(intro, loop);
    }
}
