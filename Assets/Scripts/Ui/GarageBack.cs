using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class GarageBack : MonoBehaviour
    {
        [SerializeField] private Button back;
        [SerializeField] private GameObject mainMenu;

        void Start()
        {
            back.onClick.AddListener(OnBack);
        }

        private void OnBack()
        {
            mainMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}