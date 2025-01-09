using System;
using UnityEditor;
using UnityEngine;

namespace Mods
{
    public abstract class CarMod : MonoBehaviour, ICarMod
    {
        [SerializeField] private string id;
        [SerializeField] private Texture2D thumbnail;

        string ICarMod.Id
        {
            get => id;
            set => id = value;
        }

        public Texture2D Thumbnail => thumbnail;
#if UNITY_EDITOR
        private void Reset()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = System.Guid.NewGuid().ToString();
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}