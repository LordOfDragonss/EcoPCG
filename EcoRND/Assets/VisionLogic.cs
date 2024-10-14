using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class VisionLogic : MonoBehaviour
{
    [SerializeField] CreatureController personalController;
    [SerializeField] Creature currentCreature;

    private void Start()
    {
        personalController = GetComponentInParent<CreatureController>();
        currentCreature = personalController.creature;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Plant" && personalController.creature.diet == Diet.Herbivore)
        {
            personalController.MoveTowards(other.transform.position);
            personalController.hasTarget = true;
        }
        if (other.gameObject.tag == "Creature")
        {
            var OpposingCreatureController = other.gameObject.GetComponent<CreatureController>();
            if (OpposingCreatureController != null)
            {
                Creature Opposingcreature = OpposingCreatureController.creature;
                bool isSameHuntType = Opposingcreature.huntType == currentCreature.huntType;
                bool isHunteronPrey = currentCreature.huntType == HuntType.Predator && Opposingcreature.huntType ==  HuntType.Prey;
                bool isPreyOnHunter = currentCreature.huntType == HuntType.Prey && Opposingcreature.huntType == HuntType.Predator;
                if (isSameHuntType && !personalController.hasProcreated && !OpposingCreatureController.hasProcreated)
                {
                    personalController.MoveTowards(OpposingCreatureController.transform.position);
                    personalController.hasTarget = true;
                }
                if (isHunteronPrey)
                {
                    personalController.MoveTowards(OpposingCreatureController.transform.position);
                    personalController.hasTarget = true;
                }
                if (isPreyOnHunter)
                {
                    personalController.MoveAway(OpposingCreatureController.transform.position);
                    personalController.hasTarget = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Creature")
        {
            var OpposingCreatureController = other.gameObject.GetComponent<CreatureController>();
            if (OpposingCreatureController != null)
            {
                Creature Opposingcreature = OpposingCreatureController.creature;

                if (personalController.hasTarget)
                {
                    personalController.hasTarget = false;
                }
            }
        }
    }
}
