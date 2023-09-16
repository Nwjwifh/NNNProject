using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpriteRenderer[] colors;

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

    private void Start()
    {
        
    }

    private void Update()
    {
        if(shouldBeLit)
        {
            stayLitCounter -= Time.deltaTime;

            if(stayLitCounter < 0 )
            {
                colors[activeSequence[positionInSequence]].color = new Color(colors[activeSequence[positionInSequence]].color.r, colors[activeSequence[positionInSequence]].color.g, colors[activeSequence[positionInSequence]].color.b, 0.5f);
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

                    colors[activeSequence[positionInSequence]].color = new Color(colors[activeSequence[positionInSequence]].color.r, colors[activeSequence[positionInSequence]].color.g, colors[activeSequence[positionInSequence]].color.b, 1f);

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

        colorSelect = Random.Range(0, colors.Length);

        activeSequence.Add(colorSelect);

        colors[activeSequence[positionInSequence]].color = new Color(colors[activeSequence[positionInSequence]].color.r, colors[activeSequence[positionInSequence]].color.g, colors[activeSequence[positionInSequence]].color.b, 1f);

        stayLitCounter = stayLit;
        shouldBeLit = true;
    }

    public void ColorPressed(int whichButton)
    {
        if (gameActive)
        {
            if (activeSequence[inputInSequence] == whichButton)
            {
                Debug.Log("O");

                inputInSequence++;

                if (inputInSequence >= activeSequence.Count)
                {
                    // ����ڰ� �ùٸ� �������� �Ϸ����� �� �� ������ �߰�
                    positionInSequence = 0;
                    inputInSequence = 0;

                    colorSelect = Random.Range(0, colors.Length);

                    activeSequence.Add(colorSelect);

                    colors[activeSequence[positionInSequence]].color = new Color(colors[activeSequence[positionInSequence]].color.r, colors[activeSequence[positionInSequence]].color.g, colors[activeSequence[positionInSequence]].color.b, 1f);

                    stayLitCounter = stayLit;
                    shouldBeLit = true;

                    gameActive = false; // �߰�: �� �������� ����ϵ��� ���� Ȱ��ȭ�� ����
                }
            }
            else
            {
                Debug.Log("x");

                gameActive = false;
            }
        }
    }
}

