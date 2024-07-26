using UnityEngine;

public class ExplosaoController : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Morrendo()
    {
        Destroy(gameObject);
    }
}
