using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        //Garantindo que só existe 1 game manager por vez
        //Contando quantos game managers existem na cena
        int quantidade = FindObjectsOfType<GameManager>().Length;
        if ( quantidade > 1)
        {
            Destroy(gameObject);
        }

        //Eu não vou ser destruido quando mudar de cena
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Criando o método para ir para o jogo
    public void IniciaJogo()
    {
        //Carregar a cena que tem o meu jogo
        SceneManager.LoadScene(1);
    }

    //Criando um método que roda depois de um certo tempo
    IEnumerator PrimeiraCena()
    {
        yield return new WaitForSeconds(2f);
        //Todo codigo daqui so ira rodar depois de 2 segundos;
        SceneManager.LoadScene(0);
    }

    //Criando o metodo para ir para o inicio
    public void Inicio()
    {
        //Carregar a cena que tem o meu jogo
        //SceneManager.LoadScene(0);
        //Iniciando minha corotina
        StartCoroutine(PrimeiraCena());
    }

    public void Sair()
    {
        //Fechar o jogo
        Application.Quit();
    }
}
