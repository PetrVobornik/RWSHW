namespace Moravia.Homework.Interfaces;

public interface IDocumentWorker : IDocumentGetter, IDocumentSetter
{
    Task LoadAndSave();
}
