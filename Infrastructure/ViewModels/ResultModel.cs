using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Infrastructure.ViewModels
{
    /// <summary>
    /// 接口统一返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        public T ResultInfo { get; set; }

        public bool IsSuccess { get; set; }

        public string Status { get; set; }
    }

    /// <summary>
    /// 接口统一返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T, T2>
    {
        public T ResultInfo { get; set; }

        public T2 ResultInfo2 { get; set; }

        public bool IsSuccess { get; set; }

        public string Status { get; set; }
    }

    /// <summary>
    /// 接口统一返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T, T2, T3>
    {
        public T ResultInfo { get; set; }
        public T2 ResultInfo2 { get; set; }

        public T3 ResultInfo3 { get; set; }

        public bool IsSuccess { get; set; }

        public string Status { get; set; }
    }

    /// 接口统一返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T, T2, T3, T4>
    {
        public T ResultInfo { get; set; }
        public T2 ResultInfo2 { get; set; }

        public T3 ResultInfo3 { get; set; }

        public T4 ResultInfo4 { get; set; }

        public bool IsSuccess { get; set; }

        public string Status { get; set; }
    }
    /// 接口统一返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T, T2, T3, T4, T5>
    {
        public T ResultInfo { get; set; }
        public T2 ResultInfo2 { get; set; }

        public T3 ResultInfo3 { get; set; }

        public T4 ResultInfo4 { get; set; }

        public T4 ResultInfo5 { get; set; }

        public bool IsSuccess { get; set; }

        public string Status { get; set; }
    }
}