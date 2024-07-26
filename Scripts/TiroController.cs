using UnityEngine;

public class TiroController : MonoBehaviour
{
    private Rigidbody2D meuRB;
    [SerializeField] private float velocidade;
    // Start is called before the first frame update

    [SerializeField] private GameObject impacto;
    void Start()
    {
        meuRB = GetComponent<Rigidbody2D>();
        //Indo para cima
        //meuRB.velocity = new Vector2(0f, velocidade);

        
    }

    // Update is called once per frame
    void Update()
    {
        Destruir();
    }

    //Configurando destruição dos tiros ao colidir
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //Pegar o método PerdeVida e aplicar nele o dano(other)
        //Isso so deve rodar se ele colidiu com alguem que tem o script inimigo 01 controller
        //Checando se a tag que estou colidindo é inimigo 
        if (collision.CompareTag("Inimigo"))
        {
            var inimigo =collision.GetComponent<InimigoPai>();
            //Fazendo inimigo perder vida
            inimigo.PerdeVida(1);

        }
        else if (collision.CompareTag("Jogador"))
        {
            collision.GetComponent<PlayerController>().PerdeVida(1);
        }
        Destroy(gameObject);

        //Criando o impacto
        Instantiate(impacto, transform.position, Quaternion.identity);
    }

    private void Destruir()
    {
        //Destruindo os tiros ao passar da tela para nao acertar inimigos distantes
        //Poderia colocar um if assim no Perde vida, para os tiros não darem danos, mas fiz assim
        if(transform.position.y > 5f ||  transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
}
