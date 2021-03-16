using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TableuGameCtrl : MonoBehaviour
{   
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    [SerializeField] private AudioClip[] audioClips;

    // Tile Spawning
    [SerializeField] private GameObject iTilePreFab;
    [SerializeField] private int spawnScouterTurn = 20;
    [SerializeField] private int numOfInitialTiles = 10;
    private bool scouterSpawned = false;

    // UI and Audio
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject bns2019UI;
    private bool gameOverActive = false;
    private TableuStatUIController statUi;
    private MenuController bnsMenuCtrl;
    private AudioSource audioPlayer;
    

    // Player Movement
    private PlayerInput controls;
    private PlayerManager playerManager;

    private void Awake() 
    {
        controls = new PlayerInput();
        playerManager = new PlayerManager();
        audioPlayer = GetComponent<AudioSource>();
        statUi = transform.Find("Stat Canvas").GetComponent<TableuStatUIController>();
        bnsMenuCtrl = bns2019UI.GetComponent<MenuController>();
        StartGame();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        controls.Main.Movement.performed += 
            context => Move(context.ReadValue<Vector2>());
        
    }


    // Update is called once per frame
    void Update()
    {
        statUi.Stress = playerManager.Stress;
        statUi.Weapon = playerManager.Weapon;
        
        if(playerManager.IsDead && !gameOverActive)
            GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "InteractableTile")
        {
            GameObject tile = other.gameObject;
            TileController tileCtrl = tile.GetComponent<TileController>();

            ConsumeTile(tileCtrl);
            Destroy(tile);
        } 
    }

    private void Move(Vector2 direction)
    {
        if(CanMove(direction) && !gameOverActive)
        { // extra note that CanMove prevents us from walking ontop of a wall so those are not considered below
            
            bool move = true;
            playerManager.UpdateTurns();

            Collider2D nextTileCollider = Physics2D.OverlapCircle(transform.position + (Vector3)direction, .2f);
            if(nextTileCollider) // if we are infront of something, prevent motion into it
            {
                GameObject tile = nextTileCollider.gameObject;
                TileController tileCtrl = tile.GetComponent<TileController>();
                
                // Can we advance forward or not?
                move = Advance(tileCtrl);
            }
           
           if(move)
            {
                Vector3 oldPos = transform.position;
                transform.position += (Vector3)direction;
                
                // Spawn Tile Code
                Vector3 tilePos;
                if(direction.x == 0)
                    tilePos = new Vector3(oldPos.x, -2*direction.y, 0f);
                else
                    tilePos = new Vector3(-2*direction.x,oldPos.y, 0f);
                
                SpawnTile(tilePos);

                bnsMenuCtrl.PopulateTextElement("Score/Value", playerManager.Score.ToString());
                bnsMenuCtrl.PopulateTextElement("Turn/Value", playerManager.Turns.ToString());
            }
            
        }
    }

    public void PlayAudio(int number)
    {
        if(number < audioClips.Length && number >= 0)
            audioPlayer.PlayOneShot(audioClips[number]);
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);

        if (collisionTilemap.HasTile(gridPosition)) {
            return false;
        } 

        return true;
    }

    public bool Advance(TileController tileCtrl)
    {
        string tileType = tileCtrl.TileType;
        
        if(tileType == "buff" || tileType == "weapon" || tileType == "scouter")
            return true;
        
        int weapon = playerManager.Weapon;
        if(weapon == 0)
            return true;
        
        int oldStress = tileCtrl.Stress;
        int oldWeapon = weapon;

        // Update the stats
        tileCtrl.Stress += weapon;
        playerManager.UpdateWeapon(oldStress);

        if(Mathf.Abs(oldStress) <= oldWeapon)
        {
            // Play Sword Swing
            PlayAudio(0);
            return true;
        }
        
        // play sword break sound
        PlayAudio(1);
        return false;
    }

    public void ConsumeTile(TileController tileCtrl)
    {
        string tileType = tileCtrl.TileType;

        if(tileType == "buff" || tileType == "enemy")
        {
            int stress = tileCtrl.Stress;
            playerManager.UpdateStress(stress);

            if(stress > 0)
                PlayAudio(3);
            else if(stress < 0)
                PlayAudio(4);
        }
        else if(tileType == "weapon")
        {
            playerManager.UpdateWeapon(tileCtrl.Weapon);
            PlayAudio(2);
        }
        else if(tileType == "scouter")
        {   // Scouter logic PENDING
            PlayAudio(5);
            // Save Status to universal Stat Holder
            
            //Load next scene
            SceneManager.LoadScene("BNS2019+");
            
        }
    }

    void SpawnTile(Vector3 position)
    {
        if(Random.Range(0,7) % 7 > 1)
        {
            GameObject newTile;
            newTile = Instantiate(iTilePreFab, position, Quaternion.identity);

            if(!scouterSpawned && playerManager.Turns > spawnScouterTurn)
            { 
                TileController tileCtrl = newTile.transform.gameObject.GetComponent<TileController>();
                tileCtrl.ForceScouter();
                scouterSpawned = true;
            }
        }
       
    }

    void StartGame()
    {
        List<Vector3> placedTiles = new List<Vector3>();
        placedTiles.Add(Vector3.zero);
        for(int i=0; i < numOfInitialTiles; i++)
        {   
            Vector3 newTile;
            do
                newTile = new Vector3(Random.Range(-2, 3), Random.Range(-2, 3), 0);
            while(placedTiles.Contains(newTile));
                
            placedTiles.Add(newTile);
            SpawnTile(newTile);
        }
    }
    void GameOver()
    {
        GameObject gameOver = Instantiate(gameOverUI);
        MenuController menuCtrl = gameOver.GetComponent<MenuController>();

        menuCtrl.PopulateTextElement("Score/Value", playerManager.Score.ToString());
        menuCtrl.PopulateTextElement("Turn/Value", playerManager.Turns.ToString());
        gameOverActive = true;
    }

}

public class PlayerManager
{
    private int minStat {get;} 
    private int maxStat {get;}
    private int currentStress;
    private int currentWeapon;
    private int score;
    private int turns;
    private bool isDead;
    

    public PlayerManager()
    {
        maxStat = 10;
        minStat = 0;
        currentStress = maxStat;
        score = 0;
        turns = 0;
        currentWeapon = 0;
        isDead = false;
    }

    // Propery Acccess
    public int UpdateStress(int change)
    {   
        currentStress += change;
        currentStress = currentStress > maxStat? maxStat 
            : currentStress < minStat? minStat: currentStress;
        
        score += Mathf.Abs(change);
        isDead = (currentStress == minStat);
        return currentStress;
    }
    public int UpdateWeapon(int change)
    {   currentWeapon += change;
        currentWeapon = currentWeapon >= maxStat? maxStat 
            : currentWeapon < minStat? minStat: currentWeapon;
        
        score += Mathf.Abs(change);
        return currentWeapon;
    }

    public int UpdateTurns()
    { return turns++; }

    public bool IsDead
    {get => isDead;}

    public int Stress
    { get => currentStress; }

    public int Weapon
    { get => currentWeapon; }

    public int Turns
    { get => turns; }

    public int Score
    { get => score; }

}
