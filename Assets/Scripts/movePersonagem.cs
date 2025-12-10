using UnityEngine;
using UnityEngine.SceneManagement;

// Garante que o componente CharacterController exista no objeto
[RequireComponent(typeof(CharacterController))]
public class MovePersonagemUnity6 : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidade = 5.0f;
    [SerializeField] private float velocidadeRotacao = 720f; // Graus por segundo
    [SerializeField] private float alturaPulo = 1.2f;
    [SerializeField] private float gravidade = -20.0f;

    [Header("Referências Visuais")]
    // Tooltip ajuda a entender o que fazer no Inspector
    [Tooltip("Arraste o componente Animation aqui, se houver.")]
    [SerializeField] private Animation animacao; 

    // Variáveis privadas para controle interno
    private CharacterController _characterController;
    private Transform _cameraTransform;
    private float _velocidadeY; // Controle da gravidade (Eixo Y)

    public AudioClip somLose;
    public AudioClip puloSound;

    void Awake()
    {
        // Cache dos componentes para melhor performance
        _characterController = GetComponent<CharacterController>();

        // Tenta pegar a câmera principal de forma segura
        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("Atenção: Nenhuma câmera com a tag 'MainCamera' foi encontrada.");
        }

        // Se a animação não foi definida, tenta achar no objeto ou nos filhos
        if (animacao == null)
            animacao = GetComponentInChildren<Animation>();
    }

    void Update()
    {
        MoverPersonagem();
        GerenciarAnimacoes();
    }

    private void MoverPersonagem()
    {
        // 1. INPUT (Entrada do Jogador)
        // Unity 6 ainda suporta Input.GetAxis, mas recomenda-se o novo Input System para projetos futuros.
        float inputH = Input.GetAxisRaw("Horizontal"); 
        float inputV = Input.GetAxisRaw("Vertical");   

        // 2. CÁLCULO DA DIREÇÃO (Baseado na Câmera)
        Vector3 direcaoMovimento = Vector3.zero;

        if (_cameraTransform != null)
        {
            // Pega a frente e a direita da câmera
            Vector3 camFrente = _cameraTransform.forward;
            Vector3 camDireita = _cameraTransform.right;

            // Remove a inclinação Y (para não andar para o chão ou céu)
            camFrente.y = 0;
            camDireita.y = 0;

            // Normaliza para manter o comprimento do vetor igual a 1
            camFrente.Normalize();
            camDireita.Normalize();

            // Combina as direções
            direcaoMovimento = (camFrente * inputV) + (camDireita * inputH);
        }
        else
        {
            // Fallback se não houver câmera: move baseado no mundo (Global)
            direcaoMovimento = (Vector3.forward * inputV) + (Vector3.right * inputH);
        }

        // Normaliza se o jogador apertar duas teclas (W+D) para não andar mais rápido na diagonal
        if (direcaoMovimento.sqrMagnitude > 1f)
            direcaoMovimento.Normalize();

        // 3. ROTAÇÃO (O Pulo do Gato!)
        // Só rotacionamos se houver movimento significativo
        if (direcaoMovimento.sqrMagnitude > 0.05f)
        {
            // Cria uma rotação "olhando" para a direção do movimento
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoMovimento);
            
            // Aplica ao transform DESTE objeto (o pai de tudo)
            // RotateTowards faz a transição suave (Smooth)
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                rotacaoAlvo, 
                velocidadeRotacao * Time.deltaTime
            );
        }

        // 4. GRAVIDADE E PULO
        bool estaNoChao = _characterController.isGrounded;

        // Reseta a gravidade se estiver no chão (valor pequeno para garantir contato)
        if (estaNoChao && _velocidadeY < 0)
            _velocidadeY = -2f;

        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            // Fórmula da física para altura do pulo: v = sqrt(h * -2 * g)
            _velocidadeY = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            
            if (animacao != null) animacao.Play("JUMP");
            GetComponent<AudioSource>().PlayOneShot(puloSound, 0.7f);
        }

        // Aplica gravidade acumulativa
        _velocidadeY += gravidade * Time.deltaTime;

        // 5. APLICA O MOVIMENTO FINAL
        Vector3 movimentoFinal = direcaoMovimento * velocidade;
        movimentoFinal.y = _velocidadeY; // Insere a gravidade no vetor

        // Move o CharacterController
        _characterController.Move(movimentoFinal * Time.deltaTime);
    }

    private void GerenciarAnimacoes()
    {
        if (animacao == null) return;

        bool estaNoChao = _characterController.isGrounded;
        // Verifica se o personagem está se movendo horizontalmente (ignora Y)
        bool estaAndando = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z).magnitude > 0.1f;

        if (estaNoChao)
        {
            if (estaAndando)
            {
                if (!animacao.IsPlaying("WALK") && !animacao.IsPlaying("JUMP"))
                    animacao.CrossFade("WALK", 0.2f); // CrossFade suaviza a troca
            }
            else
            {
                if (!animacao.IsPlaying("IDLE") && !animacao.IsPlaying("JUMP"))
                    animacao.CrossFade("IDLE", 0.2f);
            }
        }
    }

void carregaFase()
    {
       // Application.LoadLevel("cena");
        SceneManager.LoadScene("cena");
    }

    private void OnTriggerEnter(Collider other)
    {
        // NOVIDADE: Usar CompareTag é mais rápido e seguro que "=="
        if (other.CompareTag("buraco"))
        {
            Invoke("carregaFase", 1f);
            GetComponent<AudioSource>().PlayOneShot(somLose, 0.7f);
        }
    }

}

