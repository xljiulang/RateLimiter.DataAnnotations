using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics.CodeAnalysis;

namespace UnitRateLimiter.Policies
{
    /// <summary>
    /// 表示一个单元分区键
    /// </summary>
    readonly struct UnitPartitionKey : IEquatable<UnitPartitionKey>
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

        /// <summary>
        /// 确定当前对象是否等于同一类型的另一个对象
        /// </summary>
        /// <param name="other">要与当前对象进行比较的对象</param>
        /// <returns>如果当前对象等于 <paramref name="other"/> 参数，则为 true；否则为 false</returns>
        public readonly bool Equals(UnitPartitionKey other)
        {
            return Endpoint == other.Endpoint && Unit == other.Unit;
        }

        /// <summary>
        /// 确定当前对象是否等于另一个对象
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象</param>
        /// <returns>如果当前对象等于 <paramref name="obj"/> 参数，则为 true；否则为 false</returns>
        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is UnitPartitionKey other && Equals(other);
        }

        /// <summary>
        /// 用作默认哈希函数
        /// </summary>
        /// <returns>当前对象的哈希代码</returns>
        public override readonly int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Endpoint);
            hashCode.Add(Unit);
            return hashCode.ToHashCode();
        }

        /// <summary>
        /// 确定两个 <see cref="UnitPartitionKey"/> 实例是否相等
        /// </summary>
        /// <param name="left">左侧实例</param>
        /// <param name="right">右侧实例</param>
        /// <returns>如果两个实例相等，则为 true；否则为 false</returns>
        public static bool operator ==(UnitPartitionKey left, UnitPartitionKey right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 确定两个 <see cref="UnitPartitionKey"/> 实例是否不相等
        /// </summary>
        /// <param name="left">左侧实例</param>
        /// <param name="right">右侧实例</param>
        /// <returns>如果两个实例不相等，则为 true；否则为 false</returns>
        public static bool operator !=(UnitPartitionKey left, UnitPartitionKey right)
        {
            return !(left == right);
        }
    }
}
