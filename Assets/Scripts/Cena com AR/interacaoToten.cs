using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para trocar de cena

public class InteracaoTotem : MonoBehaviour
{
    public GameObject menuParaMostrar; // Aqui vamos colocar o painel
    public string nomeDaCenaNova; // Nome da cena para onde vamos

    // Isso acontece quando algo entra na área do totem
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem encostou tem a etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            menuParaMostrar.SetActive(true); // Mostra o menu
            
            // Libera o mouse para clicar (caso seu jogo trave o mouse)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Função para o botão SIM
    public void ClicouSim()
    {
        // Carrega a nova cena
        SceneManager.LoadScene(nomeDaCenaNova);
    }

    // Função para o botão NÃO
    public void ClicouNao()
    {
        menuParaMostrar.SetActive(false); // Esconde o menu
    }
}