

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

		class('MyBehaviour' ,LBehaviour)

			function MyBehaviour:Start()
				print('Start')
			end
			
			function MyBehaviour:OnEnable ()
				print('on enable')
			end

			function MyBehaviour:OnDisable()
				print('on disable')
			end

			function MyBehaviour:OnDestroy (  )
				print('on OnDestroy')
			end


		classend()

		-- anonymous class
		LBehaviour.AddTo(o,class.new(false,LBehaviour,{
			Start = function (self)
				print('Start')
			end


		}))

	end,

}