using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    public PlayerCharacter Player;
    public Text ScoreText;
    public Text TimeText;
    public Image HealthBar;
    public Image StunBar;
    public float StunBarPulseSpeed;
    public Color StunColor1;
    public Color StunColor2;
    private float LevelTime;
    private float Score;
    
    void Update()
    {
        LevelTime += Time.deltaTime;
        TimeText.text = TimeToString(LevelTime);
        ScoreText.text = Score.ToString();

        float normalizedHealth = Player.GetNormalizedHealth();
        HealthBar.rectTransform.localScale = new Vector3(normalizedHealth, 1f);
        HealthBar.color = Color.Lerp(Color.red, Color.green, normalizedHealth);

        float normalizedStun = Player.GetNormalizedStun();
        StunBar.enabled = !(normalizedStun == 0f);
        StunBar.rectTransform.localScale = new Vector3(normalizedStun, 1f);
        StunBar.color = Color.Lerp(StunColor1, StunColor2, Mathf.Sin(StunBarPulseSpeed * Time.timeSinceLevelLoad));
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
