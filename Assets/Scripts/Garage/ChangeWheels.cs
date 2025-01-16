using Garage.GarageUI;
using Mods;
using UnityEngine;

namespace Garage
{
    public class ChangeWheels : MonoBehaviour
    {
        [SerializeField] private GameObject[] wheels;
        [SerializeField] private Car car;
        [SerializeField] private CarModsSo carModsSo;
        [SerializeField] private ChangeWheelsButtonsSpawner changeWheelsButtonsSpawner;
        [SerializeField] private GameObject wheelsList;
        [SerializeField] private CarSwapper carSwapper;

        private Car carData;

        private void Awake()
        {
            changeWheelsButtonsSpawner.OnWheelSelected += ChangeWheel;
        }

        private void ChangeWheel(string id)
        {
            var wheel = carModsSo.GetModById(id) as Mods.Wheel;
            car.ChangeWheel(id);
            for (int i = 0; i < car.CurrentCarInstance.Wheels.Length; i++)
            {
                var currentWheel = car.CurrentCarInstance.Wheels[i].WheelMesh.gameObject;
                var position = currentWheel.transform.position;
                var rotation = currentWheel.transform.rotation;
                var scale = currentWheel.transform.localScale;
                var newWheel = Instantiate(wheel, position, rotation, currentWheel.transform.parent);
                newWheel.transform.localScale = scale;
                Destroy(currentWheel);
                car.CurrentCarInstance.Wheels[i].WheelMesh = newWheel.transform;
            }
        }
    }
}