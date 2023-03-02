using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivalProcess : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform customerSpawnPlace;


    // For Expo Dist. we need the Lambda (the avg arrival rate, cars/hour)

    // Observed Data
    [SerializeField] private float arrivalRateInCarsPerHour = 20; // avg rate

    // Calculated Data
    private float interArrivalTimeInMin; // avg #mins between car arrivals

    // Control Simulation Data
    private bool isSimulationRunning = false;


    void Start()
    {
        isSimulationRunning = true;
        interArrivalTimeInMin = 1f / arrivalRateInCarsPerHour * 60f;
        StartCoroutine(GenerateArrivals());
    }

    IEnumerator GenerateArrivals()
    {
        while (isSimulationRunning)
        {
            GameObject carGameObject = Instantiate(customerPrefab, customerSpawnPlace.position, Quaternion.identity);
            float nextArrivalTimeInMin = Utilities.GenerateExponentiallyDistributedValue(interArrivalTimeInMin);
            yield return new WaitForSeconds(nextArrivalTimeInMin * 60);
        }
    }
}
