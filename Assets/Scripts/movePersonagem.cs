using UnityEngine;

public class MovePersonagem : MonoBehaviour
{
    // Boas práticas: Variáveis privadas com [SerializeField] para aparecerem no Editor
    // mas ficarem protegidas no código.
    [SerializeField] private float velocidade = 5.0f;
    [SerializeField] private float velocidadeGiro = 300.0f;
    [SerializeField] private float forcaPulo = 8.0f;
    [SerializeField] private float gravidade = 20.0f; // Gravidade precisa ser forte para não parecer que estamos na lua

    private CharacterController characterController;
    private Vector3 direcaoMovimento = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. ROTAÇÃO (Girar o personagem)
        float inputHorizontal = Input.GetAxis("Horizontal"); // Teclas A/D ou Setas
        transform.Rotate(0, inputHorizontal * velocidadeGiro * Time.deltaTime, 0);

        // 2. MOVIMENTO NO CHÃO (Frente/Trás)
        // Só deixamos o jogador controlar a direção se ele estiver no chão (opcional, mas comum)
        if (characterController.isGrounded)
        {
            float inputVertical = Input.GetAxis("Vertical"); // Teclas W/S ou Setas
            
            // transform.forward já nos dá a direção que o personagem está olhando
            direcaoMovimento = transform.forward * inputVertical * velocidade;

            // 3. PULO
            if (Input.GetButton("Jump")) // Barra de Espaço
            {
                direcaoMovimento.y = forcaPulo;
            }
        }

        // 4. APLICAR GRAVIDADE (Manual)
        // Subtraímos a gravidade do eixo Y a cada frame
        direcaoMovimento.y -= gravidade * Time.deltaTime;

        // 5. MOVER O PERSONAGEM (Uma única vez por frame)
        characterController.Move(direcaoMovimento * Time.deltaTime);
    }
}