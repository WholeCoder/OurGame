namespace OurGame.Sprites.SpriteObserver
{
    public interface SpriteSubject
    {
        void RegisterObserver(Sprites.SpriteObserver.SpriteObserver sObserver);
        void RemoveObserver(Sprites.SpriteObserver.SpriteObserver aObserver);
        void NotifyObservers();
    }
}
