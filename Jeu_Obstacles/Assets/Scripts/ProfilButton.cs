using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilButton : MonoBehaviour
{
    [SerializeField]
    private Text myname;

    public void SetText(string name){
        myname.text = name;
    }
}
