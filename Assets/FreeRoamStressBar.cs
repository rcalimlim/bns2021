using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeRoamStressBar : MonoBehaviour
{
    [SerializeField] float fillSpeed = 0.2f;
    [SerializeField] private int stress;
    [SerializeField] private Image barImage;
    private int maxStress = 100;

    private void Start()
    {
        stress = PlayerDataManager.Instance.CurrentStress;
        float target = (stress / (float)maxStress);
        barImage.fillAmount = target;
    }

    private void Update() 
    {
        float target = (stress / (float)maxStress);
        
        if(barImage.fillAmount < target)
        {
            barImage.fillAmount += (fillSpeed * Time.deltaTime);
        }
        
        if(barImage.fillAmount > target)
            barImage.fillAmount -= (fillSpeed * Time.deltaTime);
    }
    public void SetStress(int stressAmount)
    {
        stress = stressAmount;
    }
}
