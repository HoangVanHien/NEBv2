using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// plus box interacts with other plus box and hold button box
/// </summary>
public class PlusBox : UndoAble
{
    [SerializeField] private bool isAttached = false;
    [SerializeField] private AbleButton ableButton;

    private UndoData siblingIndexUndoData = new UndoData();
    public int siblingIndex = -1;

    protected override void Start()
    {
        ableButton = GetComponentInParent<AbleButton>();
        if (ableButton)
        {
            isAttached = true;
            siblingIndex = ableButton.IndexOfPlusBox(this);
        }
        base.Start();
        BoxCollider2DResize(new Vector2(1.05f, 1.05f));
    }

    protected override void FixedUpdate()
    {
        if (!isAttached) base.FixedUpdate();
    }

    public bool IsAttached()
    {
        return isAttached;
    }

    public bool HasButton()
    {
        return (GetComponentInChildren<ButtonBox>() != null);
    }

    public AbleButton GetAbleButton()
    {
        return ableButton;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (isAttached) return;
        PlusBox opponentPlusBox = coll.GetComponentInParent<PlusBox>();
        if (!opponentPlusBox)
        {
            return;
        }
        if (!opponentPlusBox.IsAttached())
        {
            return;
        }
        AbleButton opponentAbleButton = opponentPlusBox.GetAbleButton();
        if (!opponentAbleButton)
        {
            Debug.Log("Error attached but doesnt have able button");
            return;
        }
        if (!opponentPlusBox.transform.parent)
        {
            Debug.Log("Error attached but doesnt have parrent");
            return;
        }
        if (opponentAbleButton.isOrderOutProcessing)
        {
            return;
        }
        opponentAbleButton.isOrderOutProcessing = true;
        //Debug.Log(name + " collide" + coll.name);
        AttachPlusBox(opponentPlusBox.transform.parent.transform, opponentPlusBox);
        //Debug.Log(name + " end collide" + coll.name);
        opponentAbleButton.isOrderOutProcessing = false;
    }

    private void AttachPlusBox(Transform opponentParent, PlusBox opponentPlusBox)
    {
        //attach it first
        transform.SetParent(opponentParent, true);

        Vector3 possiblePosition = FindPlusBoxPossiblePosition(opponentPlusBox.transform.localPosition, transform.localPosition);
        if (possiblePosition == Vector3.zero)
        {
            transform.SetParent(null);
            return;
        }

        isAttached = true;
        transform.localPosition = possiblePosition;
        transform.localRotation = opponentPlusBox.transform.localRotation;
        transform.localScale = opponentPlusBox.transform.localScale;
        ableButton = opponentPlusBox.GetAbleButton();
        siblingIndex = ableButton.IndexOfPlusBox(this);
        if (HasButton())
        {
            ableButton.AddPlusBoxOrderOut(this);
        }
    }

    //private Vector3 FindPlusBoxPossiblePosition(Transform opponentParent, Vector3 thisLocalPosition)
    //{
    //    List<Vector3> possiblePosition = new List<Vector3>();
    //    float ableDistance = 0.02f;
    //    for (int i = 0; i < opponentParent.childCount; i++)//add possiblePosition by distance
    //    {
    //        if (opponentParent.GetChild(i) != transform)
    //        {
    //            Vector3 curOpponetLocalPosition = opponentParent.GetChild(i).localPosition;
    //            Vector3 curOpponetSidePosition;

    //            curOpponetSidePosition = curOpponetLocalPosition + Vector3.left;
    //            if (Vector3.SqrMagnitude(curOpponetSidePosition - thisLocalPosition) < ableDistance)
    //            {
    //                if (!possiblePosition.Contains(curOpponetSidePosition))
    //                    possiblePosition.Add(new Vector3(curOpponetSidePosition.x, curOpponetSidePosition.y, curOpponetSidePosition.z));
    //            }
    //            curOpponetSidePosition = curOpponetLocalPosition + Vector3.right;
    //            if (Vector3.SqrMagnitude(curOpponetSidePosition - thisLocalPosition) < ableDistance)
    //            {
    //                if (!possiblePosition.Contains(curOpponetSidePosition))
    //                    possiblePosition.Add(new Vector3(curOpponetSidePosition.x, curOpponetSidePosition.y, curOpponetSidePosition.z));
    //            }
    //            curOpponetSidePosition = curOpponetLocalPosition + Vector3.up;
    //            if (Vector3.SqrMagnitude(curOpponetSidePosition - thisLocalPosition) < ableDistance)
    //            {
    //                if (!possiblePosition.Contains(curOpponetSidePosition))
    //                    possiblePosition.Add(new Vector3(curOpponetSidePosition.x, curOpponetSidePosition.y, curOpponetSidePosition.z));
    //            }
    //            curOpponetSidePosition = curOpponetLocalPosition + Vector3.down;
    //            if (Vector3.SqrMagnitude(curOpponetSidePosition - thisLocalPosition) < ableDistance)
    //            {
    //                if (!possiblePosition.Contains(curOpponetSidePosition))
    //                    possiblePosition.Add(new Vector3(curOpponetSidePosition.x, curOpponetSidePosition.y, curOpponetSidePosition.z));
    //            }
    //        }
    //    }

