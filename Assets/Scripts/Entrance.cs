using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entrance : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private FileReader fileReader;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private List<Transform> spawnlocations = new List<Transform>();

    public bool generateArrivals = true;

    private float nextSpawnTime;

    private ActionTimer customerArrivalTimer;

    private Queue<CustomerData> customers;

    /*    public float arrivalRateAsCsutomerPerHour = 20; // customers/hour
        public float interArrivalTimeInHours; // = 1.0 / arrivalRateAsCustomerssPerHour;
        private float interArrivalTimeInMinutes;
        private float interArrivalTimeInSeconds;
        public float minInterArrivalTimeInSeconds = 3;
        public float maxInterArrivalTimeInSeconds = 60;*/

    void Start()
    {
        float customerTimeArrival = Random.Range(1, 4);
        customers = fileReader.GenerateCustomers();
        customerArrivalTimer = new ActionTimer();
        InvokeRepeating("SpawnCustomer", 0, customerTimeArrival);
    }

    private void SpawnCustomer()
    {

            CustomerData customer;
            if (customers.TryDequeue(out customer))
            {
                Debug.Log("Spawning Customer");
                Instantiate(customerPrefab).GetComponent<Customer>().InitalizeCustomer(customer);
                customerArrivalTimer.ClearCallbacks();
                customerArrivalTimer.StartTimer(customer.arrivalTime, SpawnCustomer, LogTime);
            }
/*        float timeToNextArrivalInSec = interArrivalTimeInSeconds;
        switch (arrivalIntervalTimeStrategy)
        {
            case ArrivalIntervalTimeStrategy.ConstantIntervalTime:
                timeToNextArrivalInSec = interArrivalTimeInSeconds;
                break;
            case ArrivalIntervalTimeStrategy.UniformIntervalTime:
                timeToNextArrivalInSec = Random.Range(minInterArrivalTimeInSeconds, maxInterArrivalTimeInSeconds);
                break;
            case ArrivalIntervalTimeStrategy.ExponentialIntervalTime:
                float U = Random.value;
                float Lambda = 1 / arrivalRateAsCustomersPerHour;
                timeToNextArrivalInSec = Utilities.GetExp(U, Lambda);
                break;
            case ArrivalIntervalTimeStrategy.ObservedIntervalTime:
                timeToNextArrivalInSec = interArrivalTimeInSeconds;
                break;
            default:
                print("No acceptable arrivalIntervalTimeStrategy:" + arrivalIntervalTimeStrategy);
                break;

        }*/
    }

    private void LogTime(float time)
    {
        Debug.Log(time);
    }

    private void Update()
    {
        customerArrivalTimer.Tick(Time.deltaTime);
    }
}
