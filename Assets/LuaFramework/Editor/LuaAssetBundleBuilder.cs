
/// <summary>
/// Lua asset bundle builder.
/// Used for building all lua scripts into an assetbundle.
/// If luaCompiler is assigned, lua scripts will be compiled to bytecodes.
/// Otherwise, only file extension will be changed to ".lua.txt"
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class LuaAssetBundleBuilder{


	private string _luaCompiler;

	public LuaAssetBundleBuilder(){
	}

	public LuaAssetBundleBuilder(string luaCompiler){
		_luaCompiler = luaCompiler;
	}

	public string luaCompiler{
		get{
			return _luaCompiler;
		}
	}

	public ShellHelper.ShellRequest Compile(string input){
		if(Application.platform == RuntimePlatform.OSXEditor){
			if(luaCompiler.EndsWith("luac")){
				ShellHelper.ShellRequest req = ShellHelper.ProcessCommand("find "+input+" -name '*.lua' -exec "+this.luaCompiler+" -o {}.txt {} \\;","./");
				return req;

			}else{
				ShellHelper.ShellRequest req = ShellHelper.ProcessCommand("find "+input+" -name '*.lua' -exec "+this.luaCompiler+" -b {} {}.txt \\;","./");
				return req;
			}

		}else{
			return null;
		}
	}

	public ShellHelper.ShellRequest Compile(params string[] inputs){
		ShellHelper.ShellRequest req = new ShellHelper.ShellRequest();
		int completeCount = 0;
		foreach(string input in inputs){
			ShellHelper.ShellRequest reqI = Compile(input);
			reqI.onDone += delegate() {
				completeCount ++;
				if(completeCount == inputs.Length){
					req.NotifyDone();
				}
			};
		}
		return req;
	}

	public  void Build(string outputDir,string[] inputs,BuildTarget targetPlatform){
		if(string.IsNullOrEmpty(this.luaCompiler)){
			BuildWithRaw(outputDir,inputs,targetPlatform);
		}else{
			BuildWithByteCode(outputDir,inputs,targetPlatform);
		}
	}

	public void BuildWithByteCode(string outputDir,string[] inputs,BuildTarget targetPlatform){
		if(!System.IO.Directory.Exists(outputDir)){
			System.IO.Directory.CreateDirectory(outputDir);
		}

		ShellHelper.ShellRequest req =  Compile(inputs);
		req.onDone += delegate() {
			AssetDatabase.Refresh();
			List<AssetBundleBuild> buildInfos = new List<AssetBundleBuild>();
			foreach(string input in inputs){
				string bundleName = System.IO.Path.GetFileName(input);
				string[] luaFiles = System.IO.Directory.GetFiles(input,"*.lua.txt", System.IO.SearchOption.AllDirectories);
				AssetBundleBuild bd = new AssetBundleBuild();
				bd.assetBundleName = bundleName;
				bd.assetNames = luaFiles;
				buildInfos.Add(bd);
			}

			AssetDatabase.Refresh();

			BuildPipeline.BuildAssetBundles(outputDir,buildInfos.ToArray(),BuildAssetBundleOptions.None,targetPlatform);
			//delete temp files , 
			foreach(AssetBundleBuild bd in buildInfos){
				foreach(string file in bd.assetNames){
					FileUtil.DeleteFileOrDirectory(file);
				}
			}
			string outputDirName = System.IO.Path.GetFileName(outputDir);
			RenameFile(outputDir+"/"+outputDirName,outputDir+"/LuaPlugins");
			RenameFile(outputDir+"/"+outputDirName+".manifest",outputDir+"/LuaPlugins.manifest");

			AssetDatabase.Refresh();
		};
	}

	private void RenameFile(string from,string to){
		FileUtil.DeleteFileOrDirectory(to);
		FileUtil.MoveFileOrDirectory(from,to);
	}

	public void BuildWithRaw(string outputDir,string[] inputs,BuildTarget targetPlatform){
		if(!System.IO.Directory.Exists(outputDir)){
			System.IO.Directory.CreateDirectory(outputDir);
		}
		List<AssetBundleBuild> buildInfos = new List<AssetBundleBuild>();

		foreach(string input in inputs){
			string bundleName = System.IO.Path.GetFileName(input);
			string[] luaFiles = System.IO.Directory.GetFiles(input,"*.lua", System.IO.SearchOption.AllDirectories);
			List<string> txtFiles = new List<string>();
			foreach(string file in luaFiles){
				string newPath = file+".txt";
				txtFiles.Add(newPath);
				FileUtil.ReplaceFile(file,newPath);
			}
			AssetBundleBuild bd = new AssetBundleBuild();
			bd.assetBundleName = bundleName;
			bd.assetNames = txtFiles.ToArray();
			buildInfos.Add(bd);
		}
		AssetDatabase.Refresh();

		BuildPipeline.BuildAssetBundles(outputDir,buildInfos.ToArray(),BuildAssetBundleOptions.None,targetPlatform);
		//delete temp files , 
		foreach(AssetBundleBuild bd in buildInfos){
			foreach(string file in bd.assetNames){
				FileUtil.DeleteFileOrDirectory(file);
			}
		}

		string outputDirName = System.IO.Path.GetFileName(outputDir);
		RenameFile(outputDir+"/"+outputDirName,outputDir+"/LuaPlugins");
		RenameFile(outputDir+"/"+outputDirName+".manifest",outputDir+"/LuaPlugins.manifest");
		AssetDatabase.Refresh();

	}


}
