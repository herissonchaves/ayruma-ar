using JetBrains.Annotations;
using UnityEngine;

public class giraObjeto : MonoBehaviour
{
    public Transform objetoGirar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (objetoGirar != null)
            objetoGirar.Rotate(new Vector3(0, 80, 0) * Time.deltaTime);
    }
}
