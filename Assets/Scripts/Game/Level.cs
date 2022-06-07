using System;
using System.IO;
using Block;
using Slime;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Level", menuName = "SlimeOthello/Level", order = 0)]
    [Serializable]
    public sealed class Level : ScriptableObject
    {
        public string levelName;

        public BlockMap blockMap;

        public SlimePopulation slimePopulation;

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (string.IsNullOrEmpty(levelName))
            {
                levelName = Path.GetFileNameWithoutExtension(UnityEditor.AssetDatabase.GetAssetPath(this));
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        }
#endif
    }
}
