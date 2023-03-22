using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UndoAble : Collidable
{
    private UnityAction moveEvent;
    private UnityAction undoEvent;

    protected UndoData parentUndoData = new UndoData();
    protected UndoData localPositionUndoData = new UndoData();
    protected UndoData activeUndoData = new UndoData();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        moveEvent = MoveEvent;
        undoEvent = UndoEvent;
        GameManager.instance?.MoveEventAddListener(moveEvent);
        GameManager.instance?.UndoEventAddListener(undoEvent);
    }

    protected virtual void OnDestroy()
    {
        GameManager.instance?.MoveEventRemoveListener(moveEvent);
        GameManager.instance?.UndoEventRemoveListener(undoEvent);
    }

    private void MoveEvent()
    {
        SaveUndoData();
    }
    private void UndoEvent()
    {
        GetUndoData();
    }

    private bool ParentCompare(object object1, object object2)
    {
        return (Transform)object1 == (Transform)object2;
    }
    private bool LocalPositionCompare(object object1, object object2)
    {
        return (Vector3)object1 == (Vector3)object2;
    }
    private bool ActiveCompare(object object1, object object2)
    {
        return (bool)object1 == (bool)object2;
    }

    protected virtual void SaveUndoData()
    {
        int move = GameManager.instance?.move ?? 0;
        parentUndoData.SaveUndoData(move, transform.parent, ParentCompare);
        if (transform.parent) localPositionUndoData.SaveUndoData(move, transform.localPosition, LocalPositionCompare);
        else localPositionUndoData.SaveUndoData(move, transform.position, LocalPositionCompare);
        activeUndoData.SaveUndoData(move, gameObject.activeSelf, ActiveCompare);
    }
    protected virtual void GetUndoData()
    {
        int move = GameManager.instance?.move ?? 0;
        if (parentUndoData.GetUndoData(move)) DidGetParentData();
        if (localPositionUndoData.GetUndoData(move)) DidGetLocalPositionData();
        if (activeUndoData.GetUndoData(move)) DidGetActiveData();
    }

    //Update data back to object
    protected virtual void DidGetParentData()
    {
        transform.SetParent((Transform)parentUndoData.valueStack.Pop());
        parentUndoData.indexStack.Pop();
    }
    protected virtual void DidGetLocalPositionData()
    {
        if (transform.parent) transform.localPosition = (Vector3)localPositionUndoData.valueStack.Pop();
        else transform.position = (Vector3)localPositionUndoData.valueStack.Pop();
        Debug.Log(name + " " + transform.localPosition);
        localPositionUndoData.indexStack.Pop();
    }
    protected virtual void DidGetActiveData()
    {
        gameObject.SetActive((bool)activeUndoData.valueStack.Pop());
        activeUndoData.indexStack.Pop();
    }

}

[Serializable]
public class UndoData
{
    public Stack<object> valueStack = new Stack<object>();
    public Stack<int> indexStack = new Stack<int>();

    public void SaveUndoData(int move, object value, Func<object, object, bool> CompareMethod)
    {
        StackCheck(move - 1);//cause we also have to delete case index equal move to add new move
        //if (valueStack.Count == 0 || !CompareMethod(valueStack.Peek(), value))
        //{
            valueStack.Push(value);
            indexStack.Push(move);
        //}
    }

    public bool GetUndoData(int move)
    {
        StackCheck(move);
        return (indexStack.Count > 0 && indexStack.Peek() == move);
    }

    private void StackCheck(int move)
    {
        while (true)
        {
            if (indexStack.Count < valueStack.Count) valueStack.Pop();
            else if (indexStack.Count > valueStack.Count) indexStack.Pop();
            else break;
        }
        if (indexStack.Count <= 0) return;
        while (move < indexStack.Peek())//check if there are error move in stack
        {
            valueStack.Pop();
            indexStack.Pop();
            if (valueStack.Count == 0 || indexStack.Count == 0) break;
        }
    }
}