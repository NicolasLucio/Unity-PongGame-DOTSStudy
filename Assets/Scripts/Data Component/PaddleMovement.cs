using Unity.Entities;

[GenerateAuthoringComponent]
public struct PaddleMovement : IComponentData
{
    public int direction;
    public float speed;
}
