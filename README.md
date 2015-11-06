# UniSLuaPF
Plugin framework based on Unity+SLua

##Setup


##OOP
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


