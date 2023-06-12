using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public void GameStart()
    {
        SceneChangeManager.instance.ChangeScene("Level_1");
    }
}
