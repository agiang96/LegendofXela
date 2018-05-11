using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour {
    public Text killText;
    private int kills;

    private void Start()
    {
        kills = 0;
        killText.text = "Kills: " + kills;
    }

    public void UpdateKills()
    {
        killText.text = "Killed: " + ++kills;
    }
}
