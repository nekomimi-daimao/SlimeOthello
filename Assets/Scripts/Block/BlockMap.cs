using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Block
{
    [CreateAssetMenu(fileName = "BlockMap", menuName = "SlimeOthello/BlockMap", order = 1)]
    [Serializable]
    public sealed class BlockMap : ScriptableObject
    {
        public BlockState[,] Map = new BlockState[3, 3];
        public int Row => Map.GetLength(1);
        public int Column => Map.GetLength(0);

        [HideInInspector]
        public List<BlockState> serialized = new(9);

        [HideInInspector]
        public int serializedColumn = 3;

        [HideInInspector]
        public int serializedRow = 3;

        public void ChangeMapSize(int column = 0, int row = 0)
        {
            if (column == 0)
            {
                column = Column;
            }

            if (row == 0)
            {
                row = Row;
            }

            if (column % 2 != 1 || row % 2 != 1)
            {
                return;
            }

            var nextMap = new BlockState[column, row];

            var copyC = column > Column ? Column : column;
            var copyR = row > Row ? Row : row;

            for (var c = 0; c < copyC; c++)
            {
                for (var r = 0; r < copyR; r++)
                {
                    nextMap[c, r] = Map[c, r];
                }
            }

            Map = nextMap;
        }

        private void OnEnable()
        {
            DeserializeMap();
        }

        internal void SerializeMap()
        {
            serializedColumn = Column;
            serializedRow = Row;

            serialized.Clear();
            foreach (var blockState in Map)
            {
                serialized.Add(blockState);
            }
        }

        internal void DeserializeMap()
        {
            Map = new BlockState[serializedColumn, serializedRow];
            var index = 0;
            var limit = serialized.Count;

            for (var c = 0; c < serializedColumn; c++)
            {
                for (var r = 0; r < serializedRow; r++)
                {
                    if (index >= limit)
                    {
                        return;
                    }

                    Map[c, r] = serialized[index];
                    index++;
                }
            }
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(BlockMap))]
    public class BlockMapInspector : UnityEditor.Editor
    {
        private BlockMap instance;

        private int nextRow;
        private int nextColumn;

        private void OnEnable()
        {
            instance = target as BlockMap;
            if (instance == null)
            {
                return;
            }

            instance.DeserializeMap();
            nextRow = instance.Row;
            nextColumn = instance.Column;
        }

        private readonly string[] _blockCap = { "-", "□", "■", };

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UnityEditor.EditorGUILayout.Space();

            UnityEditor.EditorGUILayout.Vector2IntField("Current", new Vector2Int(instance.Row, instance.Column));

            UnityEditor.EditorGUILayout.Space();

            for (var c = instance.Column - 1; c >= 0; c--)
            {
                GUILayout.BeginHorizontal();

                for (var r = 0; r < instance.Row; r++)
                {
                    var recent = instance.Map[c, r];
                    var next = (BlockState)UnityEditor.EditorGUILayout.Popup((int)recent, _blockCap);
                    if (recent == next)
                    {
                        continue;
                    }

                    instance.Map[c, r] = next;
                    Save();
                }

                GUILayout.EndHorizontal();
            }

            UnityEditor.EditorGUILayout.Space();

            nextColumn = UnityEditor.EditorGUILayout.IntField("ChangeColumn", nextColumn);
            nextRow = UnityEditor.EditorGUILayout.IntField("ChangeRow", nextRow);

            if (GUILayout.Button(nameof(instance.ChangeMapSize)))
            {
                instance.ChangeMapSize(nextColumn, nextRow);
                Save();
            }
        }

        private void Save()
        {
            instance.SerializeMap();
            UnityEditor.EditorUtility.SetDirty(instance);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
#endif
}
