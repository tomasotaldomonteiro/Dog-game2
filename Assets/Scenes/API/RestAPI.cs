using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RestAPI : MonoBehaviour
{
    private string uri = "https://dog-shit-game.vercel.app/";
    public IEnumerator Register(string playerName, string playerPassword)
    {
        Debug.Log(playerName + " " + playerPassword);
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm($"{uri}createPlayer/{playerName}/{playerPassword}",
                   ""))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (int.TryParse(www.downloadHandler.text, out int playerId))
                {
                    PlayerPrefs.SetInt("player_id", playerId);
                }
                else
                {
                    // Handle invalid input (e.g., show an error message)
                    Debug.LogError(www.downloadHandler.text);
                }
            }
        }
    }
    
    public IEnumerator SendCollectibleObtainedRequest(int collectableID)
    {
        int playerID = PlayerPrefs.GetInt("player_id");

        Debug.Log("Player ID: " + playerID);
        UnityWebRequest request = new UnityWebRequest($"{uri}collectable/obtain/{playerID}/{collectableID}", "PUT");
       
        request.SetRequestHeader("Content-Type", "application/json");

      
        yield return request.SendWebRequest();

        // Handle the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Collectible obtained successfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Failed to obtain collectible: " + request.error);
        }
    }
}
