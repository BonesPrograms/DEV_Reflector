using XRL.Wish;
using XRL.World;
using System;
using XRL;
using System.Linq;
using System.Reflection;
using XRL.UI;

namespace BeastReflector
{
    [HasWishCommand]

    public class FieldCommand : BaseReflective
    {
        [WishCommand("field")]
        public static void FieldWish(string input)
        {
            string[] inputs = input.Split(":");
            if (PickTarget(The.Player, "field", out var pick))
            {
                if (inputs.Length == 2)
                {
                    GetFieldCommand.GetField(inputs, pick);
                }
                else if (inputs.Length == 3)
                {
                    SetFieldCommand.SetField(inputs, pick);
                }
                else
                    IComponent<GameObject>.AddPlayerMessage("Invalid parameters");
            }

        }

    }
}