namespace Microservices.Shared.Messages
{
    public class CourseNameChangedEvent
    {
        public string CourseId { get; set; }
        public string UpdatedCourseName { get; set; }
    }
}