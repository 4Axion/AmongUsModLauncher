using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public float VLaunch;
	public float VTOI;

	public GameData (MenuOpt Scr)
	{
		VLaunch = MenuOpt.VLaunch;
		VTOI = MenuOpt.VTOI;
	}
}
