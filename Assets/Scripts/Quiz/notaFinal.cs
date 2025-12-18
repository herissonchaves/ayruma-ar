using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class notaFinal : MonoBehaviour
{
    private int idTema;
    public TextMeshProUGUI textoNotaFinal;
    public TextMeshProUGUI txtinfoTema;
    public GameObject estrela1;
    public GameObject estrela2;
    public GameObject estrela3;

    private int notaF;
    private int acertos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idTema=PlayerPrefs.GetInt("idTema");
        estrela1.SetActive(false);
        estrela2.SetActive(false);
        estrela3.SetActive(false);
        idTema= PlayerPrefs.GetInt("idTema");

        notaF = PlayerPrefs.GetInt("notaFinalTemp" + idTema.ToString());
        acertos = PlayerPrefs.GetInt("acertosTemp" + idTema.ToString());

        textoNotaFinal.text=notaF.ToString();
        if((int) acertos==1)
        {
             txtinfoTema.text="Você acertou "+acertos.ToString()+" pergunta!";
        }
        else
        {
            txtinfoTema.text="Você acertou "+acertos.ToString()+" perguntas!";
        }
        

        if(notaF==10)
        {
            estrela1.SetActive(true);
            estrela2.SetActive(true);
            estrela3.SetActive(true);
        }
        else if(notaF>=7)
        {
            estrela1.SetActive(true);
            estrela2.SetActive(true);
            estrela3.SetActive(false);
        }
        else if(notaF>=4)
        {
            estrela1.SetActive(true);
            estrela2.SetActive(false);
            estrela3.SetActive(false);
        }

    }

    public void jogarNovamente()
    {
        SceneManager.LoadScene("quiz - tema" + idTema.ToString());
    }
 
}
