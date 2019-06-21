using System.ComponentModel;

namespace Rhythmic.Screens.Select
{
    public enum RandomSelectAlgorithm
    {
        [Description("Never repeat")]
        RandomPermutation,

        [Description("Random")]
        Random
    }
}
