using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(abstractDungeonGenerator),true)]
public class RandomDungeonGenerator : Editor
{
    abstractDungeonGenerator generator ;
    private void Awake(){
        generator =(abstractDungeonGenerator)target;
    }
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        if(GUILayout.Button("create dungeon")){
            generator.GenerateDungeon();
        }
    }
}
