using System.Threading.Tasks;

namespace KafkaApi
{
    public interface IProdutoProducer
    {
        Task<string> EnviarMensagem(string topico, string mensagem);
    }
}