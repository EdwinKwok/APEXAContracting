using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEXAContracting.Common
{
    public class ResultCodes
    {
        public const string ConcurrencyException = "52001";
    }

    public enum ResultStatus
    {
        Success = 1,
        Failure = 2,
        Warning = 3,
        Unknown = 4
    }

    public class BusinessItem
    {
        public BusinessItem() { }
        public BusinessItem(string id, string userName, byte[] timestamp)
        {
            this.Id = id;
            this.UserName = userName;
            this.TimeStamp = timestamp;
        }
        public string Id;
        public string UserName;
        public byte[] TimeStamp;
    }

    /// <summary>
    /// Return type for any business layer function that doesn't return an object
    /// (typical examples are: Add, Update, Delete)
    /// </summary>
    public class BusinessResult<T>
    {
        /// <summary>
        /// Empty initialization
        /// </summary>
        public BusinessResult()
        {
            ResultStatus = ResultStatus.Failure;
            ResultCode = String.Empty;
            Message = String.Empty;
            Errors = new List<BusinessResultError>();
        }

        /// <summary>
        /// Hydrated initialization
        /// </summary>
        /// <param name="resultStatus"></param>
        public BusinessResult(ResultStatus resultStatus):this()
        {
            ResultStatus = resultStatus;
        }

        /// <summary>
        /// Hydrated initialization
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <param name="item"></param>
        public BusinessResult(ResultStatus resultStatus,T item) : this()
        {
            ResultStatus = resultStatus;
            Item = item;
        }

        /// <summary>
        /// Hydrated initialization
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <param name="message"></param>
        public BusinessResult(ResultStatus resultStatus, string resultCode):this()
        {
            ResultStatus = resultStatus;
            ResultCode = resultCode;
        }

        /// <summary>
        /// Hydrated initialization
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <param name="message"></param>
        public BusinessResult(ResultStatus resultStatus, string resultCode, string message):this()
        {
            ResultStatus = resultStatus;
            ResultCode = resultCode;
            Message = message;
        }

        /// <summary>
        /// Hydrated initialization
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <param name="message"></param>
        public BusinessResult(ResultStatus resultStatus, string resultCode, string message, T item) : this()
        {
            ResultStatus = resultStatus;
            ResultCode = resultCode;
            Message = message;
            Item = item;
        }

        /// <summary>
        /// Enumerated status of method call
        /// </summary>
        public ResultStatus ResultStatus { get; set; }

        /// <summary>
        /// Result code of error or warning
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// Detailed message of error or warning or success.
        /// Note: If Message has value, client side anglar will auto populate message on the top menu.
        /// If you don't want to display any message, set this property as empty string.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  It can be one entity or collection of entity.
        ///  The business associated data.
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        ///  This property collect all DTO models valdiation errors. Corporate with T item. 
        ///  Further, corporate with ModelState validation.
        ///  Corporate with  
        /// </summary>
        public ICollection<BusinessResultError> Errors { get; set; }

        /// <summary>
        ///  Assign input errors collection to Errors property.
        ///  Work for ModelState validation.
        /// </summary>
        /// <param name="errors"></param>
        public void AddErrors(Dictionary<string, string> errors)
        {
            if (errors != null && errors.Count > 0)
            {
                errors.ToList().ForEach(e => this.Errors.Add(new BusinessResultError { Key= e.Key, Message= e.Value}));
            }
        }
    }
    

    /// <summary>
    /// Return type for any business layer function that doesn't return an object
    /// (typical examples are: Add, Update, Delete)
    /// </summary>
    public class BusinessResult: BusinessResult<BusinessItem>
    {
        /// <summary>
        /// Empty initialization
        /// </summary>
        public BusinessResult():base()
        {
            Item = new BusinessItem();
        }

        /// <summary>
        /// Hydrated initialization
        /// </summary>
        /// <param name="resultStatus"></param>
        public BusinessResult(ResultStatus resultStatus):base()
        {
            ResultStatus = resultStatus;
            Item = new BusinessItem();
        }

        /// <summary>
        /// Hydrated initialization
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <param name="message"></param>
        public BusinessResult(ResultStatus resultStatus, string resultCode):base()
        {
            ResultStatus = resultStatus;
            ResultCode = resultCode;
            Item = new BusinessItem();
        }

        /// <summary>
        /// Hydrated initialization
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <param name="message"></param>
        public BusinessResult(ResultStatus resultStatus, string resultCode, string message):base()
        {
            ResultStatus = resultStatus;
            ResultCode = resultCode;
            Message = message;
            Item = new BusinessItem();
        }

    }

    public class BusinessResultError {
        public string Key { get; set; }

        public string Message { get; set; }
    }
}
