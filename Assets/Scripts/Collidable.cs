using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;//The object that your object want to hit
    [SerializeField] private BoxCollider2D boxCollider2D; //Your object
    private Collider2D[] hits = new Collider2D[20];//List of the object that your object hits

    protected virtual void Start()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0); //reposition all box to the round vector
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (!boxCollider2D)
        {
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            BoxCollider2DResize(new Vector2(0.2f, 0.2f)); //So it only interacts what right on top of it
        }
    }

    protected virtual void FixedUpdate()
    {
        //Collision work
        boxCollider2D.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i]) continue;

            OnCollide(hits[i]);

            hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log(coll.name);
    }

    protected void BoxCollider2DResize(Vector2 newSize)
    {
        boxCollider2D.size = newSize;
    }
}
