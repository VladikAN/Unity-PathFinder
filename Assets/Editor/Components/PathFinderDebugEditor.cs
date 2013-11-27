using Assets.Script.Components.Debug;
using UnityEditor;

namespace Assets.Editor.Components
{
    [CustomEditor(typeof(PathFinderDebug))]
    public class PathFinderDebugEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Hello world");
        }
    }
}