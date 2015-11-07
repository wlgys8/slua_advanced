

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

		local o = UnityEngine.GameObject()

		Behaviour.AddTo(o,{

			Start = function ( ... )
				print('Start')
			end,
			
			OnEnable = function ( ... )
				print('on enable')
			end,

			OnDisable = function ( ... )
				print('on disable')
			end

		})

	end,

}