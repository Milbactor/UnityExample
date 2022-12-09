using UnityEngine;
using System.Collections;

public class GeekInput{

	public static string LEFT = "L_" ;
	public static string RIGHT = "R_" ;
	public static string XAXIS = "XAxis_";
	public static string YAXIS = "YAxis_";
	public static string XBUTTON = "X_";
	public static string ABUTTON = "A_";
	public static string BBUTTON = "B_";
	public static string YBUTTON = "Y_";
	public static string STARTBUTTON = "Start_";
	public static string SELECTBUTTON = "Back_";

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	}

	public static Vector2 GetStickVector (string stickSide, int controllerID) {
		return new Vector2(Input.GetAxis(stickSide + XAXIS + controllerID), Input.GetAxis(stickSide + YAXIS + controllerID));

	}
	
	public static Vector2 GetStickVectorWorld (string stickSide, int controllerID) {
		return new Vector2(Input.GetAxis(stickSide + XAXIS + controllerID), Input.GetAxis(stickSide + YAXIS + controllerID));
		
	}

	public static float GetStickAngle  (string stickSide, int controllerID) {

		Vector2 stickVector = GetStickVector (stickSide, controllerID);
		return Mathf.Atan2 (stickVector.x, stickVector.y) * 180 / Mathf.PI;

	}
}
