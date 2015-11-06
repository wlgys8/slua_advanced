using UnityEngine;
using System.Collections;
using SLua;
using System.Collections.Generic;
using System.IO;

public class LuaManager  {


	
	private static AssetBundleManager _pluginBundleManager;
	public const string PluginRoot = "Assets/LuaPlugins";
	
	#if UNITY_EDITOR
	internal static bool isEditorMode = true;
	#else
	internal static bool isEditorMode = false;
	
	#endif

	static LuaSvr _svr;

	private static bool _isInited = false;

	public static void Setup(){
		LuaState.loaderDelegate = LoadFile;
		_svr = new LuaSvr();
		_svr.init(delegate(int obj) {

		},delegate() {
			_isInited = true;
		},false);
	}

	public static bool isInited{
		get{
			return _isInited;
		}
	}

	public static LuaState L{
		get{
			return _svr.luaState;
		}
	}

	private static byte[] LoadFile(string fileName){
		if(fileName.EndsWith(".lua") || fileName.EndsWith(".txt")){
			fileName = fileName.Substring(0,fileName.Length - 4);
		}
		if(fileName.StartsWith("file://")){
			fileName = fileName.Substring(7);
			fileName = fileName.Replace('.','/');
			return File.ReadAllBytes(fileName+".lua");

		}else if(fileName.StartsWith("plug://")){
			fileName = fileName.Substring(7);
			int idx = fileName.IndexOf('/');
			if(idx < 0 ){
				Debug.LogError("error");
				return null;
			}
			string pluginName = fileName.Substring(0,idx);
			fileName = fileName.Substring(idx+1).Replace('.','/');
			return LoadFileFromPlugin(pluginName,fileName);
		}else{
			fileName = fileName.Replace('.','/');
			TextAsset asset = Resources.Load<TextAsset>(fileName);
			if(asset == null){
				return null;
			}
			return asset.bytes;
		}

	}

	private static byte[] LoadFileFromPlugin(string pluginName,string fileName){
		try{
			string prefix =  PluginRoot;
			string fullPath = prefix + "/"+ pluginName + "/"+fileName;
			if(isEditorMode){
				try{
					byte[] ret =  File.ReadAllBytes(fullPath+".lua");
					if(ret == null){
						Debug.LogError("Maybe something error with file:"+fullPath);
					}
					return ret;
				}catch(System.Exception e){
					Debug.LogException(e);
					return null;
				}
			}
			else{
				TextAsset txt = bundleManager.LoadAsset<TextAsset>(pluginName,fullPath+".lua.txt");
				return txt.bytes;
			}
		}catch(System.Exception e){
			Debug.LogException(e);
			return null;
		}
	}


	public static object DoFile(string fileName){
		return _svr.start(fileName);
	}

	private static IEnumerator _LoadAllPlugins(){
		string path = Application.streamingAssetsPath+"/LuaPlugins";
		if(!path.Contains("file://")){
			path = "file://"+path;
		}
		AssetBundleManager.Request req = bundleManager.LoadManifest(path);
		yield return bundleManager.StartCoroutine(req.WaitUntilDone());
		req = bundleManager.LoadAllAssetBundles();
		yield return bundleManager.StartCoroutine(req.WaitUntilDone());
	}
	
	public static Coroutine LoadAllPlugins(){
 		if(isEditorMode){
			return null;
		}
		return bundleManager.StartCoroutine(_LoadAllPlugins());
	}

	public static void LaunchPlugin(){
		LuaManager.DoFile("LuaEnv/plugin.txt");
	}
	
	internal static AssetBundleManager bundleManager{
		get{
			if(_pluginBundleManager == null){
				_pluginBundleManager = AssetBundleManager.CreateManager();
			}
			return _pluginBundleManager;
		}
	}

	
	public static bool ExistFile(string pluginName,string fileName){
		string prefix = PluginRoot;
		string fullPath = prefix +"/"+ pluginName + "/"+fileName;
		if(isEditorMode){
			return File.Exists(fullPath+".lua");
		}else{
			return bundleManager.Contains(pluginName,fullPath+".lua.txt");
		}
	}

	public static string[] GetPluginList(){
		if(isEditorMode){
			string[] dirs = Directory.GetDirectories(PluginRoot);
			for(int i = 0;i<dirs.Length;i++){
				
				dirs[i] = Path.GetFileName(dirs[i]);
			}
			return dirs;
		}
		else{
			List<string> names = bundleManager.loadedBundleNames;
			int i = 0;
			foreach(string name in names){
				if(name.EndsWith(".unity3d")){
					name.Substring(0,name.Length -8);
					names[i] = name;
				}
				i++;
			}
			return names.ToArray();
		}
	}


}
