﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmigrantWaveLauncher : MonoBehaviour {

    /// <summary>
    /// Instancia um Ícone de Refugiados, com:
    ///     a) Número de Imigrantes; (semi randomizado com base em valores)
    ///     b) Ponto de instância
    ///     c) Ponto de chegada
    ///     d) Velocidade para chegar
    /// </summary>

    public int nextWaveId = 0;

    public GameObject refugees_exit_1, refugees_exit_2;
    public GameObject refugees_entrance_1, refugees_entrance_2;
    public float present_time = 0.0f;
    public float last_time;
    public float time_for_next = 0.0f;
    public int def_quant_of_refugees = 50;
    public float def_time_in_seconds = 30.0f;

    void InstantiateNewRefugeeWave(int refugee_quantity, GameObject exit, GameObject entrance, float time_in_seconds,
            float scale = 1.0f)
    {
        GameObject icone = Instantiate<GameObject>(Resources.Load<GameObject>("Onda De Imigrantes"));
        icone.transform.position = new Vector3(exit.transform.position.x,
                                               exit.transform.position.y,
                                               exit.transform.position.z);
        icone.GetComponent<ImmigrantWave>().numberOfImmigrants = refugee_quantity;
        icone.GetComponent<MovingToTheObjective>().final_objective = entrance.transform.position;
        icone.GetComponent<MovingToTheObjective>().time_to_reach_objective = time_in_seconds;
        icone.name = "Refugee Wave - ID " + nextWaveId;
        if (scale > 1.0f)
        {
            icone.transform.localScale = new Vector3(icone.transform.localScale.x * scale,
                                                     icone.transform.localScale.y * scale,
                                                     1);
        }
        nextWaveId++;
    }

    void RandomInstantaneousWaveInstance()
    {
        int refugee_quantity;
        //Ano 1: de 50 a 150 refugiados por onda.
        //Ano 2: de 150 a 200 refugiados por onda.
        GameObject exit, entrance;
        float time_in_seconds;
        //Ano 1: de 30 a 60 segundos.
        //Ano 2: de 15 a 30 segundos.
        float scale = 1.0f;

        //Superonda: 750 a 1000 refugiados, de 150 a 300 segundos. Sprite 3x maior.

        if (TimeManager.instance.year == 1)
        {
            refugee_quantity = (int)(Random.Range(1.0f, 2.0f) * def_quant_of_refugees);
            time_in_seconds = Random.Range(1.0f, 2.0f) * def_time_in_seconds;
        }
        else if (Random.Range(0.0f, 1.0f) < 0.05f)
        {
            refugee_quantity = (int)(Random.Range(15.0f, 20.0f) * def_quant_of_refugees);
            time_in_seconds =  Random.Range(5.0f, 10.0f) * def_time_in_seconds;
            scale *= 3;
        }
        else
        {
            refugee_quantity = (int)(Random.Range(3.0f, 4.0f) * def_quant_of_refugees);
            time_in_seconds = Random.Range(0.5f, 1.0f) * def_time_in_seconds;
        }

        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            exit = refugees_exit_1; entrance = refugees_entrance_1;
        } else
        {
            exit = refugees_exit_2; entrance = refugees_entrance_2;
        }

        InstantiateNewRefugeeWave(refugee_quantity, exit, entrance, time_in_seconds, scale);
    }

    // Use this for initialization
    void Start () {
        time_for_next = Random.Range(30.0f, 60.0f);
        last_time = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (!TimeManager.instance.gamePaused)
        {
            present_time += Time.time - last_time;
            last_time = Time.time;

            if (present_time >= time_for_next)
            {
                RandomInstantaneousWaveInstance();
                present_time = 0.0f;
                if (TimeManager.instance.year == 1)
                {
                    time_for_next = Random.Range(30.0f, 60.0f);
                }
                else
                {
                    time_for_next = Random.Range(15.0f, 30.0f);
                }
            }
        }
	}
}
