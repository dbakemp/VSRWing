using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject Parent;
    public GameObject RedPanel;
    public GameObject GreenPanel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Parent != null)
        {
            gameObject.transform.position = Parent.transform.position + new Vector3(0, 0.4f, 0);
        }
    }

    public void SetPercentage(int percentage)
    {
        GreenPanel.GetComponent<RectTransform>().sizeDelta = new Vector2((gameObject.GetComponent<RectTransform>().sizeDelta.x / 100.0f) * percentage, GreenPanel.GetComponent<RectTransform>().sizeDelta.y);
    }
}
