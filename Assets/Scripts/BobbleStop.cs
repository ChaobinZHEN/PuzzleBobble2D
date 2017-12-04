using UnityEngine;
using System.Collections;

public class BobbleStop : MonoBehaviour {
    
    private Transform m_ShotPos;
    private Transform m_transform;

    struct xy
    {
        public int x;
        public int y;
    }
    private xy m_xy;
    private int m_x = Config.ROW;
    private int m_y = Config.COL;

    private ArrayList m_listA = new ArrayList();
    private ArrayList m_listB = new ArrayList();
    private Stack m_stackA = new Stack();

    private int droppedCount = 0;

    public GameObject topWall;

    // Use this for initialization
    void Start () {
        m_transform = this.transform;
        m_ShotPos = GameObject.FindGameObjectWithTag("Player").transform;
        topWall = GameObject.FindWithTag("TopWall");

    }
	
	// Update is called once per frame
	void Update () {
	
	}


    void CreatShotBall()
    {
        CreateBobble.Instance.m_shotball[0] = CreateBobble.Instance.m_shotball[1];
        CreateBobble.Instance.m_shotball[0].transform.position = m_ShotPos.position;
        Vector3 shotpos = GameObject.Find("Point").transform.position;
        CreateBobble.Instance.m_shotball[1] = Instantiate(CreateBobble.Instance.bobbleStyle[Random.Range(0, CreateBobble.Instance.layerMaxBallNum)], shotpos, Quaternion.identity) as GameObject;
        CreateBobble.Instance.m_shotball[1].GetComponent<Rigidbody2D>().isKinematic = true;
        Shooter.Instance.m_Shoted = true;

    }

    void OnCollisionEnter2D (Collision2D other)
    {


        if (other.gameObject.tag == Config.STATICBOBBLE || other.gameObject.tag == Config.TOPWALL)
        {
            Destroy(GetComponent<BobbleStop>());
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tag = Config.STATICBOBBLE;
            m_xy = NearPoint(transform.position);
            transform.position = CreateBobble.Instance.m_bobble[m_xy.x, m_xy.y].pointObject.transform.position;
           
            // Create new bobble
            if (Shooter.Instance.creatable)
            { 
                CreatShotBall();
                Shooter.Instance.creatable = false;
            }

            // Put all the same color bobbles into list A
            m_listA.Clear();
            for (int i = 0; i < m_x; i++)
            {
                for (int j = 0; j < m_y - i % 2; j++)
                {
                    if (CreateBobble.Instance.m_bobble[i, j].bobbleObject != null) //不为空泡泡
                    {
                        if (CreateBobble.Instance.m_bobble[i, j].bobbleObject.GetComponent<BobbleProperty>().color == GetComponent<BobbleProperty>().color)
                        {
                            xy t_xy;
                            t_xy.x = i;
                            t_xy.y = j;
                            m_listA.Add(t_xy);
                        }
                    }
                }
            }

            CreateBobble.Instance.m_bobble[m_xy.x, m_xy.y].bobbleObject = this.gameObject;
            this.transform.parent = topWall.transform;

            // Defeat detect

            for (int i = 0; i < m_x; i++)
            {
                for (int j = 0; j < m_y - i % 2; j++)
                {
                    if (CreateBobble.Instance.m_bobble[i, j].bobbleObject != null)
                    {
                        if(CreateBobble.Instance.m_bobble[i,j].bobbleObject.transform.position.y <= (GameObject.Find("Down").gameObject.transform.position.y + Config.RADIUS))
                        {
                            Shooter.Instance.defeat = true;
                        }
                    }
                }
            }

            // Find the intersect same color bobbles and put them into list B
            all_intersect(m_xy);


            // If there are three same color intersect
            if (m_listB.Count >= 3)
            {
                for (int i = 0; i < m_listB.Count; i++)
                {
                    xy t_xy = (xy)m_listB[i];
                    CreateBobble.Instance.m_bobble[t_xy.x, t_xy.y].bobbleObject.GetComponent<BobbleProperty>().popped = true;
                    Shooter.Instance.PoppedScore();
                    CreateBobble.Instance.m_bobble[t_xy.x, t_xy.y].bobbleObject = null;


                }

            }


            // Drop bobbles
            m_listA.Clear();
            m_listB.Clear();
            m_stackA.Clear();

            // Put all the bobbles except row 1 into list A
            for (int i = 1; i < m_x; i++)
            {
                for (int j = 0; j < m_y - i % 2; j++)
                {
                    if (CreateBobble.Instance.m_bobble[i, j].bobbleObject != null)
                    {
                        xy t_xy;
                        t_xy.x = i;
                        t_xy.y = j;
                        m_listA.Add(t_xy);
                    }
                }
            }

            // Clean the bobble not intersect with row 1 out of list A
            for (int j = 0; j < m_y; j++)
            {
                if (CreateBobble.Instance.m_bobble[0, j].bobbleObject != null)
                {
                    xy t_xy;
                    t_xy.x = 0;
                    t_xy.y = j;
                    m_listA.Add(t_xy);
                    all_intersect(t_xy);
                }
            }


            if (m_listA.Count > 0)
            {
                for (int i = 0; i < m_listA.Count; i++)
                {
                    if (m_listA[i] != null)
                    {
                        xy t_xy = (xy)m_listA[i];
                        if (!CreateBobble.Instance.m_bobble[t_xy.x, t_xy.y].bobbleObject.GetComponent<BobbleProperty>().popped)
                        {
                            CreateBobble.Instance.m_bobble[t_xy.x, t_xy.y].bobbleObject.GetComponent<BobbleProperty>().dropped = true;
                            droppedCount++;
                            CreateBobble.Instance.m_bobble[t_xy.x, t_xy.y].bobbleObject = null;
                        }
                    }
                }
            }
            if (droppedCount > 0)
            {
                Shooter.Instance.DroppedScore(droppedCount);
            }
            droppedCount = 0;

            // Victory detect
            int temp = 1;
            for (int i = 0; i < m_x; i++)
            {
                for (int j = 0; j < m_y - i % 2; j++)
                {
                    if (CreateBobble.Instance.m_bobble[i, j].bobbleObject != null)
                    {
                        temp *= 0;
                    }
                }
            }
            if (temp == 1)
                Shooter.Instance.victory = true;
        }
    }
   
