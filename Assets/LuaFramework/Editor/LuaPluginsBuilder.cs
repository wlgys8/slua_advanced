using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class LuaPluginsBuilder{

	
	
	public static void BuildScriptBundle(string[] searchPaths){
		List<string> children = new List<string>();
		foreach(string searchPath in searchPaths){
			string[] fd = System.IO.Directory.GetDirectories(searchPath);
			children.AddRange(fd);
		}
		LuaBuildConfig config = LuaBuildConfig.Instance;
		LuaAssetBundleBuilder builder = new LuaAssetBundleBuilder(config.availableCompilerPath);
		builder.Build("Assets/Output/LuaPlugins/"+platformName,children.ToArray(),config.buildTarget);
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
