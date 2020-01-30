using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]

public struct PaddleInput : IComponentData
{
    public KeyCode upKey;
    public KeyCode downKey;
}

    
