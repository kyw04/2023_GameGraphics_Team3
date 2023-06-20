using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform topPos;
    public Transform downPos;
    public Transform main;
    public SpriteRenderer button;
    public Animator chain;

    public float moveSpeed = 1.5f;

    public Sprite buttonUp;
    public Sprite buttonDown;
    public AudioClip buttonClickSound;
    public AudioClip fixChainSound;

    private AudioSource audioSource;
    private bool isButtonDown;
    private bool isChainOn;
    private bool down;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isButtonDown = false;
        isChainOn = false;
        down = true;
    }

    private void Update()
    {
        if (down && downPos.position.y < main.position.y)
        {
            main.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isButtonDown && isChainOn && collision.CompareTag("PlayerFoot"))
        {
            if (topPos.position.y > main.position.y)
            {
                down = false;
                main.position += Vector3.up * moveSpeed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFoot"))
        {
            down = true;
        }
    }

    public void ButtonClick()
    {
        if (isChainOn)
        {
            audioSource.PlayOneShot(buttonClickSound);
            isButtonDown = !isButtonDown;
        }
        else
            isButtonDown = false;

        if (isButtonDown)
            button.sprite = buttonDown;
        else
            button.sprite = buttonUp;
    }

    public void FixChain()
    {
        audioSource.PlayOneShot(fixChainSound);
        chain.SetTrigger("onChain");
        isChainOn = true;
    }
}
