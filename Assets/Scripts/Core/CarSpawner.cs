using System;
using System.Collections;
using System.Collections.Generic;
using Mods;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
   [SerializeField] private Car car;

   private void Awake()
   {
      car?.ChangeCar(PlayerPrefs.GetInt("CurrentCarIndex"));
   }
   
}
