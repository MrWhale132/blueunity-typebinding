
using System;

namespace Theblueway.TypeBinding.Editor.TypeInterfaceScripts
{
    public class TypeMember
    {
        public int Index { get; internal set; }
        public string Name { get; internal set; }
        public MemberType MemberType { get; internal set; }
        public Type DeclaringType { get; internal set; }
        public Type DeclaringTypeInterface { get; internal set; }
        public long TypeInterfaceId { get; internal set; }

        public static implicit operator TypeMember(string name)
        {
            return new TypeMember { Name = name };
        }

        public static implicit operator TypeMember((int index, string name) tuple)
        {
           return new TypeMember { Index = tuple.index, Name = tuple.name };
        }
    }
}
