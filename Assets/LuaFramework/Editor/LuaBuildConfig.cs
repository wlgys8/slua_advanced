using UnityEngine;
using System.Collections;
using UnityEditor;

public class LuaBuildConfig : ScriptableObject {

	public LuaMode luaMode;
	public string buildSymbol;

	public string[] luaCompileriOS;
	public string[] luaCompilerAndroid;
	public string[] luaCompilerOSX;
	public string[] luaCompilerWindows;


	public BuildTarget buildTarget{
		get{
#if UNITY_ANDROID
			return BuildTarget.Android;
#elif UNITY_IPHONE
			return BuildTarget.iOS;
#elif UNITY_STANDALONE_OSX
			return BuildTarget.StandaloneOSXUniversal;
#elif UNITY_STANDALONE_WIN
			return BuildTarget.StandaloneWindows;
#endif
		}
	}

	public string availableCompilerPath{
		get{
			string[] luaCompilerPaths = null;
			switch(this.buildTarget){
			case BuildTarget.Android:
				luaCompilerPaths = this.luaCompilerAndroid;
				break;
			case BuildTarget.iOS:
				luaCompilerPaths = this.luaCompileriOS;
				break;
			case BuildTarget.StandaloneOSXUniversal:
				luaCompilerPaths = this.luaCompilerOSX;
				break;
			case BuildTarget.StandaloneWindows64:
				luaCompilerPaths = this.luaCompilerWindows;
				break;
			}
			string luaCompilerPath = null;
			foreach(string path in luaCompilerPaths){
				if(System.IO.File.Exists(path)){
					luaCompilerPath = path;
					break;
				}
			}
			return luaCompilerPath;
		}
	}

	private static LuaBuildConfig _instance;

	public static LuaBuildConfig Instance{
		get{
			if(_instance == null){
				_instance =  AssetDatabase.LoadAssetAtPath<LuaBuildConfig>("Assets/Build/buildConfig.asset");
				if(_instance == null){
					LuaBuildConfig config = ScriptableObject.CreateInstance<LuaBuildConfig>();
					AssetDatabase.CreateAsset(config,"Assets/Build/buildConfig.asset");
					_instance = config;
				}
			}
			return _instance;
		}
	}


	[MenuItem("Build/SelectConfig")]
	public static void SelectConfig(){
		Selection.activeObject = LuaBuildConfig.Instance;
	}

}

public enum LuaMode{
	OnStreamingAssets,
	OnServer,

}
