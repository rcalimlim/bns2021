using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalAssetController : MonoBehaviour
{
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private bool isBlocking = true;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;
    private SpriteRenderer spriteRenderer;

    public bool IsBlocking { get { return isBlocking; } }

    private void Awake()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        if (sp != null)
        {
            spriteRenderer = sp;
        }

        if (!IsEnabled())
        {
            Disable();
        }
    }

    private void Update()
    {
        // needs to basically happen once
        if (isEnabled == true && disableAfterTriggerFlag != "")
        {
            bool isDisabled = PlayerDataManager.Instance.GetTriggerFlag(disableAfterTriggerFlag);
            if (isDisabled)
            {
                if (spriteRenderer != null)
                {
                    StartCoroutine(BlinkSpriteOff());
                }
                Disable();
            }
        }
    }
    private bool IsEnabled()
    {
        bool isDisabled = false;
        if (disableAfterTriggerFlag != "")
        {
            isDisabled = PlayerDataManager.Instance.GetTriggerFlag(disableAfterTriggerFlag);
            if (isDisabled)
            {
                return false;
            }
        }

        if (enableAfterTriggerFlag == "")
        {
            return true;
        }

        return PlayerDataManager.Instance.GetTriggerFlag(enableAfterTriggerFlag);
    }

    private void Disable()
    {
        isEnabled = false;
        isBlocking = false;
        spriteRenderer.enabled = false;
    }

    private IEnumerator BlinkSpriteOff()
    {
        for (int i = 0; i < 6; ++i)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
                yield return new WaitForSecondsRealtime(0.4f);
                spriteRenderer.enabled = true;
                yield return new WaitForSecondsRealtime(0.4f);
            }
        }
        spriteRenderer.enabled = false;
    }
}
