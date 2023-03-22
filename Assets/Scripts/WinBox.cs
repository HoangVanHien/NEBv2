using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : Collidable
{
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.transform.parent == null) return;
        if (coll.transform.parent.name == "Player")
        {
            float plusBoxColliableDistance = 0.02f;
            if (Vector3.SqrMagnitude(transform.position - coll.transform.position) <= plusBoxColliableDistance)
            {
                GameObject.Find("LevelUI").GetComponent<LevelUI>().GameWin();
            }
        }
    }
}
