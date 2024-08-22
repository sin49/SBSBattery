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
            EditorGUILayout.LabelField("이벤트 작동 중", EditorStyles.boldLabel);
        else
            EditorGUILayout.LabelField("이벤트 비활성화", EditorStyles.boldLabel);


        EditorGUILayout.PropertyField(
         serializedObject.FindProperty("or"), new GUIContent("OFF=AND ON=OR")
          );
        EditorGUILayout.PropertyField(
      serializedObject.FindProperty("startonawake"), new GUIContent("실행시 바로 동작")
       );
        if(GUILayout.Button("이벤트 실행"))
        {
            eventHandler. stopevent();
            eventHandler. startevent();
        }
        EditorGUILayout.PropertyField(
           serializedObject.FindProperty("EventCheckDelay"), new GUIContent("이벤트 체크 간격")
            );

        EditorGUILayout.PropertyField(
         serializedObject.FindProperty("loop"), new GUIContent("이벤트 루프 체크")
          );
        if (eventHandler.loop)
        {
            EditorGUILayout.PropertyField(
          serializedObject.FindProperty("EventDisabletimer"), new GUIContent("이벤트를 다시 활성화하기 까지의 딜레이")
           );
        }

        // inputevents 리스트 편집
        EditorGUILayout.LabelField("Input Events", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(
           serializedObject.FindProperty("inputenum"), new GUIContent("입력 이벤트 종류")
            );
        EditorGUILayout.PropertyField(
      serializedObject.FindProperty("inputeventname"), new GUIContent("입력 이벤트 이름")
       );
        //// 리스트에 새 이벤트 추가
        if (GUILayout.Button("입력 이벤트 추가"))
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
                    labelname += "(조건 만족)";
                }
                else
                {
                    labelname += "(조건 불만족)";
                }
                // 이벤트 이름 표시
                EditorGUILayout.LabelField(labelname);

                // 삭제 버튼 추가
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    // inputevents 리스트에서 요소 제거
                    var eventToDelete = eventHandler.inputevents[i];
                    eventHandler.DeleteInputEvent(i);
                    if (!Application.isPlaying)
                    {
                        // 관련된 컴포넌트도 제거
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
        // outputevents 리스트 편집
        EditorGUILayout.LabelField("output Events", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(
           serializedObject.FindProperty("outputenum"), new GUIContent("출력 이벤트 종류")
            );
        EditorGUILayout.PropertyField(
      serializedObject.FindProperty("outputeventname"), new GUIContent("출력 이벤트 이름")
       );
        //// 리스트에 새 이벤트 추가
        if (GUILayout.Button("출력 이벤트 추가"))
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
                    labelname += "(실행됨)";
                }
                // 이벤트 이름 표시
                EditorGUILayout.LabelField(labelname);
               
                // 삭제 버튼 추가
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    // outputevents 리스트에서 요소 제거
                    var eventToDelete = eventHandler.outputevents[i];
                    eventHandler.DeleteoutputEvent(i);
                    if (!Application.isPlaying)
                    {
                        // 관련된 컴포넌트도 제거
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


        // 변경사항을 저장하고 업데이트
        if (GUI.changed)
        {
            EditorUtility.SetDirty(eventHandler);
        }
    }
}
