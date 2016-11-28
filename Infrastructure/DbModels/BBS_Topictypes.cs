using System;
using System.Linq;
using System.Text;

namespace Infrastructure.DbModel
{
    public class BBS_Topictypes
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int Typeid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:False 
        /// </summary>
        public int Displayorder {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:False 
        /// </summary>
        public string Name {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:False 
        /// </summary>
        public string Description {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? IsDeleted {get;set;}

    }
}
