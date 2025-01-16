using UnityEngine;

namespace Core
{
    public class WheelData : MonoBehaviour
    {
        [SerializeField] private MeshRenderer bodyMesh;
        [SerializeField] private Wheel[] wheels;
        [SerializeField] private GameObject decalHandler;
        public MeshRenderer BodyMesh => bodyMesh;
        public GameObject DecalHandler => decalHandler;
        public Wheel[] Wheels => wheels;
    }
}