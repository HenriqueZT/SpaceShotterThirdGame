using UnityEngine;

public class AnimacaoBoss : MonoBehaviour
{

    [SerializeField] private GameObject Boss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CriaBoss()
    {
        Instantiate(Boss, transform.position, transform.rotation);
    }

    //Método para ir para o inicio do jogo
    public void VoltarInicio()
    {
        var iniciar = FindObjectOfType<GameManager>();
        if (iniciar)
        {
            iniciar.Inicio();
        }
    }

}
