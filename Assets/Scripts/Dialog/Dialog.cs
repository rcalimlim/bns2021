using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [SerializeField] private List<DialogElement> lines;

    public List<DialogElement> Lines {
        get { return lines; }
    }
}

public enum AudioDuration
{ 
    Once,
    LoopDuringDialog,
    LoopUntilStopped
}

[System.Serializable]
public class DialogElement
{
    [SerializeField] private string text;
    [SerializeField] private FontStyle style = FontStyle.Normal;
    [SerializeField] private AudioClip effectClip;
    [SerializeField] private AudioDuration effectDuration;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioDuration musicDuration;
    [SerializeField] private int addStressToPlayer = 0;

    public string Text { get { return text; } }
    public FontStyle Style { get { return style; } }
    public AudioClip EffectClip { get { return effectClip; } }
    public AudioDuration EffectDuration { get { return effectDuration; } }
    public AudioClip MusicClip { get { return musicClip; } }
    public AudioDuration MusicDuration { get { return musicDuration; } }
    public int AddStressToPlayer { get { return addStressToPlayer; } }
}
