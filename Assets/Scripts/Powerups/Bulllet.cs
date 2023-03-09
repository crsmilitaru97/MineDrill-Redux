using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 dest;
    public Color dirtColor;
    public Color bloodColor;
    public GameObject splashParticles;

    void Update()
    {
        if (transform.position.normalized == dest.normalized)
        {
            if (transform.position.y < -1)
            {
                var particles = Instantiate(splashParticles, transform.position, Quaternion.identity);
                ParticleSystem.MainModule particlesSet = particles.GetComponent<ParticleSystem>().main;
                particlesSet.startColor = dirtColor;
                particles.SetActive(true);
            }
            Destroy(gameObject);
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, dest, Time.deltaTime * 15);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mob")
        {
            var particles = Instantiate(splashParticles, transform.position, Quaternion.identity);
            ParticleSystem.MainModule particlesSet = particles.GetComponent<ParticleSystem>().main;
            particlesSet.startColor = bloodColor;
            particles.SetActive(true);
            Destroy(gameObject);
        }
    }
}
