using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultPopup : MonoBehaviour
{
    public GameObject highScoreLabel;
    public TextMeshProUGUI titleLabel;
    public TextMeshProUGUI scoreLabel;


   //OnEnable함수로 오브젝트 활성화를 캐치할 수 있다 
   private void OnEnable()
   {
        Time.timeScale = 0;

        if (GameManager.Instance.isCleared)
        {   //PlayerPrefs으로 저장하면 디스크에 저장이 되서 게임을 껐다켜도 저장됨
            float highScore = PlayerPrefs.GetFloat("HighScore", 0); //0은 디폴트 값

            if (highScore<GameManager.Instance.timeLimit)
            {
                highScoreLabel.SetActive(true);

                PlayerPrefs.SetFloat("HighScore", GameManager.Instance.timeLimit);    
                PlayerPrefs.Save();
			}
            else
            {
                highScoreLabel.SetActive(false);     
			}
            

            titleLabel.text = "Cleared";  
            scoreLabel.text = GameManager.Instance.timeLimit.ToString("#.##");
		}
        else
        {
            highScoreLabel.SetActive(false);
            titleLabel.text = "Game Over"; 
            scoreLabel.text = "";
		}
   }

   public void PlayAgainPressed()
   {
       Time.timeScale = 1;
       SceneManager.LoadScene("GameScene");
   }

}
