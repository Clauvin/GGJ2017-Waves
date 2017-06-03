﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Basics_3;
using UnityEngine.UI;
using System;

public class CommerceEventGO : MonoBehaviour {

    public delegate double BuyOrSellValue();

    public GameObject assignedButton;

    #region Cooldown Variables
    public float actionCooldownPeriod;

    public float timeLastUsed;//saves time action was last used
    public bool isActive;//tells if action can be used or if it's cooling down
    #endregion

    public BuyOrSellValue buy_value;
    public BuyOrSellValue sell_value;

    public Commerce_Actions commerce_action = Commerce_Actions.Buying;

    public MiscInfo.variableTypes what_is_being_bought_sold;

    public void CheckObject()
    {
        if (commerce_action == Commerce_Actions.Buying) CheckBuyAction();
        else if (commerce_action == Commerce_Actions.Selling) CheckSellAction();
    }

    public void CheckBuyAction()
    {
        if (IsBuyable())
        {
            Commerce();
            Destroy(this.gameObject);
        }
    }

    public void CheckSellAction()
    {
        //se puder vender, ótimo, usar sell_function para vender e fechar a janela.
        //se não puder vender, avisar o porquê.
        if (IsSellable())
        {
            Commerce();
            Destroy(this.gameObject);
        }
    }

    public void PressedOKEventButton()
	{
        try
        {
            CheckBuyAction();
        }
        catch (FormatException e)
        {
            transform.Find("CommerceEventPanel/ProblemsDescription").GetComponent<Text>().text = e.Message;
        }
    }

    public void PressedChangeActionButton()
    {
        switch (commerce_action)
        {
            case Commerce_Actions.Buying:
                commerce_action = Commerce_Actions.Selling;
                transform.Find("CommerceEventPanel/ChangeBuySellButton/Text").GetComponent<Text>().text = "Change to Buy";
                break;
            case Commerce_Actions.Selling:
                commerce_action = Commerce_Actions.Buying;
                transform.Find("CommerceEventPanel/ChangeBuySellButton/Text").GetComponent<Text>().text = "Change to Sell";
                break;
        }
    }

    public void PressedCancelEventButton()
    {
        //Destroy this Random Event Popup
        Destroy(this.gameObject);

    }

    private int GetQuantity()
    {
        string text = GetComponentInChildren<Image>().GetComponentInChildren<InputField>().text;
        if (text == "") return 0;
        else return int.Parse(text);
    }


    private bool IsBuyable()
    {
        int quantity;

        try
        {
            quantity = GetQuantity();
        }
        catch(OverflowException oe)
        {
            throw new FormatException("Max quantity allowed for buying is " +
                int.MaxValue.ToString() + ".");
        }
       
        if (quantity > 0)
        {
            double total = ResourceManager.instance.playerCurrentMoney - quantity * buy_value();

            if (total >= 0)
            {
                return true;
            }
            else
            {
                throw new FormatException("You lack " + (total * -1).ToString() + " of money to buy" +
                    " this quantity.");
            }
        }
        else throw new FormatException("Weren't you going to buy something?");
    }

    private bool IsSellable()
    {
        int quantity;

        try
        {
            quantity = GetQuantity();
        }
        catch (OverflowException oe)
        {
            throw new FormatException("Max quantity allowed for selling is " +
                 DefineBuySellObjectQuantity().ToString() + DefineBuySellObject() + ".");
        }

        if (quantity > DefineBuySellObjectQuantity())
        {
            throw new FormatException("Max quantity allowed for selling is " +
                 DefineBuySellObjectQuantity().ToString() + DefineBuySellObject() + ".");
        }
        else if (quantity < 0)
        {
            throw new FormatException("It isn't possible sell negative quantities. You wanted to buy?");
        }
        else if (quantity == 0)
        {
            throw new FormatException("Weren't you going to sell something?");
        }
        else
        {
            return true;
        }
    }

    public void Commerce()
    {
        switch (commerce_action)
        {
            case (Commerce_Actions.Buying):
                BuyStuff();
                break;
            case (Commerce_Actions.Selling):
                SellStuff();
                break;
            default:
                break;
        }
    }

    public void BuyStuff()
    {
        double spent = buy_value() * GetQuantity();

        ResourceManager.instance.playerCurrentMoney -= (int)spent;

        switch (what_is_being_bought_sold)
        {
            case MiscInfo.variableTypes.availableBO:
                ResourceManager.instance.numberOfAvailableBorderOfficers += GetQuantity();
                ResourceManager.instance.numberOfTotalBorderOfficers += GetQuantity();
                break;
            case MiscInfo.variableTypes.availableHouses:
                ResourceManager.instance.numberOfAvailableHouses += GetQuantity();
                ResourceManager.instance.numberOfTotalHouses += GetQuantity();
                break;
            case MiscInfo.variableTypes.borderResources:
                ResourceManager.instance.borderResources += GetQuantity();
                break;
            case MiscInfo.variableTypes.socialResources:
                ResourceManager.instance.socialResources += GetQuantity(); ;
                break;
            default:
                break;
        }
    }

    public void SellStuff()
    {
        double gained = sell_value() * GetQuantity();

        ResourceManager.instance.playerCurrentMoney += (int)gained;

        switch (what_is_being_bought_sold)
        {
            case MiscInfo.variableTypes.availableBO:
                ResourceManager.instance.numberOfAvailableBorderOfficers -= GetQuantity();
                ResourceManager.instance.numberOfTotalBorderOfficers -= GetQuantity();
                break;
            case MiscInfo.variableTypes.availableHouses:
                ResourceManager.instance.numberOfAvailableHouses -= GetQuantity();
                ResourceManager.instance.numberOfTotalHouses -= GetQuantity();
                break;
            case MiscInfo.variableTypes.borderResources:
                ResourceManager.instance.borderResources -= GetQuantity();
                break;
            case MiscInfo.variableTypes.socialResources:
                ResourceManager.instance.socialResources -= GetQuantity(); ;
                break;
            default:
                break;
        }
    }

    private string DefineBuySellObject()
    {
        switch (what_is_being_bought_sold)
        {
            case MiscInfo.variableTypes.availableBO: return "frontier offices";
            case MiscInfo.variableTypes.availableHouses: return "houses ";
            case MiscInfo.variableTypes.borderResources: return "frontier resources";
            case MiscInfo.variableTypes.socialResources: return "social resources";
            default: return "";
        }
    }

    private int DefineBuySellObjectQuantity()
    {
        switch (what_is_being_bought_sold)
        {
            case MiscInfo.variableTypes.availableBO:
                return ResourceManager.instance.numberOfTotalBorderOfficers;
            case MiscInfo.variableTypes.availableHouses:
                return ResourceManager.instance.numberOfTotalHouses;
            case MiscInfo.variableTypes.borderResources:
                return ResourceManager.instance.borderResources;
            case MiscInfo.variableTypes.socialResources:
                return ResourceManager.instance.socialResources;
            default: return 0;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
