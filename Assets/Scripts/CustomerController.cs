using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private ServiceProcess ATM;
    [SerializeField] private CustomerQueue _targetQueue;

    private Transform _target;

    private bool _inQueue = false;

    public Action<GameObject> OnReachTarget;
    private bool _onTarget = false;

    public bool InService { get; set; }

    public enum CustomerState
    {
        None = -1,
        Arrived,
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
        Debug.Log("On Target: " + _onTarget);
        if (_onTarget) return;
        //Debug.Log("Not On Target");
        if (_target == null) return;
        //Debug.Log("Target Defined");

        //Debug.Log("Distance to target: " + Vector3.Distance(_target.position, transform.position));
        if (Vector3.Distance(_target.position, transform.position) <= _agent.stoppingDistance + 0.35f)
        {
            Debug.Log("Reaching Target");
            OnReachTarget?.Invoke(gameObject);
            _onTarget = true;
        }
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

    // States
    private void DoArrived()
    {
        UpdateTarget(_targetQueue.GetTarget());
        _targetQueue.OnTargetUpdate += UpdateTarget;
        _agent.isStopped = false;

        SetOnReachTarget(EnqueueSelf);
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
    public void SetOnReachTarget(Action<GameObject> DoOnReach)
    {
        _onTarget = false;
        OnReachTarget = DoOnReach;
        Debug.Log("SETTING ON_REACH");
        Debug.Log(OnReachTarget);
    }

    private void EnqueueSelf(GameObject self)
    {
        _inQueue = true;
        _targetQueue.Enqueue(self);
        _targetQueue.OnTargetUpdate -= UpdateTarget; // Unsubscribe from notifacations from new customers joining queue.
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

    internal void SetStopDistance(float value)
    {
        _agent.stoppingDistance = value;
    }
}
