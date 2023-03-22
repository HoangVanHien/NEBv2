using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Button box hold the value of button
/// </summary>
public class ButtonBox : UndoAble
{
    private bool isAttached = false;
    private float plusBoxColliableDistance = 0.02f;

    public ButtonStruct buttonStruct;

    public GameObject moveLeftPrefab, moveRightPrefab, moveUpPrefab, moveDownPrefab;

    protected override void Start()
    {
        ButtonVisualSettup();
        base.Start();
        if (transform.parent)
        {
            AddToFreePlusBox(transform.GetComponentInParent<PlusBox>());
        }
    }

    private void ButtonVisualSettup()
    {
        GameObject visual;
        //Move
        if (buttonStruct.moveLeft)
        {
            visual = Instantiate(moveLeftPrefab, transform);
            visual.transform.localPosition = Vector3.zero;
        }
        if (buttonStruct.moveRight)
        {
            visual = Instantiate(moveRightPrefab, transform);
            visual.transform.localPosition = Vector3.zero;
        }
        if (buttonStruct.moveUp)
        {
            visual = Instantiate(moveUpPrefab, transform);
            visual.transform.localPosition = Vector3.zero;
        }
        if (buttonStruct.moveDown)
        {
            visual = Instantiate(moveDownPrefab, transform);
            visual.transform.localPosition = Vector3.zero;
        }
    }

    protected override void FixedUpdate()
    {
        if (isAttached) return;
        base.FixedUpdate();
    }

    protected override void OnCollide(Collider2D coll) //only interacts with plus box
    {
        if (isAttached) return;
        if (Vector3.SqrMagnitude(transform.position - coll.transform.position) <= plusBoxColliableDistance)
        {
            PlusBox opponentPlusBox = coll.GetComponent<PlusBox>();
            if (!opponentPlusBox)
            {
                return;
            }
            AbleButton opponentAbleButton = opponentPlusBox.GetAbleButton();
            if (opponentAbleButton)
            {
                if (!opponentAbleButton.isOrderOutProcessing)//wait for the others
                {
                    opponentAbleButton.isOrderOutProcessing = true;
                    //Debug.Log(name + " collide" + coll.name);
                    AddToPlusBox(opponentPlusBox);
                    //Debug.Log(name + " end collide" + coll.name);
                    opponentAbleButton.isOrderOutProcessing = false;
                }
            }
            else AddToPlusBox(opponentPlusBox);
        }
    }

    private void AddToPlusBox(PlusBox opponentPlusBox)
    {
        //isAttached = true;
        if (!opponentPlusBox.HasButton())
        {
            AddToFreePlusBox(opponentPlusBox);
        }
        else if (opponentPlusBox.IsAttached())//Has button
        {
            //Debug.Log("HasButton "+Time.time+" " + opponentPlusBox.name);
            AddToHasButtonPlusBox(opponentPlusBox);
        }
    }


    public void AddToFreePlusBox(PlusBox opponentPlusBox)
    {
        if (!opponentPlusBox)
        {
            Debug.Log("AddToFreePlusBox: null Plus Box");
            return;
        }
        transform.SetParent(opponentPlusBox.transform);
        transform.localPosition = Vector3.zero;
        //transform.localScale = new Vector3(0.6f, 0.6f, 0);
        isAttached = true;
        opponentPlusBox.GetAbleButton()?.AddPlusBoxOrderOut(opponentPlusBox);
    }

    private void AddToHasButtonPlusBox(PlusBox opponentPlusBox)
    {
        if (!opponentPlusBox)
        {
            Debug.Log("AddToHasButtonPlusBox: null Plus Box");
            return;
        }
        AbleButton ableButton = opponentPlusBox.GetAbleButton();
        if (!ableButton)
        { 
            Debug.Log("AddToHasButtonPlusBox: albeButton null");
            return;
        }
        ableButton.AdjustButtons(this, opponentPlusBox);
    }

    //Keep the same index order in order out, just change the plus box index
    public void ChangeToAnotherPlusBox(PlusBox newPlusBox)
    {
        PlusBox curPlusBox = transform.GetComponentInParent<PlusBox>();//before change plusbox
        if (!curPlusBox)
        {
            return;
        }
        AbleButton ableButton = curPlusBox.GetAbleButton();
        if (!ableButton)
        {
            return;
        }
        AddToFreePlusBox(newPlusBox);
        //fix the order out
        int curIndex = ableButton.orderOutPlusBox.IndexOf(ableButton.IndexOfPlusBox(curPlusBox));
        int newPlusBoxIndex = ableButton.IndexOfPlusBox(newPlusBox);
        ableButton.orderOutPlusBox.RemoveAt(ableButton.orderOutPlusBox.LastIndexOf(newPlusBoxIndex));//remove the new number when AddToFree
        ableButton.orderOutPlusBox[curIndex] = newPlusBoxIndex;//set new plus box but for the same index of buttonbox in order out
    }

    public void DetroyButtonBox()
    {
        transform.SetParent(null, true);
        gameObject.SetActive(false);
    }

    protected override void DidGetParentData()
    {
        base.DidGetParentData();
        isAttached = transform.parent;
    }
}
