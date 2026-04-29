using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI lastScoreText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        if (lastScoreText != null)
        {
            lastScoreText.text = "Previous Score: " + lastScore.ToString();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}