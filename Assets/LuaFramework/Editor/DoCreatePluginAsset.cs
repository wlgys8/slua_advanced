using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class DoCreatePluginAsset : UnityEditor.ProjectWindowCallback.EndNameEditAction {

	public override void Action (int instanceId, string pathName, string resourceFile)
	{

		string parent = Path.GetDirectoryName(pathName);
		string name = Path.GetFileName(pathName);
		AssetDatabase.CreateFolder(parent,name);
		LuaEditorUtility.CreatePluginMainFile(pathName);
	}
	
	public override void CleanUp ()
	{
		base.CleanUp ();
	}
	
	public override void OnEnable ()
	{
		base.OnEnable ();
	}
}
