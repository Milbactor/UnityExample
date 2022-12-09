using UnityEngine;
using System.Collections;

public enum PlayMode{
	NORMAL,
	MULTI,
	TWOBYTWO,
};

public class PlayerDataC : MonoBehaviour {

	public string player1;
	public string player2;
	public string player3;
	public string player4;

	public PlayMode mode; 



	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

}
