using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBar : MonoBehaviour
{
    private Image barImage;
   [SerializeField] int MAX_STRESS = 200;
   [SerializeField] float fillSpeed = 0.2f;
   [SerializeField] private int stress;

   public int Stress {
       get => stress;
       set => stress = Mathf.Clamp(value,0,MAX_STRESS);
    }

   private void Awake() {
       barImage = transform.Find("bar").GetComponent<Image>();
   }

   private void Update() 
   {    float target = (stress / (float)MAX_STRESS);
        
        if(barImage.fillAmount < target)
            barImage.fillAmount += (fillSpeed * Time.deltaTime);
        
        if(barImage.fillAmount > target)
            barImage.fillAmount -= (fillSpeed * Time.deltaTime);
       
   }
}
