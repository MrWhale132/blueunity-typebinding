
using System;

namespace Theblueway.TypeBinding.Editor.TypeInterfaceScripts
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class MemberAttribute : Attribute
    {
        public int MemberIndex { get; internal set; }
        public MemberType MemberType { get; internal set; }
        public MemberAttribute(int memberIndex)
        {
            MemberIndex = memberIndex;
        }
    }


    public enum MemberType
    {
        NotSet = 0,
        Field = 1,
        Property = 2,
        Event = 3,
        Method = 4,
    }
}
