using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

public static class CommonExtension {

    public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> onItem) {
        var list = source.ToArray();

        foreach (var item in list) {
            onItem?.Invoke(item);
        }

        return list;
    }
    
    public static IEnumerable<T> For<T>(this IEnumerable<T> source, Action<int, T> onItem) {
        var list = source.ToArray();

        for (var i = 0; i < list.Length; i++) {
            onItem?.Invoke(i, list[i]);
        }

        return list;
    }

    public static T GetRandom<T>(this IEnumerable<T> source) {
        var list = source.ToArray();

        return list[Random.Range(0, list.Length)];
    }

    public static Vector3 ReflectDirection(this Vector3 normal, Vector3 direction) {
        direction.Normalize();
        normal = normal.normalized;
 
        return (2 * ((Vector3.Dot(direction, normal)) * normal) - direction).normalized * -1;
    }
    
    public static Type[] GetAssembliesTypes(this AppDomain domain) {
        return domain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .ToArray();
    }

    public static bool IsNullOrEmpty(this string source) => string.IsNullOrEmpty(source);
    
    public static string Serialize(this object source) => JsonConvert.SerializeObject(source);
    public static T Deserialize<T>(this string source) => JsonConvert.DeserializeObject<T>(source);
    public static T Deserialize<T>(this object source) => source.ToString().Deserialize<T>();

}

public static class Delay {

    public static Sequence Execute(float time, Action complete) => 
        DOTween
        .Sequence()
        .AppendInterval(time)
        .OnComplete(() => complete?.Invoke())
        .Play();

}
