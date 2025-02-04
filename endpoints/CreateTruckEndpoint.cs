using FastEndpoints;
using fleetmanagement.entities;
using fleetmanagement.dtos;
using fleetmanagement.repositories;

public class CreateTruckEndpoint: Endpoint<TruckRequest, TruckResponse>
{
    private readonly ITruckRepository _respository;

    public CreateTruckEndpoint(ITruckRepository repository){
        _respository = repository;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/api/trucks");
        AllowAnonymous();
    }

    public override async Task HandleAsync(TruckRequest req, CancellationToken ct){
        
        var newTruck = new Truck(
            req.Name ?? string.Empty, 
            req.Model ?? string.Empty,
            req.Year
        );

        var createdTruck = await _respository.AddTruckAsync(newTruck);

        var response = new TruckResponse {
            Id = createdTruck.Id,
            Name = createdTruck.Name,
            Model = createdTruck.Model,
            Year = createdTruck.Year
        };
        await SendAsync(response);
    }
}