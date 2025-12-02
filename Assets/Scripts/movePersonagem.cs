using UnityEngine;

public class MovePersonagem : MonoBehaviour
{
    [Header("Configurações de Movimento")] // Deixa o Inspector organizado
    [SerializeField] private float velocidade = 5.0f;
    [SerializeField] private float velocidadeGiro = 300.0f;
    [SerializeField] private float forcaPulo = 8.0f;
    [SerializeField] private float gravidade = 20.0f;

    // Variáveis de Componentes
    private CharacterController characterController;

    private Vector3 direcaoMovimento = Vector3.zero;

    public Animation animar;
    public GameObject jogador;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {
        // 1. Inputs
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        // 2. Rotação
        transform.Rotate(0, inputHorizontal * velocidadeGiro * Time.deltaTime, 0);

        // 3. Movimento
        if (characterController.isGrounded)
        {
            direcaoMovimento = transform.forward * inputVertical * velocidade;

            if (Input.GetButtonDown("Jump"))
            {
                direcaoMovimento.y = forcaPulo;
            }
        }
        else // Lógica para mover no ar mantendo a gravidade
        {
            float yAtual = direcaoMovimento.y;
            direcaoMovimento = transform.forward * inputVertical * velocidade;
            direcaoMovimento.y = yAtual;
        }

        // 4. Gravidade e Aplicação do Movimento
        direcaoMovimento.y -= gravidade * Time.deltaTime;
        characterController.Move(direcaoMovimento * Time.deltaTime);

        // 5. CONTROLAR A ANIMAÇÃO
        // Se a variável 'animacao' foi encontrada, atualizamos o estado

    }
}