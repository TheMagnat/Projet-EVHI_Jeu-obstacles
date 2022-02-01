using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{

    public float speed = 5f;
    public float space = 10f;


    private float startFadeDist = 50f;
    private float fadeDist = 20f;

    private float maxDist = 120f;

    private float opacity = 1f;

    private Vector3 direction;

    private GameObject leftWall;
    private GameObject rightWall;

    public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {

        transform.LookAt(target);

        direction = target - transform.position;
        direction = direction.normalized;

        leftWall = transform.GetChild(0).gameObject;
        rightWall = transform.GetChild(1).gameObject;

        leftWall.transform.Translate(-space/2f, 0f, 0f);
        rightWall.transform.Translate(space/2f, 0f, 0f);

    }

    private void FixedUpdate()
    {
        Vector3 transla = direction * speed * Time.deltaTime;

        transform.Translate(transla, Space.World);

    }

    // Update is called once per frame
    void Update()
    {

        
        //Debug.Log(direction);
        //leftWall.transform.Translate(direction * speed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, new Vector3(0f, 0f, 0f));

        opacity = Mathf.Min(1f - Mathf.Max(0f, dist - startFadeDist) / fadeDist, 1f);

        
        if (dist >= maxDist)
        {
            Destroy(gameObject);
        }

        if (opacity < 1f)
        {
            changeAlpha(opacity);
        }


    //rend.material.shader = Shader.Find("HDRenderPipeline/Lit");
    //rend.material.SetColor("_Color", color);

        //Debug.Log(Vector3.Distance(transform.position, new Vector3(0f, 0f, 0f)));
    }

    void changeAlpha(float alphaValue)
    {

        Renderer rendLeft = leftWall.GetComponent<Renderer>();
        Renderer rendRight = rightWall.GetComponent<Renderer>();

        Color oldMat = rendLeft.material.color;
        Color newColor = new Color(oldMat.r, oldMat.g, oldMat.b, alphaValue);

        rendLeft.material.color = newColor;
        rendRight.material.color = newColor;
    }
}
