using System.IO;
using UnityEngine;

namespace Block
{
    [CreateAssetMenu(fileName = "BlockMap", menuName = "SlimeOthello/BlockMap", order = 0)]
    public sealed class BlockMap : ScriptableObject
    {
        public string stageName;

        public BlockState[,] Map = new BlockState[3, 3];
        public int Row => Map.GetLength(1);
        public int Column => Map.GetLength(0);

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


#if UNITY_EDITOR
        private void OnValidate()
        {
            stageName = Path.GetFileNameWithoutExtension(UnityEditor.AssetDatabase.GetAssetPath(this));
        }
#endif
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

            for (int c = 0; c < instance.Column; c++)
            {
                GUILayout.BeginHorizontal();

                for (int r = 0; r < instance.Row; r++)
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
            UnityEditor.Undo.RecordObject(instance, "Change");
            UnityEditor.EditorUtility.SetDirty(instance);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(instance);
        }
    }
#endif
}
