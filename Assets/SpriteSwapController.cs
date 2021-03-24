using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpriteSwapController : MonoBehaviour
{
    // The name of the sprite sheet to use
    [SerializeField] private string SpriteSheetName;
    // The name of the currently loaded sprite sheet
    [SerializeField] private string LoadedSpriteSheetName;
    // The dictionary containing all the sliced up sprites in the sprite sheet
    private Dictionary<string, Sprite> spriteSheet;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        LoadSpriteSheet();
    }

    private void Update()
    {
        if (LoadedSpriteSheetName != SpriteSheetName)
        {
            LoadSpriteSheet();
        }

        // Important: The name of the sprite must be the same!
        Debug.Log(spriteSheet);
        Debug.Log(spriteRenderer.sprite.name);
        spriteRenderer.sprite = spriteSheet[spriteRenderer.sprite.name];
    }

    // Loads the sprites from a sprite sheet
    private void LoadSpriteSheet()
    {
        // Load the sprites from a sprite sheet file (png). 
        // Note: The file specified must exist in a folder named Resources
        var sprites = Resources.LoadAll<Sprite>(SpriteSheetName);
        spriteSheet = sprites.ToDictionary(x => x.name, x => x);

        // Remember the name of the sprite sheet in case it is changed later
        LoadedSpriteSheetName = SpriteSheetName;
    }
}
