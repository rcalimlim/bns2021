using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableOnceController : MonoBehaviour
{
    [SerializeField] private string triggerName;
    [SerializeField] private Dialog dialog;

    public void OnTriggerEnter2D(Collider2D other)
    {
        bool wasTriggered = PlayerDataManager.Instance.GetTriggerFlag(triggerName);
        if (!wasTriggered && other.CompareTag("Player") && !other.isTrigger)
        {
            PlayerDataManager.Instance.SetTriggerFlag(triggerName, true);
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }
    }
}
