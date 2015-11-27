using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using SLua;
using LuaInterface;

[SLua.CustomLuaClass]
public class LuaPluginUtil  {


	public static bool debug = false;


	public static string[] GetPluginList(){
		return LuaManager.Instance.GetPluginList();
	}

	public static bool ExistFile(string pluginName,string fileName){
		if(fileName.EndsWith(".lua") || fileName.EndsWith(".txt")){
			fileName = fileName.Substring(0,fileName.Length-4);
		}
		fileName = fileName.Replace(".","/");
		return LuaManager.Instance.ExistFile(pluginName,fileName);
	}

	public static void ExtractUrl(string url,out string plug,out string path){
		if(!url.StartsWith("plug://")){
			plug = "";
			path = url;
			return;
		}
		url = url.Substring(7);
		var idx = url.IndexOf('/');
		plug = url.Substring(0,idx);
		path = url.Substring(idx +1);
	}

	public static bool ExistFile(string url){
		string plug;
		string path;
		ExtractUrl(url,out plug,out path);
		if(string.IsNullOrEmpty(plug)){
			return false;
		}else{
			return ExistFile(plug,path);
		}
	}
}
