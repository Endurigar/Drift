using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private Button back;
        [SerializeField] private GameObject mainMenu;

        void Start()
        {
            back.onClick.AddListener(OnBack);
            dropdown.value = QualitySettings.GetQualityLevel();
            dropdown.onValueChanged.AddListener((arg => ChangeLevel(arg)));
        }

        private void ChangeLevel(int value)
        {
            QualitySettings.SetQualityLevel(value);
        }

        private void OnBack()
        {
            mainMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}