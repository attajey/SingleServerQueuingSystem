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

    //New as of Feb.23rd
    //Simple generation distribution - Uniform(min,max)
    //
    public float minInterServiceTimeInSeconds = 3;
    public float maxInterServiceTimeInSeconds = 60;
    //

    //New as Feb.25th
    //CarController carController;
    QueueManager queueManager; //=new QueueManager();

    public enum ServiceIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime
    }

    public ServiceIntervalTimeStrategy serviceIntervalTimeStrategy = ServiceIntervalTimeStrategy.UniformIntervalTime;

    // Start is called before the first frame update
    void Start()
    {
        interServiceTimeInHours = 1.0f / serviceRateAsCustomersPerHour;
        interServiceTimeInMinutes = interServiceTimeInHours * 60;
        interServiceTimeInSeconds = interServiceTimeInMinutes * 60;
        //queueManager = this.GetComponent<QueueManager>();
        //queueManager = new QueueManager();
        //StartCoroutine(GenerateServices());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            customerInService = other.gameObject;
            customerInService.GetComponent<CustomerController>().SetInService(true);

            //if (queueManager.Count() == 0)
            //{
            //    queueManager.Add(carInService);
            //}

            generateServices = true;
            //carController = carInService.GetComponent<CarController>();
            StartCoroutine(GenerateServices());
        }
    }
    IEnumerator GenerateServices()
    {
        while (generateServices)
        {
            //Instantiate(carPrefab, carSpawnPlace.position, Quaternion.identity);
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

            //New as of Feb.23rd
            //float timeToNextServiceInSec = Random.Range(minInterServiceTimeInSeconds,maxInterServiceTimeInSeconds);
            generateServices = false;
            yield return new WaitForSeconds(timeToNextServiceInSec);

            //yield return new WaitForSeconds(interServiceTimeInSeconds);

        }
        customerInService.GetComponent<CustomerController>().ExitService(customerExitPlace);

    }
    private void OnDrawGizmos()
    {
        //BoxCollidercarInService.GetComponent<BoxCollider>
        if (customerInService)
        {
            Renderer r = customerInService.GetComponent<Renderer>();
            r.material.color = Color.green;

        }


    }
}
