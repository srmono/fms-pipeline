using FastEndpoints;
using fleetmanagement.repositories;

public class DeleteEndpoint: EndpointWithoutRequest<bool> {
    private readonly ITruckRepository _respository;

    public DeleteEndpoint(ITruckRepository repository){
        _respository = repository;
    }

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("/api/trucks/{id:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct){
        var id = Route<int>("id");

        var result = await _respository.DeleteTruckAsync(id);

        if(!result){
            await SendNotFoundAsync();
            return;
        }

        await SendNoContentAsync();
    }
}

