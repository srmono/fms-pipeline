using FastEndpoints;
using fleetmanagement.repositories;
using fleetmanagement.dtos;

public class GetAllTrucksEndpoint : EndpointWithoutRequest<IEnumerable<TruckResponse>>
{
    private readonly ITruckRepository _repository;

    public GetAllTrucksEndpoint(ITruckRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/api/trucks");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var trucks = await _repository.GetAllTrucksAsync();
        await SendAsync(trucks.Select(t => new TruckResponse
        {
            Id = t.Id,
            Name = t.Name,
            Model = t.Model,
            Year = t.Year
        }));
    }
}

