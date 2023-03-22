using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public GameObject endLevel;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ResetLevel();
        //if (Input.GetKeyDown(KeyCode.Escape)) PauseMenu();
    }

    public void ResetLevel()
    {
        //if (GameManager.instance.CurLevelIsBaseLevel()) return;

        //GameManager.instance.ResetLevel();
        GameManager.instance.stopEverything = true;
        GameManager.instance.move = 1;
        GameManager.instance.UndoEvent();
        endLevel.SetActive(false);
        endLevel.transform.GetChild(0).gameObject.SetActive(false);
        endLevel.transform.GetChild(1).gameObject.SetActive(false);
        GameManager.instance.stopEverything = false;
    }

    public void UndoLevel()
    {
        GameManager.instance.UndoEvent();
    }

    public void PauseMenu()
    {
        //if (SettingManager.instance.IsOpen())
        //{
        //    SettingManager.instance.CloseSetting();
        //}
        //if (!GameManager.instance.CurLevelIsBaseLevel() && !transform.GetChild(3).gameObject.activeSelf)
        //{
        //    GameObject pauseMenu = transform.GetChild(2).gameObject;
        //    pauseMenu.SetActive(!pauseMenu.activeSelf);
        //    PauseLevel(pauseMenu.activeSelf);
        //}
    }

    public void PauseLevel(bool isPause)
    {
        GameManager.instance.stopEverything = isPause;
    }

    public void MainMenu()
    {
        //GameManager.instance.LoadMainMenuLevel();
    }

    public void Map()
    {
        //GameManager.instance.LoadMapLevel();
    }

    private bool levelIsEnded = false;

    public void GameWin()
    {
        if (levelIsEnded) return;
        levelIsEnded = true;
        GameManager.instance.stopEverything = true;

        CollectablePointManager collectablePointManager = GameObject.Find("CollectablePointManager").GetComponent<CollectablePointManager>();
        collectablePointManager.UpdateGameData();

        endLevel.SetActive(true);
        endLevel.transform.GetChild(0).gameObject.SetActive(true);
        int levelPoint = collectablePointManager.GetLevelPoint();
        Transform victoryText = endLevel.transform.GetChild(0).GetChild(0);
        for (int i = 0; i < levelPoint; i++)
        {
            if (i >= 3) break;
            victoryText.GetChild(i).gameObject.SetActive(true);
        }
        //AudioManager.instance.Play("Win");
    }

    public void GameOver()
    {
        if (levelIsEnded) return;
        levelIsEnded = true;
        GameManager.instance.stopEverything = true;
        endLevel.SetActive(true);
        endLevel.transform.GetChild(1).gameObject.SetActive(true);
        //AudioManager.instance.Play("Lose");
    }
}
