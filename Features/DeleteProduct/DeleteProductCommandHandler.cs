using Demo.Interfaces;
using Demo.Models;
using MediatR;

namespace Demo.Features.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Product>
{
    private readonly IProductRepositoryRemote _repositoryRemote;
    private readonly IProductRepositoryLocal _repositoryLocal;


    public DeleteProductCommandHandler(IProductRepositoryRemote repositoryRemote, IProductRepositoryLocal repositoryLocal)
    {
        _repositoryRemote = repositoryRemote;
        _repositoryLocal = repositoryLocal;
    }

    public async Task<Product> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Random random = new Random();
        bool randomBool = random.Next(2) == 0;

        if (randomBool)
        {
            
            var result = await _repositoryLocal.DeleteAsync(request.Id);
            return result;
        }
        else
        {
            var result = await _repositoryRemote.DeleteAsync(request.Id);
            return result;
        }
    }
}