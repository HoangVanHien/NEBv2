using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePointManager : MonoBehaviour
{
    //this save the point of only the current level
    public int typeOfLevelPoint;
    private int levelPoint;
    private int heartPoint;
    private int goldPoint;
    private string curLevelName;

    // Start is called before the first frame update
    private void Start()
    {
        levelPoint = 0;
        heartPoint = 0;
        goldPoint = 0;
        //curLevelName = GameManager.instance.GetCurLevelName();
    }
    public int GetLevelPoint()
    {
        return levelPoint;
    }

    public void AddCollectablePoint(int typeOfPoint)
    {
        switch (typeOfPoint)
        {
            case 0:
                {
                    AddHeartPoint();
                    break;
                }
            case 1:
                {
                    AdddGoldPoint();
                    break;
                }
        }
    }

    private void AddHeartPoint()
    {
        heartPoint++;
        if (typeOfLevelPoint == 0)
        {
            levelPoint++;
        }
    }

    public int GetHeartPoint()
    {
        return heartPoint;
    }

    private void AdddGoldPoint()
    {
        goldPoint++;
        if (typeOfLevelPoint == 1)
        {
            levelPoint++;
        }
    }

    public int GetGoldPoint()
    {
        return goldPoint;
    }

    public void UpdateGameData()
    {
        //GameManager.instance.SetLevelPoint(curLevelName, levelPoint);
        //GameManager.instance.SetHeartPoint(curLevelName, heartPoint);
        //GameManager.instance.SetGoldPoint(curLevelName, goldPoint);
    }
}
