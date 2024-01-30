using UnityEngine.UI;

public abstract class Device
{
    public ModuleKind kind { get; protected set; }

    public Image presentation;

    public abstract float CalculateConsumption(float input);
}
