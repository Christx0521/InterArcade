using UnityEngine;

public class PAC_PowerPellet : PAC_Pellet
{
    public float duration = 8f;

    protected override void Eat()
    {
        PAC_GameManager.Instance.PowerPelletEaten(this);
    }
}
