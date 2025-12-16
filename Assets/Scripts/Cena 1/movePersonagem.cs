using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Biblioteca Essencial do Unity 6

[RequireComponent(typeof(CharacterController))]
public class MovePersonagemUnity6 : MonoBehaviour
{
    [Header("Configurações de Input")]
    // Em vez de strings fixas, criamos variáveis editáveis no Inspector
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

    // Áudio
    public AudioClip somLose;
    public AudioClip puloSound;
    private AudioSource _audioSource;

    // --- IMPORTANTE: Ligar e Desligar os Inputs ---
    private void OnEnable()
    {
        // No Unity 6, precisamos "ligar a tomada" dos controles
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        // E desligar quando mudar de cena ou destruir o objeto
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();

        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;
        
        // Boa prática: TryGetComponent é mais seguro no Unity moderno, 
        // mas GetComponentInChildren funciona bem para hierarquias.
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
        // --- MUDANÇA PRINCIPAL AQUI ---
        // Unity 5: Input.GetAxis("Horizontal")
        // Unity 6: Lemos o valor direto da Ação configurada
        Vector2 inputMovimento = moveAction.ReadValue<Vector2>();

        float inputH = inputMovimento.x;
        float inputV = inputMovimento.y;

        // Lógica de Direção (Idêntica à do professor, matemática não muda)
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

        if (direcaoMovimento.sqrMagnitude > 1f)
            direcaoMovimento.Normalize();

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

        if (estaNoChao && _velocidadeY < 0)
            _velocidadeY = -2f;

        // Unity 5: Input.GetButtonDown("Jump")
        // Unity 6: Verificamos se a ação foi acionada neste quadro
        if (jumpAction.WasPressedThisFrame() && estaNoChao)
        {
            _velocidadeY = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            
            if (animacao != null) animacao.Play("JUMP");
            
            if (_audioSource != null && puloSound != null) 
                _audioSource.PlayOneShot(puloSound, 0.7f);
        }

        _velocidadeY += gravidade * Time.deltaTime;

        Vector3 movimentoFinal = direcaoMovimento * velocidade;
        movimentoFinal.y = _velocidadeY;

        _characterController.Move(movimentoFinal * Time.deltaTime);
    }

    private void GerenciarAnimacoes()
    {
        if (animacao == null) return;

        bool estaNoChao = _characterController.isGrounded;
        // Verifica a magnitude apenas nos eixos X e Z (ignora o pulo Y)
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
    
    // Funções extras
     void carregaFase()
    {
        // Boas práticas modernas: Usar LoadScene em vez de Application.LoadLevel (que era do Unity 5)
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