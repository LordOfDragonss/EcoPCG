using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionLogic : MonoBehaviour
{
    [SerializeField] CreatureController personalController;
    [SerializeField] Creature currentCreature;

    private void Start()
    {
        personalController = GetComponentInParent<CreatureController>();
        currentCreature = personalController.Creature;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Creature")
        {
            var OpposingCreatureController = other.gameObject.GetComponent<CreatureController>();
            if (OpposingCreatureController != null)
            {
                Creature Opposingcreature = OpposingCreatureController.Creature;
                bool isOppositeGender = Opposingcreature.gender != currentCreature.gender;
                bool isSameHuntType = Opposingcreature.huntType == currentCreature.huntType;
                bool isHunteronPrey = currentCreature.huntType == CreatureSettings.HuntType.Predator && Opposingcreature.huntType == CreatureSettings.HuntType.Prey;
                bool isPreyOnHunter = currentCreature.huntType == CreatureSettings.HuntType.Prey && Opposingcreature.huntType == CreatureSettings.HuntType.Predator;
                if (isOppositeGender && isSameHuntType && !personalController.hasProcreated && !OpposingCreatureController.hasProcreated)
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
                Creature Opposingcreature = OpposingCreatureController.Creature;

                if (personalController.hasTarget)
                {
                    personalController.hasTarget = false;
                }
            }
        }
    }
}
