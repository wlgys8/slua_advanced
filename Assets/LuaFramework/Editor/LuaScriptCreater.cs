using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public static class LuaScriptCreater {



	[MenuItem("Assets/Create/Lua Script")]
	public static void CreateNewLuaScript(){
		string folder = AssetDatabase.GetAssetPath(Selection.activeObject);
		if(!Directory.Exists(folder)){
			folder = System.IO.Path.GetDirectoryName(folder);
		}
		string path = AssetDatabase.GenerateUniqueAssetPath(folder +"/NewLuaScript.lua");
		File.WriteAllText(path,"",System.Text.Encoding.UTF8);
		AssetDatabase.Refresh(ImportAssetOptions.Default);
	}

	[MenuItem("Tools/Document_LuaFramework")]
	public static void OpenDoc(){
		Application.OpenURL("https://github.com/wlgys8/UniSLuaPF/blob/master/README.md");
	}
}
