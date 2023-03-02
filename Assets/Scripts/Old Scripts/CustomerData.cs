
public struct CustomerData
{
    public readonly int id;
    public readonly float arrivalTime;
    public readonly float serviceTime;

    public CustomerData(int id, float arrivalTime, float serviceTime)
    {
        this.id = id;
        this.arrivalTime = arrivalTime;
        this.serviceTime = serviceTime;
    }
}

