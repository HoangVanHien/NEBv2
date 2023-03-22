using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] private AbleButton ableButton;
    
    private Vector3 direction;
    private bool isMove = false;
    private bool isUndo = false;
    
    //private Vector3 angle;
    //private bool isRotate = false;

    private void Start()
    {
        ableButton = GetComponent<AbleButton>();
        if (!ableButton) ableButton = gameObject.AddComponent<AbleButton>();
        direction = Vector3.zero;
        //angle = Vector3.zero;
    }

    private void Update()
    {
        if (direction == Vector3.zero)
        {
            //x
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
                direction.x = -1;
                isMove = true;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                direction.x = 1;
                isMove = true;
            }

            //y
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                direction.y = -1;
                isMove = true;
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                direction.y = 1;
                isMove = true;
            }
            
            //rotate
            //else if (Input.GetKey(KeyCode.Q))
            //{
            //    angle.z = -1;
            //    isRotate = true;
            //}
            //else if (Input.GetKey(KeyCode.E))
            //{
            //    angle.z = 1;
            //    isRotate = true;
            //}
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                isUndo = true;
            }

        }
    }

    private void FixedUpdate()
    {
        if (isUndo)
        {
            GameManager.instance.UndoEvent();
            isUndo = false;
        }
        else if (isMove)
        {
            if (ableButton.moveControll.MoveAble()) GameManager.instance.MoveEvent();
            ableButton.MoveOnPress(direction);
            isMove = false;
            direction = Vector3.zero;
        }
    }
}
