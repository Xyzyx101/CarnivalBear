using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    public PlayerCharacter Player;
    public Text ScoreText;
    public Text TimeText;
    public Image HealthBar;

    private float LevelTime;
    private float Score;

    // Update is called once per frame
    void Update()
    {
        LevelTime += Time.deltaTime;
        TimeText.text = TimeToString(LevelTime);
        ScoreText.text = Score.ToString();
        float normalizedHealth = Player.GetNormalizedHealth();
        HealthBar.rectTransform.localScale = new Vector3(normalizedHealth, 1f);
        HealthBar.color = Color.Lerp(Color.red, Color.green, normalizedHealth);
        Debug.Log(normalizedHealth);
    }

    public void AddScore(float amount)
    {
        Score += amount;
    }

    string TimeToString(float time)
    {
        int minutes = (int)time / 60;
        float seconds = Mathf.Floor((time - (float)minutes * 60f) * 100f) * 0.01f;
        return minutes.ToString() + ":" + (seconds < 10f ? "0" : "") + seconds.ToString();
    }
}
