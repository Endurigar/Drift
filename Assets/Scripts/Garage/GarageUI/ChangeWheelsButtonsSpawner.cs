using System;
using Mods;
using UnityEngine;
using UnityEngine.UI;
using Decal = Mods.Decal;

namespace Garage.GarageUI
{
    public class ChangeWheelsButtonsSpawner : MonoBehaviour
    {
        [SerializeField] private Button wheelButton;
        [SerializeField] private Button decalButton;
        [SerializeField] private Transform buttonsParent;
        [SerializeField] private Transform decalParent;
        [SerializeField] private CarModsSo carModsSo;

        public event Action<string> OnWheelSelected; 
        public event Action<string> OnDecalSelected; 
        private void Start()
        {
            DecalButtonSpawner();
            ButtonsSpawner();
        }
    
        private void ButtonsSpawner()
        {
            foreach (Mods.Wheel wheel in carModsSo.GetAllModByType<Mods.Wheel>())
            {
                var newButton = Instantiate(wheelButton, buttonsParent);
                newButton.onClick.AddListener((() => OnWheelSelected?.Invoke(((ICarMod)wheel).Id)));
                newButton.gameObject.GetComponent<Image>().sprite = Sprite.Create(wheel.Thumbnail,new Rect(0,0,wheel.Thumbnail.width,wheel.Thumbnail.height),Vector2.zero);
            }
        }

        private void DecalButtonSpawner()
        {
            foreach (var decal in carModsSo.GetAllModByType<Decal>())
            { 
                var newButton = Instantiate(decalButton, decalParent);
                newButton.onClick.AddListener((() => OnDecalSelected?.Invoke(((ICarMod)decal).Id)));
                newButton.gameObject.GetComponent<Image>().sprite = Sprite.Create(decal.Thumbnail,new Rect(0,0,decal.Thumbnail.width,decal.Thumbnail.height),Vector2.zero);
            }
        }
    }
}
