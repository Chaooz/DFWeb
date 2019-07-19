using System;
namespace DarkFactorCoreNet.Models
{
    public class EditPageModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public string Command { get; set; }
    }
}
