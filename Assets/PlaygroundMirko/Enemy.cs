﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Enemy : MonoBehaviour 
{
	
	#region Variable Declarations
	// Serialized Fields
	[SerializeField] GameObject LaserBeamPrefab;
    [SerializeField] float shootInterval = 1f;
    [SerializeField] float laserSpeed = 10f;

    // Private
    float timer;
	#endregion
	
	
	
	#region Public Properties
	
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start () 
	{
        
	}

    private void Update()
    {
        transform.LookAt(GameManager.Instance.player.transform);

        if (timer >= shootInterval)
        {
            ShootLaser();
            timer = 0f;
        }

        timer += Time.deltaTime;
    }
    #endregion



    #region Public Functions
    public void ShootLaser()
    {
        GameObject laser = GameObject.Instantiate(LaserBeamPrefab, transform.position + transform.forward + Vector3.up * 0.5f, Quaternion.identity);
        laser.GetComponent<Rigidbody>().velocity = transform.forward * laserSpeed;
    }
    #endregion



    #region Private Functions

    #endregion



    #region Coroutines

    #endregion
}