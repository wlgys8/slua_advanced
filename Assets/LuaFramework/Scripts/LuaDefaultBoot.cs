using UnityEngine;
using System.Collections;

public class LuaDefaultBoot : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		LuaManager.Setup();
		while(!LuaManager.isInited){
			yield return null;
		}
		yield return LuaManager.LoadAllPlugins();
		LuaManager.LaunchPlugin();
	}
	
}
