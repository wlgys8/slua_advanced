using UnityEngine;
using System.Collections;
using SLua;
using System.Collections.Generic;
using System.IO;



public enum LuaRunMode{
	/// <summary>
	/// In editor mode,luaManager will load lua file directly from editor.
	/// All modifications on file will be work immediately. 
	/// </summary>
	Editor, 
	
	/// <summary>
	/// In AssetBundle mode,luaManager will load lua file from assetbundle.
	/// If some files were changed,they should be rebuilt as assetbundles to make the modifications work.
	/// </summary>
	AssetBundle,  
}


public class LuaManager : MonoBehaviour {
	public const string PluginRoot = "Assets/LuaPlugins";

	[Tooltip("In editor mode,luaManager will load lua file directly from editor." +
		"All modifications on file will be work immediately." +
	    "In AssetBundle mode,luaManager will load lua file from assetbundle." +
	    "If some files were changed,they should be rebuilt again to make the modifications work.")]
	public LuaRunMode mode = LuaRunMode.Editor;

	/// <summary>
	/// This search paths only work in editor.
	/// </summary>
	public string[] pluginSearchPathInEditor = new string[]{
		"Assets/LuaPlugins"
	};

	/// <summary>
	/// If autoBoot was set to false,you should boot the manager by yourself.
	/// Setup->LoadAllPlugins->LaunchPlugin
	/// </summary>
	public bool autoBoot = false;

	/// <summary>
	/// Each plugin is an assetsbundle.
	/// </summary>
	private AssetBundleManager _pluginBundleManager;


	/// <summary>
	/// Map plugin name to its path
	/// </summary>
	private Dictionary<string,string> _pluginNameToPath = new Dictionary<string, string>();

	private LuaSvr _svr;

	private bool _isInited = false;


	public LuaRunMode actualRunMode{
		get{
#if UNITY_EDITOR
			return this.mode;
#else
			return LuaRunMode.AssetBundle;
#endif
		}
	}
	public IEnumerator Start(){
		GameObject.DontDestroyOnLoad(gameObject);
		if(!autoBoot){
			yield break;
		}
		yield return Setup ();
		yield return LoadAllPlugins ();
		LaunchPlugin();
	}

	public Coroutine Setup(){
		return StartCoroutine(_Setup());
	}

	private IEnumerator _Setup(){
		LuaState.loaderDelegate = LoadFile;
		_svr = new LuaSvr();
		_svr.init(delegate(int obj) {
			
		},delegate() {
			_isInited = true;
		},false);
		while(!isInited){
			yield return null;
		}
	}


	public bool isInited{
		get{
			return _isInited;
		}
	}

	public LuaState L{
		get{
			return _svr.luaState;
		}
	}

	private byte[] LoadFile(string fileName){
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

	// for editor mode
	private string SearchPluginPath(string pluginName){
		if(_pluginNameToPath.ContainsKey(pluginName)){
			return _pluginNameToPath[pluginName];
		}
		for(int i = 0;i<this.pluginSearchPathInEditor.Length;i++){
			string searchPath = this.pluginSearchPathInEditor[i];
			string pluginPath = System.IO.Path.Combine(searchPath,pluginName);
			string mainFilePath = System.IO.Path.Combine(pluginPath,"main.lua");
			if(System.IO.Directory.Exists(pluginPath) && File.Exists(mainFilePath)){
				_pluginNameToPath.Add(pluginName,pluginPath);
				return pluginPath;
			}
		}
		return null;
	}

	private byte[] LoadFileFromPlugin(string pluginName,string fileName){
		try{
			string pluginPath = SearchPluginPath(pluginName);
			if(pluginPath == null){
				Debug.LogError("Can not find a plugin named "+pluginName);
				return null;
			}
			string fullPath = System.IO.Path.Combine(pluginPath,fileName); 
			if(actualRunMode == LuaRunMode.Editor){
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
				pluginName = pluginName.ToLower();
				TextAsset txt = bundleManager.LoadAsset<TextAsset>(pluginName,fullPath+".lua.txt");
				return txt.bytes;
			}
		}catch(System.Exception e){
			Debug.LogException(e);
			return null;
		}
	}


	public object DoFile(string fileName){
		return _svr.start(fileName);
	}

	private IEnumerator _LoadAllPlugins(){
		AssetBundleManager.Request req = bundleManager.LoadManifest("LuaPlugins");
		yield return bundleManager.StartCoroutine(req.WaitUntilDone());
		req = bundleManager.LoadAllAssetBundles();
		yield return bundleManager.StartCoroutine(req.WaitUntilDone());
	}
	
	public Coroutine LoadAllPlugins(){
 		if(actualRunMode == LuaRunMode.Editor){
			return null;
		}
		return StartCoroutine(_LoadAllPlugins());
	}

	public void LaunchPlugin(){
		this.DoFile("LuaEnv/plugin.txt");
	}
	
	internal AssetBundleManager bundleManager{
		get{
			if(_pluginBundleManager == null){
				_pluginBundleManager = AssetBundleManager.CreateManager();
			}
			return _pluginBundleManager;
		}
	}

	
	public bool ExistFile(string pluginName,string fileName){
		string pluginPath = SearchPluginPath(pluginName);
		if(pluginPath == null){
			return false;
		}
		string fullPath = System.IO.Path.Combine(pluginPath,fileName);
		if(actualRunMode == LuaRunMode.Editor){
			return File.Exists(fullPath+".lua");
		}else{
			pluginName = pluginName.ToLower();
			return bundleManager.Contains(pluginName,fullPath+".lua.txt");
		}
	}

	public string[] GetPluginList(){
		if(actualRunMode == LuaRunMode.Editor){
			List<string> pluginNames = new List<string>();
			for(int i = 0;i<pluginSearchPathInEditor.Length;i++){
				string searchPath = pluginSearchPathInEditor[i];
				string[] dirs = Directory.GetDirectories(searchPath);
				foreach(string path in dirs){
					if(File.Exists(Path.Combine(path,"main.lua"))){
						pluginNames.Add(Path.GetFileName(path));
					}
				}
			}
			return pluginNames.ToArray();
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


	private static LuaManager _instance;

	public static LuaManager Instance{
		get{
			if(_instance == null){
				_instance = GameObject.FindObjectOfType<LuaManager>();
			}
			if(_instance == null){
				_instance = new GameObject("LuaManager").AddComponent<LuaManager>();
			}
			return _instance;
		}
	}

}
