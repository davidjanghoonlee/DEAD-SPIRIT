using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayManager : MonoBehaviour {

    public bool PlayEnd;
    public float Limit_Time = 60f;
    public int Enemy_Count = 10;

    public UILabel TimeLabel;
    public UILabel EnemyLabel;
    public GameObject FinalGUI;
    public UILabel FinalMessage;
    public UILabel FinalScoreLabel;

    public UILabel PlayerName;

    void Start(){
        EnemyLabel.text = string.Format("Enemy : {0}", Enemy_Count);
        TimeLabel.text = string.Format("Time : {0:N2}", Limit_Time);

        PlayerName.text = PlayerPrefs.GetString("UserName");
    }

    void Update()
    {
        if (PlayEnd != true)
        {
            if (Limit_Time > 0)
            {
                Limit_Time -= Time.deltaTime;
                TimeLabel.text = string.Format("Time:{0:N2}", Limit_Time);
            }
            else
            {
                GameOver();
            }
        }
    }

    public void Clear()
    {
        if (PlayEnd != true)
        {
            Time.timeScale = 0;
            PlayEnd = true;
            FinalMessage.text = "Clear!!";

            //Find player and bring Player_Ctrl as PC
            Player_Ctrl PC = GameObject.Find("Player").GetComponent<Player_Ctrl>();

            //Formula for score: clear score + time left + hp left
            float score = 12345f + Limit_Time * 123f + PC.hp * 123f;
            FinalScoreLabel.text = string.Format("{0:N0}", score);

            //Enable GUI in the end
            FinalGUI.SetActive(true);

            BestCheck(score);

        }
    }

    public void GameOver()
    {
        if (PlayEnd != true)
        {
            Time.timeScale = 0;
            PlayEnd = true;
            FinalMessage.text = "Fail...";
            float score = 1234f + Enemy_Count * 123f;
            FinalScoreLabel.text = string.Format("{0:N0}", score);
            FinalGUI.SetActive(true);

            BestCheck(score);
        }

        //Find player and bring Player_Ctrl as PC
        Player_Ctrl PC = GameObject.Find("Player").GetComponent<Player_Ctrl>();
        PC.PS = PlayerState.Dead;
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainPlay");
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    public void EnemyDie()
    {
        Enemy_Count -= 1;
        EnemyLabel.text = string.Format("Enemy : {0}", Enemy_Count);
        if (Enemy_Count <= 0)
        {
            Clear();
        }
    }

    public void BestCheck(float score)
    {
        //Bring the saved best score
        float BestScore = PlayerPrefs.GetFloat("BestScore");

        //If the current score is greater than the best score, over-write it
        if (score > BestScore)
        {
            PlayerPrefs.SetFloat("BestScore", score);
            PlayerPrefs.SetString("BestPlayer", PlayerPrefs.GetString("UserName"));
        }

    }

    
}
