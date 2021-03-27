using System;
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
    private PlayerDataManager playerDataManager;

    public bool IsBlocking { get { return isBlocking; } }

    private void Awake()
    {
        // ...
    }

    private void Start()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        if (sp != null)
        {
            spriteRenderer = sp;
        }

        if (IsEnabled() == false)
        {
            Disable();
        }
    }

    private void Update()
    {
        if (IsEnabled() == false)
        {
            Disable();
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

    private IEnumerator BlinkSprite()
    {
        while (isEnabled == true && spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSecondsRealtime(0.4f);
            spriteRenderer.enabled = true;
            yield return new WaitForSecondsRealtime(0.4f);
        }
    }
}
