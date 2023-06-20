using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioClip buttonClickSound;

    public void GameStart()
    {
        SceneChangeManager.instance.audioSource.PlayOneShot(buttonClickSound);
        SceneChangeManager.instance.ChangeScene("Level_1");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
