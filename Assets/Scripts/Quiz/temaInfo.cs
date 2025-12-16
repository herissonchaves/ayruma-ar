using UnityEngine;

public class temaInfo : MonoBehaviour
{
    public int idTema;
    public GameObject estrela1;
    public GameObject estrela2;
    public GameObject estrela3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        estrela1.SetActive(false);
        estrela2.SetActive(false);
        estrela3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
