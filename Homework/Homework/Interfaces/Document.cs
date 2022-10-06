using Moravia.Homework.Interfaces;

namespace Moravia.Homework.Interfaces
{
    internal class DocumentHelper : IDataDocument
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
