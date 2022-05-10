using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public enum Ability
    {
        PushPull,
        GravityField,
        Friction
    }
    //TODO update
    class PickupAbility : Pickup
    {
        public Ability ability = Ability.PushPull;

        public override void OnPickup()
        {
            GameObject player = PlayerManager.instance;
            if(ability == Ability.PushPull)
                player.gameObject.GetComponent<Push_Pull>().enabled = true;
            if (ability == Ability.GravityField)
                player.gameObject.GetComponent<GravityField>().enabled = true;
            if (ability == Ability.Friction)
                player.gameObject.GetComponent<Friction>().enabled = true;
        }
    }
}
