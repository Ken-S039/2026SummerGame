using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    //最終目標の関数の形。値を与えてその値に応じたステージに遷移させたい。
    public void SelectStage(int stageNumber)
    {
        
    }

    //ステージ選択画面に移動
    public void LoadStageSelect()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    //ステージ1に遷移
    public void LoadStage1()
    {
        SceneManager.LoadScene("Stage1Scene");
    }

    //とりあえずキーボード入力Aでステージ遷移するようにしておく
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadStage1();
        }
    }

}
