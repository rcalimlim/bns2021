using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private Sprite[] tileSprites;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private int MAX_TILE_STAT = 10;

    private SpriteRenderer spriteRenderer;
    private AudioClip audioClip;
    private int stressLevel;
    private int weaponLevel;
    private string tile_type;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioClip = GetComponent<AudioClip>();

        // Create the tile at random with an even distrobution
        GenerateRandomTile();
    }

    // Override the randomly created tile. Please use with caution
    public void OverrideRandomTile(int value, string tile_type)
    {
        value = Mathf.Abs(value);
        if(value > 0 && value < MAX_TILE_STAT)
            return;

        if(tile_type.Equals("buff")) 
        {
            stressLevel = value;
            audioClip = audioClips[0];
        }
        else if(tile_type.Equals("weapon"))
        {
            weaponLevel = value;
            audioClip = audioClips[1];
        }
        else if(tile_type.Equals("enemy"))
        {
            stressLevel = -1 * value;
            audioClip = null;
        }
        else if(tile_type.Equals("scouter"))
        {
            stressLevel = 0;
            weaponLevel = 0;
            audioClip = audioClips[2];
        }
        else
            return;
        
        this.tile_type = tile_type;

    }

    public void GenerateRandomTile()
    {
        int spriteIndex = Random.Range(0,tileSprites.Length-1);

        spriteRenderer.sprite = tileSprites[spriteIndex];
        int tileStat = (spriteIndex % 2 == 0? Mathf.CeilToInt(Random.Range(1, MAX_TILE_STAT/2 ))
                                    : Mathf.CeilToInt(Random.Range((MAX_TILE_STAT/2) + 1, MAX_TILE_STAT ))); 
        
        if(spriteIndex < 2)
        {
            tile_type = "buff";
            stressLevel = tileStat; 
            audioClip = audioClips[0];
        }    
        else if(spriteIndex < 4)
        {
            tile_type = "weapon";
            weaponLevel = tileStat; 
            audioClip = audioClips[1];
        }
        else if(spriteIndex < 6)
        {
            tile_type = "enemy";
            stressLevel = -1 * tileStat; 
        }
        else if(spriteIndex == 6)
        {
            tile_type = "scouter";
        }
    }

    public string GetTileType()
    {
        return tile_type;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
