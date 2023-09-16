using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public SpriteRenderer[] btnColors;
    public AudioSource[] btnSounds;

    private int colorSelect;

    public float stayLit;
    private float stayLitCounter;

    public float waitBetweenLights;
    private float waitBetweenCounter;

    private bool shouldBeLit;
    private bool shouldBeDark;

    public List<int> activeSequence;
    private int positionInSequence;

    private bool gameActive;
    private int inputInSequence;

    public AudioSource correct;
    public AudioSource incorrect;

    public TMP_Text scoreText;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }

        scoreText.text = "Score: 0 / High Score: " + PlayerPrefs.GetInt("HighScore");
    }

    private void Update()
    {
        if(shouldBeLit)
        {
            stayLitCounter -= Time.deltaTime;

            if(stayLitCounter < 0 )
            {
                btnColors[activeSequence[positionInSequence]].color = new Color(btnColors[activeSequence[positionInSequence]].color.r, btnColors[activeSequence[positionInSequence]].color.g, btnColors[activeSequence[positionInSequence]].color.b, 0.5f);
                btnSounds[activeSequence[positionInSequence]].Stop();
                shouldBeLit = false;

                shouldBeDark = true;
                waitBetweenCounter = waitBetweenLights;

                positionInSequence++;
            }
        }

        if( shouldBeDark )
        {
            waitBetweenCounter -= Time.deltaTime;

            if(positionInSequence >= activeSequence.Count)
            {
                shouldBeDark = false;
                gameActive = true;
            }
            else
            {
                if(waitBetweenCounter < 0)
                {

                    btnColors[activeSequence[positionInSequence]].color = new Color(btnColors[activeSequence[positionInSequence]].color.r, btnColors[activeSequence[positionInSequence]].color.g, btnColors[activeSequence[positionInSequence]].color.b, 1f);
                    btnSounds[activeSequence[positionInSequence]].Play();

                    stayLitCounter = stayLit;
                    shouldBeLit = true;
                    shouldBeDark = false;
                }
            }
        }
    }

    public void StartGame()
    {
        activeSequence.Clear();
        positionInSequence = 0;

        inputInSequence = 0;

        colorSelect = Random.Range(0, btnColors.Length);

        activeSequence.Add(colorSelect);

        btnColors[activeSequence[positionInSequence]].color = new Color(btnColors[activeSequence[positionInSequence]].color.r, btnColors[activeSequence[positionInSequence]].color.g, btnColors[activeSequence[positionInSequence]].color.b, 1f);
        btnSounds[activeSequence[positionInSequence]].Play();

        stayLitCounter = stayLit;
        shouldBeLit = true;

        scoreText.text = "Score: 0 / High Score: " + PlayerPrefs.GetInt("HighScore");

    }

    public void ColorPressed(int whichButton)
    {
        if (gameActive)
        {

            if (activeSequence[inputInSequence] == whichButton)
            {

                inputInSequence++;

                if (inputInSequence >= activeSequence.Count)
                {
                    StartCoroutine(WaitBetweenSequences());
                }

            }
            else
            {
                //Debug.Log("WRONG!");
                incorrect.Play();

                gameActive = false;
            }
        }
    }

    IEnumerator WaitBetweenSequences()
    {
        yield return new WaitForSeconds(0.5f);

        if(activeSequence.Count > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", activeSequence.Count);
        }
        scoreText.text = "Score: " + activeSequence.Count + " / High Score: " + PlayerPrefs.GetInt("HighScore");

        positionInSequence = 0;
        inputInSequence = 0;

        colorSelect = Random.Range(0, btnColors.Length);

        activeSequence.Add(colorSelect);

        btnColors[activeSequence[positionInSequence]].color = new Color(btnColors[activeSequence[positionInSequence]].color.r, btnColors[activeSequence[positionInSequence]].color.g, btnColors[activeSequence[positionInSequence]].color.b, 1f);
        btnSounds[activeSequence[positionInSequence]].Play();

        stayLitCounter = stayLit;
        shouldBeLit = true;

        gameActive = false;

        //Debug.Log("GOOD!");
        //correct.Play();

    }
}

