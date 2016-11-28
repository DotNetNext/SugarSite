using System;
using System.Linq;
using System.Text;

namespace Infrastructure.DbModel
{
    public class BBS_Topics
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int Tid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Int16 Fid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public Byte? Iconid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Typeid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Readperm {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public Int16 Price {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:True 
        /// </summary>
        public string Poster {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:False 
        /// </summary>
        public int Posterid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Title {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Attention {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:(getdate()) 
        /// Nullable:True 
        /// </summary>
        public DateTime? Postdatetime {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:(getdate()) 
        /// Nullable:True 
        /// </summary>
        public DateTime? Lastpost {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Lastpostid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:True 
        /// </summary>
        public string Lastposter {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Lastposterid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Views {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Replies {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Displayorder {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:True 
        /// </summary>
        public string Highlight {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public Byte? Digest {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Rate {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Hide {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Attachment {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public Byte? Moderated {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Closed {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Magic {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:True 
        /// </summary>
        public int? Identify {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public Byte? Special {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Boolean IsDeleted {get;set;}

    }
}
