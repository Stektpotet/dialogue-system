using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class FlowEditor : EditorWindow
{

	private static class Styles
	{
		public static GUIContent nodeLabel = new GUIContent("Nodes");
		public static GUIContent splineLabel = new GUIContent("Spline");

		public static Color endPointElementBackground = new Color(0.3f, 0.3f, 0.7f);
		public static Color elementBackground = Color.gray;
		public static Color selectedElementBackground = new Color(0.3f, 0.3f, 1);

		public static Color controlPointColor = Color.blue;
		public static Color splineColor = Color.green;

		public static Color guiColor = GUI.color;

		public static GUIStyle visibilityToggle = "VisibilityToggle";
		public static GUIStyle bg = "flow background";
		public static GUIStyle node = "flow node 0";
	}
	private Vector2 scrollPosition = Vector2.one*100;
	private Rect scrollViewRect = new Rect(10, 10, 100, 100);
	private Vector2 size = new Vector2(100, 40);
	internal Rect defaultNodeRect { get { return new Rect(Vector2.one * 100, size); } }
	

	internal List<FlowNode> nodes = new List<FlowNode>();
	

	void OnGUI()
	{
		GUI.Box(scrollViewRect, GUIContent.none);
		Handles.DrawLine(scrollPosition, scrollPosition + Vector2.up);

		//scrollPosition = GUI.BeginScrollView(scrollViewRect, scrollPosition, position,true, true);
		BeginWindows();
		int i = 0;
		foreach(FlowNode s in nodes)
		{
			i++;
			s.windowRect = GUI.Window(i, s.windowRect, /*SHiet what do I do now, I gotta obtain the default property-drawing*/DrawStuff, "What", Styles.node);
		}
		EndWindows();
		//GUI.EndScrollView(true);
	}

	void DrawStuff(int i)
	{
		//Obtain the drawer-delegate... somehow
	}

	[CustomPropertyDrawer(typeof(FlowNode), true)]
	internal class FlowNodeDrawer<T> : PropertyDrawer where T : FlowNode
	{
		
		public static class Defaults
		{
			public static NodeDraw onDrawNode = (T node) => { GUI.Button(node.windowRect, "TESTING"); };
		}
		public delegate void NodeDraw(T node);

		public NodeDraw onDrawNode = Defaults.onDrawNode;

		T node;
		public FlowNodeDrawer(T node)
		{

		}
		
		public void DrawNode(int i)
		{
			onDrawNode.Invoke(node);
			GUI.DragWindow();
		}
	}

	internal class FlowNode
	{
		public int id;
		public Rect windowRect;
		public FlowNode(Rect windowRect, int id)
		{
			this.windowRect = windowRect;
			this.id = id;
		}
	}

	//internal class FlowNode<T> : FlowNode where T : Object
	//{
	//	public T item;
	//	public FlowNode(Rect windowRect, T item) : base(windowRect)
	//	{
	//		this.item = item;
	//	}

	//	public delegate void DrawNode(int index/*, bool focused, bool dragging*/);
	//	public DrawNode onDrawNode;		
	//}
}
