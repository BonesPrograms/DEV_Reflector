using XRL.Wish;
using XRL.World;
using System;
using XRL;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BeastReflector
{
    [HasWishCommand]

    internal class GetFieldCommand : FieldReflective
    {

        [WishCommand("readfield")]
        static void ReadField(string text) => GetField(text);

        [WishCommand("getfield")]

        static void GetField(string text)
        {
            string[] inputs = text.Split(":");
            if (inputs.Length < 2)
                IComponent<GameObject>.AddPlayerMessage("Incomplete parameters. params: type:fieldname or gameobject:fieldname");
            else if (PickTarget(The.Player, "getfield", out var pick))
            {
                string typeName = inputs[0];
                string fieldName = inputs[1];
                if (typeName.Equals("gameobject", StringComparison.OrdinalIgnoreCase))
                {
                    FieldInfo field = FindGameObjectField(fieldName, pick);
                    if (field != null)
                        DisplayField(field, pick, pick);
                }
                else
                    GetFieldWish(typeName, fieldName, pick);
            }
        }



        static void GetFieldWish(string typeName, string fieldName, GameObject pick)
        {
            object instance = FindObject(pick.PartsList, typeName);
            instance ??= FindObject(pick.Effects, typeName);
            if (instance == null)
            {
                IComponent<GameObject>.AddPlayerMessage($"{pick.DisplayName} ID: {pick.ID} does not have an IPart or Effect named {typeName}");
                return;
            }
            if (LoopForField(fieldName, instance, GetLimit(instance.GetType()), out var field))
            {
                DisplayField(field, instance, pick);
            }
            else
                IComponent<GameObject>.AddPlayerMessage($"{instance.GetType().Name} does not have a field named {fieldName}");
        }

        static void DisplayField(FieldInfo field, object instance, GameObject pick)
        {
            object value = field.GetValue(instance);
            string valueMsg = value == null ? "null" : value.ToString();
            StringBuilder text = new();
            text.Append($"{field.FieldType.Name} \"{field.Name}\"\n");
            text.Append($"in type {instance.GetType()}\n");
            text.Append($"has value {valueMsg}");
            IComponent<GameObject>.AddPlayerMessage(text.ToString());
        }
    }
}