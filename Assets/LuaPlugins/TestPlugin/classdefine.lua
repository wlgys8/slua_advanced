

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


-- inherts from classA
class('ClassB',ClassA)

function ClassB:ctor( )
	self.super = Super() -- call super ctor
	print 'call classB ctor'
end

--function ClassB:foo( ... )
--	self.super:foo() -- call super member function
--	print 'call classB member function foo'
-- end


-- private inner class
class('ClassC')


classend()

-- public innter class
ClassB.ClassD = class('ClassD')


classend()

classend()



