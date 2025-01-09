using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class MeinMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button garageButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private GameObject lobby;
        [SerializeField] private GameObject garage;
        [SerializeField] private GameObject settings;
        [SerializeField] private GameObject basicSpawner;
        [SerializeField] private TMP_Text playerCoins;
        [SerializeField] private GameObject carHandler;
        private readonly Vector3 rotationSpeed = new Vector3(0, 10, 0);


        private void Start()
        {
            playButton.onClick.AddListener((() => OnWindowSelected(lobby)));
            garageButton.onClick.AddListener((() => OnWindowSelected(garage)));
            settingButton.onClick.AddListener((() => OnWindowSelected(settings)));
            exitButton.onClick.AddListener(OnExit);
            playerCoins.text = $"Current coins:\n{PlayerPrefs.GetInt("Coins")}";
            DontDestroyOnLoad(basicSpawner);
        }

        private void Update()
        {
            RotateCar();
        }

        private void OnExit()
        {
            Application.Quit();
        }

        private void OnWindowSelected(GameObject window)
        {
            window.SetActive(true);
            this.gameObject.SetActive(false);
        }

        private void RotateCar()
        {
            if (carHandler != null && gameObject.activeInHierarchy)
            {
                carHandler.transform.Rotate(rotationSpeed * Time.deltaTime);
            }
        }
    }
}