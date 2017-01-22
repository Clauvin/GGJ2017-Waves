﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToTheObjective : MonoBehaviour {

    public Vector3 objetivo_inicial;
    public Vector3 objetivo_final;
    public float tempo;
    private float tempo_passado;
    private float ultimo_tempo;

	// Use this for initialization
	void Start () {
        objetivo_inicial = new Vector2(transform.position.x, transform.position.y);
        tempo_passado = 0;
        ultimo_tempo = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        tempo_passado += Time.time - ultimo_tempo;
        ultimo_tempo = Time.time;
        transform.position = Vector3.Lerp(objetivo_inicial, objetivo_final, tempo_passado / tempo);
	}
}
