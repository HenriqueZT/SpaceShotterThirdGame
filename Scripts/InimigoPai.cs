using UnityEngine;

public class InimigoPai : MonoBehaviour
{
    //Atributo que TODOS os inimigos devem ter
    [SerializeField] protected float velocidade;
    [SerializeField] protected int vida;
    [SerializeField] protected GameObject explosao;
    [SerializeField] protected GameObject tiro;
    protected Rigidbody2D meuRB;
    [SerializeField]protected float timer;
    [SerializeField] protected float velocidadeTiros = 5f;
    [SerializeField] protected int pontos = 10;
    [SerializeField] protected GameObject powerUp;
    [SerializeField] protected float itemRate = 0.9f;

    //Som do tiro
    [SerializeField] protected AudioClip somTiros;

    //Pegando o som da explosao
    [SerializeField] private AudioClip somExplosao;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Criando um método com o som do tiro
    protected void TocaTiro()
    {
        AudioSource.PlayClipAtPoint(somTiros, Vector3.zero);
    }

    //Criando o metodo perde vida
    public void PerdeVida(int dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            Destroy(gameObject);

            Instantiate(explosao, transform.position, Quaternion.identity);
            //Tocando o audioClip da explosao apenas se apareço na tela
            if (transform.position.y > -5.5f)
            {
                AudioSource.PlayClipAtPoint(somExplosao, new Vector3(0f, 0f, 20f));
            }

            //Fazendo inimigo dropar power up
            //Dropando item se a variável power up é valida
            if (powerUp)
            { 
            
                DropaItem();
            }
            //Ganhando pontos
            var gerador = FindObjectOfType<GeradorInimigos>();
            //gerador.DiminuiQtd();
            //Ganhando pontos se o gerador for válido
            if (gerador)
            {
                gerador.GanhandoPontos(pontos);
            }
        }
    }

    //Evento de quando eu me destruo
    private void OnDestroy()
    {
        var gerador = FindObjectOfType<GeradorInimigos>();
        //Só executa o codigo se o gerador existe
        if (gerador)
        {
            gerador.DiminuiQtd();
        }
    }

    //Se destruindo com o destruidor
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Checando se a tag que estou colidindo é inimigo 
        if (other.CompareTag("Destruidor"))
        {
            Destroy(gameObject);
            Instantiate(explosao, transform.position, Quaternion.identity);
            //var gerador = FindObjectOfType<GeradorInimigos>();
            //gerador.DiminuiQtd();
        }
    }

    //Se destruindo ao encostar no player
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Jogador"))
        {
            Destroy(gameObject);

            //var gerador = FindObjectOfType<GeradorInimigos>();
            //gerador.DiminuiQtd();

            GameObject Power = Instantiate(explosao, transform.position, Quaternion.identity);

            //Tirando vida do player
            other.gameObject.GetComponent<PlayerController>().PerdeVida(1);
            DropaItem();
        }
    }

    public void DropaItem()
    {
        //Calculando a chance de dropar o item
        float chance = Random.Range(0f, 1f);

        if (chance > itemRate)//10% de chance de dropar o item
        {
            //Criando PowerUp e identificar ele
            GameObject pU = Instantiate(powerUp, transform.position, transform.rotation);
            //Mandando o pU ser destruido em 3f
            Destroy(pU, 5.2f);

            //Achando o RB dele
            //Fazendo ele se mover e dando a velocidade para ele
            //Dando uma posicao aleatoria
            Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            pU.GetComponent<Rigidbody2D>().velocity = dir;
        }
    }
}
