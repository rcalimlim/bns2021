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

public enum AudioType 
{ 
    Music,
    Effect
}

[System.Serializable]
public class DialogElement
{
    [SerializeField] private string text;
    [SerializeField] private FontStyle style = FontStyle.Normal;
    [SerializeField] private AudioType audioType;
    [SerializeField] private AudioClip audioClip;

    public string Text { get { return text; } }
    public FontStyle Style { get { return style; } }
    public AudioType AudioType { get { return audioType; } }
    public AudioClip AudioClip { get { return audioClip; } }
}
