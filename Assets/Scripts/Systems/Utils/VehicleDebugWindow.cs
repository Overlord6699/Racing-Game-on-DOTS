#if UNITY_EDITOR

using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Drift
{
    public class VehicleDebugWindow : EditorWindow
    {
        [MenuItem("Debug/Vehicle")]
        private static void ShowWindow()
        {
            var window = GetWindow<VehicleDebugWindow>();
            window.Show();
        }

        [SerializeField] 
        private WheelData[] wheels;

        public void SetWheel(int index, WheelData wheel)
        {
            if (wheels == null) 
                wheels = new WheelData[index + 1];
            else if (wheels.Length <= index)
                Array.Resize(ref wheels, index + 1);

            wheels[index] = wheel;
        }

        private Editor editor;
        
        private void OnEnable()
        {
            editor = Editor.CreateEditor(this);
        }

        private void OnGUI()
        {
            editor.OnInspectorGUI();
        }

        [Serializable]
        public struct WheelData
        {
            public float Slip;
            public float Torque;
            public float3 Impulse;
            public float SuspensionImpulse;
            public float RotationSpeed;
        }
    }
}

#endif