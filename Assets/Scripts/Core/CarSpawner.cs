using Mods;
using UnityEngine;

namespace Core
{
   public class CarSpawner : MonoBehaviour
   {
      [SerializeField] private Car car;

      private void Awake()
      {
         car?.ChangeCar(PlayerPrefs.GetInt("CurrentCarIndex"));
      }
   }
}
