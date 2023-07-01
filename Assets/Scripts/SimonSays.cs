using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonSays : MonoBehaviour
{
    public Sprite[] sequenceSprites;
    public Button[] buttons;
    public Image sequenceImage;

    private List<int> spriteSequence;
    private List<int> playerInput;
    private int currentRound;
    private int chances;
    private bool isDisplayingSequence;

    // Start is called before the first frame update
    void Start()
    {
        spriteSequence = new List<int>();
        playerInput = new List<int>();
        currentRound = 1;
        chances = 2;

        GenerateSpriteSequence();
        StartCoroutine(DisplaySequence());
    }

    void GenerateSpriteSequence()
    {
        spriteSequence.Clear();
        int spriteCount = currentRound + 1; // Increase sequence length for each round

        for (int i = 0; i < spriteCount; i++)
        {
            int randomSpriteIndex = Random.Range(0, buttons.Length);
            spriteSequence.Add(randomSpriteIndex);
        }
    }

    IEnumerator DisplaySequence()
    {
        isDisplayingSequence = true;
        DisableButtons();

        foreach (int spriteIndex in spriteSequence)
        {
            yield return new WaitForSeconds(1f);
            sequenceImage.sprite = sequenceSprites[spriteIndex];
            yield return new WaitForSeconds(1f); // Delay between sprite changes
            sequenceImage.sprite = null;
            yield return new WaitForSeconds(0.5f); // Delay before showing the next sprite
        }

        isDisplayingSequence = false;
        EnableButtons();
    }

    void DisableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    void EnableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public void OnButtonClicked(int buttonIndex)
    {
        if (isDisplayingSequence)
            return;

        playerInput.Add(buttonIndex);

        if (playerInput.Count == spriteSequence.Count)
        {
            if (CheckSequence())
            {
                if (currentRound >= 4)
                {
                    Debug.Log("Game Over: You win!");
                }
                else
                {
                    currentRound++;
                    GenerateSpriteSequence();
                    playerInput.Clear();
                    StartCoroutine(DisplaySequence());
                }
            }
            else
            {
                chances--;
                playerInput.Clear();

                if (chances <= 0)
                {
                    Debug.Log("Game Over: You lose!");
                }
            }
        }
    }

    bool CheckSequence()
    {
        for (int i = 0; i < playerInput.Count; i++)
        {
            if (playerInput[i] != spriteSequence[i])
            {
                return false;
            }
        }
        return true;
    }
}
