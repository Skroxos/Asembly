using Newtonsoft.Json;

namespace DroneAssembly.DataBase.Models
{
    
[System.Serializable]
public struct PlayerScore
{
        [JsonProperty("Name")]
        public string Name;
        [JsonProperty("FinishTime")]
        public float FinishTime;
}
    
[System.Serializable]
public class LeaderboardData
{
       [JsonProperty("top10")]
       public PlayerScore[] top10;
}
}
