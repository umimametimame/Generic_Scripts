using UnityEngine;

public class FlagConditionalDisableInInspectorAttribute : PropertyAttribute
{
    public readonly string FlagVariableName;
    public readonly bool TrueThenDisable;
    public readonly bool ConditionalInvisible;

    public FlagConditionalDisableInInspectorAttribute(string flagVariableName,
                                                      bool trueThenDisable = false,
                                                      bool conditionalInvisible = false)
    {
        this.FlagVariableName = flagVariableName;
        this.TrueThenDisable = trueThenDisable;
        this.ConditionalInvisible = conditionalInvisible;
    }
}