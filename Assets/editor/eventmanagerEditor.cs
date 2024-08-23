using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EvenetManager))]
public class eventmanagerEditor : Editor
{
    private EvenetManager eventManager;

 
    private void OnEnable()
    {
        eventManager = (EvenetManager)target;
    }
   
    public override void OnInspectorGUI()
    {
        
 

        // ���� ������ ǥ������ ���θ� �����ϴ� ��� �߰�
        eventManager.showHandlersInScene = EditorGUILayout.Toggle("�� �信�ٰ� ǥ��", eventManager.showHandlersInScene);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Etrigger"),
          new GUIContent("Ʈ���� ������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("triggertransform"),
          new GUIContent("Ʈ���� ����"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handlertransform"),
          new GUIContent("�ڵ鷯 ����"));
        EditorGUILayout.Space();

        // �̺�Ʈ �ڵ鷯 ����Ʈ ����
        for (int i = 0; i < eventManager.eventhadles.Count; i++)
        {
            var handler = eventManager.eventhadles[i];

            // �̺�Ʈ �ڵ鷯 ���¿� ���� ���� ����
            GUIStyle style = new GUIStyle();
            if (handler.eventcomplete)
            {
                style.normal.textColor = Color.yellow;
            }
            else if (handler.evenactive)
            {
                style.normal.textColor = Color.green; // Ȱ��ȭ�� �̺�Ʈ �ڵ鷯�� ��� �۾�
            }
            else
            {
                style.normal.textColor = Color.red; // ��Ȱ��ȭ�� �̺�Ʈ �ڵ鷯�� ���� �۾�
            }


            // �̺�Ʈ �ڵ鷯 ���� ǥ��
            EditorGUILayout.BeginHorizontal();
            if (handler != null)
            {
                EditorGUILayout.LabelField($"{eventManager.eventhadles.Count} �� Handler){i}: {handler.name}", style);
                if (GUILayout.Button("Select"))
                {
                    Selection.activeGameObject = handler.gameObject;
                }
                if (GUILayout.Button("Delete"))
                {
                    if (!Application.isPlaying)
                        DestroyImmediate(handler.gameObject); // �̺�Ʈ �ڵ鷯 ����
                    else
                        Destroy(handler.gameObject);
                    eventManager.deleteeventhandler(i);
                }
            }
            else
            {
                eventManager.eventhadles.RemoveAt(i);
                i--;
                EditorGUILayout.LabelField("Handler (Destroyed)", style);
            }
            EditorGUILayout.EndHorizontal();

            // ����� ���� ������ ǥ���� ����
            //EditorGUILayout.LabelField("�ٽ� ����", EditorStyles.boldLabel);
            // ���⿡ �ٽ� ������ ǥ���� �� �ֽ��ϴ�.
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handlername"),
            new GUIContent("�ڵ鷯 �̸�"));
        // �ڵ鷯 �߰� ��ư
        if (GUILayout.Button("�ڵ鷯 �߰�"))
        {
            eventManager.addeventhandler();
        }
        //Ʈ���ŷ� ����
        // �̺�Ʈ �ڵ鷯 ����Ʈ ����
        for (int i = 0; i < eventManager.eventTriggers.Count; i++)
        {
            var trigger = eventManager.eventTriggers[i];

            
            


            // �̺�Ʈ �ڵ鷯 ���� ǥ��
            EditorGUILayout.BeginHorizontal();
            if (trigger != null)
            {
                EditorGUILayout.LabelField($"{eventManager.eventhadles.Count} �� TRigger");
                if (GUILayout.Button("Select"))
                {
                    Selection.activeGameObject = trigger.gameObject;
                }
                if (GUILayout.Button("Delete"))
                {
                    if(!Application.isPlaying)
                    DestroyImmediate(trigger.gameObject); // �̺�Ʈ �ڵ鷯 ����
                  else
                        Destroy(trigger.gameObject);
                    eventManager.deleteeventhandler(i);
                }
            }
            else
            {
                eventManager.eventTriggers.RemoveAt(i);
                i--;
                EditorGUILayout.LabelField("Trigger (Destroyed)");
            }
            EditorGUILayout.EndHorizontal();

            // ����� ���� ������ ǥ���� ����
            if (trigger != null)
            {
                EditorGUILayout.LabelField($"{trigger.name}�� ���� Ȱ��ȭ �Ǵ� �̺�Ʈ", EditorStyles.boldLabel);
                for (int n = 0; n < trigger.starthandlers.Count; n++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{n}�� {trigger.starthandlers[n].name}", EditorStyles.boldLabel);
                    if (GUILayout.Button("Select"))
                    {
                        Selection.activeGameObject = trigger.starthandlers[n].gameObject;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.LabelField($"{trigger.name}�� ���� ��Ȱ��ȭ �Ǵ� �̺�Ʈ", EditorStyles.boldLabel);
                for (int n = 0; n < trigger.stophandlers.Count; n++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{n}�� {trigger.stophandlers[n].name}", EditorStyles.boldLabel);
                    if (GUILayout.Button("Select"))
                    {
                        Selection.activeGameObject = trigger.stophandlers[n].gameObject;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
               
  
            // ���⿡ �ٽ� ������ ǥ���� �� �ֽ��ϴ�.
        }

        EditorGUILayout.Space();
 
        // �ڵ鷯 �߰� ��ư
        if (GUILayout.Button("Ʈ���� �߰�"))
        {
            eventManager.addeventTrigger();
        }

        serializedObject.ApplyModifiedProperties();
        // ���� ������ ������ ����
        if (GUI.changed)
        {
            EditorUtility.SetDirty(eventManager);
        }
    }
   
    private void OnSceneGUI()
    {
        // ���� ������ ǥ������ ���θ� Ȯ��
        if (!eventManager.showHandlersInScene) return;

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 10, 150, 100)); // UI�� ǥ�õ� ��ġ�� ũ�� ����

        GUILayout.Label("EventHandlerList", EditorStyles.boldLabel);
        // SceneView���� �̺�Ʈ �ڵ鷯 ��ġ �� ���� ǥ��
        for (int i = 0; i < eventManager.eventhadles.Count; i++)
        {
            var handler = eventManager.eventhadles[i];
            if (handler == null) continue;
            GUIStyle style = new GUIStyle();
            if (handler.eventcomplete)
            {
                style.normal.textColor = Color.yellow;
            }
            else if (handler.evenactive)
            {
                style.normal.textColor = Color.green; // Ȱ��ȭ�� �̺�Ʈ �ڵ鷯�� ��� �۾�
            }
            else
            {
                style.normal.textColor = Color.red; // ��Ȱ��ȭ�� �̺�Ʈ �ڵ鷯�� ���� �۾�
            }



            if (GUILayout.Button($"�̸�: {handler.name} (�ٽ� ����: {handler.EventCheckDelay})", style))
            {
                Selection.activeGameObject = handler.gameObject; // �ڵ鷯 ����
            }
        }
        GUILayout.EndArea();
        Handles.EndGUI();

        // SceneView���� �̺�Ʈ �ڵ鷯 ��ġ �� ���� ǥ��
        for (int i = 0; i < eventManager.eventTriggers.Count; i++)
        {
            var trigger = eventManager.eventTriggers[i];
            if (trigger == null) continue;

            Handles.color = trigger.actived ? Color.green : Color.red;
           
            Handles.Label(trigger.transform.position, $"�̸�:{trigger.name} ", EditorStyles.boldLabel);

            if (Handles.Button(trigger.transform.position, Quaternion.identity, 0.5f, 0.5f, Handles.SphereHandleCap))
            {
                Selection.activeGameObject = trigger.gameObject; // �ڵ鷯 ����
            }
            //if (trigger != null)
            //{
            //    EditorGUILayout.LabelField($"{trigger.name}�� ���� Ȱ��ȭ �Ǵ� �̺�Ʈ", EditorStyles.boldLabel);
            //    for (int n = 0; n < trigger.starthandlers.Count; n++)
            //    {
            //        EditorGUILayout.BeginHorizontal();
            //        EditorGUILayout.LabelField($"{n}�� {trigger.starthandlers[n].name}", EditorStyles.boldLabel);
            //        if (GUILayout.Button("Select"))
            //        {
            //            Selection.activeGameObject = trigger.starthandlers[n].gameObject;
            //        }
            //        EditorGUILayout.EndHorizontal();
            //    }
            //    EditorGUILayout.LabelField($"{trigger.name}�� ���� ��Ȱ��ȭ �Ǵ� �̺�Ʈ", EditorStyles.boldLabel);
            //    for (int n = 0; n < trigger.stophandlers.Count; n++)
            //    {
            //        EditorGUILayout.BeginHorizontal();
            //        EditorGUILayout.LabelField($"{n}�� {trigger.stophandlers[n].name}", EditorStyles.boldLabel);
            //        if (GUILayout.Button("Select"))
            //        {
            //            Selection.activeGameObject = trigger.stophandlers[n].gameObject;
            //        }
            //        EditorGUILayout.EndHorizontal();
            //    }
            //}
        }
    }
}
