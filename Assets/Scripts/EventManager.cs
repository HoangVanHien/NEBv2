using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventManager
{
    private UnityEvent moveEvent = new UnityEvent(); //save everytime player move
    private UnityEvent undoEvent = new UnityEvent(); //call when player undo

    public void MoveEvent()
    {
        moveEvent.Invoke();
    }

    public void MoveEventAddListener(UnityAction listener)
    {
        moveEvent.AddListener(listener);
    }

    public void MoveEventRemoveListener(UnityAction toRemove)
    {
        moveEvent.RemoveListener(toRemove);
    }

    public void MoveEventRemoveAllListeners()
    {
        moveEvent.RemoveAllListeners();
    }

    public void UndoEvent()
    {
        undoEvent.Invoke();
    }

    public void UndoEventAddListener(UnityAction listener)
    {
        undoEvent.AddListener(listener);
    }

    public void UndoEventRemoveListener(UnityAction toRemove)
    {
        undoEvent.RemoveListener(toRemove);
    }

    public void UndoEventRemoveAllListeners()
    {
        undoEvent.RemoveAllListeners();
    }
}
