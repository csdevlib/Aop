namespace BeyondNet.Aop
{
    public interface IAspectExecutor
    {
        void Execute(IJoinPoint joinPoint);
    }
}
