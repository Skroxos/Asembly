using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace DroneAssembly.DataBase
{
    public class LeaderboardAPI : MonoBehaviour
    {
      
        private string _saveScoreURL => Secrets.Secret.SaveScoreURL;
        private string _secretKey => Secrets.Secret.SecretKey;

        public void SavePlayerScore(string playerName, float time, Action<bool, string> onComplete = null)
        {
            StartCoroutine(SendScoreToServer(playerName, time, onComplete));
        }

        private IEnumerator SendScoreToServer(string playerName, float time, Action<bool, string> onComplete)
        {
           
            string timeString = time.ToString("F2");
            
            string hash = GenerateSHA256(playerName + timeString + _secretKey);

            WWWForm form = new WWWForm();
            form.AddField("player_name", playerName);
            form.AddField("completion_time", timeString);
            form.AddField("hash", hash);
            
            using (UnityWebRequest www = UnityWebRequest.Post(_saveScoreURL, form))
            {
                yield return www.SendWebRequest();
                
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    onComplete?.Invoke(false, "Chyba připojení k serveru: " + www.error);
                }
                else
                {
                    string responseText = www.downloadHandler.text.Trim();
                    if (responseText == "Success")
                    {
                        onComplete?.Invoke(true, "Skóre bylo úspěšně uloženo!");
                    }
                    else
                    {
                        onComplete?.Invoke(false, "Odpověď serveru: " + responseText);
                    }
                }
            }
        }
        
        private string GenerateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        
        private string getTop10URL => Secrets.Secret.GetTopScoresURL;

        public void GetTop10(Action<bool, LeaderboardData, string> onComplete = null)
        {
            StartCoroutine(DownloadTop10FromServer(onComplete));
        }

        private IEnumerator DownloadTop10FromServer(Action<bool, LeaderboardData, string> onComplete)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(getTop10URL))
            {
                yield return www.SendWebRequest();
                
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    onComplete?.Invoke(false, default, "Chyba připojení k serveru: " + www.error);
                }
                else
                {
                    string jsonResponse = www.downloadHandler.text.Trim();
                    
                    if (jsonResponse.Contains("error") || !jsonResponse.Contains("top10"))
                    {
                        onComplete?.Invoke(false, default, "Chyba serveru: " + jsonResponse);
                    }
                    else
                    {
                        LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(jsonResponse);
                        onComplete?.Invoke(true, data, "Žebříček úspěšně stažen!");
                    }
                }
            }
        }
        
        [ContextMenu("Test Odeslání Skóre")]
        private void TestSave()
        {
            SavePlayerScore("Karel_Tester", 42.5f, (uspech, zprava) => 
            {
                if (uspech) {
                    Debug.Log("<color=green>" + zprava + "</color>");
                } else {
                    Debug.LogError("<color=red>" + zprava + "</color>");
                }
            });
        }
        [ContextMenu("Test Stažení Top 10")]
        private void TestDownload()
        {   
            GetTop10((uspech, naseData, zprava) => 
            {
                if (uspech) 
                {
                    Debug.Log("<color=green>" + zprava + "</color>");
                    
                    int pozice = 1;
                    foreach (PlayerScore hrac in naseData.top10)
                    {
                        Debug.Log($"<b>{pozice}. místo:</b> {hrac.player_name} | Čas: {hrac.completion_time} s");
                        pozice++;
                    }
                } 
                else 
                {
                    Debug.LogError("<color=red>" + zprava + "</color>");
                }
            });
        }
        
    }
}