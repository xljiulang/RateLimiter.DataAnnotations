using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RateLimiting.DataAnnotations
{
    /// <summary>
    /// 表示一个单元分区键
    /// </summary>
    public readonly struct UnitPartitionKey : IEquatable<UnitPartitionKey>
    {
        /// <summary>
        /// 获取None实例
        /// </summary>
        public static readonly UnitPartitionKey None = new(new(null, new EndpointMetadataCollection(), "None"), string.Empty);

        /// <summary>
        /// 获取终结点
        /// </summary>
        public Endpoint Endpoint { get; }

        /// <summary>
        /// 获取单元
        /// </summary>
        public string Unit { get; }

        /// <summary>
        /// 初始化 <see cref="UnitPartitionKey"/> 结构的新实例
        /// </summary>
        /// <param name="endpoint">终结点</param>
        /// <param name="unit">单元</param>
        public UnitPartitionKey(Endpoint endpoint, string unit)
        {
            Endpoint = endpoint;
            Unit = unit;
        }

        /// <inheritdoc></inheritdoc>/>
        public readonly bool Equals(UnitPartitionKey other)
        {
            return Endpoint == other.Endpoint && Unit == other.Unit;
        }

        /// <inheritdoc></inheritdoc>/>
        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is UnitPartitionKey other && Equals(other);
        }

        /// <inheritdoc></inheritdoc>/>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Endpoint, Unit);
        }

        /// <inheritdoc></inheritdoc>/>
        public static bool operator ==(UnitPartitionKey left, UnitPartitionKey right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc></inheritdoc>/>
        public static bool operator !=(UnitPartitionKey left, UnitPartitionKey right)
        {
            return !(left == right);
        }
    }
}
