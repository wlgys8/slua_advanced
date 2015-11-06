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
		

}
