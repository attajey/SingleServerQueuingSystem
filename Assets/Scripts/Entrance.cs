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
    float timer;
    void Start()
    {
        customers = fileReader.GenerateCustomers();
        customerArrivalTimer = new ActionTimer();
        InvokeRepeating("SpawnCustomer", 0, Random.Range(1, 4));
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
