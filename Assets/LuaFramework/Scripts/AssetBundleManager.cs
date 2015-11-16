using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Asset bundle manager.
/// </summary>
public class AssetBundleManager : MonoBehaviour{


	private Dictionary<string,string> _urlMap = new Dictionary<string, string>();
	
	private Dictionary<string,AssetBundle> _loaded = new Dictionary<string, AssetBundle>();
	
	public List<string> loadedBundleNames{
		get{
			List<string> ret = new List<string>();
			ret.AddRange(_loaded.Keys);
			return ret;
		}
	}
	
	public bool isDebugOn = true;
	
	private AssetBundleManifest _manifest;


	///<summary>
	///if <bundleName> should be loaded from a remote url,then add the url for <bundleName>.
	/// Otherwise, manager will search the bundle in "Application.StreamingAssets"
	/// </summary>

	public void AddURL(string bundleName,string url){
		_urlMap.Add(bundleName,url);
	}

	private IEnumerator _LoadManifest(string bundleName,int version = -1 , System.Action<AssetBundleManifest> onDone = null){
		WWW www = null;
		string url = GetURLFromBundleName(bundleName);
		if(version < 0){
			www = new WWW(url);
		}else{
			www = WWW.LoadFromCacheOrDownload(url,version);
		}
		yield return www;
		AssetBundleManifest manifest = null;
		AssetBundle bd = www.assetBundle;
		do{
			if(bd == null){
				Debug.LogError("Load AssetBundle Failed!");
				break;
			}
			manifest = bd.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
			if(manifest == null){
				Debug.LogError("Load Manifest Failed from Assetbundle!");
				break;
			}
		}while(false);

		if(onDone != null){
			onDone(manifest);
		}
	}

	public Request LoadManifest(string bundleName,int version = -1 ){
		if(isDebugOn){
			Debug.Log(string.Format("Load {0},version = {1}",bundleName,version));
		}
		Request req = new Request();
		StartCoroutine(_LoadManifest(bundleName,version,delegate(AssetBundleManifest obj) {
			req.NotifyDone(obj);
			_manifest = obj;
			if(isDebugOn){
				Debug.Log("Load completed");
			}
		}));
		return req;
	}

	public AssetBundleManifest manifest{
		get{
			return _manifest;
		}
	}

	private IEnumerator _LoadAssetBundle(string bundleName,System.Action<AssetBundle> onDone){
		AssetBundle bd = null;
		do{
			if(_loaded.ContainsKey(bundleName)){
				break;
			}
			Hash128 hash = manifest.GetAssetBundleHash(bundleName);
			string url = GetURLFromBundleName(bundleName);
			WWW www =  WWW.LoadFromCacheOrDownload(url,hash);
			yield return www;
			bd = www.assetBundle;
			if(bd == null){
				Debug.LogError("load assetbundle failed:"+url);
				break;
			}
			_loaded.Add(bundleName,bd);
		}while(false);
		onDone(bd);
	}

	public Request LoadAssetBundle(string bundleName){
		if(isDebugOn){
			Debug.Log(string.Format("Load {0},bundleName = {1}",bundleName,bundleName));
		}
		Request req = new Request();
		StartCoroutine(_LoadAssetBundle(bundleName,delegate(AssetBundle obj) {
			req.NotifyDone(obj);
			if(isDebugOn){
				Debug.Log("load completed");
			}
		}));
		return req;
	}

	private string streamingAssetsPath{
		get{
			string url = Application.streamingAssetsPath;
			if(!url.Contains("://")){
				url = "file://"+url;
			}
			return url;
		}
	}

	private string GetURLFromBundleName(string bundleName){
		string url = null;
		if(_urlMap.ContainsKey(bundleName)){
			url = _urlMap[bundleName];
		}else{
			url = streamingAssetsPath+"/"+bundleName;
		}
		return url;
	}

	private IEnumerator _LoadAllAssetBundles(System.Action<List<AssetBundle>> onDone){
		string[] bundles = manifest.GetAllAssetBundles();
		List<AssetBundle> assetbundles = new List<AssetBundle>();
		foreach(string name in bundles){
			Request req = LoadAssetBundle(name);
			yield return StartCoroutine(req.WaitUntilDone());
			if(req.value != null){
				assetbundles.Add(req.value as AssetBundle);
			}
		}
		onDone(assetbundles);
	}

	public Request LoadAllAssetBundles(){
		Request req = new Request();
		StartCoroutine(_LoadAllAssetBundles(delegate(List<AssetBundle> bundles) {
			req.NotifyDone(bundles);
		}));
		return req;
	}


	public T LoadAsset<T>(string bundleName,string fileName) where T:Object{
		if(!_loaded.ContainsKey(bundleName)){
			Debug.LogError(bundleName + " should be loaded first");
			return null;
		}
		return _loaded[bundleName].LoadAsset<T>(fileName);
	}

	public bool Contains(string bundleName,string fileName){
		if(!_loaded.ContainsKey(bundleName)){
			return false;
		}
		return _loaded[bundleName].Contains(fileName);
	}


	
	public static AssetBundleManager CreateManager(){
		AssetBundleManager manager = new GameObject("AssetBundleManager").AddComponent<AssetBundleManager>();
		GameObject.DontDestroyOnLoad(manager.gameObject);
		return manager;
	}


	public class Request{

		private object _value;
		public event System.Action<object> onDone;

		public object value{
			get{
				return _value;
			}
		}

		public bool isDone{
			private set;
			get;
		}
		internal void NotifyDone(object value){
			_value = value;
			this.isDone = true;
			if(this.onDone != null){
				this.onDone(value);
			}
		}

		internal IEnumerator WaitUntilDone(){
			while(!isDone){
				yield return null;
			}
		}
	}
}
