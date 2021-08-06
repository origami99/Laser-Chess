using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
    public string _conditionalSourceField;
    public int _enumIndex;

    public ConditionalHideAttribute(string boolVariableName)
    {
        _conditionalSourceField = boolVariableName;
    }

    public ConditionalHideAttribute(string enumVariableName, int enumIndex)
    {
        _conditionalSourceField = enumVariableName;
        _enumIndex = enumIndex;
    }
}
