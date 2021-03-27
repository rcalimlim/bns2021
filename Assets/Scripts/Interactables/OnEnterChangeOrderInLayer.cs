using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterChangeOrderInLayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1000000;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5000;
        }
    }
}
