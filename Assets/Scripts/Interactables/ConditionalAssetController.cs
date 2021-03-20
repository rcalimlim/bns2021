using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalAssetController : MonoBehaviour
{
    [SerializeField] private bool isBlocking = true;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;

    public bool IsBlocking { get { return isBlocking; } }

    private void Update()
    {
        if (isBlocking == true && disableAfterTriggerFlag != "")
        {
            bool isDisabled = PlayerDataManager.Instance.GetTriggerFlag(disableAfterTriggerFlag);

            if (isDisabled)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false; 
                }
                isBlocking = false;
            }
        }
    }
}
