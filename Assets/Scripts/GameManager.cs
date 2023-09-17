using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> buttonSoundsList;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightSprite;
    [SerializeField] private List<Button> clickableButtons;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private CanvasGroup buttons;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private List<int> playerTaskList = new List<int>();
    private List<int> playerSequenceList = new List<int>();

    private float highlightDuration = 0.1f;
    private int score = 0;
    private int highScore = 0;

    private KeyCode[] arrowKeys = { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };

    private void Awake()
    {
        ResetButtonImages();
        LoadHighScore();
        UpdateUI();
    }

    private void Update()
    {
        DetectKeyInput();
    }

    private void DetectKeyInput()
    {
        for (int i = 0; i < arrowKeys.Length; i++)
        {
            if (Input.GetKeyDown(arrowKeys[i]))
            {
                AddToPlayerSequenceList(i);
            }
        }
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
        UpdateUI();

        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score;
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
        startButton.gameObject.SetActive(true);
        buttons.interactable = false;
    }

    private IEnumerator StartNextRound()
    {
        playerSequenceList.Clear();
        buttons.interactable = false;
        yield return new WaitForSeconds(1f);

        int newPattern = Random.Range(0, 4);
        playerTaskList.Add(newPattern);

        for (int i = 0; i < playerTaskList.Count; i++)
        {
            int index = playerTaskList[i];
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
        UpdateUI();
        StartCoroutine(StartNextRound());
        startButton.gameObject.SetActive(false);
    }
}