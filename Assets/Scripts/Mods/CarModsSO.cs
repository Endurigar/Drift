using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Mods
{
    [CreateAssetMenu(fileName = "CarMods", menuName = "Mods", order = 0)]
    public class CarModsSo : ScriptableObject
    {
        [SerializeField] private List<CarMod> carMods;
        [SerializeField]private List<WheelData> carPrefabs;

        public List<WheelData> CarPrefabs => carPrefabs;
        public ICarMod GetModById(string id)
        {
           return carMods.FirstOrDefault(element => ((ICarMod)element).Id == id);
        }

        public T[] GetAllModByType<T>() where T : ICarMod
        {
            return carMods.OfType<T>().ToArray();
        }
        
        public T[] GetAllModByTypeFromList<T>(List<string> ids) where T : ICarMod
        {
            return carMods.Where(element => ids.Contains(((ICarMod)element).Id)).OfType<T>().ToArray();
        }
    }
}