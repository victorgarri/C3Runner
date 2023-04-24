using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameStates { countDown, running, raceOver };

public class CarGameManager : MonoBehaviour
{
    //Static instance of GameManager so other scripts can access it
    public static CarGameManager instance = null;

    //States
    GameStates gameState = GameStates.countDown;

    //Time
    float raceStartedTime = 0;
    float raceCompletedTime = 0;

    //Driver information
    List<DriverInfo> driverInfoList = new List<DriverInfo>();

    //Events
    public event Action<CarGameManager> OnGameStateChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Supply dummy driver information for testing purposes
        driverInfoList.Add(new DriverInfo(1, "P1", 0, false));
    }

    void LevelStart()
    {
        gameState = GameStates.countDown;

        Debug.Log("Level started");
    }


    public GameStates GetGameState()
    {
        return gameState;
    }

    void ChangeGameState(GameStates newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;

            //Invoke game state change event
            OnGameStateChanged?.Invoke(this);
        }
    }

    public float GetRaceTime()
    {
        if (gameState == GameStates.countDown)
            return 0;
        else if (gameState == GameStates.raceOver)
            return raceCompletedTime - raceStartedTime;
        else return Time.time - raceStartedTime;
    }

    //Driver information handling
    public void ClearDriversList()
    {
        driverInfoList.Clear();
    }


    public void AddDriverToList(int playerNumber, string name, int carUniqueID, bool isAI)
    {
        driverInfoList.Add(new DriverInfo(playerNumber, name, carUniqueID, isAI));
    }

    public void SetDriversLastRacePosition(int playerNumber, int position)
    {
        DriverInfo driverInfo = FindDriverInfo(playerNumber);
        driverInfo.lastRacePosition = position;
    }

    public void AddPointsToChampionship(int playerNumber, int points)
    {
        DriverInfo driverInfo = FindDriverInfo(playerNumber);

        driverInfo.championshipPoints += points;
    }

    DriverInfo FindDriverInfo(int playerNumber)
    {
        foreach (DriverInfo driverInfo in driverInfoList)
        {
            if (playerNumber == driverInfo.playerNumber)
                return driverInfo;
        }

        Debug.LogError($"FindDriverInfoBasedOnDriverNumber failed to find driver for player number {playerNumber}");

        return null;
    }

    public List<DriverInfo> GetDriverList()
    {
        return driverInfoList;
    }

    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");

        raceStartedTime = Time.time;

        ChangeGameState(GameStates.running);
    }
    public void OnRaceCompleted()
    {
        Debug.Log("OnRaceCompleted");

        raceCompletedTime = Time.time;
        //StartCoroutine("Fade");
        ChangeGameState(GameStates.raceOver);
        
    }
    
    /// <summary>
    /// /
    /// </summary>

/*
    Animator anim;

    public GameObject Scene3D;
    public GameObject Scene2D;
    //public Transform nextSpawnPosition;

    Player3D localplayer;

    void SwitchScene()
    {
        //SceneManager.LoadScene(nameOfNextScene);

        localplayer = Scene2D.GetComponent<PlayerHolder>().localplayer;
        //localplayer.transform.position = nextSpawnPosition.position;
        localplayer.EnableFeatures();
        localplayer.EnableRB();
        localplayer.Update2DStatus(false);

        Scene3D.SetActive(true);
        Scene2D.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player player = col.gameObject.GetComponent<Player>();
            //if (player.isLocalPlayer)
            //{
            //    anim = player.transform.Find("Canvas").GetComponent<Animator>();

            //    inprogress = true; //prevent executing twice
            //    StartCoroutine("Fade");
            //}
            StartCoroutine("Fade");
            //SwitchScene();
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(3);
        if (anim != null)
        {
            anim.Play("FadeOut");
        }
        yield return new WaitForSeconds(1);
        SwitchScene();
    }*/
    
    ///
    /// 
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }

}
