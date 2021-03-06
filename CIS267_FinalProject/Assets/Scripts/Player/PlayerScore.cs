using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static int playerScore = 0;
    public static int  levelNumber = 1;
    public static int maxScore = 1000;
    private XPbar xpBar;
    // Start is called before the first frame update
    void Start()
    {
        xpBar = FindObjectOfType<XPbar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addScore(int val)
    {
        playerScore += val;
        if (playerScore >= maxScore)
        {
            int tmpScore = playerScore - 100;
            levelNumber++;
            playerScore = tmpScore;
            xpBar.UpdateLevelNumber();
        }
        xpBar.SetXP(playerScore);
    }

    public int getScore()
    {
        return playerScore;
    }

    public void addLevel()
    {
        levelNumber++;
        xpBar.UpdateLevelNumber();
    }

    public int getLevel()
    {
        return levelNumber;
    }
}
