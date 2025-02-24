using Demo.Dtos;
using Demo.Interfaces;
using Demo.Repositories.LocalRepository;
using Demo.Repositories.RemoteRepository;
using MediatR;

namespace Demo.Features.GetAllProduct;

public class GetAllProductCommandHandler : IRequestHandler<GetAllProductCommand , List<ProductDto>>
{
    private readonly IProductRepositoryLocal _localRepository;
    private readonly IProductRepositoryRemote _remoteRepository;

    public GetAllProductCommandHandler(IProductRepositoryLocal localRepository, IProductRepositoryRemote remoteRepository)
    {
        _localRepository = localRepository;
        _remoteRepository = remoteRepository;
    }

    public async Task<List<ProductDto>> Handle(GetAllProductCommand request, CancellationToken cancellationToken)
    {
        Random random = new Random();
        bool randomBool = random.Next(2) == 0;

        if (randomBool)
        {
            var products = await _localRepository.GetAll();
            var productsDto = products.Select(
                product => new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                });

            return productsDto.ToList();
        }
        else
        {
            var products = await _remoteRepository.GetAll();
            var productsDto = products.Select(
                product => new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                });

            return productsDto.ToList();
        }

        ;
    }
}