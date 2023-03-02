using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    // New Stuff
    private Transform _target;
    
    // Old Stuff
    public NavMeshAgent agent;
    public Transform target = null;
    public Transform targetCustomer;
    public Transform exit = null;

    public bool InService { get; set; }
    public GameObject AtmWindow;

    //public Transform atm;

    public enum CustomerState
    {
        None = -1,
        Arrived,
        Waiting,
        Servicing,
        Serviced
    }
    public CustomerState customerState = CustomerState.None;
    void Awake()
    {
        AtmWindow = GameObject.FindGameObjectWithTag("ATMWindow");
        target = AtmWindow.transform;
        exit = GameObject.FindGameObjectWithTag("Exit").transform;
        agent = GetComponent<NavMeshAgent>();       
    }

    public void InitCustomer(Transform firstTarget)
    {
        Debug.Log("Initing Customer");
        UpdateTarget(firstTarget);
        customerState = CustomerState.Arrived;
        Debug.Log(firstTarget);
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
                Debug.Log("Entered Servicing");
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

    private void Update()
    {
        if(Vector3.Distance(_target.position, transform.position) < 0.1f)
        {
            UpdateTarget(_target);
        }
    }

    private void DoArrived()
    {
        agent.SetDestination(_target.position);
        agent.isStopped = false;
        Debug.Log(agent.destination);
    }
    private void DoWaiting()
    {
        ;
    }
    private void DoServing()
    {
        agent.isStopped = true;
    }
    private void DoServiced()
    {
        agent.SetDestination(exit.position);
        agent.isStopped = false;
        //throw new NotImplementedException();
    }
    public void ChangeState(CustomerState newCarState)
    {
        this.customerState = newCarState;
        FSMCustomer();
    }
    public void UpdateTarget(Transform newTarget)
    {
        _target = newTarget;
    }
    public void ExitService(Transform target)
    {
        ChangeState(CustomerState.Serviced);
    }

    private void OnTriggerEnter(Collider other)
    {       
        if (other.gameObject.tag == "ATMWindow")
        {
            Debug.Log("Entered trigger atm");
            ChangeState(CustomerState.Servicing);
        }
        else if (other.gameObject.tag == "Exit")
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(this.transform.position, target.transform.position);
        if (_target)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, _target.position);

        }
        if (exit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, exit.transform.position);

        }
    }

}
