using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{   
    public CustomerData customerData;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float QUEUE_SEPERATION_DISTANCE;

    private Transform _target;
    private CustomerQueue _targetQueue;

    private float queueSelfDistance = 0.15f;

    public enum CustomerState
    {
        None = -1,
        Arrived,
        MoveToQueue,
        Waiting,
        Servicing,
        Serviced
    }
    public CustomerState customerState = CustomerState.None;
    void Start()
    {
        customerState = CustomerState.Arrived;
        FSMCustomer();
    }

    public void InitCustomer(CustomerQueue targetQueue)
    {
        _targetQueue = targetQueue;
    }

    public void Update()
    {
        if (customerState == CustomerState.Waiting)
        {
            DoWaiting();
        }
        else if (customerState == CustomerState.MoveToQueue)
        {
            DoMoveToQueue();
        }       
    }
    private void FSMCustomer()
    {
        switch (customerState)
        {
            case CustomerState.Arrived:
                DoArrived();
                break;
            case CustomerState.MoveToQueue:
                DoMoveToQueue();
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

    // States
    private void DoArrived()
    {
        UpdateTarget(_targetQueue.GetTarget());
        _targetQueue.OnTargetUpdate += UpdateTarget;
        _agent.isStopped = false;

        ChangeState(CustomerState.MoveToQueue);
        queueSelfDistance = _agent.radius * 2 + QUEUE_SEPERATION_DISTANCE;
    }
    private void DoMoveToQueue()
    {
        _agent.SetDestination(_target.position);

        Vector3 targetZeroHeight = new Vector3(_target.position.x, 0f, _target.position.z);
        Vector3 selfZeroHeight = new Vector3(transform.position.x, 0f, transform.position.z);

        //Debug.Log("DoMoveToQueue");
        //Debug.Log("Condition to Queue Self: " + Vector3.Distance(targetZeroHeight, selfZeroHeight) + " <= " + queueSelfDistance + " --> "
        //    + (Vector3.Distance(targetZeroHeight, selfZeroHeight) <= queueSelfDistance));

        if (Vector3.Distance(targetZeroHeight, selfZeroHeight) <= queueSelfDistance)
        {
            EnqueueSelf(gameObject);
            ChangeState(CustomerState.Waiting);
            _agent.stoppingDistance = _agent.radius * 2 + QUEUE_SEPERATION_DISTANCE;
        }
    }
    private void DoWaiting()
    {
        _agent.SetDestination(_target.position);
    }
    private void DoServing()
    {
        _agent.isStopped = true;
    }
    private void DoServiced()
    {
        _agent.SetDestination(_target.position);
        _agent.isStopped = false;
    }
    public void ChangeState(CustomerState newCustomerState)
    {
        customerState = newCustomerState;
        FSMCustomer();
    }
    public void ExitService(Transform target)
    {
        Debug.Log("Exit Service");
        UpdateTarget(target.gameObject);
        ChangeState(CustomerState.Serviced);
    }
    

    // Helper Functions
    private void EnqueueSelf(GameObject self)
    {
        /* Unsubscribe from notifacations from new customers joining queue before this customer joins queue,
            That way this customers target does not become itself
         */
        _targetQueue.OnTargetUpdate -= UpdateTarget;
        _targetQueue.Enqueue(self);
        ChangeState(CustomerState.Waiting);
    }

    public void UpdateTarget(GameObject newTarget)
    {
        _target = newTarget.transform;
        _agent.SetDestination(_target.position);
    }

    private void OnDrawGizmos()
    {
        if (_target)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, _target.position);
        }
    }

    public void SetStopDistance(float value)
    {
        _agent.stoppingDistance = value;
    }
}
