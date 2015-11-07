using UnityEngine;
using System.Collections;
using SLua;
using LuaInterface;

[CustomLuaClass]
public class LuaMonoBehaviour : MonoBehaviour {

	public System.Action<string,object[]> receiver;


	private void CallLuaFunction(string funcName,params object[] args){
		if(receiver != null){
			receiver(funcName,args);
		}
	}

	void Start () {
		CallLuaFunction("Start");
	}

	void OnDestroy(){
		CallLuaFunction("OnDestroy");
	}

	void OnBecameVisible(){
		CallLuaFunction("OnBecameVisible");
	}

	void OnBecameInvisible(){
		CallLuaFunction("OnBecameInvisible");
	}

	void OnEnable(){
		CallLuaFunction("OnEnable");
	}

	void OnDisable(){
		CallLuaFunction("OnDisable");
	}
}
