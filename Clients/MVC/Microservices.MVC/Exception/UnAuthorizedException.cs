namespace Microservices.MVC.Exception
{
    public class UnAuthorizedException : System.Exception
    {

        public UnAuthorizedException():base()
        {
            
        }


        public UnAuthorizedException(string message):base(message)
        {
            
        }


        public UnAuthorizedException(string message, System.Exception innException) : base(message, innException)
        {
            
        }

    }

    
}