using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[VFXBinder("Transform/Position Binder")]
public class PositionBinder : VFXBinderBase
{
    [VFXPropertyBinding("UnityEngine.Vector3")]
    public ExposedProperty pos1Property;
    public ExposedProperty pos2Property;
    public Vector3 pos1Position;
    public Vector3 targetPosition; // Cette position représente la position du joueur

    public PositionBinder(Vector3 pos1, Vector3 target)
    {
        this.pos1Position = pos1;
        this.targetPosition = target;
    }

    public override bool IsValid(VisualEffect component)
    {
        return pos1Position != null && targetPosition != null 
            && component.HasVector3(pos1Property) 
            && component.HasVector3(pos2Property);
    }

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetVector3(pos1Property, pos1Position);
        component.SetVector3(pos2Property, targetPosition);
    }
}