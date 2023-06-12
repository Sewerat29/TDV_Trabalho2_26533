using monogame.Sprites;

namespace monogame.Sprites
{
    public interface ICollidable
    {
        void OnCollide(Sprite sprite);
    }
}