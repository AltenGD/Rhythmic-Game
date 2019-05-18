using Rhythmic.Screens.Backgrounds;

namespace Rhythmic.Screens.Edit
{
    public class Editor : ScreenWhiteBox
    {
        public Editor(BackgroundScreenDefault background)
        {
            background?.Next();
        }
    }
}
