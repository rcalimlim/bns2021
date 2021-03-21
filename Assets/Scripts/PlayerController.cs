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
    [SerializeField] private Menu menu;
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

    private void Interact()
    {
        Vector2 interactPos = (Vector2)transform.position + facingDirection + new Vector2(0f, -0.5f);
        Collider2D collider = Physics2D.OverlapCircle(interactPos, 0.1f);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    private void OpenMenu()
    {
        menu.OpenMenu(controls);
    }

    private void OnDrawGizmos()
    {
        // for debugging player interaction overlap circle
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere((Vector2)transform.position + facingDirection + new Vector2(0f, -0.5f), 0.1f);
    }

    public void HandleUpdate()
    {
        if (controls.Main.Movement.triggered)
        {
            Move(controls.Main.Movement.ReadValue<Vector2>());
        }

        if (controls.Main.Interaction.triggered)
        {
            Interact();
        }

        if (controls.Main.Menu.triggered)
        {
            OpenMenu();
        }
    }

    public void updateHeightCorrection(int height)
    {
        heightAdjustment = height;
        heightCorrection = new Vector3Int(0, heightAdjustment, 0);
    }
}
