using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(NPCTankController))]
    public class CustomNPCTankControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            NPCTankController tankController = (NPCTankController) target;
            
            if (GUILayout.Button("Open FSM Editor"))
            {
                NodeBasedEditor editor = EditorWindow.GetWindow<NodeBasedEditor>();
                editor.TankController = tankController;
            }

            DrawDefaultInspector();
        }
    }
}