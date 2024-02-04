using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.NewPlayerSystem.Devices.Shields;

public class ShipModel
{
    private readonly Dictionary<Module, List<Module>> model;
    private readonly Module core;

    private readonly List<Module> modulesDescriptor;

    public ShipModel()
    {
        model = new Dictionary<Module, List<Module>>();

        // Layer 0
        core = new Module(ModuleKind.Core, this) {
            output = 2f,
        };

        // Layer 1
        Module s1 = new Module(ModuleKind.Shield, this);
        Module s2 = new Module(ModuleKind.Shield, this);
        Module d1 = new Module(ModuleKind.Device, this);
        Module d2 = new Module(ModuleKind.Device, this);
        Module d3 = new Module(ModuleKind.Device, this);

        model[core] = new List<Module>() { s1, d1, d2, d3, s2 };

        // Layer 2
        Module w1 = new Module(ModuleKind.Weapon, this);
        Module w2 = new Module(ModuleKind.Weapon, this);
        Module w3 = new Module(ModuleKind.Weapon, this);

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
                device.SetModule(modulesDescriptor[v]);
                ModelRefresh();
                return true;
            }
        }

        return false;
    }

    public void ModelRefresh()
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
    public List<Module> GetNeighbourModules(Module module)
    {
        return model[module];
    }

    public List<Shield> GetShields()
    {
        return modulesDescriptor
            .Where(module => module.kind == ModuleKind.Shield)
            .Select(module => module.attachedDevice)
            .OfType<Shield>()
            .ToList();
    }

    public List<Module> GetAllModules()
    {
        return modulesDescriptor;
    }
}

public class Module
{
    private readonly ShipModel model;
    public ModuleKind kind { get; private set; }

    public Device attachedDevice;

    // energy production|consumption in particle/sec
    public float input { get; set; } = 0;
    public float output { get; set; } = 0;

    public Module(ModuleKind kind, ShipModel model, float basicOutput = 0)
    {
        this.model = model;
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

        output = input - attachedDevice.CalculateConsumption(input);
    }

    public void UpdateModel()
    {
        model.ModelRefresh();
    }

    public List<Device> GetNeighbourDevices()
    {
        return model.GetNeighbourModules(this)
            .Where((module) => module.attachedDevice != null)
            .Select((module) => module.attachedDevice)
            .ToList();
    }

    public bool IsAttached()
    {
        return attachedDevice != null;
    }
}
