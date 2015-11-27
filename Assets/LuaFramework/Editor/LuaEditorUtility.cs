using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;

public static class LuaEditorUtility {


	[MenuItem("Assets/Lua/Create Class")]
	public static void CreateNewLuaScript(){
		string folder = GetSelectFolder();
		if(string.IsNullOrEmpty(folder)){
			Debug.LogError("Please select a folder!");
			return;
		}
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
		       ScriptableObject.CreateInstance(typeof(DoCreateLuaAsset)) as UnityEditor.ProjectWindowCallback.EndNameEditAction,
		                                                        folder+"/NewLuaScript.lua",null,
		                                                        "Assets/LuaFramework/Editor/new_lua_file_template.txt");
	}

	public static void CreatePluginMainFile(string pluginPath){
		string pluginName = Path.GetFileName(pluginPath);
		string text = File.ReadAllText("Assets/LuaFramework/Editor/new_lua_plugin_template.txt");
		text = text.Replace("#PLUGINNAME#",pluginName);
		File.WriteAllText(pluginPath+"/main.lua",text);
		AssetDatabase.Refresh(ImportAssetOptions.Default);
	}

	private static string GetSelectFolder(){
		string folder = null;
		UnityEngine.Object[] objects = Selection.GetFiltered(typeof(UnityEngine.Object),SelectionMode.TopLevel|SelectionMode.Assets);
		foreach(UnityEngine.Object o in objects){
			folder = AssetDatabase.GetAssetPath(o);
			if(Directory.Exists(folder)){
				break;
			}
		}
		return folder;
	}

	[MenuItem("Assets/Lua/Create Plugin")]
	public static void CreateLuaPlugin(){
		string folder = GetSelectFolder();
		if(string.IsNullOrEmpty(folder)){
			Debug.LogError("Please select a folder.");
			return;
		}
		Debug.Log("create plugin at :"+ folder);
		var endNameAction = ScriptableObject.CreateInstance<DoCreatePluginAsset>() as UnityEditor.ProjectWindowCallback.EndNameEditAction;
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
		                                                        endNameAction,
		                                                        folder+"/pluginName",null,
		                                                        "Assets/LuaFramework/Editor/new_lua_file_template.txt");
	}

	[MenuItem("Tools/Document_LuaFramework")]
	public static void OpenDoc(){
		Application.OpenURL("https://github.com/wlgys8/UniSLuaPF/blob/master/README.md");
	}
}
