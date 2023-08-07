using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        public Object ReturnValue {get;set;}
        public string ErrorMessage { get; set;}
        public Response(Object ReturnValue,string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
            this.ReturnValue = ReturnValue;
        }
    }

}
