using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBedInteractable : MonoBehaviour, Interactable
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private Dialog noStressDialog;
    [SerializeField] private Dialog stressedDialog;

    public void Interact()
    {
        if (PlayerDataManager.Instance.CurrentStress == PlayerDataManager.Instance.MinStress)
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(noStressDialog));
        }
        else
        {
            StartCoroutine(ActivateStressedFlow());
        }
    }

    private IEnumerator ActivateStressedFlow()
    {
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(stressedDialog));
        yield return RespawnManager.Instance.ActivateSleepSequence();
    }
}
