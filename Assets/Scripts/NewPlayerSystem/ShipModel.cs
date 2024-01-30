
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipModel
{
    private Dictionary<Module, List<Module>> model;
    private Module core;

    private List<Module> modulesDescriptor;

    public ShipModel()
    {
        model = new Dictionary<Module, List<Module>>();

        // Layer 0
        core = new Module(ModuleKind.Core);
        core.output = 1;

        // Layer 1
        Module s1 = new Module(ModuleKind.Shield);
        Module s2 = new Module(ModuleKind.Shield);
        Module d1 = new Module(ModuleKind.Device);
        Module d2 = new Module(ModuleKind.Device);
        Module d3 = new Module(ModuleKind.Device);

        model[core] = new List<Module>() { s1, d1, d2, d3, s2 };

        // Layer 2
        Module w1 = new Module(ModuleKind.Weapon);
        Module w2 = new Module(ModuleKind.Weapon);
        Module w3 = new Module(ModuleKind.Weapon);

        model[d1] = new List<Module>() { w1 };
        model[d2] = new List<Module>() { w2 };
        model[d3] = new List<Module>() { w3 };

        modulesDescriptor = new List<Module>() { core, s1, d1, w1, d2, w2, d3, w3, s2 };

        ModelRefresh();
    }

    internal bool AddDevice(int v, Device device)
    {
        if (v >= 0 && v < modulesDescriptor.Count)
        {
            if (modulesDescriptor[v].TryAttachDevice(device))
            {
                ModelRefresh();
                return true;
            }
        }

        return false;
    }

    private void ModelRefresh()
    {
        Queue<Module> queue = new Queue<Module>();
        queue.Enqueue(core);

        while (queue.Count > 0)
        {
            Module module = queue.Dequeue();
            if (!model.ContainsKey(module))
            {
                continue;
            }

            var modules = model[module];
            foreach (var m in modules)
            {
                m.input = module.output;
                m.RecalculateOutputEnergy();
                queue.Enqueue(m);
            }
        }
    }
}

public class Module
{
    public ModuleKind kind { get; private set; }

    public Device attachedDevice;

    // energy production|consumption in particle/sec
    public float input { get; set; } = 0;
    public float output { get; set; } = 0;

    public Module(ModuleKind kind, float basicOutput = 0)
    {
        this.kind = kind;
        this.output = basicOutput;
    }

    public bool TryAttachDevice(Device device)
    {
        if (attachedDevice == null && device.kind == this.kind)
        {
            this.attachedDevice = device;
            return true;
        }

        return false;
    }

    public void RecalculateOutputEnergy()
    {
        if (attachedDevice == null)
        {
            output = input;
            return;
        }

        output = attachedDevice.CalculateConsumption(input);
    }
}
