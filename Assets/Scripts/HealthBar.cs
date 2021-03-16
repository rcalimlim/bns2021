using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   private Image barImage;
   private Health health;

   private void Awake() {
       barImage = transform.Find("bar").GetComponent<Image>();
       health = new Health();
   }

   private void Update() {
       barImage.fillAmount = health.GetHealthNormalized();
   }
}

public class Health {
    public const int MAX_HEALTH = 100;

    private int healthAmount;

    public Health()
    {
        healthAmount = 100;
    }

    public Health(int start)
    {
        healthAmount = start;

    }

    public void Update(int change) 
    {
        healthAmount = Math.Max(Math.Min(healthAmount+change,MAX_HEALTH), 0);
    }

    public float GetHealthNormalized(){
        return healthAmount/MAX_HEALTH;
    }

}
