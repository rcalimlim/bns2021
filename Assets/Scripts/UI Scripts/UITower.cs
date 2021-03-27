using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITower : MonoBehaviour
{
    Queue<Vector3> destinations;
    [SerializeField] Vector3 nextDestination;
    Image image;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        destinations = new Queue<Vector3>();
        nextDestination = transform.localPosition;
        image = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {   
        float step = speed * Time.deltaTime;
        if(Vector3.Distance(transform.localPosition, nextDestination) < 0.001f)
        {
            if(destinations.Count > 0)
                nextDestination = destinations.Dequeue();
        }
        else 
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextDestination, step);
        }     

    }

    public void AddDestination(Vector3 destination)
    {
        destinations.Enqueue(destination);
    }

    public void SetImage(Sprite sprite)
    {
        image.overrideSprite  = sprite;
    }

    public void Teleport(Vector3 destination)
    {
        destinations.Clear();
        transform.localPosition = destination;
        nextDestination = destination;
    }
}
