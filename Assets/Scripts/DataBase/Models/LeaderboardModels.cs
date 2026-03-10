[System.Serializable]
public struct PlayerScore
{
    public string player_name;
    public float completion_time;
}
    
[System.Serializable]
public struct LeaderboardData
{
    public PlayerScore[] top10;
}
