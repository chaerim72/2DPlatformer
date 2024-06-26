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


   //OnEnable�Լ��� ������Ʈ Ȱ��ȭ�� ĳġ�� �� �ִ� 
   private void OnEnable()
   {
        Time.timeScale = 0;

        if (GameManager.Instance.isCleared)
        {   //PlayerPrefs���� �����ϸ� ��ũ�� ������ �Ǽ� ������ �����ѵ� �����
            float highScore = PlayerPrefs.GetFloat("HighScore", 0); //0�� ����Ʈ ��

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
