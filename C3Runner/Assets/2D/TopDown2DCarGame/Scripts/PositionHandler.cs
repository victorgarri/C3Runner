using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionHandler : MonoBehaviour
{
    //Other components
    LeaderboardUIHandler leaderboardUIHandler;

    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();
    private void Awake()
    {
      
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get all Car lap counters in the scene. 
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        //Store the lap counters in a list
        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();

        //Hook up the pased checkpoint event
        foreach (CarLapCounter lapCounters in carLapCounters)
            lapCounters.OnPassCheckpoint += OnPassCheckpoint;

        //Get the leaderboard UI handler
        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();

        //Ask the leaderboard handler to update the list
        if(leaderboardUIHandler != null)
            leaderboardUIHandler.UpdateList(carLapCounters);
    }

    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        //Sort the cars positon first based on how many checkpoints they have passed, more is always better. Then sort on time where shorter time is better
        carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckPoint()).ToList();

        //Get the cars position
        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;

        //Tell the lap counter which position the car has
        carLapCounter.SetCarPosition(carPosition);

        if (carLapCounter.IsRaceCompleted())
        {
            //Set players last position
            int playerNumber = carLapCounter.GetComponent<CarInputHandler>().playerNumber;
            CarGameManager.instance.SetDriversLastRacePosition(playerNumber, carPosition);

            //Add points to championship
            int championshipPointAwarded = FindObjectOfType<SpawnCars>().GetNumberOfCarsSpawned() - carPosition;
            CarGameManager.instance.AddPointsToChampionship(playerNumber, championshipPointAwarded);
        }

        //Ask the leaderboard handler to update the list
        if (leaderboardUIHandler != null)
            leaderboardUIHandler.UpdateList(carLapCounters);
    }
}
