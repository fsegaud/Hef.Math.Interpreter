namespace Hef.Math
{
    public interface IInterpreterContext
    {
        bool TryGetVariable(string name, out double value);
    }
}
