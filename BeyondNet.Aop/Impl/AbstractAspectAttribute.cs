using System;

namespace BeyondNet.Aop
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AbstractAspectAttribute : Attribute
    {
        public int Order { get; set; }
    }
}