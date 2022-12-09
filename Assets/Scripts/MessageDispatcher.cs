using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MessageDispatcher : MonoBehaviour
{
	public delegate void Callback (params object[] arguments);
	public Dictionary<string, List< Callback>> listeners = new Dictionary<string, List< Callback >>();
	private Dictionary<string, List<Callback>> removeList = new Dictionary<string, List< Callback>>();
	public static string lastEvent = "";
	
	public void addMessageListener(Callback callback, string type)
	{
		if (!listeners.ContainsKey(type))
		{
			// this only works when key is found..
			listeners.Add(type, new List< Callback >());
		}
		
		if (!listeners[type].Contains(callback))
		{
			listeners[type].Add(callback);
		}
		
		/*if (listeners.Count() > 10 || listeners[type].Count() > 10)
            {
                Console.WriteLine("woah");
            }*/
	}
	
	public void removeMessageHandler(Callback callback, string type)
	{
		if (!removeList.ContainsKey(type))
		{
			// this only works when key is found..
			removeList.Add(type, new List< Callback >());
		}
		
		if (!removeList[type].Contains(callback))
		{
			removeList[type].Add(callback);
		}
		
	}
	
	
	public void Update()
	{
		
		//remove everything in removelist
		
		foreach (KeyValuePair<string, List< Callback > > pair in removeList)
		{
			foreach ( Callback callback in pair.Value)
			{
				listeners[pair.Key].Remove(callback);
			}
			
		}
		
		removeList.Clear();
	}
	
	/// <summary>
	/// check if the messagetype has any listeners
	/// </summary>
	/// <param name="callback"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public bool hasMessageListener(string type)
	{
		if (listeners.ContainsKey(type))
		{
			if (listeners[type].Count > 0)
			{
				return true;
			}
		}
		
		return false;
	}
	
	/// <summary>
	/// check if a specific callback is registered
	/// </summary>
	/// <param name="callback"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public bool hasMessageListener( Callback callback, string type)
	{
		if (listeners.ContainsKey(type))
		{
			if (listeners[type].Contains(callback))
			{
				return true;
			}
		}
		
		return false;
	}
	
	public void dispatchMessage( string message, params object[] arguments)
	{
		if (listeners.ContainsKey( message ))
		{
			/*foreach ( Callback callback in listeners[message])
			{
				//Logger.log("dispatching message [" + message + " ] with arguments length: " + arguments.Length );
				callback ( arguments ); 
			}*/
			
			//fastest
			int i = 0; 
			int size = listeners[message].Count;
			lastEvent = message;
			//arguments[10] = message;
			for( i = 0; i < size; i++)
			{
				Callback callback = listeners[message][i];
				//if(( (MonoBehaviour)callback.Target).enabled )
				//{
				
				   // if( arguments.Length == 0 || arguments[0] == null){
					 //  callback( message );
					//}else{
					   callback( arguments ); 
					//}
				//}
			}

		}
	}
	
	
	
}
