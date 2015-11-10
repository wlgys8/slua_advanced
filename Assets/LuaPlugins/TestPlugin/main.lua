

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

		class('LB' ,LBehaviour)

			function LB:Start()
				print('Start')
			end
			
			function LB:OnEnable ()
				print('on enable')
			end

			function LB:OnDisable()
				print('on disable')
			end

			function LB:OnDestroy (  )
				print('on OnDestroy')
			end


		classend()

		LBehaviour.AddTo(o,LB)

	end,

}