    //    for (int i = 0; i < opponentParent.childCount; i++)
    //    {
    //        if (possiblePosition.Count <= 0) return Vector3.zero;
    //        if(opponentParent.GetChild(i)!=transform)
    //        {
    //            if (possiblePosition.Contains(opponentParent.GetChild(i).localPosition))
    //            {
    //                Debug.Log(transform.name+" has "+ opponentParent.GetChild(i).name);
    //                possiblePosition.Remove(opponentParent.GetChild(i).localPosition);
    //            }
    //        }
    //    }
    //    if (possiblePosition.Count <= 0) return Vector3.zero;
    //    return possiblePosition[0];
    //}

    private Vector3 FindPlusBoxPossiblePosition(Vector3 opponentLocalPosition, Vector3 thisLocalPosition)
    {
        float ableDistance = 0.01f;
        Vector3 result;
        if(thisLocalPosition.x > opponentLocalPosition.x)
        {
            result = CheckPossiblePosition(opponentLocalPosition + Vector3.right, thisLocalPosition, ableDistance);
            if (result != Vector3.zero) return result;
        }
        else
        {
            result = CheckPossiblePosition(opponentLocalPosition + Vector3.left, thisLocalPosition, ableDistance);
            if (result != Vector3.zero) return result;
        }

        if (thisLocalPosition.y > opponentLocalPosition.y)
        {
            result = CheckPossiblePosition(opponentLocalPosition + Vector3.up, thisLocalPosition, ableDistance);
            if (result != Vector3.zero) return result;
        }
        else
        {
            result = CheckPossiblePosition(opponentLocalPosition + Vector3.down, thisLocalPosition, ableDistance);
            if (result != Vector3.zero) return result;
        }
        return Vector3.zero;
    }

    private Vector3 CheckPossiblePosition(Vector3 checkPosition, Vector3 thisLocalPosition, float ableDistance)
    {
        if (Vector3.SqrMagnitude(checkPosition - thisLocalPosition) < ableDistance)
        {
            return checkPosition;
        }
        return Vector3.zero;
    }

    public void DetachPlusBox()
    {
        isAttached = false;
        transform.SetParent(null, true);
        siblingIndex = -1;
    }

    public void DetroyPlusBox()
    {
        if (ableButton)
        {
            if (!ableButton.isOrderOutProcessing)
            {
                ableButton.isOrderOutProcessing = true;
                if (ableButton.DestroyPlusBox(this))
                {
                    DetachPlusBox();
                    gameObject.SetActive(false);
                }
                ableButton.isOrderOutProcessing = false;
            }
        }
        else
        {
            DetachPlusBox();
            gameObject.SetActive(false);
        }
    }

    //undo

    private bool SiblingIndexCompare(object object1, object object2)
    {
        return (int)object1 == (int)object2;
    }

    protected override void SaveUndoData()
    {
        base.SaveUndoData();
        siblingIndexUndoData.SaveUndoData(GameManager.instance?.move ?? 0, siblingIndex, SiblingIndexCompare);
    }

    protected override void GetUndoData()
    {
        base.GetUndoData();
        if (siblingIndexUndoData.GetUndoData(GameManager.instance?.move ?? 0)) DidGetSiblingIndexData();
    }

    private void DidGetSiblingIndexData()
    {
        siblingIndex = (int)siblingIndexUndoData.valueStack.Pop();
        siblingIndexUndoData.indexStack.Pop();
        if (siblingIndex < 0) return;
        if (siblingIndex < (transform.parent?.childCount ?? 0)) transform.SetSiblingIndex(siblingIndex);
    }

    protected override void DidGetParentData()
    {
        base.DidGetParentData();
        isAttached = transform.parent;
    }
}
