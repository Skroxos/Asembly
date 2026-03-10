using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using DroneAssembly.DataBase.Models;
using System.Globalization;

namespace DroneAssembly.DataBase
{
    public class LeaderboardAPI : MonoBehaviour
    {
        private string _saveScoreURL => Secrets.Secret.SaveScoreURL;
        private string _secretKey => Secrets.Secret.SecretKey;
        private string _getTop10URL => Secrets.Secret.GetTopScoresURL;

        public void SavePlayerScore(string playerName, float time, Action<bool, string> onComplete = null)
        {
            StartCoroutine(SendScoreToServer(playerName, time, onComplete));
        }

        private IEnumerator SendScoreToServer(string playerName, float time, Action<bool, string> onComplete)
        {
            string timeString = time.ToString("F2", CultureInfo.InvariantCulture);
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
                    onComplete?.Invoke(false, "Server connection error: " + www.error);
                }
                else
                {
                    string responseText = www.downloadHandler.text.Trim();
                    if (responseText == "Success")
                    {
                        onComplete?.Invoke(true, "Score saved successfully!");
                    }
                    else
                    {
                        onComplete?.Invoke(false, "Server response: " + responseText);
                    }
                }
            }
        }
        
        public void GetTop10(Action<bool, LeaderboardData, string> onComplete = null)
        {
            StartCoroutine(DownloadTop10FromServer(onComplete));
        }

        private IEnumerator DownloadTop10FromServer(Action<bool, LeaderboardData, string> onComplete)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(_getTop10URL))
            {
                yield return www.SendWebRequest();
                
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    onComplete?.Invoke(false, default, "Server connection error: " + www.error);
                }
                else
                {
                    string jsonResponse = www.downloadHandler.text.Trim();
                    
                    if (jsonResponse.Contains("error") || !jsonResponse.Contains("top10"))
                    {
                        onComplete?.Invoke(false, default, "Server error: " + jsonResponse);
                    }
                    else
                    {
                        LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(jsonResponse);
                        onComplete?.Invoke(true, data, "Leaderboard downloaded successfully!");
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
        
#if UNITY_EDITOR
        [ContextMenu("Test Score Submission")]
        private void TestSave()
        {
            SavePlayerScore("Test_Player", 42.5f, (success, message) => 
            {
                if (success) {
                    Debug.Log("<color=green>" + message + "</color>");
                } else {
                    Debug.LogError("<color=red>" + message + "</color>");
                }
            });
        }

        [ContextMenu("Test Download Top 10")]
        private void TestDownload()
        {   
            GetTop10((success, data, message) => 
            {
                if (success) 
                {
                    Debug.Log("<color=green>" + message + "</color>");
                    
                    int rank = 1;
                    foreach (PlayerScore player in data.top10)
                    {
                        Debug.Log($"<b>Rank {rank}:</b> {player.player_name} | Time: {player.completion_time} s");
                        rank++;
                    }
                } 
                else 
                {
                    Debug.LogError("<color=red>" + message + "</color>");
                }
            });
        }
#endif
    }
}