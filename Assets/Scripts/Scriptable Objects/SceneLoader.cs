﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
/// 
[CreateAssetMenu(menuName = "Scriptable Objects/SceneLoader")]
public class SceneLoader : ScriptableObject 
{

    public void ChangeToGame()
    {
        SceneManager.LoadScene(Constants.SCENE_LOGIC, LoadSceneMode.Single);
        SceneManager.LoadScene(Constants.SCENE_GUI, LoadSceneMode.Additive);
        SceneManager.LoadScene(Constants.SCENE_ENVIRONMENT, LoadSceneMode.Additive);
    }

    public void ChangeToMainMenu()
    {
        SceneManager.LoadScene(Constants.SCENE_MENU, LoadSceneMode.Single);
    }

    public void ChangeToCredits()
    {
        SceneManager.LoadScene(Constants.SCENE_CREDITS, LoadSceneMode.Single);
    }
}