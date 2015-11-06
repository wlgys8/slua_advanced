using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_LuaPluginUtil : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			LuaPluginUtil o;
			o=new LuaPluginUtil();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetPluginList_s(IntPtr l) {
		try {
			var ret=LuaPluginUtil.GetPluginList();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExistFile_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			var ret=LuaPluginUtil.ExistFile(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_debug(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,LuaPluginUtil.debug);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_debug(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			LuaPluginUtil.debug=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"LuaPluginUtil");
		addMember(l,GetPluginList_s);
		addMember(l,ExistFile_s);
		addMember(l,"debug",get_debug,set_debug,false);
		createTypeMetatable(l,constructor, typeof(LuaPluginUtil));
	}
}
