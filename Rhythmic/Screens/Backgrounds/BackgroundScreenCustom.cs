using Rhythmic.Graphics;

namespace Rhythmic.Screens.Backgrounds
{
    public class BackgroundScreenCustom : BackgroundScreen
    {
        private readonly string textureName;

        public BackgroundScreenCustom(string textureName)
        {
            this.textureName = textureName;
            AddInternal(new Background(textureName));
        }

        public override bool Equals(BackgroundScreen other)
        {
            BackgroundScreenCustom backgroundScreenCustom = other as BackgroundScreenCustom;
            if (backgroundScreenCustom == null) return false;

            return base.Equals(other) && textureName == backgroundScreenCustom.textureName;
        }
    }
}
