using UnityEngine;

public class Customer : MonoBehaviour
{
    // Parameters
    private float arrivalTime;
    private float serviceTime;
    private int id;

    public Customer CreateCustomer(int id, float arrivalTime, float serviceTime)
    {
        this.id = id;
        this.arrivalTime = arrivalTime;
        this.serviceTime = serviceTime;
        return this;
    }

    public float GetArrivalTime()
    {
        return arrivalTime;
    }

    public float GetServiceTime()
    {
        return serviceTime;
    }

    public int GetId()
    {
        return id;
    }




}
