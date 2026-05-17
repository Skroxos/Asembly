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

        public async void SendData(string playerName, float time)
        {
            string timeString = time.ToString("F2", CultureInfo.InvariantCulture);
            string hash = GenerateSHA256(playerName + timeString + _secretKey);
            ScorePayload scorePayload = new ScorePayload
            {
                player_name = playerName,
                completion_time = timeString,
                hash = hash
            };

            string jsonPayload = JsonUtility.ToJson(scorePayload);

            await SendPlayerDataAsync(_saveScoreURL, jsonPayload);
        }

        public async UniTask SendPlayerDataAsync(string url, string jsonPayload)
        {
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
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
                        Debug.Log($"<color=green>Data Send (Status {request.responseCode})</color>");
                    }
                    else
                    {
                        Debug.LogError($"failed. status code: {request.responseCode}\n{request.downloadHandler.text}");
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.LogError("Time Limit Reached");
                }
                catch (UnityWebRequestException ex)
                {
                    Debug.LogError($"Error: {ex.Message}");
                }
            }
        }


        //public async void FetchData()
        //{
        //    await SendGetRequestAsync(_getTop10URL);
        //}

        public async UniTask<LeaderboardData> FetchDataAsync()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(_getTop10URL))
            {
                try
                {
                    await request.SendWebRequest().ToUniTask();

                    if (request.responseCode == 200)
                    {
                        string jsonResponse = request.downloadHandler.text.Trim();
                        return JsonUtility.FromJson<LeaderboardData>(jsonResponse);
                    }
                    else
                    {
                        Debug.LogError($"Server denied request Code: {request.responseCode}");
                        return new LeaderboardData();
                    }
                }
                catch (UnityWebRequestException ex)
                {
                    Debug.LogError(ex.Message);
                    return new LeaderboardData();
                }
            }
        }

        //public async UniTask SendGetRequestAsync(string url)
        //{
            
        //}

       

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



//#if UNITY_EDITOR
//        [ContextMenu("Test Score Submission")]
//        private  void TestSave()
//        {
//            Debug.Log("Sending data to server...");
//            SendData("Test_Player", 42.5f);
//            Debug.Log("<color=green>Data sent (check server response above)</color>");
//        }

//        [ContextMenu("Test Download Top 10")]
//        private async void TestDownload()
//        {
//            Debug.Log("Downloading leaderboard from server...");

//            LeaderboardData data = await FetchDataAsync();

//            if (data.top10 != null && data.top10.Length > 0)
//            {
//                Debug.Log($"<color=green>Loaded {data.top10.Length} entries</color>");
//                int rank = 1;
//                foreach (PlayerScore player in data.top10)
//                {
//                    Debug.Log($"<b>Rank {rank}:</b> {player.player_name} | Time: {player.completion_time} s");
//                    rank++;
//                }
//            }
//            else
//            {
//                Debug.LogError("<color=red>No data received or request failed</color>");
//            }
//        }
//#endif
    }
}