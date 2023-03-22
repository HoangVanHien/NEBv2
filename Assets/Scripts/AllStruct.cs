using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ButtonStruct
{
    //every time add a new button, add sprite to ButtonBox.cs and ButtonBox in Prefab, add image to SlotUI.cs and SlotImage in prefab
    public bool moveLeft;
    public bool moveRight;
    public bool moveUp;
    public bool moveDown;

    public ButtonStruct(bool moveLeft, bool moveRight, bool moveUp, bool moveDown)
    {
        this.moveLeft = moveLeft;
        this.moveRight = moveRight;
        this.moveDown = moveDown;
        this.moveUp = moveUp;
    }
    public ButtonStruct(bool forAll)
    {
        this.moveLeft = forAll;
        this.moveRight = forAll;
        this.moveDown = forAll;
        this.moveUp = forAll;
    }
}
