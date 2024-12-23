using System.Security.Claims;

namespace UnitRateLimiter.Routing
{
    /// <summary>
    /// 指定用于速率限制的单位来源。
    /// </summary>
    public enum UnitSource
    {
        /// <summary>
        /// 单位来源是<see cref="ClaimsPrincipal"/>类型的用户。
        /// </summary>
        User,

        /// <summary>
        /// 单位来源是路由。
        /// </summary>
        Route,

        /// <summary>
        /// 单位来源是查询字符串。
        /// </summary>
        Query,

        /// <summary>
        /// 单位来源是请求头。
        /// </summary>
        Header,

        /// <summary>
        /// 单位来源是表单数据。
        /// </summary>
        Form,

        /// <summary>
        /// 单位来源是请求体。
        /// </summary>
        Body,

        /// <summary>
        /// 单位来源是远程 IP 地址。
        /// </summary>
        RemoteIPAddress,
    }
}
