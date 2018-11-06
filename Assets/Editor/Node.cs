using System;
using UnityEditor;
using UnityEngine;

public class Node
{
    public MonoScript state;
    public Rect rect;
    public Rect staterect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<Node> OnRemoveNode;
    public Action<FSMState> OnStateAdd;
    public Action<FSMState> OnStateRemove;

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint,
        Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, Action<FSMState> onStateAdd, Action<FSMState> onStateRemove)
    {
        rect = new Rect(position.x, position.y, width, height);
        staterect = new Rect(position.x + width * 0.15f, position.y + height * 0.1f, width - width * 0.2f,
            height - height * 0.2f);
        style = nodeStyle;
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
        OnStateAdd = onStateAdd;
        OnStateRemove = onStateRemove;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
        staterect.position += delta;
    }

    public void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();

        GUI.Box(rect, title, style);

        var oldstate = state;
        state = EditorGUI.ObjectField(staterect, state, typeof(MonoScript), true) as MonoScript;
        if (oldstate != state)
        {
            if (state != null && state is FSMState)
            {
                state = oldstate;
                throw new ArgumentException("State is not a FSM State");
            }
        }
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }

                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }

                break;
        }

        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }
}