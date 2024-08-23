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
        
 

        // 씬에 정보를 표시할지 여부를 제어하는 토글 추가
        eventManager.showHandlersInScene = EditorGUILayout.Toggle("씬 뷰에다가 표시", eventManager.showHandlersInScene);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Etrigger"),
          new GUIContent("트리거 프리팹"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("triggertransform"),
          new GUIContent("트리거 모음"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handlertransform"),
          new GUIContent("핸들러 모음"));
        EditorGUILayout.Space();

        // 이벤트 핸들러 리스트 관리
        for (int i = 0; i < eventManager.eventhadles.Count; i++)
        {
            var handler = eventManager.eventhadles[i];

            // 이벤트 핸들러 상태에 따른 색상 설정
            GUIStyle style = new GUIStyle();
            if (handler.eventcomplete)
            {
                style.normal.textColor = Color.yellow;
            }
            else if (handler.evenactive)
            {
                style.normal.textColor = Color.green; // 활성화된 이벤트 핸들러는 녹색 글씨
            }
            else
            {
                style.normal.textColor = Color.red; // 비활성화된 이벤트 핸들러는 빨간 글씨
            }


            // 이벤트 핸들러 정보 표시
            EditorGUILayout.BeginHorizontal();
            if (handler != null)
            {
                EditorGUILayout.LabelField($"{eventManager.eventhadles.Count} 번 Handler){i}: {handler.name}", style);
                if (GUILayout.Button("Select"))
                {
                    Selection.activeGameObject = handler.gameObject;
                }
                if (GUILayout.Button("Delete"))
                {
                    if (!Application.isPlaying)
                        DestroyImmediate(handler.gameObject); // 이벤트 핸들러 삭제
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

            // 사용자 지정 정보를 표시할 공간
            //EditorGUILayout.LabelField("핵심 변수", EditorStyles.boldLabel);
            // 여기에 핵심 변수를 표시할 수 있습니다.
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handlername"),
            new GUIContent("핸들러 이름"));
        // 핸들러 추가 버튼
        if (GUILayout.Button("핸들러 추가"))
        {
            eventManager.addeventhandler();
        }
        //트리거로 개조
        // 이벤트 핸들러 리스트 관리
        for (int i = 0; i < eventManager.eventTriggers.Count; i++)
        {
            var trigger = eventManager.eventTriggers[i];

            
            


            // 이벤트 핸들러 정보 표시
            EditorGUILayout.BeginHorizontal();
            if (trigger != null)
            {
                EditorGUILayout.LabelField($"{eventManager.eventhadles.Count} 번 TRigger");
                if (GUILayout.Button("Select"))
                {
                    Selection.activeGameObject = trigger.gameObject;
                }
                if (GUILayout.Button("Delete"))
                {
                    if(!Application.isPlaying)
                    DestroyImmediate(trigger.gameObject); // 이벤트 핸들러 삭제
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

            // 사용자 지정 정보를 표시할 공간
            if (trigger != null)
            {
                EditorGUILayout.LabelField($"{trigger.name}을 통해 활성화 되는 이벤트", EditorStyles.boldLabel);
                for (int n = 0; n < trigger.starthandlers.Count; n++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{n}번 {trigger.starthandlers[n].name}", EditorStyles.boldLabel);
                    if (GUILayout.Button("Select"))
                    {
                        Selection.activeGameObject = trigger.starthandlers[n].gameObject;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.LabelField($"{trigger.name}을 통해 비활성화 되는 이벤트", EditorStyles.boldLabel);
                for (int n = 0; n < trigger.stophandlers.Count; n++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{n}번 {trigger.stophandlers[n].name}", EditorStyles.boldLabel);
                    if (GUILayout.Button("Select"))
                    {
                        Selection.activeGameObject = trigger.stophandlers[n].gameObject;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
               
  
            // 여기에 핵심 변수를 표시할 수 있습니다.
        }

        EditorGUILayout.Space();
 
        // 핸들러 추가 버튼
        if (GUILayout.Button("트리거 추가"))
        {
            eventManager.addeventTrigger();
        }

        serializedObject.ApplyModifiedProperties();
        // 변경 사항이 있으면 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(eventManager);
        }
    }
   
    private void OnSceneGUI()
    {
        // 씬에 정보를 표시할지 여부를 확인
        if (!eventManager.showHandlersInScene) return;

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 10, 150, 100)); // UI가 표시될 위치와 크기 설정

        GUILayout.Label("EventHandlerList", EditorStyles.boldLabel);
        // SceneView에서 이벤트 핸들러 위치 및 정보 표시
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
                style.normal.textColor = Color.green; // 활성화된 이벤트 핸들러는 녹색 글씨
            }
            else
            {
                style.normal.textColor = Color.red; // 비활성화된 이벤트 핸들러는 빨간 글씨
            }



            if (GUILayout.Button($"이름: {handler.name} (핵심 변수: {handler.EventCheckDelay})", style))
            {
                Selection.activeGameObject = handler.gameObject; // 핸들러 선택
            }
        }
        GUILayout.EndArea();
        Handles.EndGUI();

        // SceneView에서 이벤트 핸들러 위치 및 정보 표시
        for (int i = 0; i < eventManager.eventTriggers.Count; i++)
        {
            var trigger = eventManager.eventTriggers[i];
            if (trigger == null) continue;

            Handles.color = trigger.actived ? Color.green : Color.red;
           
            Handles.Label(trigger.transform.position, $"이름:{trigger.name} ", EditorStyles.boldLabel);

            if (Handles.Button(trigger.transform.position, Quaternion.identity, 0.5f, 0.5f, Handles.SphereHandleCap))
            {
                Selection.activeGameObject = trigger.gameObject; // 핸들러 선택
            }
            //if (trigger != null)
            //{
            //    EditorGUILayout.LabelField($"{trigger.name}을 통해 활성화 되는 이벤트", EditorStyles.boldLabel);
            //    for (int n = 0; n < trigger.starthandlers.Count; n++)
            //    {
            //        EditorGUILayout.BeginHorizontal();
            //        EditorGUILayout.LabelField($"{n}번 {trigger.starthandlers[n].name}", EditorStyles.boldLabel);
            //        if (GUILayout.Button("Select"))
            //        {
            //            Selection.activeGameObject = trigger.starthandlers[n].gameObject;
            //        }
            //        EditorGUILayout.EndHorizontal();
            //    }
            //    EditorGUILayout.LabelField($"{trigger.name}을 통해 비활성화 되는 이벤트", EditorStyles.boldLabel);
            //    for (int n = 0; n < trigger.stophandlers.Count; n++)
            //    {
            //        EditorGUILayout.BeginHorizontal();
            //        EditorGUILayout.LabelField($"{n}번 {trigger.stophandlers[n].name}", EditorStyles.boldLabel);
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
