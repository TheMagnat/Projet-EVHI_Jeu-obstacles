using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{

    public float speed = 5f;

    private float startFadeDist = 50f;
    private float fadeDist = 20f;

    private float maxDist = 120f;

    private float opacity = 1f;

    private Vector3 direction;

    private Vector3 target;
    public int dodgeMode = -1;

    public float dodgeDeviation = 20f;

    public bool breakShield = false;

    private bool finished = false;

    private GameObject playerObj;

    public Material armorBreakMaterial;


    private UserHandler userHandler;


    // Start is called before the first frame update
    void Start()
    {

        userHandler = GameObject.FindGameObjectWithTag("UserHandler").GetComponent<UserHandler>();


        playerObj = GameObject.FindGameObjectWithTag("Player");
        target = playerObj.transform.position;
        target.y += 1.5f;

        transform.LookAt(target);

        direction = target - transform.position;
        direction = direction.normalized;

        dodgeMode = 3;

        if (dodgeMode >= 0)
        {

            switch (dodgeMode)
            {
                case 1:
                    transform.Translate(dodgeDeviation, 0f, 0f);
                    break;

                case 2:
                    transform.Translate(-dodgeDeviation, 0f, 0f);
                    break;

                case 3:
                    breakShield = true;
                    GetComponent<MeshRenderer>().material = armorBreakMaterial;
                    break;

                default:
                    break;
            }

            
        }


    }

    private void FixedUpdate()
    {
        Vector3 transla = direction * speed * Time.deltaTime;

        transform.Translate(transla, Space.World);

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target);

        if (distance <= 0.3f && !finished)
        {
            finished = true;

            Debug.Log(playerObj.transform.position);

            var relativePoint = transform.InverseTransformPoint(playerObj.transform.position);
            if (relativePoint.x < 0.0)
            {
                //Add to right
                userHandler.strategieEvitement[2] += 1;
                Debug.Log("Object is to the left");
            }
            else if (relativePoint.x >= 0.0)
            {
                //Add to left
                userHandler.strategieEvitement[1] += 1;
                Debug.Log("Object is to the right");
            }

        }

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
    }

    void changeAlpha(float alphaValue)
    {

        Renderer rend = gameObject.GetComponent<Renderer>();

        Color oldMat = rend.material.color;
        Color newColor = new Color(oldMat.r, oldMat.g, oldMat.b, alphaValue);

        rend.material.color = newColor;
    }

}
