using System.Collections.Generic;
using UnityEngine;

namespace Mods
{
    [CreateAssetMenu(fileName = "CarList", menuName = "Cars", order = 0)]
    public class CarList : ScriptableObject
    {
        [SerializeField] private List<GameObject> cars = new List<GameObject>();

        public List<GameObject> Cars => cars;
    }
}