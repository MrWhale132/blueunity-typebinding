#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using Theblueway.Core.Runtime;
using Theblueway.Core.Runtime.Extensions;
using UnityEngine;

namespace Theblueway.TypeBinding.Runtime.TypeInterfaceScripts
{
    public abstract class TypeInterface<T> : TypeInterface { public static T? instance; }

    public abstract class TypeInterface
    {

        public const bool Static = true;

        public static bool _attributeLookupIsBuilt => _interfaceInfoById != null;
        public static Dictionary<long, TypeInterfaceInfo>? _interfaceInfoById;
        public static Dictionary<long, TypeInterface> _interfaceInstanceByIdLookup = new();

        //=============================

        public Dictionary<int, TypeMember> _typeMembersByIndex = new();


        public abstract IEnumerable<TypeMember> GetMembers();

        public IEnumerable<TypeMember> GetTypeMembers()
        {
            return _typeMembersByIndex.Values;
        }

        public TypeMember? GetTypeMemberByIndex(int index)
        {
            if (_typeMembersByIndex.TryGetValue(index, out TypeMember typeMember))
            {
                return typeMember;
            }
            else
            {
                Debug.LogError($"No TypeMember found for index {index} in TypeInterface {this.GetType().CleanAssemblyQualifiedName()}");
                return null;
            }
        }

        //=====================

        public static bool Exists(long interfaceId)
        {
            EnsureLookUp();
            return _interfaceInfoById!.ContainsKey(interfaceId);
        }


        public static Type? GetHandledType(long interfaceId)
        {
            EnsureLookUp();

            if (!Exists(interfaceId)) return null;

            var info = GetTypeInterfaceInfo(interfaceId)!;
            return info.HandledType;
        }


        public static IEnumerable<TypeInterfaceInfo> GetTypeInterfaceInfos()
        {
            EnsureLookUp();
            return _interfaceInfoById!.Values;
        }

        public static TypeInterfaceInfo? GetTypeInterfaceInfo(long interfaceId)
        {
            EnsureLookUp();
            if (_interfaceInfoById!.TryGetValue(interfaceId, out TypeInterfaceInfo info))
            {
                return info;
            }
            else
            {
                if (interfaceId != 0)
                    Debug.LogError($"No TypeInterfaceInfo found for Id {interfaceId}");
                return null;
            }
        }

        public static IEnumerable<TypeMember>? GetMembersOf(long interfaceId)
        {
            var instance = GetInstance(interfaceId);
            if (instance == null) return null;
            return instance.GetTypeMembers();
        }


        public static TypeMember? GetTypeMemberByIndex(long interfaceId, int index)
        {
            var instance = GetInstance(interfaceId);

            if (instance == null) return null;

            if (instance._typeMembersByIndex.TryGetValue(index, out TypeMember typeMember))
            {
                return typeMember;
            }
            else
            {
                Debug.LogError($"No TypeMember found for index {index} in TypeInterface {instance.GetType().CleanAssemblyQualifiedName()}");
                return null;
            }
        }




        public static TypeInterface? GetInstance(long interfaceId)
        {
            EnsureLookUp();

            if (!_interfaceInstanceByIdLookup.ContainsKey(interfaceId))
            {
                var instance = CreateInstance(interfaceId);

                if (instance == null) return null;

                Setup(instance);
            }

            return _interfaceInstanceByIdLookup[interfaceId];
        }

        public static void Setup(TypeInterface instance)
        {
            var members = instance.GetMembers();

            foreach (var member in members)
            {
                if (instance._typeMembersByIndex.ContainsKey(member.Index))
                {
                    Debug.LogError($"Duplicate TypeMember index {member.Index} found in TypeInterface {instance._typeMembersByIndex}, {instance.GetType().CleanAssemblyQualifiedName()}. ");
                    continue;
                }

                instance._typeMembersByIndex.Add(member.Index, member);
            }
        }

        public static TypeInterface? CreateInstance(long interfaceId)
        {
            if(!_interfaceInfoById!.ContainsKey(interfaceId)) return null;


            Type interfaceType = _interfaceInfoById![interfaceId].TypeInterfaceType;

            var instance = ObjectFactory.CreateInstance<TypeInterface>(interfaceType);

            _interfaceInstanceByIdLookup.Add(interfaceId, instance);

            return _interfaceInstanceByIdLookup[interfaceId];
        }



        public static void EnsureLookUp()
        {
            if (_attributeLookupIsBuilt) return;
            BuildLookup();
        }

        public static void BuildLookup()
        {
            if (_attributeLookupIsBuilt) return;
            _interfaceInfoById = new();

            var types = AppDomain.CurrentDomain.GetUserTypes();

            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<TypeInterfaceAttribute>();

                if (attr == null) continue;

                if (!type.IsAssignableTo(typeof(TypeInterface)))
                {
                    Debug.LogError($"Type {type.CleanAssemblyQualifiedName()} has {nameof(TypeInterfaceAttribute)} but does not implement {nameof(TypeInterface)}");
                    continue;
                }

                if (_interfaceInfoById.ContainsKey(attr.Id))
                {
                    Debug.LogError($"Duplicate {nameof(TypeInterfaceAttribute)} found for Id {attr.Id} on type {type.CleanAssemblyQualifiedName()}. ");
                    continue;
                }

                var info = new TypeInterfaceInfo
                {
                    Id = attr.Id,
                    HandledType = attr.HandledType,
                    TypeInterfaceType = type
                };

                _interfaceInfoById.Add(attr.Id, info);
            }
        }
    }



    public class TypeInterfaceInfo
    {
        public long Id { get; internal set; }
        public Type HandledType { get; internal set; } = null!;
        public Type TypeInterfaceType { get; internal set; } = null!;
    }



    public class TypeArg0 { }
    public class TypeArg1 { }
    public class TypeArg2 { }
    public class TypeArg3 { }
    public class TypeArg4 { }
    public class TypeArg5 { }
    public class TypeArg6 { }
    public class TypeArg7 { }
    public class TypeArg8 { }
    public class TypeArg9 { }
}
