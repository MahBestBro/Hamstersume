using System;
using UnityEngine;
using UnityEngine.UI;

public class RaceWinOverlay : MonoBehaviour
{
    [SerializeField]
    Racecourse racecourse;

    CanvasGroup canvasGroup;
    Text rankText;
    Text electricityRewardText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.0f;

        rankText = transform.Find("Rank").GetComponent<Text>();
        electricityRewardText = transform.Find("Reward").Find("Details").Find("Amount").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    string RankSuffix(int rank)
    {
        switch (rank % 10)
        {
            case 1: return "st";
            case 2: return "nd";
            case 3: return "rd";
            default: return "th";
        }
    }

    public void Show()
    {
        float[] completions = racecourse.RaceCompletions();
        RacingHamster[] hamsters = new RacingHamster[racecourse.hamsters.Length];
        racecourse.hamsters.CopyTo(hamsters, 0);
        Array.Sort(completions, hamsters);
        int rank = hamsters.Length - Array.IndexOf(hamsters, racecourse.PlayerWinner);
        
        int energyReward = 30 - 10 * Math.Max(rank - 1, 0);
        rankText.text = $"{rank}{RankSuffix(rank)} place";
        electricityRewardText.text = $"{energyReward}";

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
