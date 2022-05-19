using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    //public Leaderboard leaderboard;
    public TMP_InputField playerNameInputfield;
    
    void Start()
    {
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        //yield return leaderboard.FetchTopHighscoresRoutine();
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
