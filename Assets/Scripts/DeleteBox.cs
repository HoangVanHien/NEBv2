using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBox : Collidable
{
    protected override void OnCollide(Collider2D coll)
    {
        float plusBoxColliableDistanceX = (transform.lossyScale.x - 1) / 2 + 0.01f;
        float plusBoxColliableDistanceY = (transform.lossyScale.y - 1) / 2 + 0.01f;
        
        if (Mathf.Abs(transform.position.x - coll.transform.position.x) <= plusBoxColliableDistanceX &&
            Mathf.Abs(transform.position.y - coll.transform.position.y) <= plusBoxColliableDistanceY)
        {
            PlusBox opponentPlusBox = coll.GetComponent<PlusBox>();
            if (opponentPlusBox) DestroyPlusBox(opponentPlusBox);
        }
    }

    private void DestroyPlusBox(PlusBox plusBox)
    {
        plusBox.DetroyPlusBox();
    }
}
