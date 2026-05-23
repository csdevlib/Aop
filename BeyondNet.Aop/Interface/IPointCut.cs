using System;

namespace BeyondNet.Aop
{
    public interface IPointCut
    {
        bool CanApply(IJoinPoint joinPoint, Type type);
    }
}
