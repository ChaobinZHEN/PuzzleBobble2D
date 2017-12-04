using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {
    public static Config instance;
    public const int ROW = 19; 
    public const int COL = 12; 
    public const float DEFEATDISTANCE = 0f;
    public const float RADIUS = 0.23f;

    public const string WAll = "Wall";
    public const string WALLGROUND = "WallGround";
    public const string STATICBOBBLE = "StaticBobble";
    public const string TOPWALL = "TopWall";

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
