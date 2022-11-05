using UnityEngine;

//Crée l'attribut [ReadOnly] pour révéler les variables dans l'inspecteur sans les rendre modifiable
[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)] 
public class ReadOnlyAttribute : PropertyAttribute
{

}
