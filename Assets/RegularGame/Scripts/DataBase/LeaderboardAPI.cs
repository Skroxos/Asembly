using Cysharp.Threading.Tasks;
using DroneAssembly.DataBase.Models;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace DroneAssembly.DataBase
{
    public class LeaderboardAPI : INetworkService
    {
        private string _saveScoreURL => Secrets.Secret.SaveScoreURL;
        private string _secretKey => Secrets.Secret.SecretKey;
        private string _getTop10URL => Secrets.Secret.GetTopScoresURL;

        [Serializable]
        public class ScorePayload
        {
            public string player_name;
            public string completion_time;
            public string hash;
        }

        public string PreparePostData(string playerName, float time)
        {
            string timeString = time.ToString("F2", CultureInfo.InvariantCulture);
            string hash = GenerateSHA256(playerName + timeString + _secretKey);
            ScorePayload scorePayload = new ScorePayload
            {
                player_name = playerName,
                completion_time = timeString,
                hash = hash
            };

           return JsonUtility.ToJson(scorePayload);
        }

        public async UniTask<bool> SendPlayerDataAsync(string playerName, float time)
        {
            string jsonPayload = PreparePostData(playerName, time);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            using (UnityWebRequest request = new UnityWebRequest(_saveScoreURL, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                try
                {
                    await request.SendWebRequest().ToUniTask(cancellationToken: cts.Token);
                    if (request.responseCode == 200 || request.responseCode == 201)
                    {
                        Debug.Log($"<color=green>Data Sent (Status {request.responseCode})</color>");
                        return true;
                    }

                    Debug.LogError($"Rejected (Code {request.responseCode}): {request.downloadHandler.text}");
                    return false;
                }
                catch (OperationCanceledException)
                {
                    Debug.LogError("Time Limit Reached (Timeout)");
                    return false;
                }
                catch (UnityWebRequestException ex)
                {
                    Debug.LogError($"Network/Server Error: {ex.Message} \n {ex.Text}");
                    return false;
                }
            }
        }


      

        public async UniTask<(bool isSuccess, LeaderboardData data)> FetchDataAsync()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            using (UnityWebRequest request = UnityWebRequest.Get(_getTop10URL))
            {
                try
                {
                    await request.SendWebRequest().ToUniTask(cancellationToken: cts.Token);

                    if (request.responseCode == 200)
                    {
                       
                        string jsonResponse = request.downloadHandler.text.Trim();
                        LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(jsonResponse);
                        return (true, data);
                    }
                    else
                    {
                        Debug.LogError($"Server denied request Code: {request.responseCode}");
                        return (false, null);
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.LogError("Fetch timeout");
                    return (false, null);
                }
                catch (UnityWebRequestException ex)
                {
                    Debug.LogError(ex.Message);
                    return (false, null);
                }
            }
        }




        private string GenerateSHA256(string input)
        {
            using SHA256 sha256 = SHA256.Create();
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
}