using Demo.Dtos;
using Demo.Models;
using MediatR;

namespace Demo.Features.GetAllProduct;

public class GetAllProductCommand : IRequest<List<ProductDto>>
{
    
}