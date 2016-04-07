using UnityEngine;
using UnityEditor;

public class DialogueCreator : FlowEditor
{
	void OnEnable()
	{
			nodes.Add(new FlowNode(defaultNodeRect, nodes.Count));
			nodes[0].onDrawNode += DrawStuff;
	}
	
	[MenuItem("Window/Dialogue Creator")]
	static void Init()
	{
		DialogueCreator window = GetWindow<DialogueCreator>("Dialogue",true,typeof(SceneView));
		window.Show();
	}

#region DrawNodeCallbacks

	public void DrawStuff(Rect r, int id)
	{
		if (GUI.Button(new Rect(r.x+5,r.y-r.height*0.5f, r.width-10, r.height * 0.5f), "WHOAAAA!"))
		{ Debug.Log("whaaat"); }
	}
#endregion
}
