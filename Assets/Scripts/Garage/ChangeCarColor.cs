using Garage.GarageUI;
using Mods;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Garage
{
    public class ChangeCarColor : MonoBehaviour
    {
        [SerializeField] private Car carInstance;
        [SerializeField] private GameObject colorPicker;
        [SerializeField] private GameObject buttonsHandler;
        [SerializeField] private Button setNewColor;
        [SerializeField] private Button closeButton;
        [FormerlySerializedAs("garagaeUiHandler")] [SerializeField] private GarageUiHandler garageUiHandler;
        [SerializeField] private CarSwapper carSwapper;

        private ColorPickerTriangle colorPickerTriangle;
        private Material carMaterial;
        private Color basicColor;

        private Car carData;

        private void Start()
        {
            OnCarSwapped();
            carSwapper.OnCarSwapped += OnCarSwapped;
            setNewColor.onClick.AddListener(SetNewColor);
            closeButton.onClick.AddListener(CancelPaint);
        }

        private void Update()
        {
            if (carInstance != null && carMaterial != null)
            {
                carMaterial.color = colorPickerTriangle.TheColor;
            }
        }

        private void OnCarSwapped()
        {
            carMaterial = carInstance.CurrentCarInstance.BodyMesh.material;
            basicColor = carMaterial.color;
            colorPickerTriangle = colorPicker.GetComponent<ColorPickerTriangle>();
            colorPickerTriangle.TheColor = basicColor;
        }

        private void SetPaintMode(bool enable)
        {
            garageUiHandler.ChooseTargetPanel(buttonsHandler);
        }

        private void SetNewColor()
        {
            colorPickerTriangle.SetNewColor(carMaterial.color);
            basicColor = colorPickerTriangle.TheColor;
            carInstance.ChangeColor(basicColor);
            SetPaintMode(false);
        }

        private void CancelPaint()
        {
            colorPickerTriangle.TheColor = basicColor;
            SetPaintMode(false);
        }
    }
}