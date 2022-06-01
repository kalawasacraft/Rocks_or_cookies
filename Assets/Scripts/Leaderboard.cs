using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using LootLocker;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    int leaderboardId;
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

    void Awake()
    {
        leaderboardId = (LootLockerConfig.current.developmentMode ? 3155 : 3156);
    }

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;

        LootLockerSDKManager.SubmitScore(GameManager.GetMyPlayerID(), scoreToUpload, leaderboardId, (response) => {

            if(response.success) {
                Debug.Log("Successfully uploaded score");
            } else {
                Debug.Log("Failed" + response.Error);
            }
            done = true;
        });

        yield return FetchTopHighscoresRoutine();
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;

        LootLockerSDKManager.GetScoreListMain(leaderboardId, 10, 0, (response) => {
            
            if(response.success) {
                string tempPlayerNames = "";
                string tempPlayerScores = "";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++) {
                    
                    tempPlayerNames += members[i].rank + ". ";
                    if(members[i].player.name != "") {
                        tempPlayerNames += members[i].player.name;
                    } else {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }

                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            } else {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }
}
