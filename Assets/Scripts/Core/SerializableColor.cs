using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class SerializableColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public SerializableColor(Color color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }

        public Color ToUnityColor()
        {
            return new Color(r, g, b, a);
        }
    }
}