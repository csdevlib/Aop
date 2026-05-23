using System;

namespace BeyondNet.Aop.Aspects
{
    public class AdviceAspectAttribute : AbstractAspectAttribute
    {
        public Type Type { get; set; }

        public bool HandleException { get; set; }

        public object[] Context { get; set; }
    }
}
