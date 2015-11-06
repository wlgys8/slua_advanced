

plugin{

	name = 'TestPlugin',
	main = function ( ... )
		print('run plugin main')

		use('classdefine') 

		local a = ClassA() 
		local b = ClassB()

		local d = ClassB.ClassD()

		b:foo()

		ClassA.foo2()
		ClassA.foo3()

	end,

}