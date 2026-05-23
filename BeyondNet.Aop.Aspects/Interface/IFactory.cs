using System;

namespace BeyondNet.Aop.Aspects
{
    public interface IFactory<T>
    {
        T Create(IJoinPoint joinPoint, Type type);
    }
}
