using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnCars : MonoBehaviour
{
    int numberOfCarsSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        //Ensure that the spawn points are sorted by name
        spawnPoints = spawnPoints.ToList().OrderBy(s => s.name).ToArray();

        //Load the car data
        CarData[] carDatas = Resources.LoadAll<CarData>("CarData/");

        //Driver info
        List<DriverInfo> driverInfoList = new List<DriverInfo>(CarGameManager.instance.GetDriverList());

        //Sort the drivers based on last position
        driverInfoList = driverInfoList.OrderBy(s => s.lastRacePosition).ToList();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i].transform;

            if (driverInfoList.Count == 0)
                return;

            DriverInfo driverInfo = driverInfoList[0];

            int selectedCarID = driverInfo.carUniqueID;

            //Find the selected car
            foreach (CarData cardata in carDatas)
            {
                //We found the car data for the player
                if (cardata.CarUniqueID == selectedCarID)
                {
                    //Now spawn it on the spawnpoint
                    GameObject car = Instantiate(cardata.CarPrefab, spawnPoint.position, spawnPoint.rotation);

                    car.name = driverInfo.name;

                    car.GetComponent<CarInputHandler>().playerNumber = driverInfo.playerNumber;

                    if (driverInfo.isAI)
                    {
                        car.GetComponent<CarInputHandler>().enabled = false;
                        car.tag = "AI";
                    }
                    else
                    {
                        car.GetComponent<CarAIHandler>().enabled = false;
                        car.GetComponent<AStarLite>().enabled = false;
                        car.tag = "Player";
                    }

                    numberOfCarsSpawned++;

                    break;
                }
            }

            //Remove the spawned driver
            driverInfoList.Remove(driverInfo);
        }
    }

    public int GetNumberOfCarsSpawned()
    {
        return numberOfCarsSpawned;
    }

}
