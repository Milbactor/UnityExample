﻿//------------------------------------------------------------------------------
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


namespace AssemblyCSharp
{
	public class TimedScript : GeekBehaviour
	{
		public static string M_DURATION_END = "M_DURATION_END";
		
		public LiteTimer timer = new LiteTimer( 1f );
		public float delay = 0;
		
		// Use this for initialization
		public float duration = 1f;
		
		
		override protected void Start () {
			base.Start();
			timer.duration = duration;
			timer.onElapsed += HandleonElapsed;
			
			if( duration > 0 )timer.start();
		}
		
		protected virtual void HandleonElapsed ( LiteTimer timer)
		{
			timer.onElapsed -= HandleonElapsed;
			dispatchMessage( M_DURATION_END );
			
			if( delay != 0)
			{
				timer.duration = delay;
				timer.onElapsed += HandleonElapsedDelay;
				timer.start();
			}
		}
		
		void HandleonElapsedDelay ( LiteTimer timer)
		{
			timer.onElapsed -= HandleonElapsedDelay;
			//dispatchMessage( M_ON_DESTROY );
			GameObject.Destroy( this.gameObject );
		}
		
		// Update is called once per frame
		override protected void Update () {
			base.Update();
			timer.Update();
		}
	}
}
