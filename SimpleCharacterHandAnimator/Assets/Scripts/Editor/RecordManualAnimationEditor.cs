using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace SimpleCharacterHandAnimator
{
    [CustomEditor(typeof(RecordManualAnimation))]
    public class RecordManualAnimationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RecordManualAnimation recordTarget = (RecordManualAnimation)target;

            GUILayout.Space(20);

          

            if (recordTarget.isReadyToRecordFrame)
            {
                GUILayout.Label("Select option");

                if (GUILayout.Button("Capture Frames"))
                {
                    recordTarget.CaptureFrames();
                }


                GUILayout.Space(30);


                if (recordTarget.framesCaptured > 0)
                {
                    if (GUILayout.Button("Save Recording"))
                    {
                        recordTarget.SaveFile();
                    }
                }
            }

        }
    }
}
