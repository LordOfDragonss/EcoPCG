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
    public float maxHunger;
    private bool isDying;
    private float dyingTimer;
    [SerializeField] private float HungerDecay;
    public float Stamina;
    Rigidbody rb;
    public bool hasProcreated;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (isPreSpawned)
            creature = InitiateCreature(settings.Size, settings.Speed, settings.VisionRadius, settings.WalkRange, settings.diet, settings.huntType);
        GetComponent<MeshRenderer>().material = settings.material;
        creature.material = settings.material;
        hasProcreated = false;
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

    public Creature InitiateCreature(float size, float speed, float VisionRadius, float WalkRange, Diet diet, HuntType huntType)
    {
        return new Creature(size, speed, VisionRadius, WalkRange, diet, huntType,maxHunger);
    }


    private void Update()
    {
        transform.localScale = new Vector3(creature.size, creature.size, creature.size);
        VisionCircle.radius = creature.VisionRadius;
        if(creature.Hunger > 0 && creature.Hunger <= maxHunger)
        {
            creature.Hunger -= HungerDecay;
        }
        if(creature.Hunger <= 0)
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
        if (!hasTarget && creature.Hunger < maxHunger)
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
        if(other.gameObject.tag == "Plant" && creature.diet == Diet.Herbivore)
        {
            creature.Hunger +=  other.gameObject.GetComponent<Food>().HungerValue;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Creature")
        {
            var OpposingCreatureController = other.gameObject.GetComponent<CreatureController>();
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
                    Destroy(other.gameObject);
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
        childController.InitiateCreature(childController.settings.Size, childController.settings.Speed, childController.settings.VisionRadius, childController.settings.WalkRange, childController.settings.diet, childController.settings.huntType);
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
    public Material material;
    public Diet diet;
    public HuntType huntType;
    public float Hunger;
    public Creature(float size, float speed, float VisionRadius, float WalkRange, Diet diet, HuntType huntType, float maxHunger)
    {
        this.size = size;
        this.speed = speed;
        this.VisionRadius = VisionRadius;
        this.diet = diet;
        this.huntType = huntType;
        this.WalkRange = WalkRange;
        Hunger = maxHunger;
    }
}
