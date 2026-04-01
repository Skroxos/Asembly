using System;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
        
        [Serializable]
        private struct ScorePayload
        {
            public string player_name;
            public string completion_time;
            public string hash;
        }
        
        public async Task<(bool success, string message)> SavePlayerScoreAsync(string playerName, float time)
        {
            string timeString = time.ToString("F2", CultureInfo.InvariantCulture);
            string hash = GenerateSHA256(playerName + timeString + _secretKey);
            
            ScorePayload payloadData = new ScorePayload
            {
                player_name = playerName,
                completion_time = timeString,
                hash = hash
            };
            
            string jsonPayload = JsonUtility.ToJson(payloadData);
            
            using (UnityWebRequest request = new UnityWebRequest(_saveScoreURL, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield(); 
                }
                
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    return (false, "Network error: " + request.error);
                }
                
                if (request.responseCode == 200 || request.responseCode == 201)
                {
                    return (true, "Score saved successfully!");
                }
                
                return (false, $"Rejected (Code {request.responseCode}): {request.downloadHandler.text}");
            }
        }
        
        public async Task<(bool success, LeaderboardData data, string message)> GetTop10Async()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(_getTop10URL))
            {
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    return (false, default, "Network error: " + request.error);
                }
                
                if (request.responseCode == 200)
                {
                    string jsonResponse = request.downloadHandler.text.Trim();
                    LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(jsonResponse);
                    return (true, data, "Leaderboard downloaded successfully.");
                }

                return (false, default, $"Server error (Code {request.responseCode}): {request.downloadHandler.text}");
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
        private async void TestSave()
        {
            Debug.Log("Sending data to server...");
            
            var result = await SavePlayerScoreAsync("Test_Player", 42.5f);
            
            if (result.success) {
                Debug.Log("<color=green>" + result.message + "</color>");
            } else {
                Debug.LogError("<color=red>" + result.message + "</color>");
            }
        }

        [ContextMenu("Test Download Top 10")]
        private async void TestDownload()
        {   
            Debug.Log("Downloading leaderboard from server...");
            
            var result = await GetTop10Async();
            
            if (result.success) 
            {
                Debug.Log("<color=green>" + result.message + "</color>");
                int rank = 1;
                foreach (PlayerScore player in result.data.top10)
                {
                    Debug.Log($"<b>Rank {rank}:</b> {player.player_name} | Time: {player.completion_time} s");
                    rank++;
                }
            } 
            else 
            {
                Debug.LogError("<color=red>" + result.message + "</color>");
            }
        }
#endif
    }
}