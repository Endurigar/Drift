using Fusion;
using Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui
{
    public class PauseMenu : NetworkBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button exitButton;

        private void Start()
        {
            resumeButton.onClick.AddListener(Resume);
            exitButton.onClick.AddListener(ExitButton);
        }

        private void Resume()
        {
            this.gameObject.SetActive(false);
        }

        private void ExitButton()
        {
            Destroy(BasicSpawner.Instance.gameObject);
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}