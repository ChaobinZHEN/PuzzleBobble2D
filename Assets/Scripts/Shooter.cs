using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public static Shooter Instance;
    public float m_speed = 2f;
    public bool m_Shoted;


    private Transform m_transform;
    private Transform m_childform;
    private Transform m_ppoint;

    private Vector2 m_ShotVector3;
    private Vector2 m_creatPos;
    private Vector2 m_transformpos;

    public float m_rotSpeed = 40f;

    public bool creatable;

    public bool defeat = false;

    public bool victory = false;

    public GameObject gameOver;
    public GameObject win;

    private int score;

    public Text scoreText;





    // Use this for initialization
    void Start()
    {
        Instance = this;
        m_transform = this.transform;
        m_childform = m_transform.Find("shooter");
        m_ppoint = m_childform.Find("Point_p");
        m_transformpos = m_transform.position;
        m_Shoted = true;
        creatable = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Angle(m_transform.up, Vector3.left) >= 15.0f)
        {
            if (Input.GetKey(KeyCode.A))
            {
                m_transform.Rotate(Vector3.forward * Time.deltaTime * m_rotSpeed);
            }

        }

        if (Vector3.Angle(m_transform.up, Vector3.right) >= 15.0f)
        {
            if (Input.GetKey(KeyCode.D))
            {
                m_transform.Rotate(Vector3.back * Time.deltaTime * m_rotSpeed);
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if ((Input.GetKeyDown(KeyCode.Space)) && m_Shoted)
        {
            Shoot();
        }

        SetScoreText();

        if (defeat)
            GameOver();

        if (victory)
            Win();
    }

    void Shoot(){
        GameObject m_ballobject = CreateBobble.Instance.m_shotball[0];
        m_creatPos = m_ppoint.position;
        m_ballobject.transform.position = m_ppoint.position;
        m_ShotVector3 = m_creatPos - m_transformpos;
        m_ballobject.GetComponent<Rigidbody2D>().isKinematic = false;
        m_ballobject.GetComponent<Rigidbody2D>().velocity = m_ShotVector3 * m_speed;
        m_Shoted = false;
        creatable = true;
        m_ballobject.AddComponent<BobbleStop>();
    }

    public void PoppedScore()
    {
        score += 10;
    }

    public void DroppedScore(int count)
    {
        score += 10 * (int)Mathf.Pow(2, count);
    }

    void SetScoreText() {
        scoreText.text = "Score: " + score.ToString();
    }

    public string GetScoreText() {
        return score.ToString();
    }

    void GameOver() {
        defeat = false;
        Instantiate(gameOver, Vector3.zero, Quaternion.identity);
        Time.timeScale = 0f;
    }

    void Win() {
        defeat = false;
        Instantiate(win, Vector3.zero, Quaternion.identity);
        Time.timeScale = 0f;
    }

}
