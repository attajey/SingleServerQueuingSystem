using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Transform target;
    [SerializeField] private Transform exit;

    [SerializeField] private GameObject atmWindow;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ATMWindow")
        {
            //ChangeState(CustomerState.Servicing);
        }
        else if (other.gameObject.tag == "Exit")
        {
            Destroy(this.gameObject);
            //this.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Customer")
        {
            //agent.isStopped = true;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "ATMWindow")
    //    {
    //        ChangeState(CustomerState.Servicing);
    //    }
    //    else if (collision.gameObject.tag == "Exit")
    //    {
    //        Destroy(this.gameObject);
    //        //this.gameObject.SetActive(false);
    //    }
    //}
    void Start()
    {
        atmWindow = GameObject.FindGameObjectWithTag("ATMWindow");
        exit = GameObject.FindGameObjectWithTag("Exit").transform;

        agent = GetComponent<NavMeshAgent>();

        target = atmWindow.transform;

        customerState = CustomerState.Arrived;
        //FSMCustomer();
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
                SetAgentMovement();
                break;
            case CustomerState.Waiting:
                DoWaiting();
                SetAgentMovement();
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
        agent.SetDestination(target.position);
        
    }

    private void DoWaiting()
    {
        agent.isStopped = true;
    }

    private void DoServing()
    {
        agent.isStopped = true;
    }

    private void DoServiced()
    {
        agent.SetDestination(exit.position);
        agent.isStopped = false;
    }

    private void SetAgentMovement()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 3f)) // Look 5m in front
        {
            if (hit.transform.CompareTag("Customer"))
            {
                ChangeState(CustomerState.Waiting);
            }
            else
            {
                ChangeState(CustomerState.Arrived);
            }
        }
        else
        {
            ChangeState(CustomerState.Arrived);
        }
    }

    public void ChangeState(CustomerState newCarState)
    {
        this.customerState = newCarState;
        //FSMCustomer();
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
}
