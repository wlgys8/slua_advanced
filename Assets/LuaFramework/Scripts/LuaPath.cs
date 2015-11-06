using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LuaPath  {

	private static List<string> _searchPaths = new List<string>();

	public static void AddSearchPath(string path){
		_searchPaths.Add(path);
	}

	public static void RemoveSearchPath(string path){
		_searchPaths.Remove(path);
	}

	public static List<string> paths{
		get{
			return _searchPaths;
		}
	}

	public static List<string> GetPathNames(string fileName){
		List<string> addPaths = new List<string>(paths.Count);
		foreach(string searchPath in paths){
			addPaths.Add(GetLuaPath(searchPath,fileName));
		}
		return addPaths;
	}

	private static string GetLuaPath(string searchPath,string name) {
		string lowerName = name.ToLower();
		name = name.Replace('.', '/');
		return   "Assets/"+searchPath+"/" + name ;
	}
}
