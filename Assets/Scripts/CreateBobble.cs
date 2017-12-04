using UnityEngine;
using System.Collections;

public class CreateBobble : MonoBehaviour {
    public static CreateBobble Instance;
    public GameObject[] m_shotball = new GameObject[2];
    public GameObject[] bobbleStyle = new GameObject[8];
    public int layerMaxBallNum = 8;
    private GameObject randBobble;
    public int layersInitiate;

    public GameObject topWall;

    public GameObject emptyPoint;
    public struct Bobble
    {
        public GameObject pointObject;
        public GameObject bobbleObject;
    }
    public Bobble[,] m_bobble;
    private int row = Config.ROW;
    private int col = Config.COL;

    private Vector2 emptyPointPos;

    private float timer;

    // Use this for initialization
    void Start () {
        Instance = this;
        m_bobble = new Bobble[row, col];
        emptyPointPos = GameObject.Find("Empty Point").transform.position;
        topWall = GameObject.FindWithTag("TopWall");

        // Initiate bobbles
        InitiLayer();

        //  Create bobbles ready to shoot
        Vector3 shotpos = GameObject.FindGameObjectWithTag("Player").transform.position;
        randBobble = bobbleStyle[Random.Range(0, layerMaxBallNum)];
        m_shotball[0] = Instantiate(randBobble, shotpos, Quaternion.identity) as GameObject;
        shotpos = GameObject.Find("Point").transform.position;
        randBobble = bobbleStyle[Random.Range(0, layerMaxBallNum)];
        m_shotball[1] = Instantiate(randBobble, shotpos, Quaternion.identity) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
        // Bobble going down
        timer += Time.deltaTime;
        if (timer > 10f)
        {
            timer = 0;
            topWall.transform.Translate(Vector2.left * Config.RADIUS);
            // Defeat detect
            for (int i = 0; i < Config.ROW; i++)
            {
                for (int j = 0; j < Config.COL - i % 2; j++)
                {
                    if (m_bobble[i, j].bobbleObject != null)
                    {
                        if (m_bobble[i, j].bobbleObject.transform.position.y <= (GameObject.Find("Down").gameObject.transform.position.y + Config.RADIUS))
                        {
                            Shooter.Instance.defeat = true;
                        }
                    }
                }
            }
        }
	}

    // Initiate base
    public void InitiLayer()
    {        
        // Clear the arrays
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (m_bobble[i, j].bobbleObject != null)
                {
                    Destroy(m_bobble[i, j].bobbleObject);
                    m_bobble[i, j].bobbleObject = null;
                }
                if (m_bobble[i, j].pointObject != null)
                {
                    Destroy(m_bobble[i, j].pointObject);
                    m_bobble[i, j].pointObject = null;
                }
            }
        }

        // Create point for bobble stop postion
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < (col - (i % 2)); j++)
            {
                m_bobble[i, j].pointObject = Instantiate(emptyPoint, new Vector3(emptyPointPos.x + j * 2 * Config.RADIUS + (i % 2) * Config.RADIUS, emptyPointPos.y - i * Config.RADIUS * Mathf.Sqrt(3)), Quaternion.identity) as GameObject;
                m_bobble[i, j].pointObject.transform.parent = topWall.transform;
                m_bobble[i, j].bobbleObject = null;
            }
        }

        // Create random bobbles
        for (int i = 0; i < layersInitiate; i++)
        {
            for (int j = 0; j < (col - (i % 2)); j++)
            {
                randBobble = bobbleStyle[Random.Range(0, layerMaxBallNum)];
                m_bobble[i, j].bobbleObject = Instantiate(randBobble, m_bobble[i, j].pointObject.transform.position, Quaternion.identity) as GameObject;
                m_bobble[i, j].bobbleObject.transform.parent = topWall.transform;  
                m_bobble[i, j].bobbleObject.GetComponent<Rigidbody2D>().isKinematic = true;
                m_bobble[i, j].bobbleObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                m_bobble[i, j].bobbleObject.tag = Config.STATICBOBBLE;

            }
        }

    }

}
