using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(LuaManager))]
public class LuaManagerEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		LuaManager manager = target as LuaManager;
		_includeFoldout = EditorGUILayout.Foldout(_includeFoldout,"Ignore Plugins");
		if(_includeFoldout){
			foreach(string plugin in _plugins){
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(10);
				GUI.changed = false;
				bool include = !manager.ignorePlugins.Contains(plugin);
				bool include_new =  EditorGUILayout.Toggle(plugin,include);
				if(GUI.changed){
					if(include_new){
						manager.ignorePlugins.Remove(plugin);
					}else{
						manager.ignorePlugins.Add(plugin);
					}
				}
				EditorGUILayout.EndHorizontal();
			}

			var original = GUI.color;
			GUI.color = Color.red;
			foreach(string lostPlugin in _lostPlugins){
				bool needbreak = false;
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(10);
				EditorGUILayout.LabelField(lostPlugin,EditorStyles.whiteLabel,GUILayout.Width(110));
				if(GUILayout.Button("X",GUILayout.Width(20))){
					needbreak = true;
					_lostPlugins.Remove(lostPlugin);
					manager.ignorePlugins.Remove(lostPlugin);
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				if( needbreak) break;
			}
			GUI.color = original;
		}

	}
	private bool _includeFoldout = false;
	private string[] _plugins =  null;
	private List<string> _lostPlugins = null;

	void OnEnable(){
		LuaManager manager = target as LuaManager;
		_plugins = manager.GetPluginList(true);

		List<string> lostPlugins = new List<string>();
		lostPlugins.AddRange(manager.ignorePlugins);
		lostPlugins.RemoveAll(delegate(string obj) {
			foreach(string plugin in _plugins){
				if(plugin == obj){
					return true;
				}
			}
			return false;
		});
		_lostPlugins = lostPlugins;
	}

}
