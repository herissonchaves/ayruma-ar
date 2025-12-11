using UnityEngine;
using TMPro; // Biblioteca para mexer com textos bonitos
using UnityEngine.UI; // Biblioteca para mexer com Botões

public class TriviaManager : MonoBehaviour
{
    [Header("Configurações da UI")]
    public GameObject painelQuiz; // O painel que contem tudo
    public TextMeshProUGUI textoPergunta; // Onde escreve a pergunta
    public Button[] botoesResposta; // Lista dos seus botões
    public TextMeshProUGUI[] textosBotoes; // Texto dentro dos botões

    [Header("Perguntas")]
    public Pergunta[] listaDePerguntas; // Lista de perguntas que vamos criar no Inspector

    private int perguntaAtual;

    // Essa 'classe' define o que é uma pergunta
    [System.Serializable]
    public class Pergunta
    {
        public string enunciado;
        public string[] alternativas; // Tem que ter 4
        public int indiceCorreto; // 0, 1, 2 ou 3
    }

    void Start()
    {
        painelQuiz.SetActive(false); // Garante que comece escondido
        // Adiciona a função de clique em cada botão
        for (int i = 0; i < botoesResposta.Length; i++)
        {
            int index = i; // Necessário para salvar o índice correto
            botoesResposta[i].onClick.AddListener(() => VerificarResposta(index));
        }
    }

    // Função chamada para iniciar o jogo (vamos chamar quando o sapo encostar em algo)
    public void AbrirQuiz()
    {
        painelQuiz.SetActive(true);
        perguntaAtual = 0;
        CarregarPergunta();
    }

    void CarregarPergunta()
    {
        if (perguntaAtual < listaDePerguntas.Length)
        {
            // Pega a pergunta atual
            Pergunta p = listaDePerguntas[perguntaAtual];
            
            // Coloca os textos na tela
            textoPergunta.text = p.enunciado;
            for(int i = 0; i < 4; i++)
            {
                textosBotoes[i].text = p.alternativas[i];
            }
        }
        else
        {
            Debug.Log("Fim do Jogo! Você venceu.");
            painelQuiz.SetActive(false);
        }
    }

    void VerificarResposta(int indiceBotao)
    {
        if (indiceBotao == listaDePerguntas[perguntaAtual].indiceCorreto)
        {
            Debug.Log("Acertou!");
            perguntaAtual++; // Vai para a próxima
            CarregarPergunta();
        }
        else
        {
            Debug.Log("Errou! Tente de novo.");
            // Aqui você pode reiniciar ou tirar vida
        }
    }
}