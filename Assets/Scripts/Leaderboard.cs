using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    string leaderboardKey = "global_highscore";
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;

        LootLockerSDKManager.SubmitScore(GameManager.GetMyPlayerID(), scoreToUpload, leaderboardKey, (response) => {

            if(response.success) {
                Debug.Log("Successfully uploaded score");
            } else {
                Debug.Log("Failed" + response.Error);
            }
            done = true;
        });

        yield return new WaitWhile(() => done == false);
    }
}
