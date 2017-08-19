﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmigrantManager : MonoBehaviour {

    [System.Serializable]
    public class ImmigrantSavePackage
    {
        public int _legalImmigrantsNumber;
        public int _illegalImmigrantsNumber;
        public int _naturalizedImmigrantsNumber;
    }

    /// <summary>
    /// Set this to public for debugging in the editor
    /// </summary>
    private ImmigrantSavePackage immigrant_save_package;

    #region Public Variables
    public static ImmigrantManager instance;

	public List<ImmigrantWave> legalWaves; //holds the waves of legal immigrants
	public List<ImmigrantWave> illegalWaves;

    public ImmigrantSavePackage GetImmigrantSavePackage()
    {
        return immigrant_save_package;
    }

    public void SetImmigrantSavePackage(ImmigrantSavePackage save_package)
    {
        immigrant_save_package = save_package;
    }

	public int numberOfLegalImmigrants {
        get { return immigrant_save_package._legalImmigrantsNumber; }
        set { immigrant_save_package._legalImmigrantsNumber = value; }
    }

	public int numberOfIllegalImmigrants {
        get { return immigrant_save_package._illegalImmigrantsNumber; }
        set { immigrant_save_package._illegalImmigrantsNumber = value; }
    }

    //how many immigrants you have helped get citizenship
    public int numberOfNaturalizedImmigrants {
        get { return immigrant_save_package._naturalizedImmigrantsNumber; }
        set { immigrant_save_package._naturalizedImmigrantsNumber = value; }
    }
    #endregion

    #region Wave Functions
    public void WaveReceived(GameObject wave)
    {

        //Receive immigrant waves
        ImmigrantWave immigrants = wave.GetComponent<ImmigrantWave>();

        //How many immigrant do we have?
        int immigrants_quantity = immigrants.numberOfImmigrants;

        //Do we have homes AND resources for the homes?
        if ((ResourceManager.instance.numberOfAvailableHouses > 0) &&
            (ResourceManager.instance.socialResources > 0))
        {
            //Do we have ENOUGH homes?
            if (ResourceManager.instance.numberOfAvailableHouses >= immigrants_quantity)
            {
                //If yes, solved.
                ResourceManager.instance.numberOfAvailableHouses -= immigrants_quantity;
                legalWaveArrived(new ImmigrantWave(immigrants_quantity, TimerManager.time, true, 4));
                immigrants_quantity = 0;
            }
            else
            {
                //If not, go ahead.
                legalWaveArrived(new ImmigrantWave(ResourceManager.instance.numberOfAvailableHouses, TimerManager.time, true, 4));
                immigrants_quantity -= ResourceManager.instance.numberOfAvailableHouses;
                ResourceManager.instance.numberOfAvailableHouses = 0;
            }
        }

        //We took care of everybody?
        if (immigrants_quantity > 0)
        {
            //Time to block people entering.
            
            //Do we have enough resources?
            if (ResourceManager.instance.borderResources > 0)
            {
                //Do we have enough soldiers?
                if (ResourceManager.instance.numberOfAvailableBorderOfficers >= immigrants_quantity)
                {
                    //If yes, ok.
                    //ResourceManager.instance.numberOfAvailableBorderOfficers -= immigrants_quantity;
                    immigrants_quantity = 0;
                }
                else
                {
                    //If not...
                    immigrants_quantity -= ResourceManager.instance.numberOfAvailableBorderOfficers;
                    //ResourceManager.instance.numberOfAvailableBorderOfficers = 0;
                }
            }
        }

        //Do we have illegals?
        if (immigrants_quantity > 0)
        {
            illegalWaveArrived(new ImmigrantWave(immigrants_quantity, TimerManager.time, false, 4));
        }

    }

	public void illegalWaveArrived(ImmigrantWave illegalW)
	{
		//illegal wave arrived in country
		illegalWaves.Add(illegalW);
		//add that to total illegals
		numberOfIllegalImmigrants+= illegalW.numberOfImmigrants;
	}

	public void legalWaveArrived (ImmigrantWave legalW)
	{
		//legal wave arrived in country
		legalWaves.Add(legalW);
		//add that to total legals
		numberOfLegalImmigrants+= legalW.numberOfImmigrants;
	}
    #endregion

    public void checkForLegalizedImmigrantsVisas()
	{
        //Beginning of a new month. Check if (legal) immigrants cn get visas
		for(int i = 0; i < legalWaves.Count; i+=0)
		{
            ImmigrantWave immigrant = legalWaves[i];
            if (immigrant.checkIfBecameLegal())//if this immigrant wave can become part of the country now
            {
                StatsManager.instance.legalPopulation += immigrant.numberOfImmigrants;
                numberOfNaturalizedImmigrants += immigrant.numberOfImmigrants; //to be used as statistics in the end
                ResourceManager.instance.numberOfAvailableHouses += immigrant.numberOfImmigrants;
                //delete wave of immigrants; part of the country now
                legalWaves.Remove(immigrant);
            }
            else i++;
		}
	}

    void Awake()
    {
        instance = this;
        immigrant_save_package = new ImmigrantSavePackage();
        numberOfNaturalizedImmigrants = 0;
        numberOfLegalImmigrants = 0;
        numberOfIllegalImmigrants = 0;
    }

	// Use this for initialization
	void Start () {

        immigrant_save_package = new ImmigrantSavePackage();
		legalWaves = new List<ImmigrantWave> ();
		illegalWaves = new List<ImmigrantWave> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
