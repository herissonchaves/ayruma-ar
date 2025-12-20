using UnityEngine;

public class movimentaCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector3 offSet;
    public GameObject jogador;
    void Start()
    {
        offSet = transform.position - jogador.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = jogador.transform.position + offSet;
    }
}
