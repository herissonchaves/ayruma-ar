using UnityEngine.SceneManagement;
using UnityEngine;
public class comandosBasicos : MonoBehaviour
{
    public void carregaCena (string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void resetarPontuacoes()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("quiz - menu_temas");
    }

}
