using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePoint : UndoAble
{
    public int typeOfPoint;

    protected override void OnCollide(Collider2D coll)
    {
        if (!coll.transform.parent) return;
        if (coll.transform.parent.name == "Player")
        {
            float plusBoxColliableDistance = 0.3f;
            if (Vector3.SqrMagnitude(transform.position - coll.transform.position) <= plusBoxColliableDistance)
            {
                transform.parent.GetComponent<CollectablePointManager>().AddCollectablePoint(typeOfPoint);
                gameObject.SetActive(false);
            }
        }
    }
}
