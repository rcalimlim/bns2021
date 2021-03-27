using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITower : MonoBehaviour
{
    Queue<Vector3> destinations;
    [SerializeField] Vector3 nextDestination;
    Image image;

    Queue<Vector3> scalers;
    [SerializeField] Vector3 nextScale;

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
    {   /*
        float step = speed * Time.deltaTime;
        if(Vector3.Distance(transform.localPosition, nextDestination) < 0.001f)
        {
            if(destinations.Count > 0)
                nextDestination = destinations.Dequeue();
        }
        else 
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextDestination, step);
        }*/

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



    public static IEnumerator ScaleOverTime(Transform transform, Vector3 destination, float speed)
    {
         float step = speed * Time.deltaTime;
        while(Vector3.Distance(transform.localScale, destination) > .1f)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, destination, step);
            yield return null;
        }
        transform.localScale = destination;
    }

    public static IEnumerator MoveOverTime(Transform transform, Vector3 destination, float speed)
    {
        float step = speed * Time.deltaTime;
        while(Vector3.Distance(transform.localPosition, destination) > 1f )
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, step);
            yield return null;
        }
        transform.localPosition = destination;
    }

    static int entery=0;
    public static IEnumerator RotateOverTime(Transform transform, float direction, float angle, float speed)
    {
        entery++;

        float currentangle = transform.eulerAngles.z;
        if (currentangle > 180f) currentangle -= 360f;

        while(! (angle - 4 < currentangle && currentangle < angle + 4))
        {
            transform.Rotate(0,0, direction * speed * Time.deltaTime);
            
            currentangle = transform.eulerAngles.z;
           
            if (currentangle > 180f) 
                currentangle -= 360f;
            
            yield return null;
        }
    }

    public static IEnumerator RotateUpOverTime(Transform transform, float angle, float speed)
    {

        while(transform.rotation.z > angle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), speed * Time.time);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
    }


    // run various routines, one after the other
    public static IEnumerator ChainCoroutines( params IEnumerator[] routines ) 
    {
        foreach ( var item in routines ) 
        {
            while ( item.MoveNext() ) yield return null;
        }

        yield break;
    }
}

