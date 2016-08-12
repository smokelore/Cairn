using UnityEngine;
using System.Collections;

public class MouseHoverDisplay : MonoBehaviour 
{
    public string hoverComponentString;
    public KeyCode hoverKeyCode;
    public GameObject textPrefab;
    private TextMesh text;

    public void Start()
    {
        GameObject goText = (GameObject)Instantiate(textPrefab);
        text = goText.GetComponent<TextMesh>();
        goText.transform.parent = Camera.main.transform;
    }

    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        text.text = "";

        if (Input.GetKey(hoverKeyCode))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Component component = GetHoverComponent(hit.transform);
                if (component != null)
                {
                    text.text = GetComponentHoverString(component);
                    text.transform.position = hit.point + (-ray.direction * 2);
                    text.transform.LookAt(hit.point);
                }
            }
        }
    }

    private Component GetHoverComponent(Transform hitTransform)
    {
        if (hitTransform.GetComponent(hoverComponentString) != null)
        {
            return hitTransform.GetComponent(hoverComponentString);
        }
        else if (hitTransform.parent != null && hitTransform.parent.GetComponent(hoverComponentString) != null)
        {
            return hitTransform.parent.GetComponent(hoverComponentString);
        }

        return null;
    }

    private string GetComponentHoverString(Component component)
    {
        System.Type T = component.GetType();

        if (T.GetMethod("ToHoverString") != null)
        {
            return (string) T.GetMethod("ToHoverString").Invoke(component, null);
        }
        else
        {
            return component.transform.name;
        }
    }

    //public string GetHierarchyString(Transform transform)
    //{
    //    string hierarchy = transform.name;

    //    while (transform.parent != null)
    //    {
    //        transform = transform.parent;
    //        hierarchy = transform.name + "\n" + hierarchy;
    //    }

    //    return hierarchy;
    //}
}
