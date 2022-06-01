using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public Leaderboard leaderboard;
    public TMP_InputField playerNameInputfield;
    
    void Start()
    {
        playerNameInputfield.text = GameManager.GetMyNickname();
        StartCoroutine(SetupRoutine());
    }

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerNameInputfield.text, (response) => {
            if(response.success) {
                GameManager.SetMyNickname(playerNameInputfield.text);
                Debug.Log("Succesfully set player name");
            } else {
                Debug.Log("Could not set player name"+response.Error);
            }
        });
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return leaderboard.FetchTopHighscoresRoutine();
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        
        LootLockerSDKManager.StartGuestSession((response) => {
            
            if(response.success) {
                Debug.Log("Player was logged in");
                GameManager.SetMyPlayerID(response.player_id.ToString());
            } else {
                Debug.Log("Could not start session");
            }
            done = true;
        });

        yield return new WaitWhile(() => done == false);
    }
}
