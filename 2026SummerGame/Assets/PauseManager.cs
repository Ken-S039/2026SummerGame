using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause the game
            Time.timeScale = 0f;
            // Show pause menu UI
            // You can implement your own pause menu UI here

            //とりあえずステージ選択画面に遷移するようにしておく
            SceneManager.LoadScene("StageSelectScene");
        }
    }
}
