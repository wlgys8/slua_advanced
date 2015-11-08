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

	void OnCollisionEnter(Collision collision){
		CallLuaFunction("OnCollisionEnter",collision);
	}

	void OnCollisionEnter2D(Collision2D co){
		CallLuaFunction("OnCollisionEnter2D",co);
	}

	void OnCollisionExit(Collision co){
		CallLuaFunction("OnCollisionExit",co);
	}

	void OnCollisionExit2D(Collision2D co) {
		CallLuaFunction("OnCollisionExit2D",co);
	}

	void OnCollisionStay(Collision co){
		CallLuaFunction("OnCollisionStay",co);
	}

	void OnCollisionStay2D(Collision2D co){
		CallLuaFunction("OnCollisionStay2D",co);
	}
}
