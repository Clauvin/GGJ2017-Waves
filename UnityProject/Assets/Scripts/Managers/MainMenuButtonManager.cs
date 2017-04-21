﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonManager : MonoBehaviour {

	public GameObject creditsGO;


    public void CarregaMenuPrincipal()
    {
        Basics_2.LoadScene.LoadMainMenu();
    }

    public void CarregaGameplay()
    {
        Basics_2.LoadScene.LoadMainGameplay();
    }

    public void FechaJogo()
    {
        Basics_2.LoadScene.CloseGame();
    }

	public void showResponsibles()
	{
		creditsGO.SetActive (true);
	}

	public void hideResponsibles()
	{
		creditsGO.SetActive (false);
	}

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
