using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour
{
    public Text leaderboardText;

    public int showTop = 4;

    // Start is called before the first frame update
    void Start()
    {
        
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

                // display the top X players (defined by showTop public int)
                string tempString = "Leaderboard";

                // check if there are actually four players joined
                if (joinedList.Count >= showTop)
                {
                   for (int i = 0; i < showTop; i++)
                   {
                        tempString += "\n" + joinedList[i].GetComponent<PlayerScript>().playerName + ": " + joinedList[i].GetComponent<PlayerScript>().playerScore;
                   }
                    
                }
                else
                {
                    for (int i = 0; i < joinedList.Count; i++)
                    {
                        tempString += "\n" + joinedList[i].GetComponent<PlayerScript>().playerName + ": " + joinedList[i].GetComponent<PlayerScript>().playerScore;

                    }
                }
                // apply it to the leaderboard text
                leaderboardText.text = tempString;
            }
        }
    }
}
