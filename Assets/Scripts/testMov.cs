using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovimentoTanque : MonoBehaviour
{
    public float velocidade = 5f;
    public float velocidadeRotacao = 180f; // graus por segundo
    public float gravidade = -20f;
    public float alturaPulo = 1.2f;

    private CharacterController _cc;
    private float _velocidadeY;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // gira
        float v = Input.GetAxisRaw("Vertical");   // anda

        // 1. ROTACIONA NO PRÓPRIO EIXO
        if (Mathf.Abs(h) > 0.01f)
        {
            transform.Rotate(0f, h * velocidadeRotacao * Time.deltaTime, 0f);
        }

        // 2. MOVIMENTO LOCAL (frente do personagem)
        Vector3 movimento = transform.forward * v * velocidade;

        // 3. GRAVIDADE + PULO
        bool noChao = _cc.isGrounded;
        if (noChao && _velocidadeY < 0f)
            _velocidadeY = -2f;

        if (Input.GetButtonDown("Jump") && noChao)
        {
            _velocidadeY = Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }

        _velocidadeY += gravidade * Time.deltaTime;
        movimento.y = _velocidadeY;

        // 4. APLICA MOVIMENTO
        _cc.Move(movimento * Time.deltaTime);

        // DEBUG: desenha a frente do personagem
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
    }
}
