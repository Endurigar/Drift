using System;
using System.Collections.Generic;
using Core;

namespace Mods
{
     [Serializable]
     public class CarData
     {
          public List<string> CarMods = new List<string>();
          public SerializableColor Color = new (UnityEngine.Color.white);
          public Dictionary<string, DecalTransformData> DecalTransformDatas = new();
     }
}
