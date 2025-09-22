using AutoMapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Product, CreateProductDto>();
        CreateMap<Product, UpdateProductDto>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<ProductDto, Product>();
        CreateMap<CreateProductDto, Product>();
    }
}