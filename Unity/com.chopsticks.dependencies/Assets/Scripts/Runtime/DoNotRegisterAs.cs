using System;

namespace Chopsticks.Dependencies
{
    [AttributeUsage(AttributeTargets.Class, 
        AllowMultiple = true, 
        Inherited = true)]
    public class DoNotRegisterAs : Attribute
    {
        public DoNotRegisterAs(Type contract) { }
    }
}
