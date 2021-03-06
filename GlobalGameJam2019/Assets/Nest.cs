﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    [SerializeField] NewHighscore hS;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            LevelManager.instance.ShowLevelCompletePopup(hS.GetScore(), hS.Finished());
            hS.Finished();
        }
    }
}

