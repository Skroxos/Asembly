using System;
using Cysharp.Threading.Tasks;
using DroneAssembly.Radios.GeneralRadios;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace DroneAssembly.ScoresUI
{
    public class FinalUIManager : MonoBehaviour
    {
        [SerializeField] private SimpleEventRadio finishRadio;
        [SerializeField] private GameObject inputFieldUI;
        [SerializeField] private GameObject leaderboardUI;
        [SerializeField] private GameObject prefabLeaderboardDisplayElement;
        [SerializeField] private GameObject leaderboardPanel;
        [SerializeField] private TextMeshProUGUI playerTimeText;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button submitButton;
        private INetworkService _networkService;

        private float _time;

        [Inject]
        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;
        }


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

        public async void OnSubmitButtonClicked()
        {
            string playerName = inputField.text.Trim();
            if (string.IsNullOrWhiteSpace(playerName))
            {
                Debug.LogWarning("Player name is empty");
                return;
            }
            submitButton.interactable = false;
            await _networkService.SendPlayerDataAsync(playerName, _time);
            inputFieldUI.SetActive(false);
            ShowLeaderboard().Forget();
        }

        public void OnClickedCancel()
        {
            inputFieldUI.SetActive(false);
            ShowLeaderboard().Forget();
        }

        public async UniTaskVoid ShowLeaderboard()
        {
            leaderboardPanel.SetActive(true);
            leaderboardUI.SetActive(true);
            playerTimeText.SetText(FormatTime(_time));

            foreach (Transform child in leaderboardUI.transform)
            {
                Destroy(child.gameObject);
            }

            var results = await _networkService.FetchDataAsync();

            if (results.isSuccess)
            {
                foreach (var score in results.data)
                {
                    GameObject entry = Instantiate(prefabLeaderboardDisplayElement, leaderboardUI.transform);
                    TextMeshProUGUI entryText = entry.GetComponentInChildren<TextMeshProUGUI>();
                    entryText.SetText($"{score.Name}: {FormatTime(score.FinishTime)}");
                }
            }
            else
            {
                leaderboardPanel.SetActive(false);
                leaderboardUI.SetActive(false);
            }
        }

        
        private string FormatTime(float totalSeconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
            return timeSpan.ToString(@"mm\:ss\.ff");
        }

    }
}
