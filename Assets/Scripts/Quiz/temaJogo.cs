using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class temaJogo : MonoBehaviour
{
    public Button btnPlay;
    public TextMeshProUGUI txtTema;
    public GameObject infoTema;
    public TextMeshProUGUI txtInfoTema;
    public GameObject estrela1;
    public GameObject estrela2;
    public GameObject estrela3;

    public string[] temas;
    public int numeroQuestoes;
    private int idTema;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idTema=0;
        txtTema.text = temas[idTema];
        txtInfoTema.text="Você acertou X de X questões";
        infoTema.SetActive(false);
        estrela1.SetActive(false);
        estrela2.SetActive(false);
        estrela3.SetActive(false);
        btnPlay.interactable = false;
    }
    public void selecioneTema(int i)
    {
        idTema = i;
        PlayerPrefs.SetInt("idTema", idTema);
        txtTema.text = temas[i];
        int notaF=PlayerPrefs.GetInt("notaFinal" + idTema.ToString());
        int acertos=PlayerPrefs.GetInt("acertos" + idTema.ToString());

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


        txtInfoTema.text="Você acertou " + acertos.ToString() + " de " + numeroQuestoes.ToString() + " questões";
        infoTema.SetActive(true);
        btnPlay.interactable = true;
    }
    public void Jogar()
    {
        SceneManager.LoadScene("quiz - tema" + idTema.ToString());
    }
    // Update is called once per frame

}
