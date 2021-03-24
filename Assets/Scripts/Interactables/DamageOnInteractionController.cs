using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnInteractionController : MonoBehaviour
{
    private bool wasTriggered = false;
    [SerializeField] private uint stressAdd = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && wasTriggered == false)
        {
            PlayerDataManager.Instance.AdjustStress(20);
            wasTriggered = true;
        }
    }
}
