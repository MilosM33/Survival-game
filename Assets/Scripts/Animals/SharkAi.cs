using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAi : Fish
{
    public int health=10;
    public ParticleSystem particlesystem;
    public Transform player;

    public void TakeDamage(int damage,Vector3 pos)
    {
        health -= damage;
        StartCoroutine(showParticles());
        if(health <= 0)
        {
            AnimalController._AnimalController.StartCoroutine(AnimalController._AnimalController.SpawnShark(Random.Range(1, 10)));
            Destroy(gameObject);
        }

        target = pos;
    }
   
    IEnumerator showParticles()
    {
        particlesystem.gameObject.SetActive(true);
        particlesystem.Play();
        yield return new WaitForSeconds(Random.Range(0.5f,1.5f));
        particlesystem.gameObject.SetActive(false);
    }
    public override void Update()
    {

        if(player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime*speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), speed * Time.deltaTime);
        }
        else
        {
            base.Update();
        }
      
        

    }

    public override void OnTriggerEnter(Collider other)
    {
        //Empty
    }
    public override void ChooseSpot()
    {
        base.ChooseSpot();
        if(Random.Range(0, 500) == 1 && Player._Player.underwater)
        {
            Debug.Log("Shark comming");
            player = Player._Player.transform;
        }
    }
    
}
