using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public class EXPCurveLevelProvider : ICharacterLevelProvider
    {
        public EXPCurveLevelProvider()
        {
            ExperiencePoints = new(0);
            AvailableCharacterLevels = ExperiencePoints.GetDeriving((exp) => EXPThresholds.Where(threshold => threshold <= exp).Count());
        }
        public ModifyableValue<int> ExperiencePoints { get; private set; }
        public ModifyableValue<int> AvailableCharacterLevels { get; private set; }

        private static readonly int[] EXPThresholds = [
                  0,
            300,
            900,
          2_700,
          6_500,
         14_000,
         23_000,
         34_000,
         48_000,
         64_000,
         85_000,
        100_000,
        120_000,
        140_000,
        165_000,
        195_000,
        225_000,
        265_000,
        305_000,
        355_000
        ];
    }
}