using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EnemyTrackingAndPatrol))]
public class enemytapeditor : Editor
{
    EnemyTrackingAndPatrol tap;
    private void Awake()
    {
         tap = target as EnemyTrackingAndPatrol;
    }
    public void savedata()
    {
        tap.trackingdata = new EnemyTrackingDAta();
        tap.trackingdata.rangeSizeX = tap.rangeSizeX;
        tap.trackingdata.rangeSizeY = tap.rangeSizeY;
        tap.trackingdata.rangeSizeZ = tap.rangeSizeZ;
        tap.trackingdata.tapname = tap.dataname;
        tap.trackingdata.rangePosX = tap.rangePosX;
        tap.trackingdata.rangePosY = tap.rangePosY;
        tap.trackingdata.rangePosZ = tap.rangePosZ;
        tap.trackingdata.searchPosX = tap.searchPosX;
        tap.trackingdata.searchPosY = tap.searchPosY;
        tap.trackingdata.searchPosZ = tap.searchPosZ;
        tap.trackingdata.searchSizeX = tap.searchSizeX;
        tap.trackingdata.searchSizeY = tap.searchSizeY;
        tap.trackingdata.searchSizeZ = tap.searchSizeZ;
        tap.trackingdata.trackingDistance = tap.trackingDistance;
        tap.trackingdata.patrolWaitTime = tap.patrolWaitTime;
        tap.trackingdata.leftPatrolRange = tap.leftPatrolRange;
        tap.trackingdata.rightPatrolRange = tap.rightPatrolRange;
        tap.trackingdata.patrolDistance = tap.patrolDistance;
        tap.trackingdata.wallRayHeight = tap.wallRayHeight;
        tap.trackingdata.wallRayLength = tap.wallRayLength;
        tap.trackingdata.wallRayUpLength = tap.wallRayUpLength;
        tap.trackingdata.wallRayBackLength = tap.wallRayBackLength;
        AssetDatabase.CreateAsset(tap.trackingdata
            , $"Assets\\1.trackingandpatrol\\{tap.dataname}.asset");
    }
    public void loaddata()
    {
        if (tap.trackingdata != null)
        {
            tap.dataname = tap.trackingdata.tapname;
            tap.rangeSizeX = tap.trackingdata.rangeSizeX;
            tap.rangeSizeY = tap.trackingdata.rangeSizeY;
            tap.rangeSizeZ = tap.trackingdata.rangeSizeZ;
            tap.rangePosX = tap.trackingdata.rangePosX;
            tap.rangePosY = tap.trackingdata.rangePosY;
            tap.rangePosZ = tap.trackingdata.rangePosZ;
            tap.searchPosX = tap.trackingdata.searchPosX;
            tap.searchPosY = tap.trackingdata.searchPosY;
            tap.searchPosZ = tap.trackingdata.searchPosZ;
            tap.searchSizeX = tap.trackingdata.searchSizeX;
            tap.searchSizeY = tap.trackingdata.searchSizeY;
            tap.searchSizeZ = tap.trackingdata.searchSizeZ;
            tap.trackingDistance = tap.trackingdata.trackingDistance;
            tap.patrolWaitTime = tap.trackingdata.patrolWaitTime;
            tap.leftPatrolRange = tap.trackingdata.leftPatrolRange;
            tap.rightPatrolRange = tap.trackingdata.rightPatrolRange;
            tap.patrolDistance = tap.trackingdata.patrolDistance;
            tap.wallRayHeight = tap.trackingdata.wallRayHeight;
            tap.wallRayLength = tap.trackingdata.wallRayLength;
            tap.wallRayUpLength = tap.trackingdata.wallRayUpLength;
            tap.wallRayBackLength = tap.trackingdata.wallRayBackLength;
        }
    }
    public override void OnInspectorGUI()
    {
      

      
        base.OnInspectorGUI();
        if (GUILayout.Button("저장"))
        {
            savedata(); 
        }
        if (GUILayout.Button("불려오기"))
        {
            loaddata();
        }
        if (GUILayout.Button("폴더 이동"))
        {
            Object folder = AssetDatabase.LoadAssetAtPath<Object>($"Assets\\1.trackingandpatrol");
            if (folder != null)
            {
                // 해당 폴더를 선택하여 프로젝트 창에 표시
                Selection.activeObject = folder;
                EditorGUIUtility.PingObject(folder);
            }
            else
            {
                Debug.LogWarning($"폴더 경로를 찾을 수 없습니다.");
            }
        }
    }
}
