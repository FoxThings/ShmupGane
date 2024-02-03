using System;
using UnityEngine.UI;

public abstract class Device
{
    public delegate void updateConsumption();

    public ModuleKind kind { get; protected set; }

    public Image presentation;
    
    protected updateConsumption updateHandler;

    public abstract float CalculateConsumption(float input);

    public void setConsumptionUpdater(updateConsumption handler)
    {
        this.updateHandler = handler;
    }
}
