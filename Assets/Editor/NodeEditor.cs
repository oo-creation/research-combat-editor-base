﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Editor;


public class NodeEditor : EditorWindow {
 
    List<NodeWindow> windows = new List<NodeWindow>();
    List<int> windowsToAttach = new List<int>();
    List<int> attachedWindows = new List<int>();
    public NPCTankController _tankController;
 
    void OnGUI() {
        if (windowsToAttach.Count == 0){
            GUI.Box(new Rect(10,25,100,200), "???");
        }

        else {
            GUI.Box(new Rect(10,25,100,200), windowsToAttach[0] + " is going to be attached");
        }
        

        if (windowsToAttach.Count == 2) {
            attachedWindows.Add(windowsToAttach[0]);
            attachedWindows.Add(windowsToAttach[1]);
            windowsToAttach = new List<int>();
        }
 
        if (attachedWindows.Count >= 2) {
            for (int i = 0; i < attachedWindows.Count; i += 2) {
                DrawNodeCurve(windows[attachedWindows[i]].Rect, windows[attachedWindows[i + 1]].Rect);
            }
        }
 
        BeginWindows();
 
        if (GUILayout.Button("Create Node")) {
            windows.Add(new NodeWindow(){Rect = new Rect(110, 10, 100, 100)});
        }
 
        for (int i = 0; i < windows.Count; i++) {
            windows[i].Rect = GUI.Window(i, windows[i].Rect, DrawNodeWindow, "Window " + i);
        }
 
        EndWindows();
    }
 
 
    void DrawNodeWindow(int id) {
        if (GUILayout.Button("Attach")) {
            windowsToAttach.Add(id);
        }

        if (GUILayout.Button("Delete"))
        {
            windows.Remove(windows[id]);
            attachedWindows.Remove(id);
        }

        windows[id].State = EditorGUILayout.ObjectField(windows[id].State, typeof(MonoScript), true) as MonoScript;
 
        GUI.DragWindow();
    }
 
 
    void DrawNodeCurve(Rect start, Rect end) {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
 
        for (int i = 0; i < 3; i++) {// Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }
 
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}