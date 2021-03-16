using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private Sprite[] tileSprites;
    [SerializeField] private int MAX_TILE_STAT = 10;
    [SerializeField] private bool swap = false;
    [SerializeField] private float scouterMoveDelay = 1f;
    [SerializeField] private bool forceScouter = false;

    private SpriteRenderer spriteRenderer;
    private TableuStatUIController statUi;
    private int stressLevel = 0;
    private int weaponLevel = 0;
    private string tileType;
    private float nextMoveTime;
    
    private Vector3 playerOldPos;
    private GameObject player;
    

    // Start is called before the first frame update
    void Awake() {
        statUi = transform.Find("Stat Canvas").GetComponent<TableuStatUIController>();
        nextMoveTime = Time.time + scouterMoveDelay;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    void Start()
    {
        // For player tracking
        player = GameObject.Find("Player");
        playerOldPos = player.transform.position;

        // Create the tile at random with an even distrobution
       if(forceScouter)
            OverrideTile(0, "scouter");
        else
            GenerateRandomTile();
    }

    // Override the randomly created tile. Please use with caution
    public void OverrideTile(int value, string tile_type)
    {
        value = Mathf.Abs(value);
        if(value < 0 || value > MAX_TILE_STAT)
            return;

        if(tile_type.Equals("buff")) 
        {
            stressLevel = value;
            spriteRenderer.sprite = (value <= 5)? tileSprites[0] : tileSprites[1];
            statUi.DisableWeapon();
        }
        else if(tile_type.Equals("weapon"))
        {
            weaponLevel = value;
            spriteRenderer.sprite = (value <= 5)? tileSprites[2] : tileSprites[3];
            statUi.DisableStress();
        }
        else if(tile_type.Equals("enemy"))
        {
            stressLevel = -1 * value;
            spriteRenderer.sprite = (value <= 5)? tileSprites[4] : tileSprites[5];
            statUi.DisableWeapon();
        }
        else if(tile_type.Equals("scouter"))
        {
            stressLevel = 0;
            weaponLevel = 0;
            spriteRenderer.sprite = tileSprites[6];
            spriteRenderer.sortingOrder = 400;
            statUi.DisableStress();
            statUi.DisableWeapon();
        }
        else
            return;
        
        
        tileType = tile_type;

    }

    public void GenerateRandomTile()
    {
        int spriteIndex = Random.Range(0,tileSprites.Length-1);

        spriteRenderer.sprite = tileSprites[spriteIndex];
        int tileStat = (spriteIndex % 2 == 0? Mathf.CeilToInt(Random.Range(1, MAX_TILE_STAT/2 ))
                                    : Mathf.CeilToInt(Random.Range((MAX_TILE_STAT/2) + 1, MAX_TILE_STAT ))); 
        
        if(spriteIndex < 2)
        {
            tileType = "buff";
            stressLevel = tileStat;
            statUi.DisableWeapon();
        }    
        else if(spriteIndex < 4)
        {
            tileType = "weapon";
            weaponLevel = tileStat; 
            statUi.DisableStress();
        }
        else if(spriteIndex < 6)
        {
            tileType = "enemy";
            stressLevel = -1 * tileStat;
            statUi.DisableWeapon();
        }
        else if(spriteIndex == 6)
        {
            tileType = "scouter";
            statUi.DisableWeapon();
            statUi.DisableStress();
        }
    }

    public void ForceScouter()
    {forceScouter = true;}
    public string TileType
    {
        get => tileType;
    }

    public int Weapon 
    {
        get => weaponLevel;
    }
    
    public int Stress
    { 
        get => stressLevel; 
        set 
        {
            if(tileType == "enemy")
            {   
                stressLevel = (value > 0)? 0 : 
                    (value < -1*MAX_TILE_STAT) ? -1*MAX_TILE_STAT: value;
            } 
            else if(tileType == "buff")
            {
                stressLevel = (value <= 0)? 0 : 
                    (value > MAX_TILE_STAT) ? MAX_TILE_STAT: value;
            }
            else 
            {
                print("Tiles of type: " + tileType + " do not change in Stress levels");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        statUi.Stress = stressLevel;
        statUi.Weapon = weaponLevel;

        if(tileType == "scouter")
            Avoid();
        else    
            Follow();
    }

    void Follow()
    {
        Vector3 playerPos = player.transform.position;
        if(!Vector3.Equals(playerPos,playerOldPos))
        {         
            Vector3 tilePosition = transform.position;
            if( ((tilePosition.x == playerOldPos.x && playerOldPos.x == playerPos.x) 
                  || (tilePosition.y == playerOldPos.y && playerOldPos.y == playerPos.y)) 
                && (Vector3.Distance(tilePosition, playerPos) > Vector3.Distance(tilePosition, playerOldPos)))
            {
                transform.position += playerPos - playerOldPos;
            }
           
            playerOldPos = playerPos;
        }
    }

    void Avoid()
    {
        if(Time.time > nextMoveTime) 
        {
            Vector3 playerPos = player.transform.position;
            Vector3 tilePosition = transform.position;
            Vector3 moveVector = Vector3.zero;

            Collider2D collider;
            do
            {
                moveVector = new Vector3(Mathf.RoundToInt(Random.Range(-1,2)), Mathf.RoundToInt(Random.Range(-1,2)), 0);
                collider = Physics2D.OverlapCircle(tilePosition + moveVector, .2f);
            } while(Vector3.Equals(moveVector, Vector3.zero) || (collider?.gameObject.tag == "Player" || collider?.gameObject.tag == "Wall"));
            
            if(collider) {
                if(swap)
                {// Swap Tile
                    Transform otherTile = collider.gameObject.transform;
                    otherTile.position = tilePosition; 
                }
                else // Remove Tile
                    Destroy(collider.gameObject);            
            }

            transform.position += moveVector;
            nextMoveTime = Time.time + scouterMoveDelay; 
        }
        
    }
}
