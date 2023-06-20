using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager instance;
    public AnimationClip animationClip;
    public Image gamOverImage;
    public AudioSource audioSource;

    private Animator changeSceneAnimator;
    private AnimationEvent[] animationEvents;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(instance); }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        changeSceneAnimator =  GetComponent<Animator>();
        animationEvents = animationClip.events;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "Start")
                ChangeScene("Start");
            else
                Application.Quit();
        }
    }

    public void ChangeScene(string name)
    {
        animationEvents[0].stringParameter = name;
        animationClip.events = animationEvents;

        changeSceneAnimator.SetTrigger("doChange");
    }

    public void GameOver()
    {
        changeSceneAnimator.SetTrigger("GameOver");
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
