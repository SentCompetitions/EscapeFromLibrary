using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


[CustomEditor(typeof(QuizLock))]
[CanEditMultipleObjects]
public class QuizLockEditor : Editor
{
    QuizLock _quizLock;
    SerializedProperty _script;

    SerializedProperty _useAnimation;
    SerializedProperty _animationTriggerName;

    SerializedProperty _question;
    SerializedProperty _quizType;

    SerializedProperty _quizSelectQuestions;
    SerializedProperty _writeQuestionRightAnswer;

    SerializedProperty _quizSelectPrefab;
    SerializedProperty _answerPrefab;

    private void OnEnable()
    {
        _quizLock = (QuizLock) target;
        _script = serializedObject.FindProperty("m_Script");

        _useAnimation = serializedObject.FindProperty("useAnimation");
        _animationTriggerName = serializedObject.FindProperty("animationTriggerName");

        _question = serializedObject.FindProperty("question");
        _quizType = serializedObject.FindProperty("quizType");

        _quizSelectQuestions = serializedObject.FindProperty("quizSelectQuestions");
        _writeQuestionRightAnswer = serializedObject.FindProperty("writeQuestionRightAnswer");

        _quizSelectPrefab = serializedObject.FindProperty("quizSelectPrefab");
        _answerPrefab = serializedObject.FindProperty("answerPrefab");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.PropertyField(_script, true);
        GUI.enabled = true;

        EditorGUILayout.PropertyField(_useAnimation);
        if (_useAnimation.boolValue)
        {
            EditorGUILayout.PropertyField(_animationTriggerName);
        }

        EditorGUILayout.PropertyField(_question);
        EditorGUILayout.PropertyField(_quizType);

        switch (_quizLock.quizType)
        {
            case QuizType.Select:
                EditorGUILayout.PropertyField(_quizSelectQuestions);
                break;
            case QuizType.Write:
                EditorGUILayout.PropertyField(_writeQuestionRightAnswer, new GUIContent("Right answer"));
                break;
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_quizSelectPrefab);
        EditorGUILayout.PropertyField(_answerPrefab);

        serializedObject.ApplyModifiedProperties();
    }
}
