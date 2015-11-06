using UnityEngine;
using System.Collections;
using UnityEditor;

public class LuaPluginsBuilder{


	
	
	[MenuItem("Build/BuildLuaPlugins")]
	public static void BuildScriptBundle(){
		string[] children = System.IO.Directory.GetDirectories("Assets/Luaplugins");
		LuaBuildConfig config = LuaBuildConfig.Instance;
		LuaAssetBundleBuilder builder = new LuaAssetBundleBuilder(config.availableCompilerPath);
		builder.Build("Assets/Output/LuaPlugins/"+platformName,children,config.buildTarget);
	}

	public static string platformName{
		get{
			string platformFolder = null;
			#if UNITY_ANDROID
			platformFolder = "Android";
			#elif UNITY_IPHONE
			platformFolder = "iOS";
			#elif UNITY_STANDALONE_OSX
			platformFolder = "OSX";
			#elif UNITY_STANDALONE_WIN
			platformFolder = "Win";
			#endif
			return platformFolder;
		}
	}

}
