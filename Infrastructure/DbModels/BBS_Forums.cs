using System;
using System.Linq;
using System.Text;

namespace Infrastructure.DbModel
{
    public class BBS_Forums
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int Fid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Parentid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public Int16 Layer {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:False 
        /// </summary>
        public string Pathlist {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string Parentidlist {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:False 
        /// </summary>
        public int Subforumcount {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string Name {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Status {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('1') 
        /// Nullable:False 
        /// </summary>
        public Int16 Colcount {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Displayorder {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public Int16 Templateid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Topics {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Curtopics {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Posts {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Todayposts {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Lasttid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:False 
        /// </summary>
        public string Lasttitle {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:False 
        /// </summary>
        public DateTime Lastpost {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:False 
        /// </summary>
        public int Lastposterid {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('') 
        /// Nullable:False 
        /// </summary>
        public string Lastposter {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Allowsmilies {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Allowrss {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Allowhtml {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Allowbbcode {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Allowimgcode {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Allowblog {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Istrade {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:False 
        /// </summary>
        public int Allowpostspecial {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:True 
        /// </summary>
        public int? Allowspecialonly {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Alloweditrules {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Allowthumbnail {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:False 
        /// </summary>
        public int Allowtag {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Recyclebin {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Modnewposts {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:False 
        /// </summary>
        public int Modnewtopics {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Jammer {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Disablewatermark {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public int Inheritedmod {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:('0') 
        /// Nullable:False 
        /// </summary>
        public Int16 Autoclose {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Boolean IsDeleted {get;set;}

    }
}
