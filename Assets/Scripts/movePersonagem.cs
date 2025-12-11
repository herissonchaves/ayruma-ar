using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Biblioteca do Unity 6

// Garante que o personagem tenha o componente físico necessário
[RequireComponent(typeof(CharacterController))]
public class MovePersonagemUnity6 : MonoBehaviour
{
    [Header("Configurações de Input (Os 'Ouvidos' do Script)")]
    // Essas variáveis vão aparecer no Inspector para você configurar quais botões ler
    public InputAction moveAction; 
    public InputAction jumpAction;

    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidade = 5.0f;
    [SerializeField] private float velocidadeRotacao = 720f;
    [SerializeField] private float alturaPulo = 1.2f;
    [SerializeField] private float gravidade = -20.0f;

    [Header("Referências Visuais")]
    [SerializeField] private Animation animacao; 

    private CharacterController _characterController;
    private Transform _cameraTransform;
    private float _velocidadeY;

    // Áudio (Opcional)
    public AudioClip somLose;
    public AudioClip puloSound;
    private AudioSource _audioSource; // Criamos uma referência privada para ficar organizado

    // --- CICLO DE VIDA DO INPUT SYSTEM ---
    // Diferente do Unity 5, precisamos "Ligar" as ações quando o objeto ativa
    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    // E "Desligar" quando desativa para não dar erro de memória
    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();

        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;
        
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
        // 1. Lendo o Joystick (Vem como um Vector2: X horizontal, Y vertical)
        // Isso lê tanto o teclado (WASD) quanto o Joystick da tela se configurado corretamente
        Vector2 inputMovimento = moveAction.ReadValue<Vector2>();

        float inputH = inputMovimento.x;
        float inputV = inputMovimento.y;

        // Lógica de Direção (Mantida do original, funciona bem)
        Vector3 direcaoMovimento = Vector3.zero;

        if (_cameraTransform != null)
        {
            Vector3 camFrente = _cameraTransform.forward;
            Vector3 camDireita = _cameraTransform.right;

            camFrente.y = 0;
            camDireita.y = 0;
            camFrente.Normalize();
            camDireita.Normalize();

            direcaoMovimento = (camFrente * inputV) + (camDireita * inputH);
        }
        else
        {
            direcaoMovimento = (Vector3.forward * inputV) + (Vector3.right * inputH);
        }

        // Normaliza para não andar mais rápido na diagonal
        if (direcaoMovimento.sqrMagnitude > 1f)
            direcaoMovimento.Normalize();

        // Rotaciona o personagem para onde ele está andando
        if (direcaoMovimento.sqrMagnitude > 0.05f)
        {
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoMovimento);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                rotacaoAlvo, 
                velocidadeRotacao * Time.deltaTime
            );
        }

        // --- PULO ---
        bool estaNoChao = _characterController.isGrounded;

        // Resetar velocidade Y se estiver no chão
        if (estaNoChao && _velocidadeY < 0)
            _velocidadeY = -2f;

        // 2. Verificando se o botão de Pulo foi apertado neste frame
        if (jumpAction.WasPressedThisFrame() && estaNoChao)
        {
            // Fórmula física do pulo
            _velocidadeY = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            
            if (animacao != null) animacao.Play("JUMP");
            
            // Toca o som se existir o componente e o clip
            if (_audioSource != null && puloSound != null) 
                _audioSource.PlayOneShot(puloSound, 0.7f);
        }

        // Aplica gravidade
        _velocidadeY += gravidade * Time.deltaTime;

        // Move o CharacterController
        Vector3 movimentoFinal = direcaoMovimento * velocidade;
        movimentoFinal.y = _velocidadeY;

        _characterController.Move(movimentoFinal * Time.deltaTime);
    }

    private void GerenciarAnimacoes()
    {
        if (animacao == null) return;

        bool estaNoChao = _characterController.isGrounded;
        // Verifica se o personagem está se movendo horizontalmente
        bool estaAndando = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z).magnitude > 0.1f;

        if (estaNoChao)
        {
            if (estaAndando)
            {
                if (!animacao.IsPlaying("WALK") && !animacao.IsPlaying("JUMP"))
                    animacao.CrossFade("WALK", 0.2f);
            }
            else
            {
                if (!animacao.IsPlaying("IDLE") && !animacao.IsPlaying("JUMP"))
                    animacao.CrossFade("IDLE", 0.2f);
            }
        }
    }
    
    // Funções extras (triggers, etc) mantidas...
     void carregaFase()
    {
        SceneManager.LoadScene("cena");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("buraco"))
        {
            Invoke("carregaFase", 1f);
            if(somLose != null && _audioSource != null) _audioSource.PlayOneShot(somLose, 0.7f);
        }
    }
}