using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
    [Range(0,1)]
    [SerializeField] private float volumeinicial;
    [Range(0, 1)]
    [SerializeField] private float menorvol;
    [Range(0, 1)]
    [SerializeField] private float maiorvol;
    [Range(-1, 0)]
    [SerializeField] private float menorpan;
    [Range(0, 1)]
    [SerializeField] private float maiorpan;
    
      float incrementoaudio;

    Rigidbody rb;
    AudioSource ad;
	 
	private float nextFire;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ad = GetComponent<AudioSource>();
        incrementoaudio = maiorvol - volumeinicial;
    }

    void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            //GetComponent<AudioSource>().Play ();
            maiorvol = 1.0f - volumeinicial;
            ad.volume = volumeinicial + convertEscal(boundary.xMin, boundary.xMax, Mathf.Abs(transform.position.x), menorvol, maiorvol);
            ad.panStereo = convertEscal(boundary.xMin,boundary.xMax, transform.position.x, menorpan, maiorpan);
            ad.Play();
        }
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;
		
		GetComponent<Rigidbody>().position = new Vector3
		(
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
		
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}


    private float convertEscal(float minimo, float maximo, float valorAtual, float minimoFinal, float maxFinal)
    {
        return ((valorAtual- minimo) / (maximo- minimo))*(maxFinal- minimoFinal) + minimoFinal ;
    }
}
