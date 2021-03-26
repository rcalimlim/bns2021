using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CringeVideoController : MonoBehaviour
{
    [SerializeField] private GameObject videoComponent;
    private VideoPlayer videoPlayer;
    [SerializeField] private double videoLengthSeconds;

    public event Action OnVideoStart;
    public event Action OnVideoEnd;

    private void Start()
    {
        videoPlayer = videoComponent.GetComponent<VideoPlayer>();
        gameObject.SetActive(false);
        if (videoLengthSeconds == 0)
        {
            videoLengthSeconds = videoPlayer.length;
        }
    }

    public IEnumerator PlayVideo()
    {
        OnVideoStart.Invoke();
        gameObject.SetActive(true);
        videoPlayer.GetComponent<VideoPlayer>().Play();
        yield return new WaitForSeconds((float)videoLengthSeconds);
        Destroy(videoPlayer);
        gameObject.SetActive(false);
        OnVideoEnd.Invoke();

        // TODO: add scene switch to battle
    }
}
