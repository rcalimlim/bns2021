using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableuStatUIController : MonoBehaviour
{
    private Text stressText, weaponText;

    private int stress = 0, weapon = 0;

    void Awake() {
        Transform st = transform.Find("Stress");
        stressText = st.GetComponent<Text>();

        Transform wt = transform.Find("Weapon");
        weaponText = wt.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        stressText.text = stress.ToString();
        weaponText.text = weapon.ToString();
    }

    public int Stress 
    {
        set => stress = value;
    }

    public int Weapon
    {
        set => weapon = value;
    }

    public void DisableStress()
    { 
        stressText.enabled = false;
    }
    
    public void EnableStress()
    {
        stressText.enabled = true;
    }

    public void EnableWeapon()
    {
        weaponText.enabled = true;
    }

    public void DisableWeapon()
    {
        weaponText.enabled = false;
    }
}
