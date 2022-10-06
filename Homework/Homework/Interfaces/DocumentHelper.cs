using Moravia.Homework.Interfaces;

namespace Moravia.Homework.Interfaces
{
    public class DocumentHelper : IDataDocument
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
