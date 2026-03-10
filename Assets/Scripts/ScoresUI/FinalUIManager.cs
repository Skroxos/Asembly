using DroneAssembly.DataBase;
using DroneAssembly.Radios.GeneralRadios;
using TMPro;
using UnityEngine;

namespace DroneAssembly.ScoresUI
{
    public class FinalUIManager : MonoBehaviour
    {
        [SerializeField] private SimpleEventRadio finishRadio;
        [SerializeField] private LeaderboardAPI leaderboardAPI;
        [SerializeField] private GameObject inputFieldUI;
        [SerializeField] private GameObject leaderboardUI;
        [SerializeField] private GameObject prefabLeaderboardDisplayElement;
        [SerializeField] private GameObject leaderboardPanel;
        [SerializeField] private TextMeshProUGUI playerTimeText;
        [SerializeField] private TMP_InputField inputField;

        private float _time;
        private void OnEnable()
        {
            finishRadio.OnRaised += ShowFinalUI;
        }


        private void OnDisable()
        {
            finishRadio.OnRaised -= ShowFinalUI;
        }
        private void ShowFinalUI()
        {
            leaderboardUI.SetActive(false);
            inputFieldUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _time = Time.timeSinceLevelLoad;
        }
    
        public void OnClickedSubmit()
        {  
            
            string playerName = inputField.text.Trim();
            if (string.IsNullOrWhiteSpace(playerName))
            {
                Debug.LogWarning("Jméno hráče nesmí být prázdné.");
                return;
            }
            leaderboardAPI.SavePlayerScore(playerName, _time, (success, message) =>
            {
                if (success)
                {
                    Debug.Log("Skóre úspěšně odesláno!");
                    inputFieldUI.SetActive(false);
                    ShowLeaderboard();
                }
                else
                {
                    Debug.LogError("Chyba při odesílání skóre: " + message);
                }
            });
        }
    
        public void OnClickedCancel()
        {
            inputFieldUI.SetActive(false);
            ShowLeaderboard();
        }
        
        private void ShowLeaderboard()
        {
            leaderboardPanel.SetActive(true);
            leaderboardUI.SetActive(true);
            leaderboardAPI.GetTop10((success, data, message ) =>
            {
                if (success)
                {
                    foreach (var score in data.top10)
                    {
                        GameObject entry = Instantiate(prefabLeaderboardDisplayElement, leaderboardUI.transform);
                        TextMeshProUGUI entryText = entry.GetComponentInChildren<TextMeshProUGUI>();
                        entryText.text = $"{score.player_name}: {score.completion_time:F2} s";
                    }
                    playerTimeText.text = _time.ToString("F2") + " s";
                }
                else
                {
                    Debug.LogError("Chyba při načítání žebříčku: " + message);
                }
            });
        }
    }
    
   
}
