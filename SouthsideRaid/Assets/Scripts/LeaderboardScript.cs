using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreSet
{
    public GameObject AttachedPlayer;
    public GameObject AttachedTextBox;

    public int ScoreEasyAccess = 0;
    public void UpdateText()
    { //joinedList[i].GetComponent<PlayerScript>().playerName + "  " + joinedList[i].GetComponent<PlayerScript>().playerScore
        ScoreEasyAccess = AttachedPlayer.GetComponent<PlayerScript>().playerScore;
        AttachedTextBox.transform.GetChild(0).GetComponent<Text>().text = AttachedPlayer.GetComponent<PlayerScript>().playerName + "  " + AttachedPlayer.GetComponent<PlayerScript>().playerScore;
    }
}

public class LeaderboardScript : MonoBehaviour
{
    public GameObject[] leaderboardArray;

    private Vector3[] originalPos;
    private Vector3[] originalScale;

    private List<ScoreSet> HighScoreSet;

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
        HighScoreSet = new List<ScoreSet>();
        originalPos = new Vector3[4];
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < leaderboardArray.Length; i++)
        {
            originalPos[i] = leaderboardArray[i].transform.localPosition;

            ScoreSet scoreSet = new ScoreSet();
            scoreSet.AttachedTextBox = leaderboardArray[i];
            scoreSet.AttachedPlayer = objects[i];

            HighScoreSet.Add(scoreSet);

        }
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
            for (int i = 0; i < HighScoreSet.Count; i++)
            {
                if (HighScoreSet[i].AttachedPlayer.GetComponent<PlayerScript>().joined)
                {
                    HighScoreSet[i].AttachedTextBox.SetActive(true);
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
                HighScoreSet[i].AttachedTextBox.transform.DOLocalMove(originalPos[i], 0.2f);
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
