using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2DGameManager : MonoBehaviour
{
    public static int spawnPosIndex = -1;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI winText;

    public TextMeshProUGUI scoreText;
    public bool gameOver;
    public bool win;

    public int currentScore;


    private void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        gameOverText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
    }


    public void UpdateScore(int score)
    {
        currentScore += score;
        scoreText.text = currentScore + "";
    }

    public void GameOver()
    {
        if (!win)
        {
            gameOver = true;
            gameOverText.gameObject.SetActive(true);
            StartCoroutine("Restart");
        }
    }

    public void Win()
    {
        if (!gameOver)
        {
            winText.gameObject.SetActive(true);

            Player p = GameObject.FindWithTag("Player").GetComponent<Player>();

            p.isInControl = false;
            p.GetComponent<Animator>().enabled = false;
            p.GetComponent<SpriteRenderer>().sprite = p.winSprite;
            p.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            //Destroy(p.GetComponent<Rigidbody2D>());

            StartCoroutine("Restart");
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);//this current scene
    }
}
