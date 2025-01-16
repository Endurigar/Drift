using System;
using Mods;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class CarSwapper : MonoBehaviour
    {
        [SerializeField] private CarModsSo carModsSO;
        [SerializeField] private Car car;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;

        private int currentIndex = 0;
        private bool isForward = true;

        public event Action OnCarSwapped;

        void Start()
        {
            if (nextButton != null)
            {
                nextButton.onClick.AddListener(SwitchNext);
            }

            if (previousButton != null)
            {
                previousButton.onClick.AddListener(SwitchPrevious);
            }

            currentIndex = PlayerPrefs.GetInt("CurrentCarIndex");
            SpawnObject(currentIndex);
        }

        void SwitchNext()
        {
            isForward = true;
            SwitchObject();
        }

        void SwitchPrevious()
        {
            isForward = false;
            SwitchObject();
        }

        void SwitchObject()
        {
            if (isForward)
            {
                currentIndex++;
                if (currentIndex >= carModsSO.CarPrefabs.Count)
                {
                    currentIndex = 0;
                    isForward = false;
                }
            }
            else
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = carModsSO.CarPrefabs.Count - 1;
                    isForward = true;
                }
            }

            SpawnObject(currentIndex);
        }

        void SpawnObject(int index)
        {
            car.ChangeCar(index);
            OnCarSwapped?.Invoke();
        }
    }
}