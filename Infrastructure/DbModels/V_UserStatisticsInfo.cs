using System;
using System.Linq;
using System.Text;

namespace Infrastructure.DbModel
{
    public class V_UserStatisticsInfo
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int Id {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string UserName {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string Password {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Email {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Avatar {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? CreateTime {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? IsDeleted {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? RoleId {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string NickName {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? TopicsCount {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? RepliesCount {get;set;}

    }
}
