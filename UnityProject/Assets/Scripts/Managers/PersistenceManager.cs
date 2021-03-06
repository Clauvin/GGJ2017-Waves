﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class PersistenceManager
{

    public static GameInstance game;

    public static bool saveGame()
    {
        try
        {
            game = new GameInstance();
            game.PreparingSaveFile();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/savedGames.waves");
            bf.Serialize(file, PersistenceManager.game);
            file.Close();
        }
        catch(Exception e)
        {
            return false;
        }
        return true;

    }

    public static bool loadGame()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(Application.persistentDataPath + "/savedGames.waves");
            game = (GameInstance)bf.Deserialize(file);
            game.PlacingSavedFilesBack();
            file.Close();
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
}