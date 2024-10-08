using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using static UnityEngine.GraphicsBuffer;

public class CreatureController : MonoBehaviour
{
    public bool isPreSpawned;
    public Creature Creature;
    public CreatureSettings settings;
    public SphereCollider VisionCircle;
    public StatDisplay statDisplay;
    public bool hasTarget;
    private bool hasRandomTarget;
    private Vector3 randomDestination;
    public LayerMask groundLayer;
    Rigidbody rb;
    public bool hasProcreated;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (isPreSpawned)
            Creature = InitiateCreature(settings.Size, settings.Speed, settings.VisionRadius, settings.WalkRange, settings.gender, settings.huntType);
        GetComponent<MeshRenderer>().material = settings.material;
        Creature.material = settings.material;
        hasProcreated = false;
        statDisplay.DisableCanvas();
        statDisplay.creature = Creature;
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

    public Creature InitiateCreature(float size, float speed, float VisionRadius, float WalkRange, CreatureSettings.Gender gender, CreatureSettings.HuntType huntType)
    {
        return new Creature(size, speed, VisionRadius, WalkRange, gender, huntType);
    }


    private void Update()
    {
        transform.localScale = new Vector3(Creature.size, Creature.size, Creature.size);
        VisionCircle.radius = Creature.VisionRadius;
        if (!hasTarget)
        {
            if (!hasRandomTarget) randomDestination = PickRandomDirection();
            if (hasRandomTarget) MoveTowards(randomDestination);
            if (hasRandomTarget && Vector3.Distance(transform.position, randomDestination) < 1) hasRandomTarget = false;
        }
    }

    Vector3 PickRandomDirection()
    {
        float x = UnityEngine.Random.Range(-Creature.WalkRange, Creature.WalkRange);
        float z = UnityEngine.Random.Range(-Creature.WalkRange, Creature.WalkRange);


        Vector3 newDest = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        if (Physics.Raycast(newDest, Vector3.down, groundLayer))
        {
            hasRandomTarget = true;
        }
        return newDest;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Creature")
        {
            var OpposingCreatureController = other.gameObject.GetComponent<CreatureController>();
            if (OpposingCreatureController != null)
            {
                Creature Opposingcreature = OpposingCreatureController.Creature;
                bool isOppositeGender = Opposingcreature.gender != Creature.gender;
                bool isSameHuntType = Opposingcreature.huntType == Creature.huntType;
                bool isHunteronPrey = Creature.huntType == CreatureSettings.HuntType.Predator && Opposingcreature.huntType == CreatureSettings.HuntType.Prey;
                if (isOppositeGender && isSameHuntType)
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
                        if (Creature.gender == CreatureSettings.Gender.Female)
                        {
                            Procreate(settings, OpposingCreatureController.settings);
                        }
                        else
                        {
                            Procreate(OpposingCreatureController.settings, settings);
                        }
                    }
                }
                if (isHunteronPrey)
                {
                    Destroy(other.gameObject);
                    hasTarget = false;
                }
            }
        }
    }

    public void Procreate(CreatureSettings fatherInfo, CreatureSettings motherInfo)
    {
        GameObject child = Instantiate(this.gameObject, gameObject.transform.position + new Vector3(1, 1, 1), gameObject.transform.rotation);
        CreatureController childController = child.GetComponent<CreatureController>();
        childController.Creature.gender = (CreatureSettings.Gender)UnityEngine.Random.Range(0, 2);
        childController.settings = (Creature.gender > CreatureSettings.Gender.Male) ? fatherInfo : motherInfo;
        childController.InitiateCreature(childController.settings.Size, childController.settings.Speed, childController.settings.VisionRadius, childController.settings.WalkRange, childController.settings.gender, childController.settings.huntType);
        Creature childCreature = child.GetComponent<CreatureController>().Creature;
        childController.hasProcreated = false;
        childController.hasTarget = false;
        childCreature.size = fatherInfo.Size / motherInfo.Size;
    }

    public void MoveTowards(Vector3 target)
    {
        rb.MovePosition(Vector3.MoveTowards(transform.position, target, Creature.speed * Time.deltaTime));
    }

    public void MoveAway(Vector3 target)
    {
        Vector3 directionAway = (target - transform.position).normalized;
        Vector3 newPosition = transform.position - directionAway * Creature.speed * Time.deltaTime;
        rb.MovePosition(newPosition);
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
    public CreatureSettings.Gender gender;
    public CreatureSettings.HuntType huntType;
    public Creature(float size, float speed, float VisionRadius, float WalkRange, CreatureSettings.Gender gender, CreatureSettings.HuntType huntType)
    {
        this.size = size;
        this.speed = speed;
        this.VisionRadius = VisionRadius;
        this.gender = gender;
        this.huntType = huntType;
        this.WalkRange = WalkRange;
    }
}
