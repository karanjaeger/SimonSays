using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonSays : MonoBehaviour
{
    public Sprite[] sequenceSprites;
    public AudioClip[] sequenceAudioClips;
    public Image[] lights;
    public Button[] buttons;
    public Image sequenceImage;
    public Sprite defaultImage;
    public AudioSource[] sources;    
    public Image loseImage;
    public Bridge bridge;

    private List<int> spriteSequence;
    private List<int> playerInput;
    private int currentRound;
    private int chances;
    private bool isDisplayingSequence;
    private AudioSource audioSource;




    void Start()
    {
        loseImage.enabled = false;
        spriteSequence = new List<int>();
        playerInput = new List<int>();
        currentRound = 1;
        chances = 2;
        audioSource = GetComponent<AudioSource>();

        GenerateSpriteSequence();
        StartCoroutine(DisplaySequence());
    }

    void GenerateSpriteSequence()
    {
        spriteSequence.Clear();
        int spriteCount = currentRound + 1;

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
            audioSource.clip = sequenceAudioClips[spriteIndex];
            audioSource.Play();
            yield return new WaitForSeconds(0.5f);
            sequenceImage.sprite = defaultImage;
            yield return new WaitForSeconds(0.5f); 
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
        audioSource.clip = sequenceAudioClips[buttonIndex];
        audioSource.Play();

        if (playerInput.Count == spriteSequence.Count)
        {
            if (CheckSequence())
            {
                if (currentRound >= 4)
                {
                    
                    Debug.Log("Game Over: You win!");
                    lights[3].enabled = true;
                    StartCoroutine(VictoryCall());
                }
                else
                {
                    currentRound++;
                    RoundLED();
                    GenerateSpriteSequence();
                    playerInput.Clear();
                    StartCoroutine(DisplaySequence());
                }
            }
            else
            {
                chances--;
                playerInput.Clear();

                if (chances == 1)
                {
                    Debug.Log("You fucked up. Last chance");
                    sources[1].Play();
                    GenerateSpriteSequence();
                    StartCoroutine(DisplaySequence());
                }
                else if (chances == 0)
                {
                    StartCoroutine(DefeatCall());
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

    private IEnumerator VictoryCall()
    {
        sources[0].Play();
        yield return new WaitForSeconds(2);
        bridge.TriggerWebCall("winScenario");
        
    }

    private IEnumerator DefeatCall()
    {
        sources[1].Play();
        yield return new WaitForSeconds(1.5f);
        loseImage.enabled = true;
        yield return new WaitForSeconds(2);
        bridge.TriggerWebCall("failScenario");

    }
    private void RoundLED()
    {
        switch(currentRound)
        {
            case 2:
                lights[0].enabled = true; 
            break;
            case 3:
                lights[1].enabled = true;
            break;
            case 4:
                lights[2].enabled = true;
            break;
            case 5:
                lights[3].enabled = true;
            break;
            default:
                Debug.Log("Default");
            break;
        }
    }
}
