using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Collections.Specialized;
using UnityEngine.SceneManagement;

public class responder : MonoBehaviour
{
    private int idTema;
    public TextMeshProUGUI pergunta;
    public TextMeshProUGUI respostaA;
    public TextMeshProUGUI respostaB;
    public TextMeshProUGUI respostaC;
    public TextMeshProUGUI respostaD;
    public TextMeshProUGUI infoRespostas;

    private int idPerguntas;

    private float acertos;
    private float questoes;
    private float media;
    private int notaFinal;

    public string[] perguntas;
    public string[] alternativasA;
    public string[] alternativasB;
    public string[] alternativasC;
    public string[] alternativasD;
    public string[] respostasCorretas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idTema = PlayerPrefs.GetInt("idTema");
        idPerguntas=0;
        questoes=perguntas.Length;
        pergunta.text=perguntas[idPerguntas];
        respostaA.text=alternativasA[idPerguntas];
        respostaB.text=alternativasB[idPerguntas];
        respostaC.text=alternativasC[idPerguntas];
        respostaD.text=alternativasD[idPerguntas];

        infoRespostas.text="Respondendo "+(idPerguntas+1).ToString()+" de "+questoes.ToString()+" perguntas.";
    }

    
    public void resposta(string alternativa)
    {
        if (alternativa == "A")
        {
            if(alternativasA[idPerguntas] == respostasCorretas[idPerguntas])
            {
                acertos+=1;
            }
        }
        else if (alternativa == "B")
        {
            if(alternativasB[idPerguntas] == respostasCorretas[idPerguntas])
            {
                acertos+=1;
            }
        }
        else if (alternativa == "C")
        {
            if(alternativasC[idPerguntas] == respostasCorretas[idPerguntas])
            {
                acertos+=1;
            }
        }
        else if (alternativa == "D")
        {
            if(alternativasD[idPerguntas] == respostasCorretas[idPerguntas])
            {
                acertos+=1;
            }
        }
        proximaPergunta();
    }

    void proximaPergunta()
    {
        idPerguntas+=1;
        if(idPerguntas < perguntas.Length)
        {
            pergunta.text=perguntas[idPerguntas];
            respostaA.text=alternativasA[idPerguntas];
            respostaB.text=alternativasB[idPerguntas];
            respostaC.text=alternativasC[idPerguntas];
            respostaD.text=alternativasD[idPerguntas];

            infoRespostas.text="Respondendo "+(idPerguntas+1).ToString()+" de "+questoes.ToString()+" perguntas.";
        }
        else
        {
            media = (acertos / questoes) * 10f;
            notaFinal = Mathf.RoundToInt(media);
            //grava apenas quando bate o recorde
            if(notaFinal>PlayerPrefs.GetInt("notaFinal"+idTema.ToString()))
            {
                PlayerPrefs.SetInt("notaFinal"+idTema.ToString(), notaFinal);
                PlayerPrefs.SetInt("acertos"+idTema.ToString(), (int) acertos);
            }
            //notas que sempre sao gravadas
            PlayerPrefs.SetInt("notaFinalTemp"+idTema.ToString(), notaFinal);
            PlayerPrefs.SetInt("acertosTemp"+idTema.ToString(), (int) acertos);
            

            SceneManager.LoadScene("quiz - notaFinal");
        }
    }





}
