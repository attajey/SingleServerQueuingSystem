using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Transform target;
    [SerializeField] private Transform exit;

    [SerializeField] private Transform atm;

    //[SerializeField] private QueueManager queueManager;

    [SerializeField] private CustomerState customerState = CustomerState.None;

    public bool InService { get; set; }

    public enum CustomerState
    {
        None = -1,
        Arrived,
        Waiting,
        Servicing,
        Serviced
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        atm = GameObject.FindGameObjectWithTag("ATM").transform;
        exit = GameObject.FindGameObjectWithTag("Exit").transform;

        agent = GetComponent<NavMeshAgent>();

        target = atm.transform;

        customerState = CustomerState.Arrived;
    }

    void Update()
    {
        FSMCustomer();
    }

    private void FSMCustomer()
    {
        switch (customerState)
        {
            case CustomerState.Arrived:
                DoArrived();
                break;
            case CustomerState.Waiting:
                DoWaiting();
                break;
            case CustomerState.Servicing:
                DoServing();
                break;
            case CustomerState.Serviced:
                DoServiced();
                break;
            default:
                Debug.LogError("State Not Found!\n");
                break;
        }
    }

    private void DoArrived()
    {
        target = GetTarget();
        if (target != null)
        {
            agent.SetDestination(target.position);

        }
    }

    private void DoWaiting()
    {

    }

    private void DoServing()
    {
    }

    private void DoServiced()
    {
        agent.SetDestination(exit.position);
    }

    public void ChangeState(CustomerState newCarState)
    {
        this.customerState = newCarState;
    }

    public void ExitService(Transform target)
    {
        ChangeState(CustomerState.Serviced);
    }

    public void SetInService(bool value)
    {
        if (value)
        {
            ChangeState(CustomerState.Servicing);
        }
    }
    private Transform GetTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            return hit.transform;
        }
        else
        {
            return atm;
        }
    }
}
