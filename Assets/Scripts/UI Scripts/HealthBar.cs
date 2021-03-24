using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   private Image barImage;
   [SerializeField] int MAX_HEALTH = 200;
   [SerializeField] float fillSpeed = 0.2f;
   [SerializeField] private int health;

   public int Health {
       get => health;
       set => health = Mathf.Clamp(value,0,MAX_HEALTH);
    }

   private void Awake() {
       barImage = transform.Find("bar").GetComponent<Image>();
   }

   private void Update() 
   {    float target = (health / (float)MAX_HEALTH);
        
        if(barImage.fillAmount < target)
            barImage.fillAmount += (fillSpeed * Time.deltaTime);
        
        if(barImage.fillAmount > target)
            barImage.fillAmount -= (fillSpeed * Time.deltaTime);
       
   }
}

