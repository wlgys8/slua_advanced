using UnityEngine;
using System.Collections;
using SLua;
using LuaInterface;

[CustomLuaClass]
public class LuaComponent : MonoBehaviour {

	void Awake(){
		CallLuaFunction("Awake",_table);
	}
	void Start () {
		CallLuaFunction("Start",_table);
	}

	void OnDestroy(){
		CallLuaFunction("OnDestroy",_table);
		if(_table != null){
			_table["__index"] = null;
			LuaFunction func = LuaManager.L["setmetatable"] as LuaFunction;
			func.call(_table,null);
			_table.Dispose();
			_table = null;
		}
	}
	private void CallLuaFunction(string funcName,params object[] args){
		if(_table != null){
		//	_table.invoke(funcName,args);
			LuaFunction f = (LuaFunction)_table[funcName];
			if (f != null)
			{
				f.call(args);
			}
		}
	}

	private LuaTable _table;

	internal void Initlize(LuaTable table){
		_table = table;
		table["luaComponent"] = this;
	}


	public static LuaTable Add(GameObject go,LuaTable table){
		LuaComponent cp = go.AddComponent<LuaComponent>();
		cp.Initlize(table);
		cp.CallLuaFunction("Awake",table,cp);
		return table;
	}
}
