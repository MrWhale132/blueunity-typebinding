
using System;

namespace Theblueway.TypeBinding.Editor.TypeInterfaceScripts
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TypeInterfaceAttribute : Attribute
    {
        public long Id { get; internal set; }
        public Type HandledType { get; internal set; }
        //==============
        public Type TypeInterfaceType { get; internal set; }

        public TypeInterfaceAttribute(long id, Type handledType)
        {
            Id = id;
            HandledType = handledType;
        }
    }
}
