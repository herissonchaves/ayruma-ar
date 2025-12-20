using UnityEngine;

public class GatilhoQuiz : MonoBehaviour
{
    public TriviaManager oGerenteDoQuiz;

    // Quando o sapo ENTRA no cubo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            oGerenteDoQuiz.AbrirQuiz();
        }
    }

    // Quando o sapo SAI do cubo
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            // AQUI ESTAVA O ERRO: Mudamos de FecharQuiz para FecharTudo
            oGerenteDoQuiz.FecharTudo(); 
        }
    }
}