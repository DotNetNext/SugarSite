using System;
using System.Linq;
using System.Text;

namespace Infrastructure.DbModel
{
    public class QuestionContent
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
        public Byte[] Title {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string Content {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int TypeId {get;set;}

        /// <summary>
        /// Desc:1处理中 2已解决 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int Status {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? Sort {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? UserId {get;set;}

        /// <summary>
        /// Desc:父级ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? ParentId {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? Level {get;set;}

    }
}
