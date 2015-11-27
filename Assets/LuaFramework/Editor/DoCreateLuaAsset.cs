using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class DoCreateLuaAsset : UnityEditor.ProjectWindowCallback.EndNameEditAction {

	public override void Action (int instanceId, string pathName, string resourceFile)
	{
		string parent = Path.GetDirectoryName(pathName);
		string name = Path.GetFileNameWithoutExtension(pathName);
		string template = File.ReadAllText(resourceFile);
		template = template.Replace("#SCRIPTNAME#",name);
		File.WriteAllText(pathName,template);
		AssetDatabase.Refresh();
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
