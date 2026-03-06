using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace DroneAssembly.DataBase
{
    public class DataBaseManager : MonoBehaviour
    {
    
    private readonly string saveScoreURL = "http://localhost/drone_api/save_score.php";

   
    public void SavePlayerScore(string playerName, float time)
    {
        StartCoroutine(SendScoreToServer(playerName, time));
    }

    private IEnumerator SendScoreToServer(string playerName, float time)
    {
        
        WWWForm form = new WWWForm();
       
        form.AddField("player_name", playerName);
        
        form.AddField("completion_time", time.ToString("F2")); 
        
        using (UnityWebRequest www = UnityWebRequest.Post(saveScoreURL, form))
        {
            Debug.Log("Odesílám data na server...");
            
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Chyba připojení: " + www.error);
            }
            else
            {
                Debug.Log("Odpověď ze serveru: " + www.downloadHandler.text);
            }
        }
    }
    
    [ContextMenu("Test Odeslání Skóre")]
    private void TestSave()
    {
        SavePlayerScore("Karel_Tester", 42.5f);
    }
}
}
