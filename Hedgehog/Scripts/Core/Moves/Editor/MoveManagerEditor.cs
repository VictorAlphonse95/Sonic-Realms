﻿using System.Linq;
using Hedgehog.Core.Utils.Editor;
using UnityEditor;
using UnityEngine;

namespace Hedgehog.Core.Moves.Editor
{
    [CustomEditor(typeof(MoveManager))]
    public class MoveManagerEditor : UnityEditor.Editor
    {
        protected bool ShowEventsFoldout
        {
            get { return EditorPrefs.GetBool("MoveManagerEditor.ShowEventsFoldout", false); }
            set { EditorPrefs.SetBool("MoveManagerEditor.ShowEventsFoldout", value); }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (targets.Count() == 1)
            {
                var moveManager = target as MoveManager;
                moveManager.GetComponents(moveManager.Moves);
            }

            HedgehogEditorGUIUtility.DrawProperties(serializedObject,
                "Controller", "Moves");

            ShowEventsFoldout = EditorGUILayout.Foldout(ShowEventsFoldout, "Events");
            if (ShowEventsFoldout)
            {
                HedgehogEditorGUIUtility.DrawProperties(serializedObject,
                    "OnPerform", "OnEnd", "OnAvailable", "OnUnavailable", "OnInterrupted");
            }

            var enabled = GUI.enabled;
            GUI.enabled = Application.isPlaying;

            HedgehogEditorGUIUtility.DrawProperties(serializedObject,
                "ActiveMoves",
                "AvailableMoves",
                "UnavailableMoves");

            GUI.enabled = enabled;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
