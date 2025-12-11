using UnityEngine;

public class GatilhoQuiz : MonoBehaviour
{
    public TriviaManager oGerenteDoQuiz;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem bateu foi o Jogador (precisa ter a tag "Player" ou o nome certo)
        if (other.name == "NomeDoSeuSapoAqui" || other.CompareTag("Player")) 
        {
            oGerenteDoQuiz.AbrirQuiz();
            Destroy(gameObject); // Destroi o gatilho para não ativar de novo
        }
    }
}