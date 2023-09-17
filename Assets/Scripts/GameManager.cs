using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private List<int> playerTaskList = new List<int>();
    private List<int> playerSequenceList = new List<int>();

    public List<AudioClip> buttonSoundsList = new List<AudioClip>();

    public Sprite normalSprite;
    public Sprite highlightSprite;
    public List<Button> clickableButtons;

    public AudioClip loseSound;
    public AudioSource audioSource;
    public CanvasGroup buttons;
    public GameObject startButton;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private float highlightDuration = 0.1f;
    private int score = 0;
    private int highScore = 0;

    private KeyCode upKey = KeyCode.UpArrow;
    private KeyCode leftKey = KeyCode.LeftArrow;
    private KeyCode downKey = KeyCode.DownArrow;
    private KeyCode rightKey = KeyCode.RightArrow;

    private void Awake()
    {
        ResetButtonImages();

        LoadHighScore();

        UpdateScoreText();
        UpdateHighScoreText();
    }

    private void Update()
    {
        DetectKeyInput();
    }

    private void DetectKeyInput()
    {
        if (Input.GetKeyDown(upKey)) AddToPlayerSequenceList(0);
        else if (Input.GetKeyDown(leftKey)) AddToPlayerSequenceList(1);
        else if (Input.GetKeyDown(downKey)) AddToPlayerSequenceList(2);
        else if (Input.GetKeyDown(rightKey)) AddToPlayerSequenceList(3);
    }

    private void AddToPlayerSequenceList(int buttonID)
    {
        playerSequenceList.Add(buttonID);
        StartCoroutine(HighlightButton(buttonID));

        int minLength = Mathf.Min(playerTaskList.Count, playerSequenceList.Count);
        for (int i = 0; i < minLength; i++)
        {
            if (playerTaskList[i] != playerSequenceList[i])
            {
                StartCoroutine(PlayerLost());
                return;
            }
        }

        if (playerSequenceList.Count == playerTaskList.Count)
        {
            ScoreRound();
            StartCoroutine(StartNextRound());
        }
    }

    private void ScoreRound()
    {
        score++;
        UpdateScoreText();

        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
        }

        UpdateHighScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore;
    }

    private IEnumerator HighlightButton(int buttonID)
    {
        clickableButtons[buttonID].image.sprite = highlightSprite;
        audioSource.PlayOneShot(buttonSoundsList[buttonID]);
        yield return new WaitForSeconds(highlightDuration);
        clickableButtons[buttonID].image.sprite = normalSprite;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator PlayerLost()
    {
        audioSource.PlayOneShot(loseSound);
        playerSequenceList.Clear();
        playerTaskList.Clear();
        yield return new WaitForSeconds(2f);
        startButton.SetActive(true);
        buttons.interactable = false;
    }

    private IEnumerator StartNextRound()
    {
        playerSequenceList.Clear();
        buttons.interactable = false;
        yield return new WaitForSeconds(1f);
        playerTaskList.Add(Random.Range(0, 4));

        foreach (int index in playerTaskList)
        {
            yield return StartCoroutine(HighlightButton(index));
        }
        buttons.interactable = true;
    }

    private void ResetButtonImages()
    {
        foreach (var button in clickableButtons)
        {
            button.image.sprite = normalSprite;
        }
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public void StartGame()
    {
        score = 0;
        UpdateScoreText();
        StartCoroutine(StartNextRound());
        startButton.SetActive(false);
    }
}
