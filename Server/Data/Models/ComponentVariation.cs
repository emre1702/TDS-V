namespace TDS_Server.Data.Models
{
    public struct ComponentVariation
    {
        #region Public Fields

        public int Drawable;
        public int Palette;
        public int Texture;

        #endregion Public Fields

        #region Public Constructors

        public ComponentVariation(int drawable, int texture)
            => (Drawable, Texture, Palette) = (drawable, texture, 0);

        public ComponentVariation(int drawable, int texture, int palette)
            => (Drawable, Texture, Palette) = (drawable, texture, palette);

        #endregion Public Constructors

        #region Public Methods

        public static bool operator !=(ComponentVariation left, ComponentVariation right)
        {
            return !(left == right);
        }

        public static bool operator ==(ComponentVariation left, ComponentVariation right)
        {
            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ComponentVariation component))
                return false;
            return Drawable == component.Drawable
                && Texture == component.Texture
                && Palette == component.Palette;
        }

        public override int GetHashCode()
            => base.GetHashCode();

        #endregion Public Methods
    }
}
