using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour {

    public Text NameLabel;
    public GameObject BestData;
    public UILabel BestUserData_Label;

    void GoPlay()
    {
        if (NameLabel.text == "...NAME...")
        {
            
            return;
        }

        NameLabel.text = "Unknown User";
        //Save the text value in NameLabel to string type database UserName
        PlayerPrefs.SetString("UserName", NameLabel.text);
        //Move Scene
        SceneManager.LoadScene("MainPlay");
    }

    void BestScore()
    {
        BestUserData_Label.text = string.Format("{0}:{1:N0}", PlayerPrefs.GetString("BestPlayer"), PlayerPrefs.GetFloat("BestScore"));

        if (BestUserData_Label.text != ":0")
        {
            BestData.SetActive(true);
        }
    }

    void Quit()
    {
        Application.Quit();
    }
}
