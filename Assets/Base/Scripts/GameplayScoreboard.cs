using TMPro;
using UnityEngine;

namespace heyjoelang
{
    public class GameplayScoreboard : Singleton<GameplayScoreboard>
    {
        static int score = 0;
        public TMP_Text scoreText;
        public TMP_Text timerText;
        public void IncreaseScore()
        {
            score += 10;
            scoreText.text = string.Format("{0}", score);
        }
        public void UpdateTimerText(float timeLeft)
        {
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        public void ResetScore()
        {
            score = 0;
        }
    }
}
