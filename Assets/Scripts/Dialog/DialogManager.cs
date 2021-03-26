using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    // singleton setup
    private static DialogManager instance;
    public static DialogManager Instance { get { return instance; } }

    // player controls
    private PlayerInput controls = null;

    [SerializeField] private GameObject dialogBox;
    [SerializeField] private Text dialogText;
    [SerializeField] private int lettersPerSecond;


    // dialog display
    private Dialog dialog;
    private int currentLine = 0;
    private bool isTyping = false;

    // sound properties
    private bool shouldStopEffect = false;
    private bool shouldStopMusic = false;

    // events
    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    private void Awake()
    {
        controls = new PlayerInput();

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    public void HandleUpdate()
    {
        if (controls.Main.Interaction.triggered && isTyping == false)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            } else
            {
                if (shouldStopEffect == true) 
				{
                    SoundManager.Instance.StopEffect();
				}
                if (shouldStopMusic == true) 
				{
                    SoundManager.Instance.StopMusic();
				}
                CloseDialog();
            }
        }
    }

    public IEnumerator ShowDialog(Dialog dialog, bool isAcceptDecline = false)
    {
        foreach(Button button in dialogBox.GetComponentsInChildren<Button>(true))
        {
            button.gameObject.SetActive(false);
        }

        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();
        this.dialog = dialog;



        dialogBox.SetActive(true);

        yield return StartCoroutine(TypeDialog(dialog.Lines[0]));

        foreach(Button button in dialogBox.GetComponentsInChildren<Button>(true))
        {
            button.gameObject.SetActive(isAcceptDecline);
        }
    }

    public void CloseDialog()
    {
        currentLine = 0;
        dialogBox.SetActive(false);
        OnCloseDialog?.Invoke();
    }

    public IEnumerator TypeDialog(DialogElement line)
    {
        // play effect
        if (line.EffectClip != null) 
		{ 
            switch (line.EffectDuration)
            {
                case AudioDuration.Once:
                    SoundManager.Instance.Play(line.EffectClip);
                    break;
                case AudioDuration.LoopDuringDialog:
                    shouldStopEffect = true;
                    SoundManager.Instance.Play(line.EffectClip, true);
                    break;
                case AudioDuration.LoopUntilStopped:
                    SoundManager.Instance.Play(line.EffectClip, true);
                    break;
                default:
                    break;
			}
		}

        // play music
        if (line.MusicClip != null) 
		{
            Debug.LogFormat("music clip {0}", line.MusicDuration);
            switch (line.MusicDuration)
            {
                case AudioDuration.Once:
                    SoundManager.Instance.PlayMusic(line.MusicClip);
                    break;
                case AudioDuration.LoopDuringDialog:
                    Debug.Log("here in loop during dialog");
                    SoundManager.Instance.PlayMusic(line.MusicClip, true);
                    shouldStopMusic = true;
                    break;
                case AudioDuration.LoopUntilStopped:
                    SoundManager.Instance.PlayMusic(line.MusicClip, true);
                    break;
                default:
                    break;
			}
		}

        isTyping = true;
        dialogText.text = "";

        dialogText.fontStyle = line.Style;

        foreach (var letter in line.Text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        // trigger health change to player
        if (line.AddStressToPlayer != 0)
        {
            PlayerDataManager.Instance.AdjustStress(line.AddStressToPlayer);
        }

        isTyping = false;
    }
}
