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

[System.Serializable]
public class DialogElement
{
    [SerializeField] private string text;
    [SerializeField] private FontStyle style = FontStyle.Normal;

    public string Text { get { return text; } }
    public FontStyle Style { get { return style; } }

}
