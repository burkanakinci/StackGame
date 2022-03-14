using UnityEngine;

[CreateAssetMenu(fileName = "CollactableData", menuName = "Collactable Object Data")]
public class CollactableScriptableObject : ScriptableObject
{
    [SerializeField] private float heightMultiplier = 0.0f, flightDuration = 0.0f;

    public float HeightMultiplier
    {
        get { return heightMultiplier; }
    }
    public float FlightDuration
    {
        get { return flightDuration; }
    }

}
