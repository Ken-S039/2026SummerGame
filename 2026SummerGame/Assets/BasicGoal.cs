using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BasicGoal : MonoBehaviour
{
    [SerializeField]
    protected string stageSelectSceneName = "StageSelectScene";

    //ƒS[ƒ‹‚µ‚½‚©‚Ç‚¤‚©
    private bool isCleared = false;

    //ƒS[ƒ‹‚ÆÚG‚µ‚½‚ÉŒÄ‚Î‚ê‚é
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCleared)
            return;

        if (!other.CompareTag("Player"))
            return;

        // ”h¶ƒNƒ‰ƒX‚ÅƒS[ƒ‹ğŒ‚ğ”»’è
        if (CanGoal())
        {
            isCleared = true;
            Goal();
            Debug.Log("Goal!");
        }
    }

    /// <summary>
    /// ƒS[ƒ‹‰Â”\‚©‚Ç‚¤‚©‚ğ”»’è‚·‚é
    /// </summary>
    protected abstract bool CanGoal();

    /// <summary>
    /// ƒS[ƒ‹‚µ‚½‚Æ‚«‚Ìˆ—
    /// </summary>
    protected virtual void Goal()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
