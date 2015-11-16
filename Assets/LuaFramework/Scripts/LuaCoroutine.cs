using UnityEngine;
using System.Collections;
using SLua;

[CustomLuaClass]
public class LuaCoroutine : MonoBehaviour {

	public void ExecuteWhen(object instruction,LuaFunction func){
		StartCoroutine(_ExecuteWhen(instruction,func));
	}

	private IEnumerator _ExecuteWhen(object instruction,LuaFunction func){
		yield return instruction;
		func.call();
	}

}
