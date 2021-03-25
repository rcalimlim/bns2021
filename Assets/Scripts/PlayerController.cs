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
    [SerializeField] private float timeToMove;
    [SerializeField] private Inventory inventory;

    private Vector3Int heightCorrection;
    private PlayerInput controls;
    private Rigidbody2D rb;
    private Vector2 facingDirection = Vector2.zero;
    private bool isMoving = false;
    private Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        // make sure the right sprite is being used when scene switching
        UpdateSprite(inventory.EquippedArmor);

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

    private IEnumerator Move(Vector2 direction)
    {
        animator.SetBool("Moving", true);
        isMoving = true;
        Vector2 normalizedVector = direction;
        normalizedVector.x = Mathf.RoundToInt(normalizedVector.x);
        normalizedVector.y = Mathf.RoundToInt(normalizedVector.y);

        // face the player in the direction of the last movement attempt
        facingDirection = normalizedVector;

        // sprite direction
        animator.SetFloat("Horizontal", facingDirection.x);
        animator.SetFloat("Vertical", facingDirection.y);

        // setup movement
        Vector2 currentPos = transform.position;
        Vector2 targetPos = transform.position + (Vector3)(normalizedVector);
        float elapsedTime = 0;
        if (CanMove(normalizedVector)) {

            //transform.position += (Vector3)normalizedVector;
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(currentPos, targetPos, (elapsedTime / timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPos;
        }
        isMoving = false;
        yield return new WaitForEndOfFrame();
        animator.SetBool("Moving", false);
    }

    private bool CanMove(Vector2 direction)
    {
        // quickly check for collision tilemap
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction + heightCorrection);
        if (collisionTilemap.HasTile(gridPosition))
        {
            return false;
        }

        // else check if there's a conditional asset in the way
        Vector2 interactPos = (Vector2)transform.position + facingDirection + new Vector2(0f, -0.5f);
        Collider2D collider = Physics2D.OverlapCircle(interactPos, 0.1f);

        bool isBlocking = false;
        ConditionalAssetController cac = collider?.GetComponent<ConditionalAssetController>();
        if (cac != null)
        {
            isBlocking = cac.IsBlocking;
        }

        if (isBlocking) {
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
            Interactable[] interactables = collider.GetComponents<Interactable>();
            foreach (Interactable interactable in interactables)
            {
                interactable.Interact();
            }
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
        if (controls.Main.Movement.triggered && isMoving == false)
        {
            StartCoroutine(Move(controls.Main.Movement.ReadValue<Vector2>()));
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

    public void UpdateSprite(Item equippedArmor)
    {
        switch (equippedArmor.name) 
        {
            case ("JorFeig Everyday Casual"):
                animator.SetTrigger("ChangeToJorFei");
                break;
            case ("Fencing Gear"):
                animator.SetTrigger("ChangeToFencing");
                break;
            case ("Mukbang Magus Robe"):
                animator.SetTrigger("ChangeToChef");
                break;
            case ("NYE Stream Gear"):
                animator.SetTrigger("ChangeToNYEStream");
                break;
            case ("Waaarkout Clothes"):
                animator.SetTrigger("ChangeToWarkout");
                break;
            default:
                break;
        }
    }
}
