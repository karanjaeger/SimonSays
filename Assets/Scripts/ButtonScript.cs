using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public int buttonIndex;
    private AudioSource buttonAudio;
    private SimonSays gameManager;
    private Button button;

    private void Start()
    {
        gameManager = FindObjectOfType<SimonSays>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        gameManager.OnButtonClicked(buttonIndex);

    }
}
