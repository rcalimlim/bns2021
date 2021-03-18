using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int heightAdjustment = -1;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    private Vector3Int heightCorrection;
    private PlayerInput controls;
    private Rigidbody2D rb;
    private Vector2 facingDirection = Vector2.zero;

    private void Awake()
    {
        controls = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    private void Start()
    {
        heightCorrection = new Vector3Int(0, heightAdjustment, 0);
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Move(Vector2 direction)
    {
        // face the player in the direction of the last movement attempt
        facingDirection = direction;
        if (CanMove(direction)) {
            transform.position += (Vector3)direction;
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction + heightCorrection);

        if (collisionTilemap.HasTile(gridPosition)) {
            return false;
        } 

        return true;
    }

    public void updateHeightCorrection(int height)
    {
        heightAdjustment = height;
        heightCorrection = new Vector3Int(0, heightAdjustment, 0);
    }

    public Vector2 FacingDirection 
    { 
        get { return facingDirection; }
    }
}
