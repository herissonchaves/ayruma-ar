using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovePersonagemFinal : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject jogador;   // Objeto visual do personagem (malha)
    [SerializeField] private Animation animacao;   // Componente Animation

    [Header("Configuração")]
    public float velocidade = 5.0f;
    public float velocidadeRotacao = 720f; // 720°/s = bem responsivo
    public float alturaPulo = 1.2f;
    public float gravidade = -20.0f;       // negativo = puxando para baixo

    private CharacterController _characterController;
    private Transform _cameraTransform;
    private Transform _jogadorTransform;
    private float _velocidadeY;            // só o eixo vertical

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        // Se não arrastar nada no inspetor, usa o próprio objeto
        if (jogador == null)
            jogador = gameObject;

        _jogadorTransform = jogador.transform;

        // Tenta achar Animation no jogador (ou em filhos dele)
        if (animacao == null)
        {
            animacao = _jogadorTransform.GetComponentInChildren<Animation>();
        }

        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;
        else
            Debug.LogWarning("ALERTA: Câmera não encontrada. O movimento será relativo ao Mundo, não à câmera.");
    }

    void Update()
    {
        // 1. INPUT (Raw deixa mais responsivo)
        float inputH = Input.GetAxisRaw("Horizontal"); // A/D ou Setas Esq/Dir
        float inputV = Input.GetAxisRaw("Vertical");   // W/S ou Setas Cima/Baixo

        // 2. CALCULAR DIREÇÃO PLANA (XZ)
        Vector3 direcaoMovimento;

        if (_cameraTransform != null)
        {
            Vector3 camFrente = _cameraTransform.forward;
            Vector3 camDireita = _cameraTransform.right;

            camFrente.y = 0;
            camDireita.y = 0;

            if (camFrente.sqrMagnitude < 0.01f)
            {
                camFrente = Vector3.forward;
            }
            else
            {
                camFrente.Normalize();
            }

            camDireita.Normalize();

            direcaoMovimento = camFrente * inputV + camDireita * inputH;
        }
        else
        {
            direcaoMovimento = Vector3.forward * inputV + Vector3.right * inputH;
        }

        if (direcaoMovimento.sqrMagnitude > 1f)
            direcaoMovimento.Normalize();

        // 3. ROTAÇÃO: gira SEMPRE na direção em que está andando
        if (direcaoMovimento.sqrMagnitude > 0.0001f)
        {
            // Garante que não vamos inclinar pra cima/baixo
            Vector3 lookDir = new Vector3(direcaoMovimento.x, 0f, direcaoMovimento.z);

            // Se por algum motivo o vetor ficar zerado, não tenta girar
            if (lookDir.sqrMagnitude > 0.0001f)
            {
                Quaternion rotacaoAlvo = Quaternion.LookRotation(lookDir, Vector3.up);

                // IMPORTANTE: rotaciona o OBJETO VISUAL (jogador), não o capsule se forem diferentes
                _jogadorTransform.rotation = Quaternion.RotateTowards(
                    _jogadorTransform.rotation,
                    rotacaoAlvo,
                    velocidadeRotacao * Time.deltaTime
                );
            }
        }

        // 4. GRAVIDADE E PULO
        bool estaNoChao = _characterController.isGrounded;

        if (estaNoChao && _velocidadeY < 0)
            _velocidadeY = -2f; // "cola" no chão

        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            _velocidadeY = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            if (animacao != null)
                animacao.Play("JUMP");
        }

        _velocidadeY += gravidade * Time.deltaTime;

        // 5. MOVER O PERSONAGEM (XZ + Y)
        Vector3 movimentoFinal = direcaoMovimento * velocidade;
        movimentoFinal.y = _velocidadeY;

        _characterController.Move(movimentoFinal * Time.deltaTime);

        // 6. ANIMAÇÃO (IDLE / WALK)
        if (animacao != null && estaNoChao)
        {
            bool estaAndando = direcaoMovimento.sqrMagnitude > 0.01f;

            if (estaAndando)
            {
                if (!animacao.IsPlaying("WALK") && !animacao.IsPlaying("JUMP"))
                    animacao.Play("WALK");
            }
            else
            {
                if (!animacao.IsPlaying("IDLE") && !animacao.IsPlaying("JUMP"))
                    animacao.Play("IDLE");
            }
        }
    }
}
