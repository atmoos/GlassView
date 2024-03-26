namespace GlassView.Core;

public interface ICreate<out TSelf, in TParameter>
    where TSelf : ICreate<TSelf, TParameter>
{
    public abstract static TSelf Create(TParameter argument);
}
