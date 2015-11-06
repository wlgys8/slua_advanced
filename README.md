# UniSLuaPF
Plugin framework based on Unity+SLua

##Setup
* SLua repository : https://github.com/pangweiwei/slua
* ShellHelper repository : https://github.com/wlgys8/UnityShellHelper
* LuaPlugins : 用户的Lua代码在此处
* LuaFramwork : 框架代码在此处

Add LuaManager.cs to a scene gameobject.<br>

##LuaPlugins

Create two folders named Plugin1 & Plugin2 under LuaPlugins.<br>
Then create a file named "main.lua" under each folder.

Structure：

    -LuaPlugins
    	-Plugin1
    		-main.lua
    	-Plugin2
    		-main.lua

1.框架会遍历LuaPlugins目录下的文件夹<br>
2.每个文件夹代表一个插件<br>
3."main.lua" 作为插件的执行入口<br>

1.Framework will traverse all folders under "LuaPlugins".<br>
2.Each one folder represents a plugin.<br>
3."main.lua" is used as the entry file.<br>

### main.lua 

		plugin{
			name = 'Plugin1', --plugin name
			main = function ( ... ) --entry function
			end,
			dependencies = { -- depend on any other plugins?
				"Plugin2",
			}
		}
		
##OOP implements
实现了class关键字.可以如下定义一个类型:

    class('ClassA')

	--static field
	staticVar = 1

	--private static field
	local pStaticVar = 2

	-- construct function
	function ClassA:ctor(...)
		-- member field
		self.memberVar = 3
		print 'call class A ctor'
	end

	--member function
	function ClassA:foo( ... )
		print 'call class A member function foo'
	end

	--static function
	function ClassA.foo2( ... )
		print 'call classAstatic function foo2'
	end

	--also static function
	function foo3( ... )
		print 'call classA static function foo3'
	end

	-- private static function
	local function foo4( ... )
	end

	classend()
	
调用方式:

    local a = ClassA()
    a:foo()

##How to build plugins as assetBundles
Click Menu->Build->BuildLuaPlugins

Framework will generate assetbundles under Assets/Output/\<Platform\>/

##How to compile lua to bytecodes

