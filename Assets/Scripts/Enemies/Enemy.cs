﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pixelplacement;

/// <summary>
/// 
/// </summary>

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{

    #region Variable Declarations
    // Serialized Fields
    [Space]
    [SerializeField] protected EnemyType enemyType;
    [SerializeField] protected LaserType laserType;
    [SerializeField] protected Score score;

    [Header("References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject deathState;
    [SerializeField] protected LayerMask shieldLayer;
    [SerializeField] protected GameObject aimingLinePrefab;
    [SerializeField] protected StateMachine stateMachine;

    [Header("Audio")]
    [SerializeField] public AudioSource deathSoundSource;
    public AudioSource shotSoundSource;
    [SerializeField] protected bool playSoundOnHit;
    [ConditionalHide("playSoundOnHit", true, false)] [SerializeField] protected AudioSource hitSoundSource;
    // Private
    protected int currentHP;
    protected LineRenderer aimingLine;
    protected NavMeshAgent agent;

    // Public
    [HideInInspector] public SpawnManager spawnManager;
	#endregion
	
	
	
	#region Public Properties
	public EnemyType EnemyType { get { return enemyType; } }
	public LaserType LaserType { get { return laserType; } }
    public NavMeshAgent Agent { get { return agent; } }
    public Animator Animator { get { return animator; } }
    public LineRenderer AimingLine { get { return aimingLine; } }
    #endregion



    #region Unity Event Functions
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start () 
	{
        currentHP = enemyType.hp;
	}

    private void Update()
    {

    }
    #endregion



    #region Public Functions
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (!CheckDeath())
        {
            if (playSoundOnHit)
            {
                hitSoundSource.Play();
            }
        }
    }

    public void UpdateAimAssist(float lineLength, float lineDuration)
    {
        if (stateMachine.currentState.name == "Shoot" && aimingLine != null) Destroy(aimingLine.gameObject);

        if (!GameManager.Instance.ShieldController.ShieldUp || stateMachine.currentState.name != "ChargeLaser") return;

        Ray ray = new Ray(transform.position + transform.forward, transform.forward);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, 20f, shieldLayer);

        if (hitInfo.point != Vector3.zero) Debug.DrawLine(ray.origin, hitInfo.point, Color.green, lineLength);
        else Debug.DrawRay(ray.origin, ray.direction, Color.red, 3f);

        // Update LineRenderer if we hit the players shield
        if (hitInfo.point != Vector3.zero)
        {
            Vector3 reflection = Vector3.Reflect(ray.direction, hitInfo.normal);

            // Spawn LineRenderer if called for the first time
            if (aimingLine == null)
            {
                aimingLine = GameObject.Instantiate(aimingLinePrefab, hitInfo.point, Quaternion.identity, transform).GetComponent<LineRenderer>();
                aimingLine.GetComponent<AimingLine>().LifeTime = lineDuration;
                aimingLine.positionCount = 3;
            }

            aimingLine.SetPosition(0, transform.position);
            aimingLine.SetPosition(1, hitInfo.point);
            aimingLine.SetPosition(2, hitInfo.point + reflection * lineLength);
        }
        else if (hitInfo.point == Vector3.zero && aimingLine != null)
        {
            Destroy(aimingLine.gameObject);
        }
    }
    #endregion



    #region Private Functions
    private bool CheckDeath()
    {
        if (currentHP <= 0)
        {
            animator.SetBool("death", true);
            if(agent != null)
                agent.SetDestination(transform.position);
            transform.Find("States").GetComponent<StateMachine>().ChangeState(deathState);
            score.IncreaseScore(enemyType.scoreOnDeath);
            Invoke("KillObj", animator.GetComponent<EnemyAnimatorEventFunctions>().DissolveTime);
            return true;
        }
        return false;
    }

    private void KillObj()
    {
        spawnManager.UnregisterEnemy(gameObject);
        Destroy(gameObject);
    }
    #endregion



    #region Coroutines
    
    #endregion
}