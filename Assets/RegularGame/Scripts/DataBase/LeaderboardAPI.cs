using Cysharp.Threading.Tasks;
using DroneAssembly.DataBase.Models;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

namespace DroneAssembly.DataBase
{
    public class LeaderboardAPI : INetworkService
    {
        private string _saveScoreURL => Secrets.Secret.SaveScoreURL;
        private string _secretKey => Secrets.Secret.SecretKey;
        private string _getTop10URL => Secrets.Secret.GetTopScoresURL;

        public static readonly HttpClient _httpClient = new HttpClient();

        [Serializable]
        public class ScorePayload
        {
            public string Name;
            public float finishTime;
            public string hash;
        }

        public string PreparePostData(string playerName, float time)
        {
            string timeString = time.ToString("F2", CultureInfo.InvariantCulture);
            string hash = GenerateSHA256(playerName + timeString + _secretKey);
            ScorePayload scorePayload = new ScorePayload
            {
                Name = playerName,
                finishTime = time,
                hash = hash
            };

            return JsonUtility.ToJson(scorePayload);
        }

        public async UniTask<bool> SendPlayerDataAsync(string playerName, float time)
        {
            Debug.Log($"Pokouším se odeslat data na: {_saveScoreURL}");
            string jsonPayload = PreparePostData(playerName, time);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            try 
            {
                using var response = await _httpClient.PostAsync(_saveScoreURL, content, cts.Token);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    string errorText = await response.Content.ReadAsStringAsync();
                    Debug.LogError($"Server denied request Code: {response.StatusCode}, Message: {errorText}");
                    return false;
                }
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Request timeout");
                return false;
            }
            catch (HttpRequestException ex)
            {
                Debug.LogError(ex.Message);
                return false;
            }
        }




        public async UniTask<(bool isSuccess, PlayerScore[] data)> FetchDataAsync()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            try
            {
                using var response = await _httpClient.GetAsync(_getTop10URL, cts.Token);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    PlayerScore[] data = JsonConvert.DeserializeObject<PlayerScore[]>(jsonResponse);
                    return (true, data);
                }
                else
                {
                    string errorText = await response.Content.ReadAsStringAsync();
                    Debug.LogError($"Server denied request Code: {response.StatusCode}, Message: {errorText}");
                    return (false, null);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Request timeout");
                return (false, null);
            }
            catch (HttpRequestException ex)
            {
                Debug.LogError(ex.Message);
                return (false, null);

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