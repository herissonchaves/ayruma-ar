using UnityEngine;

public class moveOffset : MonoBehaviour
{
    private Material materialAtual;
    private float offset;
    public float velocidade=1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     materialAtual = GetComponent<Renderer>().material;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offset +=0.001f;
        materialAtual.SetTextureOffset("_MainTex", new Vector2(offset * velocidade, 0)); 
    }
}
