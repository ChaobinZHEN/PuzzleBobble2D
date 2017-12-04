using UnityEngine;
using System.Collections;

public class BobbleProperty : MonoBehaviour {

    private float timer = 0;
    public bool popped = false;
    public bool dropped = false;
    public int color;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (popped)
        {
            timer += Time.deltaTime;
            GetComponent<Animator>().SetBool("Popped", true);
            if (timer > 0.2f)
            {
                timer = 0f;
                Destroy(this.gameObject);
            }
        }

        if (dropped)
        {
            tag = "Untagged";
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().gravityScale = 5f;


            timer += Time.deltaTime;
            if (timer > 2.0f)
            {
                timer = 0f;
                Destroy(this.gameObject);
            }
        }
    }



}
