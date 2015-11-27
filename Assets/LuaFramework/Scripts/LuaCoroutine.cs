using UnityEngine;
using System.Collections;
using SLua;

[CustomLuaClass]
public class LuaCoroutine : MonoBehaviour {

	public void ExecuteWhen(object instruction,LuaFunction func,object param){
		StartCoroutine(_ExecuteWhen(instruction,func,param));
	}

	private IEnumerator _ExecuteWhen(object instruction,LuaFunction func,object param){
		yield return instruction;
		func.call(param);
	}

}
