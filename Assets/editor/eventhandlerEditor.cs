using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EventHandler))]
public class eventhandlerEditor : Editor
{
    EventHandler eventHandler;
    private void Awake()
    {

         
    }
    public override void OnInspectorGUI()
    {
        if (eventHandler == null)
            eventHandler = (EventHandler)target;

        if(eventHandler.evenactive)
            EditorGUILayout.LabelField("�̺�Ʈ �۵� ��", EditorStyles.boldLabel);
        else
            EditorGUILayout.LabelField("�̺�Ʈ ��Ȱ��ȭ", EditorStyles.boldLabel);


        EditorGUILayout.PropertyField(
         serializedObject.FindProperty("or"), new GUIContent("OFF=AND ON=OR")
          );
        EditorGUILayout.PropertyField(
      serializedObject.FindProperty("startonawake"), new GUIContent("����� �ٷ� ����")
       );
        if(GUILayout.Button("�̺�Ʈ ����"))
        {
            eventHandler. stopevent();
            eventHandler. startevent();
        }
        EditorGUILayout.PropertyField(
           serializedObject.FindProperty("EventCheckDelay"), new GUIContent("�̺�Ʈ üũ ����")
            );

        EditorGUILayout.PropertyField(
         serializedObject.FindProperty("loop"), new GUIContent("�̺�Ʈ ���� üũ")
          );
        if (eventHandler.loop)
        {
            EditorGUILayout.PropertyField(
          serializedObject.FindProperty("EventDisabletimer"), new GUIContent("�̺�Ʈ�� �ٽ� Ȱ��ȭ�ϱ� ������ ������")
           );
        }

        // inputevents ����Ʈ ����
        EditorGUILayout.LabelField("Input Events", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(
           serializedObject.FindProperty("inputenum"), new GUIContent("�Է� �̺�Ʈ ����")
            );
        EditorGUILayout.PropertyField(
      serializedObject.FindProperty("inputeventname"), new GUIContent("�Է� �̺�Ʈ �̸�")
       );
        //// ����Ʈ�� �� �̺�Ʈ �߰�
        if (GUILayout.Button("�Է� �̺�Ʈ �߰�"))
        {
            eventHandler.CreateInputEvent();
        }
        if (eventHandler.inputevents != null)
        {
            for (int i = 0; i < eventHandler.inputevents.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                string labelname = eventHandler.inputevents[i].eventname;
                if (eventHandler.inputevents[i].input())
                {
                    labelname += "(���� ����)";
                }
                else
                {
                    labelname += "(���� �Ҹ���)";
                }
                // �̺�Ʈ �̸� ǥ��
                EditorGUILayout.LabelField(labelname);

                // ���� ��ư �߰�
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    // inputevents ����Ʈ���� ��� ����
                    var eventToDelete = eventHandler.inputevents[i];
                    eventHandler.DeleteInputEvent(i);
                    if (!Application.isPlaying)
                    {
                        // ���õ� ������Ʈ�� ����
                        DestroyImmediate(eventToDelete);
                    }
                    else
                    {
                        Destroy(eventToDelete);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }
        // outputevents ����Ʈ ����
        EditorGUILayout.LabelField("output Events", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(
           serializedObject.FindProperty("outputenum"), new GUIContent("��� �̺�Ʈ ����")
            );
        EditorGUILayout.PropertyField(
      serializedObject.FindProperty("outputeventname"), new GUIContent("��� �̺�Ʈ �̸�")
       );
        //// ����Ʈ�� �� �̺�Ʈ �߰�
        if (GUILayout.Button("��� �̺�Ʈ �߰�"))
        {
            eventHandler.CreateOutputEvent();
        }
        if (eventHandler.outputevents != null)
        {
            for (int i = 0; i < eventHandler.outputevents.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                string labelname = eventHandler.outputevents[i].eventname;
                if (eventHandler.outputevents[i].actived)
                {
                    labelname += "(�����)";
                }
                // �̺�Ʈ �̸� ǥ��
                EditorGUILayout.LabelField(labelname);
               
                // ���� ��ư �߰�
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    // outputevents ����Ʈ���� ��� ����
                    var eventToDelete = eventHandler.outputevents[i];
                    eventHandler.DeleteoutputEvent(i);
                    if (!Application.isPlaying)
                    {
                        // ���õ� ������Ʈ�� ����
                        DestroyImmediate(eventToDelete);
                    }
                    else
                    {
                        Destroy(eventToDelete);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }


        // ��������� �����ϰ� ������Ʈ
        if (GUI.changed)
        {
            EditorUtility.SetDirty(eventHandler);
        }
    }
}
