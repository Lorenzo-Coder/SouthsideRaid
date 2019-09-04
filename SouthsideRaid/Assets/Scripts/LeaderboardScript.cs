using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour
{
    public Text leaderboardPos1;
    public Text leaderboardPos2;
    public Text leaderboardPos3;
    public Text leaderboardPos4;
    public Text leaderboardPos5;
    private Text[] leaderboardArray;
    private int showTop = 5;

    // Start is called before the first frame update
    void Start()
    {
        leaderboardPos1.text = "";
        leaderboardPos2.text = "";
        leaderboardPos3.text = "";
        leaderboardPos4.text = "";
        leaderboardPos5.text = "";
        leaderboardArray = new Text[5];

        leaderboardArray[0] = leaderboardPos1;
        leaderboardArray[1] = leaderboardPos2;
        leaderboardArray[2] = leaderboardPos3;
        leaderboardArray[3] = leaderboardPos4;
        leaderboardArray[4] = leaderboardPos5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameObject.FindGameObjectsWithTag("Player")==null)) // cherck if there are players in the scene
        {
            GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
            // check if any of them have joined
            int joined = 0;
            List<GameObject> joinedList = new List<GameObject>();
            for (int i = 0; i < playerList.Length; i++)
            {
                if (playerList[i].GetComponent<PlayerScript>().joined)
                {
                    joinedList.Add(playerList[i]);
                    joined++;
                }
            }

            // if more than one player has joined
            if (joined > 0)
            {
                // sort through all the players to find players with the highest score
                joinedList.Sort(delegate (GameObject a, GameObject b)
                {
                    return (b.GetComponent<PlayerScript>().playerScore.CompareTo(a.GetComponent<PlayerScript>().playerScore));
                });

                // check if there are actually five players joined
                if (joinedList.Count >= showTop)
                {
                    for (int i = 0; i < showTop; i++)
                    {
                        leaderboardArray[i].text = joinedList[i].GetComponent<PlayerScript>().playerName;
                    }
                }
                else
                {
                    for (int i = 0; i < joinedList.Count; i++)
                    {
                        leaderboardArray[i].text = joinedList[i].GetComponent<PlayerScript>().playerName;
                    }
                }
            }
        }
    }
}
