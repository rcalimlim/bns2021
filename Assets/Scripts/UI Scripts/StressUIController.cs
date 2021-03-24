using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressUIController : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] List<Text> stressTexts;

    
    [SerializeField] int playerHP = 100, MAX_HP = 200;

    public int HP {
        set => playerHP = Mathf.Clamp(0,value, MAX_HP);
    }

    // Update is called once per frame
    void Update()
    {
        stressTexts[0].text = playerHP.ToString();
        stressTexts[1].text = (MAX_HP - playerHP).ToString();
        healthBar.Health = playerHP;
    }


}
