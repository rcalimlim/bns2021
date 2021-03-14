using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int heightAdjustment = -1;
    private Vector3Int heightCorrection;
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap collisionTilemap;
    private PlayerInput controls;

    private void Awake()
    {
        controls = new PlayerInput();
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
    void Start()
    {
        heightCorrection = new Vector3Int(0, heightAdjustment, 0);
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Move(Vector2 direction)
    {
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
}
