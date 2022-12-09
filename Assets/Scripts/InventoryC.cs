using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InventoryC : GeekBehaviour {

	public static string M_ON_COLLECT = "M_ON_COLLECT";
	private Dictionary< Type, List<MonoBehaviour> > items;
	
	public float ySpacing = 5;
	
	// Use this for initialization
	void Start () {
	
		base.Start();
		items = new Dictionary<Type, List<MonoBehaviour> >();
		
		addMessageListener( args => onItemCollect( (MonoBehaviour)args[0] ), M_ON_COLLECT ) ;
	}
	
	void onItemCollect( MonoBehaviour item )
	{
		if( items.ContainsKey( item.GetType() ) == false ) {
			items[item.GetType()] = new List< MonoBehaviour >();
		}
		
		List<MonoBehaviour> itemsOftype = items[item.GetType()];
		itemsOftype.Add( item );
		
		print ("adding item : " + item + " under key: " + item.GetType() );
	}
	
	// Update is called once per frame
	void Update () {
	
		base.Update();
	}
	
	public List<MonoBehaviour> getItems( Type type )
	{
		if( items.ContainsKey( type ) == false ) return null;
		return items[type];
	}

	public int getItemCount( Type type )
	{
		if( items.ContainsKey( type ) == false ) return 0;
		return items[type].Count;
	}

	public void removeItem( Type type)
	{
		if( items.ContainsKey( type ) == false ) return;
		items[type].RemoveAt( items[type].Count - 1 );
	}
	
	void OnGUI()
	{
		int j = 0;
		
		foreach(KeyValuePair<Type, List<MonoBehaviour> > entry in items)
		{
			//print (entry.Key + " : " + entry.Value );
			for(int i = 0; i < entry.Value.Count; i++)
			{
				if(GetComponent<PlayerC>().ID == 1)
				{
					Texture texture;
					if( entry.Value[i].GetComponent<InventoryItemC>() != null )
					{
						texture = entry.Value[i].GetComponent<InventoryItemC>().icon;
					}
					else
					{
						texture = entry.Value[i].GetComponent<SpriteRenderer>().sprite.texture;
					}

					GUI.DrawTexture( new Rect( i * 75 + 5, 75 + (j * ySpacing), 75, 50), texture ); 
				}
				else
				{
					float left = 3 * 75f;
					Texture texture;
					if( entry.Value[i].GetComponent<InventoryItemC>() != null )
					{
						texture = entry.Value[i].GetComponent<InventoryItemC>().icon;
					}
					else
					{
						texture = entry.Value[i].GetComponent<SpriteRenderer>().sprite.texture;
					}
					GUI.DrawTexture( new Rect(left - (i * 75 + 5) + Screen.width* 5/6, 75 + (j * ySpacing), 75, 50), texture );
				}
			}
			
			j++;
		}
		
		//print ("-------");
	}
}
