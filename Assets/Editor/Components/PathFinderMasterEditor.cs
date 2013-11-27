using Assets.Script.Components.Master;
using UnityEditor;

namespace Assets.Editor.Components
{
    [CustomEditor(typeof(PathFinderMaster))]
    public class PathFinderMasterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Hello world");
        }
    }
}