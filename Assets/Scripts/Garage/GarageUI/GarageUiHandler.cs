using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Garage.GarageUI
{
    public class GarageUiHandler : MonoBehaviour
    {
        [SerializeField] private Button[] panelsButtons;
        [SerializeField] private GameObject[] panels;
        private Stack<GameObject> stack = new(); 

        private void Start()
        {
            for (int i = 0; i < panelsButtons.Length; i++)
            {
                var index = i;
                panelsButtons[i].onClick.AddListener((() => PanelChoose(index)));
            }
        }

        public void ChooseTargetPanel(GameObject target)
        {
            var index = Array.IndexOf(panels, target);
            PanelChoose(index);
        }
        private void PanelChoose(int id)
        {
            var lastPanel = stack.Count == 0 ?null :stack?.Pop();
            if (lastPanel!=null)
            {
                lastPanel.SetActive(false);
            }
            var newPanel = panels[id];
            if (lastPanel == newPanel)
            {
                return;
            }
            stack.Push(newPanel);
            newPanel.SetActive(true);
        }
    }
}
