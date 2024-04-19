using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator endScreen = null;

    static private bool end = false;

    void Start()
    {
        GameManager.end = false;
        this.endScreen.enabled = false;
    }

    void Update()
    {
        if (GameManager.end && !this.endScreen.enabled)
            this.endScreen.enabled = true;;
    }

    static public void GameOver() => GameManager.end = true;
}
