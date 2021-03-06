﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToTheObjective : MonoBehaviour {

    #region Public Variables
    public Vector3 initial_position;
    public Vector3 final_objective;
    public float time_to_reach_objective;
    public float passed_time;
    #endregion

    public float time_counted;

    public float GetTimeCounted() { return time_counted; }

    public void SetTimeCounted(float tc) { time_counted = tc; }

	// Use this for initialization
	void Start () {
        //to_the_objective_save_package = new ToTheObjectiveSavePackage();
        initial_position = new Vector2(transform.position.x, transform.position.y);
        passed_time = 0;
        time_counted = TimerManager.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (!TimeManager.instance.gamePaused) { 

            if (TimerManager.time - time_counted >= 0.0f) passed_time += TimerManager.time - time_counted;
            time_counted = TimerManager.time;
            transform.position = Vector3.Lerp(initial_position, final_objective, passed_time / time_to_reach_objective);

            if (passed_time / time_to_reach_objective >= 1.0f)
            {
                if (this.gameObject.tag == "Wave")ImmigrantManager.instance.WaveReceived(this.gameObject);
                Destroy(this.gameObject);
            }

        }
    }

    
}
