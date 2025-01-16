#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public static class EditorThumbnailGenerator
    {
        public static void SaveThumbnail(Texture2D thumbnail, string path)
        {
            byte[] bytes = thumbnail.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            Debug.Log("Thumbnail saved to: " + path);
        }
        public static Texture2D GetThumbnail(GameObject prefab)
        {
            return AssetPreview.GetAssetPreview(prefab);
        }
    }
}
#endif
