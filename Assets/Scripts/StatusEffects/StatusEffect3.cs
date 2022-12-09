//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections.Generic;


namespace AssemblyCSharp
{
	[RequireComponent (typeof (StatusEffectManager3))]
	public class StatusEffect3 : GeekBehaviour
	{
		public Dictionary<int, dynamic> propertyValues = new Dictionary<int, dynamic>();
		
		public static string M_STATUS_END = "M_STATUS_END";
		public static string M_STATUS_ENDED = "M_STATUS_ENDED";
		
		protected StatusEffectManager3 manager;
		public bool extendWhenStack = false;
		
		public LiteTimer timer;
		public float duration = 0f;
		
		
		virtual protected void Start()
		{
			base.Start();
			
			manager = GetComponent<StatusEffectManager3>();
			manager.Register( this );
			
		//	Debug.Log ("----- start of " + GetType() + " -----");
			
			timer = new LiteTimer( duration );
			timer.onElapsed += HandleonElapsed;
			if( duration > 0 )
			{
				timer.start();
			}
		}
		
		public void extend( float time = 0 )
		{
			timer.duration += duration;
		}
		
		public virtual void reset()
		{
			//Debug.Log(" ----- removing effect: " + GetType() + " ------" );
			manager.RemoveEffect( this );
			if( manager.containsSameType( GetType() ) ) return;
		}
		
		virtual protected void HandleonElapsed( LiteTimer timer)
		{
			reset();
			Destroy( this );
		}
		
		override protected void Update()
		{
			base.Update();
			timer.Update();
			
			//print ("time : " + timer.time );
		}
		
		/*void OnDisable()
		{
			dispatchMessage(M_STATUS_ENDED, this );
			reset();
		} */
		
		
		void OnDestroy()
		{
			dispatchMessage(M_STATUS_ENDED, this );
			reset();
		}
		
	}
}

