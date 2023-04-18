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


    public GameObject CarlosIIIGame2D;

    private void Awake()
    {
        //QualitySettings.vSyncCount = 0;  // VSync must be disabled
        //Application.targetFrameRate = 60;
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

            //Player p = GameObject.Find("Player").transform.gameObject.FindWithTag("Player").GetComponent<Player>();
            Player p = GameObject.FindGameObjectWithTag("2DCarlosIIIGame").transform.Find("Player").GetComponent<Player>();

            p.isInControl = false;
            p.GetComponent<Animator>().enabled = false;
            p.GetComponent<SpriteRenderer>().sprite = p.winSprite;
            p.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            //Destroy(p.GetComponent<Rigidbody2D>());

            //StartCoroutine("Restart");
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); //this current scene. No longer possible as it's not its own scene.

        var oldGame = GameObject.FindGameObjectWithTag("2DCarlosIIIGame");

        var oldScene3D = oldGame.transform.Find("Meta").GetComponent<CambioAEscenaPrincipal>().Scene3D;
        var oldScene2D = oldGame.transform.Find("Meta").GetComponent<CambioAEscenaPrincipal>().Scene2D;
        var oldnextSpawnPosition = oldGame.transform.Find("Meta").GetComponent<CambioAEscenaPrincipal>().nextSpawnPosition;

        oldGame.GetComponent<DestroyCall>().DestroyThisGameObject(0);

        //
        var newGame = Instantiate(CarlosIIIGame2D, transform.parent);

        gameOverText = newGame.transform.Find("CanvasUI").Find("gameOverText").GetComponent<TextMeshProUGUI>();
        winText = newGame.transform.Find("CanvasUI").Find("winText").GetComponent<TextMeshProUGUI>();
        scoreText = newGame.transform.Find("CanvasUI").Find("scoreText").GetComponent<TextMeshProUGUI>();

        newGame.transform.Find("Meta").GetComponent<CambioAEscenaPrincipal>().Scene3D = oldScene3D;
        newGame.transform.Find("Meta").GetComponent<CambioAEscenaPrincipal>().Scene2D = oldScene2D;
        newGame.transform.Find("Meta").GetComponent<CambioAEscenaPrincipal>().nextSpawnPosition = oldnextSpawnPosition;



        init();
    }

    void init()
    {
        gameOverText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
    }
}
