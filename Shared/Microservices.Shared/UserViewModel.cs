using System.Collections.Generic;

namespace Microservices.Shared.Microservices.Shared
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public List<string> Roles { get; set; }



        public IEnumerable<string> GetUserProps()
        {
            yield return Id;
            yield return UserName;
            yield return Email;
            yield return City;
        }
    }


 
}