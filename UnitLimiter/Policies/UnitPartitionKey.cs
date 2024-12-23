using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics.CodeAnalysis;

namespace UnitLimiter.Policies
{
    /// <summary>
    /// 基于Endpoint的限流
    /// </summary>
    readonly struct UnitPartitionKey : IEquatable<UnitPartitionKey>
    {
        /// <summary>
        /// 获取None实例
        /// </summary>
        public static readonly UnitPartitionKey None = new(new(null, new EndpointMetadataCollection(), "None"), null);

        /// <summary>
        /// 获取终结点
        /// </summary>
        public Endpoint Endpoint { get; }

        /// <summary>
        /// 限制的单元
        /// </summary>
        public string? Unit { get; }

        /// <summary>
        /// 基于Endpoint的限流
        /// </summary>
        /// <param name="endpoint">终结点</param>
        /// <param name="unit">用户id</param>
        public UnitPartitionKey(Endpoint endpoint, string? unit)
        {
            Endpoint = endpoint;
            Unit = unit;
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(UnitPartitionKey other)
        {
            return Endpoint == other.Endpoint && Unit == other.Unit;
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is UnitPartitionKey other && Equals(other);
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Endpoint);
            hashCode.Add(Unit);
            return hashCode.ToHashCode();
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(UnitPartitionKey left, UnitPartitionKey right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 是否不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(UnitPartitionKey left, UnitPartitionKey right)
        {
            return !(left == right);
        }
    }
}
