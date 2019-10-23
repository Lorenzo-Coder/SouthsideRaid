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
    public Text monsterLevelText;
    public Text playerCount;
    public int currentBossLevel = 0;
    public Sprite buttonUp;
    public Sprite buttonDown;
    public Text helperText;
    public Image buttonImage;
    public float frameTime = 0.25f;
    public float frameTimer = 0.0f;
    private bool isButtonUp = true;

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
        monsterLevelText.text = currentBossLevel.ToString();
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
            playerCount.text = joinedList.Count.ToString();
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

        // for changing the button and text helpers
        if (GameObject.FindGameObjectWithTag("Boss")!= null)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            if (boss.GetComponent<BossScript>().stance == Stances.Idle)
            {
                helperText.text = "Let's Get Crazy";
                //resetButton();
                clickAnimation();
            }
            else if (boss.GetComponent<BossScript>().stance == Stances.Attack)
            {
                helperText.text = "Hold to Block";
                buttonImage.sprite = buttonDown;
            }
            else if (boss.GetComponent<BossScript>().stance == Stances.Down)
            {
                if (boss.GetComponent<BossScript>().isCritical)
                {
                    helperText.text = "SPAM";
                    //resetButton
                    clickAnimation();
                }
                else
                {
                    helperText.text = "Wait for it";
                    resetButton();
                }
            }
           
            else
            {
                resetButton();
            }

        }
    }

    public void IncBossLevel()
    {
        currentBossLevel++;
    }
    private void resetButton()
    {
        frameTimer = 0.0f;
        buttonImage.sprite = buttonUp;
        isButtonUp = true;
    }
    private void clickAnimation()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer >= frameTime)
        {
            if(isButtonUp)
            {
                buttonImage.sprite = buttonDown;
            }
            else
            {
                buttonImage.sprite = buttonUp;
            }
            isButtonUp = !isButtonUp;
            frameTimer = 0.0f;
        }
    }
}
