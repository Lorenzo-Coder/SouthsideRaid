using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreSet
{
    public GameObject AttachedPlayer;
    public Text AttachedText;
    public int ScoreEasyAccess = 0;
    public void UpdateText()
    { //joinedList[i].GetComponent<PlayerScript>().playerName + "  " + joinedList[i].GetComponent<PlayerScript>().playerScore
        AttachedText.text = AttachedPlayer.GetComponent<PlayerScript>().playerName + "  " + AttachedPlayer.GetComponent<PlayerScript>().playerScore;
        ScoreEasyAccess = AttachedPlayer.GetComponent<PlayerScript>().playerScore;
    }
}

public class LeaderboardScript : MonoBehaviour
{
    public Text leaderboardPos1;
    public Text leaderboardPos2;
    public Text leaderboardPos3;
    public Text leaderboardPos4;

    private Vector3[] originalPos;
    private Vector3[] originalScale;

    private List<ScoreSet> HighScoreSet;

    private Text[] leaderboardArray;
    private int showTop = 4;

    public Text monsterLevelText;
    public Text playerCount;
    public Sprite buttonUp;
    public Sprite buttonDown;
    public Text helperText;
    public Image buttonImage;

    public int currentBossLevel = 0;

    public float frameTime = 0.25f;
    public float frameTimer = 0.0f;
    private bool isButtonUp = true;

    // Start is called before the first frame update
    void Start()
    {
        leaderboardArray = new Text[4];
        leaderboardArray[0] = leaderboardPos1;
        leaderboardArray[1] = leaderboardPos2;
        leaderboardArray[2] = leaderboardPos3;
        leaderboardArray[3] = leaderboardPos4;

        
        HighScoreSet = new List<ScoreSet>();
        GameObject[] getPlayers = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < showTop; i++)
        {
            ScoreSet tempSet = new ScoreSet();
            tempSet.AttachedPlayer = getPlayers[i];
            tempSet.AttachedText = leaderboardArray[i];
            tempSet.AttachedText.gameObject.SetActive(false);
            tempSet.AttachedText.text = "";

            HighScoreSet.Add(tempSet);
        }

        originalPos = new Vector3[4];
        originalScale = new Vector3[4];
        originalPos[0] = leaderboardPos1.transform.localPosition;
        originalScale[0] = leaderboardPos1.transform.localScale;
        originalPos[1] = leaderboardPos2.transform.localPosition;
        originalScale[2] = leaderboardPos2.transform.localScale;
        originalPos[2] = leaderboardPos3.transform.localPosition;
        originalScale[3] = leaderboardPos3.transform.localScale;
        originalPos[3] = leaderboardPos4.transform.localPosition;
        //originalScale[4] = leaderboardPos4.transform.localScale;

    }

    private List<GameObject> oldJoined = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        monsterLevelText.text = currentBossLevel.ToString();

        if (GameObject.FindGameObjectsWithTag("Player") != null) // cherck if there are players in the scene
        {
            // check if any of them have joined
            int joined = 0;
            //List<GameObject> joinedList = new List<GameObject>();
            for (int i = 0; i < HighScoreSet.Count; i++)
            {
                if (HighScoreSet[i].AttachedPlayer.GetComponent<PlayerScript>().joined)
                {
                    HighScoreSet[i].AttachedText.gameObject.SetActive(true);
                    joined++;
                }
            }

            for (int i = 0; i < HighScoreSet.Count; i++)
            {
                HighScoreSet[i].UpdateText();
            }

            // sort through all the players to find players with the highest score
            HighScoreSet.Sort(delegate (ScoreSet a, ScoreSet b)
            {
                return (b.ScoreEasyAccess.CompareTo(a.ScoreEasyAccess));
            });

            for (int i = 0; i < HighScoreSet.Count; i++)
            {
                HighScoreSet[i].AttachedText.transform.DOLocalMove(originalPos[i], 0.2f);
                //HighScoreSet[i].AttachedText.transform.DOScale(originalScale[i], 0.2f);
            }
        }


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
