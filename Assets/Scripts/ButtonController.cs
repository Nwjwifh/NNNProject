using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int btnNumber;
    private GameManager gameManager;
    private AudioSource audioSource;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // 키보드 입력 감지
        if (Input.GetKeyDown(KeyCode.UpArrow) && btnNumber == 0)
        {
            OnButtonPressed();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && btnNumber == 1)
        {
            OnButtonPressed();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && btnNumber == 2)
        {
            OnButtonPressed();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && btnNumber == 3)
        {
            OnButtonPressed();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        }
    }

    public void OnButtonPressed()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        audioSource.Play();
        gameManager.ColorPressed(btnNumber);
    }

    public void OnButtonReleased()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
    }
}
