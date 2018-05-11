using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ScoreScript : MonoBehaviour
{

    public static ScoreScript sm;
    delegate void MyDelegate();
    MyDelegate myDelegate;

    //variables to save?
    public int _highscore = 0, _currentscore = 0;
    public Text score, highscore;

    private void Awake()
    {
        if (sm == null)
        {
            DontDestroyOnLoad(gameObject);
            sm = this;
        }
        else if (sm != this)
        {
            Destroy(gameObject);
        }

    }
    public void Start()
    {
        LoadGameSave();
        myDelegate = UpdateHighScore;
        highscore.text = "High Score: " + _highscore;
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Kills: " + _currentscore;
        if (_currentscore > _highscore)
        {
            myDelegate();
            highscore.text = "High Score: " + _highscore;
        }
    }

    //updates high score and saves it in a binary file
    void UpdateHighScore()
    {
        _highscore = _currentscore;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameSave.dat");
        GameSaves gsave = new GameSaves();

        gsave._savedhighscore = _highscore;

        bf.Serialize(file, gsave);
        file.Close();
    }

    //Loads contents of save file
    void LoadGameSave()
    {
        if (File.Exists(Application.persistentDataPath + "/gameSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);
            GameSaves gSaves = (GameSaves)bf.Deserialize(file);
            _highscore = gSaves._savedhighscore;

            file.Close();
        }
    }
}

[Serializable]

class GameSaves
{
    //variables to save
    public int _savedhighscore;
}
