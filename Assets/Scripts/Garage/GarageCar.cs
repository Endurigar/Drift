using UnityEngine;

namespace Garage
{
    public class GarageCar : MonoBehaviour
    {
        [SerializeField] private GameObject[] wheels;
        [SerializeField] private GameObject decalHandler;
        [SerializeField] private GameObject carBody;

        public GameObject[] Wheels => wheels;

        public GameObject DecalHandler => decalHandler;

        public GameObject CarBody => carBody;
    }
}