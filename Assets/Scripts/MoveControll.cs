using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControll : MonoBehaviour
{

    private float boxColliderSize = 0.5f;
    public LayerMask blockingMask;

    private float normalMoveSpeed = 5;
    private float moveSpeed;
    private Vector3 beforeChangePosition;
    private Vector3 changeablePosition;
    private bool isMove = false;
    private Vector3 moveDirection;

    private float rotateSpeed = 2;
    private Vector3 changeableAngle;
    private Vector3 finalChangeAngle;
    private float finalAngleZ;
    private bool isRotate = false;
    private Vector3 rotateAngle;//z = 90 or -90
    private bool rotateLoop;

    private void FixedUpdate()
    {
        if (GameManager.instance.stopEverything) return;
        if (isMove)
        {
            if (!MoveCheck())
            {
                transform.position += changeablePosition - transform.position;
                if (beforeChangePosition == transform.position) GameManager.instance.move--;
                isMove = false;
            }
            else
            {
                transform.position += moveDirection.normalized * Time.fixedDeltaTime * moveSpeed;
            }
        }
        if (isRotate)
        {
            if (!RotateCheck())
            {
                transform.eulerAngles = changeableAngle;
                isRotate = false;
            }
            else
            {
                if (rotateAngle.z <= 0)
                {
                    if(!AngleAcceptableFalse(ToRotateAngle(transform.eulerAngles, rotateAngle), finalChangeAngle, rotateAngle, 0, 10)){
                        finalAngleZ = transform.eulerAngles.z + 90;
                    }
                    transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, finalAngleZ, Time.fixedDeltaTime * rotateSpeed));
                }
                else
                {
                    if (!AngleAcceptableFalse(ToRotateAngle(transform.eulerAngles, rotateAngle), finalChangeAngle, rotateAngle, 0, 10))
                    {
                        finalAngleZ = transform.eulerAngles.z - 90;
                    }
                    transform.eulerAngles = new Vector3(0, 0, -Mathf.LerpAngle(-transform.eulerAngles.z, 360 - finalAngleZ, Time.fixedDeltaTime * rotateSpeed));
                }
                //Debug.Log(transform.eulerAngles.z + " => " + finalAngleZ);
            }
        }
    }

    public void StopMove()
    {
        isMove = isRotate = false;
    }

    public bool MoveAble()
    {
        return !(isMove || isRotate);//actually it's (!isMove && !isRotate)
    }

    private bool MovedVectoCheck(Vector3 nowPosition, Vector3 beforePosition, Vector3 direction)
    {
        return (((nowPosition.x - beforePosition.x >= direction.x && direction.x >= 0) ||
            (nowPosition.x - beforePosition.x <= direction.x && direction.x <= 0))
            &&
            ((nowPosition.y - beforePosition.y >= direction.y && direction.y >= 0) ||
            (nowPosition.y - beforePosition.y <= direction.y && direction.y <= 0)));
    }

    private bool MoveCheck()
    {
        if(MovedVectoCheck(transform.position, changeablePosition, moveDirection.normalized))//update changeable
        {
            changeablePosition += moveDirection.normalized;
        }
        if (changeablePosition == beforeChangePosition + moveDirection)//move done
        {
            return false;
        }
        return ChildMoveableCheck();
    }

    public void Move(Vector3 direction)
    {
        isMove = true;
        beforeChangePosition = changeablePosition = transform.position;
        moveDirection = direction;
        moveSpeed = (moveDirection.magnitude + 1) / 2 * normalMoveSpeed;
    }

    private Vector3 ToRotateAngle(Vector3 originAngle, Vector3 toRotateAngle)//calculate angle
    {
        if (toRotateAngle.z == 0) return originAngle;

        Vector3 result;
        result = originAngle - toRotateAngle;
        if (result.z >= 360)
        {
            result.z -= 360;
        }
        else if (result.z < 0)
        {
            result.z += 360;
        }
        return result;
    }

    //check if 2 angle can be count as equal, compareToThis must be a const angle
    private bool AngleAcceptableFalse(Vector3 compareThisAngle, Vector3 compareToThis, Vector3 toRotateAngle, float acceptableAngleWillReach, float acceptableAnglePassed)
    {
        if (toRotateAngle.z >= 0)//right
        {
            if (compareToThis.z == 0)
            {
                if (compareThisAngle.z >= 360 - acceptableAnglePassed || compareThisAngle.z <= acceptableAngleWillReach)
                {
                    return true;
                }
            }
            else if (compareThisAngle.z >= compareToThis.z - acceptableAnglePassed && compareThisAngle.z <= compareToThis.z + acceptableAngleWillReach)
            {
                return true;
            }
        }
        else//left
        {
            if (compareToThis.z == 0)
            {
                if (compareThisAngle.z >= 360 - acceptableAngleWillReach || compareThisAngle.z <= acceptableAnglePassed)
                {
                    return true;
                }
            }
            else if (compareThisAngle.z >= compareToThis.z - acceptableAngleWillReach && compareThisAngle.z <= compareToThis.z + acceptableAnglePassed)
            {
                return true;
            }
        }
        return false;
    }

    public bool RotateCheck()
    {
        if (AngleAcceptableFalse(transform.eulerAngles, ToRotateAngle(changeableAngle, rotateAngle), rotateAngle, 2, 10))//update changeable
        {
            changeableAngle = ToRotateAngle(changeableAngle, rotateAngle);
            Debug.Log(Time.time + " " + changeableAngle.z + " " + finalChangeAngle.z);
        }
        if (changeableAngle == finalChangeAngle)//move done
        {
            return false;
        }
        if (!ChildMoveableCheck())//collide with blocking
        {
            //make it rotate backward
            finalChangeAngle = changeableAngle;
            changeableAngle = ToRotateAngle(changeableAngle, rotateAngle);
            rotateAngle.z = -rotateAngle.z;
            if (finalChangeAngle.z == 0 && rotateAngle.z <= 0) finalAngleZ = 360;
            else if (finalChangeAngle.z == 270 && rotateAngle.z >= 0) finalAngleZ = -90;
            else finalAngleZ = finalChangeAngle.z;
        }
        return true;
    }

    public void RotateMove(Vector3 angle)
    {
        isRotate = true;
        rotateLoop = (angle.z >= 4 || angle.z <= -4);
        if (angle.z < 0)
        {
            angle.z = -((-angle.z) % 4);
        }
        else angle.z = angle.z % 4;
        rotateAngle = angle.normalized * 90;
        finalChangeAngle = ToRotateAngle(transform.eulerAngles, angle * 90);
        finalAngleZ = transform.eulerAngles.z - rotateAngle.z;

        changeableAngle = transform.eulerAngles;
    }

    //is all plusbox able to go 
    private bool ChildMoveableCheck()
    {
        bool result = true;
        int length = transform.childCount;
        for (int i = 0; i < length; i++)
        {
            if (result)
            {
                Transform childCheck = transform.GetChild(i);
                if (childCheck.CompareTag("PlusBox"))
                {
                    BoxCollider2D boxCollider = childCheck.GetComponent<BoxCollider2D>();
                    RaycastHit2D hit = Physics2D.BoxCast(childCheck.position, boxCollider.size * boxColliderSize, 0, Vector2.zero, 1, blockingMask);
                    result = (hit.collider == null);
                }
            }
            else break;
        }
        return result;
    }
}
