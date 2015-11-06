using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_LuaComponent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Add_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			SLua.LuaTable a2;
			checkType(l,2,out a2);
			var ret=LuaComponent.Add(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"LuaComponent");
		addMember(l,Add_s);
		createTypeMetatable(l,null, typeof(LuaComponent),typeof(UnityEngine.MonoBehaviour));
	}
}
