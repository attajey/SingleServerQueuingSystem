using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceProcess : MonoBehaviour
{

    public GameObject customerInService;
    public Transform customerExitPlace;

    public float serviceRateAsCustomersPerHour = 25; // customer/hour
    public float interServiceTimeInHours; // = 1.0 / ServiceRateAscustomersPerHour;
    private float interServiceTimeInMinutes;
    private float interServiceTimeInSeconds;

    //public float ServiceRateAsCarsPerHour = 20; // car/hour
    public bool generateServices = false;

    //Simple generation distribution - Uniform(min,max)
    public float minInterServiceTimeInSeconds = 3;
    public float maxInterServiceTimeInSeconds = 60;
    
    //CarController carController;
    QueueManager queueManager;

    public enum ServiceIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime
    }

    public ServiceIntervalTimeStrategy serviceIntervalTimeStrategy = ServiceIntervalTimeStrategy.UniformIntervalTime;

    // Start is called before the first frame update
    void Awake()
    {
        interServiceTimeInHours = 1.0f / serviceRateAsCustomersPerHour;
        interServiceTimeInMinutes = interServiceTimeInHours * 60;
        interServiceTimeInSeconds = interServiceTimeInMinutes * 60;

        queueManager = GetComponent<QueueManager>();
    }

    public void AddCustomer(GameObject newCustomer)
    {
        GameObject lastCustomer = queueManager.Last();
        CustomerController newCustomerController = newCustomer.GetComponent<CustomerController>();

        // If the Queue is empty
        if (lastCustomer == null)
        {
            newCustomerController.InitCustomer(transform);
        }
        else
        {
            newCustomerController.InitCustomer(lastCustomer.transform);            
        }

        queueManager.Add(newCustomer);    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            customerInService = other.gameObject;
            generateServices = true;
            StartCoroutine(GenerateServices());
        }
    }
    IEnumerator GenerateServices()
    {
        while (generateServices)
        {
            float timeToNextServiceInSec = interServiceTimeInSeconds;
            switch (serviceIntervalTimeStrategy)
            {
                case ServiceIntervalTimeStrategy.ConstantIntervalTime:
                    timeToNextServiceInSec = interServiceTimeInSeconds;
                    break;
                case ServiceIntervalTimeStrategy.UniformIntervalTime:
                    timeToNextServiceInSec = Random.Range(minInterServiceTimeInSeconds, maxInterServiceTimeInSeconds);
                    break;
                case ServiceIntervalTimeStrategy.ExponentialIntervalTime:
                    float U = Random.value;
                    float Lambda = 1 / serviceRateAsCustomersPerHour;
                    timeToNextServiceInSec = Utilities.GenerateExponentiallyDistributedValue(Lambda);
                    break;
                case ServiceIntervalTimeStrategy.ObservedIntervalTime:
                    timeToNextServiceInSec = interServiceTimeInSeconds;
                    break;
                default:
                    print("No acceptable ServiceIntervalTimeStrategy:" + serviceIntervalTimeStrategy);
                    break;

            }

            generateServices = false;
            yield return new WaitForSeconds(timeToNextServiceInSec);

        }
        customerInService.GetComponent<CustomerController>().ExitService(customerExitPlace);

    }    
}
