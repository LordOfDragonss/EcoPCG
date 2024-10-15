using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using static Enums;
using static UnityEngine.GraphicsBuffer;

public class CreatureController : MonoBehaviour
{
    public bool isPreSpawned;
    public Creature creature;
    public CreatureSettings settings;
    public SphereCollider VisionCircle;
    public StatDisplay statDisplay;
    public bool hasTarget;
    private bool hasRandomTarget;
    private Vector3 randomDestination;
    public LayerMask groundLayer;
    private bool isDying;
    private float dyingTimer;
    [SerializeField] private float HungerDecay;
    Rigidbody rb;
    public bool hasProcreated;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (isPreSpawned)
            creature = InitiateCreature(settings);
        hasProcreated = false;
        GetComponent<MeshRenderer>().material.color = creature.color;
        statDisplay.DisableCanvas();
        statDisplay.creature = creature;
    }

    public void DisplayStats()
    {
        if (!statDisplay.isVisible)
        {
            statDisplay.EnableCanvas();
        }
        else
        {
            statDisplay.DisableCanvas();
        }
    }

    public Creature InitiateCreature(CreatureSettings settings)
    {
        return new Creature(settings);
    }


    private void Update()
    {
        
        transform.localScale = new Vector3(creature.size, creature.size, creature.size);
        VisionCircle.radius = creature.VisionRadius;
        if (creature.Hunger > 0)
        {
            creature.Hunger -= HungerDecay;
        }
        if(creature.Hunger > creature.maxHunger)
        {
            creature.Hunger = creature.maxHunger;
        }
        if (creature.Hunger <= 0)
        {
            isDying = true;
            creature.Hunger = 0;
            dyingTimer = 3;
        }
        if (isDying)
        {
            dyingTimer -= 0.1f * Time.time;
        }
        if (isDying && dyingTimer <= 0)
        {
            Die();
        }
        if (!hasTarget && creature.Hunger < creature.maxHunger)
        {
            if (!hasRandomTarget) randomDestination = PickRandomDirection();
            if (hasRandomTarget) MoveTowards(randomDestination);
            if (hasRandomTarget && Vector3.Distance(transform.position, randomDestination) < 1) hasRandomTarget = false;
        }
    }

    Vector3 PickRandomDirection()
    {
        float x = UnityEngine.Random.Range(-creature.WalkRange, creature.WalkRange);
        float z = UnityEngine.Random.Range(-creature.WalkRange, creature.WalkRange);


        Vector3 newDest = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        if (Physics.Raycast(newDest, Vector3.down, groundLayer))
        {
            hasRandomTarget = true;
        }
        return newDest;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Plant" && creature.diet == Diet.Herbivore || creature.diet == Diet.Omnivore)
        {
            creature.Hunger +=  other.gameObject.GetComponent<Food>().HungerValue;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Creature")
        {
            CreatureController OpposingCreatureController = other.gameObject.GetComponent<CreatureController>();
            if (OpposingCreatureController != null)
            {
                Creature Opposingcreature = OpposingCreatureController.creature;
                bool isSameHuntType = Opposingcreature.huntType == creature.huntType;
                bool isHunteronPrey = creature.huntType == HuntType.Predator && Opposingcreature.huntType == HuntType.Prey;
                if (isSameHuntType)
                {
                    if (hasProcreated || !OpposingCreatureController.hasProcreated)
                    {
                        hasTarget = false;
                    }
                    if (!hasProcreated && !OpposingCreatureController.hasProcreated)
                    {
                        hasProcreated = true;
                        hasTarget = false;
                        OpposingCreatureController.hasProcreated = true;
                        Procreate(OpposingCreatureController.creature, creature, OpposingCreatureController, this);
                    }
                }
                if (isHunteronPrey)
                {
                    OpposingCreatureController.Die();
                    creature.Hunger += Opposingcreature.size;
                    hasTarget = false;
                }
            }
        }
    }

    public void Procreate(Creature father, Creature mother, CreatureController fatherController, CreatureController motherController)
    {
        GameObject child = Instantiate(this.gameObject, gameObject.transform.position + new Vector3(1, 1, 1), gameObject.transform.rotation);
        CreatureController childController = child.GetComponent<CreatureController>();
        childController.InitiateCreature(childController.settings);
        Creature childCreature = child.GetComponent<CreatureController>().creature;
        childController.hasProcreated = false;
        childController.hasTarget = false;
        childCreature.size = father.size + mother.size;
    }

    public void MoveTowards(Vector3 target)
    {
        rb.MovePosition(Vector3.MoveTowards(transform.position, target, creature.speed * Time.deltaTime));
    }

    public void MoveAway(Vector3 target)
    {
        Vector3 directionAway = (target - transform.position).normalized;
        Vector3 newPosition = transform.position - directionAway * creature.speed * Time.deltaTime;
        rb.MovePosition(newPosition);
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
}

[Serializable]
public class Creature
{
    public float size;
    public float speed;
    public float VisionRadius;
    public float WalkRange;
    public Color color;
    public Diet diet;
    public HuntType huntType;
    public float Hunger;
    public float maxHunger;
    public Creature(CreatureSettings settings)
    {
        this.size = settings.Size;
        this.speed = settings.Speed;
        this.VisionRadius = settings.VisionRadius;
        this.color = settings.color;
        this.diet = settings.diet;
        this.huntType = settings.huntType;
        this.WalkRange = settings.WalkRange;
        this.maxHunger = settings.maxHunger;
        Hunger = maxHunger;
    }
}
