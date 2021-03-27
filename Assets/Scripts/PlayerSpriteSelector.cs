using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSpriteSelector : MonoBehaviour
{
    [SerializeField] Sprite[] playerSprites; 
    
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        string prevScene = PlayerDataManager.Instance.PrevScene;
        string currScene = SceneManager.GetActiveScene().name;
        
        switch((prevScene, currScene))
        {
            case ("BNS2019", "BNS2019+"):
                spriteRenderer.sprite = playerSprites[0]; 
                playerController.updateHeightCorrection(0);
                break;
            default:
                //spriteRenderer.sprite = playerSprites[0]; 
                break;
        }       


    }
}