    // Find the near point when the bobble stop
    xy NearPoint(Vector2 point)
    {
        float length = 100f;
        xy nearpoint = new xy();

        for (int i = 0; i < m_x; i++ )
        {
            for (int j = 0; j < m_y - (i % 2); j++)
            {
                if (CreateBobble.Instance.m_bobble[i, j].bobbleObject == null)
                {
                    float templen = Vector3.Distance(point, CreateBobble.Instance.m_bobble[i, j].pointObject.transform.position);
                    if (templen < length)
                    {
                        length = templen;

                        nearpoint.x = i;
                        nearpoint.y = j;

                    }
                }
            }
        }
        return nearpoint;
    }

    public bool intersect(Vector3 vect1, float radius1, Vector3 vect2, float radius2)
    {
        return (Vector3.Distance(vect1, vect2) < (radius1 + radius2 + radius1 * 0.01f + radius2 * 0.01f));//加上1%的误差
    }

    void all_intersect(xy t_xy)
    {
        m_stackA.Clear();
        m_listB.Clear();
        m_stackA.Push(t_xy);
        xy judgxy;
        xy tempxy;
        while (m_stackA.Count > 0)
        {
            judgxy = (xy)m_stackA.Pop();
            for (int i = 0; i < m_listA.Count; i++)
            {
                if (m_listA[i] != null)
                {
                    tempxy = (xy)m_listA[i];
                    if (intersect(CreateBobble.Instance.m_bobble[judgxy.x, judgxy.y].bobbleObject.transform.position, Config.RADIUS, CreateBobble.Instance.m_bobble[tempxy.x, tempxy.y].bobbleObject.transform.position, Config.RADIUS))
                    {
                        m_stackA.Push(tempxy);
                        m_listA[i] = null;
                    }
                }
            }
            m_listB.Add(judgxy);

        }

    }

}
