public class Trick
{
    public struct SkateboardTrick
    {
        public TrickType WhichTrick;
        public int Points;
        public string AnimationTrigger;
        public bool Unlocked;
    }
    public enum TrickType
    {
        Undefined,
        Frontflip,
        Backflip,
        Sideflip,
        Treflip
    }

    public SkateboardTrick Frontflip = new SkateboardTrick
    {
        WhichTrick = TrickType.Frontflip,
        Points = 10,
        AnimationTrigger = "frontflipTrigger",
        Unlocked = false
    };

    public SkateboardTrick Backflip = new SkateboardTrick
    {
        WhichTrick = TrickType.Backflip,
        Points = 10,
        AnimationTrigger = "backflipTrigger",
        Unlocked = false
    };

    public SkateboardTrick Sideflip = new SkateboardTrick
    {
        WhichTrick = TrickType.Sideflip,
        Points = 10,
        AnimationTrigger = "sideflipTrigger",
        Unlocked = false
    };

    public SkateboardTrick Treflip = new SkateboardTrick
    {
        WhichTrick = TrickType.Treflip,
        Points = 10,
        AnimationTrigger = "treflipTrigger",
        Unlocked = false
    };

}




