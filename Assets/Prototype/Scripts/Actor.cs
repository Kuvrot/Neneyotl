﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Damageable))]
public class Actor : MonoBehaviour
{
    NavMeshAgent agent;
    public bool isBuilder = true;
    [HideInInspector] public Damageable damageable;
    [HideInInspector] public Damageable damageableTarget;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AnimationEventListener animationEvent;
    public Coroutine currentTask;
    [HideInInspector] public ActorVisualHandler visualHandler;

    public bool isHover = false;
    bool isResource;


    //Stats  HP villager = 25 , Eagle warrior = 50 Jaguar Warrior = 65, Golem = 180
    //Stats Damage Villager = 0.350, Eagle warrior = 7, Jaguar warrior = 10 , Golem = 4.5
    //Stats speed Eagle warrior is the fastest like chavalry, Jaguar Warrior is slower than eagle warrior, Golem is very very slow.
    [Header("stats")]
    public float HP = 25;
    public float Damage = 0.350f;
    public float Speed = 3.5f;


    //Weakness
    // Eagle warrior strong against creatures and monsters, weak against distance enemies
    // Jaguar Warrior strong against distance enemies, weak with creatures
    //Golem strong to buildings, is weaknest is that walks very slow

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        animationEvent = GetComponentInChildren<AnimationEventListener>();
        visualHandler = GetComponent<ActorVisualHandler>();
        animationEvent.attackEvent.AddListener(Attack);
        isResource = GetComponent<Resource>() ? true : false;
        
    }

    private void Start()
    {
        ActorManager.instance.allActors.Add(this);
    }
    public void Update()
    {
        animator.SetFloat("Speed", Mathf.Clamp(agent.velocity.magnitude, 0, 1));
    }

    public void SetDestination(Vector3 destination)
    {
        agent.destination = destination;
    }
    public WaitUntil WaitForNavMesh()
    {
        return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    }
    void Attack()
    {
        if (damageableTarget)
        {

            if (isBuilder)
            {
                damageableTarget.Hit(0.350f);
            }
            else
            {
                damageableTarget.Hit(0.350f);
            }

        }
            
    }
    public void AttackTarget(Damageable target)
    {
        //StopTask();
        damageableTarget = target;
        currentTask = StartCoroutine(StartAttack());

        IEnumerator StartAttack()
        {
            while (damageableTarget)
            {
                SetDestination(damageableTarget.transform.position);
                yield return WaitForNavMesh();
                while (damageableTarget && Vector3.Distance(damageableTarget.transform.position, transform.position) < 4f)
                {
                    if (damageableTarget)
                        animator.SetTrigger("Attack");
                    else
                    {
                        StopTask();
                        break;
                    }
                    yield return new WaitForSeconds(1f);
                    
                }

            }

            currentTask = null;
        }
    }
    public virtual void StopTask()
    {

        animator.SetTrigger("Respawn");
        damageableTarget = null;
        GetComponent<Builder>().currentBuilding = null;
        if (currentTask != null)
        {
            StopCoroutine(currentTask);
            
        }
    }

    private void OnMouseEnter()
    {
        isHover = true;
    }
    private void OnMouseExit()
    {
        isHover = false;
    }

}
