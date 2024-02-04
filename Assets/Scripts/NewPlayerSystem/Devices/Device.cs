using System;
using UnityEngine.UI;

public abstract class Device
{
    public ModuleKind kind { get; protected set; }

    public Image presentation;
    public String description;
    
    protected Module module;

    public abstract float CalculateConsumption(float input);

    public void SetModule(Module module)
    {
        this.module = module;
    }
}
