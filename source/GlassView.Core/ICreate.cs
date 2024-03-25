namespace GlassView.Core;

public interface ICreate<out TSelf, in TParameter>
    where TSelf : ICreate<TSelf, TParameter>
{
    public abstract TSelf Create(TParameter argument);
}
