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

namespace AssemblyCSharp
{
	public class TimedWindow : MonoBehaviour
	{
		float time = 0;
		public int duration;
		public string title = "title";
		
		void Update()
		{
			time += Time.deltaTime;
			
			if( time >= duration) GameObject.Destroy( gameObject );
		}
	
		void OnGUI()
		{
			GUI.Window(0, new Rect( Screen.width / 2, Screen.height / 2, Screen.width / 3, 100 ), showContent, title );
		}
		
		void showContent( int id)
		{
		
		}
	}

}

