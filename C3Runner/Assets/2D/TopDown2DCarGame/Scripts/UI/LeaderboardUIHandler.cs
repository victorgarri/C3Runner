using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;

    SetLeaderboardItemInfo[] setLeaderboardItemInfo;

    bool isInitilized = false;

    //Oher components
    Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        //Hook up events
        CarGameManager.instance.OnGameStateChanged += OnGameStateChanged;
    }


    // Start is called before the first frame update
    void Start()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        //Get all Car lap counters in the scene. 
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        //Allocate the array
        setLeaderboardItemInfo = new SetLeaderboardItemInfo[carLapCounterArray.Length];

        //Create the leaderboard items
        for (int i = 0; i < carLapCounterArray.Length; i++)
        {
            //Set the position
            GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);

            setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<SetLeaderboardItemInfo>();

            setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
        }

        Canvas.ForceUpdateCanvases();

        isInitilized = true;
    }

    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        if (!isInitilized)
            return;

        //Create the leaderboard items
        for (int i = 0; i < lapCounters.Count; i++)
        {
            setLeaderboardItemInfo[i].SetDriverNameText(lapCounters[i].gameObject.name);
        }
    }

    //Events 
    void OnGameStateChanged(CarGameManager carGameManager)
    {
        if (CarGameManager.instance.GetGameState() == GameStates.raceOver)
        {
            canvas.enabled = true;
        }
    }

    void OnDestroy()
    {
        //Unhook events
        CarGameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
