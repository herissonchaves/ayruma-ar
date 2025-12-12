using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TriviaManager : MonoBehaviour
{
    [Header("Telas (UI)")]
    public GameObject painelQuiz;      // Tela das perguntas
    public GameObject painelResultado; // Tela final (NOVO)

    [Header("Elementos do Quiz")]
    public TextMeshProUGUI textoPergunta;
    public Button[] botoesResposta;
    public TextMeshProUGUI[] textosBotoes;
    public TextMeshProUGUI textoVidas;

    [Header("Elementos do Resultado")]
    public TextMeshProUGUI textoPontuacaoFinal; // Texto da tela final (NOVO)
    public Button botaoFecharResultado;         // Botão para sair da tela final (NOVO)

    [Header("Configurações do Jogo")]
    public Pergunta[] listaDePerguntas;
    public int totalVidas = 3;
    public int pontosPorAcerto = 10; // Quanto vale cada pergunta?

    // Variáveis de controle
    private int perguntaAtual;
    private int vidasAtuais;
    private int pontuacaoAtual; // Guarda os pontos (NOVO)

    [System.Serializable]
    public class Pergunta
    {
        public string enunciado;
        public string[] alternativas;
        public int indiceCorreto;
    }

    void Start()
    {
        // Garante que tudo comece fechado
        painelQuiz.SetActive(false);
        painelResultado.SetActive(false);

        // Configura os botões de resposta
        foreach(Button btn in botoesResposta) btn.onClick.RemoveAllListeners();
        for (int i = 0; i < botoesResposta.Length; i++)
        {
            int index = i;
            botoesResposta[i].onClick.AddListener(() => VerificarResposta(index));
        }

        // Configura o botão de fechar o resultado (se ele existir)
        if(botaoFecharResultado != null)
        {
            botaoFecharResultado.onClick.AddListener(FecharTudo);
        }
    }

    public void AbrirQuiz()
    {
        painelQuiz.SetActive(true);
        painelResultado.SetActive(false); // Esconde o resultado se tiver aberto
        
        // Reseta tudo para um novo jogo
        vidasAtuais = totalVidas;
        pontuacaoAtual = 0; // Zera a pontuação
        perguntaAtual = 0;
        
        AtualizarInterfaceVidas();
        CarregarPergunta();
    }

    // Função que fecha tudo (chamada pelo gatilho ou pelo botão)
    public void FecharTudo()
    {
        painelQuiz.SetActive(false);
        painelResultado.SetActive(false);
    }

    void CarregarPergunta()
    {
        if (perguntaAtual < listaDePerguntas.Length)
        {
            Pergunta p = listaDePerguntas[perguntaAtual];
            textoPergunta.text = p.enunciado;
            for(int i = 0; i < 4; i++)
            {
                textosBotoes[i].text = p.alternativas[i];
            }
        }
        else
        {
            // Acabaram as perguntas -> O jogador VENCEU
            MostrarFimDeJogo(true);
        }
    }

    void VerificarResposta(int indiceBotao)
    {
        if (indiceBotao == listaDePerguntas[perguntaAtual].indiceCorreto)
        {
            Debug.Log("Acertou!");
            pontuacaoAtual += pontosPorAcerto; // Soma pontos
            perguntaAtual++;
            CarregarPergunta();
        }
        else
        {
            vidasAtuais--; 
            AtualizarInterfaceVidas();

            if (vidasAtuais <= 0)
            {
                // Acabaram as vidas -> O jogador PERDEU
                MostrarFimDeJogo(false);
            }
        }
    }

    void MostrarFimDeJogo(bool venceu)
    {
        painelQuiz.SetActive(false); // Esconde as perguntas
        painelResultado.SetActive(true); // Mostra o resultado

        if (venceu)
        {
            textoPontuacaoFinal.text = "PARABÉNS!\nVocê completou o Quiz.\n\nPontuação Total: " + pontuacaoAtual;
        }
        else
        {
            textoPontuacaoFinal.text = "GAME OVER\nSuas vidas acabaram.\n\nPontuação Total: " + pontuacaoAtual;
        }
    }

    void AtualizarInterfaceVidas()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + vidasAtuais;
            if(vidasAtuais == 1) textoVidas.color = Color.red;
            else textoVidas.color = Color.white;
        }
    }
}