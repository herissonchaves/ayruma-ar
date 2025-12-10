using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // 1. Necessário para o novo sistema

[RequireComponent(typeof(CharacterController))]
public class MovePersonagemUnity6 : MonoBehaviour
{
    [Header("Configurações de Input (Novo)")]
    // 2. Criamos "espaços" para configurar os controles no Inspector
    public InputAction moveAction; 
    public InputAction jumpAction;

    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidade = 5.0f;
    [SerializeField] private float velocidadeRotacao = 720f;
    [SerializeField] private float alturaPulo = 1.2f;
    [SerializeField] private float gravidade = -20.0f;

    [Header("Referências Visuais")]
    [Tooltip("Arraste o componente Animation aqui, se houver.")]
    [SerializeField] private Animation animacao; 

    private CharacterController _characterController;
    private Transform _cameraTransform;
    private float _velocidadeY;

    public AudioClip somLose;
    public AudioClip puloSound;

    // 3. O novo sistema precisa ser "Ligado" e "Desligado" manualmente
    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;
        else
            Debug.LogWarning("Atenção: Nenhuma câmera com a tag 'MainCamera' foi encontrada.");

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
        // 4. Lendo os valores do novo sistema
        // Em vez de ler Horizontal e Vertical separados, lemos um Vector2 (X e Y juntos)
        Vector2 inputMovimento = moveAction.ReadValue<Vector2>();

        // Separa os valores para usar na lógica antiga
        float inputH = inputMovimento.x;
        float inputV = inputMovimento.y;

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

        // FÍSICA E PULO
        bool estaNoChao = _characterController.isGrounded;

        if (estaNoChao && _velocidadeY < 0)
            _velocidadeY = -2f;

        // 5. Verificando se o botão foi pressionado neste frame
        if (jumpAction.WasPressedThisFrame() && estaNoChao)
        {
            _velocidadeY = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            
            if (animacao != null) animacao.Play("JUMP");
            if (GetComponent<AudioSource>() != null) // Boa prática: checar null antes de tocar
                GetComponent<AudioSource>().PlayOneShot(puloSound, 0.7f);
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

    void carregaFase()
    {
        SceneManager.LoadScene("cena");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("buraco"))
        {
            Invoke("carregaFase", 1f);
            if(somLose != null) GetComponent<AudioSource>().PlayOneShot(somLose, 0.7f);
        }
    }
}